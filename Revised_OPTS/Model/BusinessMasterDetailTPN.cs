using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory_System.Model
{
    [Table("V_BusBillingMaster")]

    internal class BusinessMasterDetailTPN
    {
        [Key]
        public string BillNo { get; set; }
        public string TaxpayerName { get; set; }

        public string MPNo { get; set; }
        public string BusinessName { get; set; }
        public DateTime? BILLDATE { get; set; }
    }
}
