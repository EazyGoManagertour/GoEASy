using Microsoft.AspNetCore.Mvc;

namespace GoEASy.Controllers
{
    [Route("admin/account-admin")]
    public class AccountAdminController : Controller
    {
        [HttpGet("")]
        public IActionResult Index()
        {
            return View("~/Views/admin/account_admin/AccountAdmin.cshtml");
        }
    }
}
