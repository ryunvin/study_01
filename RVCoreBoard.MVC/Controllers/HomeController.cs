using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RVCoreBoard.MVC.DataContext;
using RVCoreBoard.MVC.Models;

namespace RVCoreBoard.MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly RVCoreBoardDBContext _db;

        public HomeController(RVCoreBoardDBContext db)
        {
            _db = db;
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }


        [AllowAnonymous]
        public IActionResult MenuBar()
        {
            return ViewComponent("MenuBar");
        }


        [AllowAnonymous]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
