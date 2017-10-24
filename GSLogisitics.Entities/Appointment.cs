using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSLogistics.Entities
{
    [Table("Appointments", Schema = "dbo")]
    public class Appointment
    {

        public Appointment()
        {
            Status = "A";
            Transferred = false;
        }

        [Column(Order =0), Key]
        public string CustomerId{ get; set; }

        [Column(Order =1)]
        public DateTime DateAdd { get; set; }

        [Column("PickTicket", Order = 2), Key]
        public string PickTicketId { get; set; }
        [Column(Order = 3)]
        public string PtBulk { get; set; }

        [Column("AppointmentNo", Order = 4)]
        public string AppointmentNumber { get; set; }

        //[Column("ShippDate", Order = 5)]
        [NotMapped]
        public DateTime ShipDate { get; set; }

        [Column("ShippTime", Order = 6)]
        public DateTime ShipTime { get; set; }

        [Column("ShippingTimeLimit", Order =12)]
        public DateTime? ShippingTimeLimit { get; set; }

        [Column("ScaccCode", Order = 7)]
        public string ScacCode { get; set; }

        [Column(Order = 8)]
        public string Status { get; set; }

        [Column("Transferred", Order = 9 )]
        public bool Transferred { get; set; }

        [Column("Posted", Order = 10 )]
        public bool Posted { get; set; }

        [Column("DivisionId", Order = 11)]
        public int? DivisionId { get; set; }

        [Column("DeliveryTypeId", Order = 13)]
        public short? DeliveryTypeId { get; set; }

        [Column("UserName", Order = 14)]
        public string UserName { get; set; }

        [Column("ReScheduleDate", Order = 15)]
        public DateTime? ReScheduleDate { get; set; }

        [Column("IsReSchedule", Order = 16)]
        public bool IsReSchedule { get; set; }

        [Column("Pallets", Order = 17)]
        public int? Pallets { get; set; }

        [Column("TruckNo", Order = 18)]
        public short? TruckId { get; set; }

        [Column("DriverNo", Order =19)]
        public short? DriverId { get; set; }

        [ForeignKey("TruckId")]
        public virtual Truck Truck { get; set; }
        [ForeignKey("DriverId")]
        public virtual Driver Driver { get; set; }

        [ForeignKey("ScacCode")]
        public virtual ScacCode CatScacCode { get; set; }

        [ForeignKey("CustomerId")]
        public virtual Customer Customer { get; set; }

        [ForeignKey("DivisionId")]
        public virtual CustomerDivision Division { get; set; }


    }
}
