using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSLogistics.Entities
{
    public class GSExternalContext : DbContext
    {

        public GSExternalContext()
            :base("GSLogisticsCentralEntities")
        {

        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        public static GSExternalContext Create()
        {
            return new GSExternalContext();
        }
    }
}
