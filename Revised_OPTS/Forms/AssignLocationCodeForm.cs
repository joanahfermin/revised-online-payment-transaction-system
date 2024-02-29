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

        Dictionary<string, string> RPT_DG_COLUMNS = new Dictionary<string, string>
        {
            { "TaxDec", "TDN" }, { "TaxPayerName", "TaxPayer's Name" }, { "AmountToPay", "Bill Amount" },
            { "AmountTransferred", "Transferred Amount" }, { "TotalAmountTransferred", "Total Amount Transferred" }, { "ExcessShortAmount", "Excess/Short" },
            { "Bank", "Bank" }, { "YearQuarter", "Year" }, { "Quarter", "Quarter" },
            /*{ "PaymentType", "Payment Type" },*/ { "BillingSelection", "Billing Selection" }, { "Status", "Status" }, { "RefNum", "Reference No." },
            { "RequestingParty", "Email Address" }, { "EncodedBy", "Encoded By" }, { "EncodedDate", "Encoded Date" }, { "RPTremarks", "Remarks" },

            { "VerifiedBy", "VerifiedBy" }, { "PaymentDate", "Payment Date" }, { "VerifiedDate", "Verified Date" },
            { "ValidatedBy", "Validated By" }, { "ValidatedDate", "Validated Date" }, { "UploadedBy", "Uploaded By" },
            { "UploadedDate", "Uploaded Date" }, { "ReleasedBy", "Released By" }, { "ReleasedDate", "Released Date" },
            { "LocCode", "Location Code" }, { "RepName", "Representative Name" }, { "ContactNumber", "Contact Number" },
                    { "ORConfirmDate", "Receipt Confirm Date" }, { "ORAttachedDate", "Receipt Attachment Date" },
        };

        public AssignLocationCodeForm()
        {
            InitializeComponent();
            RetrieveAndShowRptData();
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
        private void btnAssign_Click(object sender, EventArgs e)
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
                List<long> rptIDList = DgRpt.Rows.Cast<DataGridViewRow>()
                                   .Where(row => row.DataBoundItem != null && row.DataBoundItem is Rpt)
                                   .Select(row => ((Rpt)row.DataBoundItem).RptID)
                                   .ToList();
                rptService.AssignmentLocationCode(rptIDList, locationCode);
                RetrieveAndShowRptData();
                MessageBox.Show("Done");
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            RetrieveAndShowRptData();
        }

        private void RetrieveAndShowRptData()
        {
            List<Rpt> rptList = rptService.ListForLocationCodeAssignment();
            ShowDataInDataGridView(RPT_DG_COLUMNS, rptList);
        }
    }
}
