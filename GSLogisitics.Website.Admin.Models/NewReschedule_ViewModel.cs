using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSLogistics.Website.Admin.Models
{
    public class NewReschedule_ViewModel
    {
        public string AppointmentNumber { get; set; }
        public string BillOfLading { get; set; }
        public DateTime ShippingDate { get; set; }
        public DateTime ShippingTime { get; set; }
        public DateTime? ShippingTimeLimit { get; set; }
        public DateTime? ReScheduleDate { get; set; }
        
    }
}
