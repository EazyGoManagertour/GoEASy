using GoEASy.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
            _context.Bookings.Update(booking);
            await _context.SaveChangesAsync();
        }
    }
} 