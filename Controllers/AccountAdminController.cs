using BCrypt.Net;
using GoEASy.Filters;
using GoEASy.Models;
using GoEASy.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace GoEASy.Controllers
{
    // [AdminAuthorize]
    [Route("admin/account-admin")]
    public class AccountAdminController : Controller
    {
        private readonly IAdminService _adminService;

        public AccountAdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        [RequirePermission("view-admin")]
        // GET: admin/account-admin
        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            var admins = await _adminService.GetAllAdminsAsync();
            return View("~/Views/admin/account_admin/AccountAdmin.cshtml", admins);
        }

        [RequirePermission("create-admin")]
        // POST: admin/account-admin/create
        [HttpPost("create")]
        public async Task<IActionResult> Create(Admin admin)
        {
            var (isValid, errors) = ValidationService.ValidateAdmin(admin);

            if (!isValid)
            {
                TempData["Error"] = string.Join("<br/>", errors); // Hiện toast đỏ
                return RedirectToAction("Index");
            }
            admin.Password = BCrypt.Net.BCrypt.HashPassword(admin.Password);
            admin.CreatedAt = DateTime.Now;
            admin.Status = admin.Status ?? true;

            await _adminService.CreateAdminAsync(admin);
            TempData["Success"] = "Admin created successfully!";
            return RedirectToAction("Index");
        }

        [RequirePermission("edit-admin")]
        // POST: admin/account-admin/update
        [HttpPost("update")]
        public async Task<IActionResult> Update(Admin admin)
        {
            try
            {
                var (isValid, errors) = ValidationService.ValidateAdmin(admin, isUpdate: true);

                if (!isValid || admin.AdminId == 0)
                {
                    TempData["Error"] = string.Join("<br/>", errors); // Chỉ toast lỗi
                    return RedirectToAction("Index");
                }

                var existingAdmin = await _adminService.GetAdminByIdAsync(admin.AdminId);
                if (existingAdmin == null)
                {
                    TempData["Error"] = "Admin not found!";
                    return RedirectToAction("Index");
                }

                // Cập nhật thông tin
                existingAdmin.Username = admin.Username;
                existingAdmin.FullName = admin.FullName;
                existingAdmin.Email = admin.Email;

                if (!string.IsNullOrWhiteSpace(admin.Password))
                    existingAdmin.Password = BCrypt.Net.BCrypt.HashPassword(admin.Password);

                existingAdmin.Phone = admin.Phone;
                existingAdmin.Address = admin.Address;
                existingAdmin.Role = admin.Role;
                existingAdmin.Status = admin.Status;
                existingAdmin.UpdatedAt = DateTime.Now;

                await _adminService.UpdateAdminAsync(existingAdmin);
                TempData["Success"] = "Admin updated successfully!";
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Failed to update admin: " + ex.Message;
            }

            return RedirectToAction("Index");
        }

        [RequirePermission("delete-admin")]
        [HttpPost("delete-confirm")]
        public async Task<IActionResult> DeleteConfirm(int id)
        {
            var admin = await _adminService.GetAdminByIdAsync(id);
            if (admin == null)
            {
                TempData["Error"] = "Admin not found!";
                return RedirectToAction("Index");
            }

            admin.Status = false;
            admin.UpdatedAt = DateTime.Now;

            await _adminService.UpdateAdminAsync(admin);
            TempData["Success"] = "Admin disabled (soft deleted) successfully!";
            return RedirectToAction("Index");
        }

        [RequirePermission("toggle-status-admin")]
        [HttpPost("toggle-status")]
        public async Task<IActionResult> ToggleStatus(int id)
        {
            await _adminService.ToggleStatusAsync(id);
            return Ok();
        }
    }
}
