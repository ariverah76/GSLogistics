using GSLogistics.Model;
using GSLogistics.Model.Query;
using GSLogistics.Website.Admin.Models.OrderAppointments;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GSLogistics.Logic.Interface
{
    public interface IAppointmentLogic : IGSLogisticsLogic
    {
        Task<IList<Model.Appointment>> ToListAsync(AppointmentQuery query);
        IList<Model.Appointment> ToList(AppointmentQuery query);
        Task<string> Update(Appointment appointment);
        Task<int> Create(Model.Appointment appointmentModel);
        Task<int> UpdateScript(Appointment appointment, string Notes);

        Task<string> SetAppointment(Appointment appointment, OrderAppointment order);
        Task<string> SetAppointment(NewAppointment_ViewModel newAppointmentModel, OrderAppointment[] orders, string userName);
    }
}
