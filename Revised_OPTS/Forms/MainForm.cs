using Revised_OPTS.Forms;
using Revised_OPTS.Model;
using Revised_OPTS.Service;
using System.Text.RegularExpressions;


namespace Revised_OPTS
{
    public partial class MainForm : Form
    {
        IRptService rptService = ServiceFactory.Instance.GetRptService();
        IMiscService miscService = ServiceFactory.Instance.GetMiscService();
        IBusinessService businessService = ServiceFactory.Instance.GetBusinessService();

        private Image originalBackgroundImage;
        public static MainForm Instance;

        Dictionary<string, string> RPT_DG_COLUMNS = new Dictionary<string, string>
        {
            { "TaxDec", "TDN" }, { "TaxPayerName", "TaxPayer's Name" }, { "AmountToPay", "Bill Amount" },
            { "AmountTransferred", "Transferred Amount" }, { "TotalAmountTransferred", "Total Amount Transferred" }, { "ExcessShortAmount", "Excess/Short" },
            { "Bank", "Bank" }, { "YearQuarter", "Year" }, { "Quarter", "Quarter" },
            { "PaymentType", "Payment Type" }, { "BillingSelection", "Billing Selection" }, { "Status", "Status" },
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
            { "Business_Type", "Bus. Type" }, { "MP_Number", "M.P Number" }, { "TaxpayersName", "TaxPayer's Name" },
            { "MiscFees", "Misc. Fees" }, { "BillNumber", "Bill Number" }, { "BillAmount", "Bill Amount" },
            { "BusinessName", "Business Name" }, { "TotalAmount", "Total Amount" }, { "Year", "Year" },
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
            DgMainForm.DefaultCellStyle.ForeColor = Color.Black; // Change font color to blue
            DgMainForm.DefaultCellStyle.Font = new Font("Tahoma", 12, FontStyle.Regular); // Change font size and style
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

        private bool isTDN(string rpt)
        {
            //format of RPT tdn.
            Regex re = new Regex("^[A-Z]{1}-[0-9]{3}-[0-9]{5}$");
            return re.IsMatch(rpt.Trim());
        }

        private bool isBusiness(string bus)
        {
            //format of misc number.
            Regex re = new Regex("^[0-9]{2}-[0-9]{6}$");
            return re.IsMatch(bus.Trim());
        }

        private bool isMiscOccuPermit(string misc)
        {
            //format of misc number.
            Regex re = new Regex("^[M]-[0-9]{4}-[0-9]{2}-[0-9]{2}-[B][P][L][O]-[A-Z,0-9]{4}-[0-9]{6}$");
            return re.IsMatch(misc.Trim());
        }

        private void tbSearch_KeyDown(object sender, KeyEventArgs e)
        {
            Search(tbSearch.Text);
        }

        public void Search(string searchRecordinDB)
        {
            if (isTDN(searchRecordinDB))
            {
                ShowDataInDataGridView<Rpt>(RPT_DG_COLUMNS, rptService.RetrieveBySearchKeyword(searchRecordinDB));
            }
            else if (isMiscOccuPermit(searchRecordinDB))
            {
                ShowDataInDataGridView<Miscellaneous>(MISC_DG_COLUMNS, miscService.RetrieveBySearchKeyword(searchRecordinDB));
            }
            else if (isBusiness(searchRecordinDB))
            {
                ShowDataInDataGridView<Business>(BUSINESS_DG_COLUMNS, businessService.RetrieveBySearchKeyword(searchRecordinDB));
            }
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

            DgMainForm.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 12, FontStyle.Regular);
            DgMainForm.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
            DgMainForm.ColumnHeadersDefaultCellStyle.BackColor = Color.DarkSalmon;
            DgMainForm.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.DarkSalmon;
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
            originalBackgroundImage = btnAddNewRecord.BackgroundImage;
            btnAddNewRecord.BackgroundImage = null;

            Color customColor = Color.FromArgb(23, 45, 74);
            btnAddNewRecord.BackColor = customColor;
        }

        private void btnAddNewRecord_MouseLeave(object sender, EventArgs e)
        {
            btnAddNewRecord.BackgroundImage = originalBackgroundImage;
        }

        private void btnAddNewRecord_Click(object sender, EventArgs e)
        {
            new AddNewRecordForm().Show();
        }

        private void MainForm_SizeChanged(object sender, EventArgs e)
        {
            DgMainForm.Size = new Size(this.ClientSize.Width - 50, this.ClientSize.Height - 170);
        }
    }
}