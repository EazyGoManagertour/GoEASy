using GoEASy.DBConnection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class TestDbController : Controller
{
    private readonly AppDbContext _context;

    public TestDbController(AppDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        try
        {
            bool ok = _context.Database.CanConnect();
            return Content(ok ? "✅ Database connected!" : "❌ Cannot connect to database.");
        }
        catch (Exception ex)
        {
            return Content("❌ Lỗi DB: " + ex.Message);
        }
    }



}
