using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RVCoreBoard.MVC.Attributes;
using RVCoreBoard.MVC.DataContext;
using RVCoreBoard.MVC.Hubs;
using RVCoreBoard.MVC.Models;
using RVCoreBoard.MVC.Services;
using static RVCoreBoard.MVC.Models.User;

namespace RVCoreBoard.MVC.Apis
{
    public class CommentController : Controller
    {
        private readonly RVCoreBoardDBContext _db;
        private IBoardService _boardService;

        public CommentController(RVCoreBoardDBContext db, IBoardService boardService)
        {
            _db = db;
            _boardService = boardService;
        }

        [HttpPost, Route("api/commentAdd")]
        [CustomAuthorize(RoleEnum = UserLevel.Junior | UserLevel.Senior | UserLevel.Manager | UserLevel.Admin)]
        public async Task<IActionResult> CommentAdd(Comment comment)
        {
            comment.Reg_Date = DateTime.Now;

            await _db.Comments.AddAsync(comment);
            if (await _db.SaveChangesAsync() > 0)
            {
                return Json(new { success = true, responseText = "댓글이 등록되었습니다." });
            }
            return Json(new { success = true, responseText = "댓글이 등록 되지 않았습니다." });
        }

        [HttpPost, Route("api/commentDelete")]
        [CustomAuthorize(RoleEnum = UserLevel.Junior | UserLevel.Senior | UserLevel.Manager | UserLevel.Admin)]
        public async Task<IActionResult> CommentDelete(string CNo)
        {
            var comment = await _db.Comments.FirstOrDefaultAsync(c => c.CNo.Equals(int.Parse(CNo)));

            _db.Comments.Remove(comment);
            if (_db.SaveChanges() > 0)
            {
                return Json(new { success = true, responseText = "댓글이 삭제되었습니다." });
            }
            return Json(new { success = false, responseText = "오류 : 댓글이 삭제되지 않았습니다." });
        }

        [HttpPost, Route("api/commentModify")]
        [CustomAuthorize(RoleEnum = UserLevel.Junior | UserLevel.Senior | UserLevel.Manager | UserLevel.Admin)]
        public async Task<IActionResult> CommentModify(Comment comment)
        {
            comment.Reg_Date = DateTime.Now;

            _db.Entry(comment).State = EntityState.Modified;
            if (await _db.SaveChangesAsync() > 0)
            {
                return Json(new { success = true, responseText = "댓글이 수정되었습니다." });
            }
            return Json(new { success = true, responseText = "댓글이 수정되지 않았습니다." });
        }

        [HttpPost, Route("api/getCommentList")]
        [CustomAuthorize(RoleEnum = UserLevel.Junior | UserLevel.Senior | UserLevel.Manager | UserLevel.Admin)]
        public async Task<IActionResult> GetCommentList(string BNo)
        {
            var commentList = new Comment(_boardService);
            await commentList.GetCommentList(int.Parse(BNo));

            return Ok(commentList.Data);
        }
    }
}
