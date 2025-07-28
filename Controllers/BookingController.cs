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
using System.Text.Json;

namespace GoEASy.Controllers
{
    public class CancelBookingRequest
    {
        public int BookingId { get; set; }
    }

    [Route("booking")]
    public class BookingController : Controller
    {
        private readonly GoEasyContext _context;
        private readonly IMomoService _momoService;
        private readonly IPolicyService _policyService;
        
        public BookingController(GoEasyContext context, IMomoService momoService, IPolicyService policyService)
        {
            _context = context;
            _momoService = momoService;
            _policyService = policyService;
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

            // Kiểm tra chính sách: phải có ít nhất 1 người lớn
            if (adultGuests <= 0)
            {
                TempData["Error"] = "Phải có ít nhất 1 người lớn trong tour.";
                return RedirectToAction("Index", "TourDetail", new { id = tourID });
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
                PaymentStatus = "Pending", // Đang chờ thanh toán MoMo
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
        public async Task<IActionResult> BookingHistory(int page = 1)
        {
            int? userID = HttpContext.Session.GetInt32("UserID");
            if (userID == null)
            {
                TempData["Error"] = "Bạn cần đăng nhập để xem lịch sử đặt tour.";
                return RedirectToAction("Index", "Login");
            }

            int pageSize = 8;
            var query = _context.Bookings
                .Include(b => b.Tour)
                .Include(b => b.Discount)
                .Where(b => b.UserID == userID)
                .OrderByDescending(b => b.BookingDate);

            var totalBookings = await query.CountAsync();
            var totalPages = (int)Math.Ceiling((double)totalBookings / pageSize);

            var bookings = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.TotalBookings = totalBookings;
            ViewBag.PageSize = pageSize;

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

        // POST: /booking/cancel
        [HttpPost("cancel")]
        public async Task<IActionResult> CancelBooking([FromBody] CancelBookingRequest request)
        {
            int? userID = HttpContext.Session.GetInt32("UserID");
            if (userID == null)
            {
                return Json(new { success = false, message = "Bạn cần đăng nhập để hủy tour." });
            }

            var booking = await _context.Bookings
                .Include(b => b.Tour)
                .FirstOrDefaultAsync(b => b.BookingID == request.BookingId && b.UserID == userID);

            if (booking == null)
            {
                return Json(new { success = false, message = "Không tìm thấy booking." });
            }

            // Kiểm tra xem booking có thể hủy không
            if (booking.PaymentStatus == "Cancelled" || booking.PaymentStatus == "Refunded")
            {
                return Json(new { success = false, message = "Tour này đã được hủy hoặc hoàn tiền." });
            }
            
            if (booking.PaymentStatus == "Processing")
            {
                return Json(new { success = false, message = "Tour đang trong quá trình thanh toán. Vui lòng đợi hoàn tất thanh toán." });
            }
            
            if (booking.PaymentStatus != "Paid")
            {
                return Json(new { success = false, message = "Chỉ có thể hủy tour đã thanh toán." });
            }

            // Kiểm tra ngày khởi hành
            if (booking.Tour?.StartDate == null)
            {
                return Json(new { success = false, message = "Không tìm thấy ngày khởi hành." });
            }

            var daysUntilStart = (booking.Tour.StartDate.Value - DateTime.Now).Days;

            // Lấy policy hủy tour của tour này
            var cancellationPolicy = await _policyService.GetCancellationPolicyAsync(booking.TourID.Value);
            if (cancellationPolicy == null)
            {
                return Json(new { success = false, message = "Không tìm thấy chính sách hủy tour cho tour này." });
            }

            // Kiểm tra xem có thể hủy hoàn tiền không
            if (daysUntilStart < cancellationPolicy.RefundDays)
            {
                return Json(new { 
                    success = false, 
                    message = $"Không thể hủy tour hoàn tiền. Theo chính sách tour này, cần hủy trước {cancellationPolicy.RefundDays} ngày khởi hành để được hoàn tiền. Hiện tại còn {daysUntilStart} ngày." 
                });
            }

            // Tính số tiền hoàn
            var refundAmount = await _policyService.CalculateCancellationRefundAsync(booking.BookingID);

            // Cập nhật trạng thái booking
            booking.PaymentStatus = "Refunded"; // Đã hoàn tiền
            booking.Status = false; // Đánh dấu booking đã hủy
            booking.UpdatedAt = DateTime.Now;
            await _context.SaveChangesAsync();

            // Tạo thông báo cho user
            var notification = new Notification
            {
                UserId = userID.Value,
                Title = "Tour đã được hủy hoàn tiền",
                Message = $"Tour '{booking.Tour.TourName}' đã được hủy thành công. Số tiền hoàn: {refundAmount:N0} đ (theo chính sách hủy tour)",
                IsRead = false,
                CreatedAt = DateTime.Now,
                Type = "booking_cancelled",
                RelatedId = booking.BookingID
            };
            _context.Notifications.Add(notification);
            await _context.SaveChangesAsync();

            return Json(new { 
                success = true, 
                message = $"Hủy tour thành công! Số tiền hoàn: {refundAmount:N0} đ (theo chính sách hủy tour của tour này)",
                refundAmount = refundAmount
            });
        }

        // POST: /booking/cancel-expired-momo
        [HttpPost("cancel-expired-momo")]
        public async Task<IActionResult> CancelExpiredMomoBookings()
        {
            // Tìm các booking đang Processing và đã quá 15 phút (thời gian MoMo)
            var expiredBookings = await _context.Bookings
                .Where(b => b.PaymentStatus == "Processing" && 
                           b.CreatedAt < DateTime.Now.AddMinutes(-15))
                .ToListAsync();

            foreach (var booking in expiredBookings)
            {
                booking.PaymentStatus = "Cancelled";
                booking.Status = false;
                booking.UpdatedAt = DateTime.Now;
            }

            await _context.SaveChangesAsync();

            return Json(new { 
                success = true, 
                message = $"Đã hủy {expiredBookings.Count} booking hết thời gian thanh toán",
                cancelledCount = expiredBookings.Count
            });
        }
    }
}