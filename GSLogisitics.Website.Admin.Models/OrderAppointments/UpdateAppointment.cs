using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSLogistics.Website.Admin.Models.OrderAppointments
{
    public class UpdateAppointment
    {
        public string CustomerId { get; set; }
        public string PickTicket { get; set; }
        public string AppointmentNo { get; set; }
        public bool? Posted { get; set; }
        public string Status { get; set; }
    }
}
