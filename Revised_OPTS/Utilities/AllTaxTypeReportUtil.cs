using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory_System.Utilities
{
    internal class AllTaxTypeReportUtil
    {
        public const string REGENERATE_EPAYMENTS = "ALL E-PAYMENTS";
        public const string COLLECTORS_REPORT = "ALL TAXES COLLECTED";
        public const string USER_ACTIVITY = "USER ACTIVITY";

        public static string[] ALL_REPORT = { REGENERATE_EPAYMENTS, COLLECTORS_REPORT, USER_ACTIVITY };
    }
}
