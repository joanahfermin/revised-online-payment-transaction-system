﻿using Inventory_System.Exception;
using Inventory_System.Service;
using Revised_OPTS.DAL;
using Revised_OPTS.Model;
using Revised_OPTS.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Revised_OPTS.Service
{
    internal class BusinessService : IBusinessService
    {
        IBusinessRepository businessRepository = RepositoryFactory.Instance.GetBusinessRepository();
        ISecurityService securityService = ServiceFactory.Instance.GetSecurityService();

        public Business Get(object id)
        {
            using (var dbContext = ApplicationDBContext.Create())
            {
                return businessRepository.Get(id);
            }
        }

        public List<Business> RetrieveBySearchKeyword(string billNumber)
        {
            using (var dbContext = ApplicationDBContext.Create())
            {
                return businessRepository.retrieveBySearchKeyword(billNumber);
            }
        }

        public void Insert(List<Business> businessList)
        {
            using (var dbContext = ApplicationDBContext.Create())
            {
                validateBusinessDuplicateRecord(businessList);

                foreach (Business bus in businessList)
                {
                    bus.EncodedBy = securityService.getLoginUser().DisplayName;
                    bus.EncodedDate = DateTime.Now;
                    businessRepository.Insert(bus);
                }
                dbContext.SaveChanges();
            }
        }

        public void Update(Business business)
        {
            using (var dbContext = ApplicationDBContext.Create())
            {
                List<Business> businessList = new List<Business>();
                businessList.Add(business);

                validateBusinessDuplicateRecord(businessList);

                businessRepository.Update(business);
                dbContext.SaveChanges();
            }
        }

        public void RevertSelectedRecordStatus(List<Business> businessList)
        {
            using (var dbContext = ApplicationDBContext.Create())
            {
                foreach (Business business in businessList)
                {
                    if (business.Status == TaxStatus.ForPaymentValidation)
                    {
                        business.Status = TaxStatus.ForPaymentVerification;
                        business.VerifiedBy = null;
                        business.VerifiedDate = null;
                        businessRepository.Update(business);
                    }
                    else if (business.Status == TaxStatus.ForORUpload)
                    {
                        business.Status = TaxStatus.ForPaymentValidation;
                        business.ValidatedBy = null;
                        business.ValidatedDate = null;
                        businessRepository.Update(business);
                    }
                    else if (business.Status == TaxStatus.ForORPickup)
                    {
                        //TO DO: DELETE THE UPLOADED PHOTO ONCE THE STATUS IS REVERTED.
                        business.Status = TaxStatus.ForPaymentValidation;
                        business.UploadedBy = null;
                        business.UploadedDate = null;
                        businessRepository.Update(business);
                    }
                    else if (business.Status == TaxStatus.Released)
                    {
                        business.Status = TaxStatus.ForORPickup;
                        business.ReleasedBy = null;
                        business.ReleasedDate = null;
                        //business.RepName = null;
                        //business.RepContactNumber = null;
                        businessRepository.Update(business);
                    }
                }
                dbContext.SaveChanges();
            }
        }

        public void UpdateSelectedRecordsStatus(List<Business> businessList, string status)
        {
            using (var dbContext = ApplicationDBContext.Create())
            {
                using (var scope = new TransactionScope())
                {
                    foreach (Business business in businessList)
                    {
                        if (business.Status == TaxStatus.ForPaymentVerification)
                        {
                            business.Status = status;
                            business.VerifiedBy = securityService.getLoginUser().DisplayName;
                            business.VerifiedDate = DateTime.Now;
                            businessRepository.Update(business);
                        }
                        else if (business.Status == TaxStatus.ForPaymentValidation)
                        {
                            business.Status = status;
                            business.ValidatedBy = securityService.getLoginUser().DisplayName;
                            business.ValidatedDate = DateTime.Now;
                            businessRepository.Update(business);
                        }
                        else if (business.Status == TaxStatus.ForTransmittal)
                        {
                            business.Status = status;
                            business.TransmittedBy = securityService.getLoginUser().DisplayName;
                            business.TransmittedDate = DateTime.Now;
                            businessRepository.Update(business);
                        }
                    }
                    dbContext.SaveChanges();
                    scope.Complete();
                }
            }
        }

        public void validateBusinessDuplicateRecord(List<Business> listOfBusToSave)
        {
            var duplicates = listOfBusToSave
            .GroupBy(obj => new
            {
                obj.BillNumber,
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
                    throw new RptException($"Submitted record(s) contains duplicate(s). Bill Number = {duplicate.BillNumber}");
                }
            }

            //retrieve existing record from the database.
            List<Business> allDuplicateBus = new List<Business>();
            foreach (Business bus in listOfBusToSave)
            {
                List<Business> existingRecordList = businessRepository.checkExistingRecord(bus);
                allDuplicateBus.AddRange(existingRecordList);
            }
            if (allDuplicateBus.Count > 0)
            {
                string allBillNumber = string.Join(", ", allDuplicateBus.Select(t => t.BillNumber).ToHashSet());
                throw new DuplicateRecordException($"There is an existing record/s detected in the database. Please update or delete the old record/s. Bill Number = {allBillNumber}", allDuplicateBus);
            }
        }

    }
}
