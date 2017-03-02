using GSLogistics.Entities;
using GSLogistics.Entities.Concrete;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSLogistics.Logic
{
    public class LogicBase : IDisposable
    {

        private IKernel _Kernel;
        private GSLogisticsRepository _Repository;
        private GSLogisticsContext _Context;


        protected IKernel Kernel
        {
            get
            {
                return _Kernel;
            }
        }

        protected GSLogisticsContext DatabaseContext
        {
            get
            {
                return _Context;
            }
        }

        protected GSLogisticsRepository Repository
        {
            get
            {
                return _Repository;
            }
        }

        //protected Task<int> SaveChangesAsync()
        //{
        //    return 
        //}

        internal LogicBase(IKernel kernel)
        {
            if (kernel == null)
            {
                throw new ArgumentNullException("kernel");
            }

            _Kernel = kernel;

            _Context = Kernel.Get<GSLogisticsContext>();
            _Repository = Kernel.Get<GSLogisticsRepository>();
        }


        bool disposed = false;
        public void Dispose()
        {
            Dispose(true);
        }

        protected  virtual void Dispose(bool disposing)
        {
            if (disposed)
            {
                return;
            }

            if(disposing)
            {
                _Repository.Dispose();
                _Repository = null;

                _Context.Dispose();
                _Context = null;

            }

            disposed = true;
        }
    }
}
