using Inventory_System.Model;
using Inventory_System.Service;
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
        IEmailTemplateService emailTemplateService = ServiceFactory.Instance.GetEmailTemplateService();

        public void Initialize()
        {
            AutoEmailJobTimer = new System.Windows.Forms.Timer();
            AutoEmailJobTimer.Tick += new EventHandler(RunAutoEmail);
            AutoEmailJobTimer.Interval = 5*60*1000; // every 5 minutes
            AutoEmailJobTimer.Interval = 25 * 1000; // every 5 minutes
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
            //List<Rpt> rptToSendList = rptService.ListORUploadRemainingToSend(UploadedBy);
            EmailTemplate template = emailTemplateService.GetORUploadTemplate();

            MessageBox.Show(template.Subject);
        }

    }
}
