using GoEASy.Filters;
using GoEASy.Models;
using GoEASy.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoEASy.Controllers
{
   
    [Route("booking")]
    public class BookingController : Controller
    {
        private readonly GoEasyContext _context;
        private readonly IMomoService _momoService;
        public BookingController(GoEasyContext context, IMomoService momoService)
        {
            _context = context;
            _momoService = momoService;
        }

        // POST: /booking/create
        [HttpPost("create")]
        public async Task<IActionResult> Create(int tourID, int adultGuests, int childGuests, decimal totalPrice, bool isCustom = false)
        {
            int? userID = HttpContext.Session.GetInt32("UserID");
            if (userID == null)
            {
                TempData["Error"] = "Bạn cần đăng nhập để đặt tour.";
                return RedirectToAction("Index", "Login");
            }

            // Tạo booking mới
            var booking = new Booking
            {
                UserID = userID,
                TourID = tourID,
                AdultGuests = adultGuests,
                ChildGuests = childGuests,
                TotalPrice = totalPrice,
                BookingDate = DateTime.Now,
                Status = isCustom ? false : true, // custom: đang duyệt, thường: hoạt động
                PaymentStatus = "Pending",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };
            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();

            // Tạo request MoMo
            var ticks = DateTime.UtcNow.Ticks;
            var orderID = $"booking_{booking.BookingID}_{ticks}";
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserID == userID);
            var orderInfo = new OrderInfoModel
            {
                FullName = user?.FullName ?? "Khách hàng",
                OrderInfo = $"Thanh toán booking #{booking.BookingID}",
                Amount = totalPrice,
                OrderId = orderID // Đảm bảo duy nhất
            };
            var momoResponse = await _momoService.CreatePaymentAsync(orderInfo);
            if (momoResponse != null && !string.IsNullOrEmpty(momoResponse.PayUrl))
            {
                return Redirect(momoResponse.PayUrl);
            }
            else
            {
                TempData["Error"] = "Không thể tạo thanh toán MoMo. Vui lòng thử lại.";
                return RedirectToAction("Index", "Home");
            }
        }

        // GET: /booking/history
        [HttpGet("history")]
        public async Task<IActionResult> BookingHistory()
        {
            int? userID = HttpContext.Session.GetInt32("UserID");
            if (userID == null)
            {
                TempData["Error"] = "Bạn cần đăng nhập để xem lịch sử đặt tour.";
                return RedirectToAction("Index", "Login");
            }
            var bookings = await _context.Bookings
                .Include(b => b.Tour)
                .Include(b => b.Discount)
                .Where(b => b.UserID == userID)
                .OrderByDescending(b => b.BookingDate)
                .ToListAsync();
            return View("~/Views/client/BookingHistory.cshtml", bookings);
        }

        // GET: /booking/check-discount
        [HttpGet("check-discount")]
        public async Task<IActionResult> CheckDiscount(string code, decimal total)
        {
            if (string.IsNullOrWhiteSpace(code))
                return Json(new { success = false, message = "Vui lòng nhập mã giảm giá." });

            var now = DateTime.Now;
            var discount = await _context.Discounts.FirstOrDefaultAsync(d => d.Code == code && (d.Status ?? true));
            if (discount == null)
                return Json(new { success = false, message = "Mã giảm giá không tồn tại hoặc đã bị khóa." });
            if (discount.StartDate.HasValue && discount.StartDate > now)
                return Json(new { success = false, message = "Mã giảm giá chưa bắt đầu hiệu lực." });
            if (discount.EndDate.HasValue && discount.EndDate < now)
                return Json(new { success = false, message = "Mã giảm giá đã hết hạn." });
            if (discount.MinTotalPrice.HasValue && total < discount.MinTotalPrice.Value)
                return Json(new { success = false, message = $"Đơn tối thiểu {discount.MinTotalPrice.Value:N0} đ mới dùng được mã này." });
            if (discount.Percentage == null || discount.Percentage <= 0)
                return Json(new { success = false, message = "Mã giảm giá không hợp lệ." });

            decimal discountAmount = total * (discount.Percentage.Value / 100m);
            if (discount.MaxAmount.HasValue && discountAmount > discount.MaxAmount.Value)
                discountAmount = discount.MaxAmount.Value;
            if (discountAmount <= 0)
                return Json(new { success = false, message = "Mã giảm giá không áp dụng cho đơn này." });

            return Json(new
            {
                success = true,
                discountId = discount.DiscountID,
                discountAmount = discountAmount,
                description = discount.Description,
                percentage = discount.Percentage,
                maxAmount = discount.MaxAmount,
                minTotalPrice = discount.MinTotalPrice
            });
        }

        [HttpGet("momo-return")]
        public async Task<IActionResult> MomoReturn(string orderId, string resultCode, string message)
        {
            // orderId: mã booking bạn đã gửi cho MoMo (BookingID)
            // resultCode: mã kết quả thanh toán (0 = thành công)
            // message: thông báo từ MoMo

            // Tách BookingID từ orderId nếu cần
            int bookingID = 0;
            if (!string.IsNullOrEmpty(orderId))
            {
                var parts = orderId.Split('_');
                if (parts.Length >= 3 && int.TryParse(parts[1], out int parsedId))
                {
                    bookingID = parsedId;
                }
            }
            if (bookingID == 0)
            {
                TempData["Error"] = "Không xác định được booking.";
                return RedirectToAction("Index", "Home");
            }

            var booking = await _context.Bookings.FirstOrDefaultAsync(b => b.BookingID == bookingID);
            if (booking == null)
            {
                TempData["Error"] = "Không tìm thấy booking.";
                return RedirectToAction("Index", "Home");
            }

            if (resultCode == "0")
            {
                // Thành công
                booking.PaymentStatus = "Paid";
                booking.UpdatedAt = DateTime.Now;
                await _context.SaveChangesAsync();

                TempData["Success"] = "Thanh toán MoMo thành công! Cảm ơn bạn đã đặt tour.";
            }
            else
            {
                // Thất bại hoặc bị hủy
                booking.PaymentStatus = "Failed";
                booking.UpdatedAt = DateTime.Now;
                await _context.SaveChangesAsync();

                TempData["Error"] = "Thanh toán MoMo thất bại hoặc bị hủy.";
            }

            return RedirectToAction("Index", "Home");
        }
    }
} 