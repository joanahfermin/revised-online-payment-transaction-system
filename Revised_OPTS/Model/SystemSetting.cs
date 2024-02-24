using Inventory_System.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Inventory_System.Model
{
    [Table("Jo_RPT_SystemSetting")]
    public class SystemSetting
    {
        public string SettingName { get; set; }
        public string SettingValue { get; set; }
    }
}
