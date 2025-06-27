using GoEASy.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class TestDbController : Controller
{
    private readonly GoEasyContext _context;

    public TestDbController(GoEasyContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        try
        {
            // Test connection cơ bản
            bool ok = _context.Database.CanConnect();
            
            if (ok)
            {
                var dbName = _context.Database.GetDbConnection().Database;
                var tourCount = _context.Tours.Count();
                var userCount = _context.Users.Count();
                var destinationCount = _context.Destinations.Count();
                var adminCount = _context.Admins.Count();
                
                return Content($"✅ Database connected!\n" +
                             $"📊 Database: {dbName}\n" +
                             $"🏖️ Tours: {tourCount}\n" +
                             $"👥 Users: {userCount}\n" +
                             $"🗺️ Destinations: {destinationCount}\n" +
                             $"👨‍💼 Admins: {adminCount}");
            }
            else
            {
                return Content("❌ Cannot connect to database.");
            }
        }
        catch (Exception ex)
        {
            return Content($"❌ Lỗi DB: {ex.Message}");
        }
    }
}
