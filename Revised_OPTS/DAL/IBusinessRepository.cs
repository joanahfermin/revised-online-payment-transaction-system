using Revised_OPTS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revised_OPTS.DAL
{
    internal interface IBusinessRepository : IRepository<Business>
    {
        List<Business> retrieveBySearchKeyword(string billNumber);
        List<Business> checkExistingRecord(Business bus);
    }
}
