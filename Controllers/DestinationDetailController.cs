using Microsoft.AspNetCore.Mvc;
using GoEASy.Services;
using GoEASy.Models;
using System.Linq;
using System.Threading.Tasks;

namespace GoEASy.Controllers
{
    [Route("destination/details")]
    public class DestinationDetailController : Controller
    {
        private readonly DestinationService _destinationService;
        private readonly TourService _tourService;
        public DestinationDetailController(DestinationService destinationService, TourService tourService)
        {
            _destinationService = destinationService;
            _tourService = tourService;
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> Index(int id, int? categoryId = null)
        {
            var destination = await _destinationService.GetDestinationByIdAsync(id);
            if (destination == null) return NotFound();

            // Lấy tất cả tour (không filter theo destination)
            var allTours = await _tourService.GetAllToursAsync();
            var tours = allTours.AsQueryable();
            if (categoryId.HasValue)
                tours = tours.Where(t => t.CategoryId == categoryId.Value);

            // Lấy tất cả category
            var categories = await _tourService.GetAllCategoriesAsync();

            ViewBag.Categories = categories;
            ViewBag.SelectedCategoryId = categoryId;
            ViewBag.Tours = tours.ToList();

            // Popular destinations vẫn giữ nguyên logic cũ
            var allDestinations = (await _destinationService.GetAllDestinationsAsync()).Where(d => d.DestinationId != id).Take(6).ToList();
            ViewBag.PopularDestinations = allDestinations;
            return View("~/Views/client/destination-details.cshtml", destination);
        }
    }
} 