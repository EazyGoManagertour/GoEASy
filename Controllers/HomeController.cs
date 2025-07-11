using System.Diagnostics;
using GoEASy.Models;
using Microsoft.AspNetCore.Mvc;
using GoEASy.Services;

namespace GoEASy.Controllers
{
    public class HomeController : Controller
    {
        private readonly GoEASy.Services.DestinationService _destinationService;

        public HomeController(GoEASy.Services.DestinationService destinationService)
        {
            _destinationService = destinationService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var destinations = await _destinationService.GetAllDestinationsAsync();
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
