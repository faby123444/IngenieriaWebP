using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace IngenieriaWebP.Filters
{
    public class RequireLoginAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var isLoggedIn = context.HttpContext.Session.GetString("IsLoggedIn");

            if (string.IsNullOrEmpty(isLoggedIn))
            {
                context.Result = new RedirectToActionResult("Login", "Usuarios", null);
            }
        }
    }
}

