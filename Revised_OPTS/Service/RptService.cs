using Inventory_System.DAL;
using Inventory_System.Exception;
using Inventory_System.Model;
using Revised_OPTS.DAL;
using Revised_OPTS.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
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

        public void SaveAll(List<Rpt> listOfRptsToSave, List<Rpt> listOfRptsToDelete)
        {
            AssignRefNum(listOfRptsToSave);
            // add calculations here
            foreach (Rpt rpt in listOfRptsToSave)
            {
                if (rpt.RptID == 0)
                {
                    rptRepository.Insert(rpt);
                }
                else
                {
                    rptRepository.Update(rpt);
                }
            }

            foreach (Rpt rpt in listOfRptsToDelete)
            {
                if (rpt.RptID > 0)
                {
                    rptRepository.Delete(rpt);
                }
            }
        }

        public void AssignRefNum(List<Rpt> listOfPersonsToSave)
        {
            // hanapin if may existing refnum na
            string RefNum = listOfPersonsToSave.Where(person => !string.IsNullOrEmpty(person.RefNum)).Select(person => person.RefNum).FirstOrDefault();

            // if wala existing, gawa tayo bago
            if (RefNum == null)
            {
                RefNum = Guid.NewGuid().ToString(); // mag generate ito ng unique na refnum
            }

            // gamitin na refnum
            foreach (Rpt rpt in listOfPersonsToSave)
            {
                rpt.RefNum = RefNum;
            }
        }

    }
}
