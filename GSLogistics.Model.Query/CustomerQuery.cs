using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSLogistics.Model.Query
{
    public class CustomerQuery
    {
        public string CustomerId { get; set; }
        public string CustomerName { get; set; }

        public string[] CustomerIds { get; set; }
    }
}
