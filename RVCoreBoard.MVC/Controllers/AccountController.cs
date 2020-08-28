using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RVCoreBoard.MVC.DataContext;
using RVCoreBoard.MVC.Models;

namespace RVCoreBoard.MVC.Controllers
{

    public class AccountController : Controller
    {
        private readonly RVCoreBoardDBContext _db;

        public AccountController(RVCoreBoardDBContext db)
        {
            _db = db;
        }

        // <summary>
        /// 로그인
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        /// <summary>
        /// 로그인
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _db.Users.FirstOrDefault(u => u.Id.Equals(model.Id) && u.Password.Equals(model.Password));

                if (user != null)
                {
                    // 로그인 성공 시
                    HttpContext.Session.SetInt32("USER_LOGIN_KEY", user.UNo);
                    return RedirectToAction("Index", "Home");    //  로그인 성공 페이지로 이동
                }
                // 로그인 실패 시
                ModelState.AddModelError("UserIDorPWNotCorrect", "사용자 ID 혹은 비밀번호가 올바르지 않습니다.");
            }
            return View(model);
        }
        /// <summary>
        /// 로그아웃
        /// </summary>
        /// <returns></returns>
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("USER_LOGIN_KEY");
            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// 회원 가입
        /// </summary>
        /// <returns></returns>
        public IActionResult Register()
        {
            return View();
        }

        /// <summary>
        /// 회원 가입 전송
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Register(User model)
        {
            if (ModelState.IsValid)
            {
                if(_db.Users.Any( p => p.Id == model.Id))
                {
                    ModelState.AddModelError("UserIDDuplicates", "이미 사용중인 아이디 입니다.");
                    return View(model);
                }

                _db.Users.Add(model);
                _db.SaveChanges();
                return RedirectToAction("Login", "Account");
            }
            return View(model);
        }
    }
}
