namespace RVCoreBoard.MVC.Models
{
    using RVCoreBoard.MVC.Services;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class CategoryListInfoModel
    {
        private readonly IBoardService _boardService;

        public CategoryListInfoModel() { }

        public CategoryListInfoModel(IBoardService boardService)
        {
            _boardService = boardService;
        }

        public List<Category> Data { get; private set; }

        public async Task GetList(int Gid)
        {
            Data = await _boardService.GetCategoryList();

            Data = Data.Where(c => c.Gid.Equals(Gid)).ToList();
        }
    }
}
