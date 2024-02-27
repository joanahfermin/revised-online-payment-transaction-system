using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Inventory_System.Utilities
{
    internal class TaxUniqueKeyFormat
    {
        public bool isRPTTaxDecFormat(string taxDec)
        {
            //format of taxdec number.
            Regex re = new Regex("^[A|B|C|D|E|F|G]-[0-9]{3}-[0-9]{5}( / [D|E|F|G]-[0-9]{3}-[0-9]{5})*$");
            return re.IsMatch(taxDec.Trim());
        }

        //M-2023-02-02-BPLO-A176-000665 SAMPLE FORMAT OF O.P NUMBER OF OCCU PERMIT BPLO.
        public bool isOPnumberFormatBusiness(string bus)
        {
            //format of misc number.
            Regex re = new Regex("^[B]-[0-9]{4}-[0-9]{2}-[0-9]{2}-[N][X]-[0-9]{6}$");
            return re.IsMatch(bus.Trim());
        }

        //M-2023-02-02-BPLO-A176-000665 SAMPLE FORMAT OF O.P NUMBER OF OCCU PERMIT BPLO.
        public bool isOPnumberFormatOccuPermit(string misc)
        {
            //format of misc number.
            Regex re = new Regex("^[M]-[0-9]{4}-[0-9]{2}-[0-9]{2}-[B][P][L][O]-[A-Z,0-9]{4}-[0-9]{6}$");
            return re.IsMatch(misc.Trim());
        }

        //M-2023-03-03-TTMD-A176-000745 SAMPLE FORMAT OF O.P NUMBER OF OVR TTMD.
        public bool isOPnumberFormatOvrTTMD(string misc)
        {
            //format of misc number.
            Regex re = new Regex("^[M]-[0-9]{4}-[0-9]{2}-[0-9]{2}-[T][T][M][D]-[A-Z,0-9]{4}-[0-9]{6}$");
            return re.IsMatch(misc.Trim());
        }

        //M-2023-03-03-DPOS-A176-000794 SAMPLE FORMAT OF O.P NUMBER OF OVR DPOS.
        public bool isOPnumberFormatOvrDPOS(string misc)
        {
            //format of misc number.
            Regex re = new Regex("^[M]-[0-9]{4}-[0-9]{2}-[0-9]{2}-[D][P][O][S]-[A-Z,0-9]{4}-[0-9]{6}$");
            return re.IsMatch(misc.Trim());
        }

        //M-2023-03-03-MDAD-A176-000794 SAMPLE FORMAT OF O.P NUMBER OF MARKET MDAD.
        public bool isOPnumberFormatMarketMDAD(string misc)
        {
            //format of misc number.
            Regex re = new Regex("^[M]-[0-9]{4}-[0-9]{2}-[0-9]{2}-[M][D][A][D]-[A-Z,0-9]{4}-[0-9]{6}$");
            return re.IsMatch(misc.Trim());
        }

        //M-2023-08-16-CPDO-A176-000863 SAMPLE FORMAT OF O.P NUMBER OF ZONING CPDO.
        public bool isOPnumberFormatZoning(string misc)
        {
            //format of misc number.
            Regex re = new Regex("^[M]-[0-9]{4}-[0-9]{2}-[0-9]{2}-[C][P][D][O]-[A-Z,0-9]{4}-[0-9]{6}$");
            return re.IsMatch(misc.Trim());
        }


        //M-2023-03-03-LLRB-A176-000794 SAMPLE FORMAT OF O.P NUMBER OF LIQUOR.
        public bool isOPnumberFormatLiquorLLRB(string misc)
        {
            //format of misc number.
            Regex re = new Regex("^[M]-[0-9]{4}-[0-9]{2}-[0-9]{2}-[L][L][R][B]-[A-Z,0-9]{4}-[0-9]{6}$");
            return re.IsMatch(misc.Trim());
        }

    }
}
