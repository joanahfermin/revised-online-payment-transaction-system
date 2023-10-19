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
        public const string MISCELLANEOUS = "MISCELLANEOUS";
        public const string REALPROPERTYTAX = "REAL PROPERTY TAX";

        public static string[] ALL_TAX_TYPE = { REALPROPERTYTAX, BUSINESS, MISCELLANEOUS  };
    }
}
