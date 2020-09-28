using RVCoreBoard.MVC.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RVCoreBoard.MVC.Services
{
    public interface IUserService
    {
        Task<User> Login(LoginModel model);
        Task<bool> Register(User user);
        Task<List<User>> GetUserList(System.Linq.Expressions.Expression<Func<User, bool>> predicate);
        Task<User> FindUser(string id);
    }
}
