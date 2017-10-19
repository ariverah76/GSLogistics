using GSLogistics.Logic.Interface;
using GSLogistics.Model.Query;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSLogistics.Logic
{
    public partial class OrderAppointmentLogic: LogicBase , IOrderAppointmentLogic
    {
        public OrderAppointmentLogic(IKernel kernel)
            :base(kernel)
        {

        }

        public async Task<IList<Model.OrderAppointment>> ToListAsync(OrderAppointmentQuery query)
        {
            return await Repository.ToListAsync(query);
        }

        public IList<Model.OrderAppointment> ToList(OrderAppointmentQuery query)
        {
            return  Repository.ToList(query);
        }

        public async Task<int> Update(Model.OrderAppointment orderAppointment)
        {
            return await  Repository.Update(orderAppointment);
        }

       
       

    }
}
