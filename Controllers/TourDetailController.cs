using Microsoft.AspNetCore.Mvc;
using GoEASy.Services;
using System.Threading.Tasks;
using GoEASy.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace GoEASy.Controllers
{
    [Route("tour-details")]
    public class TourDetailController : Controller
    {
        private readonly TourService _tourService;
        private readonly GoEasyContext _context;
        
        public TourDetailController(TourService tourService, GoEasyContext context)
        {
            _tourService = tourService;
            _context = context;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Index(int id)
        {
            var tour = await _tourService.GetTourByIdAsync(id);
            if (tour == null)
                return NotFound();
            ViewBag.IncludedList = _tourService.GetIncludedList(tour);
            ViewBag.ExcludedList = _tourService.GetExcludedList(tour);
            ViewBag.ActivitiesList = _tourService.GetActivitiesList(tour);
            ViewBag.Itineraries = await _tourService.GetTourItinerariesAsync(id);
            ViewBag.FAQs = await _tourService.GetTourFAQsAsync(id);

            // Lấy feedback (review) cho tour này, order mới nhất trước
            var feedbacks = await _context.Reviews
                .Include(r => r.User)
                .Where(r => r.TourId == id && !string.IsNullOrEmpty(r.Comment))
                .OrderByDescending(r => r.CreatedDate)
                .ToListAsync();
            ViewBag.ClientFeedbacks = feedbacks;

            // Kiểm tra user đã booking và thanh toán tour này chưa
            int? userId = HttpContext.Session.GetInt32("UserId");
            bool canComment = false;
            if (userId != null)
            {
                canComment = await _context.Bookings.AnyAsync(b => b.UserId == userId && b.TourId == id && b.Status == true && b.PaymentStatus == "Paid");
            }
            ViewBag.CanComment = canComment;

            return View("~/Views/client/tour-details.cshtml", tour);
        }
    }
} 