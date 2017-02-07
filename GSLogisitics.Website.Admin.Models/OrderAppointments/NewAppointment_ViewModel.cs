using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSLogistics.Website.Admin.Models.OrderAppointments
{
    public class NewAppointment_ViewModel
    {

        public string AppointmentNumber { get; set; }
        public DateTime ShippingDate { get; set; }
        public DateTime ShippingTime { get; set; }
        public string ScacCode { get; set; }

        public OrderForAppointment[] Orders { get; set; }
    }

    public class OrderForAppointment
    {
        public string PurchaseOrderId { get; set; }
        public  string PickTicketId{ get; set; }
        public string PtBulk { get; set; }
        public string CustomerId { get; set; }

    }
}
