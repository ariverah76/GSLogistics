using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSLogistics.Entities
{
    [Table("CatTrucks", Schema = "dbo")]
    public class Truck 
    {
        [Column("TruckNo", Order = 1), Required]
        [Key]
        public short TruckId { get; set; }

        [MaxLength(20, ErrorMessage = "Plates field cannot exceed 20 characters")]
        public string Plates { get; set; }

        [MaxLength(500, ErrorMessage = "Maximum up to 500 characters are allowed: Description")]
        public string Description { get; set; }
    }
}
