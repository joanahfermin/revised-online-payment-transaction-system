using Inventory_System.Model;
using Inventory_System.Service;
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
    public partial class EmailTemplateForm : Form
    {
        ISystemService systemService = ServiceFactory.Instance.GetSystemService();
        private Image originalBackgroundImageRpt;

        public EmailTemplateForm()
        {
            //this.WindowState = FormWindowState.Maximized;

            InitializeComponent();
            loadEmailBodyTemplate();
        }

        private void loadEmailBodyTemplate()
        {
            EmailTemplate mgTemplate = systemService.GetORUploadTemplate();

            tbEditorEmailTemp.Text = mgTemplate.Body;
        }

        private void btnSaveRecord_Click(object sender, EventArgs e)
        {
            EmailTemplate mgTemplate = systemService.GetORUploadTemplate();
            mgTemplate.Body = tbEditorEmailTemp.Text;
            systemService.Update(mgTemplate);

            MessageBox.Show("Record successfully saved.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

            loadEmailBodyTemplate();
        }

        private void btnSaveRecord_MouseEnter(object sender, EventArgs e)
        {
            originalBackgroundImageRpt = btnSaveRecord.BackgroundImage;
            btnSaveRecord.BackgroundImage = null;

            Color customColor = Color.FromArgb(23, 45, 74);
            btnSaveRecord.BackColor = customColor;
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

        private void btnSaveRecord_MouseLeave(object sender, EventArgs e)
        {
            btnSaveRecord.BackgroundImage = originalBackgroundImageRpt;
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
