using GSLogistics.Entities.Abstract;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSLogistics.Entities.Concrete
{
    public partial class GSLogisticsRepository : IRepository
    {
        private IKernel _Kernel;
        private GSLogisticsContext _Context;

        public GSLogisticsRepository(IKernel kernel, GSLogisticsContext context = null)
        {
            if (kernel == null)
            {
                throw new ArgumentNullException(nameof(kernel));
            }

            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            _Kernel = kernel;
            _Context = _Kernel.Get<GSLogisticsContext>();

        }

        protected GSLogisticsContext Context
        {
            get
            {
                return _Context;
            }
        }

        private GSLogisticsContext CreateContext()
        {
            return _Kernel.Get<GSLogisticsContext>();
        }

        


        private GSLogisticsContext context = new GSLogisticsContext();

        public IEnumerable<OrderAppointment> OrderAppointments { get { return context.OrderAppointments.Include("Customer"); } }
        public IEnumerable<ScacCode> ScacCodes { get { return context.ScacCodes; } }

        public IEnumerable<Appointment> Appointments { get { return context.Appointments.Include("Customer"); } }
        public IEnumerable<Customer> Customers { get { return context.Customers; } }

        public IEnumerable<CustomerDivision> Divisions { get { return context.CustomerDivisions; } }

        public IEnumerable<UserInfo> UserInfos { get { return context.UserInfos; } }
        public IEnumerable<UserCustomer> UserCustomers { get { return context.UserCustomers; } }

        public void SaveAppointment(Appointment appointment)
        {
            context.Appointments.Add(appointment);

            try
            {
                context.SaveChanges();
            }
            catch(Exception ex)
            {
                throw ex;
            }

        }

        public void UpdateOrderAppointment(OrderAppointment orderAppointment)
        {
            try
            {
                var ptBulk = string.IsNullOrEmpty(orderAppointment.PtBulk) ? "" : orderAppointment.PtBulk;
                var entity = context.OrderAppointments.Where(x => x.PurchaseOrderId == orderAppointment.PurchaseOrderId && x.PickTicketId == orderAppointment.PickTicketId && x.CustomerId == orderAppointment.CustomerId && x.PtBulk == ptBulk).FirstOrDefault();


                if (entity != null)
                {
                    //if (orderAppointment.Status != 0)
                    //{
                        entity.Status = orderAppointment.Status;
                    //}

                    if (!string.IsNullOrEmpty(orderAppointment.Notes))
                    {
                        entity.Notes = orderAppointment.Notes;
                    }

                    if (!string.IsNullOrEmpty(orderAppointment.ConfirmationNumber))
                    {
                        entity.ConfirmationNumber = orderAppointment.ConfirmationNumber;
                    }

                    context.SaveChanges();
                }

                
            }
            catch(Exception exc)
            {
                throw exc;
            }
        }

        public async Task UpdateAppointment(Model.Appointment appointment)
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
                    if(!string.IsNullOrEmpty(appointment.ScacCode))
                    {
                        entity.ScacCode = appointment.ScacCode;
                    }
                    if(appointment.ShippingDate.HasValue)
                    {
                        entity.ShipDate = appointment.ShippingDate.Value;
                    }
                    if (appointment.ShippingTime.HasValue)
                    {
                        entity.ShipTime = appointment.ShippingTime.Value;
                    }


                    await context.SaveChangesAsync();
                }

            }
            catch (Exception exc)
            {
                throw exc;
            }
        }


        public void UpdateOrderAppointmentNotes(string pickTicketId, string notes)
        {
            try
            {
                
                var entity = context.OrderAppointments.Where(x => x.PickTicketId == pickTicketId).FirstOrDefault();


                if (entity != null)
                {
                    entity.Notes = notes;

                    context.SaveChanges();
                }


            }
            catch (Exception exc)
            {
                throw exc;
            }
        }

        public IList<CustomerDivision> GetDivisionByClient(string customerId)
        {
            var query = context.CustomerDivisions.Where(x => x.CustomerId == customerId);

            return query.ToList();
        }

        #region Dispose

        bool disposed = false;

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
            {
                return;
            }
            if (disposing)
            {
                context.Dispose();
                context = null;
            }

            disposed = true;
        }
        #endregion
    }
}
