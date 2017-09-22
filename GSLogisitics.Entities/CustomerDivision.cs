using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GSLogistics.Entities
{

    [Table("CatDivision", Schema = "dbo")]
    public class CustomerDivision
    {
        [Column(Order = 1), Key]
        public int DivisionId { get; set; }

        [Column(Order =3)]
        public string Description { get; set; }

        [Column(Order = 4)]
        public string CustomerId { get; set; }

        [Column(Order = 5)]
        public string NameId { get; set; }

        [ForeignKey("CustomerId")]
        public virtual Customer Customer { get; set; }
    }
}
