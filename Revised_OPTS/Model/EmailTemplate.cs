using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory_System.Model
{
    [Table("Jo_RPT_EmailTemplate")]
    public class EmailTemplate
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long TemplateID { get; set; }
        public string Name { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public int Deleted { get; set; } = 0;
        public bool isAssessment { get; set; }
        public bool isReceipt { get; set; }
    }
}
