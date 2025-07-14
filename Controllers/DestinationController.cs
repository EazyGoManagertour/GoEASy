using GoEASy.Services;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace GoEASy.Controllers
{
    [Route("destination")]
    public class DestinationController : Controller
    {
        private readonly DestinationService _destinationService;

        public DestinationController(DestinationService destinationService)
        {
            _destinationService = destinationService;
        }

        [HttpGet("")]
        [HttpGet("index")]
        public async Task<IActionResult> Index()
        {
            var destinations = (await _destinationService.GetAllDestinationsAsync())
                .Select(d => new {
                    d.DestinationId,
                    d.DestinationName,
                    ImageUrl = d.DestinationImages.FirstOrDefault(img => img.IsCover == true)?.ImageUrl ?? "/assets/images/destinations/destination1.jpg"
                }).ToList();

            return View("~/Views/client/Destination.cshtml", destinations);
        }
    }
} 