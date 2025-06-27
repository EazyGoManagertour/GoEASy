using GoEASy.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

public class TourController : Controller
{
    private readonly TourService _tourService;
    private readonly GoEasyContext _context;

    public TourController(TourService tourService, GoEasyContext context)
    {
        _tourService = tourService;
        _context = context;
    }

    // GET: /Tour
    public async Task<IActionResult> Index()
    {
        var tours = await _tourService.GetAllToursAsync();
        return View("~/Views/client/tour-list.cshtml", tours);
    }

    // GET: /Tour/SeedData - Action để tạo dữ liệu mẫu
    public async Task<IActionResult> SeedData()
    {
        // Kiểm tra xem đã có dữ liệu chưa
        if (await _context.Tours.AnyAsync())
        {
            return RedirectToAction(nameof(Index));
        }

        // Tạo destinations mẫu
        var destinations = new List<Destination>
        {
            new Destination { DestinationName = "Phú Quốc, Việt Nam", Description = "Đảo ngọc với những bãi biển đẹp" },
            new Destination { DestinationName = "Hạ Long, Việt Nam", Description = "Vịnh Hạ Long nổi tiếng thế giới" },
            new Destination { DestinationName = "Sapa, Việt Nam", Description = "Núi rừng Tây Bắc hùng vĩ" },
            new Destination { DestinationName = "Đà Nẵng, Việt Nam", Description = "Thành phố đáng sống nhất Việt Nam" },
            new Destination { DestinationName = "Nha Trang, Việt Nam", Description = "Thành phố biển xinh đẹp" }
        };

        _context.Destinations.AddRange(destinations);
        await _context.SaveChangesAsync();

        // Tạo tours mẫu
        var tours = new List<Tour>
        {
            new Tour
            {
                TourName = "Khám phá Phú Quốc 3 ngày 2 đêm",
                Description = "Hành trình khám phá đảo ngọc Phú Quốc với những bãi biển đẹp nhất, ẩm thực hải sản tươi ngon và các hoạt động giải trí thú vị.",
                AdultPrice = 2500000,
                ChildPrice = 1500000,
                StartDate = DateTime.Now.AddDays(30),
                EndDate = DateTime.Now.AddDays(33),
                MaxSeats = 20,
                AvailableSeats = 20,
                DestinationId = destinations[0].DestinationId
            },
            new Tour
            {
                TourName = "Du thuyền Vịnh Hạ Long 2 ngày 1 đêm",
                Description = "Trải nghiệm du thuyền sang trọng trên Vịnh Hạ Long, khám phá các hang động kỳ thú và thưởng thức ẩm thực đặc sản.",
                AdultPrice = 1800000,
                ChildPrice = 1200000,
                StartDate = DateTime.Now.AddDays(15),
                EndDate = DateTime.Now.AddDays(17),
                MaxSeats = 15,
                AvailableSeats = 15,
                DestinationId = destinations[1].DestinationId
            },
            new Tour
            {
                TourName = "Trekking Sapa 4 ngày 3 đêm",
                Description = "Hành trình trekking khám phá núi rừng Sapa, gặp gỡ người dân tộc thiểu số và ngắm cảnh ruộng bậc thang tuyệt đẹp.",
                AdultPrice = 3200000,
                ChildPrice = 2000000,
                StartDate = DateTime.Now.AddDays(45),
                EndDate = DateTime.Now.AddDays(49),
                MaxSeats = 12,
                AvailableSeats = 12,
                DestinationId = destinations[2].DestinationId
            },
            new Tour
            {
                TourName = "Đà Nẵng - Hội An 3 ngày 2 đêm",
                Description = "Khám phá thành phố Đà Nẵng hiện đại và phố cổ Hội An cổ kính, thưởng thức ẩm thực đặc sản miền Trung.",
                AdultPrice = 2200000,
                ChildPrice = 1400000,
                StartDate = DateTime.Now.AddDays(20),
                EndDate = DateTime.Now.AddDays(23),
                MaxSeats = 18,
                AvailableSeats = 18,
                DestinationId = destinations[3].DestinationId
            },
            new Tour
            {
                TourName = "Nha Trang - Đảo Khỉ 2 ngày 1 đêm",
                Description = "Tham quan thành phố Nha Trang, khám phá đảo Khỉ và thưởng thức các món hải sản tươi ngon.",
                AdultPrice = 1600000,
                ChildPrice = 1000000,
                StartDate = DateTime.Now.AddDays(25),
                EndDate = DateTime.Now.AddDays(27),
                MaxSeats = 16,
                AvailableSeats = 16,
                DestinationId = destinations[4].DestinationId
            }
        };

        _context.Tours.AddRange(tours);
        await _context.SaveChangesAsync();

        // Tạo tour images mẫu
        var tourImages = new List<TourImage>
        {
            new TourImage { TourId = tours[0].TourId, ImageUrl = "assets/images/destinations/tour-list1.jpg", IsCover = true },
            new TourImage { TourId = tours[1].TourId, ImageUrl = "assets/images/destinations/tour-list2.jpg", IsCover = true },
            new TourImage { TourId = tours[2].TourId, ImageUrl = "assets/images/destinations/tour-list3.jpg", IsCover = true },
            new TourImage { TourId = tours[3].TourId, ImageUrl = "assets/images/destinations/tour-list4.jpg", IsCover = true },
            new TourImage { TourId = tours[4].TourId, ImageUrl = "assets/images/destinations/tour-list5.jpg", IsCover = true }
        };

        _context.TourImages.AddRange(tourImages);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    // GET: /Tour/Details/5
    public async Task<IActionResult> Details(int id)
    {
        var tour = await _tourService.GetTourByIdAsync(id);
        if (tour == null) return NotFound();
        return View(tour);
    }

    // GET: /Tour/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: /Tour/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Tour tour)
    {
        if (ModelState.IsValid)
        {
            await _tourService.AddTourAsync(tour);
            return RedirectToAction(nameof(Index));
        }
        return View(tour);
    }

    // GET: /Tour/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        var tour = await _tourService.GetTourByIdAsync(id);
        if (tour == null) return NotFound();
        return View(tour);
    }

    // POST: /Tour/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Tour tour)
    {
        if (id != tour.TourId) return NotFound();
        if (ModelState.IsValid)
        {
            await _tourService.UpdateTourAsync(tour);
            return RedirectToAction(nameof(Index));
        }
        return View(tour);
    }

    // GET: /Tour/Delete/5
    public async Task<IActionResult> Delete(int id)
    {
        var tour = await _tourService.GetTourByIdAsync(id);
        if (tour == null) return NotFound();
        return View(tour);
    }

    // POST: /Tour/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _tourService.DeleteTourAsync(id);
        return RedirectToAction(nameof(Index));
    }
}