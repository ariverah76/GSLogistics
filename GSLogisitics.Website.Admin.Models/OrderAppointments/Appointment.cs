using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

        [Display(Name = "Shipping Date")]
        [DataType(DataType.Date)]
        [UIHint("Date")]
        public DateTime ShipDate { get; set; }
        [Display(Name = "Shipping Time")]
        [DataType(DataType.DateTime)]
        [UIHint("Time")]
        public DateTime ShipTime { get; set; }

        public string ShippingTimeFriendly
        {
            get { return ShipTime.ToShortTimeString(); }
        }
        public string SaccCode { get; set; }
        public string Carrier { get; set; }

        public string PurchaseOrder { get; set; }
        public int Pieces { get; set; }
        public int BoxesNumber { get; set; } 




    }
}
