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
    }
}
