﻿using RVCoreBoard.MVC.Helpers;
using RVCoreBoard.MVC.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Threading.Tasks;

namespace RVCoreBoard.MVC.Models
{
    public class User
    {
        private readonly IAccountService _accountService;

        public User() { }

        public User(IAccountService accountService)
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
        [Required(ErrorMessage = "아이디를 입력하세요.")]  // Not Null 설정
        [StringLength(20)]
        public string Id { get; set; }

        /// <summary>
        /// 사용자 비밀번호
        /// </summary>
        [Required(ErrorMessage = "비밀번호를 입력하세요.")]  // Not Null 설정
        [StringLength(200)]
        public string Password { get; set; }

        /// <summary>
        /// 사용자 이름
        /// </summary>
        [Required(ErrorMessage = "이름을 입력하세요.")]  // Not Null 설정
        [StringLength(15)]
        public string Name { get; set; }

        /// <summary>
        /// 사용자 생년월일
        /// </summary>
        [Required(ErrorMessage = "생년월일을 입력하세요.")]  // Not Null 설정
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Birth { get; set; }

        /// <summary>
        /// 사용자 휴대폰번호
        /// </summary>
        [Required(ErrorMessage = "휴대폰 번호를 입력하세요.")]  // Not Null 설정
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

        [Required]
        [DefaultValue(1)]
        public int Level { get; set; }

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
            var roles = Enum.GetName(typeof(UserLevel), 1 << user.Data.Level);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Sid, $"{user.Data.UNo}"),
                new Claim("Id", user.Data.Id),
                new Claim("Name", user.Data.Name),
                new Claim(ClaimTypes.Role, roles)
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
    }
}
