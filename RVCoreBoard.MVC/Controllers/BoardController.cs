using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RVCoreBoard.MVC.DataContext;
using RVCoreBoard.MVC.Models;
using RVCoreBoard.MVC.Services;
using RVCoreBoard.MVC.Helpers;
using static RVCoreBoard.MVC.Models.User;
using RVCoreBoard.MVC.Attributes;

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
        [AllowAnonymous]
        public async Task<IActionResult> Index(int Id, int? currentPage, string searchType, string searchString)
        {
            BoardListInfoModel boardListInfoModel = new BoardListInfoModel(_boardService);
            await boardListInfoModel.GetList(Id, currentPage ?? 1, searchType, searchString);

            Category category = await _db.Categorys.FirstOrDefaultAsync(c => c.Id == Id);
            ViewBag.Category = category;

            ViewBag.CurrentPage = currentPage ?? 1;
            ViewBag.SearchType = String.IsNullOrEmpty(searchType) ? null : searchType;
            ViewBag.SearchString = String.IsNullOrEmpty(searchString) ? null : searchString;

            return View(boardListInfoModel);
        }

        /// <summary>
        /// 게시물 상세보기
        /// </summary>
        /// <param name="BNo"></param>
        /// <returns></returns>
        [CustomAuthorize(RoleEnum = UserLevel.Junior | UserLevel.Senior | UserLevel.Manager | UserLevel.Admin)]
        public async Task<IActionResult> Detail(int BNo, int? currentPage, string searchType, string searchString)
        {
            Board board = new Board(_boardService);
            await board.GetDetail(BNo, true);

            User currentUser = await _db.Users.FirstOrDefaultAsync(u => u.UNo == User.Identity.GetSid());
            ViewBag.User = currentUser;

            ViewBag.CurrentPage = currentPage ?? 1;
            ViewBag.SearchType = String.IsNullOrEmpty(searchType) ?  null : searchType;
            ViewBag.SearchString = String.IsNullOrEmpty(searchString) ? null : searchString;

            return View(board.Data);
        }

        /// <summary>
        /// 게시물 추가
        /// </summary>
        /// <returns></returns>
        [CustomAuthorize(RoleEnum = UserLevel.Senior | UserLevel.Manager | UserLevel.Admin)] 
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost, CustomAuthorize(RoleEnum = UserLevel.Senior | UserLevel.Manager | UserLevel.Admin)]
        public async Task<IActionResult> Add(Board model, List<IFormFile> files)
        {
            model.UNo = User.Identity.GetSid();
            model.Reg_Date = DateTime.Now;
            model.Cnt_Read = 0;

            // TODO : file 객체로 업로드된 파일정보도 저장 필요 [파일이름, 용량 등등]
            if (ModelState.IsValid)
            {
                _db.Boards.Add(model);
                if (_db.SaveChanges() > 0)
                {
                    if (files.Count != 0)
                    {
                        var Board = _db.Boards.OrderByDescending(b => b.BNo).FirstOrDefault();
                        var rootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                        var path = Path.Combine(rootPath, @"upload\files");

                        foreach (var file in files)
                        {
                            var filename = Path.GetFileName(file.FileName);

                            var provider = new FileExtensionContentTypeProvider();
                            string contentType;
                            if (!provider.TryGetContentType(file.FileName, out contentType))
                            {
                                contentType = "application/octet-stream";
                            }

                            var attach = new Attach
                            {
                                FileFullName = $@"{path}\{Guid.NewGuid()}.{filename}",
                                FileSize = (int)file.Length,
                                ContentType = contentType,
                                BNo = Board.BNo,
                                Reg_Date = Board.Reg_Date
                            };

                            // TODO : 파일 저장 이름 버그 수정    2020. 09. 02
                            using (var fileStream = new FileStream(attach.FileFullName, FileMode.Create))
                            {
                                await file.CopyToAsync(fileStream);
                            }

                            _db.Attachs.Add(attach);
                            _db.SaveChanges();
                        }
                    }
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
        [CustomAuthorize(RoleEnum = UserLevel.Senior | UserLevel.Manager | UserLevel.Admin)]
        public async Task<IActionResult> Edit(int BNo)
        {
            Board board = new Board(_boardService);
            await board.GetDetail(BNo, false);

            return View(board.Data);
        }

        [HttpPost, CustomAuthorize(RoleEnum = UserLevel.Senior | UserLevel.Manager | UserLevel.Admin)]
        public async Task<IActionResult> Edit(Board model, List<IFormFile> files)
        {
            model.UNo = User.Identity.GetSid();

            if (ModelState.IsValid)
            {
                _db.Entry(model).State = EntityState.Modified;
                if (_db.SaveChanges() > 0)
                {
                    if (files.Count != 0)
                    {
                        var Board = await _db.Boards.Where(b => b.BNo.Equals(model.BNo)).FirstOrDefaultAsync();
                        var rootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                        var path = Path.Combine(rootPath, @"upload\files");

                        foreach (var file in files)
                        {
                            var filename = Path.GetFileName(file.FileName);

                            var provider = new FileExtensionContentTypeProvider();
                            string contentType;
                            if (!provider.TryGetContentType(file.FileName, out contentType))
                            {
                                contentType = "application/octet-stream";
                            }

                            var attach = new Attach
                            {
                                FileFullName = $@"{path}\{Guid.NewGuid()}.{filename}",
                                FileSize = (int)file.Length,
                                ContentType = contentType,
                                BNo = Board.BNo,
                                Reg_Date = Board.Reg_Date
                            };

                            using (var fileStream = new FileStream(attach.FileFullName, FileMode.Create))
                            {
                                await file.CopyToAsync(fileStream);
                            }

                            _db.Attachs.Add(attach);
                            _db.SaveChanges();
                        }
                    }
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
        [CustomAuthorize(RoleEnum = UserLevel.Senior | UserLevel.Manager | UserLevel.Admin)]
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

        [HttpPost, Route("api/getFiles")]
        [CustomAuthorize(RoleEnum = UserLevel.Senior | UserLevel.Manager | UserLevel.Admin)]
        public async Task<IActionResult> GetFiles(string BNo)
        {
            var attachs = await _db.Attachs.Where(a => a.BNo.Equals(int.Parse(BNo))).ToListAsync();


            var jsonAttachs = JsonConvert.SerializeObject(attachs);

            return Ok(jsonAttachs);
        }

        [HttpPost, Route("api/removeFile")]
        [CustomAuthorize(RoleEnum = UserLevel.Senior | UserLevel.Manager | UserLevel.Admin)]
        public async Task<IActionResult> RemvoeFile(string ANo)
        {
            var attach = await _db.Attachs.Where(a => a.ANo.Equals(int.Parse(ANo))).FirstOrDefaultAsync();

            _db.Attachs.Remove(attach);
            if (_db.SaveChanges() > 0)
            {
                return Json(new { success = true, responseText = "등록된 파일이 삭제되었습니다." });
            }
            return Json(new { success = false, responseText = "오류 : 등록된 파일이 삭제되지 않았습니다." });
        }
    }
}
