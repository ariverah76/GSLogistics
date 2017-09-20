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
    }
}
