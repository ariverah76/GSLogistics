using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSLogistics.Model
{
    public class Appointment
    {
        public string CustomerId { get; set; }
        public string ScacCode { get; set; }
        public DateTime? ShippingDate { get; set; }
        public DateTime? ShippingTime { get; set; }
        public bool? Posted { get; set; }
        public string Status { get; set; }
        public string AppointmentNumber { get; set; }
        public string PickTicket { get; set; }
        public string PtBulk { get; set; }
    }
}
