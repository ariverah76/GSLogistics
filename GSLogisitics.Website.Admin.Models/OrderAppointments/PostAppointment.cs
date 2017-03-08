using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSLogistics.Website.Admin.Models.OrderAppointments
{
    public class ActionAppointment
    {
        public AppointmentAction Action { get; set; }
        public UpdateAppointment[] Appointments { get; set; }
    }

    public enum AppointmentAction
    {
        Post =1,
        Cancel = 2
    }
}
