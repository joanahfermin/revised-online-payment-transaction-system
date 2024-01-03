using Inventory_System.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revised_OPTS.Model
{
    [Table("Jo_RPT")]
    internal class Rpt: BasePrimaryEntity, ICloneable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long RptID { get; set; }
        public string TaxDec { get; set; }
        public string? TaxPayerName { get; set; }
        public decimal? AmountToPay { get; set; }
        public decimal? AmountTransferred { get; set; }
        public decimal? TotalAmountTransferred { get; set; }
        public decimal? ExcessShortAmount { get; set; }
        public string? Bank { get; set; }
        public string? YearQuarter { get; set; }
        public string? Quarter { get; set; }
        public string? PaymentType { get; set; }
        public string? BillingSelection { get; set; }
        public string Status { get; set; }
        public string? RequestingParty { get; set; }
        public string? EncodedBy { get; set; }
        public DateTime? EncodedDate { get; set; }
        public string? RefNum { get; set; }
        public string? RPTremarks { get; set; }

        //public string SentBy { get; set; }
        //public DateTime? SentDate { get; set; }
        //public string BilledBy { get; set; }
        //public string BillCount { get; set; }
        //public DateTime? BilledDate { get; set; }

        public string? VerifiedBy { get; set; }
        public DateTime? PaymentDate { get; set; }
        public DateTime? VerifiedDate { get; set; }
        public string? ValidatedBy { get; set; }
        public DateTime? ValidatedDate { get; set; }
        public string? UploadedBy { get; set; }
        public DateTime? UploadedDate { get; set; }
        public string? ReleasedBy { get; set; }
        public DateTime? ReleasedDate { get; set; }

        //public string VerRemarks { get; set; }
        //public string ValRemarks { get; set; }
        //public string UploaderRemarks { get; set; }
        //public string ReleasedRemarks { get; set; }

        public string? LocCode { get; set; }
        public string? RepName { get; set; }
        public string? ContactNumber { get; set; }


        //public bool WithAuthorizationLetter { get; set; } = false;
        //public bool is_Released { get; set; } = false;

        //public int? DeletedRecord { get; set; } = 0;
        public int? DuplicateRecord { get; set; } = 0;

        //public bool SendAssessmentReady { get; set; } = false;
        //public bool SendReceiptReady { get; set; } = false;
        //public string CreatedBy { get; set; }
        //public DateTime? CreatedDate { get; set; }
        //public string LastUpdateBy { get; set; }
        //public DateTime? LastUpdateDate { get; set; }

        public DateTime? ORConfirmDate { get; set; }
        public DateTime? ORAttachedDate { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
