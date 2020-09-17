using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RVCoreBoard.MVC.Attributes;
using RVCoreBoard.MVC.DataContext;
using RVCoreBoard.MVC.Models;
using RVCoreBoard.MVC.Services;
using static RVCoreBoard.MVC.Models.User;

namespace RVCoreBoard.MVC.Controllers
{
    public class ManageController : Controller
    {
        private readonly RVCoreBoardDBContext _db;
        private IBoardService _boardService;

        public ManageController(RVCoreBoardDBContext db, IBoardService boardService)
        {
            _db = db;
            _boardService = boardService;
        }

        /// <summary>
        /// 게시판 관리
        /// </summary>
        /// <returns></returns>
        [CustomAuthorize(RoleEnum = UserLevel.Admin)]
        public async Task<IActionResult> BoardManage()
        {
            
            List<CategoryGroup> categoryGroupList = await _db.CatergoryGroups
                                                            .OrderBy(c => c.Gid)
                                                            .ToListAsync();
            ViewBag.CategoryGroupList = null;
            ViewBag.CurrentCategoryGroup = null;
            int firstGid = 0;

            if (categoryGroupList.Count > 0)
            {
                ViewBag.CategoryGroupList = categoryGroupList;
                firstGid = categoryGroupList.FirstOrDefault().Gid;
                ViewBag.CurrentCategoryGroup = categoryGroupList.Where(c => c.Gid == firstGid).FirstOrDefault();
            }

            
            CategoryListInfoModel categoryListInfoModel = new CategoryListInfoModel(_boardService);
            await categoryListInfoModel.GetList(firstGid);

            return View(categoryListInfoModel);
        }

        /// <summary>
        /// 게시판 그룹 추가
        /// </summary>
        /// <returns></returns>
        [HttpPost, Route("api/categoryGroupAdd")]
        [CustomAuthorize(RoleEnum = UserLevel.Admin)]
        public async Task<IActionResult> CategoryGroupAdd(CategoryGroup categoryGroup)
        {
            await _db.CatergoryGroups.AddAsync(categoryGroup);
            if (await _db.SaveChangesAsync() > 0)
            {
                var group = await _db.CatergoryGroups.OrderByDescending(c => c.Gid).FirstOrDefaultAsync();

                return Ok(group);
            }
            return NotFound();
        }

        /// <summary>
        /// 게시판 그룹 삭제
        /// </summary>
        /// <returns></returns>
        [HttpPost, Route("api/categoryGroupDelete")]
        [CustomAuthorize(RoleEnum = UserLevel.Admin)]
        public async Task<IActionResult> CategoryGroupDelete(string Gid)
        {
            var group = await _db.CatergoryGroups.FirstOrDefaultAsync(c => c.Gid.Equals(int.Parse(Gid)));

            _db.CatergoryGroups.Remove(group);
            if (_db.SaveChanges() > 0)
            {
                return Json(new { success = true, responseText = "삭제되었습니다." });
            }
            return Json(new { success = false, responseText = "오류 : 삭제되지 않았습니다." });
        }

        /// <summary>
        /// 게시판 그룹 수정
        /// </summary>
        /// <param name="categoryGroup"></param>
        /// <returns></returns>
        [HttpPost, Route("api/categoryGroupModify")]
        [CustomAuthorize(RoleEnum = UserLevel.Admin)]
        public async Task<IActionResult> CategoryGroupModify(CategoryGroup categoryGroup)
        {
            _db.Entry(categoryGroup).State = EntityState.Modified;
            if (await _db.SaveChangesAsync() > 0)
            {
                var group = await _db.CatergoryGroups.FirstOrDefaultAsync(c => c.Gid.Equals(categoryGroup.Gid));

                return Ok(group);
            }
            return NotFound();
        }

        /// <summary>
        /// 게시판 추가
        /// </summary>
        /// <returns></returns>
        [HttpPost, Route("api/categoryAdd")]
        [CustomAuthorize(RoleEnum = UserLevel.Admin)]
        public async Task<IActionResult> CategoryAdd(Category category)
        {
            category.Reg_Date = DateTime.Now;

            await _db.Categorys.AddAsync(category);
            if (await _db.SaveChangesAsync() > 0)
            {
                var cgory = await _db.Categorys.OrderByDescending(c => c.Id).FirstOrDefaultAsync();

                return Ok(cgory);
            }
            return NotFound();
        }

        /// <summary>
        /// 게시판 삭제
        /// </summary>
        /// <returns></returns>
        [HttpPost, Route("api/categoryDelete")]
        [CustomAuthorize(RoleEnum = UserLevel.Admin)]
        public async Task<IActionResult> CategoryDelete(string Id)
        {
            var cgory = await _db.Categorys.FirstOrDefaultAsync(c => c.Id.Equals(int.Parse(Id)));

            _db.Categorys.Remove(cgory);
            if (_db.SaveChanges() > 0)
            {
                return Json(new { success = true, responseText = "삭제되었습니다." });
            }
            return Json(new { success = false, responseText = "오류 : 삭제되지 않았습니다." });
        }

        /// <summary>
        /// 게시판 수정
        /// </summary>
        /// <param name="categoryGroup"></param>
        /// <returns></returns>
        [HttpPost, Route("api/categoryModify")]
        [CustomAuthorize(RoleEnum = UserLevel.Admin)]
        public async Task<IActionResult> CategoryModify(Category category)
        {
            category.Reg_Date = DateTime.Now;

            _db.Entry(category).State = EntityState.Modified;
            if (await _db.SaveChangesAsync() > 0)
            {
                var cgory = await _db.Categorys.FirstOrDefaultAsync(c => c.Id.Equals(category.Id));

                return Ok(cgory);
            }
            return NotFound();
        }

        /// <summary>
        /// 게시판 그룹 갖고오기
        /// </summary>
        /// <param name="categoryGroup"></param>
        /// <returns></returns>
        [HttpPost, Route("api/getCategoryGroups")]
        [CustomAuthorize(RoleEnum = UserLevel.Admin)]
        public async Task<IActionResult> GetCategoryGroups()
        {
            var categoryGroupsList = await _db.CatergoryGroups.OrderBy(c => c.Gid).ToListAsync();

            if(categoryGroupsList != null)
            {
                return Ok(categoryGroupsList);
            }
            return NotFound();
        }

        /// <summary>
        /// 게시판 그룹 갖고오기
        /// </summary>
        /// <param name="categoryGroup"></param>
        /// <returns></returns>
        [HttpPost, Route("api/getCategorys")]
        [CustomAuthorize(RoleEnum = UserLevel.Admin)]
        public async Task<IActionResult> GetCategorys(string Gid)
        {
            var categoryList = await _db.Categorys.OrderBy(c => c.Id).ToListAsync();
            categoryList = categoryList.Where(c => c.Gid.Equals(int.Parse(Gid))).ToList();

            if (categoryList != null)
            {
                return Ok(categoryList);
            }
            return NotFound();
        }
    }
}
