using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace GSLogistics.Website.Common.Controllers
{
    public abstract class BaseController : Controller
    {
        private IKernel _kernel;

        public BaseController(IKernel kernel)
        {
            this._kernel = kernel;
        }

        protected IKernel Kernel
        {
            get { return this._kernel; }
        }

    }
}
