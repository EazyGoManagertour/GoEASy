using Microsoft.AspNetCore.Mvc;
using GoEASy.Models;
using GoEASy.Services;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;

namespace GoEASy.Controllers
{
    [Route("admin/user-manager")]
    public class UserManagerController : Controller
    {
        private readonly IUserService _userService;

        public UserManagerController(IUserService userService)
        {
            _userService = userService;
        }

        // GET: Hiển thị danh sách users
        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            var users = await _userService.GetAllUsersAsync();
            return View("~/Views/admin/user_manager/UserManager.cshtml", users);
        }

        // GET: Lấy thông tin user theo ID (cho edit)
        [HttpGet("get/{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
                return NotFound();
            return Json(new
            {
                userId = user.UserId,
                username = user.Username,
                fullName = user.FullName,
                email = user.Email,
                phone = user.Phone,
                address = user.Address,
                isVip = user.IsVip,
                vippoints = user.Vippoints,
                roleId = user.RoleId,
                status = user.Status
            });
        }

        // POST: Thêm user mới
        [HttpPost("add")]
        public async Task<IActionResult> AddUser([FromBody] User user)
        {
            if (ModelState.IsValid)
            {
                // Validate user data
                var (isValid, errors) = ValidationService.ValidateUser(user, false);
                if (!isValid)
                {
                    return BadRequest(new { message = string.Join("; ", errors) });
                }

                if (await _userService.UsernameExistsAsync(user.Username))
                {
                    return BadRequest(new { message = "Username đã tồn tại!" });
                }
                if (!string.IsNullOrEmpty(user.Email) && await _userService.EmailExistsAsync(user.Email))
                {
                    return BadRequest(new { message = "Email đã tồn tại!" });
                }

                // Format data
                user.FullName = ValidationService.FormatName(user.FullName);
                user.Phone = ValidationService.FormatPhone(user.Phone);

                await _userService.AddUserAsync(user);
                return Json(new { success = true, message = "Thêm user thành công!" });
            }
            return BadRequest(new { message = "Dữ liệu không hợp lệ!" });
        }

        // PUT: Cập nhật user
        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] User userUpdate)
        {
            try
            {
                // Kiểm tra userUpdate có null không
                if (userUpdate == null)
                {
                    return BadRequest(new { message = "Dữ liệu cập nhật không hợp lệ!" });
                }

                var user = await _userService.GetUserByIdAsync(id);
                if (user == null)
                    return NotFound();

                // Validate user data
                var (isValid, errors) = ValidationService.ValidateUser(userUpdate, true);
                if (!isValid)
                {
                    return BadRequest(new { message = string.Join("; ", errors) });
                }

                // Kiểm tra username
                if (string.IsNullOrEmpty(userUpdate.Username))
                {
                    return BadRequest(new { message = "Username không được để trống!" });
                }

                if (await _userService.UsernameExistsAsync(userUpdate.Username, id))
                {
                    return BadRequest(new { message = "Username đã tồn tại!" });
                }

                if (!string.IsNullOrEmpty(userUpdate.Email) && await _userService.EmailExistsAsync(userUpdate.Email, id))
                {
                    return BadRequest(new { message = "Email đã tồn tại!" });
                }

                // Cập nhật các trường với null safety
                user.Username = userUpdate.Username;
                user.FullName = ValidationService.FormatName(userUpdate.FullName ?? user.FullName);
                user.Email = userUpdate.Email;
                user.Phone = ValidationService.FormatPhone(userUpdate.Phone);
                user.Address = userUpdate.Address;
                user.IsVip = userUpdate.IsVip ?? user.IsVip;
                user.Vippoints = userUpdate.Vippoints ?? user.Vippoints;
                user.RoleId = userUpdate.RoleId ?? user.RoleId;
                user.Status = userUpdate.Status ?? user.Status;

                // Chỉ cập nhật password nếu có thay đổi và không trống
                if (!string.IsNullOrEmpty(userUpdate.Password))
                {
                    user.Password = userUpdate.Password;
                }
                // Nếu password trống, giữ nguyên password cũ (không thay đổi)

                await _userService.UpdateUserAsync(user);
                return Json(new { success = true, message = "Cập nhật user thành công!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Lỗi khi cập nhật: {ex.Message}" });
            }
        }

        // DELETE: Xóa user
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
                return NotFound();
            await _userService.DeleteUserAsync(id);
            return Json(new { success = true, message = "Xóa user thành công!" });
        }

        // POST: Thay đổi trạng thái user
        [HttpPost("toggle-status/{id}")]
        public async Task<IActionResult> ToggleStatus(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
                return NotFound();
            await _userService.ToggleStatusAsync(id);
            return Json(new {
                success = true,
                message = (user.Status == true ? "Kích hoạt user thành công!" : "Vô hiệu hóa user thành công!"),
                status = user.Status
            });
        }

        // GET: Lấy danh sách roles
        [HttpGet("roles")]
        public async Task<IActionResult> GetRoles()
        {
            var roles = await _userService.GetAllRolesAsync();
            return Json(roles);
        }
    }
} 