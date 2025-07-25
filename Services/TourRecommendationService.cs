using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using GoEASy.Models; // hoặc namespace chứa TourDto

namespace GoEASy.Services
{
    public class TourRecommendationService
    {
        private readonly HttpClient _client;

        public TourRecommendationService(HttpClient client)
        {
            _client = client;
            _client.BaseAddress = new Uri("http://localhost:5001"); // Flask server URL
        }

        public async Task<List<TourDto>> GetSemanticRecommendationsAsync(string query, int topK = 5)
        {
            var payload = new { query, top_k = topK };
            var response = await _client.PostAsJsonAsync("/recommend", payload);
            response.EnsureSuccessStatusCode();

            var tours = await response.Content.ReadFromJsonAsync<List<TourDto>>();
            return tours!;
        }

        public async Task<List<TourDto>> GetPersonalizedRecommendationsAsync(int userId, int currentTourId, int topK = 5)
        {
            var payload = new
            {
                user_id = userId,
                current_tour_id = currentTourId,
                top_k = topK
            };

            var response = await _client.PostAsJsonAsync("/recommend", payload);
            response.EnsureSuccessStatusCode();

            var tours = await response.Content.ReadFromJsonAsync<List<TourDto>>();
            return tours!;
        }
    }
}
