using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RVCoreBoard.MVC.Controllers
{
    public class ErrorController : Controller
    {
        [Authorize]
        public IActionResult AccessDenied()
        {
            return View();
        }

        [AllowAnonymous]
        [Route("Error/404")]
        public IActionResult PageNotFound()
        {
            return View();
        }

        [AllowAnonymous]
        [Route("Error/500")]
        public IActionResult IntenalServerError()
        {
            return View();
        }
    }
}
