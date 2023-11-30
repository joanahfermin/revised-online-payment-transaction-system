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
        public const string MISCELLANEOUS_LIQUOR = "LIQUOR";
        public const string MISCELLANEOUS_MARKET = "MARKET";
        public const string MISCELLANEOUS_ZONING = "ZONING";

        public static string[] ALL_TAX_TYPE = { BUSINESS, MISCELLANEOUS_OCCUPERMIT, MISCELLANEOUS_OVR, MISCELLANEOUS_LIQUOR, MISCELLANEOUS_MARKET, MISCELLANEOUS_ZONING };
    }
}
