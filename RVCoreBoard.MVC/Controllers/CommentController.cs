using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RVCoreBoard.MVC.Attributes;
using RVCoreBoard.MVC.DataContext;
using RVCoreBoard.MVC.Models;

namespace RVCoreBoard.MVC.Controllers
{
    public class CommentController : Controller
    {
        private readonly RVCoreBoardDBContext _db;

        public CommentController(RVCoreBoardDBContext db)
        {
            _db = db;
        }

        [HttpPost, Route("api/commentAdd")]
        [CheckSession]
        public async Task<IActionResult> CommentAdd(Comment comment)
        {
            comment.Reg_Date = DateTime.Now;

            await _db.Comments.AddAsync(comment);
            if (await _db.SaveChangesAsync() > 0)
            {
                var cment = await _db.Comments.OrderByDescending(c => c.CNo).FirstOrDefaultAsync();

                return Ok(cment);
            }
            return NotFound();
        }

        [HttpPost, Route("api/commentDelete")]
        [CheckSession]
        public async Task<IActionResult> CommentDelete(string CNo)
        {
            var comment = await _db.Comments.FirstOrDefaultAsync(c => c.CNo.Equals(int.Parse(CNo)));

            _db.Comments.Remove(comment);
            if (_db.SaveChanges() > 0)
            {
                return Json(new { success = true, responseText = "삭제되었습니다." });
            }
            return Json(new { success = false, responseText = "오류 : 삭제되지 않았습니다." });
        }

        [HttpPost, Route("api/commentModify")]
        [CheckSession]
        public async Task<IActionResult> CommentModify(Comment comment)
        {
            comment.Reg_Date = DateTime.Now;

            _db.Entry(comment).State = EntityState.Modified;
            if (await _db.SaveChangesAsync() > 0)
            {
                var cment = await _db.Comments
                                    .Include("user")
                                    .FirstOrDefaultAsync(c => c.BNo.Equals(comment.BNo));

                return Ok(cment);
            }
            return NotFound();
        }
    }
    }
