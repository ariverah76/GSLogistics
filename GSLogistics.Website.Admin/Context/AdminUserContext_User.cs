using GSLogistics.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GSLogistics.Website.Admin.Context
{
    public partial class AdminUserContext : IUserContext
    {

        private IUserContext _UserContext;
        string[] IUserContext.CustomerIds
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        int[] IUserContext.DivisionIds
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        int IUserContext.UserId
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }
    }
}