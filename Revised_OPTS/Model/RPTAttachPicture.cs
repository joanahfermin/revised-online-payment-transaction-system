using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Inventory_System.Model
{
    [Table("Jo_RPT_Pictures")]
    internal class RPTAttachPicture
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long PictureId { get; set; }
        public long RptId { get; set; }
        public string DocumentType { get; set; }
        public string FileName { get; set; }
        public byte[] FileData { get; set; }
    }
}
