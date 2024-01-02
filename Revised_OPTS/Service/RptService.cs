using Inventory_System.DAL;
using Inventory_System.Exception;
using Inventory_System.Model;
using Inventory_System.Service;
using Revised_OPTS.DAL;
using Revised_OPTS.Model;
using Revised_OPTS.Utilities;
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
        /// <summary>
        /// Provides the IRptRepository instance from the RepositoryFactory.
        /// </summary>
        IRptRepository rptRepository = RepositoryFactory.Instance.GetRptRepository();

        /// <summary>
        /// Provides the IBankRepository instance from the RepositoryFactory.
        /// </summary>
        IBankRepository bankRepository = RepositoryFactory.Instance.GetBankRepository();

        /// <summary>
        /// Provides the ISecurityService instance from the ServiceFactory.
        /// </summary>
        ISecurityService securityService = ServiceFactory.Instance.GetSecurityService();

        /// <summary>
        /// Provides the IRptService instance from the ServiceFactory.
        /// </summary>
        //IRptService rptService = ServiceFactory.Instance.GetRptService();


        public Rpt Get(object id)
        {
            using (var dbContext = ApplicationDBContext.Create())
            {
                return rptRepository.Get(id);
            }
        }

        public List<Rpt> GetAll()
        {
            using (var dbContext = ApplicationDBContext.Create())
            {
                return rptRepository.GetAll();
            }
        }

        public List<Bank> GetAllBanks()
        {
            using (var dbContext = ApplicationDBContext.Create())
            {
                return bankRepository.GetBanks();
            }
        }

        public List<Rpt> RetrieveBySearchKeyword(string tdn)
        {
            using (var dbContext = ApplicationDBContext.Create())
            {
                return rptRepository.retrieveBySearchKeyword(tdn);
            }
        }

        public void Insert(Rpt rpt)
        {
            using (var dbContext = ApplicationDBContext.Create())
            {
                //calculatePayment(rpt);
                rpt.EncodedBy = securityService.getLoginUser().DisplayName;
                rpt.EncodedDate = DateTime.Now;
                rptRepository.Insert(rpt);
                dbContext.SaveChanges();
            }
        }

        public void Update(Rpt rpt)
        {
            using (var dbContext = ApplicationDBContext.Create())
            {
                //calculatePayment(rpt);
                rptRepository.Update(rpt);
                dbContext.SaveChanges();
            }
        }

        private void calculateTotalPayment(List<Rpt> listOfRptsToSave, decimal totalAmountTransferred)
        {
            //List<Rpt> sortedList = listOfRptsToSave.OrderByDescending(rpt => rpt.TaxDec).ThenBy(rpt => rpt.YearQuarter).ToList();
            bool firstRecord = true;

            decimal totalBillAmount = listOfRptsToSave.Sum(rpt => rpt.AmountToPay ?? 0);

            foreach (Rpt rpt in listOfRptsToSave)
            {
                rpt.AmountTransferred = 0;
                rpt.TotalAmountTransferred = 0;
                rpt.ExcessShortAmount = 0;

                if (firstRecord)
                {
                    rpt.TotalAmountTransferred = totalAmountTransferred;
                    rpt.ExcessShortAmount = totalAmountTransferred - totalBillAmount;
                }

                if (totalAmountTransferred >= rpt.AmountToPay)
                {
                    rpt.AmountTransferred = rpt.AmountToPay;
                    totalAmountTransferred = totalAmountTransferred - (rpt.AmountToPay ?? 0);
                }
                else
                {
                    rpt.AmountTransferred = totalAmountTransferred;
                    totalAmountTransferred = 0;

                }
                firstRecord = false;
            }
        }

        private void validateDuplicateRecord(List<Rpt> listOfRptsToSave)
        {
            var duplicates = listOfRptsToSave
            .GroupBy(obj => new
            {
                obj.TaxDec,
                obj.YearQuarter,
                obj.Quarter,
                obj.BillingSelection
            })
            .Where(group => group.Count() > 1)
            .SelectMany(group => group);

            if (duplicates.Any())
            {
                foreach (var duplicate in duplicates)
                {
                    //throw new RptException("Submitted record(s) contains duplicate(s). TDN = " + duplicate.TaxDec);
                    throw new RptException($"Submitted record(s) contains duplicate(s). TDN = {duplicate.TaxDec}");
                }
            }

            foreach (Rpt rpt in listOfRptsToSave)
            {
                List<Rpt> existingRecord = rptRepository.checkExistingRecord(rpt);

                if (existingRecord.Count > 0)
                {
                    //throw new RptException("There is an existing record/s detected in the database. Please update or delete the old record/s.");
                    throw new RptException($"There is an existing record/s detected in the database. Please update or delete the old record/s. TDN = {rpt.TaxDec}");
                }
            }
        }

        public void SaveAll(List<Rpt> listOfRptsToSave, List<Rpt> listOfRptsToDelete, decimal totalAmountTransferred)
        {
            using (var dbContext = ApplicationDBContext.Create())
            {
                validateDuplicateRecord(listOfRptsToSave);

                AssignRefNum(listOfRptsToSave);
                bool firstRecord = true;

                calculateTotalPayment(listOfRptsToSave, totalAmountTransferred);

                using (var scope = new TransactionScope())
                {
                    foreach (Rpt rpt in listOfRptsToSave)
                    {
                        if (rpt.RptID == 0)
                        {
                            rpt.Status = TaxStatus.ForPaymentVerification;
                            rpt.EncodedBy = securityService.getLoginUser().DisplayName;
                            rpt.EncodedDate = DateTime.Now;
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
                    dbContext.SaveChanges();
                    scope.Complete();
                }
            }

        }

        private void AssignRefNum(List<Rpt> listOfPersonsToSave)
        {
            // hanapin if may existing refnum na
            string RefNum = listOfPersonsToSave.Where(person => !string.IsNullOrEmpty(person.RefNum)).Select(person => person.RefNum).FirstOrDefault();

            // if wala existing, gawa tayo bago
            if (RefNum == null)
            {
                RefNum = GenerateRefNo();
            }

            // gamitin na refnum
            foreach (Rpt rpt in listOfPersonsToSave)
            {
                rpt.RefNum = RefNum;
            }
        }

        private static string GenerateRefNo()
        {
            string refNo = "R" + DateTimeOffset.Now.ToUnixTimeMilliseconds();
            return refNo;
        }

        public List<Rpt> RetrieveBySameRefNumAndReqParty(string refNum, string reqParty)
        {
            using (var dbContext = ApplicationDBContext.Create())
            {
                return rptRepository.retrieveBySameRefNumAndReqParty(refNum, reqParty);
            }
        }

        public void UpdateSelectedRecordsStatus(List<Rpt> rptList)
        {
            using (var dbContext = ApplicationDBContext.Create())
            {
                using (var scope = new TransactionScope())
                {
                    foreach (Rpt rpt in rptList)
                    {
                        rpt.Status = TaxStatus.ForPaymentValidation;
                        rpt.VerifiedBy = securityService.getLoginUser().DisplayName;
                        rpt.VerifiedDate = DateTime.Now;
                        rptRepository.Update(rpt);
                    }
                    dbContext.SaveChanges();
                    scope.Complete();
                }
            }
        }
    }
}
