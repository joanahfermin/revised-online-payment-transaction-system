using Revised_OPTS.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Inventory_System.Utilities
{
    internal class SearchBusinessFormat
    {
        public static bool isTDN(string rpt)
        {
            Regex re = new Regex(BusinessFormat.TAXDEC_FORMAT);
            return re.IsMatch(rpt.Trim());
        }

        public static bool isBusiness(string bus)
        {
            Regex re = new Regex(BusinessFormat.MP_FORMAT);
            return re.IsMatch(bus.Trim());
        }

        public static bool isMiscOccuPermit(string misc)
        {
            Regex re = new Regex(BusinessFormat.OCCUPERMIT_FORMAT);
            return re.IsMatch(misc.Trim());
        }

        public static bool isMiscOvrDpos(string misc)
        {
            Regex re = new Regex(BusinessFormat.OVR_DPOS_FORMAT);
            return re.IsMatch(misc.Trim());
        }
        public static bool isMiscOvrTtmd(string misc)
        {
            Regex re = new Regex(BusinessFormat.OVR_TTMD_FORMAT);
            return re.IsMatch(misc.Trim());
        }

        public static bool isMiscMarket(string misc)
        {
            Regex re = new Regex(BusinessFormat.MARKET_FORMAT);
            return re.IsMatch(misc.Trim());
        }

        public static bool isMiscZoning(string misc)
        {
            Regex re = new Regex(BusinessFormat.ZONING_FORMAT);
            return re.IsMatch(misc.Trim());
        }
        public static bool isMiscLiquor(string misc)
        {
            Regex re = new Regex(BusinessFormat.LIQUOR_FORMAT);
            return re.IsMatch(misc.Trim());
        }

        public static string GetTaxTypeFromTaxDecFormat(string taxDec)
        {
            if (isTDN(taxDec))
            {
                return TaxTypeUtil.REALPROPERTYTAX;
            }
            else if (isBusiness(taxDec))
            {
                return TaxTypeUtil.BUSINESS;
            }
            else if (isMiscOccuPermit(taxDec))
            {
                return TaxTypeUtil.MISCELLANEOUS_OCCUPERMIT;
            }
            else if (isMiscOvrDpos(taxDec))
            {
                return TaxTypeUtil.MISCELLANEOUS_OVR;
            }
            else if (isMiscOvrTtmd(taxDec))
            {
                return TaxTypeUtil.MISCELLANEOUS_OVR;
            }
            else if (isMiscMarket(taxDec))
            {
                return TaxTypeUtil.MISCELLANEOUS_MARKET;
            }
            else if (isMiscZoning(taxDec))
            {
                return TaxTypeUtil.MISCELLANEOUS_ZONING;
            }
            else if (isMiscLiquor(taxDec))
            {
                return TaxTypeUtil.MISCELLANEOUS_LIQUOR;
            }
            return null;
        }
    }
}
