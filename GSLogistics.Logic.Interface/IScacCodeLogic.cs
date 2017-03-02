using GSLogistics.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSLogistics.Logic.Interface
{
    public interface IScacCodeLogic
    {
        Task<List<ScacCode>> GetScacCodes();
    }
}
