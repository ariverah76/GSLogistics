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

        [Column(Order =1), Key]
        public DateTime DateAdd { get; set; }

        [Column(Order = 2), Key]
        public string PickTicket { get; set; }
        [Column(Order = 3), Key]
        public string PtBulk { get; set; }

        [Column("AppointmentNo", Order = 4), Key]
        public string AppointmentNumber { get; set; }

        [Column("ShippDate", Order = 5)]
        public DateTime ShipDate { get; set; }
        [Column("ShippTime", Order = 6)]
        public DateTime ShipTime { get; set; }

        [Column("ScaccCode", Order = 7)]
        public string ScacCode { get; set; }

        [Column(Order = 8)]
        public string Status { get; set; }

        [Column("Transferred", Order = 9 )]
        public bool Transferred { get; set; }

        [Column("Posted", Order = 10 )]
        public bool Posted { get; set; }

        [ForeignKey("ScacCode")]
        public virtual ScacCode CatScacCode { get; set; }

        [ForeignKey("CustomerId")]
        public virtual Customer Customer { get; set; }

    }
}
