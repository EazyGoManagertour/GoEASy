using GoEASy.Filters;
using GoEASy.Models;
using GoEASy.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GoEASy.Controllers
{
    //[AdminAuthorize]
    [Route("admin/statistics")]
    public class StatisticsAdminController : Controller
    {
        private readonly ISystemStatisticService _statService;
        private readonly GoEasyContext _context;
        public StatisticsAdminController(ISystemStatisticService statService, GoEasyContext context)
        {
            _statService = statService;
            _context = context;
        }
        [HttpGet("")]
        public IActionResult Index()
        {
            _statService.GenerateStatistic(); // Gọi stored procedure để cập nhật
            var snapshot = _statService.GetLatestSnapshot(); // Lấy bản mới nhất

            return View("~/Views/admin/statistics/StatisticsAdmin.cshtml", snapshot);
        }

        [HttpGet("chart-data")]
        public IActionResult ChartData()
        {
            var stats = _statService.GetAllSnapshots()
                .Where(s => s.SnapshotAt.HasValue)
                .GroupBy(s => s.SnapshotAt.Value.Year)
                .Select(g => new {
                    Year = g.Key, // dạng số nguyên: 2020, 2021, ...
                    TotalVisits = g.Sum(x => x.TotalVisits),
                    TotalUsers = g.Sum(x => x.TotalUsers),
                    TotalBookings = g.Sum(x => x.TotalBookings)
                })
                .OrderBy(x => x.Year)
                .ToList();

            return Json(stats);
        }

        [HttpGet("device-bounce")]
        public IActionResult DeviceBounceAnalytics()
        {
            var snapshot = _statService.GetLatestSnapshot();
            if (snapshot == null) return Json(new List<object>());

            var total = snapshot.TotalVisits == 0 ? 1 : snapshot.TotalVisits;

            var result = new List<object>
    {
        new {
            Device = "Mobile",
            PageViews = snapshot.DeviceMobile,
            BounceRate = (int)Math.Round((double)snapshot.DeviceMobile * 100 / total)
        },
        new {
            Device = "Desktop",
            PageViews = snapshot.DeviceDesktop,
            BounceRate = (int)Math.Round((double)snapshot.DeviceDesktop * 100 / total)
        },
        new {
            Device = "Tablet",
            PageViews = snapshot.DeviceTablet,
            BounceRate = (int)Math.Round((double)snapshot.DeviceTablet * 100 / total)
        }
    };

            return Json(result);
        }

        [HttpGet("chart-revenue")]
        public IActionResult ChartDataRevenue()
        {
            var stats = _statService.GetAllSnapshots();

            var result = stats
                .Where(s => s.SnapshotAt.HasValue)
                .GroupBy(s => new { s.SnapshotAt.Value.Year, s.SnapshotAt.Value.Month })
                .OrderBy(g => g.Key.Year).ThenBy(g => g.Key.Month)
                .Select(g =>
                {
                    // lấy bản ghi có SnapshotAt lớn nhất trong tháng
                    var latest = g.OrderByDescending(x => x.SnapshotAt).First();
                    return new
                    {
                        snapshotAt = new DateTime(g.Key.Year, g.Key.Month, 1),
                        totalRevenue = latest.TotalRevenue
                    };
                });

            return Json(result);
        }




    }
}
