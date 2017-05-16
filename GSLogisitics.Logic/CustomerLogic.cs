using GSLogistics.Logic.Interface;
using GSLogistics.Model;
using Ninject;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GSLogistics.Logic
{
    public partial class CustomerLogic : LogicBase, ICustomerLogic
    {
        public CustomerLogic(IKernel kernel)
            : base(kernel)
        {


        }

        public async Task<List<Customer>> ToListAsync()
        {
            return await Repository.ToListAsync();
        }

        
    }
}
