using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory_System.Model
{
    [Table("V_BusinessMasterDetail")]

    internal class BusinessMasterDetailTPN
    {
        [Key]
        public string RefNo { get; set; }
        public string BusinessName { get; set; }

    }
}
