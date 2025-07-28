using GoEASy.Models;

namespace GoEASy.Services
{
    public interface ISystemStatisticService
    {
        void GenerateStatistic(); // Gọi stored procedure
        SystemStatistic GetLatestSnapshot(); // Lấy thống kê mới nhất

        List<SystemStatistic> GetAllSnapshots();
    }
}