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
        private IQueryable<dynamic> Appointment_BuildQuery(AppointmentQuery query)
        {
            //var q = context.Appointments.Where(x => true)
            //  .Include("Customer")
            //  .Include("Division");

            var q = (from a in context.Appointments
                         join orders in context.OrderAppointments on new { a.CustomerId, a.PickTicketId } equals new { orders.CustomerId , orders.PickTicketId }
                         join customer in context.Customers on a.CustomerId equals customer.CustomerId
                         join division in context.CustomerDivisions on a.DivisionId equals division.DivisionId
                         into y
                         from z in y.DefaultIfEmpty()
                         select new { a.PickTicketId, a.Pallets, a.IsReSchedule, a.Posted, a.AppointmentNumber, a.CustomerId,a.DivisionId, a.UserName, a.PtBulk, a.DeliveryTypeId, a.DateAdd,  a.ReScheduleDate, a.ScacCode, a.ShippingTimeLimit, a.ShipTime, a.Status, a.Transferred, ShipDate = orders.ShipFor, DivisionName = z != null? z.Description : string.Empty, DivisionNameId = z != null ? z.NameId : string.Empty, Customer = customer, a.CatScacCode, a.DriverId, a.TruckId, a.Driver, a.Truck, BillOfLading = orders.BillOfLading, PurchaseOrder = orders.PurchaseOrderId, MasterBillOfLading = orders.MasterBillOfLading});



            if (string.IsNullOrEmpty(query.KeySearch))
            {

                if (!string.IsNullOrEmpty(query.PickTicketId))
                {
                    q = q.Where(x => x.PickTicketId == query.PickTicketId);
                    // anotherq = anotherq.Where(x => x.PickTicketId == query.PickTicketId);
                }
                else if (query.PickTicketsIds != null && query.PickTicketsIds.Any())
                {
                    q = q.Where(x => query.PickTicketsIds.Contains(x.PickTicketId));
                    // anotherq = anotherq.Where(x => query.PickTicketsIds.Contains(x.PickTicketId));
                }


                if (!string.IsNullOrEmpty(query.AppointmentNumber))
                {
                    q = q.Where(x => x.AppointmentNumber == query.AppointmentNumber);
                    //anotherq = anotherq.Where(x => x.AppointmentNumber == query.AppointmentNumber);
                }
            }
            else
            {
                q = q.Where(x => x.PickTicketId == query.KeySearch || x.AppointmentNumber == query.KeySearch || x.PurchaseOrder == query.KeySearch);
            }


            if (!string.IsNullOrEmpty(query.CustomerId))
            {
                q = q.Where(x => x.CustomerId == query.CustomerId);
                //anotherq = anotherq.Where(x => x.CustomerId == query.CustomerId);
            }
            else if (query.CustomerIds != null && query.CustomerIds.Any())
            {
                q = q.Where(x => query.CustomerIds.Contains(x.CustomerId));
                //anotherq = anotherq.Where(x => query.CustomerIds.Contains(x.CustomerId));
            }


            if (query.Posted.HasValue)
            {
                q = q.Where(x => x.Posted == query.Posted.Value);
               // anotherq = anotherq.Where(x => x.Posted == query.Posted.Value);
            }

            if (query.ShippingDateStart.HasValue)
            {
                q = q.Where(x => x.ShipDate >= query.ShippingDateStart.Value);
                //anotherq = anotherq.Where(x => x.ShipDate >= query.ShippingDateStart.Value);
            }
            if (query.ShippingDateEnd.HasValue)
            {
                q = q.Where(x => x.ShipDate <= query.ShippingDateEnd.Value);
                //anotherq = anotherq.Where(x => x.ShipDate <= query.ShippingDateEnd.Value);
            }
            if (query.ShippingDate.HasValue) 
            {
                q = q.Where(x => (x.ShipDate.Value.Year == query.ShippingDate.Value.Year && x.ShipDate.Value.Month == query.ShippingDate.Value.Month && x.ShipDate.Value.Day == query.ShippingDate.Value.Day)
                || (x.ReScheduleDate.Value.Year == query.ShippingDate.Value.Year && x.ReScheduleDate.Value.Month == query.ShippingDate.Value.Month && x.ReScheduleDate.Value.Day == query.ShippingDate.Value.Day));
            }
            if (!string.IsNullOrEmpty(query.Status))
            {
                q = q.Where(x => x.Status == query.Status);
              //  anotherq = anotherq.Where(x => x.Status == query.Status);
            }
            if (query.DeliveryTypeId.HasValue)
            {
                q = q.Where(x => x.DeliveryTypeId == query.DeliveryTypeId.Value);
               // anotherq = anotherq.Where(x => x.DeliveryTypeId == query.DeliveryTypeId.Value);
            }
            if (query.DivisionId.HasValue)
            {
                q = q.Where(x => x.DivisionId == query.DivisionId.Value);
                //anotherq = anotherq.Where(x => x.DivisionId == query.DivisionId.Value);
            }
            else if (query.DivisionIds != null && query.DivisionIds.Any())
            {
                q = q.Where(x => x.DivisionId.HasValue && query.DivisionIds.Contains(x.DivisionId.Value));
                //anotherq = anotherq.Where(x => x.DivisionId.HasValue && query.DivisionIds.Contains(x.DivisionId.Value));
            }

            if (query.IsReschedule.HasValue)
            {
                q = q.Where(x => x.IsReSchedule == query.IsReschedule.Value);
               // anotherq = anotherq.Where(x => x.IsReSchedule == query.IsReschedule.Value);
            }

            if (query.hasBool.HasValue)
            {
                if (query.hasBool.Value)
                {
                    q = q.Where(x => !string.IsNullOrEmpty(x.BillOfLading));
                }
                else
                {
                    q = q.Where(x => string.IsNullOrEmpty(x.BillOfLading));
                }
                
            }
            if (!String.IsNullOrEmpty(query.BillOfLading))
            {
                q = q.Where(x => x.BillOfLading == query.BillOfLading);
            }
            if (!String.IsNullOrEmpty(query.MasterBillOfLading))
            {
                q = q.Where(x => x.MasterBillOfLading == query.MasterBillOfLading);
            }
            // var q1 = q.ToList().Count();
            //  var q2 = anotherq.ToList().Count();
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
                DivisionId = x.DivisionId,
                DateAdded = x.DateAdd,
                PickTicket = x.PickTicketId,
                Posted = x.Posted,
                PtBulk = x.PtBulk,
                ScacCode = x.ScacCode,
                ShippingDate = x.ShipDate,
                ShippingTime = x.ShipTime,
                ShippingTimeLimit = x.ShippingTimeLimit,
                Status = x.Status,
                Transfered = x.Transferred,
                UserName = x.UserName,
                Carrier = x.CatScacCode.ScacCodeName,
                CustomerName = x.Customer != null ? x.Customer.CompanyName : string.Empty,
                //DivisionName = x.Division != null ? x.Division.Description : string.Empty,
                DivisionName = x.DivisionName,// != null ? x.Division.Description : string.Empty,
                DivisionNameId = x.DivisionNameId,// != null ? x.Division.NameId : null,
                DeliveryTypeId = x.DeliveryTypeId,
                ReScheduleDate = x.ReScheduleDate,
                IsReSchedule =x.IsReSchedule,
                Pallets = x.Pallets,
                DriverId = x.DriverId,
                DriverName = x.Driver?.Name,
                TruckId = x.TruckId,
                TruckDescription = x.Truck?.Description



            });

            return result.ToList();

        }

        public List<Model.Appointment> ToList(AppointmentQuery query)
        {
            var l = Appointment_BuildQuery(query)
                .AsNoTracking()
                .ToList();

            var result = l.Select(x => new Model.Appointment
            {
                AppointmentNumber = x.AppointmentNumber,
                CustomerId = x.CustomerId,
                DivisionId = x.DivisionId,
                DateAdded = x.DateAdd,
                PickTicket = x.PickTicketId,
                Posted = x.Posted,
                PtBulk = x.PtBulk,
                ScacCode = x.ScacCode,
                ShippingDate = x.ShipDate,
                ShippingTime = x.ShipTime,
                ShippingTimeLimit = x.ShippingTimeLimit,
                Status = x.Status,
                Transfered = x.Transferred,
                UserName = x.UserName,
                Carrier = x.CatScacCode.ScacCodeName,
                CustomerName = x.Customer != null ? x.Customer.CompanyName : string.Empty,
                //DivisionName = x.Division != null ? x.Division.Description : string.Empty,
                DivisionName = x.DivisionName,// != null ? x.Division.Description : string.Empty,
                DivisionNameId = x.DivisionNameId,// != null ? x.Division.NameId : null,
                DeliveryTypeId = x.DeliveryTypeId,
                ReScheduleDate = x.ReScheduleDate,
                IsReSchedule = x.IsReSchedule,
                Pallets = x.Pallets,
                DriverId = x.DriverId,
                DriverName  =  x.Driver != null ? $"{x.Driver.Name} {x.Driver.SurName}" : null,
                TruckId = x.TruckId,
                TruckDescription = x.Truck?.Description
            });

            return result.ToList();

        }
        public async Task<int> Create(Model.Appointment appointmentModel)
        {
            Entities.Appointment appointment = new Entities.Appointment();
            appointment.CustomerId = appointmentModel.CustomerId;
            appointment.DateAdd = DateTime.Now;
            appointment.PickTicketId = appointmentModel.PickTicket;
            appointment.DivisionId = appointmentModel.DivisionId;

            appointment.ScacCode = appointmentModel.ScacCode;
            appointment.ShipDate = appointmentModel.ShippingDate.Value;
            appointment.ShipTime = new DateTime(appointmentModel.ShippingDate.Value.Year, appointmentModel.ShippingDate.Value.Month, appointmentModel.ShippingDate.Value.Day, appointmentModel.ShippingTime.Value.Hour, appointmentModel.ShippingTime.Value.Minute, 0);
            appointment.AppointmentNumber = appointmentModel.AppointmentNumber;
            appointment.DeliveryTypeId = appointmentModel.DeliveryTypeId.Value;
            appointment.UserName = appointmentModel.UserName;

            if (appointmentModel.ShippingTimeLimit.HasValue)
            {
                appointment.ShippingTimeLimit = new DateTime(appointmentModel.ShippingDate.Value.Year, appointmentModel.ShippingDate.Value.Month, appointmentModel.ShippingDate.Value.Day, appointmentModel.ShippingTimeLimit.Value.Hour, appointmentModel.ShippingTimeLimit.Value.Minute, 0);
            }

            appointment.PtBulk = string.Empty;
            if (!string.IsNullOrEmpty(appointmentModel.PtBulk))
            {
                appointment.PtBulk = appointmentModel.PtBulk;
                appointment.PickTicketId = appointmentModel.PtBulk;
            }

            context.Appointments.Add(appointment);

            try
            {
                return await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }


        }

        public async Task<string> SetAppointment(Model.Appointment appointmentModel, string purchaseOrder)
        {
            using (System.Data.Entity.DbContextTransaction dbTran = context.Database.BeginTransaction())
            {
                try
                {
                    var existingAppt = context.Appointments.Where(x => x.CustomerId == appointmentModel.CustomerId && x.PickTicketId == appointmentModel.PickTicket).FirstOrDefault();

                    if (existingAppt != null)
                    {
                        if (existingAppt.Status == "D")
                        {
                            // update the existing one, could be a previous cancelation
                            context.Appointments.Remove(existingAppt);
                        }
                        else
                        {
                            throw new ArgumentException($"Order with picticket {appointmentModel.PickTicket} for client {appointmentModel.CustomerId} already have an appointment {existingAppt.AppointmentNumber}");
                        }


                        
                    }

                    Entities.Appointment appointment = new Entities.Appointment();
                    appointment.CustomerId = appointmentModel.CustomerId;
                    appointment.DateAdd = DateTime.Now;
                    appointment.PickTicketId = appointmentModel.PickTicket;
                    appointment.DivisionId = appointmentModel.DivisionId;

                    appointment.ScacCode = appointmentModel.ScacCode;
                   // appointment.ShipDate = appointmentModel.ShippingDate.Value;
                    appointment.ShipTime = new DateTime(appointmentModel.ShippingDate.Value.Year, appointmentModel.ShippingDate.Value.Month, appointmentModel.ShippingDate.Value.Day, appointmentModel.ShippingTime.Value.Hour, appointmentModel.ShippingTime.Value.Minute, 0);
                    appointment.AppointmentNumber = appointmentModel.AppointmentNumber;
                    appointment.DeliveryTypeId = appointmentModel.DeliveryTypeId.Value;
                    appointment.UserName = appointmentModel.UserName;
                    appointment.Pallets = appointmentModel.Pallets;
                    appointment.DriverId = appointmentModel.DriverId;
                    appointment.TruckId = appointmentModel.TruckId;

                    if (appointmentModel.ShippingTimeLimit.HasValue)
                    {
                        appointment.ShippingTimeLimit = new DateTime(appointmentModel.ShippingDate.Value.Year, appointmentModel.ShippingDate.Value.Month, appointmentModel.ShippingDate.Value.Day, appointmentModel.ShippingTimeLimit.Value.Hour, appointmentModel.ShippingTimeLimit.Value.Minute, 0);
                    }

                    appointment.PtBulk = string.Empty;
                    if (!string.IsNullOrEmpty(appointmentModel.PtBulk))
                    {
                        appointment.PtBulk = appointmentModel.PtBulk;
                        appointment.PickTicketId = appointmentModel.PtBulk;
                    }

                    context.Appointments.Add(appointment);


                    var ptBulk = string.IsNullOrEmpty(appointment.PtBulk) ? "" : appointment.PtBulk;

                    
                    var entity = await context.OrderAppointments.Where(x => x.PurchaseOrderId == purchaseOrder && x.PickTicketId == appointment.PickTicketId && x.CustomerId == appointment.CustomerId && x.PtBulk == ptBulk).FirstOrDefaultAsync();

                    if (entity != null)
                    {

                        entity.Status = 1;
                        entity.ShipFor = appointmentModel.ShippingDate.Value;
                    }

                    await context.SaveChangesAsync();

                    dbTran.Commit();

                    return null;
                }
                catch (Exception exc)
                {
                    dbTran.Rollback();

                    return $"{exc.Message} : { exc.InnerException.Message} ";
                }

            }
        }
        public async Task<int> UpdateScript(Model.Appointment appointment)
        {
         
            var reschDate = appointment.ReScheduleDate.HasValue ? appointment.ReScheduleDate.Value.ToShortDateString() : string.Empty;
            var timeLimit = appointment.ShippingTimeLimit.HasValue ? appointment.ShippingTimeLimit.Value.ToShortTimeString() : string.Empty;
            var shippingTime = appointment.ShippingTime.Value.ToString("yyyy-MM-dd HH:mm:ss.fff");
            var script = @"UPDATE dbo.Appointments SET AppointmentNo = {0}, 
                        ShippTime = CAST({1} AS DATETIME), 
                        ShippingTimeLimit = {2}, 
                        DeliveryTypeId = {3}, 
                        Pallets = {6}, 
                        TruckNo = {7}, 
                        DriverNo = {8} 
                        WHERE CustomerId = {4} 
                            AND PickTicket = {5} 
                            AND Posted = 0";

            var scriptOrder = @"UPDATE dbo.OrderAppointment SET ShipFor = CAST({0} AS DATETIME) WHERE CustomerId = {1} AND PickTicketId = {2} ";

            try
            {

                if (context.Database.Connection.ConnectionString.ToLower().Contains("diavolo"))
                {
                    script = "SET LANGUAGE us_english; " + script;
                    scriptOrder = "SET LANGUAGE us_english; " + scriptOrder;
                }

                script = string.Format(script, 
                    $"'{appointment.AppointmentNumber}'", 
                    $"'{shippingTime}'", 
                    string.IsNullOrEmpty(timeLimit) ? "NULL" : $"'{timeLimit}'", 
                    $"{appointment.DeliveryTypeId}", 
                    $"'{appointment.CustomerId}'", $"'{appointment.PickTicket}'", 
                    appointment.Pallets.HasValue ? $"{appointment.Pallets.Value}" : "NULL", 
                    appointment.TruckId.HasValue ? $"{appointment.TruckId.Value}": "NULL", 
                    appointment.DriverId.HasValue ? $"{appointment.DriverId.Value}" : "NULL");

                scriptOrder = string.Format(scriptOrder, $"'{shippingTime}'", $"'{appointment.CustomerId}'", $"'{appointment.PickTicket}'");

                var result = await context.Database.ExecuteSqlCommandAsync(script);
                result = await context.Database.ExecuteSqlCommandAsync(scriptOrder);
            }
            catch (Exception exc)
            {
                throw exc;
            }
            return 0;

        }

        public async Task<string> Update(Model.Appointment appointment)
        {
            try
            {
                var entity = context.Appointments.Where(x => x.CustomerId == appointment.CustomerId && x.PickTicketId == appointment.PickTicket).FirstOrDefault();

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

                    if (!string.IsNullOrEmpty(appointment.AppointmentNumber))
                    {
                        entity.AppointmentNumber = appointment.AppointmentNumber;
                    }
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
                    if (appointment.ShippingTimeLimit.HasValue)
                    {
                        entity.ShippingTimeLimit = appointment.ShippingTimeLimit;
                    }
                    if (appointment.DeliveryTypeId.HasValue)
                    {
                        entity.DeliveryTypeId = appointment.DeliveryTypeId;
                    }

                    if (appointment.ReScheduleDate.HasValue)
                    {
                        entity.ReScheduleDate = appointment.ReScheduleDate;
                    }

                    entity.IsReSchedule = appointment.IsReSchedule;


                    var result =  await context.SaveChangesAsync();
                }

            }
            catch (Exception exc)
            {
                return exc.Message;
            }
            return string.Empty;
        }

        //public async Task<string> PostAppointment(Model.Appointment appointment)
        //{
        //    var entity = context.Appointments.Where(x => x.CustomerId == appointment.CustomerId && x.PickTicket == appointment.PickTicket).FirstOrDefault();

        //    if (entity != null)
        //    {
        //        if (appointment.Posted.HasValue)
        //        {
        //            entity.Posted = appointment.Posted.Value;
        //        }
        //        if (!string.IsNullOrEmpty(appointment.Status))
        //        {
        //            entity.Status = appointment.Status;
        //        }

        //        if (!string.IsNullOrEmpty(appointment.AppointmentNumber))
        //        {
        //            entity.AppointmentNumber = appointment.AppointmentNumber;
        //        }
        //        if (!string.IsNullOrEmpty(appointment.ScacCode))
        //        {
        //            entity.ScacCode = appointment.ScacCode;
        //        }
        //        if (appointment.ShippingDate.HasValue)
        //        {
        //            entity.ShipDate = appointment.ShippingDate.Value;
        //        }
        //        if (appointment.ShippingTime.HasValue)
        //        {
        //            entity.ShipTime = appointment.ShippingTime.Value;
        //        }
        //        if (appointment.ShippingTimeLimit.HasValue)
        //        {
        //            entity.ShippingTimeLimit = appointment.ShippingTimeLimit;
        //        }
        //        if (appointment.DeliveryTypeId.HasValue)
        //        {
        //            entity.DeliveryTypeId = appointment.DeliveryTypeId;
        //        }

        //        if (appointment.ReScheduleDate.HasValue)
        //        {
        //            entity.ReScheduleDate = appointment.ReScheduleDate;
        //        }
        //    }

        //}
    }
}
