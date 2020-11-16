using RVCoreBoard.MVC.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RVCoreBoard.MVC.Services
{
    public interface IBoardService
    {
        Task<List<Board>> GetBoardList();
        // TODO : 조건 검색 Get 메서드 추가  [2020. 09. 17]
        Task<List<Board>> GetBoardList(System.Linq.Expressions.Expression<Func<Board, bool>> predicate);
        Task<Board> GetBoardDetail(int BNo, bool bDetail);
        Task<List<Category>> GetCategoryList();
        Task<List<Board>> GetRecentBoards(int count);
        Task<List<Board>> GetNotNotiBoards();
        Task<List<Comment>> GetNotNotiComments();
        Task<List<Comment>> GetRecentComments(int count);
        Task<int> GetBoardPrevBNo(int bNo, int id);
        Task<int> GetBoardNextBNo(int bNo, int id);
        Task<List<Comment>> GetCommnetList(int bNo);
    }
}
