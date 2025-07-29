using GoEASy.Models;
using GoEASy.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace GoEASy.Controllers
{
    [Route("profile")]
    public class ProfileController : Controller
    {
        private readonly GoEasyContext _context;
        private readonly IUserService _userService;

        public ProfileController(GoEasyContext context, IUserService userService)
        {
            _context = context;
            _userService = userService;
        }

        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            int? userId = HttpContext.Session.GetInt32("UserID"); // Sửa từ "UserId" thành "UserID"
            if (userId == null)
            {
                TempData["Error"] = "Bạn cần đăng nhập để xem thông tin cá nhân.";
                return RedirectToAction("Index", "Login");
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserID == userId);
            if (user == null)
            {
                TempData["Error"] = "Không tìm thấy thông tin người dùng.";
                return RedirectToAction("Index", "Login");
            }

            return View("~/Views/client/profile/Profile.cshtml", user);
        }

        [HttpPost("edit")]
        public async Task<IActionResult> Edit(string fullName, string phone, string address, string sex)
        {
            try
            {
                int? userId = HttpContext.Session.GetInt32("UserID"); // Sửa từ "UserId" thành "UserID"
                if (userId == null)
                {
                    TempData["Error"] = "Bạn cần đăng nhập để chỉnh sửa thông tin cá nhân.";
                    return RedirectToAction("Index", "Login");
                }

                var user = await _context.Users.FirstOrDefaultAsync(u => u.UserID == userId);
                if (user == null)
                {
                    TempData["Error"] = "Không tìm thấy thông tin người dùng.";
                    return RedirectToAction("Index", "Login");
                }

                // Format dữ liệu trước khi validate
                fullName = ValidationService.FormatName(fullName);
                phone = ValidationService.FormatPhone(phone);

                // Validate cơ bản
                if (string.IsNullOrWhiteSpace(fullName) || string.IsNullOrWhiteSpace(phone))
                {
                    TempData["Error"] = "Vui lòng nhập đầy đủ họ tên và số điện thoại.";
                    return RedirectToAction("Index");
                }

                // Validate định dạng
                if (!ValidationService.IsValidName(fullName))
                {
                    TempData["Error"] = "Họ tên không hợp lệ.";
                    return RedirectToAction("Index");
                }

                if (!ValidationService.IsValidPhone(phone))
                {
                    TempData["Error"] = "Số điện thoại không hợp lệ.";
                    return RedirectToAction("Index");
                }

                // Cập nhật thông tin
                user.FullName = fullName.Trim();
                user.Phone = phone.Trim();
                user.Address = address?.Trim();
                user.Sex = sex;
                user.UpdatedAt = DateTime.Now;

                await _context.SaveChangesAsync();
                TempData["Success"] = "Cập nhật thông tin thành công!";
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Lỗi khi cập nhật thông tin: " + ex.Message;
            }

            return RedirectToAction("Index");
        }

        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword(string currentPassword, string newPassword, string confirmPassword)
        {
            try
            {
                int? userId = HttpContext.Session.GetInt32("UserID"); // Sửa từ "UserId" thành "UserID"
                if (userId == null)
                {
                    TempData["Error"] = "Bạn cần đăng nhập để đổi mật khẩu.";
                    return RedirectToAction("Index", "Login");
                }

                var user = await _context.Users.FirstOrDefaultAsync(u => u.UserID == userId);
                if (user == null)
                {
                    TempData["Error"] = "Không tìm thấy thông tin người dùng.";
                    return RedirectToAction("Index", "Login");
                }

                // Validate input
                if (string.IsNullOrEmpty(currentPassword) || string.IsNullOrEmpty(newPassword) || string.IsNullOrEmpty(confirmPassword))
                {
                    TempData["Error"] = "Vui lòng nhập đầy đủ thông tin mật khẩu.";
                    return RedirectToAction("Index");
                }

                // Kiểm tra mật khẩu hiện tại
                if (!BCrypt.Net.BCrypt.Verify(currentPassword, user.Password))
                {
                    TempData["Error"] = "Mật khẩu hiện tại không đúng.";
                    return RedirectToAction("Index");
                }

                // Kiểm tra mật khẩu mới và xác nhận
                if (newPassword != confirmPassword)
                {
                    TempData["Error"] = "Mật khẩu mới và xác nhận mật khẩu không khớp.";
                    return RedirectToAction("Index");
                }

                // Validate mật khẩu mới
                if (!ValidationService.IsStrongPassword(newPassword))
                {
                    TempData["Error"] = "Mật khẩu mới phải có ít nhất 8 ký tự, bao gồm chữ hoa, chữ thường và số.";
                    return RedirectToAction("Index");
                }

                // Cập nhật mật khẩu
                user.Password = BCrypt.Net.BCrypt.HashPassword(newPassword);
                user.UpdatedAt = DateTime.Now;

                await _context.SaveChangesAsync();
                TempData["Success"] = "Đổi mật khẩu thành công!";
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Lỗi khi đổi mật khẩu: " + ex.Message;
            }

            return RedirectToAction("Index");
        }
    }
} 
