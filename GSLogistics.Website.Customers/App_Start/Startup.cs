using GSLogistics.Entities;
using GSLogistics.Website.Common;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Owin;


namespace GSLogistics.Website.Customers
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