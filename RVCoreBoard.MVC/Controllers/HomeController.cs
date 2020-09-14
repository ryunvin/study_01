using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RVCoreBoard.MVC.DataContext;
using RVCoreBoard.MVC.Models;
using RVCoreBoard.MVC.Services;

namespace RVCoreBoard.MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly RVCoreBoardDBContext _db;
        private IBoardService _boardService;

        public HomeController(RVCoreBoardDBContext db, IBoardService boardService)
        {
            _db = db;
            _boardService = boardService;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var boardListInfoModel = new BoardListInfoModel(_boardService);
            await boardListInfoModel.GetRecentBoards();

            var commentList = new Comment(_boardService);
            await commentList.GetRecentComments();

            ViewBag.BoardList = boardListInfoModel.Data;
            ViewBag.CommentList = commentList.Data;

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
