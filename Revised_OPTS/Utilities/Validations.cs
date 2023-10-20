using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory_System.Utilities
{
    internal class Validations
    {
        private static bool hasExistingError(ErrorProvider ep, Control tb)
        {
            return ep.GetError(tb).Length > 0;
        }

        public static void ValidateRequired(ErrorProvider ep, Control tb, string propertyName)
        {
            if (hasExistingError(ep, tb))
            {
                return;
            }
            if (tb.Text.Trim() == "")
            {
                ep.SetError(tb, $"{propertyName} is required.");
            }
        }
    }
}
