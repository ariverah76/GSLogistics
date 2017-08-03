using GSLogistics.Interface;
using GSLogistics.UserSecurity;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Web;

namespace GSLogistics.Website.Admin.Context
{
    public partial class AdminUserContext 
    {
        public AdminUserContext(IKernel kernel)
        {
            if (Thread.CurrentPrincipal == null || Thread.CurrentPrincipal.Identity == null || !Thread.CurrentPrincipal.Identity.IsAuthenticated)
            {
                throw new Exception("Anonymous user access");
            }
            else
            {
                var claimsIdentity = Thread.CurrentPrincipal.Identity as ClaimsIdentity;

                List<Claim> roleClaims = claimsIdentity.Claims
                    .Where(c => c.Type == ClaimTypes.Role)
                    .Select(c => c).ToList();

                GSLogisticsUserContext user;


                var userID = Microsoft.AspNet.Identity.IdentityExtensions.GetUserId(Thread.CurrentPrincipal.Identity);
                if (!string.IsNullOrWhiteSpace(userID))
                {
                    user = new GSLogisticsUserContext(kernel);
                }
                else
                {
                    throw new InvalidOperationException("Invalid Identity");
                }



                _UserContext = user;
            }
        }
    }
}