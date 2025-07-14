using Microsoft.AspNetCore.Mvc;
using GoEASy.Services;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace GoEASy.Controllers
{
    [Route("Admin/login")]
    public class LoginAdminController : Controller
    {
        private readonly IUserService _userService;
        public LoginAdminController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("")]
        public IActionResult Index()
        {
            return View("~/Views/admin/LoginAdmin.cshtml");
        }

        [HttpPost("")]
        public async Task<IActionResult> Login(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                TempData["Error"] = "Vui lòng nhập đầy đủ tên đăng nhập và mật khẩu.";
                return View("~/Views/admin/LoginAdmin.cshtml");
            }

            var user = await _userService.GetUserByUsernameAsync(username);
            if (user == null || user.Password != password)
            {
                TempData["Error"] = "Tên đăng nhập hoặc mật khẩu không đúng.";
                return View("~/Views/admin/LoginAdmin.cshtml");
            }

            // Lưu thông tin user vào session
            HttpContext.Session.SetInt32("UserId", user.UserId);
            HttpContext.Session.SetInt32("RoleId", user.RoleId ?? 1);
            HttpContext.Session.SetString("FullName", user.FullName);

            // Redirect theo role
            if (user.RoleId == 1)
            {
                return RedirectToAction("Index", "Home");
            }
            else if (user.RoleId == 2)
            {
                return RedirectToAction("Index", "ProviderTour");
            }
            else // fallback: admin hoặc role khác
            {
                return RedirectToAction("Index", "AdminTour");
            }
        }
    }
}
