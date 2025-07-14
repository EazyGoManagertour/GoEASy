using GoEASy.Models;
using GoEASy.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoEASy.Services
{
    public class BlogDetailService : IBlogDetailService
    {
        private readonly IGenericRepository<BlogDetail> _detailRepo;
        private readonly IGenericRepository<BlogImage> _imageRepo;
        private readonly IGenericRepository<BlogTagMapping> _tagMapRepo;
        private readonly GoEasyContext _context;
        public BlogDetailService(
             IGenericRepository<BlogDetail> detailRepo,
    IGenericRepository<BlogImage> imageRepo,
    IGenericRepository<BlogTagMapping> tagMapRepo,
    GoEasyContext context)
        {
            _detailRepo = detailRepo;
            _imageRepo = imageRepo;
            _tagMapRepo = tagMapRepo;
            _context = context;
        }

        public async Task<IEnumerable<BlogDetail>> GetAllAsync()
        {
            var all = await _detailRepo.GetAllAsync();

            // Load toàn bộ Blog và BlogImages
            foreach (var d in all)
            {
                d.Blog = await _context.Blogs
                    .Include(b => b.BlogImages)
                    .Include(b => b.BlogTagMappings)
                        .ThenInclude(m => m.Tag)
                    .FirstOrDefaultAsync(b => b.BlogId == d.BlogId);
            }

            return all.Where(d => d.Status == true);
        }

        public async Task<BlogDetail?> GetByIdAsync(int id)
        {
            var detail = await _detailRepo.GetByIdAsync(id);
            return detail?.Status == true ? detail : null;
        }

        public async Task<BlogDetail?> GetByBlogIdAsync(int blogId)
        {
            var all = await _detailRepo.GetAllAsync();
            return all.FirstOrDefault(d => d.BlogId == blogId && d.Status == true);
        }

        public async Task AddAsync(BlogDetail detail, BlogImage? mainImage, List<BlogImage> galleryImages, List<int> tagIds)
        {
            await _detailRepo.AddAsync(detail);
            await _detailRepo.SaveAsync();

            int blogId = detail.BlogId;

            if (mainImage != null)
            {
                mainImage.BlogId = blogId;
                mainImage.Type = "main";
                await _imageRepo.AddAsync(mainImage);
            }

            foreach (var img in galleryImages)
            {
                img.BlogId = blogId;
                img.Type = "gallery";
                await _imageRepo.AddAsync(img);
            }

            foreach (var tagId in tagIds)
            {
                var map = new BlogTagMapping
                {
                    BlogId = blogId,
                    TagId = tagId
                };
                await _tagMapRepo.AddAsync(map);
            }

            await _imageRepo.SaveAsync();
            await _tagMapRepo.SaveAsync();
        }

        public async Task UpdateAsync(
    BlogDetail detail,
    BlogImage? newMainImage,
    List<BlogImage> newGalleryImages,
    List<int> tagIds)
        {
            var existing = await _detailRepo.GetByIdAsync(detail.BlogDetailId);
            if (existing == null) return;

            existing.Introduction = detail.Introduction;
            existing.Section1Title = detail.Section1Title;
            existing.Section1Content = detail.Section1Content;
            existing.Section2Title = detail.Section2Title;
            existing.Section2Content = detail.Section2Content;
            existing.Quote = detail.Quote;
            existing.QuoteAuthor = detail.QuoteAuthor;

            _detailRepo.Update(existing);
            await _detailRepo.SaveAsync();

            int blogId = existing.BlogId;
            int blogDetailId = existing.BlogDetailId;

            // MAIN IMAGE
            if (newMainImage != null)
            {
                var oldMain = await GetMainImageAsync(blogId);
                if (oldMain != null) _imageRepo.Delete(oldMain);

                newMainImage.BlogId = blogId;
                newMainImage.Type = "main";
                await _imageRepo.AddAsync(newMainImage);
            }

            // GALLERY IMAGES
            if (newGalleryImages != null && newGalleryImages.Count > 0)
            {
                var oldGallery = await GetGalleryImagesAsync(blogId);
                foreach (var img in oldGallery) _imageRepo.Delete(img);

                foreach (var img in newGalleryImages)
                {
                    img.BlogId = blogId;
                    img.Type = "gallery";
                    await _imageRepo.AddAsync(img);
                }
            }

            // UPDATE TAG MAPPINGS (gắn với BlogDetailId)
            var oldMappings = (await _tagMapRepo.GetAllAsync())
                     .Where(m => m.BlogId == blogId).ToList();

            foreach (var m in oldMappings) _tagMapRepo.Delete(m);

            foreach (var tagId in tagIds)
            {
                await _tagMapRepo.AddAsync(new BlogTagMapping
                {
                    BlogId = blogId,
                    TagId = tagId
                });
            }

            await _imageRepo.SaveAsync();
            await _tagMapRepo.SaveAsync();
        }



        public async Task DeleteAsync(int id)
        {
            var detail = await _detailRepo.GetByIdAsync(id);
            if (detail == null) return;

            detail.Status = false;
            _detailRepo.Update(detail);
            await _detailRepo.SaveAsync();
        }

        public async Task ToggleStatusAsync(int id)
        {
            var detail = await _detailRepo.GetByIdAsync(id);
            if (detail != null)
            {
                detail.Status = !(detail.Status ?? true);
                _detailRepo.Update(detail);
                await _detailRepo.SaveAsync();
            }
        }

        public async Task<BlogImage?> GetMainImageAsync(int blogId)
        {
            var images = await _imageRepo.GetAllAsync();
            return images.FirstOrDefault(i => i.BlogId == blogId && i.Type == "main");
        }

        public async Task<List<BlogImage>> GetGalleryImagesAsync(int blogId)
        {
            var images = await _imageRepo.GetAllAsync();
            return images.Where(i => i.BlogId == blogId && i.Type == "gallery")
                         .OrderBy(i => i.Position ?? 0).ToList();
        }

        public async Task<List<BlogTagMapping>> GetTagsAsync(int blogId)
        {
            var all = await _tagMapRepo.GetAllAsync();
            return all.Where(m => m.BlogId == blogId).ToList();
        }
    }
}
