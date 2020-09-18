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
        private IUserService _userService;

        public ManageController(RVCoreBoardDBContext db, IBoardService boardService, IUserService userService)
        {
            _db = db;
            _boardService = boardService;
            _userService = userService;
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
        /// 사용자 관리
        /// </summary>
        /// <returns></returns>
        [CustomAuthorize(RoleEnum = UserLevel.Admin)]
        public async Task<IActionResult> UserManage(int? currentPage, string searchType, string searchString)
        {
            UserListInfoModel userListInfoModel = new UserListInfoModel(_userService);
            await userListInfoModel.GetList(currentPage ?? 1, searchType, searchString);

            ViewBag.CurrentPage = currentPage ?? 1;
            ViewBag.SearchType = String.IsNullOrEmpty(searchType) ? null : searchType;
            ViewBag.SearchString = String.IsNullOrEmpty(searchString) ? null : searchString;

            return View(userListInfoModel);
        }
    }
}
