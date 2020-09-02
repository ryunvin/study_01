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

        public int PageRimitCount { get; set; } = 5;

        public int RowCount { get; set; }

        public bool HasPreviousPage
        {
            get { return (CurrentPage > 1); }
        }
        public bool HasNextPage
        {
            get { return (CurrentPage < Count); }
        }

        public List<Board> Data { get; private set; }

        public async Task GetList(int currentPage)
        {
            CurrentPage = currentPage;

            Data = await _boardService.GetList();

            RowCount = Data.Count;
            Count = (int)Math.Ceiling(RowCount / (double)PageSize);

            Data = Data.Skip((currentPage - 1) * PageSize).Take(PageSize).ToList();
        }
    }
}
