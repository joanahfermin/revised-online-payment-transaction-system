using Revised_OPTS.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory_System.Utilities
{
    public class DynamicControlInfo
    {
        public string PropertyName;
        public string Label;
        public DynamicControlType ControlType;
        public string[] ComboboxChoices;
        public bool Enabled = true;
        public string InitialValue = null;
        public bool isRequired = false;
        public string format = string.Empty;
    }
}
