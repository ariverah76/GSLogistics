using GSLogistics.Interface;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSLogistics.UserSecurity
{
    public sealed class GSLogisticsUserContext : IUserContext
    {
        private IKernel _Kernel;


        public GSLogisticsUserContext()
        {
            this.CustomerIds = new List<string>();
            this.DivisionIds = new List<int>();
        }

        public GSLogisticsUserContext(IKernel kernel)
            :this()
        {
            _Kernel = kernel;
        }

        public List<string> CustomerIds { get; internal set; }
        string[] IUserContext.CustomerIds
        {
            get
            {
                return CustomerIds.ToArray();
            }
        }


        public List<int> DivisionIds { get; internal set; }
        int[] IUserContext.DivisionIds
        {
            get
            {
                return DivisionIds.ToArray();
            }
        }


        private int _UserId;
        

        int IUserContext.UserId
        {
            get { return _UserId; } 

            set
            {
                throw new NotImplementedException();
            }
        }
    }
}
