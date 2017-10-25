using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSLogistics.Website.Admin.Models.OrderAppointments
{
    public class AppointmentsForReschedule_Model
    {
        public string BillOfLading { get; set; }
        public string AppointmentNumber { get; set; }
        public string PickTicketId { get; set; }
        public string CustomerId { get; set; }
        public string CustomerName { get; set; }
        public DateTime ShippingDate { get; set; }
        public DateTime ShippingTime { get; set; }
        public int? DivisionId { get; set; }
        public string DivisionName { get; set; }

    }
}
