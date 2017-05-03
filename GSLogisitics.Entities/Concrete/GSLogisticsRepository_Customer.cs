using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSLogistics.Entities.Concrete
{
    public partial class GSLogisticsRepository
    {

        public async Task<List<Model.Division>> GetDivisionsByCustomerId(string customerId)
        {
            List<Model.Division> returnValue = new List<Model.Division>();
            var query = context.CustomerDivisions.Where(x => x.CustomerId == customerId);

            var result = await  query
                .AsNoTracking()
                .ToListAsync(); 

            foreach(var d in result)
            {
                returnValue.Add(new Model.Division() { CustomerId = d.CustomerId, DivisionId = d.DivisionId, DivisionName = d.Description });
            }

            return returnValue;
        }

    }
}
