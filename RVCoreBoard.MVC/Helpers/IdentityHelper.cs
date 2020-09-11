using System;
using System.Security.Claims;
using System.Security.Principal;

namespace RVCoreBoard.MVC.Helpers
{
    public static class IdentityHelper
    {
        public static int GetSid(this IIdentity identity)
        {
            ClaimsIdentity claimsIdentity = identity as ClaimsIdentity;
            Claim claim = claimsIdentity?.FindFirst(ClaimTypes.Sid);

            if (claim == null)
                return 0;

            return int.Parse(claim.Value);
        }
        public static string GetRole(this IIdentity identity)
        {
            ClaimsIdentity claimsIdentity = identity as ClaimsIdentity;
            Claim claim = claimsIdentity?.FindFirst(ClaimTypes.Role);

            if (claim == null)
                return string.Empty;

            return claim.Value;
        }
        public static int GetRoleLevel(this IIdentity identity)
        {
            ClaimsIdentity claimsIdentity = identity as ClaimsIdentity;
            Claim claim = claimsIdentity?.FindFirst(ClaimTypes.Role);

            if (claim == null)
                return 0;

            int value = (int)Enum.Parse(typeof(Models.User.UserLevel), claim.Value);

            return value;
        }
    }
}
