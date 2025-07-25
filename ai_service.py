import os
from dotenv import load_dotenv 


load_dotenv()
print("🔍 ENV Path:", os.getcwd())  # kiểm tra thư mục hiện tại
print("🔑 KEY:", os.getenv("OPENAI_API_KEY"))

import pyodbc
from flask import Flask, request, jsonify
from flask_cors import CORS
import openai
import faiss
import numpy as np
import traceback
 # Tự động load từ .env nếu có

# ✅ Lấy key từ biến môi trường
openai.api_key = os.getenv("OPENAI_API_KEY")

app = Flask(__name__)
CORS(app)

# Kết nối SQL lấy danh sách tour
def get_sql_connection():
    return pyodbc.connect(
        'DRIVER={ODBC Driver 17 for SQL Server};'
        'SERVER=localhost,1433;'
        'DATABASE=GoEasy;'
        'UID=sa;'
        'PWD=123456'
    )



def get_tour_image(tour_id):
    conn = get_sql_connection()
    cursor = conn.cursor()
    cursor.execute(
        "SELECT TOP 1 ImageURL FROM TourImages WHERE TourID = ? ORDER BY IsCover DESC, UploadedAt DESC", 
        tour_id
    )
    row = cursor.fetchone()
    conn.close()
    if row and row.ImageURL:
        return row.ImageURL
    else:
        return "/assets/images/no-image.jpg"

def load_tours():
    conn = get_sql_connection()
    cursor = conn.cursor()
    cursor.execute("""
    SELECT t.TourID, t.TourName, t.Description, t.AdultPrice, d.DestinationName
    FROM Tours t
    LEFT JOIN Destinations d ON t.DestinationID = d.DestinationID
    WHERE t.Status = 1
    """)
    tours = []
    for row in cursor.fetchall():
        image_url = get_tour_image(row.TourID)
        tours.append({
            "id": row.TourID,
            "title": row.TourName,
            "desc": row.Description,
            "price": float(row.AdultPrice),
            "destination": row.DestinationName,
            "image": image_url
        })
    conn.close()
    return tours

# Lấy danh sách các tour user đã xem gần đây
def get_user_recent_tours(user_id, top_n=5):
    conn = get_sql_connection()
    cursor = conn.cursor()
    cursor.execute("SELECT TOP (?) TourId FROM TourViewLogs WHERE UserId = ? ORDER BY ViewTime DESC", top_n, user_id)
    tour_ids = [row.TourId for row in cursor.fetchall()]
    conn.close()
    return tour_ids

# Lấy embedding trung bình lịch sử tour đã xem
def get_history_embedding(tour_ids, all_tours):
    emb_list = [tour["embedding"] for tour in all_tours if tour["id"] in tour_ids and "embedding" in tour]
    if emb_list:
        return np.mean(np.stack(emb_list), axis=0)
    return None




def get_embedding(text):
    client = openai.OpenAI(api_key=os.getenv("OPENAI_API_KEY"))
    response = client.embeddings.create(
        input=text,
        model="text-embedding-3-small"
    )
    return np.array(response.data[0].embedding, dtype='float32')

# --- KHỞI TẠO DATA ---
print("Loading tours and embeddings...")
tours = load_tours()
for tour in tours:
    tour["embedding"] = get_embedding(tour["desc"])

d = len(tours[0]["embedding"])
index = faiss.IndexFlatL2(d)
index.add(np.stack([tour["embedding"] for tour in tours]))

print("Ready for recommendation!")

# --- ROUTE AI GỢI Ý ---
@app.route('/recommend', methods=['POST'])
def recommend():
    try:
        user_input = request.json['query']
        user_id = int(request.json.get('user_id', 0) or 0)
        top_k = int(request.json.get('top_k', 5))
        current_tour_id = int(request.json.get('current_tour_id', 0) or 0)
        query_vec = get_embedding(user_input)

        current_tour_emb = None
        for tour in tours:
            if tour["id"] == current_tour_id:
                current_tour_emb = tour.get("embedding")
                break

        hist_emb = None
        if user_id:
            recent_tour_ids = get_user_recent_tours(user_id)
            hist_emb = get_history_embedding(recent_tour_ids, tours)

        if hist_emb is not None and current_tour_emb is not None:
            combined_vec = (0.4 * hist_emb + 0.6 * current_tour_emb + query_vec) / 2.0
        elif current_tour_emb is not None:
            combined_vec = current_tour_emb
        elif hist_emb is not None:
            combined_vec = hist_emb
        else:
            combined_vec = query_vec

        D, I = index.search(combined_vec.reshape(1, -1), top_k)

        results = []
        added_ids = set()

        for i in I[0]:
            if i >= len(tours):
                continue  # tránh lỗi index

            tour = tours[i]
            if tour["id"] in added_ids:
                continue  # ✅ Bỏ tour nếu đã có ID này
            added_ids.add(tour["id"])

            clean_tour = tour.copy()
            if "embedding" in clean_tour:
                del clean_tour["embedding"]
            results.append(clean_tour)

        return jsonify(results)

    except Exception as e:
        import traceback
        print("🔥 Lỗi AI Recommendation:", e)
        traceback.print_exc()
        return jsonify({"error": str(e)}), 500

if __name__ == "__main__":
    app.run(host="0.0.0.0", port=5001)
