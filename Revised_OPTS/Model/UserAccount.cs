using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory_System.Model
{
    [Table("Jo_RPT_Users")]

    internal class UserAccount
    {
        [Key]
        public long UserID { get; set; }
        public string UserName { get; set; }
        public string DisplayName { get; set; }
        public string? FullName { get; set; }
        public string PassWord { get; set; }
        public bool isEncoder { get; set; }
        public bool isBiller { get; set; }
        public bool isVerifier { get; set; }
        public bool isValidator { get; set; }
        public bool isUploader { get; set; }
        public bool isReleaser { get; set; }
        public bool isAutomatedEmailSender { get; set; }
        public bool isConfirmEmail { get; set; }
        public bool isActive { get; set; }
        public bool canDelete { get; set; }
        public string? MachNo { get; set; }
        public string? MachNo_Misc { get; set; }

    }
}
