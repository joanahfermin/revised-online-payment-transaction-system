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
    }
}
