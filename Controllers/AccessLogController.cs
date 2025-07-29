using Microsoft.AspNetCore.Mvc;
using GoEASy.Services;
using GoEASy.Models;

namespace GoEASy.Controllers
{
    public class AccessLogController : Controller
    {
        private readonly IAccessLogService _accessLogService;

        public AccessLogController(IAccessLogService accessLogService)
        {
            _accessLogService = accessLogService;
        }

        // GET: /AccessLog
        public IActionResult Index()
        {
            var logs = _accessLogService.GetAllLogs();
            return View(logs);
        }
    }
}
