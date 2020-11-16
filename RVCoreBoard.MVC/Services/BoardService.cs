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

        public async Task<Board> GetBoardDetail(int BNo, bool bDetail)
        {
            var board = await _db.Boards
                                .Include("user")
                                .Include(b => b.category).ThenInclude(c => c.categoryGroup)
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

        public async Task<List<Board>> GetBoardList()
        {
            var boardList = await _db.Boards
                                    .Include("user")
                                    .Include(cg => cg.category)
                                    .Include(p => p.AttachInfoList)
                                    .Include(c => c.CommentList)
                                    .OrderByDescending(p => p.BNo)
                                    .ToListAsync();
            return boardList;
        }

        public async Task<List<Board>> GetBoardList(System.Linq.Expressions.Expression<Func<Board, bool>> predicate)
        {
            var boardList = await _db.Boards
                                    .Include("user")
                                    .Include(cg => cg.category)
                                    .Include(p => p.AttachInfoList)
                                    .Include(c => c.CommentList)
                                    .Where(predicate)
                                    .OrderByDescending(p => p.BNo)
                                    .ToListAsync();
            return boardList;
        }

        public async Task<List<Category>> GetCategoryList()
        {
            var categoryList = await _db.Categorys
                                    .Include("categoryGroup")
                                    .OrderBy(p => p.Gid).ThenBy(p => p.Id)
                                    .ToListAsync();
            return categoryList;
        }

        public async Task<List<Board>> GetRecentBoards(int count)
        {
            var boardList = await _db.Boards
                                    .Include("user")
                                    .Include(cg => cg.category)
                                    .Take(count)
                                    .OrderByDescending(p => p.BNo)
                                    .ToListAsync();
            return boardList;
        }

        public async Task<List<Comment>> GetRecentComments(int count)
        {
            var commentList = await _db.Comments
                                       .Include("user")
                                       .Include(c => c.board)
                                       .ThenInclude(cg => cg.category)
                                       .Take(count)
                                       .OrderByDescending(c => c.CNo)
                                       .ToListAsync();
            return commentList;
        }
        public async Task<int> GetBoardPrevBNo(int bNo, int id)
        {
            var prevBNo = await _db.Boards
                                    .Include(cg => cg.category)
                                    .Where(b => b.BNo < bNo && b.category.Id == id)
                                    .OrderByDescending(b => b.BNo)
                                    .Select(b => b.BNo)
                                    .FirstOrDefaultAsync();
            return prevBNo;
        }
        public async Task<int> GetBoardNextBNo(int bNo, int id)
        {
            var nextBNo = await _db.Boards
                                    .Include(cg => cg.category)
                                    .Where(b => b.BNo > bNo && b.category.Id == id)
                                    .OrderBy(b => b.BNo)
                                    .Select(b => b.BNo)
                                    .FirstOrDefaultAsync();
            return nextBNo;
        }
        public async Task<List<Comment>> GetCommnetList(int bNo)
        {
            var commentList = await _db.Comments
                                        .Include("user")
                                        .Where(c => c.BNo.Equals(bNo))
                                        .OrderBy(c => c.Reg_Date)
                                        .ToListAsync();
            return commentList;
        }
    }
}
