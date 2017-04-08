using GSLogistics.Entities.Abstract;
using GSLogistics.Website.Admin.Models;
using GSLogistics.Website.Admin.Models.OrderAppointments;
using GSLogistics.Website.Common.Controllers;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
namespace GSLogistics.Website.Admin.Controllers
{
    [Authorize]
    public class OrderAppointmentController : Controller // : BaseController
    {

        //TOOD: Implement ninject kernel on the constructor
        //TODO: Implement ViewModel to pass through the view
        private IRepository repository;

        public OrderAppointmentController(IRepository orderApptRepository)
        {
            this.repository = orderApptRepository;
        }

        //public OrderAppointmentController(IKernel kernel)
        //    : base(kernel)
        //{

        //}

        [HttpGet]
        public ActionResult List()
        {
            var model = new OrderAppointmentsIndex_ViewModel();
            //model.CancelDateStartDate = DateTime.Today.AddDays(-60);
            //model.CancelDateEndDate = DateTime.Today;
            //model.ShippingDateStart = DateTime.Today.AddDays(-60);
            //model.ShippingDateEnd = DateTime.Today.AddDays(15);

            return this.List(model);

        }

        [HttpPost]
        public ActionResult List(OrderAppointmentsIndex_ViewModel model)
        {
            
            var clients = repository.OrderAppointments.Select(x => new { Id = x.CustomerId, Name = x.Customer.CompanyName }).ToList();

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
                var divs = repository.GetDivisionByClient(model.SelectedClientId).Select(d => new { Id = d.DivisionId, Name = d.Description }).ToList();
                Dictionary<int, string> result3 = new Dictionary<int, string>();
                divs.ForEach(x => result3.Add(x.Id, x.Name));
                ViewBag.Divisions = new SelectList(result3, "Key", "Value", null);
            }
            else
            {
                ViewBag.Divisions = new SelectList(new Dictionary<int, string>(), "Key", "Value", null);
            }

            

            var shippingCompanies = repository.ScacCodes.Select(x => new { Id = x.ScacCodeId, Name = x.ScacCodeName }).ToList();

            Dictionary<string, string> result2 = new Dictionary<string, string>();
            foreach (var sc in shippingCompanies)
            {
                if (!result2.ContainsKey(sc.Id.ToString()))
                {
                    result2.Add(sc.Id.ToString(), $"{sc.Id.ToString()} {sc.Name}");
                }
            }

            ViewBag.ScacCodes = new SelectList(result2, "Key", "Value", null);

            var status = new Dictionary<string, string>();
            status.Add("1", "Posted");
            status.Add("2", "Pending");

            ViewBag.AppointmentStatus = new SelectList(status, "Key", "Value", null);

            List<Models.OrderAppointment> orders = new List<Models.OrderAppointment>();

            var ordersforAppt = repository.OrderAppointments.AsQueryable();

            //ordersforAppt = ordersforAppt.Where(x => x.Status == 0);

            if (!string.IsNullOrEmpty(model.SelectedClientId))
            {
                ordersforAppt = ordersforAppt.Where(x => x.CustomerId == model.SelectedClientId);
                
            }

            if (model.SelectedDivisionId.HasValue && model.SelectedDivisionId.Value != 0)
            {
                ordersforAppt = ordersforAppt.Where(x => x.DivisionId == model.SelectedDivisionId);
            }

            if (model.CancelDateStartDate.HasValue)
            {
                ordersforAppt = ordersforAppt.Where(x => x.StartDate >= model.CancelDateStartDate.Value);
            }
            if (model.CancelDateEndDate.HasValue)
            {
                ordersforAppt = ordersforAppt.Where(x => x.StartDate <= model.CancelDateEndDate.Value);
            }

            if (model.ShipFor.HasValue)
            {
                ordersforAppt = ordersforAppt.Where(x => x.ShipFor.HasValue && (x.ShipFor.Value.Year == model.ShipFor.Value.Year && x.ShipFor.Value.Month == model.ShipFor.Value.Month && x.ShipFor.Value.Day == model.ShipFor.Value.Day));
            }
            
            foreach (var o in ordersforAppt.Where(x => x.Status == 0).ToList())
            {
                try
                {
                    orders.Add(new Models.OrderAppointment() { BoxesNumber = o.BoxesCount.Value, BoxSize = o.BoxSize, CustomerId = o.CustomerId, CustomerName = o.CompanyName, EndDate = o.EndDate.Value, EstimatedShippingDate = o.ScheduledShippingDate, PickTicketId = o.PickTicketId, Pieces = o.Pieces.Value, PurchaseOrderId = o.PurchaseOrderId, StartDate = o.StartDate.Value, Volume = o.Size.Value, Weight = o.Weigth, StoreName = o.ShipTo, DivisionName = o.Division.Description, PtBulk = o.PtBulk, Notes = o.Notes , ConfirmationNumber = o.ConfirmationNumber, DivisionId = o.DivisionId, ShipFor = o.ShipFor });
                }
                catch (Exception exc)
                {
                    throw exc;
                }

            }
          
           // var orderAppts = repository.OrderAppointments.Where(x => x.ShipFor >= new DateTime(2017,04,18)).ToList();

            model.OrderAppointments = orders;

            return View("List", model);

        }

       

        [HttpPost]
        public ActionResult SetAppointment(NewAppointment_ViewModel model)
        {
            foreach (var order in model.Orders)
            {
                Entities.Appointment appointment = new Entities.Appointment();
                appointment.CustomerId = order.CustomerId;
                appointment.DateAdd = DateTime.Now;
                appointment.PickTicket = order.PickTicketId;
                appointment.DivisionId = order.DivisionId;
                
                appointment.ScacCode = model.ScacCode;
                appointment.ShipDate = model.ShippingDate;
                appointment.ShipTime = new DateTime(model.ShippingDate.Year, model.ShippingDate.Month, model.ShippingDate.Day, model.ShippingTime.Hour, model.ShippingTime.Minute,0);
                appointment.AppointmentNumber = model.AppointmentNumber;
                appointment.DeliveryTypeId = model.DeliveryTypeId;

                if (model.ShippingTimeLimit.HasValue)
                {
                    appointment.ShippingTimeLimit = new DateTime(model.ShippingDate.Year, model.ShippingDate.Month, model.ShippingDate.Day, model.ShippingTimeLimit.Value.Hour, model.ShippingTimeLimit.Value.Minute, 0);
                }

                appointment.PtBulk = string.Empty;
                if (!string.IsNullOrEmpty(order.PtBulk))
                {
                    appointment.PtBulk = order.PtBulk;
                    appointment.PickTicket = order.PtBulk;
                }
                repository.SaveAppointment(appointment);

                Entities.OrderAppointment oappt = new Entities.OrderAppointment() { PurchaseOrderId = order.PurchaseOrderId, PickTicketId = order.PickTicketId, PtBulk = order.PtBulk, CustomerId = order.CustomerId, Status = 1 };

                repository.UpdateOrderAppointment(oappt);
                    
            }

            return Json(new { url = "OrderAppointment/List" });
        }

        [HttpPost]
        public ActionResult SetConfirmationNumber(NewAppointment_ViewModel model)
        {
            foreach (var order in model.Orders)
            {
                Entities.OrderAppointment oappt = new Entities.OrderAppointment() { PurchaseOrderId = order.PurchaseOrderId, PickTicketId = order.PickTicketId, PtBulk = order.PtBulk, CustomerId = order.CustomerId, ConfirmationNumber = model.ConfirmationNumber};

                repository.UpdateOrderAppointment(oappt);

            }

            return Json(new { url = "OderAppointment/List" });
        }

        [HttpPost]
        public async Task<ActionResult> ActionAppointments(ActionAppointment model)
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

                await repository.UpdateAppointment(appointment);
                if (model.Action == AppointmentAction.Cancel)
                {
                    Entities.OrderAppointment orderAppointment = new Entities.OrderAppointment()
                    {
                        CustomerId = appt.CustomerId,
                        PickTicketId = appt.PickTicket,
                        PurchaseOrderId = appt.PurchaseOrderId,
                        Status = 0
                    };

                     repository.UpdateOrderAppointment(orderAppointment);

                }
            }
            

            return Json(new { url = "OrderAppointment/List" });
        }

        [HttpPost]
        public ActionResult SaveEditedAppointment(NewAppointment_ViewModel model)
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
                    ShippingTime = new DateTime(model.ShippingDate.Year, model.ShippingDate.Month, model.ShippingDate.Day, model.ShippingTime.Hour, model.ShippingDate.Minute, 0),
                    ScacCode = model.ScacCode,
                    DateAdded = appt.DateAdded,
                };

                if (model.ShippingTimeLimit.HasValue)
                {
                    appointment.ShippingTimeLimit = new DateTime(model.ShippingDate.Year, model.ShippingDate.Month, model.ShippingDate.Day, model.ShippingTimeLimit.Value.Hour, model.ShippingTimeLimit.Value.Minute, 0);
                }

                repository.UpdateAppointment(appointment);
            }

            return Json(new { url = "OrderAppointment/List" });
        }


        [HttpPost]
        public ActionResult UpdateOrderAppointment(UpdateOrderAppointment update)
        {

            Entities.OrderAppointment oappt = new Entities.OrderAppointment() { PurchaseOrderId = update.PurchaseOrderId, PickTicketId = update.PickTicketId, PtBulk = update.PtBulk, CustomerId = update.CustomerId, Notes = update.Notes };

            repository.UpdateOrderAppointment(oappt);

            return Json(new { url = "List" });
        }
        

        [HttpPost]
        public JsonResult UpdateNotes()
        {
            
            string[] keys = Request.Form.AllKeys;
           

            var result = keys[1].Split(new char[] { '[', ']' }, StringSplitOptions.RemoveEmptyEntries);

            Entities.OrderAppointment oappt = new Entities.OrderAppointment() { PickTicketId = result[1], ShipTo = Request.Form[keys[1]] };

            repository.UpdateOrderAppointmentNotes(result[1], Request.Form[keys[1]]);


            var resp = new data() { DT_RowId = result[1], PickTicketId = result[1], StoreName = Request.Form[keys[1]] };


            return Json(resp);
        }

        [HttpPost]
        public async Task<ActionResult> UpdateAppointment(UpdateAppointment update)
        {

            Model.Appointment appt = new Model.Appointment() { CustomerId = update.CustomerId, PickTicket = update.PickTicket, AppointmentNumber = update.AppointmentNo, Posted = update.Posted ?? false, Status = update.Status};

           await repository.UpdateAppointment(appt) ;

            return Json(new { url = "List" });
        }

        public PartialViewResult GetAppointments(OrderAppointmentsIndex_ViewModel model)
        {
            List<Models.Appointment> appointments = new List<Models.Appointment>();

            var appointmentList = repository.Appointments.AsQueryable();

            if (!string.IsNullOrEmpty(model.SelectedStatus))
            {
                if (model.SelectedStatus == "1")
                {
                    appointmentList = appointmentList.Where(x => x.Posted);

                }
                else
                {
                    appointmentList = appointmentList.Where(x => !x.Posted);
                }
            }
            else
            {
                appointmentList = appointmentList.Where(x => !x.Posted);
            }

            if (!string.IsNullOrEmpty(model.AppointmentNumberSearch))
            {
                appointmentList = appointmentList.Where(x => x.AppointmentNumber == model.AppointmentNumberSearch);
            }

            if (model.ShippingDateStart.HasValue)
            {
                appointmentList = appointmentList.Where(x => x.ShipDate >= model.ShippingDateStart.Value);
            }

            if (model.ShippingDateEnd.HasValue)
            {
                appointmentList = appointmentList.Where(x => x.ShipDate <= model.ShippingDateEnd.Value);
            }

            //if (!string.IsNullOrEmpty(model.AppointmentPOSearch))
            //{
            //    appointmentList = appointmentList.Where(x => x.o)
            //}

            appointmentList = appointmentList.Where(x => x.Status == "A");
            

            var orderAppts = repository.OrderAppointments.ToList();

            var list = appointmentList.ToList();

            foreach (var appt in appointmentList.ToList())
            {
                var thisAppointment = new Models.Appointment()
                {
                    AppointmentNo = appt.AppointmentNumber,
                    CustomerName = appt.Customer.CompanyName,
                    CustomerId = appt.CustomerId,
                    Carrier = appt.CatScacCode.ScacCodeName,
                    PickTicket = appt.PickTicket,
                    PtBulk = appt.PtBulk,
                    SaccCode = appt.ScacCode,
                    ShipDate = appt.ShipDate,
                    ShipTime = appt.ShipTime, 
                    Posted = appt.Posted.ToString(),
                    DateAdded = appt.DateAdd,
                    ShipTimeLimit = appt.ShippingTimeLimit
                    
                };

                var orderAppt = orderAppts.Where(x => x.CustomerId == thisAppointment.CustomerId && x.PickTicketId == thisAppointment.PickTicket).FirstOrDefault();
                if (orderAppt != null)
                {
                    thisAppointment.PurchaseOrder = orderAppt.PurchaseOrderId;
                    thisAppointment.Pieces = orderAppt.Pieces.Value;
                    thisAppointment.BoxesNumber = orderAppt.BoxesCount.Value;
                    thisAppointment.ShipTo = orderAppt.ShipTo;
                }

                appointments.Add(thisAppointment);
            }

            model.Appointments = appointments;

            return PartialView("_AppointmentList",appointments);

        }



        #region LogReport

        public PartialViewResult GetLogReport(LogReportIndex_ViewModel model)
        {
            List<Models.Appointment> appointments = new List<Models.Appointment>();

            var appointmentList = repository.Appointments.AsQueryable();

            appointmentList = appointmentList.Where(x => x.Posted == true);
            if (model.SelectedDay.HasValue)
            {
                appointmentList = appointmentList.Where(x => x.ShipDate.Year == model.SelectedDay.Value.Year && x.ShipDate.Month == model.SelectedDay.Value.Month && x.ShipDate.Day == model.SelectedDay.Value.Day);
            }

            if (model.DeliveryTypeId.HasValue)
            {
                appointmentList = appointmentList.Where(x => x.DeliveryTypeId == model.DeliveryTypeId.Value);
            }

            if (!string.IsNullOrEmpty(model.SelectedClientId))
            {
                appointmentList = appointmentList.Where(x => x.CustomerId == model.SelectedClientId);
            }

            if (model.SelectedDivisionId.HasValue)
            {
                appointmentList = appointmentList.Where(x => x.DivisionId == model.SelectedDivisionId.Value);
            }

            var orderAppts = repository.OrderAppointments.ToList();

            var list = appointmentList.ToList();

            foreach (var appt in appointmentList.ToList())
            {
                var thisAppointment = new Models.Appointment()
                {
                    AppointmentNo = appt.AppointmentNumber,
                    CustomerName = appt.Customer.CompanyName,
                    CustomerId = appt.CustomerId,
                    DivisionId = appt.Division.NameId,
                    DivisionName = appt.Division.Description,
                    Carrier = appt.CatScacCode.ScacCodeName,
                    PickTicket = appt.PickTicket,
                    PtBulk = appt.PtBulk,
                    SaccCode = appt.ScacCode,
                    ShipDate = appt.ShipDate,
                    ShipTime = appt.ShipTime,
                    Posted = appt.Posted.ToString(),
                    DateAdded = appt.DateAdd

                };

                var orderAppt = orderAppts.Where(x => x.CustomerId == thisAppointment.CustomerId && x.PickTicketId == thisAppointment.PickTicket).FirstOrDefault();
                if (orderAppt != null)
                {
                    thisAppointment.PurchaseOrder = orderAppt.PurchaseOrderId;
                    thisAppointment.Pieces = orderAppt.Pieces.Value;
                    thisAppointment.BoxesNumber = orderAppt.BoxesCount.Value;
                    thisAppointment.ShipTo = orderAppt.ShipTo;
                }

                appointments.Add(thisAppointment);
            }


            return PartialView("_LogReport_AppointmentsPartial", appointments);
        }

        [HttpGet]
        public ActionResult LogReport()
        {
            var model = new LogReportIndex_ViewModel();
            model.SelectedDay = DateTime.Today;
            return this.LogReport(model);
        }

        [HttpPost]
        public ActionResult LogReport(LogReportIndex_ViewModel model)
        {
            var clients = repository.Customers.OrderBy(x=> x.CompanyName).Select(x => new { Id = x.CustomerId, Name = x.CompanyName }).ToList();

            Dictionary<string, string> result = new Dictionary<string, string>();
            clients.ForEach(x => result.Add(x.Id, x.Name));

            ViewBag.Customers = new SelectList(result, "Key", "Value", null);

            Dictionary<int, string> result2 = new Dictionary<int, string>();
            result2.Add(7777, "Select a Customer");

            //var divisions = repository.Divisions.Select(d => new { Id = d.DivisionId, Name = d.Description }).ToList();

            //divisions.ForEach(x => );
            ViewBag.Divisions = new SelectList(result2, "Key", "Value", null);

            return this.View("LogReport", model);
        }



        #endregion  

        [HttpGet]
        public ActionResult GetDivisionByClient(string customerId)
        {
            if (String.IsNullOrEmpty(customerId))
            {
                //throw new ArgumentNullException("countryId");
                return new EmptyResult();
            }
            
            var divisions = repository.GetDivisionByClient(customerId).Select(d => new { Id = d.DivisionId, Name = $"{d.NameId} {d.Description}" }).ToList();
            
            divisions.Insert(0,new { Id = 0, Name = "Select" });

            
            return Json(divisions, JsonRequestBehavior.AllowGet);
        }

        private class data
        {
            public string DT_RowId { get; set; }
            public string PickTicketId { get; set; }
            public string StoreName { get; set; }
        }


    }
}
