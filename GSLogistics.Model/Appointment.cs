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
        public string CustomerName { get; set; }
        public string ScacCode { get; set; }
        public string Carrier { get; set; }
        public DateTime? ShippingDate { get; set; }
        public DateTime? ShippingTime { get; set; }
        public bool? Posted { get; set; }
        public string Status { get; set; }
        public string AppointmentNumber { get; set; }
        public string PickTicket { get; set; }
        public string PtBulk { get; set; }
        public DateTime DateAdded { get; set; }

        public DateTime? ShippingTimeLimit { get; set; }
        public bool? Transfered { get; set; }
        public string UserName { get; set; }
        public int? DivisionId { get; set; }
        public string DivisionName { get; set; }
        public string DivisionNameId { get; set; }
        public short? DeliveryTypeId { get; set; }
        public string BillOfLading { get; set; }

        public DateTime? ReScheduleDate { get; set; }
        public bool IsReSchedule { get; set; }
        public int? Pallets { get; set; }
        public short? TruckId { get; set; }
        public short? DriverId { get; set; }
        public string TruckDescription { get; set; }
        public string DriverName { get; set; }

        public string MasterBillOfLading { get; set; }
    }
}
