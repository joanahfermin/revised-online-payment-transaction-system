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
            return new RptRepository(ApplicationDBContext.Instance);
        }

        public IMiscRepository GetMiscRepository()
        {
            return new MiscRepository(ApplicationDBContext.Instance);
        }

        public IBusinessRepository GetBusinessRepository()
        {
            return new BusinessRepository(ApplicationDBContext.Instance);
        }

        public IBankRepository GetBankRepository()
        {
            return new BankRepository(ApplicationDBContext.Instance);
        }

        public IRptTaxbillTPNRepository GetRptRetrieveTaxpayerNameRepository()
        {
            return new RptTaxbillTPNRepository(ITDDFMUDAILY2022ApplicationDBContext.Instance);
        }

        public IBusinessMasterDetailTPNRepository GetBusinessRetrieveTaxpayerNameRepository()
        {
            return new BusinessMasterDetailTPNRepository(ITDDFMUDAILY2022ApplicationDBContext.Instance);
        }

        public IMiscDetailsBillingStageRepository MiscRetrieveTaxpayerNameRepository()
        {
            return new MiscDetailsBillingStageRepository(ITDDFMUDAILY2023ApplicationDBContext.Instance);
        }

    }
}
