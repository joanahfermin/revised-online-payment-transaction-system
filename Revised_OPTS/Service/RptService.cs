using Inventory_System.DAL;
using Inventory_System.Exception;
using Inventory_System.Model;
using Revised_OPTS.DAL;
using Revised_OPTS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Revised_OPTS.Service
{
    internal class RptService : IRptService
    {
        IRptRepository rptRepository = RepositoryFactory.Instance.GetRptRepository();
        IBankRepository bankRepository = RepositoryFactory.Instance.GetBankRepository();

        public Rpt Get(object id)
        {
            return rptRepository.Get(id);
        }

        public List<Rpt> GetAll()
        {
            return rptRepository.GetAll();
        }

        public List<Bank> GetAllBanks()
        {
            return bankRepository.GetBanks();
        }

        public List<Rpt> RetrieveBySearchKeyword(string tdn)
        {
            return rptRepository.retrieveBySearchKeyword(tdn);
        }

        public void Insert(Rpt rpt)
        {
            calculateRpt(rpt);
            rptRepository.Insert(rpt);
        }

        public void Update(Rpt rpt)
        {
            calculateRpt(rpt);
            rptRepository.Update(rpt);
        }

        private void calculateRpt(Rpt rpt)
        {
            rpt.ExcessShortAmount = rpt.AmountTransferred - rpt.AmountToPay;
        }
    }
}
