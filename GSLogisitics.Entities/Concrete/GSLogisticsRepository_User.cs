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

        public async Task<IEnumerable<string>> GetCustomersForUser(string userId)
        {
            var user = await context.Users.Where(x => x.Id == userId).FirstOrDefaultAsync();

            if (user != null)
            {
                var query = context.UserInfos
                  .Where(x => x.UserName == user.UserName);

                var result = await query.FirstOrDefaultAsync();

                return result.UserCustomers.Select(x => x.CustomerId).ToList();
            }
            else
            {
                return new List<string>();
            }
            
        }

        public async Task<IEnumerable<int>> GetDivisionsForUser(string userId)
        {
            var user = await context.Users.Where(x => x.Id == userId).FirstOrDefaultAsync();

            if (user != null)
            {
                var query = context.UserInfos
                  .Where(x => x.UserName == user.UserName);

                var result = await query.FirstOrDefaultAsync();

                return result.UserCustomers.Select(x => x.DivisionId).ToList();
            }
            else
            {
                return new List<int>();
            }

        }

        public async Task<List<UserCustomer>> GetUserCustomers(string userId)
        {
            var user = await context.Users.Where(x => x.Id == userId).FirstOrDefaultAsync();
            if (user != null)
            {
                var query = context.UserInfos
                  .Where(x => x.UserName == user.UserName);

                var result = await query.FirstOrDefaultAsync();

                return result.UserCustomers.ToList();
            }
            else
            {
                return new List<UserCustomer>();
            }
        }

        public async Task<bool> AssignCustomers(string userId, IList<string> customerIds)
        {
            var user = await context.Users.Where(x => x.Id == userId).FirstOrDefaultAsync();

            var gsUser = await context.UserInfos.Where(x => x.UserName == user.UserName).FirstOrDefaultAsync();
            var currentCustIds = gsUser.UserCustomers.Select(x => x.CustomerId).ToList();
            //TODO: filter the ids already assigned


            foreach (string custId in customerIds.Where(x=> !currentCustIds.Contains(x)))
            {
                var usercust = new UserCustomer();
                usercust.CustomerId = custId;
                usercust.UserId = gsUser.UserId;

                gsUser.UserCustomers.Add(usercust);

                await context.SaveChangesAsync();
            }

            return true;
        }

        public async Task<bool> AssignDivisions(string userId, Dictionary<int, string> customerDivisions)
        {
            var user = await context.Users.Where(x => x.Id == userId).FirstOrDefaultAsync();

            var gsUser = await context.UserInfos.Where(x => x.UserName == user.UserName).FirstOrDefaultAsync();
            var currentDivIds = gsUser.UserCustomers.Select(x => x.DivisionId).ToList();

            var divisionIds = customerDivisions.Keys;

            foreach (int divId in divisionIds.Where(x => !currentDivIds.Contains(x)))
            { 
                var division = await context.CustomerDivisions.FirstOrDefaultAsync(x => x.DivisionId == divId);

                if (division != null)
                {
                    var usercust = new UserCustomer();
                    usercust.CustomerId = division.CustomerId;
                    usercust.DivisionId = division.DivisionId;
                    usercust.UserId = gsUser.UserId;

                    gsUser.UserCustomers.Add(usercust);

                    await context.SaveChangesAsync();
                }
            }

            return true;
        }

        public async Task<UserInfo> CreateUserInfoAsync(string userName, string description)
        {
            var user = new UserInfo()
            {
                Description = description,
                UserName = userName
            };

            context.UserInfos.Add(user);

            await context.SaveChangesAsync();

            return user;
        }

       
        public async Task<bool> DeleteCustomerRole(string userId, string customerId, int divisionId)
        {
            var user = await context.Users.Where(x => x.Id == userId).FirstOrDefaultAsync();

            var gsUser = await context.UserInfos.Where(x => x.UserName == user.UserName).FirstOrDefaultAsync();

            var userCust = new Entities.UserCustomer()
            {
                CustomerId = customerId,
                DivisionId = divisionId,
                UserId = gsUser.UserId

            };

            var userCustomer = await context.UserCustomers.Where(x => x.CustomerId == customerId && x.DivisionId == divisionId && x.UserId == gsUser.UserId).FirstAsync();

            context.UserCustomers.Remove(userCustomer);
            //gsUser.UserCustomers.Remove(userCust);
            var result = await context.SaveChangesAsync();

            return true;
        }

        public async Task<UserInfo> UserInfo_FirstOrDefaultAsync(string userId)
        {
            var user = await context.Users.Where(x => x.Id == userId).FirstOrDefaultAsync();

            if (user != null)
            {
                return await context.UserInfos.Where(x => x.UserName == user.UserName).FirstOrDefaultAsync();
            }
            return null;
        }

    }
}
