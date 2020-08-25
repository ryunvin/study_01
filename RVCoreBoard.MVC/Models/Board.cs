using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RVCoreBoard.MVC.Models
{
    public class Board
    {
        /// <summary>
        /// 게시물 번호
        /// </summary>
        [Key]
        public int BNo { get; set; }

        /// <summary>
        /// 게시물 제목
        /// </summary>
        [Required(ErrorMessage = "제목을 입력하세요.")]  // Not Null 설정
        public string Title { get; set; }

        /// <summary>
        /// 게시물 내용
        /// </summary>
        [Required(ErrorMessage = "내용을 입력하세요.")]  // Not Null 설정
        public string Content { get; set; }

        [Required]
        public DateTime Reg_Date { get; set; }
        
        [Required]
        public int Cnt_Read { get; set; }

        /// <summary>
        /// 작성자(FK_사용자번호)
        /// </summary>
        [Required]  // Not Null 설정
        public int UNo { get; set; }

        [ForeignKey("UNo")]
        public virtual User user { get; set; }
    }
}
