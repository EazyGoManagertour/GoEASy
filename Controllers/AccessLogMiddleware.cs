using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Threading.Tasks;
using GoEASy.Services;

public class AccessLogMiddleware
{
    private readonly RequestDelegate _next;

    public AccessLogMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, IAccessLogService accessLogService)
    {
        // Chỉ log nếu là request chính (loại trừ file tĩnh)
        var path = context.Request.Path.Value?.ToLower();
        if (path != "/" && path != "/home" && path != "/home/index" && path != "/")
        {
            await _next(context);
            return;
        }

        // Lấy thông tin người dùng nếu đã đăng nhập
        int? userId = null;
        if (context.User.Identity?.IsAuthenticated == true)
        {
            var userIdClaim = context.User.Claims.FirstOrDefault(c => c.Type == "UserID");
            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int parsedId))
            {
                userId = parsedId;
            }
        }

        // Chuẩn hóa IP
        var ipAddress = context.Connection.RemoteIpAddress?.MapToIPv4().ToString() ?? "unknown";

        // User Agent
        var userAgent = context.Request.Headers["User-Agent"].ToString();

        // Ghi log
        accessLogService.LogAccess(userId, ipAddress, userAgent);

        await _next(context);
    }
}
