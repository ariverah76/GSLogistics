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
        public async Task<List<Model.Truck>> GetTrucks()
        {

            List<Model.Truck> trucks = new List<Model.Truck>();

            var result = await context.Trucks.ToListAsync();

            trucks =  result.Select(x => new Model.Truck() { TruckId = x.TruckId, Plates = x.Plates, Description = x.Description }).ToList();

            return trucks;
        }

    }
}
