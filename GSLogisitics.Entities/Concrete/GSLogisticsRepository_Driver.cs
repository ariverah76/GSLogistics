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
        public async Task<List<Model.Driver>> GetDrivers() {

            List<Model.Driver> drivers = new List<Model.Driver>();

            var result = await context.Drivers.ToListAsync();

            drivers = result.Select(x => new Model.Driver() { DriverId = x.DriverId, Name = x.Name, SurName = x.SurName }).ToList();

            return drivers;
        }
    }
}
