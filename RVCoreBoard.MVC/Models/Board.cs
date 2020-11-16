using RVCoreBoard.MVC.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Threading.Tasks;

namespace RVCoreBoard.MVC.Models
{
    public class Board
    {
        private readonly IBoardService _boardService;

        public Board() { }

        public Board(IBoardService boardService)
        {
            _boardService = boardService;
        }

        /// <summary>
        /// 게시물 번호
        /// </summary>
        [Key]
        public int BNo { get; set; }

        /// <summary>
        /// 게시물 제목
        /// </summary>
        [Required(ErrorMessage = "제목을 입력하세요.")]  // Not Null 설정
        [StringLength(150)]
        public string Title { get; set; }

        /// <summary>
        /// 게시물 내용
        /// </summary>
        [Required(ErrorMessage = "내용을 입력하세요.")]  // Not Null 설정
        public string Content { get; set; }

        /// <summary>
        /// 게시물 등록 일시
        /// </summary>
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime Reg_Date { get; set; }

        /// <summary>
        /// 게시물 조회수
        /// </summary>
        [Required]
        public int Cnt_Read { get; set; }

        [Required]
        public int Id { get; set; }

        /// <summary>
        /// 작성자(FK_사용자번호)
        /// </summary>
        [Required]  // Not Null 설정
        public int UNo { get; set; }

        /// <summary>
        /// 알림 여부
        /// </summary>
        public bool IsNoti { get; set; }

        [ForeignKey("UNo")]
        public virtual User user { get; set; }

        [ForeignKey("Id")]
        public virtual Category category { get; set; }


        // TODO : 첨부파일 정보 리스트 속성 추가     2020. 09. 02
        public ICollection<Attach> AttachInfoList { get; set; }

        public List<Comment> CommentList { get; set; }
        public int PrevBNo { get; set; } = 0;
        public int NextBNo { get; set; } = 0;
        public Board Data { get; private set; }

        public async Task GetDetail(int BNo, bool bDetail)
        {
            Data = await _boardService.GetBoardDetail(BNo, bDetail);

            Data.PrevBNo = await _boardService.GetBoardPrevBNo(BNo, Data.category.Id);
            Data.NextBNo = await _boardService.GetBoardNextBNo(BNo, Data.category.Id);
        }
    }
}