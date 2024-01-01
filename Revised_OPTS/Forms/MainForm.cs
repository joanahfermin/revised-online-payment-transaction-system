using Inventory_System.Forms;
using Inventory_System.Utilities;
using Revised_OPTS.Forms;
using Revised_OPTS.Model;
using Revised_OPTS.Service;
using Revised_OPTS.Utilities;
using System.Globalization;
using System.Text.RegularExpressions;


namespace Revised_OPTS
{
    public partial class MainForm : Form
    {
        IRptService rptService = ServiceFactory.Instance.GetRptService();
        IMiscService miscService = ServiceFactory.Instance.GetMiscService();
        IBusinessService businessService = ServiceFactory.Instance.GetBusinessService();

        private Image originalBackgroundImageNonRpt;
        private Image originalBackgroundImageRpt;

        public static MainForm Instance;

        string selectedRecordFormat = "";
        long selectedRecordId = 0;

        Dictionary<string, string> RPT_DG_COLUMNS = new Dictionary<string, string>
        {
            { "TaxDec", "TDN" }, { "TaxPayerName", "TaxPayer's Name" }, { "AmountToPay", "Bill Amount" },
            { "AmountTransferred", "Transferred Amount" }, { "TotalAmountTransferred", "Total Amount Transferred" }, { "ExcessShortAmount", "Excess/Short" },
            { "Bank", "Bank" }, { "YearQuarter", "Year" }, { "Quarter", "Quarter" },
            /*{ "PaymentType", "Payment Type" }, { "BillingSelection", "Billing Selection" },*/ { "Status", "Status" },
            { "RequestingParty", "Email Address" }, { "EncodedBy", "Encoded By" }, { "EncodedDate", "Encoded Date" },
                    { "RefNum", "Reference No." }, { "RPTremarks", "Remarks" },

            { "VerifiedBy", "VerifiedBy" }, { "PaymentDate", "Payment Date" }, { "VerifiedDate", "Verified Date" },
            { "ValidatedBy", "Validated By" }, { "ValidatedDate", "Validated Date" }, { "UploadedBy", "Uploaded By" },
            { "UploadedDate", "Uploaded Date" }, { "ReleasedBy", "Released By" }, { "ReleasedDate", "Released Date" },
            { "LocCode", "Location Code" }, { "RepName", "Representative Name" }, { "ContactNumber", "Contact Number" },
                    { "ORConfirmDate", "Receipt Confirm Date" }, { "ORAttachedDate", "Receipt Attachment Date" },
        };

        Dictionary<string, string> BUSINESS_DG_COLUMNS = new Dictionary<string, string>
        {
            { "Business_Type", "Bus. Type" }, { "MP_Number", "M.P Number" }, { "BillNumber", "Bill Number" }, 
            { "TaxpayersName", "TaxPayer's Name" }, { "BusinessName", "Business Name" }, { "BillAmount", "Bill Amount" },
            { "TotalAmount", "Transferred Amount" }, { "MiscFees", "Misc. Fees" }, { "Year", "Year" },
            { "Qtrs", "Quarter" }, { "Status", "Status" }, { "PaymentChannel", "Bank" },
            { "VerifiedBy", "Verified By" }, { "VerifiedDate", "Verified Date" }, { "DateOfPayment", "Payment Date" },
            { "ValidatedBy", "Validated By" }, { "ValidatedDate", "Validated Date" }, { "RequestingParty", "Email Address" },
            { "EncodedDate", "Encoded Date" }, { "BussinessRemarks", "Remarks" }, { "EncodedBy", "Encoded By" },
            { "ContactNumber", "Contact Number" }, { "UploadedBy", "Uploaded By" }, { "UploadedDate", "Uploaded Date" },
                     { "ReleasedBy", "Released By" }, { "ReleasedDate", "Released Date" },
        };

        Dictionary<string, string> MISC_DG_COLUMNS = new Dictionary<string, string>
        {
            { "MiscType", "Misc Type" }, { "TaxpayersName", "TaxPayer's Name" }, { "OrderOfPaymentNum", "O.P Number" }, { "OPATrackingNum", "OPA Tracking No." },
            { "AmountToBePaid", "Bill Amount" }, { "TransferredAmount", "Transferred Amount" }, { "ExcessShort", "Excess/Short" }, { "Status", "Status" }, 
            { "PaymentDate", "Bank" }, { "VerifiedBy", "Verified By" }, { "VerifiedDate", "Verified Date" }, { "ValidatedBy", "Validated By" }, 
            { "ValidatedDate", "Validated Date" }, { "RequestingParty", "Email Address" }, { "RefNum", "Reference No." },
            { "EncodedDate", "Encoded Date" }, { "EncodedBy", "Encoded By" }, { "Remarks", "Remarks" }, { "ReleasedBy", "Released By" }, 
            { "ReleasedDate", "Released Date" }, { "RepName", "Representative Name" }, { "ContactNumber", "Contact Number" },
        };

        public MainForm()
        {
            InitializeComponent();
            DataGridUI();
            Instance = this;
            DgMainForm.CellFormatting += DgMainForm_CellFormatting;

            ContextMenuStrip contextMenuStrip1 = new ContextMenuStrip();

            ToolStripMenuItem menuItemEdit = new ToolStripMenuItem("Edit");
            menuItemEdit.Click += MenuItemEdit_Click;
            ToolStripMenuItem menuItemVerifiedPayment = new ToolStripMenuItem("Payment Verified");
            menuItemVerifiedPayment.Click += MenuItemVerifiedPayment_Click;

            contextMenuStrip1.Items.Add(menuItemEdit);
            contextMenuStrip1.Items.Add(menuItemVerifiedPayment);
            DgMainForm.ContextMenuStrip = contextMenuStrip1;
        }

        private void MenuItemVerifiedPayment_Click(object? sender, EventArgs e)
        {
            DataGridViewSelectedRowCollection selectedRows = DgMainForm.SelectedRows;

            List<Rpt> rptList = new List<Rpt>();
            foreach (DataGridViewRow row in selectedRows)
            {
                //conversion of row to Rpt
                Rpt selectedRptRecord = row.DataBoundItem as Rpt;

                if (selectedRptRecord.Status == TaxStatus.ForPaymentVerification)
                {
                    rptList.Add(selectedRptRecord);
                }
                // TO DO: LAGAY SA TRANSACTION
            }
            rptService.UpdateSelectedRecordsStatus(rptList);
            DgMainForm.Refresh();
        }

        private void MenuItemEdit_Click(object? sender, EventArgs e)
        {
            DataGridViewRow selectedRow = DgMainForm.CurrentRow;
            if (selectedRow != null)
            {
                Rpt selectedRptRecord = selectedRow.DataBoundItem as Rpt;
                if (selectedRptRecord != null)
                {
                    new RPTAddUpdateRecordForm(selectedRptRecord.RefNum, selectedRptRecord.RequestingParty).Show();
                }
            }
        }

        private void DgMainForm_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.Value is decimal decimalValue)
            {
                e.Value = decimalValue.ToString("N2"); 
                e.FormattingApplied = true;
            }
        }

        public void InitializeData()
        {
            List<Rpt> dataList = rptService.GetAll();
            DgMainForm.AutoGenerateColumns = false;

            //DgMainForm.Columns.Add("TaxDec", "TDN");
            //DgMainForm.Columns.Add("TaxPayerName", "Taxpayer's Name");
            //DgMainForm.Columns.Add("AmountToPay", "Bill Amount");

            //DgMainForm.Columns["TaxDec"].DataPropertyName = "TaxDec";
            //DgMainForm.Columns["TaxPayerName"].DataPropertyName = "TaxPayerName";
            //DgMainForm.Columns["AmountToPay"].DataPropertyName = "AmountToPay";

            foreach (var kvp in RPT_DG_COLUMNS)
            {
                DgMainForm.Columns.Add(kvp.Key, kvp.Value);
                DgMainForm.Columns[kvp.Key].DataPropertyName = kvp.Key;
            }
            DgMainForm.DataSource = dataList;
        }

        public void DataGridUI()
        {
            DgMainForm.DefaultCellStyle.Font = new Font("Tahoma", 12, FontStyle.Regular);
            DgMainForm.BackgroundColor = Color.AliceBlue;
            this.WindowState = FormWindowState.Maximized;
            DgMainForm.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCells;
            DgMainForm.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

            DgMainForm.DefaultCellStyle.BackColor = Color.AliceBlue;

            DgMainForm.ColumnHeadersDefaultCellStyle.ForeColor = Color.MidnightBlue;
            DgMainForm.GridColor = Color.DarkGray;
            DgMainForm.CellBorderStyle = DataGridViewCellBorderStyle.Single;
            DgMainForm.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Raised;

            DgMainForm.DefaultCellStyle.SelectionBackColor = Color.AliceBlue;
        }

        private void tbSearch_KeyDown(object sender, KeyEventArgs e)
        {
            Search(tbSearch.Text);
        }

        public void Search(string searchRecordinDB)
        {
            if (SearchBusinessFormat.isTDN(searchRecordinDB))
            {
                ShowDataInDataGridView<Rpt>(RPT_DG_COLUMNS, rptService.RetrieveBySearchKeyword(searchRecordinDB));
            }
            //else if (SearchBusinessFormat.isMiscOccuPermit(searchRecordinDB) || SearchBusinessFormat.isMiscOvrTtmd(searchRecordinDB)
            //    || SearchBusinessFormat.isMiscOvrDpos(searchRecordinDB) || SearchBusinessFormat.isMiscMarket(searchRecordinDB)
            //    || SearchBusinessFormat.isMiscZoning(searchRecordinDB) || SearchBusinessFormat.isMiscLiquor(searchRecordinDB))
            //{
            //    ShowDataInDataGridView<Miscellaneous>(MISC_DG_COLUMNS, miscService.RetrieveBySearchKeyword(searchRecordinDB));
            //}
            //else if (SearchBusinessFormat.isBusiness(searchRecordinDB))
            //{
            //    ShowDataInDataGridView<Business>(BUSINESS_DG_COLUMNS, businessService.RetrieveBySearchKeyword(searchRecordinDB));
            //}
        }

        private void ShowDataInDataGridView<T>(Dictionary<string, string> columnMappings, List<T> dataList)
        {
            DgMainForm.Columns.Clear();
            DgMainForm.AutoGenerateColumns = false;

            foreach (var kvp in columnMappings)
            {
                DgMainForm.Columns.Add(kvp.Key, kvp.Value);
                DgMainForm.Columns[kvp.Key].DataPropertyName = kvp.Key;
            }
            DgMainForm.DataSource = dataList;

            if (dataList.Count == 0)
            {
                MessageBox.Show("No data found.");
            }

            DgMainForm.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 12, FontStyle.Regular);
            DgMainForm.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
            DgMainForm.ColumnHeadersDefaultCellStyle.BackColor = Color.DarkSalmon;
            DgMainForm.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.DarkSalmon;
        }

        private void DgMainForm_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow selectedRow = DgMainForm.Rows[e.RowIndex];
                if (selectedRow.DataBoundItem is Rpt selectedRecord)
                {
                    Rpt tdnRecord = selectedRow.DataBoundItem as Rpt;
                    selectedRecordFormat = tdnRecord.TaxDec;
                    selectedRecordId = tdnRecord.RptID;
                }
                else if (selectedRow.DataBoundItem is Business businessSelectedRecord)
                {
                    Business businessRecord = selectedRow.DataBoundItem as Business;
                    selectedRecordFormat = businessRecord.MP_Number;
                    selectedRecordId = businessRecord.BusinessID;

                }
                else if (selectedRow.DataBoundItem is Miscellaneous miscSelectedRecord)
                {
                    Miscellaneous miscRecord = selectedRow.DataBoundItem as Miscellaneous;
                    selectedRecordFormat = miscRecord.OrderOfPaymentNum;
                    selectedRecordId = miscRecord.MiscID;
                }
            }
        }

        private void DgMainForm_DoubleClick(object sender, EventArgs e)
        {
            bool isRptTDNFormatCorrect = SearchBusinessFormat.isTDN(selectedRecordFormat);
            bool isBusinessMpNumFormatCorrect = SearchBusinessFormat.isBusiness(selectedRecordFormat);
 
            if (isRptTDNFormatCorrect)
            {
                Rpt retrieveRptRecord = rptService.Get(selectedRecordId);
                string taxType = TaxTypeUtil.REALPROPERTYTAX;
                NonRPTAddUpdateRecordForm updateRecord = new NonRPTAddUpdateRecordForm(retrieveRptRecord.RptID, taxType);
                updateRecord.ShowDialog();
            }
            else if (isBusinessMpNumFormatCorrect)
            {
                Business retrieveBusinessRecord = businessService.Get(selectedRecordId);
                string taxType = TaxTypeUtil.BUSINESS;
                NonRPTAddUpdateRecordForm updateRecord = new NonRPTAddUpdateRecordForm(retrieveBusinessRecord.BusinessID, taxType);
                updateRecord.ShowDialog();
            }
            else
            {
                string taxType = SearchBusinessFormat.GetTaxTypeFromFormat(selectedRecordFormat);
                if (taxType != null)
                {
                    Miscellaneous retrieveMiscRecord = miscService.Get(selectedRecordId);
                    NonRPTAddUpdateRecordForm updateRecord = new NonRPTAddUpdateRecordForm(retrieveMiscRecord.MiscID, taxType);
                    updateRecord.ShowDialog();
                }
            }
        }

        private void DgMainForm_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                MainDGRightClick.Show(DgMainForm, e.Location);
            }
        }

        private void btnAddNewRecord_MouseEnter(object sender, EventArgs e)
        {
            originalBackgroundImageNonRpt = btnNonRptAddNewRecord.BackgroundImage;
            btnNonRptAddNewRecord.BackgroundImage = null;

            Color customColor = Color.FromArgb(23, 45, 74);
            btnNonRptAddNewRecord.BackColor = customColor;
        }

        private void btnAddNewRecord_MouseLeave(object sender, EventArgs e)
        {
            btnNonRptAddNewRecord.BackgroundImage = originalBackgroundImageNonRpt;
        }

        private void btnAddNewRecord_Click(object sender, EventArgs e)
        {
            new NonRPTAddUpdateRecordForm().Show();
        }

        private void MainForm_SizeChanged(object sender, EventArgs e)
        {
            DgMainForm.Size = new Size(this.ClientSize.Width - 50, this.ClientSize.Height - 170);
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (LoginForm.INSTANCE != null)
            {
                LoginForm.INSTANCE.Close();
            }
        }

        private void btnAddRptRecord_Click(object sender, EventArgs e)
        {
            new RPTAddUpdateRecordForm().Show();
        }

        private void btnAddRptRecord_MouseEnter(object sender, EventArgs e)
        {
            originalBackgroundImageRpt = btnAddRptRecord.BackgroundImage;
            btnAddRptRecord.BackgroundImage = null;

            Color customColor = Color.FromArgb(23, 45, 74);
            btnAddRptRecord.BackColor = customColor;
        }

        private void btnAddRptRecord_MouseLeave(object sender, EventArgs e)
        {
            btnAddRptRecord.BackgroundImage = originalBackgroundImageRpt;
        }
    }
}