using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RVCoreBoard.MVC.Attributes;
using RVCoreBoard.MVC.DataContext;
using RVCoreBoard.MVC.Models;
using RVCoreBoard.MVC.Services;

namespace RVCoreBoard.MVC.Controllers
{
    public class BoardController : Controller
    {
        private readonly RVCoreBoardDBContext _db;
        private IBoardService _boardService;

        public BoardController(RVCoreBoardDBContext db, IBoardService boardService)
        {
            _db = db;
            _boardService = boardService;
        }

        /// <summary>
        /// 게시판 리스트
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Index(int? currentPage)
        {
            BoardListInfoModel boardListInfoModel = new BoardListInfoModel(_boardService);
            await boardListInfoModel.GetList(currentPage ?? 1);

            return View(boardListInfoModel);
        }

        /// <summary>
        /// 게시물 목록
        /// </summary>
        /// <param name="BNo"></param>
        /// <returns></returns>
        public async Task<IActionResult> Detail(int BNo)
        {
            Board board = new Board(_boardService);
            await board.GetDetail(BNo);

            return View(board.Data);
        }

        /// <summary>
        /// 게시물 추가
        /// </summary>
        /// <returns></returns>
        [CheckSession]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost, CheckSession]
        public IActionResult Add(Board model)
        {
            model.UNo = int.Parse(HttpContext.Session.GetInt32("USER_LOGIN_KEY").ToString());
            model.Reg_Date = DateTime.Now;
            model.Cnt_Read = 0;

            if (ModelState.IsValid)
            {
                _db.Boards.Add(model);
                if (_db.SaveChanges() > 0)
                {
                    return Redirect("Index");
                }

                ModelState.AddModelError(string.Empty, "게시물을 등록할 수 없습니다.");
            }
            return View(model);
        }

        /// <summary>
        /// 게시물 수정
        /// </summary>
        /// <returns></returns>
        [CheckSession]
        public IActionResult Edit(int BNo)
        {
            var Board = _db.Boards.FirstOrDefault(b => b.BNo.Equals(BNo));

            return View(Board);
        }

        [HttpPost, CheckSession]
        public IActionResult Edit(Board model)
        {
            model.UNo = int.Parse(HttpContext.Session.GetInt32("USER_LOGIN_KEY").ToString());

            if (ModelState.IsValid)
            {
                _db.Entry(model).State = EntityState.Modified;
                if (_db.SaveChanges() > 0)
                {
                    return Redirect($"Detail?BNo={model.BNo}");
                }
                ModelState.AddModelError(string.Empty, "게시물을 수정할 수 없습니다.");
            }
            return View(model);
        }

        /// <summary>
        /// 게시물 삭제 
        /// </summary>
        /// <returns></returns>
        public IActionResult Delete(int BNo)
        {
            var Board = _db.Boards.FirstOrDefault(b => b.BNo.Equals(BNo));

            _db.Boards.Remove(Board);
            if (_db.SaveChanges() > 0)
            {
                return Redirect("Index");
            }
            return Redirect($"Detail?BNo={BNo}");
        }
    }
}
