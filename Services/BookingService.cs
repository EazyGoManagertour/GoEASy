using GoEASy.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace GoEASy.Services
{
    public class BookingService
    {
        private readonly GoEasyContext _context;
        public BookingService(GoEasyContext context)
        {
            _context = context;
        }

        // Lấy tất cả booking của các tour mà provider này sở hữu
        public async Task<List<Booking>> GetBookingsByProviderAsync(int providerId)
        {
            // Lấy các tour do provider này tạo
            var tourIds = await _context.Tours
                .Where(t => t.CreatedBy == providerId)
                .Select(t => t.TourId)
                .ToListAsync();

            // Lấy các booking của các tour đó, include User và Tour
            var bookings = await _context.Bookings
                .Include(b => b.User)
                .Include(b => b.Tour)
                .Where(b => b.TourId != null && tourIds.Contains(b.TourId.Value))
                .OrderByDescending(b => b.BookingDate)
                .ToListAsync();

            return bookings;
        }

        public async Task UpdateBookingAsync(Booking booking)
        {
            // Lấy trạng thái cũ trước khi update
            var oldBooking = await _context.Bookings.AsNoTracking().FirstOrDefaultAsync(b => b.BookingId == booking.BookingId);
            bool wasPending = oldBooking?.Status == false;
            bool isNowConfirmed = booking.Status == true;

            _context.Bookings.Update(booking);
            await _context.SaveChangesAsync();

            // Nếu chuyển từ chờ duyệt sang xác nhận, gửi notification cho user
            if (wasPending && isNowConfirmed && booking.UserId != null && booking.TourId != null)
            {
                var tour = await _context.Tours.FirstOrDefaultAsync(t => t.TourId == booking.TourId);
                string tourName = tour?.TourName ?? "[Tour]";
                string startDate = tour?.StartDate?.ToString("dd/MM/yyyy") ?? "?";
                var notification = new Notification
                {
                    UserId = booking.UserId.Value,
                    Title = "Tour đã xác nhận",
                    Message = $"Tour '{tourName}' khởi hành ngày {startDate} đã được xác nhận thành công.",
                    Type = "BookingConfirmed",
                    RelatedId = booking.BookingId,
                    CreatedAt = DateTime.Now,
                    IsRead = false
                };
                _context.Notifications.Add(notification);
                await _context.SaveChangesAsync();
            }
        }
    }
} 