using Inventory_System.Service;
using Revised_OPTS.DAL;
using Revised_OPTS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public List<Business> RetrieveBySearchKeyword(string mpNum)
        {
            using (var dbContext = ApplicationDBContext.Create())
            {
                return businessRepository.retrieveBySearchKeyword(mpNum);
            }
        }

        public void Insert(Business business)
        {
            using (var dbContext = ApplicationDBContext.Create())
            {
                business.EncodedBy = securityService.getLoginUser().DisplayName;
                business.EncodedDate = DateTime.Now;
                businessRepository.Insert(business);
                dbContext.SaveChanges();
            }
        }

        public void Update(Business business)
        {
            using (var dbContext = ApplicationDBContext.Create())
            {
                businessRepository.Update(business);
                dbContext.SaveChanges();
            }

        }
    }
}
