using GoEASy.Models;
using GoEASy.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GoEASy.Services
{
    public class CategoryService : GenericRepository<TourCategory>, ICategoryService
    {
        private readonly GoEasyContext _context;

        public CategoryService(GoEasyContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<TourCategory>> GetAllCategoriesAsync()
        {
            return await _context.TourCategories.ToListAsync();
        }

        public async Task<List<TourCategory>> GetPagedCategoriesAsync(int page, int pageSize)
        {
            return await _context.TourCategories
                .OrderByDescending(c => c.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<int> GetTotalPagesAsync(int pageSize)
        {
            var totalCategories = await _context.TourCategories.CountAsync();
            return (int)Math.Ceiling((double)totalCategories / pageSize);
        }

        public async Task<TourCategory?> GetCategoryByIdAsync(int id)
        {
            return await _context.TourCategories.FindAsync(id);
        }

        public async Task CreateCategoryAsync(TourCategory category)
        {
            await _context.TourCategories.AddAsync(category);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateCategoryAsync(TourCategory category)
        {
            _context.TourCategories.Update(category);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCategoryAsync(TourCategory category)
        {
            _context.TourCategories.Remove(category);
            await _context.SaveChangesAsync();
        }

        public async Task ToggleStatusAsync(int id)
        {
            var category = await _context.TourCategories.FindAsync(id);
            if (category != null)
            {
                category.Status = !(category.Status ?? true);
                category.UpdatedAt = DateTime.Now;
                _context.TourCategories.Update(category);
                await _context.SaveChangesAsync();
            }
        }
    }
}
