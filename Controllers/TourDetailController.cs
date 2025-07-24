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
        public async Task<IActionResult> Index(int id, int page = 1)
        {
            int? userId = HttpContext.Session.GetInt32("UserID");
            if (userId != null)
            {
                var logService = new TourLogService(_context);
                await logService.LogAsync(id, userId, "view");
            }


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
               .Where(r => r.TourID == id && !string.IsNullOrEmpty(r.Comment))
               .OrderByDescending(r => r.CreatedDate)
               .ToListAsync();
            int pageSize = 5;
            int totalFeedbacks = feedbacks.Count;
            int totalPages = (int)Math.Ceiling((double)totalFeedbacks / pageSize);
            var pagedFeedbacks = feedbacks.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            ViewBag.ClientFeedbacks = pagedFeedbacks;
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;

            // Kiểm tra user đã booking và thanh toán tour này chưa
            bool canComment = false;
            if (userId != null)
            {
                canComment = await _context.Bookings.AnyAsync(b => b.UserID == userId && b.TourID == id && b.Status == true && b.PaymentStatus == "Paid");
            }
            ViewBag.CanComment = canComment;

            return View("~/Views/client/tour-details.cshtml", tour);
        }
    }
}

