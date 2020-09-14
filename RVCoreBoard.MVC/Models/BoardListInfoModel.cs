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

        public int RecentCount { get; set; } = 15;

        public bool HasPreviousPage
        {
            get { return (CurrentPage > 1); }
        }
        public bool HasNextPage
        {
            get { return (CurrentPage < Count); }
        }

        public List<Board> Data { get; private set; }

        public async Task GetList(int Gid, int currentPage, string searchType, string searchString)
        {
            CurrentPage = currentPage;

            Data = await _boardService.GetBoardList();

            Data = Data.Where(d => d.category.Gid.Equals(Gid)).ToList();

            if (!String.IsNullOrEmpty(searchString))
            {
                switch (searchType)
                {
                    case "All":
                        Data = Data.Where(s => s.user.Name.Contains(searchString) || s.Content.Contains(searchString)
                                        || s.CommentList.Any(c => c.Content.Contains(searchString)) || s.Title.Contains(searchString)).ToList();
                        break;
                    case "Title":
                        Data = Data.Where(s => s.Title.Contains(searchString)).ToList();
                        break;
                    case "Writer":
                        Data = Data.Where(s => s.user.Name.Contains(searchString)).ToList();
                        break;
                    case "Content":
                        Data = Data.Where(s => s.Content.Contains(searchString)).ToList();
                        break;
                    case "Comment":
                        Data = Data.Where(s => s.CommentList.Any(c => c.Content.Contains(searchString))).ToList();
                        break;
                    case "FileName":
                        Data = Data.Where(s => s.AttachInfoList.Any(c => c.FileFullName.Contains(searchString))).ToList();
                        break;
                }
            }

            RowCount = Data.Count;
            Count = (int)Math.Ceiling(RowCount / (double)PageSize);

            Data = Data.Skip((currentPage - 1) * PageSize).Take(PageSize).ToList();
        }

        public async Task GetRecentBoards()
        {
            Data = await _boardService.GetRecentBoards(RecentCount);
        }
    }
}
