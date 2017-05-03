using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSLogistics.Model.Query
{
    public class AppointmentQuery
    {
        public string PickTicketId { get; set; }
        public string CustomerId { get; set; }

        public string AppointmentNumber { get; set; }
        public string PtBulk { get; set; }
    }
}
