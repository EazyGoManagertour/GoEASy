using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using GoEASy.Models;

public class RequirePermissionAttribute : ActionFilterAttribute
{
    private readonly string _slug;

    public RequirePermissionAttribute(string slug)
    {
        _slug = slug;
    }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var httpContext = context.HttpContext;
        var session = httpContext.Session;
        var db = (GoEasyContext)httpContext.RequestServices.GetService(typeof(GoEasyContext));
        var adminId = session.GetInt32("AdminID");

        // ❌ Chưa login → redirect
        if (adminId == null)
        {
            context.Result = new RedirectToActionResult("Login", "Admin", null);
            return;
        }

        var admin = db.Admins.Include(a => a.Rule).FirstOrDefault(a => a.AdminID == adminId);
        if (admin == null || admin.Rule == null || string.IsNullOrEmpty(admin.Rule.ListRuleSlug))
        {
            HandleNoPermission(context, "Bạn không có quyền truy cập.");
            return;
        }

        var slugs = admin.Rule.ListRuleSlug.Split(',').Select(s => s.Trim()).ToList();
        if (!slugs.Contains(_slug))
        {
            // Phát hiện request là JS / API
            var isApiRequest = httpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest" ||
                               httpContext.Request.ContentType?.Contains("application/json") == true;

            if (isApiRequest || httpContext.Request.Path.StartsWithSegments("/api") || httpContext.Request.Method == "POST")
            {
                // Trả JSON lỗi quyền
                context.Result = new JsonResult(new { error = "Forbidden" })
                {
                    StatusCode = StatusCodes.Status403Forbidden
                };
            }
            else if (_slug.StartsWith("view-"))
            {
                context.Result = new ViewResult
                {
                    ViewName = "~/Views/admin/rules/404.cshtml"
                };
            }
            else
            {
                var controller = context.Controller as Controller;
                if (controller != null)
                {
                    controller.TempData["Error"] = "Bạn không có quyền thực hiện hành động này.";
                }

                context.Result = new RedirectToActionResult("Index", GetControllerName(context), null);
            }

            return;
        }

        base.OnActionExecuting(context);
    }

    private void HandleNoPermission(ActionExecutingContext context, string errorMessage)
    {
        var controller = context.Controller as Controller;
        if (controller != null)
        {
            controller.TempData["Error"] = errorMessage;
        }

        context.Result = new ViewResult
        {
            ViewName = "~/Views/admin/rules/404.cshtml"
        };
    }

    private string GetControllerName(ActionExecutingContext context)
    {
        return context.RouteData.Values["controller"]?.ToString() ?? "Home";
    }
}
