using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory_System.Model
{
    //All common fields in RPT, BUSINES, MISC
    public class BasePrimaryEntity
    {
        public int? DeletedRecord { get; set; } = 0;
        public string RefNum { get; set; }
    }
}
