using GoEASy.Filters;
using GoEASy.Models;
using GoEASy.Repositories;
using GoEASy.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace GoEASy.Controllers
{
    [AdminAuthorize]
    [Route("admin/blog")]
    public class BlogController : Controller
    {
        private readonly IBlogService _blogService;
        private readonly ICategoryService _categoryService;
        private readonly IGenericRepository<BlogImage> _blogImageRepo;
        private readonly IWebHostEnvironment _env;

        public BlogController(
            IBlogService blogService,
            ICategoryService categoryService,
            IGenericRepository<BlogImage> blogImageRepo,
            IWebHostEnvironment env)
        {
            _blogService = blogService;
            _categoryService = categoryService;
            _blogImageRepo = blogImageRepo;
            _env = env;
        }

        // GET: admin/blog
        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            var blogs = await _blogService.GetAllAsync();
            ViewBag.Categories = await _categoryService.GetAllCategoriesAsync();

            return View("~/Views/admin/blog/Blog.cshtml", blogs);
        }

        // POST: admin/blog/create
        [HttpPost("create")]
        public async Task<IActionResult> Create(Blog blog, IFormFile avatar)
        {
            try
            {
                if (avatar != null && avatar.Length > 0)
                {
                    var image = await SaveImageAsync(avatar);
                    if (image != null)
                    {
                        blog.BlogImages.Add(image);
                    }
                }

                var adminId = HttpContext.Session.GetInt32("AdminID");

                if (adminId.HasValue)
                {
                    blog.AuthorAdminID = adminId.Value;
                }
                else
                {
                    TempData["Error"] = "You Need Login.";
                    return RedirectToAction("Index");
                }

                blog.CreatedAt = DateTime.Now;
                blog.Status = blog.Status ?? true;

                await _blogService.CreateAsync(blog);
                TempData["Success"] = "Blog created and submitted for approval!";
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Failed to create blog: " + ex.Message;
            }

            return RedirectToAction("Index");
        }

        // POST: admin/blog/update
        [HttpPost("update")]
        public async Task<IActionResult> Update(Blog blog, IFormFile? avatar)
        {
            try
            {
                var existing = await _blogService.GetByIdAsync(blog.BlogID);
                if (existing == null)
                {
                    TempData["Error"] = "Blog not found!";
                    return RedirectToAction("Index");
                }

                existing.Title = blog.Title;
                existing.ShortDescription = blog.ShortDescription;
                existing.CategoryID = blog.CategoryID;
                existing.UpdatedAt = DateTime.Now;

                if (avatar != null && avatar.Length > 0)
                {
                    var oldImage = existing.BlogImages.FirstOrDefault(i => i.IsMain == true);

                    if (oldImage != null)
                    {
                        // Xóa ảnh vật lý
                        var relativePath = oldImage.ImageURL.Replace('/', Path.DirectorySeparatorChar).TrimStart(Path.DirectorySeparatorChar);
                        var oldPath = Path.Combine(_env.WebRootPath, relativePath);

                        if (System.IO.File.Exists(oldPath))
                        {
                            System.IO.File.Delete(oldPath);
                        }

                        // Xóa khỏi DB
                        _blogImageRepo.Delete(oldImage);
                        await _blogImageRepo.SaveAsync();
                    }

                    // Lưu ảnh mới
                    var newImage = await SaveImageAsync(avatar);
                    if (newImage != null)
                    {
                        existing.BlogImages.Add(newImage);
                    }
                }

                await _blogService.UpdateAsync(existing);
                TempData["Success"] = "Blog updated successfully!";
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Failed to update blog: " + ex.Message;
            }

            return RedirectToAction("Index");
        }

        // POST: admin/blog/delete
        [HttpPost("delete")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _blogService.SoftDeleteAsync(id);
                TempData["Success"] = "Blog deleted.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Failed to delete blog: " + ex.Message;
            }

            return RedirectToAction("Index");
        }

        // POST: admin/blog/toggle-status
        [HttpPost("toggle-status")]
        public async Task<IActionResult> ToggleStatus(int id)
        {
            try
            {
                await _blogService.ToggleStatusAsync(id);
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }

        // 📁 Hàm xử lý lưu ảnh (DRY)
        private async Task<BlogImage?> SaveImageAsync(IFormFile avatar)
        {
            try
            {
                // Dùng tên gốc kèm mã GUID để tránh trùng
                string originalFileName = Path.GetFileNameWithoutExtension(avatar.FileName);
                string extension = Path.GetExtension(avatar.FileName);
                string fileName = $"{originalFileName}_{Guid.NewGuid()}{extension}";

                string folderPath = Path.Combine(_env.WebRootPath, "image", "blogs");
                string filePath = Path.Combine(folderPath, fileName);

                Directory.CreateDirectory(folderPath);

                // Nếu tồn tại file trùng, xóa trước khi lưu
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await avatar.CopyToAsync(stream);
                }

                return new BlogImage
                {
                    ImageURL = $"/image/blogs/{fileName}",
                    IsMain = true,
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
