using GoEASy.Models;
using GoEASy.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace GoEASy.Controllers
{
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
    }
} 