using Inventory_System.Model;
using Microsoft.Data.SqlClient;
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

        public List<Rpt> RetrieveForORUploadRegular(DateTime date, string bank, string validatedBy)
        {
            return getDbSet().Where(rpt =>
                    rpt.ValidatedDate.HasValue &&
                    rpt.ValidatedDate.Value.Date == date.Date &&
                    rpt.Bank == bank &&
                    rpt.ValidatedBy == validatedBy
                ).OrderBy(rpt => rpt.ValidatedDate).ToList();

        }
        public List<Rpt> retrieveBySearchKeyword(string tdn)
        {

            return getDbSet()
                //SELECT * FROM Jo_RPT where TaxDec LIKE @TaxDec and DeletedRecord != 1
                .Where(j => j.TaxDec.Contains(tdn) && j.DeletedRecord != 1)
                //UNION
                .Union(
                    //SELECT *FROM Jo_RPT where RefNum
                    getDbSet()
                        .Where(j => getDbSet()
                            // (select RefNum FROM Jo_RPT where TaxDec LIKE @TaxDec)
                            .Where(subJ => subJ.TaxDec.Contains(tdn))
                            .Select(subJ => subJ.RefNum)
                            //RefNum in (select RefNum
                            .Contains(j.RefNum)
                            //and DeletedRecord != 1  AND RefNum IS NOT NULL AND RefNum != ''
                            && j.DeletedRecord != 1 && j.RefNum != null && j.RefNum != "")
                )
                //order by RefNum desc, EncodedDate asc
                .OrderByDescending(j => j.RefNum)
                .ThenBy(j => j.EncodedDate)
                .ToList();

            //var parameterValue = new SqlParameter("@TaxDec", tdn);

            //var sql = "SELECT * FROM Jo_RPT where TaxDec LIKE @TaxDec and DeletedRecord != 1 " +
            //          "UNION SELECT *FROM Jo_RPT where RefNum in (select RefNum FROM Jo_RPT where TaxDec LIKE @TaxDec) " +
            //          "and DeletedRecord != 1  AND RefNum IS NOT NULL AND RefNum != '' order by RefNum desc, EncodedDate asc";

            //return  getDbSet().FromSqlRaw(sql, parameterValue).ToList();
        }
    }
}
