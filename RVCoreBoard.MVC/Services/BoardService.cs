namespace RVCoreBoard.MVC.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using RVCoreBoard.MVC.DataContext;
    using RVCoreBoard.MVC.Models;
    using Microsoft.EntityFrameworkCore;

    public class BoardService : IBoardService
    {
        private readonly RVCoreBoardDBContext _db;

        public BoardService(RVCoreBoardDBContext db)
        {
            _db = db;
        }

        public Task<Board> GetDetail()
        {
            throw new NotImplementedException();
        }

        public async Task<List<Board>> GetList()
        {
            var boardList = await _db.Boards
                                    .Include("user")
                                    .OrderByDescending(p => p.BNo)
                                    .ToListAsync();
            return boardList;
        }
    }
}
