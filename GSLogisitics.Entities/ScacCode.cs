using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace GSLogistics.Entities
{
    [Table("CatScacCode", Schema = "dbo")]
    public class ScacCode
    {
        [Column("ScacCode", Order = 1), Key]
        public string ScacCodeId  { get; set; }

        [Column("Carrier", Order = 2)]
        public string ScacCodeName { get; set; }


    }
}
