using System.Diagnostics;
using GoEASy.Models;
using Microsoft.AspNetCore.Mvc;
using GoEASy.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;

namespace GoEASy.Controllers
{
    public class HomeController : Controller
    {
        private readonly GoEASy.Services.DestinationService _destinationService;
        private readonly GoEASy.Services.TourService _tourService;
        private readonly IBlogService _blogService;
        private readonly GoEasyContext _context;
        private readonly IMomoService _momoService;

        public HomeController(
            GoEASy.Services.DestinationService destinationService,
            GoEASy.Services.TourService tourService,
            IBlogService blogService,
            GoEasyContext context,
            IMomoService momoService)
        {
            _destinationService = destinationService;
            _tourService = tourService;
            _blogService = blogService;
            _context = context;
            _momoService = momoService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var Alldestinations = await _destinationService.GetAllDestinationsAsync();
                var destinations = Alldestinations.OrderByDescending(d => d.CreatedAt).Take(6).ToList();
                ViewBag.Destinations = destinations;

                var allTours = await _tourService.GetAllToursAsync();
                var tours = allTours.OrderByDescending(t => t.StartDate).Take(8).ToList();
                ViewBag.Tours = tours;

                var Allblogs = await _blogService.GetAllAsync();
                ViewBag.Blogs = Allblogs.OrderByDescending(b => b.CreatedAt).Take(3).Where(b => b.IsApproved == 1).ToList();

                return View("~/Views/client/Home.cshtml", destinations);
            }
            catch (Exception ex)
            {
                // Log error và trả về view với null
                Console.WriteLine($"Error in HomeController: {ex.Message}");
                return View("~/Views/client/Home.cshtml", null);
            }
        }

        [HttpGet]
        public async Task<IActionResult> PaymentCallBack()
        {
            // Lấy thông tin từ query string
            var response = _momoService.PaymentExecuteAsync(Request.Query);
            // Tách bookingID từ orderId
            var orderId = response.OrderId;
            int bookingID = 0;
            if (!string.IsNullOrEmpty(orderId) && orderId.StartsWith("booking_") && orderId.Split('_').Length >= 3)
            {
                var bookingIdStr = orderId.Split('_')[1];
                int.TryParse(bookingIdStr, out bookingID);
            }
            if (bookingID > 0)
            {
                var booking = await _context.Bookings.FirstOrDefaultAsync(b => b.BookingID == bookingID);
                if (booking != null)
                {
                    if (Request.Query["errorCode"] == "0") // Thành công
                    {
                        booking.PaymentStatus = "Paid";
                        await _context.SaveChangesAsync();
                        // Thêm notification cho user
                        if (booking.UserID != null)
                        {
                            var tour = await _context.Tours.FirstOrDefaultAsync(t => t.TourID == booking.TourID);
                            string tourName = tour?.TourName ?? "[Tour]";
                            string startDate = tour?.StartDate?.ToString("dd/MM/yyyy") ?? "?";
                            string title, message;
                            if (booking.Status == false)
                            {
                                title = "Thanh toán thành công & chờ duyệt";
                                message = $"Bạn đã thanh toán thành công tour '{tourName}' khởi hành ngày {startDate}. Booking của bạn đang chờ duyệt. Mã booking: #{booking.BookingID}";
                            }
                            else
                            {
                                title = "Thanh toán thành công";
                                message = $"Bạn đã thanh toán thành công tour '{tourName}' khởi hành ngày {startDate}. Mã booking: #{booking.BookingID}";
                            }
                            var notification = new Notification
                            {
                                UserId = booking.UserID.Value,
                                Title = title,
                                Message = message,
                                Type = "PaymentSuccess",
                                RelatedId = booking.BookingID,
                                CreatedAt = DateTime.Now,
                                IsRead = false
                            };
                            _context.Notifications.Add(notification);
                            await _context.SaveChangesAsync();
                        }
                        TempData["Success"] = "Thanh toán thành công!";
                    }
                    else
                    {
                        booking.PaymentStatus = "Failed";
                        await _context.SaveChangesAsync();
                        TempData["Error"] = "Thanh toán thất bại hoặc bị hủy.";
                    }
                }
            }
            else
            {
                TempData["Error"] = "Không tìm thấy booking!";
            }
            return RedirectToAction("Index", "Home");
        }
    }
}
