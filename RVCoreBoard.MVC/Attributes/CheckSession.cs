using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;

namespace RVCoreBoard.MVC.Attributes
{
    public class CheckSession : ActionFilterAttribute, IActionFilter
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var ctx = context.HttpContext;
            if (ctx.Session.GetInt32("USER_LOGIN_KEY") == null)
            {
                context.Result = new RedirectToRouteResult(new RouteValueDictionary(new { action = "Login", controller = "Account" }));
            }
            base.OnActionExecuting(context);
        }
    }
}
