﻿using Inventory_System.DAL;
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

        public List<Rpt> RetrieveBySearchKeywordEmailAddress(string tdn)
        {
            using (var dbContext = ApplicationDBContext.Create())
            {
                return SpecialSort(rptRepository.retrieveBySearchKeywordEmailAddress(tdn));
                //return /*SpecialSort*/(rptRepository.retrieveBySearchKeyword(tdn));
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

        public List<Rpt> RetrieveBySearchKeywordForPaymentValidationOnly(string tdn)
        {
            using (var dbContext = ApplicationDBContext.Create())
            {
                return SpecialSort(rptRepository.retrieveBySearchKeywordForPaymentValidationOnly(tdn));
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
                string allTaxdec = string.Join(", ", allDuplicateRpts.Select(t => t.TaxDec).ToHashSet());
                throw new DuplicateRecordException($"There is an existing record/s detected in the database. Please update or delete the old record/s. TDN = {allTaxdec}", allDuplicateRpts);
            }
        }

        public void validateDuplicateRecord(List<Rpt> rptList, List<Business> businessList, List<Miscellaneous> miscList)
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
        public void SaveAll(List<Rpt> listOfRptsToSave, List<Rpt> listOfRptsToDelete, decimal totalAmountTransferred)
        {
            SaveAll(listOfRptsToSave, listOfRptsToDelete, totalAmountTransferred, true);
        }


        //saving all rpt record in the add/update multiple record form.
        public void SaveAll(List<Rpt> listOfRptsToSave, List<Rpt> listOfRptsToDelete, decimal totalAmountTransferred, bool validate)
        {
            using (var dbContext = ApplicationDBContext.Create())
            {
                if (validate)
                {
                    validateRptDuplicateRecord(listOfRptsToSave);
                }
                using (var scope = new TransactionScope())
                {
                    if (listOfRptsToSave.Count > 0)
                    {
                        AssignRefNum(listOfRptsToSave);

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

        public void SaveAllEPayment(List<Rpt> rptList, List<Business> businessList, List<Miscellaneous> miscList)
        {
            SaveAllEPayment(rptList, businessList, miscList, true);
        }

        //save records copy/paste from gcashpaymaya excel
        public void SaveAllEPayment(List<Rpt> rptList, List<Business> businessList, List<Miscellaneous> miscList, bool validate)
        {
            using (var dbContext = ApplicationDBContext.Create())
            {
                if (validate)
                {
                    validateDuplicateRecord(rptList, businessList, miscList);
                }

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

            foreach (string refNum in refNumSet)
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
                    List<long> rptIDList = rptList.Select(rpt => rpt.RptID).ToList();
                    rptRepository.UpdateStatus(rptIDList, status, securityService.getLoginUser().DisplayName, DateTime.Now);


                    //foreach (Rpt rpt in rptList)
                    //{
                    //    if (rpt.Status == TaxStatus.ForPaymentVerification || rpt.Status == TaxStatus.ForPaymentValidation || rpt.Status == TaxStatus.ForORUpload || rpt.Status == TaxStatus.ForORPickup)
                    //    {
                    //        rpt.Status = status;
                    //        rpt.VerifiedBy = securityService.getLoginUser().DisplayName;
                    //        rpt.VerifiedDate = DateTime.Now;
                    //        rptRepository.Update(rpt);
                    //    }
                    //}


                    dbContext.SaveChanges();
                    scope.Complete();
                }
            }
        }


        public void CheckRevertStatus(List<Rpt> rptStatusList, string expectedRevertedStatus)
        {
            foreach (Rpt rpt in rptStatusList)
            {
                if (rpt.Status != TaxStatus.ForPaymentValidation && expectedRevertedStatus == TaxStatus.ForPaymentVerification)
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
                            rpt.LocCode = null;
                            rpt.SendReceiptReady = false;
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

        public int CountORUploadRemainingToSend(string uploadedBy)
        {
            using (var dbContext = ApplicationDBContext.Create())
            {
                return rptRepository.CountORUploadRemainingToSend(uploadedBy);
            }
        }

        public List<Rpt> ListORUploadRemainingToSend(string UploadedBy)
        {
            using (var dbContext = ApplicationDBContext.Create())
            {
                return rptRepository.ListORUploadRemainingToSend(UploadedBy);
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

                        Rpt rpt = rptRepository.Get(rptId);

                        if (existing != null)
                        {
                            pictureRepository.PhysicalDelete(existing);
                            rpt.UploadedBy = null;
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

                //                return dbContext.allTaxTypeReports.FromSqlRaw<AllTaxTypeReport>(
                //                    "SELECT * FROM ( " +
                //"SELECT 'RPT' as TaxType, TaxDec as BillNumber, Collection, Billing, ExcessShort, RPTremarks as Remarks, VerifiedDate, EncodedDate " +
                //"FROM ( " +
                //"    SELECT TaxDec, AmountToPay as Billing, TotalAmountTransferred as Collection,  0 as ExcessShort, RPTremarks, VerifiedDate, RPTID, EncodedDate " +
                //"    FROM Jo_RPT r " +
                //"    WHERE Bank in (SELECT BankName FROM Jo_RPT_Banks WHERE isEBank = 1) " +
                //"    AND DeletedRecord = 0 " +
                //"    AND CAST(ValidatedDate AS Date) >= CAST(@FromDate AS Date) " +
                //"    AND CAST(ValidatedDate AS Date) <= CAST(@ToDate AS Date) " +
                //"    AND ValidatedBy = @UserName " +
                //"    UNION ALL " +
                //"    SELECT TaxDec, AmountToPay as Billing, TotalAmountTransferred as Collection, ExcessShortAmount as ExcessShort, RPTremarks, (SELECT MIN(VerifiedDate) FROM Jo_RPT r2 WHERE r2.RefNum = r.RefNum) as VerifiedDate, RPTID, EncodedDate " +
                //"    FROM Jo_RPT r " +
                //"    WHERE Bank NOT IN (SELECT BankName FROM Jo_RPT_Banks WHERE isEBank = 1) AND RefNum IS NOT NULL " +
                //"    AND DeletedRecord = 0 " +
                //"    AND CAST(ValidatedDate AS Date) >= CAST(@FromDate AS Date) " +
                //"    AND CAST(ValidatedDate AS Date) <= CAST(@ToDate AS Date) " +
                //"    AND ValidatedBy = @UserName " +
                //"    UNION ALL " +
                //"    SELECT TaxDec, AmountToPay as Billing, TotalAmountTransferred as Collection, ExcessShortAmount as ExcessShort, RPTremarks, VerifiedDate, RPTID, EncodedDate " +
                //"    FROM Jo_RPT r " +
                //"    WHERE Bank NOT IN (SELECT BankName FROM Jo_RPT_Banks WHERE isEBank = 1) AND RefNum IS NULL " +
                //"    AND DeletedRecord = 0 " +
                //"    AND CAST(ValidatedDate AS Date) >= CAST(@FromDate AS Date) " +
                //"    AND CAST(ValidatedDate AS Date) <= CAST(@ToDate AS Date) " +
                //"    AND ValidatedBy = @UserName " +
                //"    ) AS sReportView " +
                //"UNION ALL " +
                //"SELECT 'BUSINESS' as TaxType, BillNumber as BillNumber, Collection, Billing, ExcessShort, BussinessRemarks as Remarks, VerifiedDate, EncodedDate " +
                //"FROM ( " +
                //"    SELECT BillNumber, BillAmount as Billing, TotalAmount as Collection, MP_Number, ExcessShort, BussinessRemarks, VerifiedDate, EncodedDate " +
                //"    FROM Jo_Business b " +
                //"    WHERE PaymentChannel in (SELECT BankName FROM Jo_RPT_Banks WHERE isEBank = 1) " +
                //"    AND DeletedRecord = 0 " +
                //"    AND CAST(ValidatedDate AS Date) >= CAST(@FromDate AS Date) " +
                //"    AND CAST(ValidatedDate AS Date) <= CAST(@ToDate AS Date) " +
                //"    AND ValidatedBy = @UserName " +
                //"    UNION ALL " +
                //"    SELECT BillNumber, BillAmount as Billing, TotalAmount as Collection, MP_Number, ExcessShort, BussinessRemarks, (SELECT MIN(VerifiedDate) FROM Jo_Business b2 WHERE b2.RefNum = b.RefNum) as VerifiedDate, EncodedDate " +
                //"    FROM Jo_Business b " +
                //"    WHERE PaymentChannel NOT IN (SELECT BankName FROM Jo_RPT_Banks WHERE isEBank = 1) AND RefNum IS NOT NULL " +
                //"    AND DeletedRecord = 0 " +
                //"    AND CAST(ValidatedDate AS Date) >= CAST(@FromDate AS Date) " +
                //"    AND CAST(ValidatedDate AS Date) <= CAST(@ToDate AS Date) " +
                //"    AND ValidatedBy = @UserName " +
                //"    UNION ALL " +
                //"    SELECT BillNumber, TotalAmount as Collection, BillAmount as Billing, MP_Number, ExcessShort, BussinessRemarks, VerifiedDate, EncodedDate " +
                //"    FROM Jo_Business b " +
                //"    WHERE PaymentChannel NOT IN (SELECT BankName FROM Jo_RPT_Banks WHERE isEBank = 1) AND RefNum IS NULL " +
                //"    AND DeletedRecord = 0 " +
                //"    AND CAST(ValidatedDate AS Date) >= CAST(@FromDate AS Date) " +
                //"    AND CAST(ValidatedDate AS Date) <= CAST(@ToDate AS Date) " +
                //"    AND ValidatedBy = @UserName " +
                //") AS BusinessReportView " +
                //"UNION ALL " +
                //"SELECT 'MISC' as TaxType, OrderOfPaymentNum as BillNumber, Collection, Billing, ExcessShort, Remarks, VerifiedDate, EncodedDate " +
                //"FROM ( " +
                //"    SELECT OrderOfPaymentNum, AmountToBePaid as Billing, TransferredAmount as Collection, ExcessShort, Remarks, VerifiedDate, EncodedDate " +
                //"    FROM Jo_MISC m " +
                //"    WHERE ModeOfPayment in (SELECT BankName FROM Jo_RPT_Banks WHERE isEBank = 1) " +
                //"    AND DeletedRecord = 0 " +
                //"    AND CAST(ValidatedDate AS Date) >= CAST(@FromDate AS Date) " +
                //"    AND CAST(ValidatedDate AS Date) <= CAST(@ToDate AS Date) " +
                //"    AND ValidatedBy = @UserName " +
                //"    UNION ALL " +
                //"    SELECT OrderOfPaymentNum, TransferredAmount as Collection, AmountToBePaid as Billing, ExcessShort, Remarks, (SELECT MIN(VerifiedDate) FROM Jo_MISC m2 WHERE m2.RefNum = m.RefNum) as VerifiedDate, EncodedDate " +
                //"    FROM Jo_MISC m " +
                //"    WHERE ModeOfPayment NOT IN (SELECT BankName FROM Jo_RPT_Banks WHERE isEBank = 1) AND RefNum IS NOT NULL " +
                //"    AND DeletedRecord = 0 " +
                //"    AND CAST(ValidatedDate AS Date) >= CAST(@FromDate AS Date) " +
                //"    AND CAST(ValidatedDate AS Date) <= CAST(@ToDate AS Date) " +
                //"    AND ValidatedBy = @UserName " +
                //"    UNION ALL " +
                //"    SELECT OrderOfPaymentNum, TransferredAmount as Collection, AmountToBePaid as Billing, ExcessShort, Remarks, VerifiedDate, EncodedDate " +
                //"    FROM Jo_MISC m " +
                //"    WHERE ModeOfPayment NOT IN (SELECT BankName FROM Jo_RPT_Banks WHERE isEBank = 1) AND RefNum IS NULL " +
                //"    AND DeletedRecord = 0 " +
                //"    AND CAST(ValidatedDate AS Date) >= CAST(@FromDate AS Date) " +
                //"    AND CAST(ValidatedDate AS Date) <= CAST(@ToDate AS Date) " +
                //"    AND ValidatedBy = @UserName " +
                //") AS MiscReportView " +
                //") AS AllReportView " +
                //"ORDER BY VerifiedDate, EncodedDate;",
                //                    new[] { new SqlParameter("@FromDate", dateFrom), new SqlParameter("@ToDate", dateTo), new SqlParameter("@UserName", UserName) }).ToList();
                List<AllTaxTypeReport> list1 = dbContext.allTaxTypeReports.FromSqlRaw<AllTaxTypeReport>(

                "SELECT 'RPT' as TaxType, TaxDec as BillNumber, Collection, Billing, ExcessShort, RPTremarks as Remarks, ValidatedDate, EncodedDate, 0.00 AS MiscFees " +
                "FROM ( " +
                "    SELECT TaxDec, AmountToPay as Billing, TotalAmountTransferred as Collection,  0 as ExcessShort, RPTremarks, ValidatedDate, RPTID, EncodedDate " +
                "    FROM Jo_RPT r " +
                "    WHERE Bank in (SELECT BankName FROM Jo_RPT_Banks WHERE isEBank = 1) " +
                "    AND DeletedRecord = 0 " +
                "    AND CAST(ValidatedDate AS Date) >= CAST(@FromDate AS Date) " +
                "    AND CAST(ValidatedDate AS Date) <= CAST(@ToDate AS Date) " +
                "    AND ValidatedBy = @UserName " +
                "    UNION ALL " +
                "    SELECT TaxDec, AmountToPay as Billing, TotalAmountTransferred as Collection, ExcessShortAmount as ExcessShort, RPTremarks, (SELECT MIN(ValidatedDate) FROM Jo_RPT r2 WHERE r2.RefNum = r.RefNum) as ValidatedDate, RPTID, EncodedDate " +
                "    FROM Jo_RPT r " +
                "    WHERE Bank NOT IN (SELECT BankName FROM Jo_RPT_Banks WHERE isEBank = 1) AND RefNum IS NOT NULL " +
                "    AND DeletedRecord = 0 " +
                "    AND CAST(ValidatedDate AS Date) >= CAST(@FromDate AS Date) " +
                "    AND CAST(ValidatedDate AS Date) <= CAST(@ToDate AS Date) " +
                "    AND ValidatedBy = @UserName " +
                "    UNION ALL " +
                "    SELECT TaxDec, AmountToPay as Billing, TotalAmountTransferred as Collection, ExcessShortAmount as ExcessShort, RPTremarks, ValidatedDate, RPTID, EncodedDate " +
                "    FROM Jo_RPT r " +
                "    WHERE Bank NOT IN (SELECT BankName FROM Jo_RPT_Banks WHERE isEBank = 1) AND RefNum IS NULL " +
                "    AND DeletedRecord = 0 " +
                "    AND CAST(ValidatedDate AS Date) >= CAST(@FromDate AS Date) " +
                "    AND CAST(ValidatedDate AS Date) <= CAST(@ToDate AS Date) " +
                "    AND ValidatedBy = @UserName " +
                "    ) AS sReportView ORDER BY ValidatedDate, EncodedDate;",
                                    new[] { new SqlParameter("@FromDate", dateFrom), new SqlParameter("@ToDate", dateTo), new SqlParameter("@UserName", UserName) }).ToList();


                List<AllTaxTypeReport> list2 = dbContext.allTaxTypeReports.FromSqlRaw<AllTaxTypeReport>(
                "SELECT 'BUSINESS' as TaxType, BillNumber as BillNumber, Collection, Billing, ExcessShort, BussinessRemarks as Remarks, VerifiedDate, EncodedDate, MiscFees " +
                "FROM ( " +
                "    SELECT BillNumber, TotalAmount as Collection, BillAmount as Billing, MP_Number, ExcessShort, BussinessRemarks, VerifiedDate, EncodedDate, MiscFees " +
                "    FROM Jo_Business b " +
                "    WHERE PaymentChannel in (SELECT BankName FROM Jo_RPT_Banks WHERE isEBank = 1) " +
                "    AND DeletedRecord = 0 " +
                "    AND CAST(ValidatedDate AS Date) >= CAST(@FromDate AS Date) " +
                "    AND CAST(ValidatedDate AS Date) <= CAST(@ToDate AS Date) " +
                "    AND ValidatedBy = @UserName " +
                "    UNION ALL " +
                "    SELECT BillNumber, TotalAmount as Collection, BillAmount as Billing, MP_Number, ExcessShort, BussinessRemarks, (SELECT MIN(VerifiedDate) FROM Jo_Business b2 WHERE b2.RefNum = b.RefNum) as VerifiedDate, EncodedDate,MiscFees " +
                "    FROM Jo_Business b " +
                "    WHERE PaymentChannel NOT IN (SELECT BankName FROM Jo_RPT_Banks WHERE isEBank = 1) AND RefNum IS NOT NULL " +
                "    AND DeletedRecord = 0 " +
                "    AND CAST(ValidatedDate AS Date) >= CAST(@FromDate AS Date) " +
                "    AND CAST(ValidatedDate AS Date) <= CAST(@ToDate AS Date) " +
                "    AND ValidatedBy = @UserName " +
                "    UNION ALL " +
                "    SELECT BillNumber, TotalAmount as Collection, BillAmount as Billing, MP_Number, ExcessShort, BussinessRemarks, VerifiedDate, EncodedDate,MiscFees " +
                "    FROM Jo_Business b " +
                "    WHERE PaymentChannel NOT IN (SELECT BankName FROM Jo_RPT_Banks WHERE isEBank = 1) AND RefNum IS NULL " +
                "    AND DeletedRecord = 0 " +
                "    AND CAST(ValidatedDate AS Date) >= CAST(@FromDate AS Date) " +
                "    AND CAST(ValidatedDate AS Date) <= CAST(@ToDate AS Date) " +
                "    AND ValidatedBy = @UserName " +
                ") AS BusinessReportView ORDER BY VerifiedDate, EncodedDate;",
                                    new[] { new SqlParameter("@FromDate", dateFrom), new SqlParameter("@ToDate", dateTo), new SqlParameter("@UserName", UserName) }).ToList();

                List<AllTaxTypeReport> list3 = dbContext.allTaxTypeReports.FromSqlRaw<AllTaxTypeReport>(
                "SELECT 'MISC' as TaxType, OrderOfPaymentNum as BillNumber, Collection, Billing, ExcessShort, Remarks, VerifiedDate, EncodedDate, 0.00 AS MiscFees " +
                "FROM ( " +
                "    SELECT OrderOfPaymentNum, AmountToBePaid as Billing, TransferredAmount as Collection, ExcessShort, Remarks, VerifiedDate, EncodedDate " +
                "    FROM Jo_MISC m " +
                "    WHERE ModeOfPayment in (SELECT BankName FROM Jo_RPT_Banks WHERE isEBank = 1) " +
                "    AND DeletedRecord = 0 " +
                "    AND CAST(ValidatedDate AS Date) >= CAST(@FromDate AS Date) " +
                "    AND CAST(ValidatedDate AS Date) <= CAST(@ToDate AS Date) " +
                "    AND ValidatedBy = @UserName " +
                "    UNION ALL " +
                "    SELECT OrderOfPaymentNum, TransferredAmount as Collection, AmountToBePaid as Billing, ExcessShort, Remarks, (SELECT MIN(VerifiedDate) FROM Jo_MISC m2 WHERE m2.RefNum = m.RefNum) as VerifiedDate, EncodedDate " +
                "    FROM Jo_MISC m " +
                "    WHERE ModeOfPayment NOT IN (SELECT BankName FROM Jo_RPT_Banks WHERE isEBank = 1) AND RefNum IS NOT NULL " +
                "    AND DeletedRecord = 0 " +
                "    AND CAST(ValidatedDate AS Date) >= CAST(@FromDate AS Date) " +
                "    AND CAST(ValidatedDate AS Date) <= CAST(@ToDate AS Date) " +
                "    AND ValidatedBy = @UserName " +
                "    UNION ALL " +
                "    SELECT OrderOfPaymentNum, TransferredAmount as Collection, AmountToBePaid as Billing, ExcessShort, Remarks, VerifiedDate, EncodedDate " +
                "    FROM Jo_MISC m " +
                "    WHERE ModeOfPayment NOT IN (SELECT BankName FROM Jo_RPT_Banks WHERE isEBank = 1) AND RefNum IS NULL " +
                "    AND DeletedRecord = 0 " +
                "    AND CAST(ValidatedDate AS Date) >= CAST(@FromDate AS Date) " +
                "    AND CAST(ValidatedDate AS Date) <= CAST(@ToDate AS Date) " +
                "    AND ValidatedBy = @UserName " +
                ") AS MiscReportView ORDER BY VerifiedDate, EncodedDate; ",
                                    new[] { new SqlParameter("@FromDate", dateFrom), new SqlParameter("@ToDate", dateTo), new SqlParameter("@UserName", UserName) }).ToList();


                List<AllTaxTypeReport> concatenatedList = new List<AllTaxTypeReport>();
                concatenatedList.AddRange(list1);
                concatenatedList.AddRange(list2);
                concatenatedList.AddRange(list3);
                return concatenatedList;


            }
        }



        public void ChangeStatusForORPickUp(Rpt rpt)
        {
            string UploadedBy = securityService.getLoginUser().DisplayName;
            using (var dbContext = ApplicationDBContext.Create())
            {
                using (var scope = new TransactionScope())
                {
                    if (rpt.UploadedBy == null)
                    {
                        rpt.UploadedBy = UploadedBy;
                    }
                    rpt.Status = TaxStatus.ForORPickup;
                    rpt.UploadedDate = DateTime.Now;
                    rptRepository.Update(rpt);
                    dbContext.SaveChanges();
                    scope.Complete();
                }
            }
        }

        public List<Rpt> ListForLocationCodeAssignment(string locationCode)
        {
            using (var dbContext = ApplicationDBContext.Create())
            {
                return rptRepository.ListForLocationCodeAssignment(locationCode);
            }
        }

        public void AssignmentLocationCode(List<long> rptIDList, string locationCode)
        {
            using (var dbContext = ApplicationDBContext.Create())
            {
                using (var scope = new TransactionScope())
                {
                    rptRepository.AssignmentLocationCode(rptIDList, locationCode);
                    dbContext.SaveChanges();
                    scope.Complete();
                }
            }
        }

        public List<Rpt> RetrieveNoName()
        {
            using (var dbContext = ApplicationDBContext.Create())
            {
                return rptRepository.RetrieveNoName();
            }
        }

        public List<UserActivityReport> RetrieveAllUserActivityReport(DateTime _dateFrom, DateTime _dateTo)
        {
            using (var dbContext = ApplicationDBContext.Create())
            {
                {
                    return dbContext.allUserActivityReports.FromSqlRaw<UserActivityReport>(
                    "SELECT DisplayName, (SELECT COUNT(*) FROM Jo_RPT r WHERE DeletedRecord = 0 AND r.EncodedBy = u.DisplayName AND CAST(EncodedDate AS Date) >= CAST(@FromDate AS Date) AND CAST(EncodedDate AS Date) <= CAST(@ToDate AS Date)) AS EncodedCount, " +
                    "(SELECT COUNT(*) FROM Jo_RPT r WHERE DeletedRecord = 0 AND r.VerifiedBy = u.DisplayName AND CAST(VerifiedDate AS Date) >= CAST(@FromDate AS Date) AND CAST(VerifiedDate AS Date) <= CAST(@ToDate AS Date)) AS VerifiedCount, " +
                    "(SELECT COUNT(*) FROM Jo_RPT r WHERE DeletedRecord = 0 AND r.ValidatedBy = u.DisplayName AND CAST(ValidatedDate AS Date) >= CAST(@FromDate AS Date) AND CAST(ValidatedDate AS Date) <= CAST(@ToDate AS Date)) AS ValidatedCount, " +
                    "(SELECT COUNT(*) FROM Jo_RPT r WHERE DeletedRecord = 0 AND r.UploadedBy = u.DisplayName AND CAST(UploadedDate AS Date) >= CAST(@FromDate AS Date) AND CAST(UploadedDate AS Date) <= CAST(@ToDate AS Date)) AS UploadCount, " +
                    "(SELECT COUNT(*) FROM Jo_RPT r WHERE DeletedRecord = 0 AND r.ReleasedBy = u.DisplayName AND CAST(ReleasedDate AS Date) >= CAST(@FromDate AS Date) AND CAST(ReleasedDate AS Date) <= CAST(@ToDate AS Date)) AS ReleasedCount, " +
                    "(SELECT COUNT(*) FROM Jo_MISC m WHERE DeletedRecord = 0 AND m.EncodedBy = u.DisplayName AND CAST(EncodedDate AS Date) >= CAST(@FromDate AS Date) AND CAST(EncodedDate AS Date) <= CAST(@ToDate AS Date)) AS MiscEncodedCount, " +
                    "(SELECT COUNT(*) FROM Jo_MISC m WHERE DeletedRecord = 0 AND m.VerifiedBy = u.DisplayName AND CAST(VerifiedDate AS Date) >= CAST(@FromDate AS Date) AND CAST(VerifiedDate AS Date) <= CAST(@ToDate AS Date)) AS MiscVerifiedCount, " +
                    "(SELECT COUNT(*) FROM Jo_MISC m WHERE DeletedRecord = 0 AND m.ValidatedBy = u.DisplayName AND CAST(ValidatedDate AS Date) >= CAST(@FromDate AS Date) AND CAST(ValidatedDate AS Date) <= CAST(@ToDate AS Date)) AS MiscValidatedCount, " +
                    "(SELECT COUNT(*) FROM Jo_MISC m WHERE DeletedRecord = 0 AND m.ReleasedBy = u.DisplayName AND CAST(ReleasedDate AS Date) >= CAST(@FromDate AS Date) AND CAST(ReleasedDate AS Date) <= CAST(@ToDate AS Date)) AS MiscReleasedCount, " +
                    "(SELECT COUNT(*) FROM Jo_Business b WHERE DeletedRecord = 0 AND b.EncodedBy = u.DisplayName AND CAST(EncodedDate AS Date) >= CAST(@FromDate AS Date) AND CAST(EncodedDate AS Date) <= CAST(@ToDate AS Date)) AS BusinessEncodedCount, " +
                    "(SELECT COUNT(*) FROM Jo_Business b WHERE DeletedRecord = 0 AND b.VerifiedBy = u.DisplayName AND CAST(VerifiedDate AS Date) >= CAST(@FromDate AS Date) AND CAST(VerifiedDate AS Date) <= CAST(@ToDate AS Date)) AS BusinessVerifiedCount, " +
                    "(SELECT COUNT(*) FROM Jo_Business b WHERE DeletedRecord = 0 AND b.ValidatedBy = u.DisplayName AND CAST(ValidatedDate AS Date) >= CAST(@FromDate AS Date) AND CAST(ValidatedDate AS Date) <= CAST(@ToDate AS Date)) AS BusinessValidatedCount " +
                    " FROM Jo_RPT_Users u WHERE isActive = 1 ORDER BY DisplayName ASC ; ",
                    new[] { new SqlParameter("@FromDate", _dateFrom), new SqlParameter("@ToDate", _dateTo) }).ToList();
                }
            }
        }
    }
}

