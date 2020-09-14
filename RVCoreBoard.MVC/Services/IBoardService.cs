using RVCoreBoard.MVC.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RVCoreBoard.MVC.Services
{
    public interface IBoardService
    {
        Task<List<Board>> GetBoardList();
        Task<Board> GetBoardDetail(int BNo, bool bDetail);
        Task<List<Category>> GetCategoryList();
        Task<List<Board>> GetRecentBoards(int count);
        Task<List<Comment>> GetRecentComments(int count);
    }
}
