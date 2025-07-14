using GoEASy.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace GoEASy.Services
{
    public class BlogService : IBlogService
    {
        private readonly GoEasyContext _context;
        private readonly IWebHostEnvironment _env;

        public BlogService(GoEasyContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public async Task<IEnumerable<Blog>> GetAllAsync()
        {
            return await _context.Blogs
                .Include(b => b.BlogImages)
                .Include(b => b.AuthorAdmin)
                .Include(b => b.AuthorUser)
                .Include(b => b.BlogTagMappings).ThenInclude(m => m.Tag)
                .Include(b => b.Category)
                .Where(b => b.Status == true)
                .OrderByDescending(b => b.CreatedAt)
                .ToListAsync();
        }

        public async Task<Blog?> GetByIdAsync(int id)
        {
            return await _context.Blogs
                .Include(b => b.BlogDetail)
                .Include(b => b.BlogImages)
                .Include(b => b.AuthorAdmin)
                .Include(b => b.AuthorUser)
                .Include(b => b.Category)
                .FirstOrDefaultAsync(b => b.BlogId == id);
        }

        public async Task CreateAsync(Blog blog)
        {
            blog.IsApproved = 0;
            blog.Status = true;
            blog.CreatedAt = DateTime.Now;
            blog.UpdatedAt = DateTime.Now;

            await _context.Blogs.AddAsync(blog);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Blog blog)
        {
            blog.UpdatedAt = DateTime.Now;
            _context.Blogs.Update(blog);
            await _context.SaveChangesAsync();
        }

        public async Task SoftDeleteAsync(int id)
        {
            var blog = await _context.Blogs.FindAsync(id);
            if (blog != null)
            {
                blog.Status = false; // ✅ Chuyển sang Disabled
                blog.UpdatedAt = DateTime.Now;
                _context.Blogs.Update(blog);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Blog>> GetUnapprovedBlogsAsync()
        {
            return await _context.Blogs
                .Include(b => b.BlogImages)
                .Include(b => b.AuthorAdmin)
                .Include(b => b.AuthorUser)
                .Include(b => b.Category)
                .Where(b => b.IsApproved == 0 && (b.Status ?? true))
                .ToListAsync();
        }

        public async Task ApproveAsync(int id)
        {
            var blog = await _context.Blogs.FindAsync(id);
            if (blog != null)
            {
                blog.IsApproved = 1;
                blog.UpdatedAt = DateTime.Now;
                _context.Blogs.Update(blog);
                await _context.SaveChangesAsync();
            }
        }

        public async Task ToggleStatusAsync(int id)
        {
            var blog = await _context.Blogs.FindAsync(id);
            if (blog != null)
            {
                blog.Status = !(blog.Status ?? true);
                blog.UpdatedAt = DateTime.Now;
                _context.Blogs.Update(blog);
                await _context.SaveChangesAsync();
            }
        }

        public async Task RejectAsync(int id)
        {
            var blog = await _context.Blogs.FindAsync(id);
            if (blog != null)
            {
                blog.IsApproved = 2;
                blog.UpdatedAt = DateTime.Now;
                _context.Blogs.Update(blog);
                await _context.SaveChangesAsync();
            }
        }

    }
}
