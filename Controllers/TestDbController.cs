using GoEASy.Models;
using GoEASy.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace GoEASy.Controllers
{
    [Route("testdb")]
    public class TestDbController : Controller
    {
        private readonly GoEasyContext _context;

        public TestDbController(GoEasyContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IActionResult> TestRoleTable()
        {
            try
            {
                // Test connection to the database
                await _context.Database.OpenConnectionAsync();
                await _context.Database.CloseConnectionAsync();

                return Content("success");
            }
            catch (Exception ex)
            {
                return Content($"error: {ex.Message}");
            }
        }

        // GET: /testdb/create-test-user
        [HttpGet("create-test-user")]
        public async Task<IActionResult> CreateTestUser()
        {
            try
            {
                // Tạo user test
                var testUser = new User
                {
                    Username = "testuser",
                    Email = "test@example.com",
                    Password = HashPassword("123456"), // Hash password
                    FullName = "Test User",
                    Phone = "0123456789",
                    Status = true,
                    Vippoints = 0,
                    IsVip = false,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };

                // Kiểm tra user đã tồn tại chưa
                var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == testUser.Email);
                if (existingUser != null)
                {
                    return Content("User test đã tồn tại! Email: test@example.com, Password: 123456");
                }

                _context.Users.Add(testUser);
                await _context.SaveChangesAsync();

                return Content("Đã tạo user test thành công! Email: test@example.com, Password: 123456");
            }
            catch (Exception ex)
            {
                return Content($"Lỗi: {ex.Message}");
            }
        }

        // GET: /testdb/update-user-password
        [HttpGet("update-user-password")]
        public async Task<IActionResult> UpdateUserPassword(string email, string newPassword)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
                if (user == null)
                {
                    return Content($"Không tìm thấy user với email: {email}");
                }

                // Hash password mới
                user.Password = HashPassword(newPassword);
                user.UpdatedAt = DateTime.Now;
                
                await _context.SaveChangesAsync();

                return Content($"Đã cập nhật password cho user {email} thành công!");
            }
            catch (Exception ex)
            {
                return Content($"Lỗi: {ex.Message}");
            }
        }

        // GET: /testdb/list-users
        [HttpGet("list-users")]
        public async Task<IActionResult> ListUsers()
        {
            try
            {
                var users = await _context.Users.Select(u => new { u.UserId, u.Email, u.FullName, u.Status }).ToListAsync();
                var result = string.Join("<br/>", users.Select(u => $"ID: {u.UserId}, Email: {u.Email}, Name: {u.FullName}, Status: {u.Status}"));
                return Content(result);
            }
            catch (Exception ex)
            {
                return Content($"Lỗi: {ex.Message}");
            }
        }

        // Helper method để hash password
        private static string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            }
        }
    }
}
