using RVCoreBoard.MVC.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RVCoreBoard.MVC.Models
{
    public class CategoryGroup
    {
        /// <summary>
        /// 카테고리 번호
        /// </summary>
        [Key]
        public int Gid { get; set; }

        /// <summary>
        /// 카테고리 이름
        /// </summary>
        [Required(ErrorMessage = "카테고리 그룹 이름을 입력하세요.")] 
        [StringLength(60)]
        public string Gname { get; set; }
    }
}
