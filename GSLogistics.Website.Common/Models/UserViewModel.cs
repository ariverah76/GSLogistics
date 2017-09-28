using GSLogistics.Model;
using GSLogistics.Website.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace GSLogistics.Website.Common.Models
{
    public class CreateUserViewModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }

    public class RoleEditModel
    {
        public IdentityRole Role { get; set; }
        public IEnumerable<ApplicationUser> Members { get; set; }
        public IEnumerable<ApplicationUser> NonMembers { get; set; }
    }
    public class RoleModificationModel
    {
        [Required]
        public string RoleName { get; set; }
        public string[] IdsToAdd { get; set; }
        public string[] IdsToDelete { get; set; }
    }

    public class ClientRoleEditModel
    {
        public string UserId { get; set; }
        public IEnumerable<SelectListItem> Customers { get; set; }
        public string SelectedCustomerId { get; set; }
        public IEnumerable<string> SelectedCustomers { get; set; }
        public IEnumerable<SelectListItem> Divisions { get; set; }
        [Required(ErrorMessage = "Please select at least one division")]
        public IEnumerable<int> SelectedDivisions { get; set; }

        public CustomerRolesForCustomer[] NewRoles { get; set; }

        public CustomerRolesForCustomer[] CurrentRoles { get; set; }
    }

    public class ClientRoleDeleteModel
    {
        public string UserId { get; set; }
        public string CustomerId { get; set; }
        public int DivisionId { get; set; }

    }

    public class CustomerRolesForCustomer
    {
        public string CustomerId { get; set; }
        public string CustomerName { get; set; }
        public int DivisionId { get; set; }
        public string DivisionName { get; set; }
    }
}
