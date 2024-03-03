using Inventory_System.DAL;
using Inventory_System.Exception;
using Inventory_System.Model;
using Inventory_System.Utilities;
using Revised_OPTS.DAL;
using Revised_OPTS.Model;
using Revised_OPTS.Service;
using Revised_OPTS.Utilities;
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
    public partial class ExistingRecordForm : Form
    {
        IRptService rptService = ServiceFactory.Instance.GetRptService();
        IRPTAttachPictureRepository rptAttachPictureRepository = RepositoryFactory.Instance.GetRPTAttachPictureRepository();

        private DynamicGridContainer<Rpt> RptDynamicGridContainer;
        private DynamicGridContainer<Business> BusinessDynamicGridContainer;
        private DynamicGridContainer<Miscellaneous> MiscDynamicGridContainer;

        private Image originalBackgroundImage;

        private long RptID = 0;

        public ExistingRecordForm(List<Rpt> existingRecordList, List<Business> existingBusRecordList, List<Miscellaneous> existingMiscRecordList)
        {
            InitializeComponent();
            InitializeRptDataGridView();
            InitializeBusinessDataGridView();
            InitializeMiscDataGridView();

            DgRptAddUpdateForm.DefaultCellStyle.Font = new Font("Tahoma", 12, FontStyle.Regular);
            DgBusAddUpdateForm.DefaultCellStyle.Font = new Font("Tahoma", 12, FontStyle.Regular);
            DgMiscAddUpdateForm.DefaultCellStyle.Font = new Font("Tahoma", 12, FontStyle.Regular);

            this.WindowState = FormWindowState.Maximized;

            RptDynamicGridContainer.PopulateData(existingRecordList);
            BusinessDynamicGridContainer.PopulateData(existingBusRecordList);
            MiscDynamicGridContainer.PopulateData(existingMiscRecordList);

            DgRptAddUpdateForm.SelectionChanged += DgRptAddUpdateForm_SelectionChanged;
        }

        private void DgRptAddUpdateForm_SelectionChanged(object? sender, EventArgs e)
        {
            DataGridViewRow selectedRow = DgRptAddUpdateForm.CurrentRow;

            if (selectedRow != null)
            {
                Rpt selectedRpt = (Rpt)selectedRow.DataBoundItem;
                if (selectedRpt != null)
                {
                    RptID = selectedRpt.RptID;
                    loadRptReceipt(RptID);
                }
            }
        }

        public void loadRptReceipt(long rptId)
        {
            if (pbReceipt.Image != null)
            {
                pbReceipt.Image.Dispose();
            }
            pbReceipt.Image = null;

            RPTAttachPicture pix = rptService.getRptReceipt(rptId);
            if (pix != null)
            {
                pbReceipt.Image = Image.FromStream(new MemoryStream(pix.FileData));
                pbReceipt.SizeMode = PictureBoxSizeMode.StretchImage;
            }
        }

        private void InitializeRptDataGridView()
        {
            List<Bank> bankList = rptService.GetAllBanks();
            string[] bankNames = bankList.Select(bank => bank.BankName).ToList().ToArray();

            DynamicGridInfo[] gridInfoArray = new DynamicGridInfo[] {
                new DynamicGridInfo{PropertyName="RptID", Label = "Rpt ID", isReadOnly = true },
                new DynamicGridInfo{PropertyName="TaxDec", Label = "TDN", isRequired=true, isReadOnly = true  },
                new DynamicGridInfo{PropertyName="TaxPayerName", Label = "TaxPayer's Name", isRequired=true, isReadOnly = true  },
                new DynamicGridInfo{PropertyName="AmountToPay", Label = "Bill Amount", isRequired=true, isReadOnly = true  },
                new DynamicGridInfo{PropertyName="TotalAmountTransferred", Label = "Total Amount Transferred", isRequired=true, isReadOnly = true  },
                new DynamicGridInfo{PropertyName="Bank", Label = "Bank", GridType = DynamicGridType.ComboBox, ComboboxChoices = bankNames, isRequired=true, isReadOnly = true  },
                new DynamicGridInfo{PropertyName="PaymentDate", Label = "Payment Date", GridType = DynamicGridType.DatetimePicker, isRequired=true, isReadOnly = true  },
                new DynamicGridInfo{PropertyName="YearQuarter", Label = "Year", decimalValue = true, isRequired=true},
                new DynamicGridInfo{PropertyName="Quarter", Label = "Quarter", GridType=DynamicGridType.ComboBox, ComboboxChoices = Quarter.ALL_QUARTER, isRequired=true },
                new DynamicGridInfo{PropertyName="Status", Label = "Status", GridType=DynamicGridType.ComboBox, ComboboxChoices = TaxStatus.STATUS, isReadOnly = true },
                new DynamicGridInfo{PropertyName="BillingSelection", Label = "Billing Selection", GridType=DynamicGridType.ComboBox, ComboboxChoices = BillingSelectionUtil.ALL_BILLING_SELECTION, isRequired=true },
                new DynamicGridInfo{PropertyName="EncodedBy", Label = "Encoded By", isReadOnly = true },
                new DynamicGridInfo{PropertyName="EncodedDate", Label = "Encoded Date", GridType = DynamicGridType.DatetimePicker, isRequired=true, isReadOnly = true  },
                new DynamicGridInfo{PropertyName="RequestingParty", Label = "Email Address", isReadOnly = true },
                new DynamicGridInfo{PropertyName="RPTremarks", Label = "Remarks", isReadOnly = true },
            };
            RptDynamicGridContainer = new DynamicGridContainer<Rpt>(DgRptAddUpdateForm, gridInfoArray, true, true);
        }

        private void InitializeBusinessDataGridView()
        {
            List<Bank> bankList = rptService.GetAllBanks();
            string[] bankNames = bankList.Select(bank => bank.BankName).ToList().ToArray();

            DynamicGridInfo[] gridInfoArray = new DynamicGridInfo[] {
                new DynamicGridInfo{PropertyName="BusinessID", Label = "Bus. ID", isReadOnly = true },
                new DynamicGridInfo{PropertyName="Business_Type", Label = "Business Type", isRequired=true },
                new DynamicGridInfo{PropertyName="MP_Number", Label = "MP Number", isRequired=true },
                new DynamicGridInfo{PropertyName="TaxpayersName", Label = "TaxPayer's Name", isRequired=true },
                new DynamicGridInfo{PropertyName="BusinessName", Label = "Business Name", isRequired=true },
                new DynamicGridInfo{PropertyName="BillNumber", Label = "Bill Number", isRequired=true },
                new DynamicGridInfo{PropertyName="AmountToPay", Label = "Bill Amount", isRequired=true },
                new DynamicGridInfo{PropertyName="MiscFees", Label = "Misc. Fees", isRequired=true },
                new DynamicGridInfo{PropertyName="TotalAmount", Label = "Total Amount Transferred", isRequired=true },
                new DynamicGridInfo{PropertyName="PaymentChannel", Label = "Bank", GridType = DynamicGridType.ComboBox, ComboboxChoices = bankNames, isRequired=true },
                new DynamicGridInfo{PropertyName="DateOfPayment", Label = "Payment Date", GridType = DynamicGridType.DatetimePicker, isRequired=true },
                new DynamicGridInfo{PropertyName="Year", Label = "Year", decimalValue = true},
                new DynamicGridInfo{PropertyName="Qtrs", Label = "Quarter", GridType=DynamicGridType.ComboBox, ComboboxChoices = Quarter.ALL_QUARTER, isRequired=false },
                new DynamicGridInfo{PropertyName="Status", Label = "Status", GridType=DynamicGridType.ComboBox, ComboboxChoices = TaxStatus.STATUS, isReadOnly = true },
                new DynamicGridInfo{PropertyName="EncodedBy", Label = "Encoded By", isReadOnly = true },
                new DynamicGridInfo{PropertyName="EncodedDate", Label = "Encoded Date", GridType = DynamicGridType.DatetimePicker, isRequired=true, isReadOnly = true  },
                new DynamicGridInfo{PropertyName="RequestingParty", Label = "Email Address" },
                new DynamicGridInfo{PropertyName="BussinessRemarks", Label = "Remarks"},
            };
            BusinessDynamicGridContainer = new DynamicGridContainer<Business>(DgBusAddUpdateForm, gridInfoArray, true, true);
        }

        private void InitializeMiscDataGridView()
        {
            List<Bank> bankList = rptService.GetAllBanks();
            string[] bankNames = bankList.Select(bank => bank.BankName).ToList().ToArray();

            DynamicGridInfo[] gridInfoArray = new DynamicGridInfo[] {
                new DynamicGridInfo{PropertyName="MiscID", Label = "Misc. ID", isReadOnly = true },
                new DynamicGridInfo{PropertyName="MiscType", Label = "Misc. Type", isRequired=true },
                new DynamicGridInfo{PropertyName="TaxpayersName", Label = "TaxPayer's Name", isRequired=true },
                new DynamicGridInfo{PropertyName="OrderOfPaymentNum", Label = "Bill Number", isRequired=true },
                new DynamicGridInfo{PropertyName="OPATrackingNum", Label = "OPA Tracking No.", isRequired=true },
                new DynamicGridInfo{PropertyName="AmountToBePaid", Label = "Bill Amount", isRequired=true },
                new DynamicGridInfo{PropertyName="TransferredAmount", Label = "Total Amount Transferred", isRequired=true },
                new DynamicGridInfo{PropertyName="ModeOfPayment", Label = "Bank", GridType = DynamicGridType.ComboBox, ComboboxChoices = bankNames, isRequired=true },
                new DynamicGridInfo{PropertyName="PaymentDate", Label = "Payment Date", GridType = DynamicGridType.DatetimePicker, isRequired=true },
                new DynamicGridInfo{PropertyName="Status", Label = "Status", GridType=DynamicGridType.ComboBox, ComboboxChoices = TaxStatus.STATUS, isReadOnly = true },
                new DynamicGridInfo{PropertyName="RefNum", Label = "Reference Number" },
                new DynamicGridInfo{PropertyName="RequestingParty", Label = "Email Address" },
                new DynamicGridInfo{PropertyName="EncodedBy", Label = "Encoded By", isReadOnly = true },
                new DynamicGridInfo{PropertyName="EncodedDate", Label = "Encoded Date", GridType = DynamicGridType.DatetimePicker, isRequired=true, isReadOnly = true  },
                new DynamicGridInfo{PropertyName="Remarks", Label = "Remarks" },
            };
            MiscDynamicGridContainer = new DynamicGridContainer<Miscellaneous>(DgMiscAddUpdateForm, gridInfoArray, true, true);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSaveRecord_Click(object sender, EventArgs e)
        {
            List<Rpt> listOfRptsToSave = RptDynamicGridContainer.GetData();

            try
            {
                rptService.UpdateAllinDuplicateRecordForm(listOfRptsToSave);
            }
            catch (RptException ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            //TO DO: BUSINESS AND MISC
        }

        private void btnSaveRecord_MouseEnter(object sender, EventArgs e)
        {
            originalBackgroundImage = btnSaveRecord.BackgroundImage;
            btnSaveRecord.BackgroundImage = null;

            Color customColor = Color.FromArgb(23, 45, 74);
            btnSaveRecord.BackColor = customColor;
        }

        private void btnSaveRecord_MouseLeave(object sender, EventArgs e)
        {
            btnSaveRecord.BackgroundImage = originalBackgroundImage;
        }

        private void btnClose_MouseEnter(object sender, EventArgs e)
        {
            originalBackgroundImage = btnClose.BackgroundImage;
            btnClose.BackgroundImage = null;

            Color customColor = Color.FromArgb(23, 45, 74);
            btnClose.BackColor = customColor;
        }

        private void btnClose_MouseLeave(object sender, EventArgs e)
        {
            btnClose.BackgroundImage = originalBackgroundImage;
        }
    }
}
