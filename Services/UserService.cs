using GoEASy.Models;
using GoEASy.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace GoEASy.Services
{
    public class UserService : IUserService
    {
        private readonly IGenericRepository<User> _userRepo;
        private readonly IGenericRepository<Role> _roleRepo;
        private readonly GoEasyContext _context;

        public UserService(IGenericRepository<User> userRepo, IGenericRepository<Role> roleRepo, GoEasyContext context)
        {
            _userRepo = userRepo;
            _roleRepo = roleRepo;
            _context = context;
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _context.Users.Include(u => u.Role).OrderByDescending(u => u.CreatedAt).ToListAsync();
        }

        public Task<User> GetUserByIdAsync(int id)
        {
            return _context.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.UserId == id);
        }

        public async Task CreateUserAsync(User user)
        {
            // Set default values (CreatedAt đã được set trong Controller)
            if (user.UpdatedAt == null)
                user.UpdatedAt = System.DateTime.Now;
            if (user.Status == null)
                user.Status = true;
            if (user.Vippoints == null)
                user.Vippoints = 0;
            if (user.IsVip == null)
                user.IsVip = false;

            await _userRepo.AddAsync(user);
            await _userRepo.SaveAsync();
        }

        public async Task UpdateUserAsync(User user)
        {
            _userRepo.Update(user);
            await _userRepo.SaveAsync();
        }

        public async Task DeleteUserAsync(int id)
        {
            var user = await _userRepo.GetByIdAsync(id);
            if (user != null)
            {
                _userRepo.Delete(user);
                await _userRepo.SaveAsync();
            }
        }

        public Task<IEnumerable<Role>> GetAllRolesAsync()
        {
            return _roleRepo.GetAllAsync();
        }

        public Task<bool> UsernameExistsAsync(string username, int? excludeId = null)
        {
            return _context.Users.AnyAsync(u => u.Username == username && (!excludeId.HasValue || u.UserId != excludeId));
        }

        public Task<bool> EmailExistsAsync(string email, int? excludeId = null)
        {
            return _context.Users.AnyAsync(u => u.Email == email && (!excludeId.HasValue || u.UserId != excludeId));
        }

        public Task<User> GetUserByUsernameAsync(string username)
        {
            return _context.Users.FirstOrDefaultAsync(u => u.Username == username);
        }

        public Task<User> GetUserByEmailAsync(string email)
        {
            return _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task ToggleStatusAsync(int id)
        {
            var user = await _userRepo.GetByIdAsync(id);
            if (user != null)
            {
                user.Status = !(user.Status ?? false);
                user.UpdatedAt = System.DateTime.Now;
                _userRepo.Update(user);
                await _userRepo.SaveAsync();
            }
        }
    }
} 