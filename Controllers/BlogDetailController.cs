using GoEASy.Filters;
using GoEASy.Models;
using GoEASy.Repositories;
using GoEASy.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace GoEASy.Controllers
{
    [AdminAuthorize]
    [Route("admin/blog-detail")]
    public class BlogDetailController : Controller
    {
        private readonly IBlogDetailService _blogDetailService;
        private readonly IGenericRepository<Blog> _blogRepo;
        private readonly IGenericRepository<BlogTag> _tagRepo;
        private readonly IWebHostEnvironment _env;

        public BlogDetailController(
            IBlogDetailService blogDetailService,
            IGenericRepository<Blog> blogRepo,
            IGenericRepository<BlogTag> tagRepo,
            IWebHostEnvironment env)
        {
            _blogDetailService = blogDetailService;
            _blogRepo = blogRepo;
            _tagRepo = tagRepo;
            _env = env;
        }

        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            var details = await _blogDetailService.GetAllAsync();
            ViewBag.Blogs = await _blogRepo.GetAllAsync();
            ViewBag.Tags = await _tagRepo.GetAllAsync();
            return View("~/Views/admin/blog/BlogDetail.cshtml", details);
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create(BlogDetail model, IFormFile? mainImage, List<IFormFile>? galleryImages, List<int> selectedTags)
        {
            try
            {
                model.Status = true;
                model.CreatedAt = DateTime.Now;

                BlogImage? main = await SaveImageAsync(mainImage, "main");
                List<BlogImage> gallery = new();

                if (galleryImages != null)
                {
                    int pos = 1;
                    foreach (var file in galleryImages)
                    {
                        var img = await SaveImageAsync(file, "gallery", pos++);
                        if (img != null) gallery.Add(img);
                    }
                }

                await _blogDetailService.AddAsync(model, main, gallery, selectedTags);
                TempData["Success"] = "Created Blog Detail!";
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Failed to create: " + ex.Message;
            }

            return RedirectToAction("Index");
        }

        [HttpPost("update")]
        public async Task<IActionResult> Update(BlogDetail model, IFormFile? mainImage, List<IFormFile>? galleryImages, List<int> selectedTags)
        {
            try
            {

                BlogImage? newMain = await SaveImageAsync(mainImage, "main");
                List<BlogImage> newGallery = new();

                if (galleryImages != null)
                {
                    int pos = 1;
                    foreach (var file in galleryImages)
                    {
                        var img = await SaveImageAsync(file, "gallery", pos++);
                        if (img != null) newGallery.Add(img);
                    }
                }

                await _blogDetailService.UpdateAsync(model, newMain, newGallery, selectedTags);
                TempData["Success"] = "Updated Blog Detail!";
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Failed to update: " + ex.Message;
            }

            return RedirectToAction("Index");
        }

        [HttpPost("delete")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _blogDetailService.DeleteAsync(id);
                TempData["Success"] = "Deleted.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Failed to delete: " + ex.Message;
            }

            return RedirectToAction("Index");
        }

        [HttpPost("toggle-status")]
        public async Task<IActionResult> ToggleStatus(int id)
        {
            try
            {
                await _blogDetailService.ToggleStatusAsync(id);
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }

        private async Task<BlogImage?> SaveImageAsync(IFormFile? file, string type, int? position = null)
        {
            if (file == null || file.Length == 0) return null;

            try
            {
                string fileName = $"{Path.GetFileNameWithoutExtension(file.FileName)}_{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
                string folder = Path.Combine(_env.WebRootPath, "image", "blogs");
                Directory.CreateDirectory(folder);

                string fullPath = Path.Combine(folder, fileName);

                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                return new BlogImage
                {
                    ImageURL = $"/image/blogs/{fileName}",
                    Type = type,
                    Position = position,
                    UploadedAt = DateTime.Now
                };
            }
            catch
            {
                return null;
            }
        }
    }
}
