using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject;

using GSLogistics.Logic.Interface;
using GSLogistics.Entities;
using GSLogistics.Website.Common.Models;

namespace GSLogistics.Logic
{
    public class UserLogic : LogicBase, IUserLogic
    {
        public UserLogic(IKernel kernel) : base(kernel)
        {
        }

        public async Task<IEnumerable<string>> GetCustomersLinkedByUser(string userId)
        {

            return await  Repository.GetCustomersForUser(userId);
        }

        public async Task<bool> AssignCustomers(string userId, List<string> customerIds)
        {
            return await Repository.AssignCustomers(userId, customerIds);
        }

        public async Task<bool> AssignDivisions(string userId, Dictionary<int, string> customerDivisions)
        {
            return await Repository.AssignDivisions(userId, customerDivisions);
        }

        public async Task<IEnumerable<int>> GetDivisionsForUser(string userId)
        {
            return await Repository.GetDivisionsForUser(userId);
        }

        public async Task<Model.UserInfo> CreateAsync(string userName, string description)
        {
            var result = await Repository.CreateUserInfoAsync(userName, description);

            return new Model.UserInfo() { Description = result.Description, UserName = result.UserName };
        }

        public async Task<List<CustomerRolesForCustomer>> GetUserCustomers(string userId)
        {
            List<CustomerRolesForCustomer> returnValue = new List<CustomerRolesForCustomer>();
            var result = await Repository.GetUserCustomers(userId);

            var custIds = result.Select(x => x.CustomerId.Trim()).ToArray();

            var cust = await Repository.ToListAsync(new Model.Query.CustomerQuery() { CustomerIds = custIds });

            var divs = await Repository.GetDivisionsByCustomerIds(custIds);

            foreach(var x in result)
            {
                var customer = cust.FirstOrDefault(c => c.CustomerId.Trim() == x.CustomerId.Trim());
                var division = divs.FirstOrDefault(d => d.DivisionId == x.DivisionId);
                returnValue.Add(new CustomerRolesForCustomer() { CustomerId = x.CustomerId, CustomerName = customer?.CompanyName, DivisionId = x.DivisionId, DivisionName = division?.Description });
            }

            return returnValue;
        }

        public async Task<bool> DeleteCustomerRole(string userId, string customerId, int divisionId)
        {
            return await Repository.DeleteCustomerRole(userId, customerId, divisionId);
        }

        public async Task<Model.UserInfo> GetUserInfo(string userId)
        {
            var result = await Repository.UserInfo_FirstOrDefaultAsync(userId);

            
            return result != null ? new Model.UserInfo() { UserId = result.UserId, Description = result.Description, UserName = result.UserName } : null ;
        }

    }
}
