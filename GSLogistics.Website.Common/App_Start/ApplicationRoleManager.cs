using GSLogistics.Entities;
using GSLogistics.Website.Common.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSLogistics.Website.Common
{
    public class ApplicationRoleManager : RoleManager<IdentityRole>
    {
        public ApplicationRoleManager(RoleStore<IdentityRole> store)
            : base(store)
        {

        }

        public static ApplicationRoleManager Create(IdentityFactoryOptions<ApplicationRoleManager> options, IOwinContext context)
        {
            try
            {

                ApplicationRoleManager appRoleManager = new ApplicationRoleManager(new RoleStore<IdentityRole>(context.Get<GSLogisticsContext>()));

                return appRoleManager;
            }
            catch (Exception exc)
            {
                throw exc;
            }
           
        }
    }
}
