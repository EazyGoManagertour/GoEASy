using GoEASy.Models;
using GoEASy.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GoEASy.Services
{
    public class AdminService : IAdminService
    {
        private readonly IGenericRepository<Admin> _adminRepo;

        public AdminService(IGenericRepository<Admin> adminRepo)
        {
            _adminRepo = adminRepo;
        }

        public Task<IEnumerable<Admin>> GetAllAdminsAsync()
        {
            return _adminRepo.GetAllAsync();
        }

        public Task<Admin> GetAdminByIdAsync(int id)
        {
            return _adminRepo.GetByIdAsync(id);
        }

        public async Task CreateAdminAsync(Admin admin)
        {
            await _adminRepo.AddAsync(admin);
            await _adminRepo.SaveAsync();
        }

        public async Task UpdateAdminAsync(Admin admin)
        {
            _adminRepo.Update(admin);
            await _adminRepo.SaveAsync();
        }

        public async Task DeleteAdminAsync(int id)
        {
            var admin = await _adminRepo.GetByIdAsync(id);
            if (admin != null)
            {
                _adminRepo.Delete(admin);
                await _adminRepo.SaveAsync();
            }
        }
    }
}
