using System.Diagnostics;
using GoEASy.Models;
using Microsoft.AspNetCore.Mvc;
using GoEASy.Services;

namespace GoEASy.Controllers
{
    public class HomeController : Controller
    {
        private readonly GoEASy.Services.DestinationService _destinationService;
        private readonly GoEASy.Services.TourService _tourService;
        private readonly IBlogService _blogService;

        public HomeController(GoEASy.Services.DestinationService destinationService, GoEASy.Services.TourService tourService, IBlogService blogService)
        {
            _destinationService = destinationService;
            _tourService = tourService;
            _blogService = blogService;
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
    }
}
