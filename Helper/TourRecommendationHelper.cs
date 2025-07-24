using GoEASy.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoEASy.Helpers
{
    public class TourRecommendationHelper
    {
        private readonly GoEasyContext _context;
        public TourRecommendationHelper(GoEasyContext context) => _context = context;

        // Recommend theo lịch sử + tourId vừa xem
        public async Task<List<Tour>> GetSuggestedToursByHistory(int? userId, int currentTourId, int top = 5)
        {
            // 1. Kiểm tra userId hợp lệ
            if (userId == null)
                return new List<Tour>();

            // 2. Lấy ID các tour đã xem (loại bỏ tour hiện tại)
            var historyTourIds = await _context.TourViewLogs
                .Where(l => l.UserID == userId && l.TourID != currentTourId)
                .OrderByDescending(l => l.ViewTime)
                .Select(l => l.TourID.Value) // .Value vì l.TourId là int?
                .Distinct()
                .Take(10)
                .ToListAsync();

            // 3. Lấy tour hiện tại và Destination (phải Include Destination)
            var currentTour = await _context.Tours
                .Include(t => t.Destination)
                .FirstOrDefaultAsync(t => t.TourID == currentTourId);

            if (currentTour == null || currentTour.Destination == null)
                return new List<Tour>(); // Không tìm thấy tour hoặc destination

            // 4. Gợi ý tour: cùng Destination hoặc đã từng xem, loại bỏ tour hiện tại
            var suggested = await _context.Tours
                .Include(t => t.Destination)
                .Where(t => t.TourID != currentTourId &&
                    (historyTourIds.Contains(t.TourID) ||
                     t.DestinationID == currentTour.DestinationID ||
                     (t.Description != null && t.Description.Contains(currentTour.Destination.DestinationName))))
                .Take(top)
                .ToListAsync();

            return suggested;
        }
    }
}
