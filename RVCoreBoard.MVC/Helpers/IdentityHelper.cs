using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

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
    }
}
