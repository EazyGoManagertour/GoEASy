using Microsoft.AspNetCore.Mvc;

namespace GoEASy.Controllers
{
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