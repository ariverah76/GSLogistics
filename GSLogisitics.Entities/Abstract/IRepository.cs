﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSLogistics.Entities.Abstract
{
    public interface IRepository
    {
        IEnumerable<OrderAppointment> OrderAppointments { get; }
        IEnumerable<ScacCode> ScacCodes { get; }

        void SaveAppointment(Appointment appointment);
        void UpdateOrderAppointment(OrderAppointment orderAppointment);
        void UpdateOrderAppointmentNotes(string pickTicketId, string notes);
    }
}