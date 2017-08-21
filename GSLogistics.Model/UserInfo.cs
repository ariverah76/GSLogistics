using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSLogistics.Model
{
    public class UserInfo
    {

        public UserInfo()
        {
            UserCustomers = new List<UserCustomer>();
        }

        public int UserId { get; set; }
        public string  Description { get; set; }
        public string UserName { get; set; }
        public List<UserCustomer> UserCustomers { get; set; }
    }

    public class UserCustomer
    {
        public int UserId { get; set; }
        public string CustomerId { get; set; }
        public int DivisionId { get; set; }
    }
}
