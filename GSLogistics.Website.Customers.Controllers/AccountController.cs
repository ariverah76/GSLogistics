using GSLogistics.Logic.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web;
using GSLogistics.Website.Customers.Models;
using Microsoft.Owin.Security;
using GSLogistics.Website.Common;
using Microsoft.AspNet.Identity.Owin;
using GSLogistics.Website.Models;
using Microsoft.AspNet.Identity;
using System.Security.Claims;

namespace GSLogistics.Website.Customers.Controllers
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

                    return Redirect(string.IsNullOrEmpty(returnUrl) ? "/Home/Index" : returnUrl);
                }
              
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);

        }


        [Authorize]
        public ActionResult LogOff()
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
