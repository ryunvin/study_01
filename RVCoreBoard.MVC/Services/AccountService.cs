namespace RVCoreBoard.MVC.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using RVCoreBoard.MVC.DataContext;
    using RVCoreBoard.MVC.Models;
    using Microsoft.EntityFrameworkCore;

    public class AccountService : IAccountService
    {
        private readonly RVCoreBoardDBContext _db;

        public AccountService(RVCoreBoardDBContext db)
        {
            _db = db;
        }

        public async Task<User> Login(LoginModel model)
        {
            var user = await _db.Users
                            .FirstOrDefaultAsync(u => u.Id.Equals(model.Id) && u.Password.Equals(model.Password));
            
            return user;
        }

        public async Task<bool> Register(User user)
        {
            await _db.Users.AddAsync(user);
            if (await _db.SaveChangesAsync() > 0)
            {
                return true;
            }
            return false;
        }
    }
}
