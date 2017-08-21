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
        public async Task<Model.UserInfo> GetUserInfoAsync(string userName)
        {
            var query = context.UserInfos
                .Include("UserCustomer")
                .Where(x => x.UserName == userName);

            var result = await query
                .AsNoTracking()
                .FirstOrDefaultAsync();

            if (result != null)
            {
                var user = new Model.UserInfo() { UserId = result.UserId, UserName = result.UserName };

                if (result.UserCustomers != null && result.UserCustomers.Any())
                {
                    foreach (var c in result.UserCustomers)
                    {
                        user.UserCustomers.Add(new Model.UserCustomer() { UserId = c.UserId, CustomerId = c.CustomerId, DivisionId = c.DivisionId });
                    }
                }
                return user;

            }

            return null;
        }
    }
}
