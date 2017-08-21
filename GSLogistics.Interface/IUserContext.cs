using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSLogistics.Interface
{
    public interface IUserContext
    {
        int UserId { get;set;}
        string UserName { get; set; }

        //bool IsClientAdmin { get; set; }

        //bool IsAdmin { get; set; }
        //bool IsDataEntry { get; set; }

        string[] CustomerIds { get; }

        int[] DivisionIds { get; }

    }
}
