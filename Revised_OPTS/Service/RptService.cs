using Inventory_System.DAL;
using Inventory_System.Exception;
using Inventory_System.Forms;
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
        IBusinessRepository businessRepository = RepositoryFactory.Instance.GetBusinessRepository();
        IMiscRepository miscRepository = RepositoryFactory.Instance.GetMiscRepository();

        /// <summary>
        /// Provides the IBankRepository instance from the RepositoryFactory.
        /// </summary>
        IBankRepository bankRepository = RepositoryFactory.Instance.GetBankRepository();

        /// <summary>
        /// Provides the ISecurityService instance from the ServiceFactory.
        /// </summary>
        ISecurityService securityService = ServiceFactory.Instance.GetSecurityService();

        IBusinessService businessService = ServiceFactory.Instance.GetBusinessService();

        /// <summary>
        /// Provides the IRptService instance from the ServiceFactory.
        /// </summary>
        //IRptService rptService = ServiceFactory.Instance.GetRptService();

        IRPTAttachPictureRepository pictureRepository = RepositoryFactory.Instance.GetRPTAttachPictureRepository();

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

        public List<Bank> GetRegularBanks()
        {
            using (var dbContext = ApplicationDBContext.Create())
            {
                return bankRepository.GetRegularBanks();
            }
        }
        public List<Bank> GetElectronicBanks()
        {
            using (var dbContext = ApplicationDBContext.Create())
            {
                return bankRepository.GetElectronicBanks();
            }
        }

        public List<Rpt> RetrieveBySearchKeyword(string tdn)
        {
            using (var dbContext = ApplicationDBContext.Create())
            {
                return rptRepository.retrieveBySearchKeyword(tdn);
            }
        }

        public List<Rpt> RetrieveForORUploadRegular(DateTime date, string validatedBy)
        {
            using (var dbContext = ApplicationDBContext.Create())
            {
                return rptRepository.RetrieveForORUploadRegular(date, validatedBy);
            }
        }

        public void Insert(Rpt rpt)
        {
            using (var dbContext = ApplicationDBContext.Create())
            {
                //calculatePaymentForSingleRecords();

                rpt.TotalAmountTransferred = rpt.AmountTransferred;

                rpt.ExcessShortAmount = rpt.TotalAmountTransferred - rpt.AmountToPay;
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
                rpt.TotalAmountTransferred = rpt.AmountTransferred;

                rpt.ExcessShortAmount = rpt.TotalAmountTransferred - rpt.AmountToPay;
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

        private void validateRptDuplicateRecord(List<Rpt> listOfRptsToSave)
        {
            var duplicates = listOfRptsToSave
            .GroupBy(obj => new
            {
                obj.TaxDec,
                obj.YearQuarter,
                obj.Quarter,
                obj.BillingSelection,
                obj.DeletedRecord,
                obj.DuplicateRecord,
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

            //retrieve existing record from the database.
            List<Rpt> allDuplicateRpts = new List<Rpt>();
            foreach (Rpt rpt in listOfRptsToSave)
            {
                List<Rpt> existingRecordList = rptRepository.checkExistingRecord(rpt);
                allDuplicateRpts.AddRange(existingRecordList);
            }
            if (allDuplicateRpts.Count > 0)
            {
                string allTaxdec = string.Join(", " , allDuplicateRpts.Select(t => t.TaxDec).ToHashSet());
                throw new DuplicateRecordException($"There is an existing record/s detected in the database. Please update or delete the old record/s. TDN = {allTaxdec}", allDuplicateRpts);
            }
        }

        private void validateMiscDuplicateRecord(List<Miscellaneous> listOfMiscToSave)
        {
            var duplicates = listOfMiscToSave
            .GroupBy(obj => new
            {
                obj.OrderOfPaymentNum,
                //obj.YearQuarter,
                //obj.Quarter,
                //obj.BillingSelection,
                obj.DeletedRecord,
                obj.DuplicateRecord,
            })
            .Where(group => group.Count() > 1)
            .SelectMany(group => group);

            if (duplicates.Any())
            {
                foreach (var duplicate in duplicates)
                {
                    //throw new RptException("Submitted record(s) contains duplicate(s). TDN = " + duplicate.TaxDec);
                    throw new RptException($"Submitted record(s) contains duplicate(s). Bill Number = {duplicate.OrderOfPaymentNum}");
                }
            }

            //retrieve existing record from the database.
            List<Miscellaneous> allDuplicateMisc = new List<Miscellaneous>();
            foreach (Miscellaneous misc in listOfMiscToSave)
            {
                List<Miscellaneous> existingRecordList = miscRepository.checkExistingRecord(misc);
                allDuplicateMisc.AddRange(existingRecordList);
            }
            if (allDuplicateMisc.Count > 0)
            {
                string allTaxdec = string.Join(", ", allDuplicateMisc.Select(t => t.OrderOfPaymentNum).ToHashSet());
                throw new DuplicateRecordException($"There is an existing record/s detected in the database. Please update or delete the old record/s. Bill Number = {allTaxdec}", allDuplicateMisc);
            }
        }

        private void validateDuplicateRecord(List<Rpt> rptList, List<Business> businessList, List<Miscellaneous> miscList)
        {
            List<Rpt> duplicateRptList = new List<Rpt>();
            try
            {
                validateRptDuplicateRecord(rptList);
            }
            catch (DuplicateRecordException ex)
            {
                duplicateRptList = ex.duplicateRptList;
            }

            List<Business> duplicateBusinessList = new List<Business>();
            try
            {
                businessService.validateBusinessDuplicateRecord(businessList);
            }
            catch (DuplicateRecordException ex)
            {
                duplicateBusinessList = ex.duplicateBusList;
            }

            List<Miscellaneous> duplicateMiscList = new List<Miscellaneous>();
            try
            {
                validateMiscDuplicateRecord(miscList);
            }
            catch (DuplicateRecordException ex)
            {
                duplicateMiscList = ex.duplicateMiscList;
            }

            if (duplicateRptList.Any() || duplicateBusinessList.Any() || duplicateMiscList.Any())
            {
                throw new DuplicateRecordException("Existing record(s) detected.", duplicateRptList, duplicateBusinessList, duplicateMiscList);
            }
        }

        //saving all rpt record in the add/update multiple record form.
        public void SaveAll(List<Rpt> listOfRptsToSave, List<Rpt> listOfRptsToDelete, decimal totalAmountTransferred)
        {
            using (var dbContext = ApplicationDBContext.Create())
            {
                validateRptDuplicateRecord(listOfRptsToSave);

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

        //save records copy/paste from gcashpaymaya excel
        public void SaveAllEPayment(List<Rpt> rptList, List<Business> businessList, List<Miscellaneous> miscList)
        {
            using (var dbContext = ApplicationDBContext.Create())
            {
                validateDuplicateRecord(rptList, businessList, miscList);

                AssignRefNum(rptList);
                AssignRefNum(businessList);
                AssignRefNum(miscList);

                bool firstRecord = true;

                //calculateTotalpaymentOfGcashAndPaymaya(rptList);

                using (var scope = new TransactionScope())
                {
                    foreach (Rpt rpt in rptList)
                    {
                        rpt.Status = TaxStatus.ForPaymentVerification;
                        rpt.EncodedBy = securityService.getLoginUser().DisplayName;
                        rpt.EncodedDate = DateTime.Now;
                        rptRepository.Insert(rpt);
                    }

                    foreach (Business bus in businessList)
                    {
                        bus.Status = TaxStatus.ForPaymentVerification;
                        bus.EncodedBy = securityService.getLoginUser().DisplayName;
                        bus.EncodedDate = DateTime.Now;
                        businessRepository.Insert(bus);
                    }

                    foreach (Miscellaneous misc in miscList)
                    {
                        misc.Status = TaxStatus.ForPaymentVerification;
                        misc.EncodedBy = securityService.getLoginUser().DisplayName;
                        misc.EncodedDate = DateTime.Now;
                        miscRepository.Insert(misc);
                    }

                    dbContext.SaveChanges();
                    scope.Complete();

                    //foreach (Rpt rpt in listOfRptsToDelete)
                    //{
                    //    if (rpt.RptID > 0)
                    //    {
                    //        rptRepository.Delete(rpt);
                    //    }
                    //}

                }
            }
        }

        //private void calculateTotalpaymentOfGcashAndPaymaya(List<Rpt> listOfRptsToSave)
        //{
        //    using (var dbContext = ApplicationDBContext.Create())
        //    {
        //        decimal? totalAmount = 0;

        //        foreach (Rpt item in listOfRptsToSave)
        //        {
        //            if (item.PaymentType.Contains("GCASH"))
        //            {
        //                totalAmount = item.AmountTransferred + item.AmountTransferred;
        //            }
        //        }
        //    }
        //}


        private void AssignRefNum(IEnumerable<BasePrimaryEntity> listOfEntityToSave)
        {
            // hanapin if may existing refnum na
            string RefNum = listOfEntityToSave.Where(person => !string.IsNullOrEmpty(person.RefNum)).Select(person => person.RefNum).FirstOrDefault();

            // if wala existing, gawa tayo bago
            if (RefNum == null)
            {
                RefNum = GenerateRefNo();
            }

            // gamitin na refnum
            foreach (BasePrimaryEntity entity in listOfEntityToSave)
            {
                entity.RefNum = RefNum;
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

        public void UpdateSelectedRecordsStatus(List<Rpt> rptList, string status)
        {
            using (var dbContext = ApplicationDBContext.Create())
            {
                using (var scope = new TransactionScope())
                {
                    foreach (Rpt rpt in rptList)
                    {
                        if (rpt.Status == TaxStatus.ForPaymentVerification)
                        {
                            rpt.Status = status;
                            rpt.VerifiedBy = securityService.getLoginUser().DisplayName;
                            rpt.VerifiedDate = DateTime.Now;
                            rptRepository.Update(rpt);
                        }
                        else if (rpt.Status == TaxStatus.ForPaymentValidation)
                        {
                            rpt.Status = status;
                            rpt.ValidatedBy = securityService.getLoginUser().DisplayName;
                            rpt.ValidatedDate = DateTime.Now;
                            rptRepository.Update(rpt);
                        }
                        else if (rpt.Status == TaxStatus.ForORUpload || rpt.Status == TaxStatus.ForORPickup)
                        {
                            rpt.Status = status;
                            rpt.ReleasedBy = securityService.getLoginUser().DisplayName;
                            rpt.ReleasedDate = DateTime.Now;
                            rptRepository.Update(rpt);
                        }
                    }
                    dbContext.SaveChanges();
                    scope.Complete();
                }
            }
        }

        public void RevertSelectedRecordStatus(List<Rpt> rptList)
        {
            using (var dbContext = ApplicationDBContext.Create())
            {
                foreach (Rpt rpt in rptList)
                {
                    if (rpt.Status == TaxStatus.ForPaymentValidation)
                    {
                        rpt.Status = TaxStatus.ForPaymentVerification;
                        rpt.VerifiedBy = null;
                        rpt.VerifiedDate = null;
                        rptRepository.Update(rpt);
                    }
                    else if (rpt.Status == TaxStatus.ForORUpload)
                    {
                        rpt.Status = TaxStatus.ForPaymentValidation;
                        rpt.ValidatedBy = null;
                        rpt.ValidatedDate = null;
                        rptRepository.Update(rpt);
                    }
                    else if (rpt.Status == TaxStatus.ForORPickup)
                    {
                        rpt.Status = TaxStatus.ForPaymentValidation;
                        rpt.UploadedBy = null;
                        rpt.UploadedDate = null;
                        rptRepository.Update(rpt);
                    }
                    else if (rpt.Status == TaxStatus.Released)
                    {
                        rpt.Status = TaxStatus.ForORPickup;
                        rpt.ReleasedBy = null;
                        rpt.ReleasedDate = null;
                        rpt.RepName = null;
                        rpt.ContactNumber = null;
                        rptRepository.Update(rpt);
                    }
                }
                dbContext.SaveChanges();
            }
        }

        public void Delete(Rpt rpt)
        {
            using (var dbContext = ApplicationDBContext.Create())
            {
                using (var scope = new TransactionScope())
                {
                    rpt.DeletedRecord = 1;
                    rptRepository.Update(rpt);
                    dbContext.SaveChanges();
                    scope.Complete();
                }
            }
        }

        public void UploadReceipt(RPTAttachPicture pix)
        {
            try
            {
                using (var dbContext = ApplicationDBContext.Create())
                {
                    using (var scope = new TransactionScope())
                    {
                        RPTAttachPicture existing = pictureRepository.getRptReceipt(pix.RptId);
                        if (existing != null && existing.PictureId > 0)
                        {
                            pictureRepository.PhysicalDelete(existing);
                        }

                        Rpt rpt = rptRepository.Get(pix.RptId);
                        rpt.UploadedBy = securityService.getLoginUser().DisplayName;
                        rpt.ORAttachedDate = DateTime.Now;
                        rptRepository.Update(rpt);

                        pictureRepository.Insert(pix);
                        dbContext.SaveChanges();
                        scope.Complete();
                    }
                }
            }
            catch (RptException ex)
            {
                throw new("Error uploading OR.", ex);
            }
        }

        public RPTAttachPicture getRptReceipt(long rptId)
        {
            using (var dbContext = ApplicationDBContext.Create())
            {
                return pictureRepository.getRptReceipt(rptId);
            }
        }

        public List<Rpt> RetrieveBySameRefNumInUploadingEpayment(string taxdec)
        {
            using (var dbContext = ApplicationDBContext.Create())
            {
                return rptRepository.RetrieveBySameRefNumInUploadingEpayment(taxdec);
            }
        }

        public void DeleteAttachedOR(long rptId)
        {
            try
            {
                using (var dbContext = ApplicationDBContext.Create())
                {
                    using (var scope = new TransactionScope())
                    {

                        RPTAttachPicture existing = pictureRepository.getRptReceipt(rptId);
                        if (existing != null)
                        {
                            pictureRepository.PhysicalDelete(existing);
                        }
                        dbContext.SaveChanges();
                        scope.Complete();
                        MessageBox.Show("Successfully deleted OR.");
                    }
                }
            }
            catch (RptException ex)
            {
                throw new("Error deleting attached OR.", ex);
            }
        }

        //List<Rpt> IRptService.DetectExistingRecord(string taxdec, string year, string quarter, string billingSelection)
        //{
        //    using (var dbContext = ApplicationDBContext.Create())
        //    {
        //        return rptRepository.DetectExistingRecord(taxdec, year, quarter, billingSelection);
        //    }
        //}
    }
}
