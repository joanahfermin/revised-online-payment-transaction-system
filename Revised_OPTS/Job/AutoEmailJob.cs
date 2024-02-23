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

        public void Initialize()
        {
            AutoEmailJobTimer = new System.Windows.Forms.Timer();
            AutoEmailJobTimer.Tick += new EventHandler(RunAutoEmail);
            AutoEmailJobTimer.Interval = 10000; // every 10 seconds
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

        }

    }
}
