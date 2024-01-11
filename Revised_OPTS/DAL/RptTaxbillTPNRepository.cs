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
    internal class RptTaxbillTPNRepository : BaseRepository<RptTaxbillTPN>, IRptTaxbillTPNRepository
    {
        protected DbSet<RptTaxbillTPN> getDbSet()
        {
            return ITDDFMUDAILY2024ApplicationDBContext.Instance.Set<RptTaxbillTPN>();
        }
        public RptTaxbillTPN retrieveByTDN(string tpn)
        {
            //return null;
            return getDbSet().Where(e => e.PSTDN == tpn).OrderByDescending(e => e.BILLDATE).FirstOrDefault();
        }
    }
}
