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

        public List<Business> RetrieveBySearchKeyword(string mpNum)
        {
            return businessRepository.retrieveBySearchKeyword(mpNum);
        }

    }
}
