using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GSLogistics.Model;
using System.Collections;
using GSLogistics.Model.Query;
using System.Data.Entity;

namespace GSLogistics.Entities.Concrete
{
    public partial class GSLogisticsRepository
    {
        private IQueryable<GSLogistics.Entities.OrderAppointment> OrderAppointment_BuildQuery(OrderAppointmentQuery query)
        {
            var q = context.OrderAppointments.Where(x => true)
              .Include("Customer");

            if (!string.IsNullOrEmpty(query.CustomerId))
            {
                q = q.Where(x => x.CustomerId == query.CustomerId);
            }

            if (query.DivisionId.HasValue )
            {
                q = q.Where(x => x.DivisionId == query.DivisionId);
            }

            if (query.CancelDateStartDate.HasValue)
            {
                q = q.Where(x => x.EndDate >= query.CancelDateStartDate.Value);
            }
            if (query.CancelDateEndDate.HasValue)
            {
                q = q.Where(x => x.StartDate <= query.CancelDateEndDate.Value);
            }

            if (query.ShipFor.HasValue)
            {
                q = q.Where(x => x.ShipFor.HasValue && (x.ShipFor.Value.Year == query.ShipFor.Value.Year && x.ShipFor.Value.Month == query.ShipFor.Value.Month && x.ShipFor.Value.Day == query.ShipFor.Value.Day));
            }

            if (query.Status.HasValue)
            {
                q = q.Where(x => x.Status == query.Status);
            }

            return q;

        }
        //public async Task<IList<OrderAppointment>> ToListAsync(OrderAppointmentQuery query)
        //{
        //    var l = await OrderAppointment_BuildQuery(query)
        //        .AsNoTracking()
        //        .ToListAsync();

        //    var result = l.Select( x => new Model.OrderAppointment )

        //} 
    }
}
