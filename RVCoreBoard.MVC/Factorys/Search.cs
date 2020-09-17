using RVCoreBoard.MVC.Models;
using RVCoreBoard.MVC.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static RVCoreBoard.MVC.Models.BoardListInfoModel;

namespace RVCoreBoard.MVC.Factorys
{
    public abstract class  SearchBase
    {
        private readonly IBoardService _boardService;
        public SearchBase(IBoardService boardService)
        {
            _boardService = boardService;
        }

        protected IBoardService BoardService { get => _boardService; }

        public abstract Task<List<Board>> Search(int id, string searchString);
    }
}
