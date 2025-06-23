using Microsoft.AspNetCore.Mvc;

namespace GoEASy.Controllers
{
    [Route("Admin/[controller]")]
    public class StatisticsAdminController : Controller
    {
        [HttpGet("")]
        public IActionResult Index()
        {
            return View("~/Views/admin/statistics/StatisticsAdmin.cshtml");
        }
    }
}
