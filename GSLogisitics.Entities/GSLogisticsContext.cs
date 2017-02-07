﻿using GSLogistics.Website.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace GSLogistics.Website.Models
{
    public class ApplicationUser : IdentityUser
    {

    }
}

namespace GSLogistics.Entities
{
    public class GSLogisticsContext : IdentityDbContext<ApplicationUser>
    {

      

        public GSLogisticsContext() :
            base("GSLogisiticsWebEntities")
        {

        }

        public static GSLogisticsContext Create()
        {
            return new GSLogisticsContext();
        }


        public DbSet<OrderAppointment> OrderAppointments { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<CustomerDivision> CustomerDivisions { get; set; }
        public DbSet<ScacCode> ScacCodes { get; set; }
        public DbSet<Appointment> Appointments { get; set; }

    }

    public class IdentityDbInit : DropCreateDatabaseIfModelChanges<GSLogisticsContext>
    {
        protected override void Seed(GSLogisticsContext context)
        {
            PerformInitialSetup(context);
            base.Seed(context);
        }

        public void PerformInitialSetup(GSLogisticsContext context)
        {
          //  ApplicationUserManager
        }
    }
}