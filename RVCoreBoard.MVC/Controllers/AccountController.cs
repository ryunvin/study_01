using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RVCoreBoard.MVC.DataContext;
using RVCoreBoard.MVC.Models;
using RVCoreBoard.MVC.Services;

namespace RVCoreBoard.MVC.Controllers
{

    public class AccountController : Controller
    {
        private readonly RVCoreBoardDBContext _db;
        private IAccountService _accountService;

        public AccountController(RVCoreBoardDBContext db, IAccountService accountService)
        {
            _db = db;
            _accountService = accountService;
        }

        // <summary>
        /// 로그인
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        /// <summary>
        /// 로그인
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                User user = new User(_accountService);
                await user.Login(model);

                if (user.Data != null)
                {
                    // 로그인 성공 시
                    var claims = user.BuildClaims(user.Data);

                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));

                    return RedirectToLocal(returnUrl); 
                }
                // 로그인 실패 시
                ModelState.AddModelError("UserIDorPWNIncorrect", "사용자 ID 혹은 비밀번호가 올바르지 않습니다.");
            }
            return View(model);
        }
        /// <summary>
        /// 로그아웃
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// 회원 가입
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
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
        [AllowAnonymous]
        public async Task<IActionResult> Register(User model)
        {
            if (ModelState.IsValid)
            {
                if (_db.Users.Any(p => p.Id == model.Id))
                {
                    ModelState.AddModelError("UserIDDuplicates", "이미 사용중인 아이디 입니다.");
                    return View(model);
                }

                User user = new User(_accountService);
                bool IsRegister = await user.Register(model);

                if (IsRegister)
                {
                    return RedirectToAction("Login", "Account");
                }
                
            }
            return View(model);
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
    }
}
