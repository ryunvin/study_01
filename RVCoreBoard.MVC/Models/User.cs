using System;
using System.ComponentModel.DataAnnotations;

namespace RVCoreBoard.MVC.Models
{
    public class User
    {
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
        [StringLength(50)]
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
    }
}
