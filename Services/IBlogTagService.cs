using GoEASy.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GoEASy.Services
{
    public interface IBlogTagService
    {
        Task<IEnumerable<BlogTag>> GetAllAsync();
        Task<BlogTag?> GetByIdAsync(int id);
        Task CreateAsync(BlogTag tag);
        Task UpdateAsync(BlogTag tag);
        Task ToggleStatusAsync(int id);
    }
}
