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

        public async Task<List<Model.Customer>> ToListAsync(CustomerQuery query)
        {
            List<Model.Customer> returnValue = new List<Model.Customer>();

            var q = context.Customers.Where(x => true);

            if(!string.IsNullOrEmpty(query.CustomerId))
            {
                q = q.Where(x => x.CustomerId == query.CustomerId);
            }

            if (query.CustomerIds.Any())
            {
                q = q.Where(x => query.CustomerIds.Contains(x.CustomerId));
            }

            var result = await q
                .AsNoTracking()
                .ToListAsync();

            result.ForEach(x => returnValue.Add(new Model.Customer() { CustomerId = x.CustomerId, CompanyName = x.CompanyName }));

            return returnValue;
        }

        public async Task<Model.Customer> FirstOrDefaultAsync(string identifier)
        {
            var result = await context.Customers.Where(x => x.CustomerId == identifier).FirstOrDefaultAsync();

            if (result != null)
            {
                return new Model.Customer() { CustomerId = result.CustomerId, CompanyName = result.CompanyName };
            }

            return null;

        }

        public async Task<Model.Division> FirstOrDefaultAsync(int divisionId)
        {
            var result = await context.CustomerDivisions.Where(x => x.DivisionId == divisionId).FirstOrDefaultAsync();

            if (result != null)
            {
                return new Model.Division() { DivisionId = result.DivisionId, Description = result.Description, Name = result.NameId};
            }

            return null;

        }

    }
}
