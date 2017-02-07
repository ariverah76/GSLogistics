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
            //Mock<IRepository> mock = new Mock<IRepository>();
            //mock.Setup(m => m.OrderAppointments).Returns(new List<OrderAppointment>
            //{
            //    new OrderAppointment { AppointmentNumber = "1000001", BoxesNumber = 232, BoxSize = "MC5", CustomerId = 800500, PickTicketId = "70016052", Pieces = 1392, PurchaseOrderId = "015626292A", ShippingCompanyId = 900100, StoreId = 900, Volume = 329, Weight = 942, CustomerName ="ELOH", EstimatedShippingDate = new DateTime(2016,12,28) , StoreName = "SHOPKO-OMAHA", StartDate = new DateTime(2016,12,15), EndDate = new DateTime(2016,12,29) },
            //    new OrderAppointment { AppointmentNumber = "1000002", BoxesNumber = 151, BoxSize = "MC5", CustomerId = 800500, PickTicketId = "70016054", Pieces = 906, PurchaseOrderId = "015626292B", ShippingCompanyId = 900100, StoreId = 901, Volume = 214, Weight = 613, CustomerName ="ELOH", EstimatedShippingDate = new DateTime(2016,12,28) , StoreName = "SHOPKO-BOISE",StartDate = new DateTime(2016,12,11), EndDate = new DateTime(2016,12,28) },
            //    new OrderAppointment { AppointmentNumber = "1000003", BoxesNumber = 7, BoxSize = "MC10", CustomerId = 800500, PickTicketId = "60148585", Pieces = 192, PurchaseOrderId = "1044007", ShippingCompanyId = 900100, StoreId = 902, Volume = 20, Weight = 234, CustomerName ="ELOH", EstimatedShippingDate = new DateTime(2016,12,29) , StoreName = "DUTTY-FREE",StartDate = new DateTime(2016,12,16), EndDate = new DateTime(2016,12,29) },
            //    new OrderAppointment { AppointmentNumber = "1000004", BoxesNumber = 4, BoxSize = "MC12", CustomerId = 800500, PickTicketId = "70557189", Pieces = 168, PurchaseOrderId = "015626292A", ShippingCompanyId = 900100, StoreId = 902, Volume = 329, Weight = 942, CustomerName ="ELOH", EstimatedShippingDate = new DateTime(2016,12,29) , StoreName = "DUTTY-FREE",StartDate = new DateTime(2016,12,12), EndDate = new DateTime(2016,12,30) },
            //    new OrderAppointment { AppointmentNumber = "1000005", BoxesNumber = 704, BoxSize = "23X16X16", CustomerId = 800500, PickTicketId = "60145785", Pieces = 804, PurchaseOrderId = "389165", ShippingCompanyId = 900100, StoreId = 903, Volume = 77.5M, Weight = 994, CustomerName ="ELOH", EstimatedShippingDate = new DateTime(2016,12,30) , StoreName = "FORMAN-MILLS",StartDate = new DateTime(2016,12,11), EndDate = new DateTime(2016,12,27) },
            //    new OrderAppointment { AppointmentNumber = "1000006", BoxesNumber = 200, BoxSize = "23X16X16", CustomerId = 800500, PickTicketId = "60144358", Pieces = 1392, PurchaseOrderId = "428774", ShippingCompanyId = 900100, StoreId = 904, Volume = 51,  CustomerName ="ELOH", EstimatedShippingDate = new DateTime(2016,12,30) , StoreName = "DRESS BARN- 4150",StartDate = new DateTime(2016,12,12), EndDate = new DateTime(2016,12,31) },
            //    new OrderAppointment { AppointmentNumber = "1000007", BoxesNumber = 648, BoxSize = "MC5", CustomerId = 800500, PickTicketId = "60148492-8506", Pieces = 1392, PurchaseOrderId = "00118344511", ShippingCompanyId = 900100, StoreId = 905, Volume = 329, Weight = 912, CustomerName ="ELOH", EstimatedShippingDate = new DateTime(2016,12,30) , StoreName = "NEXCOM",StartDate = new DateTime(2016,12,25), EndDate = new DateTime(2016,12,30) },
            //    new OrderAppointment { AppointmentNumber = "1000008", BoxesNumber = 60, BoxSize = "MC10", CustomerId = 800500, PickTicketId = "60148587", Pieces = 1392, PurchaseOrderId = "22391974", ShippingCompanyId = 900100, StoreId = 906, Volume = 329, Weight = 545, CustomerName ="ELOH", EstimatedShippingDate = new DateTime(2016,12,30) , StoreName = "STAGE- JACKSONVILLE",StartDate = new DateTime(2016,12,1), EndDate = new DateTime(2016,12,31) },
            //    new OrderAppointment { AppointmentNumber = "1000009", BoxesNumber = 864, BoxSize = "MC5", CustomerId = 800500, PickTicketId = "60147322", Pieces = 12, PurchaseOrderId = "23991976", ShippingCompanyId = 900100, StoreId = 906, Volume = 329, Weight = 232, CustomerName ="ELOH", EstimatedShippingDate = new DateTime(2016,12,30) , StoreName = "STAGE- JACKSONVILLE",StartDate = new DateTime(2016,12,8), EndDate = new DateTime(2016,12,30) },
            //    new OrderAppointment { AppointmentNumber = "1000010", BoxesNumber = 120, BoxSize = "MC5", CustomerId = 800500, PickTicketId = "75057194", Pieces = 54, PurchaseOrderId = "015626292A", ShippingCompanyId = 900100, StoreId = 907, Volume = 329, Weight = 92, CustomerName ="ELOH", EstimatedShippingDate = new DateTime(2016,12,30) , StoreName = "STAGE- SOUTH HILL", StartDate = new DateTime(2016,12,3), EndDate = new DateTime(2016,12,28) },
            //    new OrderAppointment { AppointmentNumber = "1000011", BoxesNumber = 1885, BoxSize = "MC8", CustomerId = 800500, PickTicketId = "70017633", Pieces = 68, PurchaseOrderId = "1314473", ShippingCompanyId = 900100, StoreId = 907, Volume = 329, Weight = 9, CustomerName ="ELOH", EstimatedShippingDate = new DateTime(2016,12,30) , StoreName = "STAGE- SOUTH HILL", StartDate = new DateTime(2016,12,28), EndDate = new DateTime(2016,12,31) },
            //    new OrderAppointment { AppointmentNumber = "1000012", BoxesNumber = 608, BoxSize = "MC5", CustomerId = 800500, PickTicketId = "60147334", Pieces = 787, PurchaseOrderId = "1314472", ShippingCompanyId = 900100, StoreId = 908, Volume = 329, Weight = 75, CustomerName ="ELOH", EstimatedShippingDate = new DateTime(2016,12,31) , StoreName = "GORMANS- OMAHA", StartDate = new DateTime(2016,12,18), EndDate = new DateTime(2016,12,30) },
            //    new OrderAppointment { AppointmentNumber = "1000013", BoxesNumber = 255, BoxSize = "MC5", CustomerId = 800500, PickTicketId = "60148563", Pieces = 7893, PurchaseOrderId = "015626292A", ShippingCompanyId = 900100, StoreId = 909, Volume = 329, Weight = 223, CustomerName ="ELOH", EstimatedShippingDate = new DateTime(2017,1,5) , StoreName = "GORDMANS-CLAYTON", StartDate = new DateTime(2016,12,11), EndDate = new DateTime(2016,12,29) },
            //    new OrderAppointment { AppointmentNumber = "1000014", BoxesNumber = 1296, BoxSize = "MC5", CustomerId = 800500, PickTicketId = "60148572", Pieces = 434, PurchaseOrderId = "1698200", ShippingCompanyId = 900100, StoreId = 915, Volume = 35, Weight = 46, CustomerName ="ELOH", EstimatedShippingDate = new DateTime(2017,1,6) , StoreName = "MARINE WARSAW #60001", StartDate = new DateTime(2016,12,15), EndDate = new DateTime(2017,1,2) },
            //    new OrderAppointment { AppointmentNumber = "1000015", BoxesNumber = 522, BoxSize = "MC5", CustomerId = 800500, PickTicketId = "75056807-6868", Pieces = 869, PurchaseOrderId = "1698200", ShippingCompanyId = 900100, StoreId = 915, Volume = 329, Weight = 942, CustomerName ="ELOH", EstimatedShippingDate = new DateTime(2017,1,6) , StoreName = "MARINE WARSAW #60001",StartDate = new DateTime(2016,12,20), EndDate = new DateTime(2017,1,2) }
            //});

            //_kernel.Bind<IRepository>().ToConstant(mock.Object);

            _kernel.Bind<IRepository>().To<GSLogisticsRepository>();

            _kernel.Bind<IAuthProvider>().To<AccountLogic>();
        }
    }
}