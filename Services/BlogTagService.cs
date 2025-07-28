using GoEASy.Models;
using GoEASy.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GoEASy.Services
{
    public class BlogTagService : IBlogTagService
    {
        private readonly IGenericRepository<BlogTag> _repository;

        public BlogTagService(IGenericRepository<BlogTag> repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<BlogTag>> GetAllAsync() => await _repository.GetAllAsync();

        public async Task<BlogTag?> GetByIdAsync(int id) => await _repository.GetByIdAsync(id);

        public async Task CreateAsync(BlogTag tag)
        {
            await _repository.AddAsync(tag);
            await _repository.SaveAsync();
        }

        public async Task UpdateAsync(BlogTag tag)
        {
            _repository.Update(tag);
            await _repository.SaveAsync();
        }

        public async Task ToggleStatusAsync(int id)
        {
            var tag = await _repository.GetByIdAsync(id);
            if (tag != null)
            {
                tag.Status = !(tag.Status ?? false);
                _repository.Update(tag);
                await _repository.SaveAsync();
            }
        }
    }
}
