using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSLogistics.Entities
{
    [Table("CatDrivers", Schema = "dbo")]
    public class Driver
    {
        [Column("DriverNo", Order = 1), Required]
        [Key]
        public short DriverId { get; set; }

        [MaxLength(30, ErrorMessage = "Name: field cannot exceed 30 characters")]
        public string Name { get; set; }

        [MaxLength(500, ErrorMessage = "Maximum up to 30 characters are allowed: Sur Name")]
        public string SurName { get; set; }
    }
}
