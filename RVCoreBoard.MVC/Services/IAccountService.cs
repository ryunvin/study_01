using RVCoreBoard.MVC.Models;
using System.Threading.Tasks;

namespace RVCoreBoard.MVC.Services
{
    public interface IAccountService
    {
        Task<User> Login(LoginModel model);
        Task<bool> Register(User user);
    }
}
