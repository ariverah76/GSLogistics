using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSLogistics.Website.Admin.Models
{
    public class OrderAppointmentsIndex_ViewModel
    {
        [Display(Name = "Start Date")]
        [DataType(DataType.Date)]
        [UIHint("Date")]
        public DateTime? CancelDateStartDate { get; set; }
        [Display(Name = "End Date")]
        [DataType(DataType.Date)]
        [UIHint("Date")]
        public DateTime? CancelDateEndDate { get; set; }

        [Display(Name = "Customer")]
        public string SelectedClientId { get; set; }


        private List<OrderAppointment> _orderAppointments;
        public List<OrderAppointment> OrderAppointments
        {
            get
            {
                if (_orderAppointments == null)
                {
                    _orderAppointments = new List<OrderAppointment>();
                }
                return _orderAppointments;
            }
            set
            {
                _orderAppointments = value;
            }
            
        }


        #region appointments

        [Display(Name = "Shipping Date")]
        [Required]
        public DateTime? ShippingDate { get; set; }

        [DataType(DataType.Time)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:H:mm}")]
        [Display(Name = "Shipping Time")]
        [Required]
        public DateTime? ShippingTime { get; set; }

        [Display(Name = "Appointment Number")]
        [Required]
        public string AppointmentNumber { get; set; }

        [Display(Name = "Shipping via")]
        [Required]
        public string ShippingCompanyId { get; set; }
        public int ShippingCompanyName { get; set; }

        #endregion
    }
}
    