using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSLogistics.Entities
{
    [Table("CatUsers", Schema = "dbo")]
    public class User
    {

        [Column("UserNo", Order = 1), Key]
        public int UserId { get; set; }

        [Column("DescriptionProfile", Order = 2)]
        public string Description { get; set; }

        [Column("UserName", Order = 3)]
        public string UserName { get; set; }

        [ForeignKey("UserId")]
        public virtual List<UserCustomer> UserCustomers { get; set; }

    }
}
