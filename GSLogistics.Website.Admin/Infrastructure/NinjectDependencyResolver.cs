using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Moq;
using GSLogistics.Entities.Abstract;
using GSLogistics.Entities.Concrete;
using GSLogistics.Logic.Interface;
using GSLogisitics.Logic;
using GSLogistics.Logic;

namespace GSLogistics.Website.Admin.Infrastructure
{
    public class NinjectDependencyResolver : IDependencyResolver
    {
        private IKernel _kernel;

        public NinjectDependencyResolver(IKernel kernel)
        {
            _kernel = kernel;
            AddBindings();
        }

        public object GetService(Type serviceType)
        {
            return _kernel.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return _kernel.GetAll(serviceType);
        }

        private void AddBindings()
        {
            _kernel.Bind<GSLogistics.Entities.GSLogisticsContext>().ToSelf();
            _kernel.Bind<IRepository>().To<GSLogisticsRepository>();

            _kernel.Bind<IAuthProvider>().To<AccountLogic>();
            _kernel.Bind<IScacCodeLogic>().To<ScacCodeLogic>();
            _kernel.Bind<IOrderAppointmentLogic>().To<OrderAppointmentLogic>();
            _kernel.Bind<IDivisionLogic>().To<DivisionLogic>();
            _kernel.Bind<ICustomerLogic>().To<CustomerLogic>();
            _kernel.Bind<IAppointmentLogic>().To<AppointmentLogic>();
            _kernel.Bind<IUserLogic>().To<UserLogic>();
        }
    }
}