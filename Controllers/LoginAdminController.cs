using GoEASy.Models;
using GoEASy.Services;
using Microsoft.AspNetCore.Mvc;

namespace GoEASy.Controllers
{
    [Route("admin/login")]
    public class LoginAdminController : Controller
    {
        private readonly IAdminService _adminService;

        public LoginAdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        // GET: /admin/login
        [HttpGet("")]
        public IActionResult Index()
        {
            // Nếu đã đăng nhập → chuyển hướng đến trang quản lý tài khoản
            if (HttpContext.Session.GetInt32("AdminID") != null)
            {
                return RedirectToAction("account-admin", "admin"); // hoặc Dashboard, tùy bạn
            }

            // Chưa đăng nhập → hiển thị form đăng nhập
            return View("~/Views/Admin/LoginAdmin.cshtml");
        }


        // POST: /admin/login
        [HttpPost("")]
        public async Task<IActionResult> Login(string username, string password)
        {
            try
            {
                if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                {
                    TempData["Error"] = "Vui lòng nhập tên đăng nhập và mật khẩu.";
                    return RedirectToAction("Index");
                }

                // Lấy tất cả admin và kiểm tra
                var allAdmins = await _adminService.GetAllAdminsAsync();
                var admin = allAdmins.FirstOrDefault(a => a.Username == username && BCrypt.Net.BCrypt.Verify(password, a.Password) && a.Status == true);

                if (admin != null)
                {
                    // Tạo session cho admin
                    HttpContext.Session.SetInt32("AdminID", admin.AdminId);
                    HttpContext.Session.SetString("AdminUsername", admin.Username);
                    HttpContext.Session.SetString("AdminFullName", admin.FullName);
                    HttpContext.Session.SetString("AdminRole", admin.Role);

                    TempData["Success"] = "Đăng nhập quản trị thành công!";
                    return RedirectToAction("category", "admin"); // Có thể điều chỉnh controller/action sau đăng nhập
                }
                else
                {
                    TempData["Error"] = "Tên đăng nhập hoặc mật khẩu không đúng, hoặc tài khoản bị vô hiệu hóa.";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Lỗi hệ thống: " + ex.Message;
                return RedirectToAction("Index");
            }
        }

        // POST: /admin/logout
        [HttpPost("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("AdminID");
            HttpContext.Session.Remove("AdminUsername");
            HttpContext.Session.Remove("AdminFullName");
            HttpContext.Session.Remove("AdminRole");

            TempData["Success"] = "Đăng xuất quản trị thành công.";
            return RedirectToAction("Index");
        }
    }
}