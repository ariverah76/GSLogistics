using GSLogistics.Model.Query;
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
                returnValue.Add(new Model.Division() { CustomerId = d.CustomerId, DivisionId = d.DivisionId, Name = d.NameId, Description = d.Description });
            }

            return returnValue;
        }

        public async Task<List<Model.Customer>> ToListAsync()
        {
            List<Model.Customer> returnValue = new List<Model.Customer>();
            var query = context.Customers.Where(x => true);

            var result = await query
                .AsNoTracking()
                .ToListAsync();

            result.ForEach(x => returnValue.Add(new Model.Customer() { CustomerId = x.CustomerId, CompanyName = x.CompanyName }));

            return returnValue;
        }

    }
}
