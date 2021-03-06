﻿using GSLogistics.Logic.Interface;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSLogistics.Logic
{
    public class DivisionLogic : LogicBase, IDivisionLogic
    {
        public DivisionLogic(IKernel kernel)
            : base(kernel)
        {

        }

        public async Task<List<Model.Division>> GetDivisionByCustomerId(string customerId)
        {

            return await Repository.GetDivisionsByCustomerId(customerId);
        }

        public async  Task<List<Model.Division>> GetDivisionsByCustomerIds(string[] customerIds)
        {
            return await Repository.GetDivisionsByCustomerIds(customerIds);
        }


        public async Task<Model.Division> GetFirstOrDefaultAsync(int divisionId)
        {
            return await Repository.FirstOrDefaultAsync(divisionId);
        }


    }
}
