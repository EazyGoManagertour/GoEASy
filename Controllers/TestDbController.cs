<<<<<<< HEAD
﻿using GoEASy.Models;
=======
﻿using GoEASy.DBConnection;
>>>>>>> 33b4a60fd6af242bb2c1562a2cbee06a7b57d2bb
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class TestDbController : Controller
{
<<<<<<< HEAD
    private readonly GoEasyContext _context;

    public TestDbController(GoEasyContext context)
=======
    private readonly AppDbContext _context;

    public TestDbController(AppDbContext context)
>>>>>>> 33b4a60fd6af242bb2c1562a2cbee06a7b57d2bb
    {
        _context = context;
    }

    public IActionResult Index()
    {
        try
        {
<<<<<<< HEAD
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
=======
            bool ok = _context.Database.CanConnect();
            return Content(ok ? "✅ Database connected!" : "❌ Cannot connect to database.");
        }
        catch (Exception ex)
        {
            return Content("❌ Lỗi DB: " + ex.Message);
        }
    }



>>>>>>> 33b4a60fd6af242bb2c1562a2cbee06a7b57d2bb
}
