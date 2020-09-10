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
    }
}
