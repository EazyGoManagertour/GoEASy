using GoEASy.Services;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace GoEASy.Controllers
{
    [Route("blog")]
    public class ClientBlogController : Controller
    {
        private readonly IBlogService _blogService;
        private readonly ICategoryService _categoryService;
        public ClientBlogController(
            IBlogService blogService,
            ICategoryService categoryService)
        {
            _blogService = blogService;
            _categoryService = categoryService;
        }

        [HttpGet("")]
        public async Task<IActionResult> Index(string? keyword, string? category, int page = 1, int pageSize = 3)
        {
            var blogs = await _blogService.GetAllAsync();

            // ✅ Lọc dữ liệu
            blogs = blogs.Where(b => b.IsApproved == 1).ToList();

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                blogs = blogs.Where(b =>
                    (b.Title != null && b.Title.Contains(keyword, StringComparison.OrdinalIgnoreCase)) ||
                    (b.AuthorAdmin != null && b.AuthorAdmin.FullName.Contains(keyword, StringComparison.OrdinalIgnoreCase)) ||
                    (b.AuthorUser != null && b.AuthorUser.FullName.Contains(keyword, StringComparison.OrdinalIgnoreCase)) ||
                    (b.BlogTagMappings != null &&
                     b.BlogTagMappings.Any(m => m.Tag != null && m.Tag.Name.Contains(keyword, StringComparison.OrdinalIgnoreCase)))
                ).ToList();
            }

            if (!string.IsNullOrWhiteSpace(category))
            {
                blogs = blogs.Where(b => b.Category != null && b.Category.CategoryName == category).ToList();
            }

            ViewBag.Categories = await _categoryService.GetAllCategoriesAsync();
            ViewBag.RecentBlogs = blogs.OrderByDescending(b => b.CreatedAt).Take(3).ToList();

            // ✅ Tính phân trang
            int totalItems = blogs.Count();
            int totalPages = (int)Math.Ceiling((double)totalItems / pageSize);
            ViewBag.TotalPages = totalPages;
            ViewBag.CurrentPage = page;

            // ✅ Lấy blog theo trang
            var pagedBlogs = blogs
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return View("~/Views/client/blog/ListBlog.cshtml", pagedBlogs);
        }

    }

}
