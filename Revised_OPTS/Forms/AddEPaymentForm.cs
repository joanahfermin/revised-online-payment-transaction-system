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
    public partial class AddEPaymentForm : Form
    {
        Dictionary<string, string> RPT_DG_COLUMNS = new Dictionary<string, string>
        {
            { "TaxDec", "TDN" }, { "TaxPayerName", "TaxPayer's Name" }, { "AmountToPay", "Bill Amount" },
            { "AmountTransferred", "Transferred Amount" }, { "Bank", "Bank" }, { "YearQuarter", "Year" }, { "Quarter", "Quarter" },
            { "BillingSelection", "Billing Selection" }, { "RequestingParty", "Requesting Party" },
        };


        public AddEPaymentForm()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;
        }

        private void AddEPaymentForm_Load(object sender, EventArgs e)
        {

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
