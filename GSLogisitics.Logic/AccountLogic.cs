using GSLogistics.Logic.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Security;

namespace GSLogisitics.Logic
{
    public class AccountLogic : IAuthProvider
    {
        public bool Authenticate(string username, string password)
        {
            bool result = FormsAuthentication.Authenticate(username, password);

            if (result)
            {
                FormsAuthentication.SetAuthCookie(username, false);
            }

            return result;

        }


        


    }
}
