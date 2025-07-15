using GoEASy.Filters;
using GoEASy.Models;
using GoEASy.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace GoEASy.Controllers
{
    [AdminAuthorize]
    [Route("admin/content-approval")]
    public class ContentApprovalController : Controller
    {
        private readonly IBlogService _blogService;
        private readonly GoEasyContext _context;

        public ContentApprovalController(IBlogService blogService, GoEasyContext context)
        {
            _blogService = blogService;
            _context = context;
        }

        // GET: admin/content-approval
        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            var pendingBlogs = await _blogService.GetUnapprovedBlogsAsync();
            return View("~/Views/admin/content_approval/ContentApproval.cshtml", pendingBlogs);
        }

        // POST: admin/content-approval/approve
        [HttpPost("approve")]
        public async Task<IActionResult> Approve(int id)
        {
            await _blogService.ApproveAsync(id);
            TempData["Success"] = "Blog approved successfully!";
            return RedirectToAction("Index");
        }

        // POST: admin/content-approval/reject
        [HttpPost("reject")]
        public async Task<IActionResult> Reject(int id)
        {
            await _blogService.RejectAsync(id); // thêm dòng này
            TempData["ToastReject"] = "Blog has been rejected!";
            return RedirectToAction("Index");
        }


        [HttpGet("get-blog-detail")]
        public async Task<IActionResult> GetBlogDetail(int blogId)
        {
            var blog = await _context.Blogs
                .Include(b => b.BlogDetail)
                .Include(b => b.BlogImages)
                .Include(b => b.AuthorAdmin)
                .Include(b => b.AuthorUser)
                .Include(b => b.BlogTagMappings)
                     .ThenInclude(m => m.Tag)
                .FirstOrDefaultAsync(b => b.BlogId == blogId);

            if (blog == null)
                return NotFound();

            return Json(new
            {
                blog.Title,
                blog.ShortDescription,
                author = blog.AuthorAdmin?.FullName ?? blog.AuthorUser?.FullName ?? "Unknown",
                introduction = blog.BlogDetail?.Introduction,
                section1Title = blog.BlogDetail?.Section1Title,
                section1Content = blog.BlogDetail?.Section1Content,
                section2Title = blog.BlogDetail?.Section2Title,
                section2Content = blog.BlogDetail?.Section2Content,
                quote = blog.BlogDetail?.Quote,
                quoteAuthor = blog.BlogDetail?.QuoteAuthor,
                mainImage = blog.BlogImages.FirstOrDefault(i => i.Type == "main")?.ImageUrl,
                gallery = blog.BlogImages.Where(i => i.Type == "gallery").Select(i => i.ImageUrl).ToList(),
                tags = blog.BlogTagMappings.Select(m => m.Tag?.Name).Where(t => t != null).ToList()
            });

        }
    }
}
