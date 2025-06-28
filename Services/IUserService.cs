using GoEASy.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GoEASy.Services
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User> GetUserByIdAsync(int id);
        Task<User> GetUserByUsernameAsync(string username);
        Task<User> GetUserByEmailAsync(string email);
        Task<bool> UsernameExistsAsync(string username, int? excludeId = null);
        Task<bool> EmailExistsAsync(string email, int? excludeId = null);
        Task AddUserAsync(User user);
        Task UpdateUserAsync(User user);
        Task DeleteUserAsync(int id);
        Task ToggleStatusAsync(int id);
        Task<IEnumerable<Role>> GetAllRolesAsync();
    }
} 