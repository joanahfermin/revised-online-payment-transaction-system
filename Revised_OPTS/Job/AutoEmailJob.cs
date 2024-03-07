using Inventory_System.Model;
using Inventory_System.Service;
using Inventory_System.Utilities;
using Revised_OPTS.Model;
using Revised_OPTS.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Inventory_System.Job
{
    public class AutoEmailJob
    {
        private System.Windows.Forms.Timer AutoEmailJobTimer;

        IRptService rptService = ServiceFactory.Instance.GetRptService();
        ISecurityService securityService = ServiceFactory.Instance.GetSecurityService();
        ISystemService systemService = ServiceFactory.Instance.GetSystemService();

        public void Initialize()
        {
            AutoEmailJobTimer = new System.Windows.Forms.Timer();
            AutoEmailJobTimer.Tick += new EventHandler(RunAutoEmail);
            AutoEmailJobTimer.Interval = 3 * 60 * 1000; // every 3 minutes
            //AutoEmailJobTimer.Interval = 25 * 1000; // every 5 minutes
            AutoEmailJobTimer.Start();
        }

        public void RunAutoEmail(object sender, EventArgs e)
        {
            Task.Run(() => RunAutoEmailLogic());
        }

        private async void RunAutoEmailLogic()
        {
            Console.WriteLine("RunAutoEmail");
            SendORReceipt();
        }

        //Send email of status: FOR OR UPLOAD in the background. 
        public void SendORReceipt()
        {
            UserAccount account = securityService.getLoginUser();
            string UploadedBy = account.DisplayName;
            List<Rpt> rptToSendList = rptService.ListORUploadRemainingToSend(UploadedBy);
            EmailTemplate template = systemService.GetORUploadTemplate();

            foreach (Rpt rpt in rptToSendList)
            {
                RPTAttachPicture RetrieveIdAndImage = rptService.getRptReceipt(rpt.RptID);

                string body = "ATTENTION: " + rpt.TaxPayerName + " (" + rpt.TaxDec + ") " + rpt.YearQuarter + " \n" + template.Body + "\n\n" + rpt.UploadedBy + "-CTO";
                string subject = template.Subject + " - " + rpt.TaxDec + "(" + rpt.YearQuarter + ")";

                bool result = GmailUtil.SendMail(rpt.RequestingParty, subject, body, RetrieveIdAndImage);
                if (result == true)
                {                    
                    rptService.ChangeStatusForORPickUp(rpt);
                }
            }
        }
    }
}
