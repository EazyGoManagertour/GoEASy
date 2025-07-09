using GoEASy.Models;
using GoEASy.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace GoEASy.Controllers
{
    [Route("admin/category")]
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        // GET: admin/category
        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            var categories = await _categoryService.GetAllCategoriesAsync();
            return View("~/Views/admin/category/Category.cshtml", categories);
        }

        // POST: admin/category/create
        [HttpPost("create")]
        public async Task<IActionResult> Create(TourCategory category)
        {
            try
            {
                category.CreatedAt = DateTime.Now;
                category.Status = category.Status ?? true;

                await _categoryService.CreateCategoryAsync(category);
                TempData["Success"] = "Category created successfully!";
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Failed to create category: " + ex.Message;
            }

            return RedirectToAction("Index");
        }

        // POST: admin/category/update
        [HttpPost("update")]
        public async Task<IActionResult> Update(TourCategory category)
        {
            try
            {
                var existing = await _categoryService.GetCategoryByIdAsync(category.CategoryId);
                if (existing == null)
                {
                    TempData["Error"] = "Category not found!";
                    return RedirectToAction("Index");
                }

                existing.CategoryName = category.CategoryName;
                existing.Description = category.Description;
                existing.Status = category.Status;
                existing.UpdatedAt = DateTime.Now;

                await _categoryService.UpdateCategoryAsync(existing);
                TempData["Success"] = "Category updated successfully!";
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Failed to update category: " + ex.Message;
            }

            return RedirectToAction("Index");
        }

        // POST: admin/category/delete
        [HttpPost("delete")]
        public async Task<IActionResult> Delete(int id)
        {
            var category = await _categoryService.GetCategoryByIdAsync(id);
            if (category == null)
            {
                TempData["Error"] = "Category not found!";
                return RedirectToAction("Index");
            }

            category.Status = false;
            category.UpdatedAt = DateTime.Now;

            await _categoryService.UpdateCategoryAsync(category);
            TempData["Success"] = "Category disabled (soft deleted) successfully!";
            return RedirectToAction("Index");
        }

        // POST: admin/category/toggle-status
        [HttpPost("toggle-status")]
        public async Task<IActionResult> ToggleStatus(int id)
        {
            await _categoryService.ToggleStatusAsync(id);
            return Ok();
        }
    }
}
