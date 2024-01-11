using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory_System.Model
{
    [Table("TAXBILLSSTAGE")]

    internal class RptTaxbillTPN
    {
        [Key]
        public string PSTDN { get; set; }
        public string ONAME { get; set; }

        public DateTime? BILLDATE { get; set; }

    }
}
