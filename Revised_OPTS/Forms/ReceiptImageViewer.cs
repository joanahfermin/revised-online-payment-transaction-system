using Inventory_System.Model;
using Revised_OPTS.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Inventory_System.Forms
{
    public partial class ReceiptImageViewer : Form
    {
        long RptID = 0;
        IRptService rptService = ServiceFactory.Instance.GetRptService();
        private Image originalCloseBackgroundImage;

        public ReceiptImageViewer(long rptID)
        {
            InitializeComponent();
            RptID = rptID;

            RPTAttachPicture retrievedPic = rptService.getRptReceipt(rptID);
            pbReceipt.Image = Image.FromStream(new MemoryStream(retrievedPic.FileData));
            pbReceipt.SizeMode = PictureBoxSizeMode.StretchImage;
        }

        private void btnClose_MouseEnter(object sender, EventArgs e)
        {
            originalCloseBackgroundImage = btnClose.BackgroundImage;
            btnClose.BackgroundImage = null;

            Color customColor = Color.FromArgb(23, 45, 74);
            btnClose.BackColor = customColor;
        }

        private void btnClose_MouseLeave(object sender, EventArgs e)
        {
            btnClose.BackgroundImage = originalCloseBackgroundImage;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
