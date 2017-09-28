using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSLogistics.Entities
{
    [Table("UserCustomer", Schema = "dbo")]
    public class UserCustomer
    {
        [Column("UserId", Order = 1), Key]
        public byte UserId { get; set; }

        [Column("CustomerId", Order = 2), Key]
        public string CustomerId { get; set; }

        [Column("DivisionId", Order = 3), Key]
        public int DivisionId { get; set; }

        [ForeignKey("CustomerId")]
        public virtual Customer Customer { get; set; }
        [ForeignKey("DivisionId")]
        public virtual CustomerDivision Division { get; set; }
    }
}
