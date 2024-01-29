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
    internal class MiscService : IMiscService
    {
        IMiscRepository miscRepository = RepositoryFactory.Instance.GetMiscRepository();
        ISecurityService securityService = ServiceFactory.Instance.GetSecurityService();

        public Miscellaneous Get(object id)
        {
            using (var dbContext = ApplicationDBContext.Create())
            {
                return miscRepository.Get(id);
            }
        }

        public List<Miscellaneous> RetrieveBySearchKeyword(string opNum)
        {
            using (var dbContext = ApplicationDBContext.Create())
            {
                return miscRepository.retrieveBySearchKeyword(opNum);
            }
        }

        public void Insert(Miscellaneous misc)
        {
            using (var dbContext = ApplicationDBContext.Create())
            {
                misc.ExcessShort = misc.TransferredAmount - misc.AmountToBePaid;
                misc.EncodedBy = securityService.getLoginUser().DisplayName;
                misc.EncodedDate = DateTime.Now;
                miscRepository.Insert(misc);
                dbContext.SaveChanges();
            }
        }

        public void Update(Miscellaneous misc)
        {
            using (var dbContext = ApplicationDBContext.Create())
            {
                miscRepository.Update(misc);
                dbContext.SaveChanges();
            }
        }

        void IMiscService.RevertSelectedRecordStatus(List<Miscellaneous> miscList)
        {
            using (var dbContext = ApplicationDBContext.Create())
            {
                foreach (Miscellaneous misc in miscList)
                {
                    if (misc.Status == TaxStatus.ForPaymentValidation)
                    {
                        misc.Status = TaxStatus.ForPaymentVerification;
                        misc.VerifiedBy = null;
                        misc.VerifiedDate = null;
                        miscRepository.Update(misc);
                    }
                    else if (misc.Status == TaxStatus.ForORUpload)
                    {
                        misc.Status = TaxStatus.ForPaymentValidation;
                        misc.ValidatedBy = null;
                        misc.ValidatedDate = null;
                        miscRepository.Update(misc);
                    }
                    else if (misc.Status == TaxStatus.ForORPickup)
                    {
                        //TO DO: DELETE THE UPLOADED PHOTO ONCE THE STATUS IS REVERTED.
                        misc.Status = TaxStatus.ForPaymentValidation;
                        //misc.UploadedBy = null;
                        //misc.UploadedDate = null;
                        miscRepository.Update(misc);
                    }
                    else if (misc.Status == TaxStatus.Released)
                    {
                        misc.Status = TaxStatus.ForORPickup;
                        misc.ReleasedBy = null;
                        misc.ReleasedDate = null;
                        //business.RepName = null;
                        //business.RepContactNumber = null;
                        miscRepository.Update(misc);
                    }
                }
                dbContext.SaveChanges();
            }
        }

        void IMiscService.UpdateSelectedRecordsStatus(List<Miscellaneous> miscList, string status)
        {
            using (var dbContext = ApplicationDBContext.Create())
            {
                using (var scope = new TransactionScope())
                {
                    foreach (Miscellaneous misc in miscList)
                    {
                        misc.Status = status;
                        misc.VerifiedBy = securityService.getLoginUser().DisplayName;
                        misc.VerifiedDate = DateTime.Now;
                        miscRepository.Update(misc);
                    }
                    dbContext.SaveChanges();
                    scope.Complete();
                }
            }
        }
    }
}
