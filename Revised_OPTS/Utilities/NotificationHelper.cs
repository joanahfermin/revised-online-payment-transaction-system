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
        public void notifyUserAndRefreshRecord(string keyWord)
        {
            MessageBox.Show("Record successfully saved.");
            MainForm.Instance.Search(keyWord);
        }

    }
}
