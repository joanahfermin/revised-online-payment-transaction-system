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
        public List<Rpt> checkExistingRecord(Rpt rpt)
        {
            return getDbSet()
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
            return getDbSet().Where(j => j.RefNum == refNum && j.DeletedRecord != 1 && j.RequestingParty == reqParty)
            .OrderBy(j => j.RptID).ThenBy(j => j.TaxDec).ToList();
        }

        public List<Rpt> retrieveBySearchKeyword(string tdn)
        {
            return getDbSet()
                .Where(j => j.TaxDec.Contains(tdn) && j.DeletedRecord != 1)
                .Union(getDbSet().Where(j => getDbSet().Where(subJ => subJ.TaxDec.Contains(tdn)).Select(subJ => subJ.RefNum)
                .Contains(j.RefNum) && j.DeletedRecord != 1)).OrderByDescending(j => j.RefNum).ThenBy(j => j.EncodedDate).ToList();
        }
        //return getDbSet()
        //.Where(t => t.TaxDec.Contains(tdn))
        //.OrderByDescending(t => t.EncodedDate)
        //.Union(getDbSet().Where(j => j.TaxDec.Contains(tdn) && j.DeletedRecord != 1))
        //.Union(getDbSet().Where(j =>
        //    (j.RefNum != null && j.RefNum != "") && tdn.Contains(j.RefNum) && j.DeletedRecord != 1))
        //.OrderByDescending(j => j.RefNum)
        //.ThenBy(j => j.EncodedDate)
        //.ToList();
    }
}
