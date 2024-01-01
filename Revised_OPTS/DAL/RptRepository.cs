using Inventory_System.Model;
using Microsoft.EntityFrameworkCore;
using Revised_OPTS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revised_OPTS.DAL
{
    internal class RptRepository : BaseRepository<Rpt>, IRptRepository
    {
        public RptRepository(DbContext dBContext) : base(dBContext)
        {
        }

        public List<Rpt> checkExistingRecord(Rpt rpt)
        {
            return dbSet
                    .Where(jo => jo.TaxDec == rpt.TaxDec
                        && jo.YearQuarter == rpt.YearQuarter
                        && jo.Quarter == rpt.Quarter
                        && jo.DeletedRecord != 1
                        && jo.DuplicateRecord == 0
                        && jo.BillingSelection == rpt.BillingSelection
                        && jo.RptID != rpt.RptID)
                    .ToList();
        }

        public List<Rpt> retrieveBySameRefNumAndReqParty(string refNum, string reqParty)
        {
            return dbSet.Where(j => j.RefNum == refNum && j.DeletedRecord != 1 && j.RequestingParty == reqParty)
            .OrderByDescending(j => j.RefNum).ThenBy(j => j.TaxDec).ToList();
        }

        public List<Rpt> retrieveBySearchKeyword(string tdn)
        {
            return dbSet.Where(t => t.TaxDec.Contains(tdn)).OrderByDescending(t => t.EncodedDate)
                .Union(dbSet.Where(j => j.TaxDec.Contains(tdn) && j.DeletedRecord != 1))
                .Union(dbSet.Where(j => dbSet.Where(subJ => subJ.TaxDec.Contains(tdn)).Select(subJ => subJ.RefNum)
                .Contains(j.RefNum) && j.DeletedRecord != 1)).OrderByDescending(j => j.RefNum)
                .ThenBy(j => j.EncodedDate).ToList();
        }
    }
}
