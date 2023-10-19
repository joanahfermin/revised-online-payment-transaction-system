using Revised_OPTS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revised_OPTS.DAL
{
    internal class BusinessRepository : BaseRepository<Business>, IBusinessRepository
    {
        public List<Business> retrieveBySearchKeyword(string mpNum)
        {
            return dbSet.Where(b => b.MP_Number.Contains(mpNum)).ToList();
        }
    }
}
