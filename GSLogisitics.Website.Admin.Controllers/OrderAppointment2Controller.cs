using GSLogistics.Website.Common.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Ninject;
using GSLogistics.Website.Admin.Models;
using GSLogistics.Logic.Interface;
using GSLogistics.Model.Query;

namespace GSLogistics.Website.Admin.Controllers
{
    public class OrderAppointment2Controller : BaseController
    {
        public OrderAppointment2Controller(IKernel kernel) : base(kernel)
        {
        }

        [HttpGet]
        public async Task<ActionResult> List()
        {
            var model = new OrderAppointmentsIndex_ViewModel();
           

            return await this.List(model);
        }

        [HttpPost]
        public async Task<ActionResult> List(OrderAppointmentsIndex_ViewModel model)
        {

            List<Models.OrderAppointment> orders = new List<Models.OrderAppointment>();

            using (var scacLogic = Kernel.Get<IScacCodeLogic>())
            {
                var shippingCompanies = await scacLogic.GetScacCodes();

                Dictionary<string, string> result2 = new Dictionary<string, string>();
                foreach (var sc in shippingCompanies)
                {
                    if (!result2.ContainsKey(sc.ScacCodeId.ToString()))
                    {
                        result2.Add(sc.ScacCodeId.ToString(), $"{sc.ScacCodeId.ToString()} {sc.Carrier}");
                    }
                }

                ViewBag.ScacCodes = new SelectList(result2, "Key", "Value", null);
            }

            var status = new Dictionary<string, string>();
            status.Add("1", "Posted");
            status.Add("2", "Pending");

            ViewBag.AppointmentStatus = new SelectList(status, "Key", "Value", null);
            using (var orderLogic = Kernel.Get<IOrderAppointmentLogic>())
            using (var divLogic = orderLogic.GetLogic<IDivisionLogic>())
            {

                var ordersforAppt = await orderLogic.ToListAsync(
                    new OrderAppointmentQuery()
                    {
                        CancelDateEndDate = model.CancelDateEndDate,
                        CancelDateStartDate = model.CancelDateStartDate,
                        CustomerId = model.SelectedClientId,
                        DivisionId = model.SelectedDivisionId,
                        ShipFor = model.ShipFor,
                        Status = 0
                    });

                foreach (var o in ordersforAppt)
                {
                    try
                    {
                        orders.Add(new Models.OrderAppointment() { BoxesNumber = o.BoxesCount.Value, BoxSize = o.BoxSize, CustomerId = o.CustomerId, CustomerName = o.CustomerName, EndDate = o.EndDate.Value, PickTicketId = o.PickTicketId, Pieces = o.Pieces.Value, PurchaseOrderId = o.PurchaseOrderId, StartDate = o.StartDate.Value, Volume = o.Size.Value, Weight = o.Weigth, StoreName = o.ShipTo, DivisionName = o.DivisionName, PtBulk = o.PtBulk, Notes = o.Notes, ConfirmationNumber = o.ConfirmationNumber, DivisionId = o.DivisionId, ShipFor = o.ShipFor });
                    }
                    catch (Exception exc)
                    {
                        throw exc;
                    }

                }

                model.OrderAppointments = orders;

                var clients = ordersforAppt.Select(x => new { Id = x.CustomerId, Name = x.CustomerName }).ToList();

                Dictionary<string, string> result = new Dictionary<string, string>();
                foreach (var c in clients)
                {
                    if (!result.ContainsKey(c.Id.ToString()))
                    {
                        result.Add(c.Id.ToString(), c.Name);
                    }
                }
                ViewBag.Customers = new SelectList(result, "Key", "Value", null);

                if (!string.IsNullOrEmpty(model.SelectedClientId))
                {
                    var divisions = await divLogic.GetDivisionByCustomerId(model.SelectedClientId); 
                    var divs = divisions.Select(d => new { Id = d.DivisionId, Name = d.DivisionName }).ToList();
                    //var divs = repository.GetDivisionByClient(model.SelectedClientId).Select(d => new { Id = d.DivisionId, Name = d.Description }).ToList();
                    Dictionary<int, string> result3 = new Dictionary<int, string>();
                    divs.ForEach(x => result3.Add(x.Id, x.Name));
                    ViewBag.Divisions = new SelectList(result3, "Key", "Value", null);
                }
                else
                {
                    ViewBag.Divisions = new SelectList(new Dictionary<int, string>(), "Key", "Value", null);
                }
            }

            return View("../OrderAppointment/List", model);
        }
    }
}
