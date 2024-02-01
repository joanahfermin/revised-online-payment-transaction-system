using Revised_OPTS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revised_OPTS.DAL
{
    internal interface IMiscRepository : IRepository<Miscellaneous>
    {
        List<Miscellaneous> retrieveBySearchKeyword(string billNum);
        List<Miscellaneous> checkExistingRecord(Miscellaneous billNum);
    }
}
