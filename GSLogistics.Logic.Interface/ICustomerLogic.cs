using GSLogistics.Model;
using GSLogistics.Model.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSLogistics.Logic.Interface
{
    public interface ICustomerLogic : IGSLogisticsLogic
    {

        Task<List<Customer>> ToListAsync();
        Task<List<Model.Customer>> ToListAsync(CustomerQuery query);
        Task<Customer> FirstOrDefaultAsync(string identifier);
    }
}
