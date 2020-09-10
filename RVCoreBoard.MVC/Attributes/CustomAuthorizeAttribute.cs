using Microsoft.AspNetCore.Authorization;
using System;
using static RVCoreBoard.MVC.Models.User;

namespace RVCoreBoard.MVC.Attributes
{
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {

        private UserLevel roleEnum;
        public UserLevel RoleEnum
        {
            get { return roleEnum; }
            set { roleEnum = value; base.Roles = value.ToString(); }
        }
    }
}
