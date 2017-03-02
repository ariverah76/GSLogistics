using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;
using GSLogistics.Model;
using GSLogistics.Logic.Interface;

namespace GSLogistics.Logic
{
    public class ScacCodeLogic : LogicBase, IScacCodeLogic
    {
        public ScacCodeLogic(IKernel kernel)
            : base(kernel)
        {

        }

        public async Task<List<ScacCode>> GetScacCodes()
        {
            return  await Repository.GetScacCodes();
            
        }

    }
}
