using Revised_OPTS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory_System.Utilities
{
    internal class NotificationHelper
    {
        //RECORD SUCCESSFULLY SAVED.
        public static void notifyUserAndRefreshRecord(string keyWord)
        {
            MessageBox.Show("Record successfully saved.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            MainForm.Instance.Search(keyWord);
        }
    }
}
