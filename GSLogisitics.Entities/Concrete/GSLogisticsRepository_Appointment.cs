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
        private IQueryable<GSLogistics.Entities.Appointment> Appointment_BuildQuery(AppointmentQuery query)
        {
            var q = context.Appointments.Where(x => true)
              .Include("Customer");

            if (!string.IsNullOrEmpty(query.PickTicketId))
            {
                q = q.Where(x => x.PickTicket == query.PickTicketId);
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

        public async Task<List<Model.Appointment>> ToListAsync(AppointmentQuery query)
        {
            var l = await Appointment_BuildQuery(query)
                .AsNoTracking()
                .ToListAsync();

            var result = l.Select(x => new Model.Appointment
            {
                AppointmentNumber = x.AppointmentNumber,
                CustomerId = x.CustomerId,
                DateAdded = x.DateAdd,
                PickTicket = x.PickTicket,
                Posted = x.Posted,
                PtBulk = x.PtBulk,
                ScacCode = x.ScacCode,
                ShippingDate = x.ShipDate,
                ShippingTime = x.ShipTime,
                ShippingTimeLimit = x.ShippingTimeLimit,
                Status = x.Status,
                Transfered = x.Transferred,
                UserName = x.UserName,
                Carrier = x.CatScacCode.ScacCodeName

            });

            return result.ToList();

        }

        public async Task<int> Update(Model.Appointment appointment)
        {
            try
            {
                var entity = context.Appointments.Where(x => x.AppointmentNumber == appointment.AppointmentNumber && x.CustomerId == appointment.CustomerId && x.PickTicket == appointment.PickTicket).FirstOrDefault();

                if (entity != null)
                {
                    if (appointment.Posted.HasValue)
                    {
                        entity.Posted = appointment.Posted.Value;
                    }
                    if (!string.IsNullOrEmpty(appointment.Status))
                    {
                        entity.Status = appointment.Status;
                    }

                    //if (!string.IsNullOrEmpty(appointment.AppointmentNumber))
                    //{
                    //    entity.AppointmentNumber = appointment.AppointmentNumber;
                    //}
                    if (!string.IsNullOrEmpty(appointment.ScacCode))
                    {
                        entity.ScacCode = appointment.ScacCode;
                    }
                    if (appointment.ShippingDate.HasValue)
                    {
                        entity.ShipDate = appointment.ShippingDate.Value;
                    }
                    if (appointment.ShippingTime.HasValue)
                    {
                        entity.ShipTime = appointment.ShippingTime.Value;
                    }


                    return await context.SaveChangesAsync();
                }

            }
            catch (Exception exc)
            {
                throw exc;
            }
            return 0;
        }
    }
}
