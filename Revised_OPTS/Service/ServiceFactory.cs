using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revised_OPTS.Service
{
    internal class ServiceFactory
    {
        public static ServiceFactory Instance = new ServiceFactory();

        public IRptService GetRptService()
        {
            return new RptService();
        }

        public IMiscService GetMiscService()
        {
            return new MiscService();
        }

        public IBusinessService GetBusinessService()
        {
            return new BusinessService();
        }
    }
}
