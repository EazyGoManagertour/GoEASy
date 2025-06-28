using GoEASy.Models;
using GoEASy.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace GoEASy.Controllers
{
    [Route("admin/account-admin")]
    public class AccountAdminController : Controller
    {
        private readonly IAdminService _adminService;

        public AccountAdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        // GET: admin/account-admin
        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            var admins = await _adminService.GetAllAdminsAsync();
            return View("~/Views/admin/account_admin/AccountAdmin.cshtml", admins);
        }

        // POST: admin/account-admin/create
        [HttpPost("create")]
        public async Task<IActionResult> Create(Admin admin)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    TempData["Error"] = "Please fill in all required fields.";
                    return RedirectToAction("Index");
                }

                admin.CreatedAt = DateTime.Now;
                await _adminService.CreateAdminAsync(admin);

                TempData["Success"] = "Admin added successfully!";
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Failed to create admin: " + ex.Message;
            }

            return RedirectToAction("Index");
        }



        [HttpPost("update")]
        public async Task<IActionResult> Update(Admin admin)
        {
            try
            {
                // ⚠️ Xóa lỗi validation với Password nếu người dùng không nhập
                if (string.IsNullOrWhiteSpace(admin.Password))
                {
                    ModelState.Remove(nameof(admin.Password));
                }

                if (!ModelState.IsValid || admin.AdminId == 0)
                {
                    TempData["Error"] = "Invalid update data!";
                    return RedirectToAction("Index");
                }

                var existingAdmin = await _adminService.GetAdminByIdAsync(admin.AdminId);
                if (existingAdmin == null)
                {
                    TempData["Error"] = "Admin not found!";
                    return RedirectToAction("Index");
                }

                existingAdmin.Username = admin.Username;
                existingAdmin.FullName = admin.FullName;
                existingAdmin.Email = admin.Email;
                if (!string.IsNullOrEmpty(admin.Password))
                    existingAdmin.Password = admin.Password;
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


        [HttpPost("delete-confirm")]
        public async Task<IActionResult> DeleteConfirm(int id)
        {
            var admin = await _adminService.GetAdminByIdAsync(id);
            if (admin == null)
            {
                TempData["Error"] = "Admin not found!";
                return RedirectToAction("Index");
            }

            await _adminService.DeleteAdminAsync(id);
            TempData["Success"] = "Admin deleted successfully!";
            return RedirectToAction("Index");
        }

    }
}
