using RVCoreBoard.MVC.Models;
using RVCoreBoard.MVC.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using static RVCoreBoard.MVC.Models.BoardListInfoModel;

namespace RVCoreBoard.MVC.Factorys
{
    internal class Writer : SearchBase
    {
        public Writer(IBoardService boardService) : base(boardService)
        {
        }

        public override async Task<List<Board>> Search(int id, string searchString)
        {
            return await base.BoardService.GetBoardList(s => s.category.Id == id && s.user.Name.Contains(searchString));
        }
    }
}
