using RVCoreBoard.MVC.Models;
using RVCoreBoard.MVC.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static RVCoreBoard.MVC.Models.BoardListInfoModel;

namespace RVCoreBoard.MVC.Factorys
{
    internal class All : SearchBase
    {
        public All(IBoardService boardService) : base(boardService)
        {
        }

        public override async Task<List<Board>> Search(int id, string searchString)
        {
            return await base.BoardService.GetBoardList(s => s.category.Id == id && (s.user.Name.Contains(searchString) || s.Content.Contains(searchString)
                                        || s.CommentList.Any(c => c.Content.Contains(searchString)) || s.Title.Contains(searchString)));
        }
    }
}
