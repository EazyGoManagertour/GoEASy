using GoEASy.Services;
using Microsoft.AspNetCore.Mvc;

namespace GoEASy.Controllers
{
    [Route("login")]
    public class LoginController : Controller
    {
        private readonly LoginService _loginService;
        private readonly IAccessLogService _accessLogService;
       
        public LoginController(LoginService loginService, IAccessLogService accessLogService)
        {
            _loginService = loginService;
            _accessLogService = accessLogService;
        }
        // GET: /login
        [HttpGet("")]
        public IActionResult Index()
        {
            return View("~/Views/client/login.cshtml");
        }

        // POST: /login
        [HttpPost("")]
        public async Task<IActionResult> Login(string email, string password)
        {
            try
            {
                if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
                {
                    TempData["Error"] = "Email và mật khẩu không được để trống";
                    return RedirectToAction("Index");
                }

                var (success, message, user) = await _loginService.LoginAsync(email, password);

                if (success && user != null)
                {
                    // Tạo session cho user
                    HttpContext.Session.SetInt32("UserID", user.UserID); // Sửa lại dùng SetInt32
                    HttpContext.Session.SetString("UserEmail", user.Email ?? "");
                    HttpContext.Session.SetString("UserName", user.FullName);
                    HttpContext.Session.SetString("UserRole", user.Role?.RoleName ?? "User");
                    HttpContext.Session.SetInt32("RoleID", user.RoleID ?? 1);

                    TempData["Success"] = "Đăng nhập thành công!";
                    var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
                    var userAgent = Request.Headers["User-Agent"].ToString();
                    _accessLogService.LogAccess(user.UserID, ipAddress, userAgent);
                    // Redirect theo role
                    if (user.RoleID == 1)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    else if (user.RoleID == 2)
                    {
                        return RedirectToAction("Index", "ProviderTour");
                    }
                    else // fallback: admin hoặc role khác
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    TempData["Error"] = message;
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                // Log lỗi chi tiết
                Console.WriteLine($"Login Error: {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");
                
                TempData["Error"] = "Lỗi hệ thống: " + ex.Message;
                return RedirectToAction("Index");
            }
        }

        // POST: /login/register
        [HttpPost("register")]
        public async Task<IActionResult> Register(string fullName, string email, string password, string? phone)
        {
            try
            {
                if (string.IsNullOrEmpty(fullName) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
                {
                    TempData["Error"] = "Vui lòng điền đầy đủ thông tin bắt buộc";
                    return RedirectToAction("Index");
                }

                if (!ValidationService.IsValidEmail(email))
                {
                    TempData["Error"] = "Email không hợp lệ";
                    return RedirectToAction("Index");
                }

                if (password.Length < 6)
                {
                    TempData["Error"] = "Mật khẩu phải có ít nhất 6 ký tự";
                    return RedirectToAction("Index");
                }

                // Tạo user mới
                var user = new Models.User
                {
                    Username = email.Split('@')[0], // Tạo username từ email
                    Email = email,
                    Password = password,
                    FullName = ValidationService.FormatName(fullName),
                    Phone = phone,
                    Address = null
                };

                var success = await _loginService.RegisterAsync(user);

                if (success)
                {
                    TempData["Success"] = "Đăng ký thành công! Vui lòng đăng nhập.";
                }
                else
                {
                    TempData["Error"] = "Email đã tồn tại hoặc có lỗi xảy ra";
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Lỗi hệ thống: " + ex.Message;
                return RedirectToAction("Index");
            }
        }

        // POST: /login/logout
        [HttpPost("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            TempData["Success"] = "Đăng xuất thành công";
            return RedirectToAction("Index", "Home");
        }
    }
} 