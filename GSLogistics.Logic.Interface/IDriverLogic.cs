using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSLogistics.Logic.Interface
{
    public interface  IDriverLogic : IGSLogisticsLogic
    {
        Task<List<Model.Driver>> GetDrivers();
    }
}
