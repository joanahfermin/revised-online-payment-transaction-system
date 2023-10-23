using Microsoft.EntityFrameworkCore;
using Revised_OPTS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revised_OPTS.DAL
{
    internal class MiscRepository : BaseRepository<Miscellaneous>, IMiscRepository
    {
        public MiscRepository(DbContext dBContext) : base(dBContext)
        {
        }

        public List<Miscellaneous> retrieveBySearchKeyword(string billNum)
        {
            return dbSet.Where(m => m.OrderOfPaymentNum.Contains(billNum)).ToList();
        }
    }
}
