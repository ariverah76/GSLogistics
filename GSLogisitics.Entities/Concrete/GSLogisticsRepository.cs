﻿using GSLogistics.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSLogistics.Entities.Concrete
{
    public class GSLogisticsRepository : IRepository
    {
        private GSLogisticsContext context = new GSLogisticsContext();

        public IEnumerable<OrderAppointment> OrderAppointments { get { return context.OrderAppointments; } }
        public IEnumerable<ScacCode> ScacCodes { get { return context.ScacCodes; } }

        public IEnumerable<Appointment> Appointments { get { return context.Appointments; } }

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
                    entity.Status = orderAppointment.Status;

                    context.SaveChanges();
                }

                
            }
            catch(Exception exc)
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

    }
}