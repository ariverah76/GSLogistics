using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSLogistics.Logic.Interface
{
    public interface ITruckLogic : IGSLogisticsLogic
    {
        Task<List<Model.Truck>> GetTrucks();
    }
}
