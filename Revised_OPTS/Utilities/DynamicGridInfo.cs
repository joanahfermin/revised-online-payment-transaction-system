using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory_System.Utilities
{
    internal class DynamicGridInfo
    {
        public string PropertyName;
        public string Label;
        public DynamicGridType GridType = DynamicGridType.Text;
        public string[] ComboboxChoices;
        public bool isReadOnly = false;
        public bool isRequired = false;
        public string format = string.Empty;
        public bool decimalValue = false;
        public bool isEnabled = true;
    }
}
