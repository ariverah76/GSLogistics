using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace GSLogistics.Website.Admin.Models
{
    public class LogReportIndex_ViewModel
    {
        [Display(Name = "Date")]
        [DataType(DataType.Date)]
        [UIHint("Date")]
        public DateTime? SelectedDay { get; set; }

        [Display(Name = "Customer")]
        public string SelectedClientId { get; set; }

        [Display(Name = "Division")]
        public int? SelectedDivisionId { get; set; }

        [Display (Name = "Pick Ticket")]
        public string SelectedPickTicket { get; set; }

        public string[] AvailableClientIds { get; set; }
        public int[] AvailableDivisionIds { get; set; }


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
        [Display(Name = "Delivery Type")]
        public short? DeliveryTypeId { get; set; }

        public string ReportFormat { get; set; }

       
        [Display(Name = "Shipping Date")]
        [Required]
        public DateTime? NewShippingDate { get; set; }

        public DateTime? NewReScheduleDate { get; set; }


        [DataType(DataType.Time)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:H:mm}")]
        [Display(Name = "Shipping Time")]
        [Required]
        public DateTime? NewShippingTime { get; set; }

        [DataType(DataType.Time)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:H:mm}")]
        [Display(Name = "Shipping Time Limit")]
        public DateTime? NewShippingTimeLimit { get; set; }



    }
}
