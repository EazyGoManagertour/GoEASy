using Microsoft.AspNetCore.Mvc;
using GoEASy.Services;
using System.Threading.Tasks;
using GoEASy.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Text.Json;

namespace GoEASy.Controllers
{
    [Route("tour-details")]
    public class TourDetailController : Controller
    {
        private readonly TourService _tourService;
        private readonly GoEasyContext _context;
        private readonly IPolicyService _policyService;

        public TourDetailController(TourService tourService, GoEasyContext context, IPolicyService policyService)
        {
            _tourService = tourService;
            _context = context;
            _policyService = policyService;
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

            // Lấy chính sách cho tour
            var policies = await _policyService.GetTourPoliciesAsync(id);
            ViewBag.TourPolicies = policies;
            ViewBag.ChildPolicyMessage = await _policyService.GetPolicyMessageAsync(id, "ChildPolicy");
            ViewBag.CancellationPolicyMessage = await _policyService.GetPolicyMessageAsync(id, "CancellationPolicy");

            // Lấy policy data cho JavaScript
            var childPolicy = await _policyService.GetPolicyByTypeAsync(id, "ChildPolicy");
            if (childPolicy != null)
            {
                try
                {
                    var policyData = JsonSerializer.Deserialize<ChildPolicyData>(childPolicy.PolicyValue ?? "{}");
                    if (policyData == null || policyData.MaxChildrenPerAdult == 0)
                        policyData = new ChildPolicyData { MinAdultsPerChild = 1, MaxChildrenPerAdult = 3, Message = "Phải có ít nhất 1 người lớn cho mỗi 3 trẻ em" };
                    ViewBag.ChildPolicyData = policyData;
                }
                catch
                {
                    ViewBag.ChildPolicyData = new ChildPolicyData { MinAdultsPerChild = 1, MaxChildrenPerAdult = 3, Message = "Phải có ít nhất 1 người lớn cho mỗi 3 trẻ em" };
                }
            }
            else
            {
                ViewBag.ChildPolicyData = new ChildPolicyData { MinAdultsPerChild = 1, MaxChildrenPerAdult = 3, Message = "Phải có ít nhất 1 người lớn cho mỗi 3 trẻ em" };
            }

            return View("~/Views/client/tour-details.cshtml", tour);
        }

        [HttpGet("{id}/policy")]
        public async Task<IActionResult> GetPolicyData(int id)
        {
            var childPolicy = await _policyService.GetPolicyByTypeAsync(id, "ChildPolicy");
            if (childPolicy == null)
            {
                return Json(new { 
                    MinAdultsPerChild = 1, 
                    MaxChildrenPerAdult = 3, 
                    Message = "Phải có ít nhất 1 người lớn cho mỗi 3 trẻ em" 
                });
            }

            try
            {
                var policyData = JsonSerializer.Deserialize<ChildPolicyData>(childPolicy.PolicyValue ?? "{}");
                return Json(policyData);
            }
            catch
            {
                return Json(new { 
                    MinAdultsPerChild = 1, 
                    MaxChildrenPerAdult = 3, 
                    Message = "Phải có ít nhất 1 người lớn cho mỗi 3 trẻ em" 
                });
            }
        }
    }
}