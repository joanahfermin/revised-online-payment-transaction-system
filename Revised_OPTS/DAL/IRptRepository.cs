using Inventory_System.Model;
using Revised_OPTS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revised_OPTS.DAL
{
    internal interface IRptRepository : IRepository<Rpt>
    {
        List<Rpt> retrieveBySearchKeyword(string tdn);
        List<Rpt> retrieveBySameRefNumAndReqParty(string refNum, string reqParty);
        List<Rpt> checkExistingRecord(Rpt rpt);
    }
}
