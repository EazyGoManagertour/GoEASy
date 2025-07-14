using GoEASy.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace GoEASy.Controllers
{
    [Route("review")]
    public class ReviewController : Controller
    {
        private readonly GoEasyContext _context;
        public ReviewController(GoEasyContext context)
        {
            _context = context;
        }

        // POST: /review/post
        [HttpPost("post")]
        public async Task<IActionResult> PostReview(int tourId, int rating, string comment)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            int? roleId = HttpContext.Session.GetInt32("RoleId");
            if (userId == null || roleId != 1)
            {
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                    return Json(new { success = false, error = "Bạn cần đăng nhập tài khoản khách để đánh giá." });
                TempData["Error"] = "Bạn cần đăng nhập tài khoản khách để đánh giá.";
                return RedirectToAction("Index", "TourDetail", new { id = tourId });
            }

            // Kiểm tra user đã booking tour này chưa
            bool hasBooking = await _context.Bookings.AnyAsync(b => b.UserId == userId && b.TourId == tourId && b.Status == true && b.PaymentStatus == "Paid");
            if (!hasBooking)
            {
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                    return Json(new { success = false, error = "Bạn chỉ có thể đánh giá khi đã đặt và thanh toán tour này." });
                TempData["Error"] = "Bạn chỉ có thể đánh giá khi đã đặt và thanh toán tour này.";
                return RedirectToAction("Index", "TourDetail", new { id = tourId });
            }

            // BỎ kiểm tra đã review, cho phép review nhiều lần

            var review = new Review
            {
                UserId = userId,
                TourId = tourId,
                Rating = rating,
                Comment = comment,
                CreatedDate = DateTime.Now
            };
            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();

            // Lấy tên user
            var user = await _context.Users.FindAsync(userId);

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Json(new
                {
                    success = true,
                    review = new
                    {
                        fullName = user?.FullName,
                        rating = rating,
                        comment = comment,
                        createdDate = review.CreatedDate?.ToString("dd/MM/yyyy HH:mm")
                    }
                });
            }

            TempData["Success"] = "Cảm ơn bạn đã đánh giá!";
            return RedirectToAction("Index", "TourDetail", new { id = tourId });
        }
    }
} 