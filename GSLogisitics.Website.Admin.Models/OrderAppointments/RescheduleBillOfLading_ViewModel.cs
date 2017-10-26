using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSLogistics.Website.Admin.Models
{
    public class RescheduleBillOfLading_ViewModel
    {
        public string BillOfLading { get; set; }
        public string  AppointmentNumber { get; set; }
        public string CustomerId { get; set; }
        public string CustomerName { get; set; }
        public int? DivisionId { get; set; }
        public string DivisionName { get; set; }
        public string SaccCode { get; set; }
        public string Carrier { get; set; }

        [Display(Name = "Shipping Date")]
        [DataType(DataType.Date)]
        [UIHint("Date")]
        public DateTime ShippingDate { get; set; }
        public DateTime ShippingTime { get; set; }
        public string ShippingTimeFriendly
        {
            get { return ShippingTime.ToShortTimeString(); }
        }

        public DateTime? ShippingTimeLimit { get; set; }

        [DisplayFormat(DataFormatString = "{0:F}", ApplyFormatInEditMode = false)]
        public int TotalOrders { get; set; }
        [DisplayFormat(DataFormatString = "{0:F}", ApplyFormatInEditMode = false)]
        public int TotalPieces { get; set; }
        public int TotalBoxes { get;set;}

    }
}
