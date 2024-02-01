using Microsoft.EntityFrameworkCore;
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
        //public List<Business> retrieveBySearchKeyword(string mpNum)
        //{
        //    return getDbSet().Where(b => b.MP_Number.Contains(mpNum)).ToList();
        //}

        public List<Business> retrieveBySearchKeyword(string billNumber)
        {
            return getDbSet()
                //SELECT * FROM Jo_RPT where TaxDec LIKE @TaxDec and DeletedRecord != 1
                .Where(b => b.BillNumber.Contains(billNumber) && b.DeletedRecord != true)
                //UNION
                .Union(
                    //SELECT *FROM Jo_RPT where RefNum
                    getDbSet()
                        .Where(b => getDbSet()
                            // (select RefNum FROM Jo_RPT where TaxDec LIKE @TaxDec)
                            .Where(subb => subb.BillNumber.Contains(billNumber))
                            .Select(subb => subb.RefNum)
                            //RefNum in (select RefNum
                            .Contains(b.RefNum)
                            //and DeletedRecord != 1  AND RefNum IS NOT NULL AND RefNum != ''
                            && b.DeletedRecord != true && b.RefNum != null && b.RefNum != "")
                )
                //order by RefNum desc, EncodedDate asc
                .OrderByDescending(b => b.RefNum)
                .ThenBy(b => b.EncodedDate)
                .ToList();
        }

        public List<Business> checkExistingRecord(Business bus)
        {
            return getDbSet()
                    .Where(jo => jo.BillNumber == bus.BillNumber
                        //&& jo.YearQuarter == rpt.YearQuarter
                        //&& jo.Quarter == rpt.Quarter
                        && jo.DeletedRecord == false
                        && jo.DuplicateRecord == true
                        && jo.BusinessID != bus.BusinessID)
                    .ToList();
        }
    }
}
