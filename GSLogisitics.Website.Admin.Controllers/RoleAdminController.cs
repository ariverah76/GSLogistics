using GSLogistics.Logic.Interface;
using GSLogistics.Website.Common;
using GSLogistics.Website.Common.Controllers;
using GSLogistics.Website.Common.Models;
using GSLogistics.Website.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Ninject;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace GSLogistics.Website.Admin.Controllers
{
    public class RoleAdminController : BaseController
    {
        public RoleAdminController(IKernel kernel)
            :base(kernel)
        {

        }

        public ActionResult Index()
        {
            return View(RoleManager.Roles.ToList());
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create([Required] string name)
        {
            if (ModelState.IsValid)
            {
                IdentityResult result = await RoleManager.CreateAsync(new IdentityRole(name));

                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    AddErrorsFromResult(result);
                }
            }

            return View(name);
        }

        [HttpPost]
        public async Task<ActionResult> Delete(string id)
        {
            IdentityRole role = await RoleManager.FindByIdAsync(id);
            if (role != null)
            {
                IdentityResult result = await RoleManager.DeleteAsync(role);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return View("Error", result.Errors);
                }
            }
            else
            {
                return View("Error", new string[] { "Role Not Found" });
            }
        }



        private void AddErrorsFromResult(IdentityResult result)
        {
            foreach (string error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        [HttpGet]

        public async Task<ActionResult> EditCustomers(string id)
        {
            var model = new ClientRoleEditModel();
            using (var custLogic = Kernel.Get<ICustomerLogic>())
            using (var userLogic = Kernel.Get<IUserLogic>())
            {
                var clients = await custLogic.ToListAsync();

                Dictionary<string, string> result = new Dictionary<string, string>();
                foreach (var c in clients.OrderBy(x => x.CompanyName))
                {
                    if (!result.ContainsKey(c.CustomerId.ToString()))
                    {
                        result.Add(c.CustomerId.ToString(), c.CompanyName);
                    }
                }

                model.UserId = id;
                model.Customers = new SelectList(result, "Key", "Value", null);

                var assignedCustomers = await userLogic.GetCustomersLinkedByUser(id);
                model.SelectedCustomers = assignedCustomers;

                return View("CustomerRoleEdit", model);
            }
        }

        [HttpPost]
        public async Task<ActionResult> EditCustomers(ClientRoleEditModel model)
        {
            using (var userLogic = Kernel.Get<IUserLogic>())
            {
                var result = await userLogic.AssignCustomers(model.UserId, model.SelectedCustomers.ToList());

                if (result)
                {
                    return RedirectToAction("Index", "Admin");
                }

                return View("CustomerRoleEdit", model);
            }

        }

        public async Task<ActionResult> Edit(string id)
        {
            IdentityRole role = await RoleManager.FindByIdAsync(id);
            string[] memberIDs = role.Users.Select(x => x.UserId).ToArray();
            IEnumerable<ApplicationUser> members = UserManager.Users.Where(x => memberIDs.Any(y => y == x.Id));

            IEnumerable<ApplicationUser> nonMembers = UserManager.Users.Except(members);
            return View(new RoleEditModel
            {
                Role = role,
                Members = members,
                NonMembers = nonMembers
            });
        }

        [HttpPost]
        public async Task<ActionResult> Edit(RoleModificationModel model)
        {
            IdentityResult result;
            if (ModelState.IsValid)
            {
                foreach (string userId in model.IdsToAdd ?? new string[] { })
                {
                    result = await UserManager.AddToRoleAsync(userId, model.RoleName);
                    if (!result.Succeeded)
                    {
                        return View("Error", result.Errors);
                    }
                }
                foreach (string userId in model.IdsToDelete ?? new string[] { })
                {
                    result = await UserManager.RemoveFromRoleAsync(userId,
                    model.RoleName);
                    if (!result.Succeeded)
                    {
                        return View("Error", result.Errors);
                    }
                }
                return RedirectToAction("Index");
            }
            return View("Error", new string[] { "Role Not Found" });
        }

        #region Identity

        private ApplicationRoleManager RoleManager
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<ApplicationRoleManager>();
            }
        }

        private ApplicationUserManager UserManager
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
        }
        #endregion
    }
}
