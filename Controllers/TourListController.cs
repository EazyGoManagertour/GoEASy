using Microsoft.AspNetCore.Mvc;

namespace GoEASy.Controllers
{
    [Route("/tour-list")]
    public class TourListController : Controller
    {
        public IActionResult Index()
        {
            return View("~/Views/client/Tour-list.cshtml");
        }
    }
}
