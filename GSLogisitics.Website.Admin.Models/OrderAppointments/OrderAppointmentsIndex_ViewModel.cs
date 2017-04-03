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
        [Display(Name = "Division")]
        public int? SelectedDivisionId { get; set; }

        [Display(Name = "Status")]
        public string SelectedStatus { get; set; }

        [Display(Name = "Shipping Date Start")]
        public DateTime? ShippingDateStart { get; set; }

        [Display(Name = "Shipping Date End")]
        public DateTime? ShippingDateEnd { get; set; }

        [Display(Name ="Appointment #")]
        public string AppointmentNumberSearch { get; set; }

        [Display(Name ="PO#")]
        public string AppointmentPOSearch { get; set; }

        [Display(Name = "Confirmation Number")]
        [Required]
        public string ConfirmationNumber { get; set; }

        [Display(Name = "Ship For")]
        public DateTime? ShipFor { get; set; }

        public bool EnableShipForFilter { get; set; }



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


        private List<Appointment> _appointments;
        public List<Appointment> Appointments
        {
            get
            {
                if (_appointments == null)
                {
                    _appointments = new List<Appointment>();
                }
                return _appointments;
            }
            set
            {
                _appointments = value;
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

        [DataType(DataType.Time)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:H:mm}")]
        [Display(Name = "Shipping Time Limit")]
        public DateTime? ShippingTimeLimit { get; set; }

        [Display(Name = "Delivery Type")]
        
        [Required( ErrorMessage = "Please specify type of Delivery")]
        public int? DeliveryTypeId { get; set; }

        #endregion
    }
}
    