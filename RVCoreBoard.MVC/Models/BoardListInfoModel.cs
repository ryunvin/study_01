namespace RVCoreBoard.MVC.Models
{
    using RVCoreBoard.MVC.Attributes;
    using RVCoreBoard.MVC.Factorys;
    using RVCoreBoard.MVC.Services;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class BoardListInfoModel
    {
        public enum SearchType
        {
            [ExtensionEnum(typeof(All))]
            All,
            [ExtensionEnum(typeof(Title))]
            Title,
            [ExtensionEnum(typeof(Writer))]
            Writer,
            [ExtensionEnum(typeof(Content))]
            Content,
            [ExtensionEnum(typeof(Comment))]
            Comment,
            [ExtensionEnum(typeof(FileName))]
            FileName
        }

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

        public int RecentCount { get; set; } = 10;

        public bool HasPreviousPage
        {
            get { return (CurrentPage > 1); }
        }
        public bool HasNextPage
        {
            get { return (CurrentPage < Count); }
        }

        public List<Board> Data { get; private set; }

        public async Task GetList(int id, int currentPage, string searchType, string searchString)
        {

            CurrentPage = currentPage;

            // 검색
            if (!String.IsNullOrEmpty(searchString))
            {
                // TODO : searchType은 하드코딩의 코드 보단 정형화된 Enum타입을 쓴느 것이 좋습니다.  [2020. 09. 17]
                // TODO : 검색 조건식이 복잡해지는 경우 확장성을 위해 팩토리패턴등을 활용하여 별도 검색처리를 담당하는 클래스를 만들어 분리 하는 것이 좋습니다.   [2020. 09. 17]
                //switch (searchType)
                //{
                //    case SearchType.All:
                //        All all = new All(searchString);
                //        Data = await _boardService.GetBoardList(all.predicate);
                //        break;
                //    case SearchType.Title:
                //        Data = await _boardService.GetBoardList(s => s.Title.Contains(searchString));
                //        break;
                //    case SearchType.Writer:
                //        Data = await _boardService.GetBoardList(s => s.user.Name.Contains(searchString));
                //        break;
                //    case SearchType.Content:
                //        Data = await _boardService.GetBoardList(s => s.Content.Contains(searchString));
                //        break;
                //    case SearchType.Comment:
                //        Data = await _boardService.GetBoardList(s => s.CommentList.Any(c => c.Content.Contains(searchString)));
                //        break;
                //    case SearchType.FileName:
                //        Data = await _boardService.GetBoardList(s => s.AttachInfoList.Any(c => c.FileFullName.Contains(searchString)));
                //        break;
                //}
                SearchType sType = (SearchType)Enum.Parse(typeof(SearchType), searchType);
                SearchBase searchBase = SearchFactory.GetSearchBoardList(id, sType, _boardService);
                Data = await searchBase.Search(id, searchString);
            }
            else
            {
                Data = await _boardService.GetBoardList(d => d.category.Id.Equals(id));
            }

            RowCount = Data.Count;
            Count = (int)Math.Ceiling(RowCount / (double)PageSize);

            Data = Data.Skip((currentPage - 1) * PageSize).Take(PageSize).ToList();
        }

        // TODO : 메서드 새로 작성     [2020. 09. 17]
        //public async Task GetList(int Id, int currentPage, string searchType, string searchString)
        //{
        //    CurrentPage = currentPage;

        //    Data = await _boardService.GetBoardList();

        //    // TODO : await _boardService.GetBoardList() 부분에서 불필요하게 모든 데이터를 가져와서 카테고리에 맞는 데이터를 필터링 하고있음   [2020. 09. 17]
        //    Data = Data.Where(d => d.category.Id.Equals(Id)).ToList();

        //    // TODO : 검색시 await _boardService.GetBoardList() 부분에서 불필요하게 모든 데이터를 가져와서 필터링 하고있음   [2020. 09. 17]
        //    if (!String.IsNullOrEmpty(searchString))
        //    {
        //        switch (searchType)
        //        {
        //            case "All":
        //                Data = Data.Where(s => s.user.Name.Contains(searchString) || s.Content.Contains(searchString)
        //                                || s.CommentList.Any(c => c.Content.Contains(searchString)) || s.Title.Contains(searchString)).ToList();
        //                break;
        //            case "Title":
        //                Data = Data.Where(s => s.Title.Contains(searchString)).ToList();
        //                break;
        //            case "Writer":
        //                Data = Data.Where(s => s.user.Name.Contains(searchString)).ToList();
        //                break;
        //            case "Content":
        //                Data = Data.Where(s => s.Content.Contains(searchString)).ToList();
        //                break;
        //            case "Comment":
        //                Data = Data.Where(s => s.CommentList.Any(c => c.Content.Contains(searchString))).ToList();
        //                break;
        //            case "FileName":
        //                Data = Data.Where(s => s.AttachInfoList.Any(c => c.FileFullName.Contains(searchString))).ToList();
        //                break;
        //        }
        //    }

        //    RowCount = Data.Count;
        //    Count = (int)Math.Ceiling(RowCount / (double)PageSize);

        //    Data = Data.Skip((currentPage - 1) * PageSize).Take(PageSize).ToList();
        //}

        public async Task GetRecentBoards()
        {
            Data = await _boardService.GetRecentBoards(RecentCount);
        }
    }
}
