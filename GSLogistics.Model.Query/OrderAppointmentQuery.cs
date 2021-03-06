﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSLogistics.Model.Query
{
    public class OrderAppointmentQuery
    {
        public string CustomerId { get; set; }
        public int? DivisionId { get; set; }
        public DateTime? CancelDateStartDate { get; set; }
        public DateTime? CancelDateEndDate { get; set; }
        public DateTime? ShipFor { get; set; }

        public int? Status { get; set; }
        public string PurchaseOrder { get; set; }
        public string BillOfLading { get; set; }

        public string[]  CustomerIds { get; set; }
        public int[] DivisionIds { get; set; }

        public bool EmptyBol { get; set; }

        public string MasterBillOfLading { get; set; }


        public string PtBulk { get; set; }
        public string PickTicketId { get; set; }
    }
}
