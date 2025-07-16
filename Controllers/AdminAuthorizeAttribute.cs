using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace GoEASy.Filters
{
    public class AdminAuthorizeAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var session = context.HttpContext.Session;
            var isLoggedIn = session.GetInt32("AdminID") != null;

            if (!isLoggedIn)
            {
                context.Result = new RedirectToActionResult("Index", "LoginAdmin", null);
            }

            base.OnActionExecuting(context);
        }
    }
}