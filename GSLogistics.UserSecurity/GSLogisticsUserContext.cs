using GSLogistics.Entities;
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

        public GSLogisticsUserContext(IKernel kernel, string userName)
            :this(kernel)
        {
            var clientInfo = GenerateClientUserInfo(userName);

            CustomerIds = clientInfo.CustomerIds.ToList();
            DivisionIds = clientInfo.DivisionIds.ToList();

            UserName = clientInfo.UserName;


           // Session["ClientUserContext"] = clientInfo;
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

        public string UserName
        {
            get;

            set;
        }

        private ClientUserInfo GenerateClientUserInfo(string userName)
        {

            using (var context = new GSLogisticsContext())
            {
                var userInfo = new ClientUserInfo();
                var user = context.UserInfos.Where(x => x.UserName == userName).FirstOrDefault();

                if (user != null)
                {
                    userInfo.UserName = user.UserName;
                    
                    if (user.UserCustomers != null  && user.UserCustomers.Any())
                    {
                        List<string> clientIds = new List<string>();
                        List<int> divisionIds = new List<int>();

                        clientIds = user.UserCustomers.Select(x => x.CustomerId).Distinct().ToList();
                        divisionIds = user.UserCustomers.Select(x => x.DivisionId).Distinct().ToList();

                        userInfo.CustomerIds = clientIds.ToArray();
                        userInfo.DivisionIds = divisionIds.ToArray();

                    }
                }
                return userInfo;
            }
        }

        private class ClientUserInfo
        {
            public string UserName { get; set; }
            public string[] CustomerIds { get; set; }
            public int[] DivisionIds { get; set; }
        }
    }
}
