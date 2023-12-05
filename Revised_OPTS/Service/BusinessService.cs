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
            return businessRepository.Get(id);
        }

        public List<Business> RetrieveBySearchKeyword(string mpNum)
        {
            return businessRepository.retrieveBySearchKeyword(mpNum);
        }

        public void Insert(Business business)
        {
            business.EncodedBy = securityService.getLoginUser().DisplayName;
            business.EncodedDate = DateTime.Now;
            businessRepository.Insert(business);
            ApplicationDBContext.Instance.SaveChanges();
        }

        public void Update(Business business)
        {
            businessRepository.Update(business);
            ApplicationDBContext.Instance.SaveChanges();
        }
    }
}
