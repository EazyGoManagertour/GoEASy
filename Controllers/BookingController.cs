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
    [AdminAuthorize]
    [Route("booking")]
    public class BookingController : Controller
    {
        private readonly GoEasyContext _context;
        private readonly VnPayLibrary _vnPay;
        private const string Vnp_TmnCode = "ZK3P8DST";
        private const string Vnp_HashSecret = "DB0EM555MQPM0Y6OATBT2768HZJQMCRR";
        private const string Vnp_Url = "https://sandbox.vnpayment.vn/paymentv2/vpcpay.html";
        private const string Vnp_ReturnUrl = "https://localhost:7034/booking/vnpay-return";

        public BookingController(GoEasyContext context)
        {
            _context = context;
            _vnPay = new VnPayLibrary();
        }

        // POST: /booking/create
        [HttpPost("create")]
        public async Task<IActionResult> Create(int tourId, int adultGuests, int childGuests, decimal totalPrice, bool isCustom = false)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                TempData["Error"] = "Bạn cần đăng nhập để đặt tour.";
                return RedirectToAction("Index", "Login");
            }

            // Tạo booking mới
            var booking = new Booking
            {
                UserId = userId,
                TourId = tourId,
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

            // Tạo request VNPAY
            var orderId = booking.BookingId.ToString();
            var amount = ((long)totalPrice * 100).ToString(); // VNPAY yêu cầu x100
            var ipAddress = Utils.GetIpAddress(HttpContext);
            var createDate = DateTime.Now.ToString("yyyyMMddHHmmss");

            _vnPay.AddRequestData("vnp_Version", VnPayLibrary.VERSION);
            _vnPay.AddRequestData("vnp_Command", "pay");
            _vnPay.AddRequestData("vnp_TmnCode", Vnp_TmnCode);
            _vnPay.AddRequestData("vnp_Amount", amount);
            _vnPay.AddRequestData("vnp_CreateDate", createDate);
            _vnPay.AddRequestData("vnp_CurrCode", "VND");
            _vnPay.AddRequestData("vnp_IpAddr", ipAddress);
            _vnPay.AddRequestData("vnp_Locale", "vn");
            _vnPay.AddRequestData("vnp_OrderInfo", $"Thanh toan booking #{orderId}");
            _vnPay.AddRequestData("vnp_OrderType", "other");
            _vnPay.AddRequestData("vnp_ReturnUrl", Vnp_ReturnUrl);
            _vnPay.AddRequestData("vnp_TxnRef", orderId);

            var paymentUrl = _vnPay.CreateRequestUrl(Vnp_Url, Vnp_HashSecret);
            return Redirect(paymentUrl);
        }

        // GET: /booking/vnpay-return
        [HttpGet("vnpay-return")]
        public async Task<IActionResult> VnPayReturn()
        {
            var vnp_ResponseCode = Request.Query["vnp_ResponseCode"].ToString();
            var vnp_TxnRef = Request.Query["vnp_TxnRef"].ToString();
            var vnp_SecureHash = Request.Query["vnp_SecureHash"].ToString();

            var vnPay = new VnPayLibrary();
            foreach (var key in Request.Query.Keys)
            {
                if (key.StartsWith("vnp_"))
                {
                    vnPay.AddResponseData(key, Request.Query[key]);
                }
            }
            bool valid = vnPay.ValidateSignature(vnp_SecureHash, Vnp_HashSecret);
            if (!valid)
            {
                TempData["Error"] = "Sai checksum! Giao dịch không hợp lệ.";
                return RedirectToAction("Index", "Home");
            }

            // Xử lý kết quả giao dịch
            int bookingId = int.Parse(vnp_TxnRef);
            var booking = await _context.Bookings.FirstOrDefaultAsync(b => b.BookingId == bookingId);
            if (booking == null)
            {
                TempData["Error"] = "Không tìm thấy booking!";
                return RedirectToAction("Index", "Home");
            }

            if (vnp_ResponseCode == "00")
            {
                booking.PaymentStatus = "Paid";
                await _context.SaveChangesAsync();
                // Thêm thông báo cho user
                if (booking.UserId != null)
                {
                    // Lấy thông tin tour
                    var tour = await _context.Tours.FirstOrDefaultAsync(t => t.TourId == booking.TourId);
                    string tourName = tour?.TourName ?? "[Tour]";
                    string startDate = tour?.StartDate?.ToString("dd/MM/yyyy") ?? "?";
                    string title, message;
                    if (booking.Status == false) // custom booking, chờ duyệt
                    {
                        title = "Thanh toán thành công & chờ duyệt";
                        message = $"Bạn đã thanh toán thành công tour '{tourName}' khởi hành ngày {startDate}. Booking của bạn đang chờ duyệt. Mã booking: #{booking.BookingId}";
                    }
                    else // booking thường
                    {
                        title = "Thanh toán thành công";
                        message = $"Bạn đã thanh toán thành công tour '{tourName}' khởi hành ngày {startDate}. Mã booking: #{booking.BookingId}";
                    }
                    var notification = new Notification
                    {
                        UserId = booking.UserId.Value,
                        Title = title,
                        Message = message,
                        Type = "PaymentSuccess",
                        RelatedId = booking.BookingId,
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
            return RedirectToAction("Index", "Home");
        }

        // GET: /booking/history
        [HttpGet("history")]
        public async Task<IActionResult> BookingHistory()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                TempData["Error"] = "Bạn cần đăng nhập để xem lịch sử đặt tour.";
                return RedirectToAction("Index", "Login");
            }
            var bookings = await _context.Bookings
                .Include(b => b.Tour)
                .Include(b => b.Discount)
                .Where(b => b.UserId == userId)
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
                discountId = discount.DiscountId,
                discountAmount = discountAmount,
                description = discount.Description,
                percentage = discount.Percentage,
                maxAmount = discount.MaxAmount,
                minTotalPrice = discount.MinTotalPrice
            });
        }

        // GET: /booking/history-data (AJAX endpoint)
        [HttpGet("history-data")]
        public async Task<IActionResult> BookingHistoryData()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return Json(new { error = "Unauthorized" });
            }
            
            var bookings = await _context.Bookings
                .Include(b => b.Tour)
                    .ThenInclude(t => t.Destination)
                .Include(b => b.Discount)
                .Where(b => b.UserId == userId)
                .OrderByDescending(b => b.BookingDate)
                .Select(b => new {
                    b.BookingId,
                    b.BookingDate,
                    b.TotalPrice,
                    b.AdultGuests,
                    b.ChildGuests,
                    b.Status,
                    Tour = new {
                        b.Tour.TourName,
                        b.Tour.StartDate,
                        Destination = new {
                            b.Tour.Destination.DestinationName
                        }
                    }
                })
                .ToListAsync();
            
            return Json(bookings);
        }
    }
} 
