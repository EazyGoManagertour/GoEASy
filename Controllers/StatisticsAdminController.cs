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
                .Take(6)                         // lấy 6 năm mới nhất
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
        public IActionResult ChartDataRevenue(int year)
        {
            var stats = _statService.GetAllSnapshots();

            // Lọc theo năm
            var filtered = stats
                .Where(s => s.SnapshotAt.HasValue && s.SnapshotAt.Value.Year == year)
                .GroupBy(s => s.SnapshotAt.Value.Month)
                .Select(g =>
                {
                    var latest = g.OrderByDescending(x => x.SnapshotAt).First();
                    return new
                    {
                        month = g.Key,
                        totalRevenue = latest.TotalRevenue
                    };
                })
                .ToList();

            // Bổ sung đủ 12 tháng nếu thiếu
            var fullYear = Enumerable.Range(1, 12)
                .Select(m =>
                {
                    var monthData = filtered.FirstOrDefault(x => x.month == m);
                    return new
                    {
                        snapshotAt = new DateTime(year, m, 1),
                        totalRevenue = monthData?.totalRevenue ?? 0
                    };
                });

            return Json(fullYear);
        }

        [HttpGet("available-years")]
        public IActionResult GetAvailableYears()
        {
            var stats = _statService.GetAllSnapshots();

            var years = stats
                .Where(s => s.SnapshotAt.HasValue)
                .Select(s => s.SnapshotAt.Value.Year)
                .Distinct()
                .OrderByDescending(y => y)
                .ToList();

            return Json(years);
        }




    }
}
