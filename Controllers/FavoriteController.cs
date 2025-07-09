using Microsoft.AspNetCore.Mvc;
using GoEASy.Services;
using System.Threading.Tasks;

namespace GoEASy.Controllers
{
    public class FavoriteController : Controller
    {
        private readonly IFavoriteService _favoriteService;
        public FavoriteController(IFavoriteService favoriteService)
        {
            _favoriteService = favoriteService;
        }

        public async Task<IActionResult> Index()
        {
            var favorites = await _favoriteService.GetAllFavoritesAsync();
            return View("~/Views/client/favorite.cshtml", favorites);
        }
    }
} 