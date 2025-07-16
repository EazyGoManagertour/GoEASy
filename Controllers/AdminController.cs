using GoEASy.Filters;
using Microsoft.AspNetCore.Mvc;

namespace GoEASy.Controllers
{
    [AdminAuthorize]
    [Route("admin")]
    public class AdminController : Controller
    {
        [HttpGet("")]
        public IActionResult Index()
        {
            return RedirectToAction("Index", "LoginAdmin");
        }
    }
}