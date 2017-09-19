using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GSLogistics.Model;
using System.Data.Entity;
using System.Data;
using System.Transactions;

namespace GSLogistics.Entities.Concrete
{
    public partial class GSLogisticsRepository
    {
        public async Task<List<Model.ScacCode>> GetScacCodes()
        {
            List<Model.ScacCode> scacCodes = new List<Model.ScacCode>();
            var transactionOptions = new System.Transactions.TransactionOptions();
            //set it to read uncommited
            transactionOptions.IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted;

            using (var t = new TransactionScope(TransactionScopeOption.Required, transactionOptions, TransactionScopeAsyncFlowOption.Enabled))
            {
                var result = await Context.ScacCodes.ToListAsync();

               
                scacCodes =  result.Select(x => new Model.ScacCode() { Carrier = x.ScacCodeName, ScacCodeId = x.ScacCodeId }).ToList();

                t.Complete();
                t.Dispose();
            }
            return scacCodes;
                
            

        }
    }
}
