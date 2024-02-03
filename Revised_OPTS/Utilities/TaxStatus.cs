using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revised_OPTS.Utilities
{
    internal class TaxStatus
    {
        public const string ForPaymentVerification = "FOR PAYMENT VERIFICATION";
        public const string ForPaymentValidation = "FOR PAYMENT VALIDATION";
        public const string ForORUpload = "FOR O.R UPLOAD";
        public const string ForORPickup = "FOR O.R PICK UP";
        public const string Released = "RELEASED";

        public const string ForTransmittal = "FOR TRANSMITTAL";
        public const string Transmitted = "TRANSMITTED";

        public static string[] STATUS = { ForPaymentVerification, ForPaymentValidation, ForORUpload, ForORPickup, Released };
    }
}
