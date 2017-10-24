using GSLogistics.Logic.Interface;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSLogistics.Logic
{
    public class TruckLogic: LogicBase, ITruckLogic
    {
        public TruckLogic(IKernel kernel):
            base(kernel)
        {

        }

        public async Task<List<Model.Truck>> GetTrucks()
        {
            return await Repository.GetTrucks();
        }
    }

}
