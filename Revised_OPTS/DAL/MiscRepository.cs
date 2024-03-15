using Microsoft.EntityFrameworkCore;
using Revised_OPTS.Model;
using Revised_OPTS.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revised_OPTS.DAL
{
    internal class MiscRepository : BaseRepository<Miscellaneous>, IMiscRepository
    {
        public List<Miscellaneous> checkExistingRecord(Miscellaneous misc)
        {
            return getDbSet()
        .Where(jo => jo.OrderOfPaymentNum == misc.OrderOfPaymentNum
            //&& jo.YearQuarter == rpt.YearQuarter
            //&& jo.Quarter == rpt.Quarter
            && jo.DeletedRecord == 0
            && jo.DuplicateRecord == 0
            && jo.MiscID != misc.MiscID)
        .ToList();
        }

        public List<Miscellaneous> retrieveBySearchKeyword(string billNum)
        {
            //return getDbSet().Where(m => m.OrderOfPaymentNum.Contains(billNum)).ToList();

            return getDbSet()
            //SELECT * FROM Jo_RPT where TaxDec LIKE @TaxDec and DeletedRecord != 1
                .Where(j => j.OrderOfPaymentNum.Contains(billNum) && j.DeletedRecord != 1)
            //UNION
                .Union(
            //SELECT *FROM Jo_RPT where RefNum
                getDbSet()
                    .Where(j => getDbSet()
                // (select RefNum FROM Jo_RPT where TaxDec LIKE @TaxDec)
                    .Where(subJ => subJ.OrderOfPaymentNum.Contains(billNum))
                    .Select(subJ => subJ.RefNum)
                //RefNum in (select RefNum
                    .Contains(j.RefNum)
                //and DeletedRecord != 1  AND RefNum IS NOT NULL AND RefNum != ''
                    && j.DeletedRecord != 1 && j.RefNum != null && j.RefNum != ""))

            //order by RefNum desc, EncodedDate asc
                .OrderByDescending(j => j.RefNum)
                .ThenBy(j => j.EncodedDate)
                .ToList();
        }

        public List<Miscellaneous> retrieveBySearchKeywordForPaymentValidationOnly(string billNum)
        {
            //return getDbSet().Where(m => m.OrderOfPaymentNum.Contains(billNum)).ToList();

            return getDbSet()
            //SELECT * FROM Jo_RPT where TaxDec LIKE @TaxDec and DeletedRecord != 1
                .Where(j => j.OrderOfPaymentNum.Contains(billNum) && j.DeletedRecord != 1 && j.Status == TaxStatus.ForPaymentValidation)
            //UNION
                .Union(
            //SELECT *FROM Jo_RPT where RefNum
                getDbSet()
                    .Where(j => getDbSet()
                    // (select RefNum FROM Jo_RPT where TaxDec LIKE @TaxDec)
                    .Where(subJ => subJ.OrderOfPaymentNum.Contains(billNum))
                    .Select(subJ => subJ.RefNum)
                    //RefNum in (select RefNum
                    .Contains(j.RefNum)
                    //and DeletedRecord != 1  AND RefNum IS NOT NULL AND RefNum != ''
                    && j.DeletedRecord != 1 && j.RefNum != null && j.RefNum != "" && j.Status == TaxStatus.ForPaymentValidation))

            //order by RefNum desc, EncodedDate asc
                .OrderByDescending(j => j.RefNum)
                .ThenBy(j => j.EncodedDate)
                .ToList();
        }

        public List<Miscellaneous> retrieveBySameRefNum(string refNum)
        {
            return getDbSet().Where(j => j.RefNum == refNum && j.DeletedRecord != 1)
            .OrderBy(j => j.MiscID).ToList();
        }

        public List<Miscellaneous> RetrieveNoName()
        {
            throw new NotImplementedException();
        }
    }
}
