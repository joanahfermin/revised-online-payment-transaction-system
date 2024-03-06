using Inventory_System.Utilities;
using Revised_OPTS;
using Revised_OPTS.Model;
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
    public partial class AssignLocationCodeForm : Form
    {
        IRptService rptService = ServiceFactory.Instance.GetRptService();
        private Image originalBackgroundImageNonRpt;
        private Image originalBackgroundImageRpt;

        Dictionary<string, string> RPT_DG_COLUMNS = new Dictionary<string, string>
        {
            { "Bank", "Bank" }, { "UploadedBy", "Uploaded By" },
            { "UploadedDate", "Uploaded Date" }, { "LocCode", "Location Code" }, { "TaxDec", "TDN" }, { "TaxPayerName", "TaxPayer's Name" }, { "AmountToPay", "Bill Amount" },
            { "AmountTransferred", "Transferred Amount" }, { "TotalAmountTransferred", "Total Amount Transferred" }, { "ExcessShortAmount", "Excess/Short" },
            { "YearQuarter", "Year" }, { "Quarter", "Quarter" },
            { "BillingSelection", "Billing Selection" }, { "Status", "Status" }, { "RefNum", "Reference No." },
            { "RequestingParty", "Email Address" }, { "EncodedBy", "Encoded By" }, { "EncodedDate", "Encoded Date" }, { "RPTremarks", "Remarks" },

            { "VerifiedBy", "VerifiedBy" }, { "PaymentDate", "Payment Date" }, { "VerifiedDate", "Verified Date" },
            { "ValidatedBy", "Validated By" }, { "ValidatedDate", "Validated Date" },
        };

        public AssignLocationCodeForm()
        {
            InitializeComponent();
            RetrieveAndShowRptData();

            DgRpt.CellFormatting += DgRpt_CellFormatting;
        }

        private void ShowDataInDataGridView(Dictionary<string, string> columnMappings, List<Rpt> rptList)
        {
            DgRpt.Columns.Clear();
            DgRpt.AutoGenerateColumns = false;

            foreach (var kvp in columnMappings)
            {
                DgRpt.Columns.Add(kvp.Key, kvp.Value);
                DgRpt.Columns[kvp.Key].DataPropertyName = kvp.Key;
            }
            DgRpt.DataSource = rptList;
            //DgRpt.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 12, FontStyle.Regular);
            //DgRpt.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
            //DgRpt.ColumnHeadersDefaultCellStyle.BackColor = Color.DarkSalmon;
            //DgRpt.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.DarkSalmon;
        }

        private void DgRpt_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.Value is decimal decimalValue)
            {
                e.Value = decimalValue.ToString("N2");
                e.FormattingApplied = true;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (DgRpt.Rows.Count == 0)
            {
                MessageBox.Show("No records found.");
                return;
            }
            if (tbLocationCode.Text.Trim().Length == 0)
            {
                MessageBox.Show("Please Enter Location Code");
                return;
            }
            if (DialogResult.Yes == MessageBox.Show($"Are you sure? ", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
            {
                String locationCode = tbLocationCode.Text.Trim();
                List<long> rptIDList = DgRpt.SelectedRows.Cast<DataGridViewRow>()
                                   .Where(row => row.DataBoundItem != null && row.DataBoundItem is Rpt)
                                   .Select(row => ((Rpt)row.DataBoundItem).RptID)
                                   .ToList();
                rptService.AssignmentLocationCode(rptIDList, locationCode);
                RetrieveAndShowRptData();
                btnRefresh_Click_1(sender, e);


                //Rpt rpt = rptService.Get(rptIDList.First());
                //string firstTaxdecRecord = rpt.TaxDec;
                //NotificationHelper.notifyUserAndRefreshRecord(firstTaxdecRecord);

                //btnClose_Click(sender, e);
            }
        }

        private void RetrieveAndShowRptData()
        {
            List<Rpt> rptList = rptService.ListForLocationCodeAssignment(tbSearchLocationCode.Text);
            ShowDataInDataGridView(RPT_DG_COLUMNS, rptList);
        }

        private void btnRefresh_Click_1(object sender, EventArgs e)
        {
            RetrieveAndShowRptData();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnRefresh_MouseEnter(object sender, EventArgs e)
        {
            originalBackgroundImageNonRpt = btnSearch.BackgroundImage;
            btnSearch.BackgroundImage = null;

            Color customColor = Color.FromArgb(23, 45, 74);
            btnSearch.BackColor = customColor;
        }

        private void btnRefresh_MouseLeave(object sender, EventArgs e)
        {
            btnSearch.BackgroundImage = originalBackgroundImageNonRpt;
        }

        private void btnSave_MouseEnter(object sender, EventArgs e)
        {
            originalBackgroundImageNonRpt = btnSave.BackgroundImage;
            btnSave.BackgroundImage = null;

            Color customColor = Color.FromArgb(23, 45, 74);
            btnSave.BackColor = customColor;
        }

        private void btnSave_MouseLeave(object sender, EventArgs e)
        {
            btnSave.BackgroundImage = originalBackgroundImageNonRpt;
        }

        private void btnClose_MouseEnter(object sender, EventArgs e)
        {
            originalBackgroundImageNonRpt = btnClose.BackgroundImage;
            btnClose.BackgroundImage = null;

            Color customColor = Color.FromArgb(23, 45, 74);
            btnClose.BackColor = customColor;
        }

        private void btnClose_MouseLeave(object sender, EventArgs e)
        {
            btnClose.BackgroundImage = originalBackgroundImageNonRpt;
        }
    }
}
