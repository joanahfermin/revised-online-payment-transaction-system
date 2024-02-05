using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revised_OPTS.Utilities
{
    internal class TaxStatus
    {

        //these 3 const string is here for reference only (old status) of the old system.
        public const string FOR_ASSESSMENT = "FOR ASSESSMENT";
        public const string ASSESSMENT_PRINTED = "ASSESSMENT PRINTED";
        public const string BILL_SENT = "BILL SENT";

        public const string ForPaymentVerification = "FOR PAYMENT VERIFICATION";
        public const string ForPaymentValidation = "FOR PAYMENT VALIDATION";
        public const string ForORUpload = "FOR O.R UPLOAD";
        public const string ForORPickup = "FOR O.R PICK UP";
        public const string Released = "RELEASED";

        public const string ForTransmittal = "FOR TRANSMITTAL";
        public const string Transmitted = "TRANSMITTED";

        public static string[] STATUS = { FOR_ASSESSMENT, ASSESSMENT_PRINTED, BILL_SENT, ForPaymentVerification, ForPaymentValidation, ForORUpload, ForORPickup, Released };
    }
}
