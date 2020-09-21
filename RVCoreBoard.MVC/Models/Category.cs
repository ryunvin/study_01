using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RVCoreBoard.MVC.Models
{
    public class Category
    {
        /// <summary>
        /// 카테고리 번호
        /// </summary>
        [Key, Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// 카테고리 이름
        /// </summary>
        [Required(ErrorMessage = "게시판 이름을 입력하세요.")] 
        [StringLength(60)]
        public string Name { get; set; }

        /// <summary>
        /// 사용자 등급
        /// </summary>
        [Required(ErrorMessage = "사용자 등급을 설정하세요.")]
        public short Level { get; set; }
        
        /// <summary>
        /// 게시판 생성 날짜
        /// </summary>
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime Reg_Date { get; set; }

        [Key, Column(Order = 0)]
        public int Gid { get; set; }

        [ForeignKey("Gid")]
        public virtual CategoryGroup categoryGroup { get; set; }
    }
}
