using Inventory_System.Model;
using Revised_OPTS.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory_System.DAL
{
    internal interface IRptTaxbillTPNRepository : IRepository<RptTaxbillTPN>
    {
        RptTaxbillTPN retrieveByTDN(string tdn);
    }
}
