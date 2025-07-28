using GoEASy.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Net.Mail;
using System.Net;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;

namespace GoEASy.Controllers
{
    [Route("login")]
    public class LoginController : Controller
    {
        private readonly LoginService _loginService;
        private readonly IConfiguration _configuration;

        public LoginController(LoginService loginService, IConfiguration configuration)
        {
            _loginService = loginService;
            _configuration = configuration;
        }

        // GET: /login
        [HttpGet("")]
        public IActionResult Index()
        {
            return View("~/Views/client/auth/login.cshtml");
        }

        // POST: /login
        [HttpPost("")]
        public async Task<IActionResult> Login(string username, string email, string password)
        {
            try
            {
                if (string.IsNullOrEmpty(password) || (string.IsNullOrEmpty(username) && string.IsNullOrEmpty(email)))
                {
                    TempData["Error"] = "Vui lòng nhập username hoặc email và mật khẩu";
                    return RedirectToAction("Index");
                }

                GoEASy.Models.User? user = null;
                string message = "";
                bool success = false;

                if (!string.IsNullOrEmpty(username))
                {
                    user = await _loginService.GetUserByUsernameAsync(username);
                    if (user != null && _loginService.VerifyPassword(password, user.Password))
                    {
                        success = true;
                        message = "Đăng nhập thành công";
                    }
                    else
                    {
                        message = "Sai username hoặc mật khẩu";
                    }
                }
                else if (!string.IsNullOrEmpty(email))
                {
                    // Nếu email có dạng username (không chứa @), thử đăng nhập như username
                    if (!email.Contains("@"))
                    {
                        user = await _loginService.GetUserByUsernameAsync(email);
                        if (user != null && _loginService.VerifyPassword(password, user.Password))
                        {
                            success = true;
                            message = "Đăng nhập thành công";
                        }
                        else
                        {
                            message = "Sai username hoặc mật khẩu";
                        }
                    }
                    else
                    {
                        (success, message, user) = await _loginService.LoginAsync(email, password);
                    }
                }

                if (success && user != null)
                {
                    // Tạo session cho user
                    HttpContext.Session.SetInt32("UserId", user.UserId);
                    HttpContext.Session.SetString("UserEmail", user.Email ?? "");
                    HttpContext.Session.SetString("UserName", user.FullName);
                    HttpContext.Session.SetString("UserRole", user.Role?.RoleName ?? "User");
                    HttpContext.Session.SetInt32("RoleId", user.RoleId ?? 1);

                    TempData["Success"] = "Đăng nhập thành công!";
                    // Redirect theo role
                    if (user.RoleId == 1)
                        return RedirectToAction("Index", "Home");
                    else if (user.RoleId == 2)
                        return RedirectToAction("Index", "ProviderTour");
                    else
                        return RedirectToAction("Index", "Home");
                }
                else
                {
                    TempData["Error"] = message;
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
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
                    var newUser = await _loginService.GetUserByEmailAsync(user.Email);
                    HttpContext.Session.SetInt32("UserId", newUser.UserId);
                    HttpContext.Session.SetString("UserEmail", newUser.Email ?? "");
                    HttpContext.Session.SetString("UserName", newUser.FullName);
                    HttpContext.Session.SetString("UserRole", newUser.Role?.RoleName ?? "User");
                    HttpContext.Session.SetInt32("RoleId", newUser.RoleId ?? 1);
                    HttpContext.Session.SetString("NeedCompleteProfile", "true");
                    TempData["Success"] = "Đăng ký thành công! Vui lòng bổ sung thông tin cá nhân.";
                    return RedirectToAction("CompleteProfile", "ExternalAuth");
                }
                else
                {
                    TempData["Error"] = "Email đã tồn tại hoặc có lỗi xảy ra";
                    return RedirectToAction("Index");
                }
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

        // GET: /login/forgot-password
        [HttpGet("forgot-password")]
        public IActionResult ForgotPassword()
        {
            return View("~/Views/client/auth/ForgotPassword.cshtml");
        }

        // POST: /login/forgot-password
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPasswordPost(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                TempData["Error"] = "Vui lòng nhập email.";
                return RedirectToAction("ForgotPassword");
            }
            var user = await _loginService.GetUserByEmailAsync(email);
            if (user == null)
            {
                TempData["Error"] = "Không tìm thấy tài khoản với email này.";
                return RedirectToAction("ForgotPassword");
            }
            // Tạo token reset password (giả lập, thực tế nên lưu vào DB)
            var token = Guid.NewGuid().ToString();
            // Tạo link reset password
            var resetLink = Url.Action("ResetPassword", "Login", new { email = email, token = token }, Request.Scheme);
            // Đọc cấu hình email từ appsettings.json
            var emailSettings = _configuration.GetSection("EmailSettings");
            string fromEmail = emailSettings["From"];
            string fromPassword = emailSettings["Password"];
            try
            {
                var mail = new MailMessage();
                mail.From = new MailAddress(fromEmail, "GoEasy Team");
                mail.To.Add(email);
                mail.Subject = "Yêu cầu đặt lại mật khẩu GoEasy";
                string logoUrl = "https://i.postimg.cc/sxBXQxmJ/logo-main.png";
                mail.Body = $@"
                <div style='font-family: Arial, sans-serif; max-width: 600px; margin: auto; border: 1px solid #eee; border-radius: 10px; overflow: hidden;'>
                    <div style='background: #f8fafc; padding: 24px 24px 12px 24px; text-align: center;'>
                        <img src='{logoUrl}' alt='GoEasy Logo' style='height: 60px; margin-bottom: 10px;'>
                        <h2 style='color: #2ed573; margin: 0;'>GoEasy - Đặt lại mật khẩu</h2>
                    </div>
                    <div style='padding: 24px; background: #fff;'>
                        <p style='font-size: 16px; color: #222;'>Chào <b>{user.FullName}</b>,</p>
                        <p style='font-size: 15px; color: #333;'>Bạn vừa yêu cầu đặt lại mật khẩu cho tài khoản GoEasy.</p>
                        <div style='margin: 24px 0; text-align: center;'>
                            <a href='{resetLink}' style='display: inline-block; background: #2ed573; color: #fff; padding: 12px 28px; border-radius: 8px; text-decoration: none; font-size: 16px; font-weight: 600;'>Đặt lại mật khẩu</a>
                        </div>
                        <p style='font-size: 14px; color: #888; margin-bottom: 0;'>Nếu bạn không thực hiện yêu cầu này, vui lòng bỏ qua email này.</p>
                        <p style='font-size: 15px; color: #222; margin-top: 24px;'>Trân trọng,<br><b>Đội ngũ GoEasy</b></p>
                    </div>
                </div>
                ";
                mail.IsBodyHtml = true;
                using (var smtp = new SmtpClient("smtp.gmail.com", 587))
                {
                    smtp.Credentials = new NetworkCredential(fromEmail, fromPassword);
                    smtp.EnableSsl = true;
                    await smtp.SendMailAsync(mail);
                }
                TempData["Success"] = "Đã gửi email hướng dẫn đặt lại mật khẩu. Vui lòng kiểm tra hộp thư đến.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Có lỗi khi gửi email: " + ex.Message;
            }
            // TODO: Lưu token vào DB để xác thực ở bước reset password
            return RedirectToAction("ForgotPassword");
        }

        // GET: /login/reset-password
        [HttpGet("reset-password")]
        public IActionResult ResetPassword(string email, string token)
        {
            // TODO: Kiểm tra token hợp lệ (giả lập, luôn hợp lệ ở bước này)
            ViewBag.Email = email;
            ViewBag.Token = token;
            return View("~/Views/client/auth/ResetPassword.cshtml");
        }

        // POST: /login/reset-password
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPasswordPost(string email, string token, string newPassword, string confirmPassword)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(token))
            {
                TempData["Error"] = "Yêu cầu không hợp lệ.";
                return RedirectToAction("ForgotPassword");
            }
            if (string.IsNullOrWhiteSpace(newPassword) || newPassword.Length < 6)
            {
                TempData["Error"] = "Mật khẩu mới phải có ít nhất 6 ký tự.";
                return RedirectToAction("ResetPassword", new { email, token });
            }
            if (newPassword != confirmPassword)
            {
                TempData["Error"] = "Mật khẩu xác nhận không khớp.";
                return RedirectToAction("ResetPassword", new { email, token });
            }
            // TODO: Kiểm tra token hợp lệ (giả lập, luôn hợp lệ ở bước này)
            var user = await _loginService.GetUserByEmailAsync(email);
            if (user == null)
            {
                TempData["Error"] = "Không tìm thấy tài khoản.";
                return RedirectToAction("ForgotPassword");
            }
            // Cập nhật mật khẩu mới (hash bằng BCrypt)
            user.Password = BCrypt.Net.BCrypt.HashPassword(newPassword);
            user.UpdatedAt = DateTime.Now;
            await _loginService.UpdateUserAsync(user);
            TempData["Success"] = "Đặt lại mật khẩu thành công! Bạn có thể đăng nhập với mật khẩu mới.";
            return RedirectToAction("Index");
        }

        [HttpGet("/register")]
        public IActionResult RegisterPage()
        {
            return View("~/Views/client/auth/register.cshtml");
        }
    }
} 