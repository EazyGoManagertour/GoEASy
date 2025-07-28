using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GoEASy.Models;
using GoEASy.Services;
using GoEASy.Models;

namespace GoEASy.Controllers
{
    [Route("/tour-list")]
    public class TourListController : Controller
    {
        private readonly TourService _tourService;
        private readonly IFavoriteService _favoriteService;

        public TourListController(TourService tourService, IFavoriteService favoriteService)
        {
            _tourService = tourService;
            _favoriteService = favoriteService;
        }

        [Route("")]
        [Route("index")]
        public async Task<IActionResult> Index(
            int? destinationId = null,
            int? categoryId = null,
            DateTime? startDate = null,
            string? sortBy = null,
            string? sortOrder = "asc",
            int page = 1,
            int pageSize = 5)
        {
            try
            {
                // Debug logging
                Console.WriteLine($"Search params: destinationId={destinationId}, categoryId={categoryId}, startDate={startDate}, sortBy={sortBy}, page={page}, pageSize={pageSize}");

                // Lấy tất cả tour phù hợp filter
                var allTours = await _tourService.SearchToursAsync(
                    DestinationID: destinationId,
                    CategoryID: categoryId,
                    startDate: startDate,
                    sortBy: sortBy,
                    sortOrder: sortOrder
                );
                var totalTours = allTours.Count();
                var totalPages = (int)Math.Ceiling((double)totalTours / pageSize);
                var tours = allTours.Skip((page - 1) * pageSize).Take(pageSize).ToList();

                // Lấy danh sách destinations và categories cho filter dropdowns
                var destinations = await _tourService.GetAllDestinationsAsync();
                var categories = await _tourService.GetAllCategoriesAsync();

                // Lấy danh sách favorite tour id của user
                var userId = HttpContext.Session.GetInt32("UserID");
                List<int> favoriteTourIds = new List<int>();
                if (userId != null)
                {
                    favoriteTourIds = (await _favoriteService.GetFavoritesByUserIdAsync(userId.Value))
                        .Select(f => f.TourID)
                        .ToList();
                }
                ViewBag.FavoriteTourIds = favoriteTourIds;

                ViewBag.Destinations = destinations;
                ViewBag.Categories = categories;
                ViewBag.SelectedDestinationId = destinationId;
                ViewBag.SelectedCategoryId = categoryId;
                ViewBag.StartDate = startDate;
                ViewBag.SortBy = sortBy;
                ViewBag.SortOrder = sortOrder;
                ViewBag.TotalPages = totalPages;
                ViewBag.CurrentPage = page;
                ViewBag.PageSize = pageSize;
                ViewBag.TotalTours = totalTours;

                return View("~/Views/client/Tour-list.cshtml", tours);
            }
            catch (Exception ex)
            {
                // Debug logging
                Console.WriteLine($"Error in search: {ex.Message}");

                // Return empty lists if there's an error
                ViewBag.Destinations = new List<Destination>();
                ViewBag.Categories = new List<TourCategory>();

                return View("~/Views/client/Tour-list-new.cshtml", new List<Tour>());
            }
        }

        // API endpoint cho AJAX search (nếu cần)
        [HttpGet]
        [Route("search")]
        public async Task<IActionResult> Search(
            int? destinationId = null,
            int? categoryId = null,
            DateTime? startDate = null,
            string? sortBy = null,
            string? sortOrder = "asc")
        {
            try
            {
                var tours = await _tourService.SearchToursAsync(
                    DestinationID: destinationId,
                    CategoryID: categoryId,
                    startDate: startDate,
                    sortBy: sortBy,
                    sortOrder: sortOrder
                );

                return Json(new { success = true, tours = tours });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // Test action để kiểm tra dữ liệu
        [HttpGet]
        [Route("test")]
        public async Task<IActionResult> Test()
        {
            try
            {
                var allTours = await _tourService.GetAllToursAsync();
                var destinations = await _tourService.GetAllDestinationsAsync();
                var categories = await _tourService.GetAllCategoriesAsync();

                return Json(new
                {
                    success = true,
                    totalTours = allTours.Count(),
                    totalDestinations = destinations.Count(),
                    totalCategories = categories.Count(),
                    tours = allTours.Select(t => new {
                        id = t.TourID,
                        name = t.TourName,
                        destinationId = t.DestinationID,
                        categoryId = t.CategoryID,
                        startDate = t.StartDate,
                        adultPrice = t.AdultPrice
                    }).ToList()
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // Seed dữ liệu mẫu nếu cần
        [HttpGet]
        [Route("seed")]
        public async Task<IActionResult> SeedData()
        {
            try
            {
                var context = new GoEasyContext();

                // Kiểm tra xem đã có dữ liệu chưa
                if (await context.Tours.AnyAsync())
                {
                    return Json(new { success = true, message = "Data already exists" });
                }

                // Tạo categories mẫu
                var categories = new List<TourCategory>
                {
                    new TourCategory { CategoryName = "Du lịch biển", Description = "Các tour du lịch biển" },
                    new TourCategory { CategoryName = "Du lịch núi", Description = "Các tour du lịch núi" },
                    new TourCategory { CategoryName = "Du lịch văn hóa", Description = "Các tour du lịch văn hóa" }
                };

                context.TourCategories.AddRange(categories);
                await context.SaveChangesAsync();

                // Tạo destinations mẫu
                var destinations = new List<Destination>
                {
                    new Destination { DestinationName = "Phú Quốc, Việt Nam", Description = "Đảo ngọc với những bãi biển đẹp" },
                    new Destination { DestinationName = "Hạ Long, Việt Nam", Description = "Vịnh Hạ Long nổi tiếng thế giới" },
                    new Destination { DestinationName = "Sapa, Việt Nam", Description = "Núi rừng Tây Bắc hùng vĩ" },
                    new Destination { DestinationName = "Đà Nẵng, Việt Nam", Description = "Thành phố đáng sống nhất Việt Nam" },
                    new Destination { DestinationName = "Nha Trang, Việt Nam", Description = "Thành phố biển xinh đẹp" }
                };

                context.Destinations.AddRange(destinations);
                await context.SaveChangesAsync();

                // Tạo tours mẫu
                var tours = new List<Tour>
                {
                    new Tour
                    {
                        TourName = "Khám phá Phú Quốc 3 ngày 2 đêm",
                        Description = "Hành trình khám phá đảo ngọc Phú Quốc với những bãi biển đẹp nhất",
                        AdultPrice = 2500000,
                        ChildPrice = 1500000,
                        StartDate = DateTime.Now.AddDays(30),
                        EndDate = DateTime.Now.AddDays(33),
                        MaxSeats = 20,
                        AvailableSeats = 20,
                        DestinationID = destinations[0].DestinationID,
                        CategoryID = categories[0].CategoryID
                    },
                    new Tour
                    {
                        TourName = "Du thuyền Vịnh Hạ Long 2 ngày 1 đêm",
                        Description = "Trải nghiệm du thuyền sang trọng trên Vịnh Hạ Long",
                        AdultPrice = 1800000,
                        ChildPrice = 1200000,
                        StartDate = DateTime.Now.AddDays(15),
                        EndDate = DateTime.Now.AddDays(17),
                        MaxSeats = 15,
                        AvailableSeats = 15,
                        DestinationID = destinations[1].DestinationID,
                        CategoryID = categories[0].CategoryID
                    },
                    new Tour
                    {
                        TourName = "Trekking Sapa 4 ngày 3 đêm",
                        Description = "Hành trình trekking khám phá núi rừng Sapa",
                        AdultPrice = 3200000,
                        ChildPrice = 2000000,
                        StartDate = DateTime.Now.AddDays(45),
                        EndDate = DateTime.Now.AddDays(49),
                        MaxSeats = 12,
                        AvailableSeats = 12,
                        DestinationID = destinations[2].DestinationID,
                        CategoryID = categories[1].CategoryID
                    },
                    new Tour
                    {
                        TourName = "Đà Nẵng - Hội An 3 ngày 2 đêm",
                        Description = "Khám phá thành phố Đà Nẵng và phố cổ Hội An",
                        AdultPrice = 2200000,
                        ChildPrice = 1400000,
                        StartDate = DateTime.Now.AddDays(20),
                        EndDate = DateTime.Now.AddDays(23),
                        MaxSeats = 18,
                        AvailableSeats = 18,
                        DestinationID = destinations[3].DestinationID,
                        CategoryID = categories[2].CategoryID
                    },
                    new Tour
                    {
                        TourName = "Nha Trang - Đảo Khỉ 2 ngày 1 đêm",
                        Description = "Tham quan thành phố Nha Trang và đảo Khỉ",
                        AdultPrice = 1600000,
                        ChildPrice = 1000000,
                        StartDate = DateTime.Now.AddDays(25),
                        EndDate = DateTime.Now.AddDays(27),
                        MaxSeats = 16,
                        AvailableSeats = 16,
                        DestinationID = destinations[4].DestinationID,
                        CategoryID = categories[0].CategoryID
                    }
                };

                context.Tours.AddRange(tours);
                await context.SaveChangesAsync();

                return Json(new { success = true, message = "Data seeded successfully" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
}
