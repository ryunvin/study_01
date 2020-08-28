using System;
using System.ComponentModel.DataAnnotations;

namespace RVCoreBoard.MVC.Models
{
    public class LoginModel
    {
        /// <summary>
        /// 사용자 아이디
        /// </summary>
        [Required(ErrorMessage = "아이디를 입력하세요.")]  // Not Null 설정
        public string Id { get; set; }

        /// <summary>
        /// 사용자 비밀번호
        /// </summary>
        [Required(ErrorMessage = "비밀번호를 입력하세요.")]  // Not Null 설정
        public string Password { get; set; }
    }
}
