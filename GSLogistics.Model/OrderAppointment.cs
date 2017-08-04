using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSLogistics.Model
{
    public class OrderAppointment 
    {
        public string ShipTo { get; set; }
        public string PurchaseOrderId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string PickTicketId { get; set; }
        public string PtBulk { get; set; }
        public int? Pieces { get; set; }
        public decimal? Weigth { get; set; } // LBS
        public int? BoxesCount { get; set; } //CTNS
        public decimal? Size { get; set; } //Cubic
        public string BoxSize { get; set; } // CTN Size
        public string CustomerId { get; set; }
        public int? DivisionId { get; set; }
        public string DivisionName { get; set; }
        public string ScacCode { get; set; }
        public string ConfirmationNumber { get; set; }
        public int Status { get; set; }
        public string Notes { get; set; }
        public DateTime? ShipFor { get; set; }
        public string BillOfLading { get; set; }
        public string CustomerName { get; set; }
        public short? DeliveryTypeId { get; set; }
        public string Shipping { get; set; }

    }
}
