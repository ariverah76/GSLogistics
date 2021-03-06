﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSLogistics.Logic.Interface
{
    public interface IGSLogisticsLogic : IDisposable
    {
        T GetLogic<T>()
           where T : IGSLogisticsLogic;
    }
}
