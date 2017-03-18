using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSLogistics.Entities.Abstract
{
    public interface IRepository : IDisposable
    {
        IEnumerable<OrderAppointment> OrderAppointments { get; }
        IEnumerable<ScacCode> ScacCodes { get; }
        IEnumerable<Appointment> Appointments { get; }
        IEnumerable<Customer> Customers { get; }
        IEnumerable<CustomerDivision> Divisions { get; }

        void SaveAppointment(Appointment appointment);
        void UpdateOrderAppointment(OrderAppointment orderAppointment);
        void UpdateOrderAppointmentNotes(string pickTicketId, string notes);
        Task UpdateAppointment(Model.Appointment appointment);
        IList<CustomerDivision> GetDivisionByClient(string customerId);
    }
}
