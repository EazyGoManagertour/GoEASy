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

        public HomeController(GoEASy.Services.DestinationService destinationService, GoEASy.Services.TourService tourService)
        {
            _destinationService = destinationService;
            _tourService = tourService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var destinations = await _destinationService.GetAllDestinationsAsync();
                var allTours = await _tourService.GetAllToursAsync();
                var tours = allTours.OrderByDescending(t => t.StartDate).Take(6).ToList();
                ViewBag.Tours = tours;
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
