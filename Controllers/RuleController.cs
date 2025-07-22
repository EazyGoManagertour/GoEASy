using GoEASy.Filters;
using GoEASy.Models;
using GoEASy.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace GoEASy.Controllers
{
    //[AdminAuthorize]
    [Route("admin/rules")]
    public class RulesController : Controller
    {
        private readonly IRuleService _ruleService;
        private readonly IAdminService _adminService;
        private readonly IActionService _actionService;
        private readonly GoEasyContext _context;

        public RulesController(IRuleService ruleService, IAdminService adminService, IActionService actionService, GoEasyContext context)
        {
            _ruleService = ruleService;
            _adminService = adminService;
            _actionService = actionService;
            _context = context;
        }

        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            var rules = await _ruleService.GetAllRulesAsync();
            ViewBag.Rules = rules;
            var admins = await _adminService.GetAllAdminsAsync(); 
            ViewBag.Admins = admins;
            var actions = await _actionService.GetAllActionsAsync();
            ViewBag.Actions = actions;

            return View("~/Views/admin/rules/Rules.cshtml", rules);
        }

        [HttpGet("get-role")]
        public async Task<IActionResult> GetRoleByAdmin(int adminId)
        {
            var admin = await _adminService.GetAdminByIdAsync(adminId);
            if (admin == null) return NotFound();

            return Json(new { role = admin.Role });
        }

        [HttpGet("get-rule")]
        public async Task<IActionResult> GetRuleByAdmin(int adminId)
        {
            var admin = await _context.Admins
                .Include(a => a.Rule) // Include để lấy thông tin từ bảng Rule
                .FirstOrDefaultAsync(a => a.AdminId == adminId);

            if (admin == null)
                return NotFound();

            return Json(new { role = admin.Rule?.RuleName ?? "Chưa có quyền nào" });
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create(Rule rule)
        {
            if (string.IsNullOrWhiteSpace(rule.RuleName) || string.IsNullOrWhiteSpace(rule.Slug))
            {
                TempData["Error"] = "Rule name and slug are required.";
                return RedirectToAction("Index");
            }

            rule.CreatedAt = DateTime.Now;
            rule.IsOpen = rule.IsOpen ?? true;

            await _ruleService.AddRuleAsync(rule);
            TempData["Success"] = "Rule created successfully!";
            return RedirectToAction("Index");
        }

        [HttpPost("update")]
        public async Task<IActionResult> Update(Rule rule)
        {
            try
            {
                if (rule.RuleId == 0 || string.IsNullOrWhiteSpace(rule.RuleName))
                {
                    TempData["Error"] = "Invalid rule data.";
                    return RedirectToAction("Index");
                }

                var existing = await _ruleService.GetRuleByIdAsync(rule.RuleId);
                if (existing == null)
                {
                    TempData["Error"] = "Rule not found!";
                    return RedirectToAction("Index");
                }

                existing.RuleName = rule.RuleName;
                existing.Slug = rule.Slug;
                existing.ListRuleSlug = rule.ListRuleSlug;
                existing.IsOpen = rule.IsOpen;

                await _ruleService.UpdateRuleAsync(existing);
                TempData["Success"] = "Rule updated successfully!";
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Failed to update rule: " + ex.Message;
            }

            return RedirectToAction("Index");
        }


        [HttpPost("create-permissions")]
        public async Task<IActionResult> CreatePermission([FromForm] string ActionName, [FromForm] string ActionSlug)
        {
            if (string.IsNullOrWhiteSpace(ActionName) || string.IsNullOrWhiteSpace(ActionSlug))
            {
                TempData["Error"] = "Action Name và Slug không được để trống.";
                return RedirectToAction("Index");
            }

            // Kiểm tra trùng tên hoặc slug
            bool isDuplicate = _context.Actions
                .Any(a => a.ActionName == ActionName || a.ActionSlug == ActionSlug);

            if (isDuplicate)
            {
                TempData["Error"] = "Tên hoặc Slug đã tồn tại, vui lòng chọn giá trị khác.";
                return RedirectToAction("Index");
            }

            var action = new GoEASy.Models.Action
            {
                ActionName = ActionName,
                ActionSlug = ActionSlug,
                CreatedAt = DateTime.Now
            };

            await _actionService.AddActionAsync(action);

            TempData["Success"] = "Thêm permission mới thành công!";
            return RedirectToAction("Index");
        }
        [HttpPost("update-permission")]
        public async Task<IActionResult> UpdatePermission(int ActionId, string ActionName, string ActionSlug)
        {
            var action = await _context.Actions.FindAsync(ActionId);
            if (action == null)
            {
                TempData["Error"] = "Permission không tồn tại!";
                return RedirectToAction("Index");
            }

            // Kiểm tra trùng tên hoặc slug (ngoại trừ chính nó)
            bool isDuplicate = await _context.Actions
                .AnyAsync(a =>
                    (a.ActionName == ActionName || a.ActionSlug == ActionSlug) &&
                    a.ActionId != ActionId);

            if (isDuplicate)
            {
                TempData["Error"] = "Tên hoặc Slug đã tồn tại!";
                return RedirectToAction("Index");
            }

            action.ActionName = ActionName;
            action.ActionSlug = ActionSlug;
            await _context.SaveChangesAsync();

            TempData["Success"] = "Cập nhật permission thành công!";
            return RedirectToAction("Index");
        }

        [HttpPost("delete-permission")]
        public async Task<IActionResult> DeletePermission(int ActionId)
        {
            var action = await _context.Actions.FindAsync(ActionId);
            if (action != null)
            {
                _context.Actions.Remove(action);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Đã xoá permission.";
            }
            return RedirectToAction("Index");
        }

        [HttpPost("apply-permissions")]
        public async Task<IActionResult> ApplyPermissions(int AdminId, int RuleId, string SelectedSlugs)
        {
            Console.WriteLine($"📥 Gửi về: AdminId = {AdminId}, RuleId = {RuleId}, Slugs = {SelectedSlugs}");

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // Tìm đúng Admin
                var admin = await _context.Admins.FirstOrDefaultAsync(a => a.AdminId == AdminId);
                if (admin == null)
                {
                    Console.WriteLine("Không tìm thấy admin.");
                    TempData["Error"] = "Admin không tồn tại!";
                    return RedirectToAction("Index");
                }

                admin.RuleId = RuleId;
                _context.Admins.Update(admin);
                Console.WriteLine("Gán RuleId cho Admin thành công");

                // Cập nhật Rule
                var rule = await _context.Rules.FirstOrDefaultAsync(r => r.RuleId == RuleId);
                if (rule != null)
                {
                    rule.ListRuleSlug = SelectedSlugs?.Trim();
                    _context.Rules.Update(rule);
                    Console.WriteLine("✅ Cập nhật ListRuleSlug thành công");
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                TempData["Success"] = "Update Successfull!";
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                TempData["Error"] = $" Error: {ex.Message}";
            }

            return RedirectToAction("Index");
        }

        [HttpGet("search-permissions")]
        public async Task<IActionResult> SearchPermissions(string keyword)
        {
            keyword = keyword?.Trim().ToLower() ?? "";

            var results = await _context.Actions
                .Where(a =>
                    string.IsNullOrEmpty(keyword) ||
                    a.ActionName.ToLower().Contains(keyword) ||
                    a.ActionSlug.ToLower().Contains(keyword))
                .ToListAsync();

            return PartialView("_PermissionTableBody", results);
        }



    }
}