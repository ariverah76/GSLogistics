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
              .Include("Customer")
              .Include("Division");

            if (!string.IsNullOrEmpty(query.CustomerId))
            {
                q = q.Where(x => x.CustomerId == query.CustomerId);
            }

            if (query.DivisionId.HasValue && query.DivisionId != 0 )
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
        public async Task<List<Model.OrderAppointment>> ToListAsync(OrderAppointmentQuery query)
        {
            var l = await OrderAppointment_BuildQuery(query)
                .AsNoTracking()
                .ToListAsync();

            var result = l.Select(x => new Model.OrderAppointment
            {
                BillOfLading = x.BillOfLading,
                BoxesCount = x.BoxesCount,
                BoxSize = x.BoxSize,
                ConfirmationNumber = x.ConfirmationNumber,
                CustomerId = x.CustomerId,
                DivisionId = x.DivisionId,
                EndDate = x.EndDate,
                Notes = x.Notes,
                PickTicketId = x.PickTicketId,
                Pieces = x.Pieces,
                PtBulk = x.PtBulk,
                PurchaseOrderId = x.PurchaseOrderId,
                ScacCode = x.ScacCode,
                ShipFor = x.ShipFor,
                ShipTo = x.ShipTo,
                Size = x.Size,
                StartDate = x.StartDate,
                Status = x.Status,
                Weigth = x.Weigth,
                CustomerName = x.Customer.CompanyName,
                DivisionName = x.Division.Description
            });

            return result.ToList();

        }
        public List<Model.OrderAppointment> ToList(OrderAppointmentQuery query)
        {
            var l =  OrderAppointment_BuildQuery(query)
                .AsNoTracking()
                .ToList();

            var result = l.Select(x => new Model.OrderAppointment
            {
                BillOfLading = x.BillOfLading,
                BoxesCount = x.BoxesCount,
                BoxSize = x.BoxSize,
                ConfirmationNumber = x.ConfirmationNumber,
                CustomerId = x.CustomerId,
                DivisionId = x.DivisionId,
                EndDate = x.EndDate,
                Notes = x.Notes,
                PickTicketId = x.PickTicketId,
                Pieces = x.Pieces,
                PtBulk = x.PtBulk,
                PurchaseOrderId = x.PurchaseOrderId,
                ScacCode = x.ScacCode,
                ShipFor = x.ShipFor,
                ShipTo = x.ShipTo,
                Size = x.Size,
                StartDate = x.StartDate,
                Status = x.Status,
                Weigth = x.Weigth,
                CustomerName = x.Customer !=null ? x.Customer.CompanyName : "",
                DivisionName = x.Division !=null ? x.Division.Description : ""
            });

            return result.ToList();

        }

        public async Task<int> Update(Model.OrderAppointment orderAppointment)
        {

            try
            {
                var ptBulk = string.IsNullOrEmpty(orderAppointment.PtBulk) ? "" : orderAppointment.PtBulk;
                var entity = context.OrderAppointments.Where(x => x.PurchaseOrderId == orderAppointment.PurchaseOrderId && x.PickTicketId == orderAppointment.PickTicketId && x.CustomerId == orderAppointment.CustomerId && x.PtBulk == ptBulk).FirstOrDefault();

                if (entity != null)
                {

                    entity.Status = orderAppointment.Status;

                    if (!string.IsNullOrEmpty(orderAppointment.Notes))
                    {
                        entity.Notes = orderAppointment.Notes;
                    }

                    if (!string.IsNullOrEmpty(orderAppointment.ConfirmationNumber))
                    {
                        entity.ConfirmationNumber = orderAppointment.ConfirmationNumber;
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
