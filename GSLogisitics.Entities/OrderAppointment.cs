﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSLogistics.Entities
{
    [Table("OrderAppointment", Schema = "dbo")]
    public class OrderAppointment
    {
        [Column(Order = 0)]
        public string ShipTo { get; set; }

        [Column(Order = 1), Key]
        public string PurchaseOrderId { get; set; }

        [Column(Order = 2)]
        public DateTime? StartDate { get; set; }
        [Column(Order = 3)]
        public DateTime? EndDate { get; set; }

        [Column(Order = 4), Key]
        public string PickTicketId { get; set; }

        [Column(Order = 17)]
        public string PtBulk { get; set; }

        [Column(Order = 5)]
        public int? Pieces { get; set; }

        [Column(Order = 6)]
        public decimal? Weigth { get; set; } // LBS
        [Column(Order = 7)]
        public int? BoxesCount { get; set; } //CTNS
        [Column(Order = 8)]
        public decimal? Size { get; set; } //Cubic
        [Column(Order = 9)]
        public string BoxSize { get; set; } // CTN Size
        [Column(Order = 10)]
        public DateTime? ScheduledShippingDate { get; set; }
        [Column(Order = 11)]
        public string CompanyName { get; set; }

        [Column(Order = 12), Key]
        public string CustomerId { get; set; }
        [Column(Order = 13)]
        public int? DivisionId { get; set; }
        [Column(Order = 14)]
        public string DivisionName { get; set; }
        [Column(Order = 15)]
        public string ScacCode { get; set; }

        [ForeignKey("CustomerId")]
        public virtual Customer Customer { get; set; }

        [Column(Order = 19)]
        public string ConfirmationNumber { get; set; }

        [ForeignKey("DivisionId")]
        public virtual CustomerDivision Division { get; set; }

        [Column(Order = 16)]
        public int Status { get; set; }

        [Column(Order = 18)]
        public string Notes { get; set; }

        [Column(Order = 20)]
        public DateTime? ShipFor { get; set; }

        [Column("BOL", Order = 21)]
        public string BillOfLading { get; set; }

        [Column("Delivery", Order = 22)]
        public string Delivery { get; set; }

        [Column("Shipping", Order = 23)]
        public string Shipping { get; set; }

        [Column("pathPOD", Order = 24)]
        public string PathPOD { get; set; }

        [Column("ExternalBol", Order = 25)]
        public bool ExternalBol { get; set; }

        [Column("ShippingDateChanged", Order = 26)]
        public bool ShippingDateChanged { get; set; }

        [Column("MasterBOL", Order = 27)]
        public string MasterBillOfLading { get; set; }


    }
}
