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

        [Display(Name = "Shipping Time Limit")]
        [DataType(DataType.DateTime)]
        [UIHint("Time")]
        public DateTime? ShipTimeLimit { get; set; }

        public string ShippingTimeFriendly
        {
            get { return ShipTime.ToShortTimeString(); }
        }
        public string ScaccCode { get; set; }
        public string Carrier { get; set; }

        public string PurchaseOrder { get; set; }
        public int Pieces { get; set; }
        public int BoxesNumber { get; set; }
        public string Posted { get; set; }
        public DateTime DateAdded { get; set; }

        public int? DivisionId { get; set; }
        public string DivisionName { get; set; }
        public string DivisionNameId { get; set; }

        public string DivisionDisplay
        {
            get { return $"{DivisionNameId} {DivisionName}"; }
        }
        public string ShipTo { get; set; }
        public short? DeliveryTypeId { get; set; }

        public string BillOfLading { get; set; }

        public string pathPOD { get; set; }

        [DataType(DataType.Date)]
        [UIHint("Date")]
        public DateTime? ReScheduleDate { get; set; }

        public bool ExternalBol { get; set; }

        public int? Pallets { get; set; }

        public short? DriverId { get; set; }
        public short? TruckId { get; set; }



    }
}
