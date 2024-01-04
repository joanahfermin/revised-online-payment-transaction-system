﻿using Inventory_System.Model;
using Microsoft.EntityFrameworkCore;
using Revised_OPTS.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory_System.DAL
{
    internal class MiscDetailsBillingStageRepository : BaseRepository<MiscDetailsBillingStage>, IMiscDetailsBillingStageRepository
    {
        protected DbSet<MiscDetailsBillingStage> getDbSet()
        {
            return ITDDFMUDAILY2023ApplicationDBContext.Instance.Set<MiscDetailsBillingStage>();
        }

        public MiscDetailsBillingStage retrieveByBillNum(string billNum)
        {
            return getDbSet().Where(e => e.BillNumber == billNum && e.TaxpayerLName != null).FirstOrDefault();
        }

    }
}
