using RVCoreBoard.MVC.Models;
using RVCoreBoard.MVC.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RVCoreBoard.MVC.Factorys
{
    internal class FileName : BoardSearchBase
    {
        public FileName(IBoardService boardService) : base(boardService)
        {
        }

        public override async Task<List<Board>> Search(int id, string searchString)
        {
            return await base.BoardService.GetBoardList(s => s.category.Id == id && s.CommentList.Any(c => c.Content.Contains(searchString)));
        }
    }
}
