using Microsoft.Extensions.Logging.Abstractions;
using RVCoreBoard.MVC.Models;
using RVCoreBoard.MVC.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RVCoreBoard.MVC.Factorys
{
    internal class Level : UserSearchBase
    {
        public Level(IUserService userService) : base(userService)
        {
        }

        public override async Task<List<User>> Search(string searchString)
        {
            return await base.UserService.GetUserList(s => s.Level == int.Parse(searchString));
        }
    }
}
