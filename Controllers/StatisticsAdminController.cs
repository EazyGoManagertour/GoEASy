using GoEASy.Filters;
using Microsoft.AspNetCore.Mvc;

namespace GoEASy.Controllers
{
    [AdminAuthorize]
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
