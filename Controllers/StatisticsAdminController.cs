using Microsoft.AspNetCore.Mvc;

namespace GoEASy.Controllers
{
    [Route("admin/statistics")]
    public class StatisticsAdminController : Controller
    {
        [HttpGet("")]
        public IActionResult Index()
        {
            return View("~/Views/admin/statistics/StatisticsAdmin.cshtml");
        }
    }
}
