using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSLogistics.Website.Admin.Models.OrderAppointments
{
    public class OrderStatus
    {
        public string CustomerID { get; set; }
        public string PickTicket { get; set; }
        public string PO { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int TotalPcs { get; set; }
        public string Picked { get; set; }
        public string PickedFull { get; set; }
        public string SpecialWork { get; set; }
        public string AssignCarton { get; set; }
        public string Packing { get; set; }
        public string PackingFull { get; set; }
        public double LbsShipping { get; set; }
        public string Carton { get; set; }
        public int TotalCtns { get; set; }
        public double Cubic { get; set; }

    }
}
