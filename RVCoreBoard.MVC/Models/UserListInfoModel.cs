namespace RVCoreBoard.MVC.Models
{
    using RVCoreBoard.MVC.Attributes;
    using RVCoreBoard.MVC.Factorys;
    using RVCoreBoard.MVC.Services;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class UserListInfoModel
    {
        public enum SearchType
        {
            [ExtensionEnum(typeof(Id))]
            Id,
            [ExtensionEnum(typeof(Name))]
            Name,
            [ExtensionEnum(typeof(Level))]
            Level
        }

        private readonly IUserService _userService;

        public UserListInfoModel() { }

        public UserListInfoModel(IUserService userService)
        {
            _userService = userService;
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

        public List<User> Data { get; private set; }

        public async Task GetList(int currentPage, string searchType, string searchString)
        {
            CurrentPage = currentPage;

            // 검색
            if (!String.IsNullOrEmpty(searchString))
            {
                SearchType sType = (SearchType)Enum.Parse(typeof(SearchType), searchType);
                UserSearchBase searchBase = SearchFactory.GetSearchUserList(sType, _userService);
                Data = await searchBase.Search(searchString);
            }
            else
            {
                Data = await _userService.GetUserList(null);
            }

            RowCount = Data.Count;
            Count = (int)Math.Ceiling(RowCount / (double)PageSize);

            Data = Data.Skip((currentPage - 1) * PageSize).Take(PageSize).ToList();
        }
    }
}
