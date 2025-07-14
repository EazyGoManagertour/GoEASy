using GoEASy.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GoEASy.Services
{
    public interface IBlogDetailService
    {
        Task<IEnumerable<BlogDetail>> GetAllAsync();
        Task<BlogDetail?> GetByIdAsync(int id);
        Task<BlogDetail?> GetByBlogIdAsync(int blogId);

        Task AddAsync(BlogDetail detail, BlogImage? mainImage, List<BlogImage> galleryImages, List<int> tagIds);
        Task UpdateAsync(BlogDetail detail, BlogImage? newMainImage, List<BlogImage> newGalleryImages, List<int> tagIds);
        Task DeleteAsync(int id);
        Task ToggleStatusAsync(int id);

        Task<BlogImage?> GetMainImageAsync(int blogId);
        Task<List<BlogImage>> GetGalleryImagesAsync(int blogId);
        Task<List<BlogTagMapping>> GetTagsAsync(int blogId);
    }
}
