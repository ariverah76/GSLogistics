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

        public bool? Posted { get; set; }

        public DateTime? ShippingDateStart { get; set; }
        public DateTime? ShippingDateEnd { get; set; }
        public string Status { get; set; }
        public DateTime? ShippingDate { get; set; }
        public short? DeliveryTypeId { get; set; }

        public int? DivisionId { get; set; }
    }
}
