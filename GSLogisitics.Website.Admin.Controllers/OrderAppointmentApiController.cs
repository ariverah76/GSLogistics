using GSLogistics.Entities.Abstract;
using GSLogistics.Entities.Concrete;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;


namespace GSLogistics.Website.Admin.Controllers
{
    public class OrderAppointmentApiController : ApiController
    {
        private IRepository repository;

        public OrderAppointmentApiController(IRepository orderApptRepository)
        {
            this.repository = orderApptRepository;
        }

        public OrderAppointmentApiController()
        {
            this.repository = new GSLogisticsRepository();
        }

        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("api/orderappointment/updatenotes")]
        public IHttpActionResult UpdateNotes()
        {

            var context = new HttpContextWrapper(HttpContext.Current);
            HttpRequestBase request = context.Request;

            string[] keys = request.Form.AllKeys;


            var result = keys[1].Split(new char[] { '[', ']' }, StringSplitOptions.RemoveEmptyEntries);

            Entities.OrderAppointment oappt = new Entities.OrderAppointment() { PickTicketId = result[1], ShipTo = request.Form[keys[1]] };

            repository.UpdateOrderAppointmentNotes(result[1], request.Form[keys[1]]);


            var resp = new data() { DT_RowId = result[1],  Notes = request.Form[keys[1]] };

            var serialized = JsonConvert.SerializeObject(resp, Formatting.Indented);

            return Json(resp);
        }

        internal class data
        {
            public string DT_RowId { get; set; }
          //  public string PickTicketId { get; set; }
            public string Notes { get; set; }
        }

    }
}
