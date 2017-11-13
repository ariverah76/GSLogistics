using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GSLogistics.Model;
using System.Collections;
using GSLogistics.Model.Query;
using System.Data.Entity;
using System.Transactions;

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
                q = q.Where(x => x.StartDate >= query.CancelDateStartDate.Value);
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

            if (!string.IsNullOrEmpty(query.PurchaseOrder))
            {
                q = q.Where(x => x.PurchaseOrderId == query.PurchaseOrder);
            }
            if (!string.IsNullOrEmpty(query.BillOfLading))
            {
                q = q.Where(x => x.BillOfLading == query.BillOfLading);
            }
            else if (query.EmptyBol)
            {
                q = q.Where(x => string.IsNullOrEmpty(x.BillOfLading));
            }
            
            if (!string.IsNullOrEmpty(query.MasterBillOfLading))
            {
                q = q.Where(x => x.MasterBillOfLading == query.MasterBillOfLading);
            }

            return q;

        }
        public async Task<List<Model.OrderAppointment>> ToListAsync(OrderAppointmentQuery query)
        {
            List<Entities.OrderAppointment> l = new List<OrderAppointment>();

            var transactionOptions = new System.Transactions.TransactionOptions();
            transactionOptions.IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted;

            using (var t = new TransactionScope(TransactionScopeOption.Required, transactionOptions, TransactionScopeAsyncFlowOption.Enabled))
            {
                l = await OrderAppointment_BuildQuery(query)
               .AsNoTracking()
               .ToListAsync();

                t.Complete();
                t.Dispose();
            }

           

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
                CustomerName = x.Customer != null ? x.Customer.CompanyName : string.Empty,
                DivisionName = x.Division != null ? x.Division.Description : string.Empty,
                DeliveryTypeId = string.IsNullOrEmpty(x.Delivery) ? default(short?) : x.Delivery == "P" ? (short)1 : (short)2,
                Shipping  = GetShippingDescription(x.Shipping),
                PathPOD = x.PathPOD, 
                ExternalBol = x.ExternalBol,
                ShippingDateChanged = x.ShippingDateChanged,
                MasterBillOfLading = x.MasterBillOfLading
                
            });

            return result.ToList();

        }

        public List<Model.OrderAppointment> ToList(OrderAppointmentQuery query)
        {
            List<Entities.OrderAppointment> l = new List<OrderAppointment>();
            var transactionOptions = new System.Transactions.TransactionOptions();
            transactionOptions.IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted;

            using (var t = new TransactionScope(TransactionScopeOption.Required, transactionOptions))
            {
                 l = OrderAppointment_BuildQuery(query)
                .AsNoTracking()
                .ToList();

                t.Complete();
                t.Dispose();
            }

                

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
                CustomerName = x.Customer != null ? x.Customer.CompanyName : string.Empty,
                DivisionName = x.Division != null ? x.Division.Description : string.Empty,
                DeliveryTypeId = string.IsNullOrEmpty(x.Delivery) ? default(short?) : x.Delivery == "P" ? (short)1 : (short)2,
                Shipping = GetShippingDescription(x.Shipping),
                PathPOD = x.PathPOD,
                ExternalBol = x.ExternalBol,
                ShippingDateChanged = x.ShippingDateChanged,
                MasterBillOfLading = x.MasterBillOfLading

            });

            return result.ToList();

        }

        private string GetShippingDescription(string key)
        {
            switch(key.ToUpper())
            {
                case "":
                    return "No pallet assigned";
                case "C":
                    return "Shipping Complete";
                case "P":
                    return "Shipping Partial";
                case "N":
                    return "Pallet assigned but no work in shipping";
                default:
                    return string.Empty;
            }
        }
        //public List<Model.OrderAppointment> ToList(OrderAppointmentQuery query)
        //{
        //    var l =  OrderAppointment_BuildQuery(query)
        //        .AsNoTracking()
        //        .ToList();

        //    var result = l.Select(x => new Model.OrderAppointment
        //    {
        //        BillOfLading = x.BillOfLading,
        //        BoxesCount = x.BoxesCount,
        //        BoxSize = x.BoxSize,
        //        ConfirmationNumber = x.ConfirmationNumber,
        //        CustomerId = x.CustomerId,
        //        DivisionId = x.DivisionId,
        //        EndDate = x.EndDate,
        //        Notes = x.Notes,
        //        PickTicketId = x.PickTicketId,
        //        Pieces = x.Pieces,
        //        PtBulk = x.PtBulk,
        //        PurchaseOrderId = x.PurchaseOrderId,
        //        ScacCode = x.ScacCode,
        //        ShipFor = x.ShipFor,
        //        ShipTo = x.ShipTo,
        //        Size = x.Size,
        //        StartDate = x.StartDate,
        //        Status = x.Status,
        //        Weigth = x.Weigth,
        //        CustomerName = x.Customer !=null ? x.Customer.CompanyName : "",
        //        DivisionName = x.Division !=null ? x.Division.Description : ""
        //    });

        //    return result.ToList();

        //}

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

                    if (orderAppointment.ShippingDateChanged.HasValue)
                    {
                        entity.ShippingDateChanged = orderAppointment.ShippingDateChanged.Value;
                    }

                    if (orderAppointment.ShipFor.HasValue)
                    {
                        entity.ShipFor = orderAppointment.ShipFor.Value;
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
