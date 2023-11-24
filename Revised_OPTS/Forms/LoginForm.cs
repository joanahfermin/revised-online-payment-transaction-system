using Inventory_System.Exception;
using Inventory_System.Service;
using Revised_OPTS;
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
    public partial class LoginForm : Form
    {
        ISecurityService securityService = ServiceFactory.Instance.GetSecurityService();

        public static LoginForm INSTANCE;

        public LoginForm()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
            INSTANCE = this;
        }

        private void btnSaveRecord_Click(object sender, EventArgs e)
        {
            string userName = tbUsername.Text.Trim();
            string passWord = tbPassword.Text.Trim();

            try
            {
                securityService.login(userName, passWord);
            }
            catch (RptException ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }

            MainForm mainForm = new MainForm();
            mainForm.Show();
            this.Hide();
        }
    }
}
