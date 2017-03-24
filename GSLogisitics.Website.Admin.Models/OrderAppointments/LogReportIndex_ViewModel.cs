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

    }
}
