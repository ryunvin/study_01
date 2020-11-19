using RVCoreBoard.MVC.Helpers;
using RVCoreBoard.MVC.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Claims;
using System.Threading.Tasks;

namespace RVCoreBoard.MVC.Models
{
    public class User
    {
        private readonly IUserService _accountService;

        public User() { }

        public User(IUserService accountService)
        {
            _accountService = accountService;
        }

        [Flags]
        public enum UserLevel
        {
            Newbie = 1 << 0,
            Junior = 1 << 1,
            Senior = 1 << 2,
            Manager = 1 << 3,
            Admin = 1 << 4
        }
        /// <summary>
        /// 사용자 번호
        /// </summary>
        [Key]   // PK 설정
        public int UNo { get; set; }

        /// <summary>
        /// 사용자 아이디
        /// </summary>
        [Required(ErrorMessage = "아이디를 입력해주세요.")]  // Not Null 설정
        [StringLength(20)]
        public string Id { get; set; }

        /// <summary>
        /// 사용자 비밀번호
        /// </summary>
        [Required(ErrorMessage = "비밀번호를 입력해주세요.")]  // Not Null 설정
        [StringLength(200)]
        public string Password { get; set; }

        /// <summary>
        /// 사용자 이름
        /// </summary>
        [Required(ErrorMessage = "이름을 입력해주세요.")]  // Not Null 설정
        [StringLength(15)]
        public string Name { get; set; }

        /// <summary>
        /// 사용자 생년월일
        /// </summary>
        [Required(ErrorMessage = "생년월일을 입력해주세요.")]  // Not Null 설정
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Birth { get; set; }

        /// <summary>
        /// 사용자 휴대폰번호
        /// </summary>
        [StringLength(20)]
        public string Phone { get; set; }

        /// <summary>
        /// 사용자 전화번호
        /// </summary>
        [StringLength(20)]
        public string Tel { get; set; }

        /// <summary>
        /// 사용자 주소
        /// </summary>
        [StringLength(300)]
        public string Address { get; set; }

        /// <summary>
        /// 사용자 상세주소
        /// </summary>
        [StringLength(300)]
        public string DetailAddress { get; set; }

        /// <summary>
        /// 사용자 이메일
        /// </summary>
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; }

        /// <summary>
        /// 사용자 등급
        /// </summary>
        [Required]
        public short Level { get; set; }

        [NotMapped]
        [Required(ErrorMessage = "인증번호를 입력해주세요.")]
        public string AuthenticationNumber { get; set; }

        public User Data { get; private set; }
        
        public string ConvertPassword(string pwd)
        {
            var sha = new System.Security.Cryptography.HMACSHA512();
            sha.Key = System.Text.Encoding.UTF8.GetBytes(pwd.Length.ToString());

            var hash = sha.ComputeHash(System.Text.Encoding.UTF8.GetBytes(pwd));

            return Convert.ToBase64String(hash);
        }

        public IList<Claim> BuildClaims(User user)
        {
            var roles = Enum.GetName(typeof(UserLevel), 1 << user.Level);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Sid, $"{user.UNo}"),
                new Claim("Id", user.Id),
                new Claim("Name", user.Name),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Role, roles),
                new Claim("RoleLevel", user.Level.ToString())
            };

            return claims;
        }

        public async Task Login(LoginModel model)
        {
            model.Password = ConvertPassword(model.Password);
            Data = await _accountService.Login(model);
        }

        public async Task<bool> Register(User user)
        {
            user.Password = ConvertPassword(user.Password);
            return await _accountService.Register(user);
        }
        public async Task FindUser(string id)
        {
            Data = await _accountService.FindUser(id);
        }
    }
}
