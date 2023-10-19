using Revised_OPTS.DAL;
using Revised_OPTS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revised_OPTS.Service
{
    internal class MiscService : IMiscService
    {
        IMiscRepository rptRepository = RepositoryFactory.Instance.GetMiscRepository();

        public List<Miscellaneous> RetrieveBySearchKeyword(string opNum)
        {
            return rptRepository.retrieveBySearchKeyword(opNum);
        }

    }
}
