using Inventory_System.Model;
using Microsoft.EntityFrameworkCore;
using Revised_OPTS.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory_System.DAL
{
    internal class BusinessMasterDetailTPNRepository : BaseRepository<BusinessMasterDetailTPN>, IBusinessMasterDetailTPNRepository
    {
        public BusinessMasterDetailTPNRepository(DbContext dBContext) : base(dBContext)
        {
        }

        public BusinessMasterDetailTPN retrieveByMpNo(string mpNum)
        {
            return dbSet.Where(e => e.RefNo == mpNum).FirstOrDefault();
        }
    }
}
