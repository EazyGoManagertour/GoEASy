using Microsoft.AspNetCore.Mvc;
using GoEASy.Services;
using System.Threading.Tasks;

namespace GoEASy.Controllers
{
    [Route("blog-detail/{id}")]
    public class ClientBlogDetailController : Controller
    {
        private readonly IBlogService _blogService;
        private readonly ICategoryService _categoryService;

        public ClientBlogDetailController(
            IBlogService blogService,
            ICategoryService categoryService)
        {
            _blogService = blogService;
            _categoryService = categoryService;
        }


        public async Task<IActionResult> Index(int id, string? keyword, string? category)
        {
            // ✅ Nếu có Search hoặc Category → Chuyển sang trang blog
            if (!string.IsNullOrWhiteSpace(keyword) || !string.IsNullOrWhiteSpace(category))
            {
                var query = new List<string>();
                if (!string.IsNullOrWhiteSpace(keyword))
                    query.Add($"keyword={Uri.EscapeDataString(keyword)}");
                if (!string.IsNullOrWhiteSpace(category))
                    query.Add($"category={Uri.EscapeDataString(category)}");

                var queryString = string.Join("&", query);
                return Redirect($"/blog?{queryString}");
            }

            // ✅ Nếu không tìm kiếm thì vẫn hiển thị BlogDetail như cũ
            var blog = await _blogService.GetByIdAsync(id);
            if (blog == null || blog.IsApproved != 1)
            {
                return NotFound();
            }

            var blogs = await _blogService.GetAllAsync();
            blogs = blogs.Where(b => b.IsApproved == 1).ToList();

            ViewBag.Categories = await _categoryService.GetAllCategoriesAsync();
            ViewBag.RecentBlogs = blogs.OrderByDescending(b => b.CreatedAt).Take(3).ToList();

            return View("~/Views/client/blog/ClientBlogDetail.cshtml", blog);
        }
    }
}
