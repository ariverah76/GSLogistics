using GSLogistics.Entities;
using GSLogistics.Entities.Concrete;
using GSLogistics.Logic.Interface;
using Ninject;
using Ninject.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSLogistics.Logic
{
    public class LogicBase : IGSLogisticsLogic, IDisposable
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

        public T_Logic GetLogic<T_Logic>()
           where T_Logic : IGSLogisticsLogic
        {
            return Kernel.Get<T_Logic>(
                new ConstructorArgument("kernel", Kernel));
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
