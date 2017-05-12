using GSLogistics.Model.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSLogistics.Logic.Interface
{
    public interface IOrderAppointmentLogic : IGSLogisticsLogic
    {
        Task<IList<Model.OrderAppointment>> ToListAsync(OrderAppointmentQuery query);
        Task<int> Update(Model.OrderAppointment orderAppointment);
        IList<Model.OrderAppointment> ToList(OrderAppointmentQuery query);
    }
}
