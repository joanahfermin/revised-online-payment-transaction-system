using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Inventory_System.Utilities
{
    internal class TaxUniqueKeyFormat
    {
        public bool isRPTTaxDecFormat(string taxDec)
        {
            //format of taxdec number.
            Regex re = new Regex("^[D|E|F|G]-[0-9]{3}-[0-9]{5}( / [D|E|F|G]-[0-9]{3}-[0-9]{5})*$");
            return re.IsMatch(taxDec.Trim());
        }
    }
}
