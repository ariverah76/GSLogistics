using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GSLogistics.Model;
using System.Data.Entity;

namespace GSLogistics.Entities.Concrete
{
    public partial class GSLogisticsRepository
    {
        public async Task<List<Model.ScacCode>> GetScacCodes()
        {
            var result = await Context.ScacCodes.ToListAsync();


            return  result.Select(x => new Model.ScacCode() { Carrier = x.ScacCodeName, ScacCodeId = x.ScacCodeId }).ToList();

        }
    }
}
