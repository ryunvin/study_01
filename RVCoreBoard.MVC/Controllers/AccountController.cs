using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RVCoreBoard.MVC.DataContext;
using RVCoreBoard.MVC.Models;
using RVCoreBoard.MVC.Services;

namespace RVCoreBoard.MVC.Controllers
{

    public class AccountController : Controller
    {
        private readonly RVCoreBoardDBContext _db;
        private IUserService _userService;

        public AccountController(RVCoreBoardDBContext db, IUserService userService)
        {
            _db = db;
            _userService = userService;
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
                User user = new User(_userService);
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

                // 인증번호 체크
                string requestUrl = $"http://arooong.synology.me:8002/command/CheckAuthNum?authnum={model.AuthenticationNumber}";

                WebClient wc = new WebClient();
                string responseData = await wc.DownloadStringTaskAsync(requestUrl);
                if (string.IsNullOrWhiteSpace(responseData) ||
                    responseData != "{\"output\":\"OK\"}")
                {
                    ModelState.AddModelError("AuthenticationNumber", "인증에 실패했습니다.");
                    return View();
                }
                else
                {
                    WebClient wc2 = new WebClient();
                    wc2.DownloadStringTaskAsync($"http://arooong.synology.me:8002/command/DelAuthNum?authnum={model.AuthenticationNumber}");
                }

                User user = new User(_userService);
                bool IsRegister = await user.Register(model);

                if (IsRegister)
                {
                    return RedirectToAction("Login", "Account");
                }
                
            }
            return View(model);
        }

        /// <summary>
        ///  비밀번호 찾기
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        /// <summary>
        /// 비밀번호 찾기
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword(string id, string name)
        {
            User user = new User(_userService);
            await user.FindUser(id);

            if (user.Data != null)
            {
                // 해당 유저가 있을 시
                if (user.Data.Name.Equals(name))
                    return Redirect($"ChangePassword?Id={id}");
            }
            // 로그인 실패 시
            ModelState.AddModelError("UserIDorNameIncorrect", "사용자 ID 혹은 이름이 올바르지 않습니다.");
            return View();
        }

        /// <summary>
        ///  비밀번호 변경
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ChangePassword(string Id)
        {
            ViewBag.Id = Id;
            return View();
        }

        /// <summary>
        /// 비밀번호 찾기
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ChangePassword(string id, string password)
        {
            User user = new User(_userService);
            await user.FindUser(id);

            if (user.Data != null)
            {
                user.Data.Password = user.ConvertPassword(password);
                _db.Entry(user.Data).State = EntityState.Modified;
                if (_db.SaveChanges() > 0)
                {
                    return RedirectToAction("Login", "Account");
                }
            }
            return Redirect($"ChangePassword?Id={id}"); 
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
