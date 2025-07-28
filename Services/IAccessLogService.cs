using GoEASy.Models;
using System.Collections.Generic;

namespace GoEASy.Services
{
    public interface IAccessLogService
    {
        void LogAccess(int? userId, string ip, string userAgent);
        List<AccessLog> GetAllLogs();


    }
}