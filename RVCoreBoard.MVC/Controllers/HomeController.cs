using System;
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

        /// <summary>
        /// 게시판 리스트
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public async Task<IActionResult> TotalSearch(string totalSearchString)
        {
            var containBoardList = await _db.Boards
                                            .Where(b => b.Title.Contains(totalSearchString) || b.Content.Contains(totalSearchString))
                                            .Include(b => b.category)
                                            .Take(5)
                                            .ToListAsync();

            var contatinCommentList = await _db.Comments
                                                .Where(c => c.Content.Contains(totalSearchString))
                                                .Include(c => c.board).ThenInclude(b => b.category)
                                                .Take(5)
                                                .ToListAsync();

            ViewBag.ContainBoardList = containBoardList;
            ViewBag.ContainCommentList = contatinCommentList;


            ViewBag.TotalSearchString = String.IsNullOrEmpty(totalSearchString) ? null : totalSearchString;

            return View();
        }

        [AllowAnonymous]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
