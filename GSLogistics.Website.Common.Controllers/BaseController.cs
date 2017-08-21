using GSLogistics.Interface;
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

        private IUserContext _UserContext;

        protected IUserContext UserContext
        {
            get
            {
                return GetEmulatedUserContext();
            }
        }

        protected IUserContext GetEmulatedUserContext()
        {
            try
            {
                if (Session == null)
                {
                    return GetUserContext();
                }

                var start = DateTime.Now;

                var emulatedUserIdentifier = Session["EmulatedUser"];

                if (emulatedUserIdentifier == null)
                {
                    return GetUserContext();
                }
                else
                {
                    return emulatedUserIdentifier as IUserContext;
                }
            }
            catch (Exception e)
            {
               
                return GetUserContext();
            }
        }

        protected IUserContext GetUserContext()
        {
            if (_UserContext == null)
            {
                _UserContext = _kernel.Get<IUserContext>();
            }
            return _UserContext;
        }

    }
}
