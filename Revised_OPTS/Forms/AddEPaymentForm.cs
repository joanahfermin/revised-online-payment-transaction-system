using Inventory_System.Model;
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
        List<ElectronicPayment> data = new List<ElectronicPayment>();

        public AddEPaymentForm()
        {
            InitializeComponent();
            //dgRptList.DataSource = data;

            this.WindowState = FormWindowState.Maximized;

            dgRptList.KeyUp += dgRptList_KeyDown;
        }

        private void dgRptList_KeyDown(object? sender, KeyEventArgs e)
        {
            PasteDataFromClipboard();
        }

        private void PasteDataFromClipboard()
        {
            string dataAsString = Clipboard.GetText();

            List<int> IgnoreColumnList = new List<int>();
            IgnoreColumnList.Add(0);
            IgnoreColumnList.Add(1);
            IgnoreColumnList.Add(2);
            IgnoreColumnList.Add(8);
            IgnoreColumnList.Add(10);
            IgnoreColumnList.Add(11);

            //For every record copied, .split splits strings into several lines. r and n = new line, RemoveEmptyEnries, kapag walang laman ang row, iignore lang nya.
            string[] rowArray = dataAsString.Split(new Char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

            if (rowArray.Length > 0)
            {
                dgRptList.ClearSelection();

                foreach (string row in rowArray)
                {
                    //Splits the records through TAB. \t = tab.
                    string[] columnArray = row.Split(new char[] { '\t' });

                    if (columnArray.Length > 0)
                    {
                        ElectronicPayment ep = new ElectronicPayment();
                        ep.EpaymentRef = columnArray[0];
                        ep.EpaymentTransactionRef = columnArray[1];
                        ep.BillerRef = columnArray[2];
                        ep.ServiceProvider = columnArray[3];
                        ep.BillerId = columnArray[4];
                        ep.BillerInfo1 = columnArray[5];
                        ep.BillerInfo2 = columnArray[6];
                        ep.BillerInfo3 = columnArray[7];
                        ep.PaymentRef = columnArray[8];
                        ep.AmountDue = Convert.ToDecimal(columnArray[9]);
                        ep.AmountWithTransactionFee = Convert.ToDecimal(columnArray[10]);
                        if (ep.Subtotal == null)
                        {
                            ep.Subtotal = null;
                        }
                        else
                        {
                            ep.Subtotal = Convert.ToDecimal(columnArray[11]);
                        }
                        //ep.Subtotal = Convert.ToDecimal(columnArray[11]);
                        if (DateTime.TryParse(columnArray[12], out DateTime date))
                        {
                            ep.Date = date;
                        }
                        data.Add(ep);
                    }
                }
                dgRptList.DataSource = data;
            }
        }

        private void AddEPaymentForm_Load(object sender, EventArgs e)
        {

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSaveAll_Click(object sender, EventArgs e)
        {

        }
    }
}
