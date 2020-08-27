namespace RVCoreBoard.MVC.Models
{
    using RVCoreBoard.MVC.Services;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class BoardListInfoModel
    {
        private readonly IBoardService _boardService;

        public BoardListInfoModel() { }

        public BoardListInfoModel(IBoardService boardService)
        {
            _boardService = boardService;
        }

        public int CurrentPage { get; set; } = 1;

        public int Count { get; set; }

        public int PageSize { get; set; } = 10;

        public int RowCount { get; set; }

        public List<Board> Data { get; private set; }

        public async Task GetList()
        {
            Data = await _boardService.GetList();
        }
    }
}
