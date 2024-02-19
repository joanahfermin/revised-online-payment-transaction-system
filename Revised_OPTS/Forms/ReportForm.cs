using Inventory_System.Utilities;
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
    public partial class ReportForm : Form
    {
        private Image originalBackgroundImageRpt;

        public ReportForm()
        {
            InitializeComponent();
            InitializeTaxType();
            //DgMainForm.DefaultCellStyle.Font = new Font("Tahoma", 12, FontStyle.Regular);
            this.WindowState = FormWindowState.Maximized;
        }

        public void InitializeTaxType()
        {
            foreach (string allReport in ReportUtil.ALL_REPORT.OrderBy(reportType => reportType))
            {
                cbTaxType.Items.Add(allReport);
            }
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {

        }

        private void btnClose_MouseEnter(object sender, EventArgs e)
        {
            originalBackgroundImageRpt = btnClose.BackgroundImage;
            btnClose.BackgroundImage = null;

            Color customColor = Color.FromArgb(23, 45, 74);
            btnClose.BackColor = customColor;
        }

        private void btnClose_MouseLeave(object sender, EventArgs e)
        {
            btnClose.BackgroundImage = originalBackgroundImageRpt;
        }

        private void btnGenerate_MouseEnter(object sender, EventArgs e)
        {
            originalBackgroundImageRpt = btnGenerate.BackgroundImage;
            btnGenerate.BackgroundImage = null;

            Color customColor = Color.FromArgb(23, 45, 74);
            btnGenerate.BackColor = customColor;
        }

        private void btnGenerate_MouseLeave(object sender, EventArgs e)
        {
            btnGenerate.BackgroundImage = originalBackgroundImageRpt;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
