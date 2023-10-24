using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory_System.Utilities
{
    public class DynamicControlInfoUtil
    {
        public static DynamicControlInfo[] Clone(DynamicControlInfo[] infoArray)
        {
            return infoArray.Select( original => new DynamicControlInfo
            {
                PropertyName = original.PropertyName,
                Label = original.Label,
                ControlType = original.ControlType,
                ComboboxChoices = original.ComboboxChoices,
                Enabled = original.Enabled,
                InitialValue = original.InitialValue,
                isRequired = original.isRequired,
                format = original.format,
            }).ToArray();
        }
    }
}
