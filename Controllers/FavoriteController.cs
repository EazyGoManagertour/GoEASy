using Microsoft.AspNetCore.Mvc;
using GoEASy.Services;
using GoEASy.Models;

namespace GoEASy.Controllers
{
    [Route("favorite")]
    public class FavoriteController : Controller
    {
        private readonly IFavoriteService _favoriteService;

        public FavoriteController(IFavoriteService favoriteService)
        {
            _favoriteService = favoriteService;
        }

        // GET: /favorite
        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            // Lấy userId từ session
            var userId = HttpContext.Session.GetInt32("UserID");
            List<int> favoriteTourIds = new List<int>();
            if (userId != null)
            {
                // Giả sử bạn đã inject IFavoriteService vào controller này
                favoriteTourIds = (await _favoriteService.GetFavoritesByUserIdAsync(userId.Value))
                                    .Select(f => f.TourID)
                                    .ToList();
            }
            ViewBag.FavoriteTourIds = favoriteTourIds;
            
            if (userId == null)
            {
                // Nếu chưa đăng nhập, chuyển hướng về trang đăng nhập
                TempData["Error"] = "Vui lòng đăng nhập để xem danh sách yêu thích";
                return RedirectToAction("Index", "Login");
            }

            // Lấy danh sách favorite của user
            var favorites = await _favoriteService.GetFavoritesByUserIdAsync(userId.Value);
            
            return View("~/Views/client/favorite/favorite.cshtml", favorites);
        }

        // POST: /favorite/remove
        [HttpPost("remove")]
        public async Task<IActionResult> Remove(int tourId)
        {
            var userId = HttpContext.Session.GetInt32("UserID");
            
            if (userId == null)
            {
                return Json(new { success = false, message = "Vui lòng đăng nhập" });
            }

            var result = await _favoriteService.RemoveFavoriteAsync(userId.Value, tourId);
            
            if (result)
            {
                return Json(new { success = true, message = "Đã xóa khỏi danh sách yêu thích" });
            }
            else
            {
                return Json(new { success = false, message = "Không tìm thấy tour yêu thích" });
            }
        }
        [HttpPost("add")]
        public async Task<IActionResult> Add(int tourId)
        {
            var userId = HttpContext.Session.GetInt32("UserID");
            if (userId == null)
                return Json(new { success = false, message = "Vui lòng đăng nhập" });

            var result = await _favoriteService.AddFavoriteAsync(userId.Value, tourId);
            if (result)
                return Json(new { success = true, message = "Đã thêm vào danh sách yêu thích" });
            else
                return Json(new { success = false, message = "Đã có trong danh sách yêu thích" });
        }
    }
}