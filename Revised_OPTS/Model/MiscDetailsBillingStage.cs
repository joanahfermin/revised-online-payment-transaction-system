using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory_System.Model
{
    [Table("MiscDetailsBillingSTAGE")]

    internal class MiscDetailsBillingStage
    {
        [Key]
        public string TaxpayerLName { get; set; }
        public string BillNumber { get; set; }
    }
}
