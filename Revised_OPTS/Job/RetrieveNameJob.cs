using Inventory_System.DAL;
using Inventory_System.Model;
using Revised_OPTS.DAL;
using Revised_OPTS.Model;
using Revised_OPTS.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory_System.Job
{
    internal class RetrieveNameJob
    {
        private System.Windows.Forms.Timer AutoRetrieveNameJobTimer;

        IRptService rptService = ServiceFactory.Instance.GetRptService();
        IBusinessService busService = ServiceFactory.Instance.GetBusinessService();

        IRptTaxbillTPNRepository rptRetrieveTaxpayerNameRep = RepositoryFactory.Instance.GetRptRetrieveTaxpayerNameRepository();
        IBusinessMasterDetailTPNRepository busRetrieveTNameRep = RepositoryFactory.Instance.GetBusinessRetrieveTaxpayerNameRepository();

        public void Initialize()
        {
            AutoRetrieveNameJobTimer = new System.Windows.Forms.Timer();
            AutoRetrieveNameJobTimer.Tick += new EventHandler(RunAutoRetrieveName);
            AutoRetrieveNameJobTimer.Interval = 3 * 60 * 1000; // every 3 minutes
            AutoRetrieveNameJobTimer.Start();
        }

        public void RunAutoRetrieveName(object sender, EventArgs e)
        {
            Task.Run(() => RunAutoRetrieveNameLogic());
        }

        private async void RunAutoRetrieveNameLogic()
        {
            Console.WriteLine("RunAutoEmail");
            RptRetrieveName();
            BusinessRetrieveName();
        }

        public void RptRetrieveName()
        {
            List<Rpt> retrieveNoNameRptList = rptService.RetrieveNoName();

            foreach (Rpt rpt in retrieveNoNameRptList)
            {
                RptTaxbillTPN retrievedTPN = rptRetrieveTaxpayerNameRep.retrieveByTDN(rpt.TaxDec);

                if (retrievedTPN != null)
                {
                    rpt.TaxPayerName = retrievedTPN.ONAME;
                    rptService.Update(rpt);
                }
            }
        }

        public void BusinessRetrieveName()
        {
            List<Business> retrieveNoNameBusList = busService.RetrieveNoName();

            foreach (Business bus in retrieveNoNameBusList)
            {
                BusinessMasterDetailTPN retrieveBusName = busRetrieveTNameRep.retrieveByBillNumber(bus.BillNumber);

                if (retrieveBusName != null)
                {
                    bus.TaxpayersName = retrieveBusName.TaxpayerName;
                    busService.Update(bus);
                }
            }
        }
    }
}
