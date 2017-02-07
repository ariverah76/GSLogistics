using GSLogistics.Logic.Interface;
using GSLogistics.Website.Admin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Microsoft.Owin.Security;
using System.Security.Claims;
using GSLogistics.Website.Models;
using Microsoft.AspNet.Identity;
using GSLogistics.Website.Common;
using System.Web;
using Microsoft.AspNet.Identity.Owin;

namespace GSLogistics.Website.Admin.Controllers
{

    [Authorize]
    public class AccountController : Controller
    {
        IAuthProvider authProvider;

        public AccountController(IAuthProvider auth)
        {
            authProvider = auth;
        }

        [AllowAnonymous]
        public ViewResult Login()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                return View("Error", new string[] { "Access Denied" });
            }
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {

                ApplicationUser user = await UserManager.FindAsync(model.UserName, model.Password);

                if (user == null)
                {
                    ModelState.AddModelError("", "Invalid user name or password.");
                }
                else
                {
                    ClaimsIdentity identity = await UserManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
                    AuthManager.SignOut();
                    AuthManager.SignIn(new AuthenticationProperties
                    {
                        IsPersistent = false
                    }, identity);

                    return Redirect(string.IsNullOrEmpty(returnUrl) ? "/OrderAppointment/List" : returnUrl);
                }
                //if (authProvider.Authenticate(model.UserName, model.Password))
                //{
                //    return Redirect(returnUrl ?? Url.Action("List", "OrderAppointment"));
                //}
                //else
                //{
                //    ModelState.AddModelError("", "Incorrect user name or password");
                //    return View();
                //}
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);

        }

        [Authorize]
        public ActionResult Logout()
        {
            AuthManager.SignOut();
            return RedirectToAction("Login", "Account");
        }


        private IAuthenticationManager AuthManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private ApplicationUserManager UserManager
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
        }
    }
}
