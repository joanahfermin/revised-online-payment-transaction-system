using Inventory_System.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revised_OPTS.DAL
{
    internal class RepositoryFactory 
    {
        public static RepositoryFactory Instance = new RepositoryFactory();

        public IRptRepository GetRptRepository()
        {
            return new RptRepository();
        }

        public IMiscRepository GetMiscRepository()
        {
            return new MiscRepository();
        }

        public IBusinessRepository GetBusinessRepository()
        {
            return new BusinessRepository();
        }

        public IBankRepository GetBankRepository()
        {
            return new BankRepository();
        }

        public IUserRepository GetUserRepository()
        {
            return new UserRepository();
        }

        public IRptTaxbillTPNRepository GetRptRetrieveTaxpayerNameRepository()
        {
            return new RptTaxbillTPNRepository();
        }

        public IBusinessMasterDetailTPNRepository GetBusinessRetrieveTaxpayerNameRepository()
        {
            return new BusinessMasterDetailTPNRepository();
        }

        public IMiscDetailsBillingStageRepository MiscRetrieveTaxpayerNameRepository()
        {
            return new MiscDetailsBillingStageRepository();
        }
    }
}
