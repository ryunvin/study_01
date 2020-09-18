using RVCoreBoard.MVC.Models;
using RVCoreBoard.MVC.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RVCoreBoard.MVC.Factorys
{
    public abstract class BoardSearchBase
    {
        private readonly IBoardService _boardService;
        public BoardSearchBase(IBoardService boardService)
        {
            _boardService = boardService;
        }

        protected IBoardService BoardService { get => _boardService; }

        public abstract Task<List<Board>> Search(int id, string searchString);
    }
}
