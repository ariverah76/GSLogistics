using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSLogistics.Website.Admin.Models
{
    public class Appointment
    {
        public string CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string PickTicket { get; set; }
        public string PtBulk { get; set; }
        public string AppointmentNo { get; set; }
        public DateTime ShipDate { get; set; }
        public DateTime ShipTime { get; set; }
        public string SaccCode { get; set; }
        public string Carrier { get; set; }




    }
}
