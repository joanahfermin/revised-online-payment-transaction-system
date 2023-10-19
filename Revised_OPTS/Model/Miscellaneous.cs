using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revised_OPTS.Model
{
    [Table("Jo_MISC")]

    internal class Miscellaneous
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long MiscID { get; set; }
        public string MiscType { get; set; }
        public string TaxpayersName { get; set; }
        public string OrderOfPaymentNum { get; set; }


    }
}
