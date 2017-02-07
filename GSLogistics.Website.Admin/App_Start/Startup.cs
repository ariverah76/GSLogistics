using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using GSLogistics.Entities;
using GSLogistics.Website.Common;
using Microsoft.AspNet.Identity;

namespace GSLogistics.Website.Admin
{
    public partial class Startup
    {
        public void ConfigureAuth(IAppBuilder app)
        {
            app.CreatePerOwinContext(

                () =>
                {
                    return new GSLogisticsContext();
                }
                );

            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);
            app.CreatePerOwinContext<ApplicationRoleManager>(ApplicationRoleManager.Create);


            app.UseCookieAuthentication(new Microsoft.Owin.Security.Cookies.CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login")
            });
        
        }
    }
}
