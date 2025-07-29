using GoEASy.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.Extensions.Configuration;
using System.Text;
using Newtonsoft.Json;

namespace GoEASy.Controllers
{
    [Route("review")]
    public class ReviewController : Controller
    {
        private readonly GoEasyContext _context;
        private readonly IConfiguration _config;
        public ReviewController(GoEasyContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        // POST: /review/post
        [HttpPost("post")]
        public async Task<IActionResult> PostReview(int tourId, int rating, string comment)
        {
            int? userId = HttpContext.Session.GetInt32("UserID");
            int? roleId = HttpContext.Session.GetInt32("RoleID");
            bool isAjax = Request.Headers["X-Requested-With"] == "XMLHttpRequest";

            if (userId == null || roleId != 1)
            {
                if (isAjax)
                    return Json(new { success = false, error = "Bạn cần đăng nhập tài khoản khách để đánh giá." });
                TempData["Error"] = "Bạn cần đăng nhập tài khoản khách để đánh giá.";
                return RedirectToAction("Index", "TourDetail", new { id = tourId });
            }

            // Kiểm tra user đã booking tour này chưa
            bool hasBooking = await _context.Bookings.AnyAsync(b => b.UserID == userId && b.TourID == tourId && b.Status == true && b.PaymentStatus == "Paid");
            if (!hasBooking)
            {
                if (isAjax)
                    return Json(new { success = false, error = "Bạn chỉ có thể đánh giá khi đã đặt và thanh toán tour này." });
                TempData["Error"] = "Bạn chỉ có thể đánh giá khi đã đặt và thanh toán tour này.";
                return RedirectToAction("Index", "TourDetail", new { id = tourId });
            }

            // Kiểm tra rating hợp lệ
            if (rating < 1 || rating > 5)
            {
                if (isAjax)
                    return Json(new { success = false, error = "Bạn phải chọn số sao từ 1 đến 5!" });
                TempData["Error"] = "Bạn phải chọn số sao từ 1 đến 5!";
                return RedirectToAction("Index", "TourDetail", new { id = tourId });
            }

            // --- KIỂM DUYỆT AI ---
            bool isNegative = await IsCommentNegative(comment);
            if (isNegative)
            {
                if (isAjax)
                    return Json(new { success = false, error = "Không được bình luận quá tiêu cực!" });
                TempData["Error"] = "Không được bình luận quá tiêu cực!";
                return RedirectToAction("Index", "TourDetail", new { id = tourId });
            }
            // --- END KIỂM DUYỆT ---

            var review = new Review
            {
                TourID = tourId,
                UserID = userId,
                Rating = rating,
                Comment = comment,
                CreatedDate = DateTime.Now
            };
            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();

            if (isAjax)
            {
                // Trả về thông tin comment mới cho frontend
                var user = await _context.Users.FindAsync(userId);
                return Json(new { 
                    success = true, 
                    comment = new {
                        userName = user?.FullName ?? "Khách",
                        rating = rating,
                        comment = comment,
                        createdDate = DateTime.Now.ToString("dd/MM/yyyy HH:mm")
                    }
                });
            }

            TempData["Success"] = "Cảm ơn bạn đã đánh giá!";
            return RedirectToAction("Index", "TourDetail", new { id = tourId });
        }

        // --- HÀM KIỂM DUYỆT AI ---
        private async Task<bool> IsCommentNegative(string comment)
        {
            var apiKey = _config["OpenAI:ApiKey"];
            var endpoint = _config["OpenAI:Endpoint"] ?? "https://api.openai.com/v1/chat/completions";
            
            // Log debug
            System.IO.File.AppendAllText("ai_moderation_log.txt", $"\n[{DateTime.Now}] DEBUG: API_KEY_LENGTH={apiKey?.Length}, ENDPOINT={endpoint}\n");
            
            if (string.IsNullOrEmpty(apiKey)) {
                System.IO.File.AppendAllText("ai_moderation_log.txt", $"[{DateTime.Now}] ERROR: API_KEY_IS_NULL_OR_EMPTY\n");
                return false;
            }
            
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
                var requestBody = new
                {
                    model = "gpt-3.5-turbo",
                    messages = new[] {
                        new { role = "system", content = "Bạn là một AI kiểm duyệt bình luận. Nếu bình luận sau đây có bất kỳ từ ngữ thô tục, xúc phạm, chửi bới, tiêu cực, tiếng lóng, tiếng Việt, tiếng Anh, hoặc bất kỳ ý xúc phạm nào, hãy trả lời 'NEGATIVE'. Nếu bình luận bình thường hoặc góp ý lịch sự, hãy trả lời 'OK'. Chỉ trả về đúng 1 từ: NEGATIVE hoặc OK." },
                        new { role = "user", content = comment }
                    },
                    max_tokens = 1,
                    temperature = 0
                };
                var json = JsonConvert.SerializeObject(requestBody);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                
                try {
                    var response = await client.PostAsync(endpoint, content);
                    var responseString = await response.Content.ReadAsStringAsync();
                    
                    // Log response
                    System.IO.File.AppendAllText("ai_moderation_log.txt", $"[{DateTime.Now}] RESPONSE_STATUS={response.StatusCode}, RESPONSE_CONTENT={responseString}\n");
                    
                    if (!response.IsSuccessStatusCode) {
                        System.IO.File.AppendAllText("ai_moderation_log.txt", $"[{DateTime.Now}] ERROR: HTTP_{response.StatusCode}\n");
                        return false;
                    }
                    
                    dynamic obj = JsonConvert.DeserializeObject(responseString);
                    string aiResult = obj.choices[0].message.content.ToString().Trim().ToUpper();
                    
                    // Log kết quả cuối cùng
                    System.IO.File.AppendAllText("ai_moderation_log.txt", $"[{DateTime.Now}] COMMENT: {comment}\nAI_RESULT: {aiResult}\nFINAL_DECISION: {(aiResult.ToUpper().Contains("NEG") ? "BLOCK" : "ALLOW")}\n");
                    
                    return aiResult.ToUpper().Contains("NEG");
                }
                catch (Exception ex) {
                    System.IO.File.AppendAllText("ai_moderation_log.txt", $"[{DateTime.Now}] EXCEPTION: {ex.Message}\n");
                    return false;
                }
            }
        }
    }
} 