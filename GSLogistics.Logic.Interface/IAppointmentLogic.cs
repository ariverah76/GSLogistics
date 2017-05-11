using GSLogistics.Model;
using GSLogistics.Model.Query;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GSLogistics.Logic.Interface
{
    public interface IAppointmentLogic : IGSLogisticsLogic
    {
        Task<IList<Model.Appointment>> ToListAsync(AppointmentQuery query);
        IList<Model.Appointment> ToList(AppointmentQuery query);
        Task<int> Update(Appointment appointment);
        Task<int> Create(Model.Appointment appointmentModel);
    }
}
