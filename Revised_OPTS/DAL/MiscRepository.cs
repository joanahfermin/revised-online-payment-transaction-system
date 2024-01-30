﻿using Microsoft.EntityFrameworkCore;
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
    }
}
