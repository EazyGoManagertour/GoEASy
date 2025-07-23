using GoEASy.Filters;
using GoEASy.Models;
using GoEASy.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace GoEASy.Controllers
{
    // [AdminAuthorize]
    [Route("admin/user-manager")]
    public class UserManagerController : Controller
    {
        private readonly IUserService _userService;

        public UserManagerController(IUserService userService)
        {
            _userService = userService;
        }

        // GET: admin/user-manager
        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            var users = await _userService.GetAllUsersAsync();
            var roles = await _userService.GetAllRolesAsync();

            ViewBag.Roles = roles;
            return View("~/Views/admin/user_manager/UserManager.cshtml", users);
        }

        // POST: admin/user-manager/create
        [HttpPost("create")]
        public async Task<IActionResult> Create(User user)
        {
            try
            {
                // Format dữ liệu trước khi validate
                user.FullName = ValidationService.FormatName(user.FullName);
                user.Phone = ValidationService.FormatPhone(user.Phone);

                // Validate sử dụng ValidationService
                var (isValid, errors) = ValidationService.ValidateUser(user, isUpdate: false);
                if (!isValid)
                {
                    TempData["Error"] = string.Join("; ", errors);
                    return RedirectToAction("Index");
                }

                // Validate unique username and email
                if (await _userService.UsernameExistsAsync(user.Username))
                {
                    TempData["Error"] = "Username đã tồn tại.";
                    return RedirectToAction("Index");
                }

                if (!string.IsNullOrEmpty(user.Email) && await _userService.EmailExistsAsync(user.Email))
                {
                    TempData["Error"] = "Email đã tồn tại.";
                    return RedirectToAction("Index");
                }

                if (string.IsNullOrEmpty(user.Sex))
                {
                    user.Sex = "Male";
                }
                if (!string.IsNullOrEmpty(user.Password))
                {
                    user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
                }
                
                user.CreatedAt = DateTime.Now;
                await _userService.CreateUserAsync(user);

                TempData["Success"] = "Thêm user thành công!";
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Lỗi khi tạo user: " + ex.Message;
            }

            return RedirectToAction("Index");
        }

        // POST: admin/user-manager/update
        [HttpPost("update")]
        public async Task<IActionResult> Update(User user)
        {
            try
            {
                if (user.UserID == 0)
                {
                    TempData["Error"] = "Dữ liệu cập nhật không hợp lệ!";
                    return RedirectToAction("Index");
                }

                var existingUser = await _userService.GetUserByIdAsync(user.UserID);
                if (existingUser == null)
                {
                    TempData["Error"] = "Không tìm thấy user!";
                    return RedirectToAction("Index");
                }

                // Format dữ liệu trước khi validate
                user.FullName = ValidationService.FormatName(user.FullName);
                user.Phone = ValidationService.FormatPhone(user.Phone);

                // Validate sử dụng ValidationService (isUpdate = true)
                var (isValid, errors) = ValidationService.ValidateUser(user, isUpdate: true);
                if (!isValid)
                {
                    TempData["Error"] = string.Join("; ", errors);
                    return RedirectToAction("Index");
                }

                // Validate unique username and email (excluding current user)
                if (await _userService.UsernameExistsAsync(user.Username, user.UserID))
                {
                    TempData["Error"] = "Username đã tồn tại.";
                    return RedirectToAction("Index");
                }

                if (!string.IsNullOrEmpty(user.Email) && await _userService.EmailExistsAsync(user.Email, user.UserID))
                {
                    TempData["Error"] = "Email đã tồn tại.";
                    return RedirectToAction("Index");
                }

                // Update user properties
                existingUser.Username = user.Username;
                existingUser.FullName = user.FullName;
                existingUser.Email = user.Email;
                if (!string.IsNullOrEmpty(user.Password))
                {
                    existingUser.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
                }
                existingUser.Phone = user.Phone;
                existingUser.Address = user.Address;
                existingUser.Sex = user.Sex;
                existingUser.RoleID = user.RoleID;
                existingUser.IsVIP = user.IsVIP;
                existingUser.VIPPoints  = user.VIPPoints ;
                existingUser.Status = user.Status;
                existingUser.UpdatedAt = DateTime.Now;

                await _userService.UpdateUserAsync(existingUser);

                TempData["Success"] = "Cập nhật user thành công!";
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Lỗi khi cập nhật user: " + ex.Message;
            }

            return RedirectToAction("Index");
        }

        // POST: admin/user-manager/delete-confirm
        [HttpPost("delete-confirm")]
        public async Task<IActionResult> DeleteConfirm(int id)
        {
            try
            {
                var user = await _userService.GetUserByIdAsync(id);
                if (user == null)
                {
                    TempData["Error"] = "Không tìm thấy user!";
                    return RedirectToAction("Index");
                }

                await _userService.DeleteUserAsync(id);
                TempData["Success"] = "Xóa user thành công!";
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Lỗi khi xóa user: " + ex.Message;
            }

            return RedirectToAction("Index");
        }

        // POST: admin/user-manager/toggle-status/{id}

        // GET: admin/user-manager/get-roles
        [HttpGet("get-roles")]
        public async Task<IActionResult> GetRoles()
        {
            try
            {
                var roles = await _userService.GetAllRolesAsync();
                return Json(roles);
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message });
            }
        }
        [HttpPost("toggle-status")]
        public async Task<IActionResult> ToggleStatus(int id)
        {
            await _userService.ToggleStatusAsync(id);
            return Ok();
        }
    }
}