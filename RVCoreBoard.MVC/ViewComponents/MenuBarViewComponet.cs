using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RVCoreBoard.MVC.DataContext;
using System.Linq;
using System.Threading.Tasks;

namespace RVCoreBoard.MVC.Components
{
    [ViewComponent(Name = "MenuBar")]
    public class MenuBarViewComponet : ViewComponent
    {
        private readonly RVCoreBoardDBContext _db;

        public MenuBarViewComponet(RVCoreBoardDBContext db)
        {
            _db = db;
        }

        [AllowAnonymous]
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var categoryGroupList = await _db.CatergoryGroups
                                            .OrderBy(c => c.Gid)
                                            .ToListAsync();
            var categoryList = await _db.Catergorys
                                    .OrderBy(c => c.Gid).ThenBy(c => c.Id)
                                    .ToListAsync();

            ViewBag.CategoryGroupList = categoryGroupList;
            ViewBag.CategoryList = categoryList;

            return View("MenuBar");
        }

    }
}
