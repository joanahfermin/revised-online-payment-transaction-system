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

        public List<Rpt> checkExistingRecord(string tdn, string year, string quarter, string billingSelection)
        {
            return dbSet
                    .Where(jo => jo.TaxDec == tdn
                        && jo.YearQuarter == $"{year}"
                        && jo.Quarter == quarter
                        && jo.DeletedRecord != 1
                        && jo.DuplicateRecord == 0
                        && jo.BillingSelection == billingSelection)
                    .ToList();
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
