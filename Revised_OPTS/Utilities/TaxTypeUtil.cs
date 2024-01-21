using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revised_OPTS.Utilities
{
    internal class TaxTypeUtil
    {
        public const string BUSINESS = "BUSINESS";
        public const string REALPROPERTYTAX = "REAL PROPERTY TAX";
        public const string MISCELLANEOUS_OCCUPERMIT = "OCCUPATIONAL PERMIT";
        public const string MISCELLANEOUS_OVR = "OVR";

        //public const string MISCELLANEOUS_OVR_TTMD = "OVR TTMD";
        //public const string MISCELLANEOUS_OVR_DPOS = "OVR DPOS";

        public const string MISCELLANEOUS_LIQUOR = "LIQUOR";
        public const string MISCELLANEOUS_MARKET = "MARKET";
        public const string MISCELLANEOUS_ZONING = "ZONING";

        //NO PAYMENT IN GCASH/PAYMAYA YET.
        public const string MISCELLANEOUS_PTR = "PTR";
        public const string MISCELLANEOUS_HEALTH_CERTIFICATE = "HEALTH CERTIFICATE";
        public const string MISCELLANEOUS_CONTRACTORS_TAX = "CONTRACTOR'S TAX";
        public const string MISCELLANEOUS_SPECIAL_PERMIT = "SPECIAL PERMIT";
        public const string MISCELLANEOUS_AMMENDMENT = "AMMENDMENT";
        public const string MISCELLANEOUS_MAYORS_PERMIT = "MAYOR'S PERMIT";

        public static string[] ALL_TAX_TYPE = { REALPROPERTYTAX, BUSINESS, MISCELLANEOUS_OCCUPERMIT, MISCELLANEOUS_OVR,
            MISCELLANEOUS_LIQUOR, MISCELLANEOUS_MARKET, MISCELLANEOUS_ZONING };
            
            //,MISCELLANEOUS_PTR, MISCELLANEOUS_HEALTH_CERTIFICATE, MISCELLANEOUS_CONTRACTORS_TAX, MISCELLANEOUS_SPECIAL_PERMIT, 
            //MISCELLANEOUS_AMMENDMENT, MISCELLANEOUS_MAYORS_PERMIT};
    }
}
