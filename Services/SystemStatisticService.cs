using GoEASy.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace GoEASy.Services
{
    public class SystemStatisticService : ISystemStatisticService
    {
        private readonly GoEasyContext _context;

        public SystemStatisticService(GoEasyContext context)
        {
            _context = context;
        }

        public void GenerateStatistic()
        {
            _context.Database.ExecuteSqlRaw("EXEC GenerateSystemStatistics");
        }

        public SystemStatistic GetLatestSnapshot()
        {
            return _context.SystemStatistics
                .OrderByDescending(s => s.SnapshotAt)
                .FirstOrDefault() ?? new SystemStatistic();
        }
        public List<SystemStatistic> GetAllSnapshots()
        {
            return _context.SystemStatistics
                          .OrderBy(s => s.SnapshotAt)
                          .ToList();
        }
    }
}