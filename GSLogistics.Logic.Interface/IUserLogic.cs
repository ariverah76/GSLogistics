using GSLogistics.Model;
using GSLogistics.Website.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSLogistics.Logic.Interface
{
    public interface IUserLogic : IGSLogisticsLogic
    {
        Task<IEnumerable<string>> GetCustomersLinkedByUser(string userId);
        Task<bool> AssignCustomers(string userId, List<string> customerIds);
        Task<bool> AssignDivisions(string userId, Dictionary<int, string> divisionIds);
        Task<IEnumerable<int>> GetDivisionsForUser(string userId);
        Task<UserInfo> CreateAsync(string userName, string description);
        Task<List<CustomerRolesForCustomer>> GetUserCustomers(string userId);

        Task<bool> DeleteCustomerRole(string userId, string customerId, int divisionId);

        Task<Model.UserInfo> GetUserInfo(string userId);
    }
}
