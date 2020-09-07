using RVCoreBoard.MVC.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RVCoreBoard.MVC.Services
{
    public interface IBoardService
    {
        Task<List<Board>> GetList();
        Task<Board> GetDetail(int BNo, bool bDetail);
    }
}
