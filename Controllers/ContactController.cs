using GoEASy.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Net;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GoEASy.Controllers
{
    [Route("contact")]
    public class ContactController : Controller
    {
        private readonly GoEasyContext _context;
        private readonly IConfiguration _configuration;
        private readonly ILogger<ContactController> _logger;
        public ContactController(GoEasyContext context, IConfiguration configuration, ILogger<ContactController> logger)
        {
            _context = context;
            _configuration = configuration;
            _logger = logger;
        }

        // GET: /contact
        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            _logger.LogInformation("[Contact] GET /contact called");
            // Nếu user đã đăng nhập, lấy thông tin user để autofill form
            int? userId = HttpContext.Session.GetInt32("UserID"); // Sửa từ "UserId" thành "UserID"
            if (userId != null)
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.UserID == userId);
                ViewBag.UserFullName = user?.FullName;
                ViewBag.UserEmail = user?.Email;
                ViewBag.UserPhone = user?.Phone;
            }
            return View("~/Views/client/contact/Contact.cshtml");
        }

        // POST: /contact
        [HttpPost("")]
        public async Task<IActionResult> Submit(string fullName, string email, string phone, string message, string? termsCondition)
        {
            _logger.LogInformation($"[Contact] POST /contact: fullName={fullName}, email={email}, phone={phone}, message={message}, termsCondition={termsCondition}");
            Console.WriteLine($"[Contact] POST /contact: fullName={fullName}, email={email}, phone={phone}, message={message}, termsCondition={termsCondition}");
            // Kiểm tra dữ liệu bắt buộc
            if (string.IsNullOrWhiteSpace(fullName) || string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(message))
            {
                _logger.LogWarning("[Contact] Missing required fields");
                TempData["Error"] = "Vui lòng điền đầy đủ thông tin bắt buộc.";
                return RedirectToAction("Index");
            }
            // Kiểm tra đã tick đồng ý điều khoản chưa
            if (termsCondition != "on")
            {
                _logger.LogWarning("[Contact] User did not agree to terms");
                TempData["Error"] = "Bạn cần đồng ý lưu thông tin trước khi gửi liên hệ.";
                return RedirectToAction("Index");
            }
            try
            {
                // Lưu liên hệ vào database
                var contact = new Contact
                {
                    FullName = fullName,
                    Email = email,
                    Phone = phone,
                    Message = message,
                    CreatedAt = DateTime.Now,
                    Status = "New"
                };
                _context.Contacts.Add(contact);
                await _context.SaveChangesAsync();
                _logger.LogInformation("[Contact] Saved contact to database");
                Console.WriteLine("[Contact] Saved contact to database");

                // Đọc cấu hình email từ appsettings.json
                var emailSettings = _configuration.GetSection("EmailSettings");
                string fromEmail = emailSettings["From"];
                string fromPassword = emailSettings["Password"];

                // Gửi email xác nhận cho người dùng
                var mail = new System.Net.Mail.MailMessage();
                mail.From = new System.Net.Mail.MailAddress(fromEmail, "GoEasy Team"); // Địa chỉ gửi đi
                mail.To.Add(email); // Địa chỉ nhận là email người dùng nhập
                mail.Subject = "Xác nhận gửi liên hệ thành công";
                string logoUrl = "https://i.postimg.cc/sxBXQxmJ/logo-main.png";
 // Đường dẫn logo, chỉnh lại domain nếu deploy thật
                mail.Body = $@"
                <div style='font-family: Arial, sans-serif; max-width: 600px; margin: auto; border: 1px solid #eee; border-radius: 10px; overflow: hidden;'>
                    <div style='background: #f8fafc; padding: 24px 24px 12px 24px; text-align: center;'>
                        <img src='{logoUrl}' alt='GoEasy Logo' style='height: 60px; margin-bottom: 10px;'>
                        <h2 style='color: #2ed573; margin: 0;'>GoEasy - Xác nhận liên hệ</h2>
                    </div>
                    <div style='padding: 24px; background: #fff;'>
                        <p style='font-size: 16px; color: #222;'>Chào <b>{fullName}</b>,</p>
                        <p style='font-size: 15px; color: #333;'>Cảm ơn bạn đã liên hệ với <b>GoEasy</b>! Chúng tôi đã nhận được thông tin của bạn và sẽ phản hồi sớm nhất có thể.</p>
                        <hr style='margin: 24px 0;'>
                        <div style='background: #f6f6f6; border-radius: 8px; padding: 16px; margin-bottom: 16px;'>
                            <h4 style='margin: 0 0 8px 0; color: #0099ff;'>Nội dung liên hệ của bạn:</h4>
                            <div style='font-size: 15px; color: #444;'>{message}</div>
                        </div>
                        <p style='font-size: 14px; color: #888; margin-bottom: 0;'>Nếu bạn không thực hiện yêu cầu này, vui lòng bỏ qua email này.</p>
                        <p style='font-size: 15px; color: #222; margin-top: 24px;'>Trân trọng,<br><b>Đội ngũ GoEasy</b></p>
                    </div>
                </div>
                ";
                mail.IsBodyHtml = true;

                using (var smtp = new System.Net.Mail.SmtpClient("smtp.gmail.com", 587))
                {
                    smtp.Credentials = new System.Net.NetworkCredential(fromEmail, fromPassword); // Thay bằng email và app password thật
                    smtp.EnableSsl = true;
                    await smtp.SendMailAsync(mail);
                }
                _logger.LogInformation("[Contact] Sent confirmation email to user");
                Console.WriteLine("[Contact] Sent confirmation email to user");
                TempData["Success"] = "Gửi liên hệ thành công! Vui lòng kiểm tra email để xác nhận.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[Contact] Error when saving contact or sending email");
                Console.WriteLine($"[Contact] Error: {ex.Message}");
                TempData["Success"] = "Gửi liên hệ thành công! Tuy nhiên có lỗi khi gửi email xác nhận: " + ex.Message;
            }
            return RedirectToAction("Index");
        }
    }
} 
