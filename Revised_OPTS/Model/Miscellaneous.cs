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
        public string? MiscType { get; set; }
        public string? TaxpayersName { get; set; }
        public string? OrderOfPaymentNum { get; set; }
        public string? ModeOfPayment { get; set; }
        public string? OPATrackingNum { get; set; }
        public decimal? AmountToBePaid { get; set; }
        public decimal? TransferredAmount { get; set; }
        public decimal? ExcessShort { get; set; }
        public DateTime? PaymentDate { get; set; }
        public string? Status { get; set; }
        public string? RequestingParty { get; set; }
        public string? Remarks { get; set; }
        public string? VerifiedBy { get; set; }
        public DateTime? VerifiedDate { get; set; }
        public DateTime? LBPDate { get; set; }
        public string? ValidatedBy { get; set; }
        public DateTime? ValidatedDate { get; set; }
        public string? RefNum { get; set; }
        public string? EncodedBy { get; set; }
        public DateTime? EncodedDate { get; set; }
        public string? TransmittedBy { get; set; }
        public DateTime? TransmittedDate { get; set; }
        public string? ReleasedBy { get; set; }
        public string? RepName { get; set; }
        public string? ContactNumber { get; set; }
        public DateTime? ReleasedDate { get; set; }
        public string? LastUpdateBy { get; set; }
        public string? LastUpdateDate { get; set; }

        public bool DeletedRecord { get; set; } = false;

        public int DuplicateRecord { get; set; } = 0;

        //public string Profession { get; set; }
        //public string LastORDate { get; set; }
        //public string LastORNo { get; set; }
        //public string PRC_IBP_No { get; set; }


    }
}
