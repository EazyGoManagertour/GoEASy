using GoEASy.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GoEASy.Controllers
{
    public class TestDbController : Controller
    {
        private readonly GoEasyContext _context;

        public TestDbController(GoEasyContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> TestRoleTable()
        {
            var roles = await _context.Roles.ToListAsync();
            return Content($"Số lượng roles: {roles.Count}");
        }
    }
}
