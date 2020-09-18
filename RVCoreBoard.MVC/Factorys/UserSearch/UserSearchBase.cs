using RVCoreBoard.MVC.Models;
using RVCoreBoard.MVC.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RVCoreBoard.MVC.Factorys
{
    public abstract class UserSearchBase
    {
        private readonly IUserService _userService;
        public UserSearchBase(IUserService userService)
        {
            _userService = userService;
        }

        protected IUserService UserService { get => _userService; }

        public abstract Task<List<User>> Search(string searchString);
    }
}
