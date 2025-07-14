using GoEASy.Models;
using GoEASy.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace GoEASy.Controllers
{
    [Route("admin/blog-tag")]
    public class BlogTagController : Controller
    {
        private readonly IBlogTagService _blogTagService;

        public BlogTagController(IBlogTagService blogTagService)
        {
            _blogTagService = blogTagService;
        }

        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            var tags = await _blogTagService.GetAllAsync();
            return View("~/Views/admin/blog/BlogTag.cshtml", tags);
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create(BlogTag tag)
        {
            try
            {
                tag.Status = tag.Status ?? true;
                await _blogTagService.CreateAsync(tag);
                TempData["Success"] = "Blog tag created successfully!";
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Failed to create blog tag: " + ex.Message;
            }

            return RedirectToAction("Index");
        }

        [HttpPost("update")]
        public async Task<IActionResult> Update(BlogTag tag)
        {
            try
            {
                var existing = await _blogTagService.GetByIdAsync(tag.TagId);
                if (existing == null)
                {
                    TempData["Error"] = "Blog tag not found!";
                    return RedirectToAction("Index");
                }

                existing.Name = tag.Name;
                existing.Status = tag.Status;

                await _blogTagService.UpdateAsync(existing);
                TempData["Success"] = "Blog tag updated successfully!";
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Failed to update blog tag: " + ex.Message;
            }

            return RedirectToAction("Index");
        }

        [HttpPost("delete")]
        public async Task<IActionResult> Delete(int id)
        {
            var tag = await _blogTagService.GetByIdAsync(id);
            if (tag == null)
            {
                TempData["Error"] = "Blog tag not found!";
                return RedirectToAction("Index");
            }

            tag.Status = false;
            await _blogTagService.UpdateAsync(tag);
            TempData["Success"] = "Blog tag disabled successfully!";
            return RedirectToAction("Index");
        }

        [HttpPost("toggle-status")]
        public async Task<IActionResult> ToggleStatus(int id)
        {
            await _blogTagService.ToggleStatusAsync(id);
            return Ok();
        }
    }
}
