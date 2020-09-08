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

        public async Task<Board> GetDetail(int BNo, bool bDetail)
        {
            var board = await _db.Boards
                                .Include("user")
                                // TODO : 첨부 파일 정보 Include해서 데이터 Get    2020. 09. 02
                                .Include(p => p.AttachInfoList).ThenInclude(p => p.board)
                                .Include(c => c.CommentList).ThenInclude(c => c.board)
                                .Include(c => c.CommentList).ThenInclude(c => c.user)
                                .FirstOrDefaultAsync(b => b.BNo.Equals(BNo));
            if (bDetail)
            {
                board.Cnt_Read++;

                _db.Entry(board).State = EntityState.Modified;
                _db.SaveChanges();
            }
            return board;
        }

        public async Task<List<Board>> GetList()
        {
            var boardList = await _db.Boards
                                    .Include("user")
                                    .Include(p => p.AttachInfoList)
                                    .Include(c => c.CommentList)
                                    .OrderByDescending(p => p.BNo)
                                    .ToListAsync();
            return boardList;
        }
    }
}
