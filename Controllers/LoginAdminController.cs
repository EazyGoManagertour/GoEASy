using Microsoft.AspNetCore.Mvc;

namespace GoEASy.Controllers
{
    [Route("Admin/[controller]")]
    public class LoginAdminController : Controller
    {
        [HttpGet("")]
        public IActionResult Index()
        {
            return View("~/Views/admin/LoginAdmin.cshtml");
        }
    }
}
