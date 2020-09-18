using RVCoreBoard.MVC.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RVCoreBoard.MVC.Models
{
    public class Comment
    {
        private readonly IBoardService _boardService;

        public Comment() { }

        public Comment(IBoardService boardService)
        {
            _boardService = boardService;
        }

        /// <summary>
        /// 댓글 번호
        /// </summary>
        [Key]
        public int CNo { get; set; }

        /// <summary>
        /// 게시물 내용
        /// </summary>
        [Required(ErrorMessage = "내용을 입력하세요.")]  // Not Null 설정
        [StringLength(400)]
        public string Content { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Reg_Date { get; set; }

        /// <summary>
        /// 작성자(FK_사용자번호)
        /// </summary>
        [Required]  // Not Null 설정
        public int UNo { get; set; }

        /// <summary>
        /// 게시물 번호(FK_게시물번호)
        /// </summary>
        [Required]  // Not Null 설정
        public int BNo { get; set; }

        [ForeignKey("UNo")]
        public virtual User user { get; set; }

        [ForeignKey("BNo")]
        public virtual Board board { get; set; }

        public int RecentCount { get; set; } = 10;

        public List<Comment> Data { get; private set; }

        public async Task GetRecentComments()
        {
            Data = await _boardService.GetRecentComments(RecentCount);
        }
        public async Task GetCommentList(int bNo)
        {
            Data = await _boardService.GetCommnetList(bNo);
        }
    }
}
