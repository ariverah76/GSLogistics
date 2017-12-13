using GSLogistics.Logic.Interface;
using GSLogistics.Model.Query;
using GSLogistics.UserSecurity;
using GSLogistics.Website.Admin.Models;
using GSLogistics.Website.Admin.Models.OrderAppointments;
using GSLogistics.Website.Common;
using GSLogistics.Website.Common.Controllers;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace GSLogistics.Website.Admin.Controllers
{
    [Authorize]
    public class OrderAppointmentController :  BaseController
    {

        private const string kMasterBol = "M565000009999";

        private const string kValidateShippingDateSetting = "DisableShippingDateValidation";

        public OrderAppointmentController(IKernel kernel)
            :base(kernel)
        {
            //this.repository = orderApptRepository;
        }

        //public OrderAppointmentController(IKernel kernel)
        //    : base(kernel)
        //{

        //}


        public PartialViewResult GetOrdersForAppointment(OrderAppointmentsIndex_ViewModel model)
        {
            List<Models.OrderAppointment> orders = new List<Models.OrderAppointment>();

            using (var orderLogic = Kernel.Get<IOrderAppointmentLogic>())
            using (var divLogic = orderLogic.GetLogic<IDivisionLogic>())
            {

                var query = new OrderAppointmentQuery()
                {
                    CancelDateEndDate = model.CancelDateEndDate,
                    CancelDateStartDate = model.CancelDateStartDate,
                    CustomerId = model.SelectedClientId,
                    DivisionId = model.SelectedDivisionId,
                    ShipFor = model.ShipFor,
                    Status = 0
                };

                orders = this.GetOrders(orderLogic, query);

                model.OrderAppointments = orders;

            }

            return PartialView("_OrderList", orders);
        }

        [HttpGet]
        [GSDenyAttribute(Roles = "Clients")]
        public async  Task<ActionResult> List()
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
                        result2.Add(sc.ScacCodeId.ToString().Trim(), $"{sc.ScacCodeId.ToString()} {sc.Carrier}");
                    }
                }

                ViewBag.ScacCodes = new SelectList(result2, "Key", "Value", null);
            }

            var shippingDateSetting = Utility.Utility.ReadSetting(kValidateShippingDateSetting);

            bool disableShippingDateValidation = true;

            bool.TryParse(shippingDateSetting, out disableShippingDateValidation);

            ViewBag.ValidateShippingDate = !disableShippingDateValidation;


            var status = new Dictionary<string, string>();
            status.Add("1", "Posted");
            status.Add("2", "Pending");

            using(var tLogic = Kernel.Get<ITruckLogic>())
            {
                var listTrucks = await tLogic.GetTrucks();
                Dictionary<int, string> trucks = new Dictionary<int, string>();
                foreach (var truck in listTrucks)
                {
                    if (!trucks.ContainsKey(truck.TruckId))
                    {
                        trucks.Add(truck.TruckId, $"{truck.Plates} {truck.Description}");
                    }
                }

                ViewBag.Trucks = new SelectList(trucks, "Key", "Value", null);

            }
            using (var driverLogic = Kernel.Get<IDriverLogic>())
            {
                var listDrivers = await driverLogic.GetDrivers();
                Dictionary<int, string> drivers = new Dictionary<int, string>();
                foreach (var driver in listDrivers)
                {
                    if (!drivers.ContainsKey(driver.DriverId))
                    {
                        drivers.Add(driver.DriverId, $"{driver.Name} {driver.SurName}");
                    }
                }

                ViewBag.Drivers = new SelectList(drivers, "Key", "Value", null);

            }

            ViewBag.AppointmentStatus = new SelectList(status, "Key", "Value", null);
            using (var orderLogic = Kernel.Get<IOrderAppointmentLogic>())
            using (var divLogic = orderLogic.GetLogic<IDivisionLogic>())
            using (var custLogic = Kernel.Get<ICustomerLogic>())
            {

                var query = new OrderAppointmentQuery()
                {
                    CancelDateEndDate = model.CancelDateEndDate,
                    CancelDateStartDate = model.CancelDateStartDate,
                    CustomerId = model.SelectedClientId,
                    DivisionId = model.SelectedDivisionId,
                    ShipFor = model.ShipFor,
                    Status = 0
                };

                //orders = await this.GetOrders(orderLogic, query);

                //var ordersforAppt = await orderLogic.ToListAsync(
                //    new OrderAppointmentQuery()
                //    {
                //        CancelDateEndDate = model.CancelDateEndDate,
                //        CancelDateStartDate = model.CancelDateStartDate,
                //        CustomerId = model.SelectedClientId,
                //        DivisionId = model.SelectedDivisionId,
                //        ShipFor = model.ShipFor,
                //        Status = 0
                //    });

                //foreach (var o in ordersforAppt)
                //{
                //    try
                //    {
                //        orders.Add(new Models.OrderAppointment() { BoxesNumber = o.BoxesCount.Value, BoxSize = o.BoxSize, CustomerId = o.CustomerId, CustomerName = o.CustomerName, EndDate = o.EndDate.Value, PickTicketId = o.PickTicketId, Pieces = o.Pieces.Value, PurchaseOrderId = o.PurchaseOrderId, StartDate = o.StartDate.Value, Volume = o.Size.Value, Weight = o.Weigth, StoreName = o.ShipTo, DivisionName = o.DivisionName, PtBulk = o.PtBulk, Notes = o.Notes, ConfirmationNumber = o.ConfirmationNumber, DivisionId = o.DivisionId, ShipFor = o.ShipFor });
                //    }
                //    catch (Exception exc)
                //    {
                //        throw exc;
                //    }

                //}

                // model.OrderAppointments = orders;

                var clients = await custLogic.ToListAsync();// orders.Select(x => new { Id = x.CustomerId, Name = x.CustomerName }).ToList();

                Dictionary<string, string> result = new Dictionary<string, string>();
                foreach (var c in clients)
                {
                    if (!result.ContainsKey(c.CustomerId.ToString()))
                    {
                        result.Add(c.CustomerId.ToString(), c.CompanyName);
                    }
                }
                ViewBag.Customers = new SelectList(result, "Key", "Value", null);

                if (!string.IsNullOrEmpty(model.SelectedClientId))
                {
                    var divisions = await divLogic.GetDivisionByCustomerId(model.SelectedClientId);
                    var divs = divisions.Select(d => new { Id = d.DivisionId, Name = d.Description }).ToList();

                    Dictionary<int, string> result3 = new Dictionary<int, string>();
                    divs.ForEach(x => result3.Add(x.Id, x.Name));
                    ViewBag.Divisions = new SelectList(result3, "Key", "Value", null);
                }
                else
                {
                    ViewBag.Divisions = new SelectList(new Dictionary<int, string>(), "Key", "Value", null);
                }
            }

            return View("List", model);
        }

        private List<OrderAppointment> GetOrders(IOrderAppointmentLogic orderLogic, OrderAppointmentQuery query)
        {
            List<Models.OrderAppointment> orders = new List<Models.OrderAppointment>();

            var ordersforAppt =  orderLogic.ToList(query);

            foreach (var o in ordersforAppt)
            {
                try
                {
                    orders.Add(new Models.OrderAppointment() { BoxesNumber = o.BoxesCount.Value, BoxSize = o.BoxSize, CustomerId = o.CustomerId, CustomerName = o.CustomerName, EndDate = o.EndDate.Value, PickTicketId = o.PickTicketId, Pieces = o.Pieces.Value, PurchaseOrderId = o.PurchaseOrderId, StartDate = o.StartDate.Value, Volume = o.Size.Value, Weight = o.Weigth, StoreName = o.ShipTo, DivisionName = o.DivisionName, PtBulk = o.PtBulk, Notes = o.Notes, ConfirmationNumber = o.ConfirmationNumber, DivisionId = o.DivisionId, ShipFor = o.ShipFor, BillOfLading = o.BillOfLading, ScacCode = o.ScacCode , DeliveryTypeId = o.DeliveryTypeId, Shipping = o.Shipping});
                }
                catch (Exception exc)
                {
                    throw exc;
                }

            }

            return orders;

        }
        public async Task<ActionResult> GenerateOrdersReport(OrderAppointmentsIndex_ViewModel model)
        {

            var query = new OrderAppointmentQuery()
            {
                CancelDateEndDate = model.CancelDateEndDate,
                CancelDateStartDate = model.CancelDateStartDate,
                CustomerId = model.SelectedClientId,
                DivisionId = model.SelectedDivisionId,
                ShipFor = model.ShipFor,
                Status = 0
            };

            var title = new StringBuilder();
           

            using (var orderLogic = Kernel.Get<IOrderAppointmentLogic>())
            using (var customerLogic = orderLogic.GetLogic<ICustomerLogic>())
            using (var divLogic = orderLogic.GetLogic<IDivisionLogic>())
            {

                var appointments =  this.GetOrders(orderLogic, query);



                if (model.CancelDateStartDate.HasValue)
                {
                    title.AppendLine($"Start Date : {model.CancelDateStartDate.Value.ToShortDateString()}");
                }
                if (model.CancelDateEndDate.HasValue)
                {
                    title.AppendLine($"End Date : {model.CancelDateEndDate.Value.ToShortDateString()}");
                }

                if (!string.IsNullOrEmpty(model.SelectedClientId))
                {
                    var custNmae = await customerLogic.FirstOrDefaultAsync(model.SelectedClientId);
                    title.AppendLine($"Customer : {custNmae.CompanyName ?? "" }");
                }
                if (model.SelectedDivisionId.HasValue)
                {
                    var div = await divLogic.GetFirstOrDefaultAsync(model.SelectedDivisionId.Value);
                    title.AppendLine($"Division : {div.Name} {div.Description}");
                }


                var reportingService = new Reporting.ReportingService();

                string mimeType = null;
                Dictionary<string, string> reportParameters = new Dictionary<string, string>()
                {
                    { "Title", title.ToString() }
                };

                var result = reportingService.RenderReport(appointments, "OrderReport.rdlc", "DataSet1", "pdf", out mimeType, reportParameters);

                var fName = string.Format("OrderReport_{0}", DateTime.Now.ToShortDateString().Replace("/", "").Replace(":", ""));
                Session[fName] = result;

                return Json(new { success = true, fName, mimeType, ReportFormat = "pdf" }, JsonRequestBehavior.AllowGet);
            }

        }

       

       [HttpGet]
       public async Task<ActionResult> SearchPickTicket(string pickTicketId)
        {
            using (var appointmentLogic = Kernel.Get<IAppointmentLogic>())
            {

                var pts = await appointmentLogic.ToListAsync(new AppointmentQuery()
                {
                    PickTicketId = pickTicketId
                });
                var pt = pts.FirstOrDefault(); 
               // var pt = repository.Appointments.Where(x => x.PickTicket == pickTicketId).FirstOrDefault();

                if (pt != null)
                {
                    return Json(new { result = "Success", appointmentNumber = pt.AppointmentNumber, IsPosted = pt.Posted, shippingDate = pt.ShippingDate, carrier = pt.Carrier, pickTicketId = pickTicketId }, JsonRequestBehavior.AllowGet);
                }

                return Json(new { result = "NotFound", pickTicketId = pickTicketId }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public async Task<ActionResult> SearchPurchaseOrder(string purchaseOrder)
        {
            using (var appointmentLogic = Kernel.Get<IAppointmentLogic>())
            using (var orderLogic = appointmentLogic.GetLogic<IOrderAppointmentLogic>())
            {
                var orders = await orderLogic.ToListAsync(new OrderAppointmentQuery()
                {
                    PurchaseOrder = purchaseOrder
                });

                if (orders.Any())
                {
                    IList<string> pickticketIds = new List<string>();
                    foreach (var o in orders)
                    {
                        pickticketIds.Add(o.PickTicketId);
                    }

                    var appts = await appointmentLogic.ToListAsync(new AppointmentQuery()
                    {
                        PickTicketsIds = pickticketIds.ToArray()
                    });
                    var pt = appts.FirstOrDefault();

                    if (pt != null)
                    {
                        return Json(new { result = "Success", appointmentNumber = pt.AppointmentNumber, IsPosted = pt.Posted, shippingDate = pt.ShippingDate, carrier = pt.Carrier, pickTicketId = pt.PickTicket, purchaseOrder = orders.FirstOrDefault().PurchaseOrderId }, JsonRequestBehavior.AllowGet);
                    }
                }

                return Json(new { result = "NotFound", purchaseOrder = purchaseOrder }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public async Task<ActionResult> SearchByBol(string bol)
        {
            using (var orderLogic = Kernel.Get<IOrderAppointmentLogic>())
            {
                if (!string.IsNullOrEmpty(bol))
                {
                    var orders = await orderLogic.ToListAsync(new OrderAppointmentQuery()
                    {
                        BillOfLading = bol,
                        Status = 0
                    });

                    if (orders != null && orders.Any())
                    {
                        return Json(new { result = "Success", orders = orders }, JsonRequestBehavior.AllowGet);
                    }
                }

                return Json(new { result = "NotFound" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [Authorize(Roles = "Administrators")]
        public async Task<ActionResult> SetAppointment(NewAppointment_ViewModel model)
        {
            //No needed for now!!!

            //using (var woContext = new GSLogistics.Entities.GSExternalContext())
            //{

            //    var scriptToRun = Resources.Scripts.OrderStatusByPickTicket;

            //    var clientId = model.Orders.FirstOrDefault().CustomerId;
            //    var picktickets = model.Orders.Select(x => x.PickTicketId).ToList();

            //    string pts = string.Empty;
            //    picktickets.ForEach(x => pts += string.Format("'{0}',", x));
            //    pts = pts.Substring(0,pts.Length - 1);

            //    scriptToRun = string.Format(scriptToRun, clientId, pts);

            //    var result = woContext.Database.SqlQuery<OrderStatus>(scriptToRun).ToList();

            //    return Json(new { result = "Error" });
            //}

            // TODO: Bring all the appts  with same BOL and post them all at once (just in case something were missing on the client side)
            StringBuilder sb = new StringBuilder();

            foreach(var order in model.Orders)
            {
                if (string.IsNullOrEmpty(order.CustomerId) || order.DivisionId == 0 || string.IsNullOrEmpty(order.PickTicketId) || string.IsNullOrEmpty(order.PurchaseOrderId))
                {
                    sb.AppendLine($"Order has missing info. Purchase Order: {order.PurchaseOrderId}, PickTicket: {order.PickTicketId} Customer: {order.CustomerId} Division: {order.DivisionId}");
                }
            }
            List<Model.OrderAppointment> orders = new List<Model.OrderAppointment>();

            if (sb.Length == 0)
            {
                using (var ol = Kernel.Get<IOrderAppointmentLogic>())
                using (var apptLogic = ol.GetLogic<IAppointmentLogic>())
                {

                   

                    foreach (var order in model.Orders)
                    {

                        Model.Appointment appointment = new Model.Appointment();
                        appointment.CustomerId = order.CustomerId;
                        appointment.PickTicket = order.PickTicketId;
                        appointment.DivisionId = order.DivisionId == 0 ? default(int?) : order.DivisionId;

                        appointment.ScacCode = model.ScacCode;
                        appointment.ShippingDate = model.ShippingDate;
                        appointment.ShippingTime = new DateTime(model.ShippingDate.Year, model.ShippingDate.Month, model.ShippingDate.Day, model.ShippingTime.Hour, model.ShippingTime.Minute, 0);
                        appointment.AppointmentNumber = model.AppointmentNumber;
                        appointment.DeliveryTypeId = model.DeliveryTypeId;
                        appointment.UserName = User.Identity.Name;
                        appointment.Pallets = model.Pallets;
                        appointment.DriverId = model.DriverId;
                        appointment.TruckId = model.TruckId;

                        if (model.ShippingTimeLimit.HasValue)
                        {
                            appointment.ShippingTimeLimit = new DateTime(model.ShippingDate.Year, model.ShippingDate.Month, model.ShippingDate.Day, model.ShippingTimeLimit.Value.Hour, model.ShippingTimeLimit.Value.Minute, 0);
                        }

                        appointment.PtBulk = order.PtBulk;


                        Model.OrderAppointment oappt = new Model.OrderAppointment()
                        {
                            PurchaseOrderId = order.PurchaseOrderId,
                            PickTicketId = order.PickTicketId,
                            PtBulk = order.PtBulk,
                            CustomerId = order.CustomerId,
                            ShipFor = model.ShippingDate,
                            Notes = model.Notes,
                            Status = 1
                        };
                        orders.Add(oappt);
                   

                    }
                    string result = string.Empty;
                    try
                    {

                        result = await apptLogic.SetAppointment(model,orders.ToArray(), User.Identity.Name);
                    }
                    catch
                    {

                        //return new HttpStatusCodeResult(HttpStatusCode.ExpectationFailed, result);

                    }

                    if (!string.IsNullOrEmpty(result))
                    {
                        sb.AppendLine($"The attempt of appointment got the follow error: {result}");
                    }
                }
            }

            return Json(new { result = sb.ToString(), url = "OrderAppointment/List" });
        }

        [HttpPost]
        [Authorize(Roles = "Administrators")]
        public async Task<ActionResult> SetConfirmationNumber(NewAppointment_ViewModel model)
        {
            using (var logic = Kernel.Get<IOrderAppointmentLogic>())
            {
                foreach (var order in model.Orders)
                {
                    Model.OrderAppointment oappt = new Model.OrderAppointment()
                    {
                        PurchaseOrderId = order.PurchaseOrderId,
                        PickTicketId = order.PickTicketId,
                        PtBulk = order.PtBulk,
                        CustomerId = order.CustomerId,
                        ConfirmationNumber = model.ConfirmationNumber
                    };
                    //Entities.OrderAppointment oappt = new Entities.OrderAppointment() { PurchaseOrderId = order.PurchaseOrderId, PickTicketId = order.PickTicketId, PtBulk = order.PtBulk, CustomerId = order.CustomerId, ConfirmationNumber = model.ConfirmationNumber };
                    await logic.Update(oappt);
                    //repository.UpdateOrderAppointment(oappt);

                }
            }
            return Json(new { url = "OderAppointment/List" });
        }

        [HttpPost]
        [Authorize(Roles = "Administrators")]
        public async Task<ActionResult> ActionAppointments(ActionAppointment model)
        {
            StringBuilder sb = new StringBuilder();
            // TODO: Validate there is not an existing appointment already posted
            using (var appointmentLogic = Kernel.Get<IAppointmentLogic>())
            using (var ol = appointmentLogic.GetLogic<IOrderAppointmentLogic>())
            {
                foreach (var appt in model.Appointments)
                {
                    Model.Appointment appointment = new Model.Appointment()
                    {
                        CustomerId = appt.CustomerId,
                        PickTicket = appt.PickTicket,
                        AppointmentNumber = appt.AppointmentNo,
                        Posted = appt.Posted.Value,
                        Status = appt.Status
                    };

                    var result = await appointmentLogic.Update(appointment);

                    if (string.IsNullOrEmpty(result))
                    {
                        if (model.Action == AppointmentAction.Cancel && (!appt.Posted.Value && appt.Status == "D"))
                        {
                            Model.OrderAppointment orderAppointment = new Model.OrderAppointment()
                            {
                                CustomerId = appt.CustomerId,
                                PickTicketId = appt.PickTicket,
                                PurchaseOrderId = appt.PurchaseOrderId,
                                Status = 0
                            };

                            await ol.Update(orderAppointment);
                        }
                    }
                    else
                    {
                        sb.AppendLine($"Order: {appt.PickTicket} Client : {appt.CustomerId} failed to be posted. Error : {result}");
                    }
                    //await repository.UpdateAppointment(appointment);
                }
            }

            return Json(new { result = sb.ToString(),  url = "OrderAppointment/List" });
        }

        [HttpPost]
        [Authorize(Roles = "Administrators")]
        public async Task<ActionResult> SaveEditedAppointment(NewAppointment_ViewModel model)
        {
            using (var logic = Kernel.Get<IAppointmentLogic>())
            {
                foreach (var appt in model.Orders)
                {
                    Model.Appointment appointment = new Model.Appointment()
                    {
                        CustomerId = appt.CustomerId,
                        AppointmentNumber = model.AppointmentNumber,
                        PickTicket = appt.PickTicketId,
                        PtBulk = appt.PtBulk,
                        ShippingDate = model.ShippingDate,
                        ShippingTime = new DateTime(model.ShippingDate.Year, model.ShippingDate.Month, model.ShippingDate.Day, model.ShippingTime.Hour, model.ShippingTime.Minute, 0),
                        ScacCode = model.ScacCode,
                        DateAdded = appt.DateAdded,
                        DeliveryTypeId = model.DeliveryTypeId,
                        ReScheduleDate = model.ReScheduleDate,
                        Pallets = model.Pallets,
                        TruckId = model.TruckId,
                        DriverId = model.DriverId
                    };

                    if (model.ShippingTimeLimit.HasValue)
                    {
                        appointment.ShippingTimeLimit = new DateTime(model.ShippingDate.Year, model.ShippingDate.Month, model.ShippingDate.Day, model.ShippingTimeLimit.Value.Hour, model.ShippingTimeLimit.Value.Minute, 0);
                    }
                    await logic.UpdateScript(appointment, model.Notes);
                    //await logic.Update(appointment);

                   // repository.UpdateAppointment(appointment);
                }
            }
            return Json(new { url = "OrderAppointment/List" });
        }


        [HttpPost]
        public async Task<ActionResult> UpdateOrderAppointment(UpdateOrderAppointment update)
        {
            using (var logic = Kernel.Get<IOrderAppointmentLogic>())
            {
                Model.OrderAppointment oappt = new Model.OrderAppointment()
                {
                    PurchaseOrderId = update.PurchaseOrderId,
                    PickTicketId = update.PickTicketId,
                    PtBulk = update.PtBulk,
                    CustomerId = update.CustomerId,
                    Notes = update.Notes
                };

                //Entities.OrderAppointment oappt = new Entities.OrderAppointment() { PurchaseOrderId = update.PurchaseOrderId, PickTicketId = update.PickTicketId, PtBulk = update.PtBulk, CustomerId = update.CustomerId, Notes = update.Notes };

                await logic.Update(oappt);
            //repository.UpdateOrderAppointment(oappt);
            }
            return Json(new { url = "List" });
        }
        
        

        [HttpPost]
        public async Task<ActionResult> UpdateAppointment(UpdateAppointment update)
        {

            Model.Appointment appt = new Model.Appointment() { CustomerId = update.CustomerId, PickTicket = update.PickTicket, AppointmentNumber = update.AppointmentNo, Posted = update.Posted ?? false, Status = update.Status};
            using (var logic = Kernel.Get<IAppointmentLogic>())
            {
                var x = await logic.Update(appt);
            }
                //await repository.UpdateAppointment(appt);

            return Json(new { url = "List" });
        }

        public PartialViewResult GetAppointments(OrderAppointmentsIndex_ViewModel model)
        {
            var query = new AppointmentQuery();
            List<Models.Appointment> appointments = new List<Models.Appointment>();

            if (!string.IsNullOrEmpty(model.SelectedStatus))
            {
                if (model.SelectedStatus == "1")
                {
                    query.Posted = true;
                }
                else
                {
                    query.Posted = false;
                }
            }
            else
            {
                query.Posted = false;
            }

            if (!string.IsNullOrEmpty(model.AppointmentNumberSearch))
            {
                query.AppointmentNumber = model.AppointmentNumberSearch;
            }

            if (model.ShippingDateStart.HasValue)
            {
                query.ShippingDateStart = model.ShippingDateStart.Value;
            }

            if (model.ShippingDateEnd.HasValue)
            {
                query.ShippingDateEnd = model.ShippingDateEnd.Value;
            }

            if (model.ShipForAppt.HasValue)
            {
                query.ShipFor = model.ShipForAppt;
            }

            query.Status = "A";

            using (var aLogic = Kernel.Get<IAppointmentLogic>())
            using (var oLogic = Kernel.Get<IOrderAppointmentLogic>())
            {
                var orderAppts = oLogic.ToList(new OrderAppointmentQuery());
                var list2 = aLogic.ToList(query);

                foreach (var appt in list2)
                {
                    var thisAppointment = this.GetAppointmentWebModel(appt, orderAppts);
                    

                    appointments.Add(thisAppointment);
                }

                model.Appointments = appointments;
            }
            return PartialView("_AppointmentList",appointments);

        }



        #region LogReport

        private List<Models.Appointment> GetAppointments(LogReportIndex_ViewModel model)
        {
            List<Models.Appointment> appointments = new List<Models.Appointment>();



            var query = new AppointmentQuery()
            {
                Posted = true,
                IsReschedule = false,
                KeyColumnSearch = true,
            };

            var userContext = Session["UserContext"] as GSLogisticsUserContext;
            if (userContext != null)
            {
                query.CustomerIds = userContext.CustomerIds.ToArray();
                query.DivisionIds = userContext.DivisionIds.ToArray();
            }

            if (model.SelectedDay.HasValue)
            {
                query.ShippingDate = model.SelectedDay.Value;
            }

            if (model.DeliveryTypeId.HasValue)
            {
                query.DeliveryTypeId = model.DeliveryTypeId.Value;
            }

            if (!string.IsNullOrEmpty(model.SelectedClientId))
            {
                query.CustomerId = model.SelectedClientId;
            }
            else if (model.AvailableClientIds != null && model.AvailableClientIds.Any())
            {
                query.CustomerIds = model.AvailableClientIds;
            }


            if (model.SelectedDivisionId.HasValue && model.SelectedDivisionId.Value != 0)
            {
                query.DivisionId = model.SelectedDivisionId.Value;
            }
            else if (model.AvailableDivisionIds != null && model.AvailableDivisionIds.Any())
            {
                query.DivisionIds = model.AvailableDivisionIds;
            }

            if (!string.IsNullOrEmpty(model.SelectedPickTicket))
            {
                query.ShippingDate = null;
                query.KeySearch = model.SelectedPickTicket;
            }

            using (var oLogic = Kernel.Get<IOrderAppointmentLogic>())
            using (var aLogic = oLogic.GetLogic<IAppointmentLogic>())
            {

                var orderAppts = oLogic.ToList(new OrderAppointmentQuery());
                var list = aLogic.ToList(query);



                foreach (var appt in list)
                {
                    var thisAppointment = new Models.Appointment()
                    {
                        AppointmentNo = appt.AppointmentNumber,
                        CustomerName = appt.CustomerName,
                        CustomerId = appt.CustomerId,
                        DivisionId = appt.DivisionId,
                        DivisionName = appt.DivisionName,
                        DivisionNameId = appt.DivisionNameId,
                        Carrier = appt.Carrier,
                        PickTicket = appt.PickTicket,
                        PtBulk = appt.PtBulk,
                        ScaccCode = appt.ScacCode,
                        ShipDate = appt.ShippingDate.Value,
                        ShipTime = appt.ShippingTime.Value,
                        Posted = appt.Posted.ToString(),
                        DateAdded = appt.DateAdded,
                        DeliveryTypeId = appt.DeliveryTypeId,
                        DriverName = appt.DriverName,
                        DriverId = appt.DriverId,
                        Pallets = appt.Pallets,
                        Status = appt.Status,
                        ReScheduleCount = appt.RescheduleCount
                    };

                    var orderAppt = orderAppts.Where(x => x.CustomerId == thisAppointment.CustomerId && x.PickTicketId == thisAppointment.PickTicket).FirstOrDefault();
                    if (orderAppt != null)
                    {
                        thisAppointment.PurchaseOrder = orderAppt.PurchaseOrderId;
                        thisAppointment.Pieces = orderAppt.Pieces.Value;
                        thisAppointment.BoxesNumber = orderAppt.BoxesCount.Value;
                        thisAppointment.ShipTo = orderAppt.ShipTo;
                        thisAppointment.BillOfLading = string.IsNullOrEmpty(orderAppt.BillOfLading) ? $"BOL{thisAppointment.AppointmentNo}" : orderAppt.BillOfLading;
                        thisAppointment.pathPOD = orderAppt.PathPOD;
                        thisAppointment.AnyChildBolHasPOD = !string.IsNullOrEmpty(orderAppt.PathPOD);
                        thisAppointment.ExternalBol = orderAppt.ExternalBol;
                        thisAppointment.MasterBillOfLading = orderAppt.MasterBillOfLading;
                        thisAppointment.Notes = orderAppt.Notes;
                        

                        //remove this after tests 
                        //if (orderAppt.BillOfLading == "06799500002077790" || orderAppt.BillOfLading == "06799500002077806" || orderAppt.BillOfLading == "06799500002077820")
                        //{
                        //    if (orderAppt.BillOfLading == "06799500002077806")
                        //    {
                        //        thisAppointment.pathPOD = @"C:\\temp\Alex.pdf";
                        //    }
                        //    thisAppointment.MasterBillOfLading = kMasterBol;
                        //}

                        //if (orderAppt.BillOfLading == "08828880002077934" || orderAppt.BillOfLading == "08828880002077842")
                        //{
                           
                        //    thisAppointment.MasterBillOfLading = kMasterBol;
                        //}
                    }

                    appointments.Add(thisAppointment);
                }

                var moveables = appointments.Where(x => !string.IsNullOrEmpty(x.MasterBillOfLading)).ToList();

                List<Models.Appointment> masterBols = new List<Models.Appointment>();
                masterBols.AddRange(moveables);
                appointments.RemoveAll(x => !string.IsNullOrEmpty(x.MasterBillOfLading));

                var grouped = masterBols.GroupBy(x => x.MasterBillOfLading);
                foreach (var g in grouped)
                {

                    var orders = g.GroupBy(x => x.BillOfLading);
                    foreach (var o in orders)
                    {
                        var singleOrder = o.FirstOrDefault();

                        var thisAppointment = new Models.Appointment()
                        {
                            AppointmentNo = singleOrder.AppointmentNo,
                            CustomerName = singleOrder.CustomerName,
                            CustomerId = singleOrder.CustomerId,
                            Carrier = singleOrder.Carrier,
                            ScaccCode = singleOrder.ScaccCode,
                            ShipDate = singleOrder.ShipDate,
                            ShipTime = singleOrder.ShipTime,
                            Posted = singleOrder.Posted.ToString(),
                            DateAdded = singleOrder.DateAdded,
                            Pieces = o.Sum(x => x.Pieces),
                            BoxesNumber = o.Sum(x => x.BoxesNumber),
                            PickTicket = $"BOL:{o.Key}",
                            BillOfLading = g.Key,
                            ShipTo = singleOrder.ShipTo,
                            MasterBillOfLading = g.Key,
                            AnyChildBolHasPOD = g.Any(x => !string.IsNullOrEmpty(x.pathPOD)),
                            ExternalBol = o.Any(x => x.ExternalBol),
                            DeliveryTypeId =singleOrder.DeliveryTypeId,
                            pathPOD = o.FirstOrDefault(x => !string.IsNullOrEmpty(x.pathPOD))?.pathPOD,
                            Notes = singleOrder.Notes

                        };

                        appointments.Add(thisAppointment);
                    }

                }

            }
          

            appointments = appointments.OrderBy(x => x.PickTicket).ToList();
            return appointments;
        }

        [HttpPost]
        public async Task<ActionResult> RenderMasterPOD(string data)
        {
            string bol = data.Trim();
            using (var oLogic = Kernel.Get<IOrderAppointmentLogic>())
            {
                var orders = await oLogic.ToListAsync(new OrderAppointmentQuery() { MasterBillOfLading = bol });
                List<string> renderPods = new List<string>(); 
                if (orders.Any())
                {
                    //var mockBols = new List<string> { "06799500002077790", "06799500002077806", "06799500002077820" };
                    //for (var i = 0; i < 3; i++)
                    //{
                    //    var fName = this.PutPODFileOnSession(@"C:\\Temp\hunter.pdf", mockBols.ToArray()[i]);
                    //    renderPods.Add(fName);
                    //}


                    var childBols = orders.GroupBy(x => x.BillOfLading);
                    foreach(var g in childBols)
                    {
                        var order = g.FirstOrDefault(x => !string.IsNullOrEmpty(x.PathPOD));

                        var fName = this.PutPODFileOnSession(order.PathPOD, order.BillOfLading);

                        if (!string.IsNullOrEmpty(fName))
                        {
                            renderPods.Add(fName);
                        }

                    }
                     
                }

                return Json(new { success = true, pods = renderPods.ToArray(), mimeType = "application/pdf", format = "pdf" }, JsonRequestBehavior.AllowGet);

            }
        }


        [HttpPost]
        public async Task<ActionResult> RenderPOD(string data)
        {
            StringBuilder sb = new StringBuilder();
            string bol = data.Trim();
            string filePath = string.Empty;
            string path;
        
            using (var oLogic = Kernel.Get<IOrderAppointmentLogic>())
            {
                var orders = await oLogic.ToListAsync(new OrderAppointmentQuery() { BillOfLading = bol });

                if (orders != null && orders.Any())
                {
                    var order = orders.FirstOrDefault(x => !string.IsNullOrEmpty(x.PathPOD.Trim()));

                    if (order != null)
                    {
                       
                        Uri uriAddress = new Uri(order.PathPOD.Trim());

                        string podPath = $"{uriAddress.Segments[2]}{uriAddress.Segments[3]}";

                         path = Server.MapPath($"~/podfiles/{podPath}");// order.PathPOD;

                        if (System.IO.File.Exists(path))
                        {
                            var byteResult = System.IO.File.ReadAllBytes(path);

                            var fName = string.Format("ProofOfDelivery_BillOfLading_{0}", data);
                            Session[fName] = byteResult;

                            return Json(new { success = true, fileName = fName, mimeType = "application/pdf", format = "pdf" }, JsonRequestBehavior.AllowGet);

                        }
                        else
                        {
                            sb.Append("File does not exists or its name has changed");
                            return Json(new { success = false, message = sb.ToString(), path = path }, JsonRequestBehavior.AllowGet);
                        }

                    }

                }

                // var filePath = Server.MapPath("/PodFiles");
                sb.Append("Document is not available yet");
                return Json(new { success = false, message = sb.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }

        private string PutPODFileOnSession(string pathPOD, string bol)
        {
            Uri uriAddress = new Uri(pathPOD.Trim());
            string fName = null;
            string podPath = $"{uriAddress.Segments[2]}{uriAddress.Segments[3]}";

            var path = Server.MapPath($"~/podfiles/{podPath}");
            var mockPath = @"C:\\temp\hunter.pdf";
            path = mockPath;

            if (System.IO.File.Exists(path))
            {
                var byteResult = System.IO.File.ReadAllBytes(path);

                fName = string.Format("ProofOfDelivery_BillOfLading_{0}", bol);
                Session[fName] = byteResult;
            }

            return fName;
        }
        /// <summary>
        /// Set a BOL as 'Reschedule  from Log Report
        /// </summary>
        /// <param name="bol">bol string</param>
        /// <param name="appointmentnumber">appt #</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> RescheduleBol(string bol, string appointmentnumber)
        {
            StringBuilder sb = new StringBuilder();
            using (var oLogic = Kernel.Get<IOrderAppointmentLogic>())
            using (var aLogic = Kernel.Get<IAppointmentLogic>())
            {

                if (string.IsNullOrEmpty(bol))
                {
                    sb.AppendLine("Missing Bill Of Lading Number.");
                }
                else
                {
                    IList<Model.Appointment> appointments = new List<Model.Appointment>();

                    // check if it is a master bol
                    var orders = await oLogic.ToListAsync(new OrderAppointmentQuery() { MasterBillOfLading = bol });

                    if (orders.Any())
                    {
                        appointments = await aLogic.ToListAsync(new AppointmentQuery() { MasterBillOfLading = bol, Posted = true, Status = "A" });

                    }
                    else
                    {
                        appointments = await aLogic.ToListAsync(new AppointmentQuery() { BillOfLading = bol, Posted = true, Status = "A" });
                    }

                    foreach (var appt in appointments)
                    {
                        appt.IsReSchedule = true;

                        var result = await aLogic.Update(appt);

                        if (!string.IsNullOrEmpty(result))
                        {
                            sb.AppendLine($"Picket Ticket {appt.PickTicket}, Bill Of Lading {bol}, could not be re-scheduled. Error: {result}");
                        }
                    }
                }
            }

            return Json(new { result = sb.ToString() }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<ActionResult> MarkBolAsSent(string bol, string appointmentnumber)
        {
            StringBuilder sb = new StringBuilder();
            using (var oLogic = Kernel.Get<IOrderAppointmentLogic>())
            using (var aLogic = Kernel.Get<IAppointmentLogic>())
            {

                if (string.IsNullOrEmpty(bol))
                {
                    sb.AppendLine("Missing Bill Of Lading Number.");
                }
                else
                {
                    IList<Model.Appointment> appointments = new List<Model.Appointment>();

                    // check if it is a master bol
                    var orders = await oLogic.ToListAsync(new OrderAppointmentQuery() { MasterBillOfLading = bol });

                    if (orders.Any())
                    {
                        appointments = await aLogic.ToListAsync(new AppointmentQuery() { MasterBillOfLading = bol, Posted = true});

                    }
                    else
                    {
                        appointments = await aLogic.ToListAsync(new AppointmentQuery() { BillOfLading = bol, Posted = true });
                    }

                    foreach (var appt in appointments)
                    {
                        appt.Status = "S";

                        var result = await aLogic.Update(appt);

                        if (!string.IsNullOrEmpty(result))
                        {
                            sb.AppendLine($"Picket Ticket {appt.PickTicket}, Bill Of Lading {bol}, could not be re-scheduled. Error: {result}");
                        }
                    }
                }
            }

            return Json(new { result = sb.ToString() }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GenerateLogReport(LogReportIndex_ViewModel model)
        {
            //var model = new LogReportIndex_ViewModel()
            //{
            //    DeliveryTypeId = deliveryTypeId,
            //    SelectedClientId = customerId,
            //    SelectedDivisionId = divisionId,
            //    SelectedDay = selectedDay
                
            //};
            // TODO: is possible get the appointments from client?
            
            var appointments =  this.GetAppointments(model);

            var reportingService = new Reporting.ReportingService();

            string mimeType = null;
            var result = reportingService.RenderReport(appointments, "LogReport.rdlc", "DataSet1", model.ReportFormat, out mimeType);

            var fName = string.Format("LogReport_{0}", DateTime.Now.ToShortDateString().Replace("/", "").Replace(":", ""));
            Session[fName] = result;

            return Json(new { success = true, fName, mimeType, model.ReportFormat }, JsonRequestBehavior.AllowGet);


        }

        [HttpGet]
        public ActionResult DownloadReport(string reportName, string format, string fileExtension)
        {
            
            var reportingService = new Reporting.ReportingService();

            var reportBytes = Session[reportName];
            if (reportBytes == null)
            {
                return new EmptyResult();
            }

            Session[reportName] = null;

            FileResult fileResult = new FileContentResult(reportBytes as byte[], format);
            fileResult.FileDownloadName = reportingService.GetReportName(reportName.Replace(".rdlc", string.Empty), fileExtension);
            return fileResult;
        }


       

        public PartialViewResult GetLogReport(LogReportIndex_ViewModel model)
        {
            List<Models.Appointment> appointments = new List<Models.Appointment>();

            appointments = this.GetAppointments(model);

            return PartialView("_LogReport_AppointmentsPartial", appointments);
        }

        [HttpGet]
        public async Task<ActionResult> LogReport()
        {
            var model = new LogReportIndex_ViewModel();
            model.SelectedDay = DateTime.Today;

            var userContext = Session["UserContext"] as GSLogisticsUserContext;
            if (userContext !=null)
            {
                model.AvailableClientIds = userContext.CustomerIds.ToArray();
            }
            return await this.LogReport(model);
        }

        [HttpPost]
        public async Task<ActionResult> LogReport(LogReportIndex_ViewModel model)
        {

            using (var logic = Kernel.Get<ICustomerLogic>())
            {



                Dictionary<string, string> result = new Dictionary<string, string>();
                List<Model.Customer> cust = new List<Model.Customer>();
                if (model.AvailableClientIds !=null && model.AvailableClientIds.Any())
                {
                    cust = await logic.ToListAsync(new CustomerQuery() { CustomerIds = model.AvailableClientIds });
                }
                else
                {
                    cust = await logic.ToListAsync();
                }
                var clients = cust.OrderBy(x => x.CompanyName).Select(x => new { Id = x.CustomerId, Name = x.CompanyName }).ToList();
                //var clients = repository.Customers.OrderBy(x => x.CompanyName).Select(x => new { Id = x.CustomerId, Name = x.CompanyName }).ToList();

                clients.ForEach(x => result.Add(x.Id, x.Name));

                ViewBag.Customers = new SelectList(result, "Key", "Value", null);

                Dictionary<int, string> result2 = new Dictionary<int, string>();
                result2.Add(7777, "Select a Customer");

                //var divisions = repository.Divisions.Select(d => new { Id = d.DivisionId, Name = d.Description }).ToList();

                //divisions.ForEach(x => );
                ViewBag.Divisions = new SelectList(result2, "Key", "Value", null);

                return this.View("LogReport", model);
            }
        }



        #endregion  

        [HttpGet]
        public async Task<ActionResult> GetDivisionByClient(string customerId, bool addPlaceholder = true)
        {
            if (String.IsNullOrEmpty(customerId))
            {
                return new EmptyResult();
            }
            using (var divLogic = Kernel.Get<IDivisionLogic>())
            {
                var divisions2 = await divLogic.GetDivisionByCustomerId(customerId);

                var userContext = Session["UserContext"] as GSLogisticsUserContext;
                if (userContext != null)
                {
                    if (userContext != null && userContext.DivisionIds.Any())
                    {
                        var availableDivisionsIds = userContext.DivisionIds.ToList();
                        divisions2 = divisions2.Where(x => availableDivisionsIds.Contains(x.DivisionId)).ToList();
                    }
                    
                } 

                var divs = divisions2.OrderBy(x => x.DivisionId).ThenBy(x => x.Name).Select(d => new { Id = d.DivisionId, Name = $"{d.Name} {d.Description}" }).ToList();
                if (addPlaceholder)
                {
                    divs.Insert(0, new { Id = 0, Name = "Select" });
                }

                return Json(divs.OrderBy(x => x.Id).ThenBy(x => x.Name), JsonRequestBehavior.AllowGet);
            }
            
        }


        [HttpGet]
        public async Task<ActionResult> GetDivisionByClientIds(string[] customerIds)
        {
            if (customerIds != null  && customerIds.Any())
            {
                return new EmptyResult();
            }
            using (var divLogic = Kernel.Get<IDivisionLogic>())
            {


                // var divisions = repository.GetDivisionByClient(customerId).Select(d => new { Id = d.DivisionId, Name = $"{d.NameId} {d.Description}" }).ToList();
                var divisions2 = await divLogic.GetDivisionsByCustomerIds(customerIds);// (d => new { Id = d.DivisionId, Name = $"{d.NameId} {d.Description}" }).ToList();

                var userContext = Session["UserContext"] as GSLogisticsUserContext;
                if (userContext != null)
                {
                    if (userContext != null && userContext.DivisionIds.Any())
                    {
                        var availableDivisionsIds = userContext.DivisionIds.ToList();
                        divisions2 = divisions2.Where(x => availableDivisionsIds.Contains(x.DivisionId)).ToList();
                    }

                }

                var divs = divisions2.Select(d => new { Id = d.DivisionId, Name = $"{d.CustomerName} - {d.Name} {d.Description}" }).ToList();
                divs.Insert(0, new { Id = 0, Name = "Select" });

                return Json(divs, JsonRequestBehavior.AllowGet);
            }

        }




        public PartialViewResult GetLogReportOverdue(LogReportIndex_ViewModel model)
        {
            List<Models.Appointment> appointments = new List<Models.Appointment>();
            List<RescheduleBillOfLading_ViewModel> bolList = new List<RescheduleBillOfLading_ViewModel>();

            var query = new AppointmentQuery()
            {
                Posted = true,
                //ShippingDateEnd = DateTime.Today,
                IsReschedule = true,
                
            };

       
            if (model.SelectedDay.HasValue)
            {
                query.ShippingDate = model.SelectedDay.Value;
            }
            

            if (!string.IsNullOrEmpty(model.SelectedClientId))
            {
                query.CustomerId = model.SelectedClientId;
            }
            


            if (model.SelectedDivisionId.HasValue && model.SelectedDivisionId.Value != 0)
            {
                query.DivisionId = model.SelectedDivisionId.Value;
            }
            

            using (var oLogic = Kernel.Get<IOrderAppointmentLogic>())
            using (var aLogic = oLogic.GetLogic<IAppointmentLogic>())
            {

                var orderAppts = oLogic.ToList(new OrderAppointmentQuery());
               // var ordersWithPOD = orderAppts.Where(x => string.IsNullOrEmpty(x.PathPOD) || string.IsNullOrWhiteSpace(x.PathPOD)).ToList();
                var list = aLogic.ToList(query);

                foreach (var appt in list )
                {
                    var thisAppointment = new Models.Appointment()
                    {
                        AppointmentNo = appt.AppointmentNumber,
                        CustomerName = appt.CustomerName,
                        CustomerId = appt.CustomerId,
                        DivisionId = appt.DivisionId,
                        DivisionName = appt.DivisionName,
                        DivisionNameId = appt.DivisionNameId,
                        Carrier = appt.Carrier,
                        PickTicket = appt.PickTicket,
                        PtBulk = appt.PtBulk,
                        ScaccCode = appt.ScacCode,
                        ShipDate = appt.ShippingDate.Value,
                        ShipTime = appt.ShippingTime.Value,
                        Posted = appt.Posted.ToString(),
                        DateAdded = appt.DateAdded,
                        DeliveryTypeId = appt.DeliveryTypeId

                    };
                    //&& string.IsNullOrEmpty(x.PathPOD) not sure if this apply 
                    var orderAppt = orderAppts.Where(x => x.CustomerId == thisAppointment.CustomerId && x.PickTicketId == thisAppointment.PickTicket ).FirstOrDefault();
                    if (orderAppt != null)
                    {
                        thisAppointment.PurchaseOrder = orderAppt.PurchaseOrderId;
                        thisAppointment.Pieces = orderAppt.Pieces.Value;
                        thisAppointment.BoxesNumber = orderAppt.BoxesCount.Value;
                        thisAppointment.ShipTo = orderAppt.ShipTo;
                        thisAppointment.BillOfLading = orderAppt.BillOfLading;
                        thisAppointment.pathPOD = orderAppt.PathPOD;

                        appointments.Add(thisAppointment);
                    }
                }

                var grouped = appointments.GroupBy(x => x.BillOfLading);
                foreach (var g in grouped)
                {
                    var groupelement = g.First();
                    bolList.Add(new RescheduleBillOfLading_ViewModel()
                    {
                        BillOfLading = g.Key,
                        AppointmentNumber = groupelement.AppointmentNo,
                        Carrier = groupelement.Carrier,
                        CustomerId = groupelement.CustomerId,
                        CustomerName = groupelement.CustomerName,
                        DivisionId = groupelement.DivisionId,
                        DivisionName = groupelement.DivisionName,
                        SaccCode = groupelement.ScaccCode,
                        ShippingTime = groupelement.ShipTime,
                        ShippingDate = groupelement.ShipDate,
                        ShippingTimeLimit = groupelement.ShipTimeLimit,
                        TotalBoxes = g.Sum(x => x.BoxesNumber),
                        TotalOrders = g.Count(),
                        TotalPieces = g.Sum(x => x.Pieces)
                    });
                }
            }


            return PartialView("_LogReport_Overdue", bolList);
        }

        [HttpGet]
        public async Task<ActionResult> Reschedule()
        {
            var model = new LogReportIndex_ViewModel();
            model.SelectedDay = DateTime.Today;

            return await this.Reschedule(model);
        }

        [HttpPost]
        public async Task<ActionResult> Reschedule(LogReportIndex_ViewModel model)
        {

            using (var logic = Kernel.Get<ICustomerLogic>())
            {



                Dictionary<string, string> result = new Dictionary<string, string>();
                List<Model.Customer> cust = new List<Model.Customer>();
                if (model.AvailableClientIds != null && model.AvailableClientIds.Any())
                {
                    cust = await logic.ToListAsync(new CustomerQuery() { CustomerIds = model.AvailableClientIds });
                }
                else
                {
                    cust = await logic.ToListAsync();
                }
                var clients = cust.OrderBy(x => x.CompanyName).Select(x => new { Id = x.CustomerId, Name = x.CompanyName }).ToList();
              

                clients.ForEach(x => result.Add(x.Id, x.Name));

                ViewBag.Customers = new SelectList(result, "Key", "Value", null);

                Dictionary<int, string> result2 = new Dictionary<int, string>();
                result2.Add(7777, "Select a Customer");
                
                ViewBag.Divisions = new SelectList(result2, "Key", "Value", null);

                return this.View("LogReportReSchedule", model);
            }
        }

        [HttpGet]
        public async Task<ActionResult> GetAppointmentsByBol(string bol)
        {
            StringBuilder sb = new StringBuilder();

            List<Appointment> appointments = new List<Appointment>();

            sb.AppendLine("<table cellpadding=\"10\" cellspacing=\"0\" border=\"0\" style=\"padding-left:50px; \">'");
            sb.AppendLine("<thead>");
            sb.AppendLine("<tr>");
            sb.AppendLine("<th>PO#</th>");
            sb.AppendLine("<th>PT#</th>");
            sb.AppendLine("<th style=\"text-align: right \">Pieces</th>");
            sb.AppendLine("<th>Boxes</th>");
            sb.AppendLine("</tr>");
            sb.AppendLine("</thead>");

            using (var oLogic = Kernel.Get<IOrderAppointmentLogic>())
            using (var logic = Kernel.Get<IAppointmentLogic>())
            {
                var appts = await logic.ToListAsync(new AppointmentQuery()
                {
                    BillOfLading = bol,
                    IsReschedule = true
                });
                var orderAppts = oLogic.ToList(new OrderAppointmentQuery());

                
                foreach (var appt in appts)
                {

                    var thisAppointment = this.GetAppointmentWebModel(appt, orderAppts);


                    appointments.Add(thisAppointment);
                }


                foreach (var a in appointments)
                {
                    sb.AppendLine("<tr>");
                    sb.AppendLine($"<td>{a.PurchaseOrder}</td><td>{a.PickTicket}</td><td style=\"text-align: right \">{a.Pieces.ToString("N")}</td><td style=\"text-align: right \">{a.BoxesNumber.ToString("N")}</td>");
                    sb.AppendLine("</tr>");
                }
            }
            var totalBoxes = appointments.Sum(x => x.BoxesNumber);
            var totalPieces = appointments.Sum(x => x.Pieces);
            sb.AppendLine("<tfoot>");
            sb.AppendLine("<td colspan=\"2\"></td>");
            sb.AppendLine($"<td>{totalPieces}</td>");
            sb.AppendLine($"<td>{totalBoxes.ToString("F")}</td>");
            sb.AppendLine("</tfoot>");
            sb.AppendLine("</table>");

            return Json(sb.ToString(), JsonRequestBehavior.AllowGet);
        }


        private Appointment GetAppointmentWebModel(Model.Appointment appt, IList<Model.OrderAppointment> orders)
        {
            var thisAppointment = new Models.Appointment()
            {
                AppointmentNo = appt.AppointmentNumber,
                CustomerName = appt.CustomerName,
                CustomerId = appt.CustomerId,
                Carrier = appt.Carrier,
                PickTicket = appt.PickTicket,
                PtBulk = appt.PtBulk,
                ScaccCode = appt.ScacCode,
                ShipDate = appt.ShippingDate.Value,
                ShipTime = appt.ShippingTime.Value,
                Posted = appt.Posted.ToString(),
                DateAdded = appt.DateAdded,
                ShipTimeLimit = appt.ShippingTimeLimit,
                DeliveryTypeId = appt.DeliveryTypeId,
                DivisionId = appt.DivisionId,
                DivisionName = appt.DivisionName,
                DivisionNameId = appt.DivisionNameId,
                ReScheduleDate = appt.ReScheduleDate,
                Pallets = appt.Pallets,
                TruckId = appt.TruckId,
                DriverId = appt.DriverId

            };

            var orderAppt = orders.Where(x => x.CustomerId == thisAppointment.CustomerId && x.PickTicketId == thisAppointment.PickTicket).FirstOrDefault();
            if (orderAppt != null)
            {
                thisAppointment.PurchaseOrder = orderAppt.PurchaseOrderId;
                thisAppointment.Pieces = orderAppt.Pieces.Value;
                thisAppointment.BoxesNumber = orderAppt.BoxesCount.Value;
                thisAppointment.ShipTo = orderAppt.ShipTo;
                thisAppointment.BillOfLading = orderAppt.BillOfLading;
            }

            return thisAppointment;
        }
        /// <summary>
        /// Save the BOL as reschedule once is selected from reschedule view
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> RescheduleBillOfLading(NewReschedule_ViewModel model)
        {
            using (var oLogic = Kernel.Get<IOrderAppointmentLogic>())
            using (var logic = Kernel.Get<IAppointmentLogic>())
            {
                var appts = await logic.ToListAsync(new AppointmentQuery()
                {
                    BillOfLading = model.BillOfLading,
                    AppointmentNumber = model.AppointmentNumber,
                    IsReschedule = true
                });

                var orders = await oLogic.ToListAsync(new OrderAppointmentQuery()
                {
                    BillOfLading = model.BillOfLading
                });

                string result = string.Empty;
                foreach(var appt in appts)
                {
                    DateTime actualShippingDate = model.ReScheduleDate.HasValue ? model.ReScheduleDate.Value : model.ShippingDate;
                    appt.IsReSchedule = false;
                    appt.ReScheduleDate = model.ReScheduleDate;
                    appt.ShippingTime = new DateTime(actualShippingDate.Year, actualShippingDate.Month, actualShippingDate.Day, model.ShippingTime.Hour, model.ShippingTime.Minute, 0);
                    appt.ShippingTime = model.ShippingTime;
                    if (model.ShippingTimeLimit.HasValue)
                    {
                        appt.ShippingTimeLimit = new DateTime(actualShippingDate.Year, actualShippingDate.Month, actualShippingDate.Day, model.ShippingTimeLimit.Value.Hour, model.ShippingTimeLimit.Value.Minute, 0);
                    }
                    

                    var order = orders.FirstOrDefault(x => x.CustomerId == appt.CustomerId && x.PickTicketId == appt.PickTicket);

                    if(order.ShipFor != model.ShippingDate)
                    {
                        order.ShippingDateChanged = true;
                        order.ShipFor = model.ShippingDate;
                    }

                    await oLogic.Update(order);
                    result += await logic.Update(appt);

                   


                }

                return Json(new { Success = result.Length == 0, Error = result });
            }

            
        }

        private class data
        {
            public string DT_RowId { get; set; }
            public string PickTicketId { get; set; }
            public string StoreName { get; set; }
        }


    }
}
