using GSLogistics.Logic.Interface;
using Ninject;

namespace GSLogistics.Logic
{
    public partial class CustomerLogic : LogicBase, ICustomerLogic
    {
        public CustomerLogic(IKernel kernel)
            : base(kernel)
        {

        }

        
    }
}
