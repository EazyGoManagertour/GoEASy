using GoEASy.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace GoEASy.Controllers
{
    [Route("notification")]
    public class NotificationController : Controller
    {
        private readonly GoEasyContext _context;
        public NotificationController(GoEasyContext context) { _context = context; }

        // SSE endpoint
        [HttpGet("stream")]
        public async Task Stream()
        {
            Response.ContentType = "text/event-stream";
            int? userId = HttpContext.Session.GetInt32("UserID");
            if (userId == null) return;

            var lastId = 0;
            while (!HttpContext.RequestAborted.IsCancellationRequested)
            {
                var notifications = _context.Notifications
                    .Where(n => n.UserId == userId && n.NotificationId > lastId)
                    .OrderBy(n => n.NotificationId)
                    .ToList();

                foreach (var n in notifications)
                {
                    await Response.WriteAsync($"data: {System.Text.Json.JsonSerializer.Serialize(n)}\n\n");
                    await Response.Body.FlushAsync();
                    lastId = n.NotificationId;
                }
                await Task.Delay(3000); // Poll mỗi 3s
            }
        }
    }
} 