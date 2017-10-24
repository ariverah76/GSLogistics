using GSLogistics.Logic.Interface;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSLogistics.Logic
{
    public class DriverLogic : LogicBase, IDriverLogic
    {
        public DriverLogic(IKernel kernel)
            : base(kernel)
        {

        }

        public async Task<List<Model.Driver>> GetDrivers()
        {
            return await Repository.GetDrivers();
        }
    }
}
