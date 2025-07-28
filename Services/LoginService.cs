using GoEASy.Models;
using GoEASy.Services;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace GoEASy.Services
{
    public class LoginService
    {
        private readonly GoEasyContext _context;

        public LoginService(GoEasyContext context)
        {
            _context = context;
        }

        public async Task<(bool Success, string Message, User? User)> LoginAsync(string email, string password)
        {
            try
            {
                // Validation
                var (isValid, errors) = ValidateLoginInput(email, password);
                if (!isValid)
                {
                    return (false, string.Join(", ", errors), null);
                }

                // Tìm user theo email
                var user = await _context.Users
                    .Include(u => u.Role)
                    .FirstOrDefaultAsync(u => u.Email == email && u.Status == true);

                if (user == null)
                {
                    return (false, "Email không tồn tại hoặc tài khoản đã bị khóa", null);
                }

                // Kiểm tra password
                if (!VerifyPassword(password, user.Password))
                {
                    return (false, "Mật khẩu không đúng", null);
                }
                // Cập nhật thời gian đăng nhập cuối
                user.UpdatedAt = DateTime.Now;
                await _context.SaveChangesAsync();

                return (true, "Đăng nhập thành công", user);
            }
            catch (Exception ex)
            {
                return (false, "Lỗi hệ thống: " + ex.Message, null);
            }
        }

        private (bool IsValid, List<string> Errors) ValidateLoginInput(string email, string password)
        {
            var errors = new List<string>();

            // Email validation
            if (string.IsNullOrWhiteSpace(email))
            {
                errors.Add("Email không được để trống");
            }
            else if (!ValidationService.IsValidEmail(email))
            {
                errors.Add("Email không hợp lệ");
            }

            // Password validation
            if (string.IsNullOrWhiteSpace(password))
            {
                errors.Add("Mật khẩu không được để trống");
            }
            else if (password.Length < 6)
            {
                errors.Add("Mật khẩu phải có ít nhất 6 ký tự");
            }

            return (errors.Count == 0, errors);
        }

        public bool VerifyPassword(string inputPassword, string storedPassword)
        {
            // So sánh bằng BCrypt
            return BCrypt.Net.BCrypt.Verify(inputPassword, storedPassword);
        }

        public async Task<User?> GetUserByUsernameAsync(string username)
        {
            return await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Username == username && u.Status == true);
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Email == email && u.Status == true);
        }

        public async Task UpdateUserAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> RegisterAsync(User user)
        {
            try
            {
                // Kiểm tra email đã tồn tại chưa
                var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == user.Email);
                if (existingUser != null)
                {
                    return false;
                }

                // Hash password bằng BCrypt
                user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
                
                // Set default values
                user.CreatedAt = DateTime.Now;
                user.UpdatedAt = DateTime.Now;
                user.Status = true;
                user.Vippoints = 0;
                user.IsVip = false;

                _context.Users.Add(user);
                await _context.SaveChangesAsync();
                
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
} 