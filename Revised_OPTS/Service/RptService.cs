using Inventory_System.DAL;
using Inventory_System.Exception;
using Inventory_System.Forms;
using Inventory_System.Model;
using Inventory_System.Service;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
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

        IMiscService miscService = ServiceFactory.Instance.GetMiscService();

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
                return SpecialSort(rptRepository.retrieveBySearchKeyword(tdn));
                //return /*SpecialSort*/(rptRepository.retrieveBySearchKeyword(tdn));
            }
        }

        public List<Rpt> RetrieveForORUploadRegular(DateTime date, string validatedBy)
        {
            using (var dbContext = ApplicationDBContext.Create())
            {
                return rptRepository.RetrieveForORUploadRegular(date, validatedBy);
            }
        }

        public void ConfirmSendOrUpload(List<long> RptIDList)
        {
            using (var dbContext = ApplicationDBContext.Create())
            {
                using (var scope = new TransactionScope())
                {
                    rptRepository.ConfirmSendOrUpload(RptIDList);
                    dbContext.SaveChanges();
                    scope.Complete();
                }
            }
        }

        public void Insert(Rpt rpt)
        {
            using (var dbContext = ApplicationDBContext.Create())
            {
                rpt.TotalAmountTransferred = rpt.AmountTransferred;

                rpt.ExcessShortAmount = rpt.TotalAmountTransferred - rpt.AmountToPay;
                rpt.EncodedBy = securityService.getLoginUser().DisplayName;
                rpt.EncodedDate = DateTime.Now;
                rpt.Status = TaxStatus.ForPaymentVerification;
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
                    throw new RptException($"Operation invalid. You are trying to insert records that has duplicate(s). TDN = {duplicate.TaxDec}");
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
                miscService.validateMiscDuplicateRecord(miscList);
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

        public void UpdateAllinDuplicateRecordForm(List<Rpt> listOfRptsToSave)
        {
            try
            {
                using (var dbContext = ApplicationDBContext.Create())
                {
                    using (var scope = new TransactionScope())
                    {
                        foreach (Rpt rpt in listOfRptsToSave)
                        {
                            rpt.LastUpdateBy = securityService.getLoginUser().DisplayName;
                            rpt.LastUpdateDate = DateTime.Now.ToString();

                            rptRepository.Update(rpt);
                        }
                        dbContext.SaveChanges();
                        scope.Complete();
                    }
                }
            }
            catch (RptException ex)
            {

                throw new RptException("Invalid action.");
            }
        }

        //saving all rpt record in the add/update multiple record form.
        public void SaveAll(List<Rpt> listOfRptsToSave, List<Rpt> listOfRptsToDelete, decimal totalAmountTransferred)
        {
            using (var dbContext = ApplicationDBContext.Create())
            {
                validateRptDuplicateRecord(listOfRptsToSave);
                using (var scope = new TransactionScope())
                {
                    if (listOfRptsToSave.Count > 0)
                    {
                        if (listOfRptsToSave.Count > 1)
                        {
                            AssignRefNum(listOfRptsToSave);
                        }
                        bool firstRecord = true;

                        calculateTotalPayment(listOfRptsToSave, totalAmountTransferred);

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
                    }
                    foreach (Rpt rptToDelete in listOfRptsToDelete)
                    {
                        if (rptToDelete.RptID > 0)
                        {
                            rptRepository.Delete(rptToDelete);
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
                List<Rpt> tempList = rptRepository.retrieveBySameRefNumAndReqParty(refNum, reqParty);
                return SpecialSort(tempList);
            }
        }

        //correct sorting of rpt record if data pasted in the system has additional record.
        //NOTE: invalid, in case record to be added is the same as the taxdec of the original record.
        public List<Rpt> SpecialSort(List<Rpt> rptList)
        {
            List<Rpt> resultList = new List<Rpt>();
            List<long> processedRpdIDList = new List<long>(resultList.Count);
            HashSet<string> refNumSet = rptList.Select(rpt => rpt.RefNum).ToHashSet();

            foreach(string refNum in refNumSet)
            {
                foreach (Rpt rpt in rptList)
                {
                    if (rpt.RefNum != refNum)
                    {
                        continue;
                    }
                    if (!processedRpdIDList.Contains(rpt.RptID))
                    {
                        resultList.Add(rpt);
                        processedRpdIDList.Add(rpt.RptID);
                        foreach (Rpt rpt2 in rptList)
                        {
                            if (rpt2.RefNum != refNum)
                            {
                                continue;
                            }
                            if (!processedRpdIDList.Contains(rpt2.RptID) && rpt.TaxDec == rpt2.TaxDec)
                            {
                                resultList.Add(rpt2);
                                processedRpdIDList.Add(rpt2.RptID);
                            }
                        }
                    }
                }
            }

            return resultList;
        }

        public void ReleaseReceipt(List<Rpt> rptList, string status, string repName, string contactNum, string releaser)
        {
            using (var dbContext = ApplicationDBContext.Create())
            {
                using (var scope = new TransactionScope())
                {
                    foreach (Rpt rpt in rptList)
                    {
                        rpt.Status = status;
                        rpt.RepName = repName;
                        rpt.ContactNumber = contactNum;
                        rpt.ReleasedBy = releaser;
                        //rpt.ReleasedBy = securityService.getLoginUser().DisplayName;
                        rpt.ReleasedDate = DateTime.Now;
                        rptRepository.Update(rpt);
                    }
                    dbContext.SaveChanges();
                    scope.Complete();
                }
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

        public void CheckRevertStatus(List<Rpt> rptStatusList, string expectedRevertedStatus)
        {
            foreach (Rpt rpt in rptStatusList)
            {
                if ( rpt.Status != TaxStatus.ForPaymentValidation && expectedRevertedStatus == TaxStatus.ForPaymentVerification)
                {
                    throw new RptException($"INCORRECT STATUS DETECTED. Please check the record(s) you are trying to revert. The record(s) you are trying to revert should have a status of '{TaxStatus.ForPaymentValidation}'. \n\n" +
                                         "Please be informed of the following CORRECT sequence of statuses:\n" +
                                         "FOR PAYMENT VERIFICATION > FOR PAYMENT VALIDATION > FOR O.R UPLOAD > FOR O.R PICK UP > RELEASED");
                }

                else if (rpt.Status != TaxStatus.ForORUpload && expectedRevertedStatus == TaxStatus.ForPaymentValidation)
                {
                    throw new RptException($"INCORRECT STATUS DETECTED. Please check the record(s) you are trying to revert. The record(s) you are trying to revert should have a status of '{TaxStatus.ForORUpload}'. \n\n" +
                                         "Please be informed of the following CORRECT sequence of statuses:\n" +
                                         "FOR PAYMENT VERIFICATION > FOR PAYMENT VALIDATION > FOR O.R UPLOAD > FOR O.R PICK UP > RELEASED");
                }

                else if (rpt.Status != TaxStatus.ForORPickup && expectedRevertedStatus == TaxStatus.ForORUpload)
                {
                    throw new RptException($"INCORRECT STATUS DETECTED. Please check the record(s) you are trying to revert. The record(s) you are trying to revert should have a status of '{TaxStatus.ForORPickup}'\n\n" +
                                         "Please be informed of the following CORRECT sequence of statuses:\n" +
                                         "FOR PAYMENT VERIFICATION > FOR PAYMENT VALIDATION > FOR O.R UPLOAD > FOR O.R PICK UP > RELEASED");
                }

                else if (rpt.Status != TaxStatus.Released && expectedRevertedStatus == TaxStatus.ForORPickup)
                {
                    throw new RptException($"INCORRECT STATUS DETECTED. Please check the record(s) you are trying to revert. The record(s) you are trying to revert should have a status of '{TaxStatus.Released}'. \n\n" +
                                         "Please be informed of the following CORRECT sequence of statuses:\n" +
                                         "FOR PAYMENT VERIFICATION > FOR PAYMENT VALIDATION > FOR O.R UPLOAD > FOR O.R PICK UP > RELEASED");
                }
            }
        }

        //public void CheckAttachedPicture(List<Rpt> rptList)
        //{
        //    List<Rpt> rptWithAttachedORList = new List<Rpt>();

        //    foreach (Rpt rpt in rptList)
        //    {
        //        RPTAttachPicture existing = pictureRepository.getRptReceipt(rpt.RptID);

        //        if (rpt.Status == TaxStatus.ForORPickup && existing.PictureId != null)
        //        {
        //            rptWithAttachedORList.Add(rpt);
        //        }
        //    }
        //    if (rptWithAttachedORList.Count > 0)
        //    {
        //        MessageBox.Show("Attached receipt detected. Kindly note that if an attempt is made to revert a record containing an attached receipt picture, the system will automatically delete the associated image.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //    }
        //}

        public void RevertSelectedRecordStatus(List<Rpt> rptList, string expectedRevertedStatus)
        {
            using (var dbContext = ApplicationDBContext.Create())
            {
                using (var scope = new TransactionScope())
                {
                    foreach (Rpt rpt in rptList)
                    {
                        if (rpt.Status == TaxStatus.ForPaymentValidation)
                        {
                            rpt.Status = expectedRevertedStatus;
                            rpt.VerifiedBy = null;
                            rpt.VerifiedDate = null;
                            rptRepository.Update(rpt);
                        }
                        else if (rpt.Status == TaxStatus.ForORUpload)
                        {
                            rpt.Status = expectedRevertedStatus;
                            rpt.ValidatedBy = null;
                            rpt.ValidatedDate = null;
                            rptRepository.Update(rpt);
                        }
                        else if (rpt.Status == TaxStatus.ForORPickup)
                        {
                            RPTAttachPicture existing = pictureRepository.getRptReceipt(rpt.RptID);
                            if (existing != null)
                            {
                                pictureRepository.PhysicalDelete(existing);
                            }

                            rpt.Status = expectedRevertedStatus;
                            rpt.UploadedBy = null;
                            rpt.UploadedDate = null;
                            rptRepository.Update(rpt);
                        }
                        else if (rpt.Status == TaxStatus.Released)
                        {
                            rpt.Status = expectedRevertedStatus;
                            rpt.ReleasedBy = null;
                            rpt.ReleasedDate = null;
                            rpt.RepName = null;
                            rpt.ContactNumber = null;
                            rptRepository.Update(rpt);
                        }
                    }
                    dbContext.SaveChanges();
                    scope.Complete();
                }
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

        public void DeleteAll(List<Rpt> rptsToDelete)
        {
            using (var dbContext = ApplicationDBContext.Create())
            {
                using (var scope = new TransactionScope())
                {
                    foreach (Rpt rpt in rptsToDelete)
                    {
                        rpt.DeletedRecord = 1;
                        rptRepository.Update(rpt);
                    }
                    dbContext.SaveChanges();
                    scope.Complete();
                }
            }
        }

        public int CoundForORUploadWithPhoto(List<long> rptIDList)
        {
            using (var dbContext = ApplicationDBContext.Create())
            {
                return rptRepository.CoundForORUploadWithPhoto(rptIDList);
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

        public List<Rpt> RetrieveBySameRefNum(string refNum)
        {
            using (var dbContext = ApplicationDBContext.Create())
            {
                return SpecialSort(rptRepository.retrieveBySameRefNum(refNum));
            }
        }

        public List<Rpt> RetrieveBySameRefNumInUploadingEpayment(string taxdec)
        {
            using (var dbContext = ApplicationDBContext.Create())
            {
                return SpecialSort(rptRepository.RetrieveBySameRefNumInUploadingEpayment(taxdec));
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
                        MessageBox.Show("Sucessfully deleted receipt.");
                    }
                }
            }
            catch (RptException ex)
            {
                throw new("Error deleting attached OR.", ex);
            }
        }

        public List<AllTaxTypeReport> RetrieveByValidatedDate(DateTime dateFrom, DateTime dateTo)
        {
            using (var dbContext = ApplicationDBContext.Create())
            {
                string UserName = securityService.getLoginUser().DisplayName;
                //List<string> regularBanks = bankRepository.GetRegularBanks().Select(p => p.BankName).ToList();
                List<string> eBanks = bankRepository.GetElectronicBanks().Select(p => p.BankName).ToList();

                return dbContext.allTaxTypeReports.FromSqlRaw<AllTaxTypeReport>(
                "SELECT 'RPT' as TaxType, TaxDec as BillNumber, TaxPayerName, Collection, Billing, ExcessShort, RPTremarks as Remarks " +
                "FROM ( " +
                " SELECT TaxDec, TaxPayerName, AmountTransferred as Collection,  AmountToPay as Billing, 0 as ExcessShort, RPTremarks, ValidatedDate, RPTID, EncodedDate " +
                " FROM Jo_RPT r " +
                " WHERE Bank in (SELECT BankName FROM Jo_RPT_Banks where isEBank = 1) " +
                " and DeletedRecord = 0 and CAST(ValidatedDate AS Date)>= CAST(@FromDate AS Date) and CAST(ValidatedDate AS Date) <= CAST(@ToDate AS Date) and ValidatedBy=@UserName " +
                " UNION " +

                " SELECT TaxDec, TaxPayerName, TotalAmountTransferred as Collection, AmountToPay as Billing, ExcessShortAmount as ExcessShort, RPTremarks, (select min(ValidatedDate) from Jo_RPT r2 where r2.RefNum = r.RefNum ) as ValidatedDate, RPTID, EncodedDate " +
                " FROM Jo_RPT r " +
                " WHERE Bank not in (SELECT BankName FROM Jo_RPT_Banks where isEBank = 1) and RefNum is not null " +
                " and DeletedRecord = 0 and CAST(ValidatedDate AS Date)>= CAST(@FromDate AS Date) and CAST(ValidatedDate AS Date) <= CAST(@ToDate AS Date) and ValidatedBy=@UserName " +
                " UNION " +

                " SELECT TaxDec, TaxPayerName, TotalAmountTransferred as Collection, AmountToPay as Billing, ExcessShortAmount as ExcessShort, RPTremarks, ValidatedDate, RPTID, EncodedDate " +
                " FROM Jo_RPT r " +
                " WHERE Bank not in (SELECT BankName FROM Jo_RPT_Banks where isEBank = 1) and RefNum is null " +
                " and DeletedRecord = 0 and CAST(ValidatedDate AS Date)>= CAST(@FromDate AS Date) and CAST(ValidatedDate AS Date) <= CAST(@ToDate AS Date) and ValidatedBy=@UserName " +
                ") AS ReportView " +
                "order by ValidatedDate, EncodedDate ",

                new[] { new SqlParameter("@FromDate", dateFrom), new SqlParameter("@ToDate", dateTo), new SqlParameter("@UserName", UserName) }).ToList();
            }
        }
    }
}
