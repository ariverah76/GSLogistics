﻿using GSLogistics.Entities;
using GSLogistics.Entities.Abstract;
using GSLogistics.Website.Admin.Models;
using GSLogistics.Website.Admin.Models.OrderAppointments;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
namespace GSLogistics.Website.Admin.Controllers
{
    [Authorize]
    public class OrderAppointmentController : Controller
    {

        //TOOD: Implement ninject kernel on the constructor
        //TODO: Implement ViewModel to pass through the view
        private IRepository repository;

        public OrderAppointmentController(IRepository orderApptRepository)
        {
            this.repository = orderApptRepository;
        }

        [HttpGet]
        public ActionResult List()
        {
            var model = new OrderAppointmentsIndex_ViewModel();
            model.CancelDateStartDate = DateTime.Today.AddDays(-30);
            model.CancelDateEndDate = DateTime.Today;


            var clients = repository.OrderAppointments.Select(x => new { Id = x.CustomerId, Name = x.CompanyName }).ToList();


            Dictionary<string, string> result = new Dictionary<string, string>();
            foreach(var c in clients)
            {
                if (!result.ContainsKey(c.Id.ToString()))
                {
                    result.Add(c.Id.ToString(), c.Name);
                }
            }
            ViewBag.Customers = new SelectList(result, "Key", "Value", null);


            var shippingCompanies = repository.ScacCodes.Select(x => new { Id = x.ScacCodeId, Name = x.ScacCodeName }).ToList();

            Dictionary<string, string> result2 = new Dictionary<string, string>();
            foreach(var sc in shippingCompanies)
            {
                if (!result2.ContainsKey(sc.Id.ToString()))
                {
                    result2.Add(sc.Id.ToString(), $"{sc.Id} - {sc.Name}");
                }
            }

            ViewBag.ScacCodes = new SelectList(result2, "Key", "Value", null);


            List<Models.OrderAppointment> orders = new List<Models.OrderAppointment>();

            var ordersforAppt = repository.OrderAppointments.AsQueryable();

            ordersforAppt = ordersforAppt.Where(x => x.Status == 0);
            if (! string.IsNullOrEmpty(model.SelectedClientId))
            {
                ordersforAppt = ordersforAppt.Where(x => x.ScheduledShippingDate >= model.CancelDateStartDate && x.ScheduledShippingDate <= model .CancelDateEndDate);
            }



            foreach(var o in ordersforAppt.ToList())
            {
                try
                {
                    orders.Add(new Models.OrderAppointment() { BoxesNumber = o.BoxesCount.Value, BoxSize = o.BoxSize, CustomerId = o.CustomerId, CustomerName = o.CompanyName, EndDate = o.EndDate.Value, EstimatedShippingDate = o.ScheduledShippingDate, PickTicketId = o.PickTicketId, Pieces = o.Pieces.Value, PurchaseOrderId = o.PurchaseOrderId, StartDate = o.StartDate.Value, Volume = o.Size.Value, Weight = o.Weigth, StoreName = o.ShipTo, DivisionName = o.DivisionName, PtBulk = o.PtBulk, Notes = o.Notes });
                }
                catch(Exception exc)
                {
                    throw exc;
                }
                
            }


            model.OrderAppointments = orders.Where(x => x.EndDate >= model.CancelDateStartDate && x.EndDate <= model.CancelDateEndDate).ToList();

            return View("List", model);
        }

        [HttpPost]
        public ActionResult List(OrderAppointmentsIndex_ViewModel model)
        {
            var clients = repository.OrderAppointments.Select(x => new { Id = x.CustomerId, Name = x.CompanyName }).ToList();


            Dictionary<string, string> result = new Dictionary<string, string>();
            foreach (var c in clients)
            {
                if (!result.ContainsKey(c.Id.ToString()))
                {
                    result.Add(c.Id.ToString(), c.Name);
                }
            }
            ViewBag.Customers = new SelectList(result, "Key", "Value", null);


            var shippingCompanies = repository.ScacCodes.Select(x => new { Id = x.ScacCodeId, Name = x.ScacCodeName }).ToList();

            Dictionary<string, string> result2 = new Dictionary<string, string>();
            foreach (var sc in shippingCompanies)
            {
                if (!result2.ContainsKey(sc.Id.ToString()))
                {
                    result2.Add(sc.Id.ToString(), sc.Name);
                }
            }

            ViewBag.ScacCodes = new SelectList(result2, "Key", "Value", null);
            
            List<Models.OrderAppointment> orders = new List<Models.OrderAppointment>();

            var ordersforAppt = repository.OrderAppointments.AsQueryable();

            ordersforAppt = ordersforAppt.Where(x => x.Status == 0);

            if (!string.IsNullOrEmpty(model.SelectedClientId))
            {
                ordersforAppt = ordersforAppt.Where(x => x.CustomerId == model.SelectedClientId);
                
                    // ordersforAppt.Where(x => x.ScheduledShippingDate >= model.CancelDateStartDate && x.ScheduledShippingDate <= model.CancelDateEndDate);
            }

            if (model.CancelDateStartDate.HasValue)
            {
                ordersforAppt = ordersforAppt.Where(x => x.ScheduledShippingDate >= model.CancelDateStartDate.Value);
            }
            if (model.CancelDateEndDate.HasValue)
            {
                ordersforAppt = ordersforAppt.Where(x => x.ScheduledShippingDate <= model.CancelDateEndDate.Value);
            }

            foreach (var o in ordersforAppt.ToList())
            {
                try
                {
                    orders.Add(new Models.OrderAppointment() { BoxesNumber = o.BoxesCount.Value, BoxSize = o.BoxSize, CustomerId = o.CustomerId, CustomerName = o.CompanyName, EndDate = o.EndDate.Value, EstimatedShippingDate = o.ScheduledShippingDate, PickTicketId = o.PickTicketId, Pieces = o.Pieces.Value, PurchaseOrderId = o.PurchaseOrderId, StartDate = o.StartDate.Value, Volume = o.Size.Value, Weight = o.Weigth, StoreName = o.ShipTo, DivisionName = o.DivisionName, PtBulk = o.PtBulk, Notes = o.Notes });
                }
                catch (Exception exc)
                {
                    throw exc;
                }

            }

            model.OrderAppointments = orders;


            return View("List", model);

        }

        [HttpPost]
        public ActionResult SetAppointment(NewAppointment_ViewModel model)
        {
            foreach (var order in model.Orders)
            {
                Appointment appointment = new Appointment();
                appointment.CustomerId = order.CustomerId;
                appointment.DateAdd = DateTime.Now;
                appointment.PickTicket = order.PickTicketId;
                appointment.PtBulk = string.IsNullOrEmpty(order.PtBulk) ? order.PickTicketId : order.PtBulk;
                appointment.ScacCode = model.ScacCode;
                appointment.ShipDate = model.ShippingDate;
                appointment.ShipTime = model.ShippingTime;
                appointment.AppointmentNumber = model.AppointmentNumber;

                repository.SaveAppointment(appointment);

                Entities.OrderAppointment oappt = new Entities.OrderAppointment() { PurchaseOrderId = order.PurchaseOrderId, PickTicketId = order.PickTicketId, PtBulk = order.PtBulk, CustomerId = order.CustomerId, Status = 1 };

                repository.UpdateOrderAppointment(oappt);
                    
            }

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

        private class data
        {
            public string DT_RowId { get; set; }
            public string PickTicketId { get; set; }
            public string StoreName { get; set; }
        }


    }
}