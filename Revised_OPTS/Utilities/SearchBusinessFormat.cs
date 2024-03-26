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
        public static bool isEmailAdd(string eAdd)
        {
            Regex re = new Regex(BusinessFormat.EMAIL_FORMAT);
            return re.IsMatch(eAdd.Trim());
        }

        public static bool isTDN(string rpt)
        {
            Regex re = new Regex(BusinessFormat.TAXDEC_FORMAT);
            return re.IsMatch(rpt.Trim());
        }

        public static bool isBusiness(string bus)
        {
            Regex re = new Regex(BusinessFormat.BUSINESS_BILLNUM_FORMAT);
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

        public static string GetTaxTypeFromFormat(string taxType)
        {
            if (isTDN(taxType))
            {
                return TaxTypeUtil.REALPROPERTYTAX;
            }
            else if (isEmailAdd(taxType))
            {
                return TaxTypeUtil.REALPROPERTYTAX;
            }
            else if (isBusiness(taxType))
            {
                return TaxTypeUtil.BUSINESS;
            }
            else if (isMiscOccuPermit(taxType))
            {
                return TaxTypeUtil.MISCELLANEOUS_OCCUPERMIT;
            }
            else if (isMiscOvrDpos(taxType))
            {
                return TaxTypeUtil.MISCELLANEOUS_OVR;
            }
            else if (isMiscOvrTtmd(taxType))
            {
                return TaxTypeUtil.MISCELLANEOUS_OVR;
            }
            else if (isMiscMarket(taxType))
            {
                return TaxTypeUtil.MISCELLANEOUS_MARKET;
            }
            else if (isMiscZoning(taxType))
            {
                return TaxTypeUtil.MISCELLANEOUS_ZONING;
            }
            else if (isMiscLiquor(taxType))
            {
                return TaxTypeUtil.MISCELLANEOUS_LIQUOR;
            }
            return null;
        }
    }
}
