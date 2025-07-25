using GoEASy.Models;
using GoEASy.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System.Linq;

namespace GoEASy.Controllers
{
    [Route("provider/booking")]
    public class BookingProviderController : Controller
    {
        private readonly BookingService _bookingService;
        public BookingProviderController(BookingService bookingService)
        {
            _bookingService = bookingService;
        }

        // GET: provider/booking
        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            int? providerId = HttpContext.Session.GetInt32("UserID");
            if (providerId == null)
                return RedirectToAction("Index", "LoginAdmin");

            var bookings = await _bookingService.GetBookingsByProviderAsync(providerId.Value);
            return View("~/Views/provider/BookingProvider.cshtml", bookings);
        }

        [HttpPost("update")]
        public async Task<IActionResult> Update(int BookingId, string Status)
        {
            int? providerId = HttpContext.Session.GetInt32("UserID");
            if (providerId == null)
                return RedirectToAction("Index", "LoginAdmin");

            // Lấy booking cần update
            var bookings = await _bookingService.GetBookingsByProviderAsync(providerId.Value);
            var booking = bookings.FirstOrDefault(b => b.BookingID == BookingId);
            if (booking == null)
            {
                TempData["Error"] = "Không tìm thấy booking!";
                return RedirectToAction("Index");
            }

            // Cập nhật trạng thái
            booking.Status = Status == "true";
            await _bookingService.UpdateBookingAsync(booking);

            TempData["Success"] = "Cập nhật trạng thái booking thành công!";
            return RedirectToAction("Index");
        }
    }
} 