using GSLogistics.Logic.Interface;
using GSLogistics.Model;
using GSLogistics.Model.Query;
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

        public async Task<int> Update(Appointment appointment)
        {
            return await Repository.Update(appointment);
        }


    }
}
