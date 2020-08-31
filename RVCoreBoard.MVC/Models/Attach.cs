using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RVCoreBoard.MVC.Models
{
    public class Attach
    {
        /// <summary>
        /// 파일 인덱스
        /// </summary>
        [Key]
        public int ANo { get; set; }

        /// <summary>
        /// 파일 이름
        /// </summary>
        [Required]
        public string FileFullName { get; set; }

        /// <summary>
        /// 파일 사이즈
        /// </summary>
        [Required]
        public int FileSize { get; set; }

        /// <summary>
        /// 파일 등록 일시
        /// </summary>
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime Reg_Date { get; set; }

        /// <summary>
        /// 게시물 번호(FK_게시물 번호)
        /// </summary>
        [Required]  // Not Null 설정
        public int BNo  { get; set; }

        [ForeignKey("BNo")]
        public virtual Board board { get; set; }
    }
}
