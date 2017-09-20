using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject;

using GSLogistics.Logic.Interface;

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
    }
}
