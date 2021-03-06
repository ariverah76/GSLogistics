﻿using GSLogistics.Logic.Interface;
using GSLogistics.Model;
using GSLogistics.Model.Query;
using GSLogistics.Website.Admin.Models.OrderAppointments;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSLogistics.Logic
{
    public class AppointmentLogic : LogicBase, IAppointmentLogic
    {
        public AppointmentLogic(IKernel kernel)
            :base(kernel)
        {

        }

        public async Task<IList<Model.Appointment>> ToListAsync(AppointmentQuery query)
        {
            return await Repository.ToListAsync(query);
        }

        public async Task<string> Update(Appointment appointment)
        {
            return await Repository.Update(appointment);
        }
        public async Task<int> Create(Model.Appointment orderAppointment)
        {
            return await Repository.Create(orderAppointment);
        }

        public IList<Model.Appointment> ToList(AppointmentQuery query)
        {
            return Repository.ToList(query);
        }


        public async Task<int> UpdateScript(Appointment appointment, string Notes)
        {
            return await Repository.UpdateScript(appointment, Notes);
        }

        public async Task<string> SetAppointment(Appointment appointment, OrderAppointment order)
        {
            return await Repository.SetAppointment(appointment, order);
        }

        public async Task<string> SetAppointment(NewAppointment_ViewModel newAppointmentModel, OrderAppointment[] orders, string userName)
        {
            return await Repository.SetAppointment(newAppointmentModel, orders, userName);
           // Task<string> SetAppointment(NewAppointment_ViewModel newAppointmentModel, OrderAppointment[] orders, string userName)
        }
    }
}
