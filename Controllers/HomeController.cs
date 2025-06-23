using System.Diagnostics;
using GoEASy.Models;
using Microsoft.AspNetCore.Mvc;

namespace GoEASy.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View("~/Views/client/Home.cshtml");
        }
    }
}
