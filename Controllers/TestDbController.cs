using GoEASy.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GoEASy.Controllers
{
    [Route("testdb")]
    public class TestDbController : Controller
    {
        private readonly GoEasyContext _context;

        public TestDbController(GoEasyContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IActionResult> TestRoleTable()
        {
            try
            {
                // Test connection to the database
                await _context.Database.OpenConnectionAsync();
                await _context.Database.CloseConnectionAsync();

                return Content("success");
            }
            catch (Exception ex)
            {
                return Content($"error: {ex.Message}");
            }
        }
    }
}
