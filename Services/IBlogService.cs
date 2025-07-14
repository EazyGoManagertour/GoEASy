using GoEASy.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GoEASy.Services
{
    public interface IBlogService
    {
        Task<IEnumerable<Blog>> GetAllAsync();
        Task<Blog?> GetByIdAsync(int id);
        Task CreateAsync(Blog blog);
        Task UpdateAsync(Blog blog);
        Task SoftDeleteAsync(int id);
        Task<IEnumerable<Blog>> GetUnapprovedBlogsAsync();
        Task ApproveAsync(int id);
        Task RejectAsync(int id);
        Task ToggleStatusAsync(int id);
    }
}
