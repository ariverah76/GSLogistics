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
        [Column("UserId", Order = 1)]
        public int UserId { get; set; }

        [Column("CustomerId", Order = 2)]
        public string CustomerId { get; set; }

        [Column("DivisionId", Order = 3)]
        public int DivisionId { get; set; }
    }
}
