using GSLogistics.Model.Query;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSLogistics.Entities.Concrete
{
    public partial class GSLogisticsRepository
    {
        private IQueryable<GSLogistics.Entities.Appointment> OrderAppointment_BuildQuery(AppointmentQuery query)
        {
            var q = context.Appointments.Where(x => true)
              .Include("Customer");

            if (!string.IsNullOrEmpty(query.PickTicketId))
            {
                q = q.Where(x => x.CustomerId == query.PickTicketId);
            }

            if (!string.IsNullOrEmpty(query.AppointmentNumber))
            {
                q = q.Where(x => x.AppointmentNumber == query.AppointmentNumber);
            }

            if (!string.IsNullOrEmpty(query.CustomerId))
            {
                q = q.Where(x => x.CustomerId == query.CustomerId);
            }
            return q;

        }
    }
}
