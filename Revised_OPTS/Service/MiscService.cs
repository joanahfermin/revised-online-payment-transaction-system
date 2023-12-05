using Inventory_System.Service;
using Revised_OPTS.DAL;
using Revised_OPTS.Model;
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
            return miscRepository.Get(id);
        }

        public List<Miscellaneous> RetrieveBySearchKeyword(string opNum)
        {
            return miscRepository.retrieveBySearchKeyword(opNum);
        }

        public void Insert(Miscellaneous misc)
        {
            misc.ExcessShort = misc.TransferredAmount - misc.AmountToBePaid;
            misc.EncodedBy = securityService.getLoginUser().DisplayName;
            misc.EncodedDate = DateTime.Now;
            miscRepository.Insert(misc);
            ApplicationDBContext.Instance.SaveChanges();
        }

        public void Update(Miscellaneous misc)
        {
            miscRepository.Update(misc);
            ApplicationDBContext.Instance.SaveChanges();
        }
    }
}
