using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revised_OPTS.Model
{
    [Table("Jo_RPT_Banks")]

    internal class Bank
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long BankID { get; set; }
        public string BankName { get; set; }
        public bool isEBank { get; set; }
}
}
