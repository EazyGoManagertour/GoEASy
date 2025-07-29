using GoEASy.Models;
using GoEASy.Services;
using System.Collections.Generic;
using System.Linq;

namespace GoEASy.Services
{
    public class AccessLogService : IAccessLogService
    {
        private readonly GoEasyContext _context;

        public AccessLogService(GoEasyContext context)
        {
            _context = context;
        }

        public void LogAccess(int? userId, string ip, string userAgent)
        {
            var recent = _context.AccessLogs
                .Where(l => l.IPAddress == ip)
                .OrderByDescending(l => l.CreatedAt)
                .FirstOrDefault();

            if (recent != null && recent.CreatedAt.HasValue &&
                (DateTime.Now - recent.CreatedAt.Value).TotalMinutes < 5)
            {
                // Đã log trong 2 phút qua => không log lại
                return;
            }

            var log = new AccessLog
            {
                UserID = userId,
                IPAddress = ip,
                UserAgent = userAgent,
                CreatedAt = DateTime.Now
            };

            _context.AccessLogs.Add(log);
            _context.SaveChanges();
        }

        public List<AccessLog> GetAllLogs()
        {
            return _context.AccessLogs
                .OrderByDescending(l => l.CreatedAt)
                .ToList();
        }
    }
}