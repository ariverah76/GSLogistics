using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSLogistics.Website.Admin.Models
{
    public class OrderAppointment
    {
        public string CustomerId { get; set; }
        public string CustomerName { get; set; }
        public int StoreId { get; set; }
        public string StoreName { get; set; }
        public string PurchaseOrderId { get; set; }
        public string PickTicketId { get; set; }
        public int Pieces { get; set; }
        public decimal? Weight { get; set; } // LBS
        public int BoxesNumber { get; set; } //CTNS
        public decimal Volume { get; set; } //Cubic
        public string BoxSize { get; set; } // CTN Size

        [Display(Name = "Start Date")]
        [DataType(DataType.Date)]
        [UIHint("Date")]
        public DateTime StartDate { get; set; }

        [Display(Name = "End Date")]
        [DataType(DataType.Date)]
        [UIHint("Date")]
        public DateTime EndDate { get; set; }


       
        public string Status { get; set; }

        [Display(Name = "CREATE DATE")]
        [DataType(DataType.Date)]
        [UIHint("Date")]
        public DateTime? EstimatedShippingDate { get; set; }

        public string PtBulk { get; set; }

        public string DivisionName { get; set; }
        public string Notes { get; set; }
        public string ConfirmationNumber { get; set; }

        public int? DivisionId { get; set; }

        public DateTime? ShipFor { get; set; }

        public string BillOfLading { get; set; }

        public string ScacCode { get; set; }
        public short? DeliveryTypeId { get; set; }

    }
}
