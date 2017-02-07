using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSLogistics.Entities
{

    [Table("CatCustomers", Schema = "dbo")]
    public class Customer
    {
        [Column("CustomerID", Order = 1) , Key]
        public string CustomerId { get; set; }
        [Column(Order = 2)]
        public string CompanyName { get; set; }
    }
}
