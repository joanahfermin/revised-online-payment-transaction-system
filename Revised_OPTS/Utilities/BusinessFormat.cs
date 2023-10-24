using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory_System.Utilities
{
    internal class BusinessFormat
    {
        public const string TAXDEC_FORMAT = "^[A|B|C|D|E|F|G]-[0-9]{3}-[0-9]{5}( / [A|B|C|D|E|F|G]-[0-9]{3}-[0-9]{5})*$";
        public const string MP_FORMAT = "^[0-9]{2}-[0-9]{6}$";
        public const string BUSINESS_BILLNUM_FORMAT = "^[B]-[0-9]{4}-[0-9]{2}-[0-9]{2}-[N][X]-[0-9]{6}$";

        public const string OCCUPERMIT_FORMAT = "^[M]-[0-9]{4}-[0-9]{2}-[0-9]{2}-[B][P][L][O]-[A-Z,0-9]{4}-[0-9]{6}$";
        public const string OVR_TTMD_FORMAT = "^[M]-[0-9]{4}-[0-9]{2}-[0-9]{2}-[T][T][M][D]-[A-Z,0-9]{4}-[0-9]{6}$";
        public const string OVR_DPOS_FORMAT = "^[M]-[0-9]{4}-[0-9]{2}-[0-9]{2}-[D][P][O][S]-[A-Z,0-9]{4}-[0-9]{6}$";
        public const string MARKET_FORMAT = "^[M]-[0-9]{4}-[0-9]{2}-[0-9]{2}-[M][D][A][D]-[A-Z,0-9]{4}-[0-9]{6}$";
        public const string ZONING_FORMAT = "^[M]-[0-9]{4}-[0-9]{2}-[0-9]{2}-[C][P][D][O]-[A-Z,0-9]{4}-[0-9]{6}$";

        public const string LIQUOR_FORMAT = "^[M]-[0-9]{4}-[0-9]{2}-[0-9]{2}-[L][L][R][B]-[A-Z,0-9]{4}-[0-9]{6}$";

    }
}
