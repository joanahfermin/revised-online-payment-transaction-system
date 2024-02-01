using Inventory_System.Utilities;
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
        private DynamicGridContainer<Rpt> RptDynamicGridContainer;
        private DynamicGridContainer<Business> BusinessDynamicGridContainer;
        private DynamicGridContainer<Miscellaneous> MiscDynamicGridContainer;

        public ExistingRecordForm(List<Rpt> existingRecordList, List<Miscellaneous> existingMiscRecordList, List<Business> existingBusRecordList)
        {
            InitializeComponent();
            InitializeRptDataGridView();
            InitializeBusinessDataGridView();

            DgRptAddUpdateForm.DefaultCellStyle.Font = new Font("Tahoma", 12, FontStyle.Regular);
            this.WindowState = FormWindowState.Maximized;

            RptDynamicGridContainer.PopulateData(existingRecordList);
            BusinessDynamicGridContainer.PopulateData(existingBusRecordList);
            //MiscDynamicGridContainer.PopulateData(existingMiscRecordList);
        }

        private void InitializeRptDataGridView()
        {
            List<Bank> bankList = rptService.GetAllBanks();
            string[] bankNames = bankList.Select(bank => bank.BankName).ToList().ToArray();

            DynamicGridInfo[] gridInfoArray = new DynamicGridInfo[] {
                new DynamicGridInfo{PropertyName="RptID", Label = "ID", isReadOnly = true },
                new DynamicGridInfo{PropertyName="TaxDec", Label = "TDN", isRequired=true },
                new DynamicGridInfo{PropertyName="TaxPayerName", Label = "TaxPayer's Name", isRequired=true },
                new DynamicGridInfo{PropertyName="AmountToPay", Label = "Bill Amount", isRequired=true },
                new DynamicGridInfo{PropertyName="TotalAmountTransferred", Label = "Total Amount Transferred", isRequired=true },
                new DynamicGridInfo{PropertyName="Bank", Label = "Bank", GridType = DynamicGridType.ComboBox, ComboboxChoices = bankNames, isRequired=true },
                new DynamicGridInfo{PropertyName="PaymentDate", Label = "Payment Date", GridType = DynamicGridType.DatetimePicker, isRequired=true },
                new DynamicGridInfo{PropertyName="YearQuarter", Label = "Year", decimalValue = true, isRequired=true},
                new DynamicGridInfo{PropertyName="Quarter", Label = "Quarter", GridType=DynamicGridType.ComboBox, ComboboxChoices = Quarter.ALL_QUARTER, isRequired=true },
                new DynamicGridInfo{PropertyName="Status", Label = "Status", GridType=DynamicGridType.ComboBox, ComboboxChoices = TaxStatus.STATUS, isRequired=true },
                new DynamicGridInfo{PropertyName="BillingSelection", Label = "Billing Selection", GridType=DynamicGridType.ComboBox, ComboboxChoices = BillingSelectionUtil.ALL_BILLING_SELECTION, isRequired=true },
                new DynamicGridInfo{PropertyName="RequestingParty", Label = "Email Address" },
                new DynamicGridInfo{PropertyName="RPTremarks", Label = "Remarks"},
            };
            RptDynamicGridContainer = new DynamicGridContainer<Rpt>(DgRptAddUpdateForm, gridInfoArray, true, true);
        }

        private void InitializeBusinessDataGridView()
        {
            List<Bank> bankList = rptService.GetAllBanks();
            string[] bankNames = bankList.Select(bank => bank.BankName).ToList().ToArray();

            DynamicGridInfo[] gridInfoArray = new DynamicGridInfo[] {
                new DynamicGridInfo{PropertyName="BusinessID", Label = "BusID", isReadOnly = true },
                new DynamicGridInfo{PropertyName="Business_Type", Label = "Business Type", isRequired=true },
                new DynamicGridInfo{PropertyName="MP_Number", Label = "MP Number", isRequired=true },
                new DynamicGridInfo{PropertyName="TaxpayersName", Label = "TaxPayer's Name", isRequired=true },
                new DynamicGridInfo{PropertyName="BusinessName", Label = "Business Name", isRequired=true },
                new DynamicGridInfo{PropertyName="BillNumber", Label = "Bill Number", isRequired=true },
                new DynamicGridInfo{PropertyName="AmountToPay", Label = "Bill Amount", isRequired=true },
                new DynamicGridInfo{PropertyName="MiscFees", Label = "Misc. Fees", isRequired=true },
                new DynamicGridInfo{PropertyName="TotalAmount", Label = "Total Amount Transferred", isRequired=true },
                new DynamicGridInfo{PropertyName="PaymentChannel", Label = "Bank", GridType = DynamicGridType.ComboBox, ComboboxChoices = bankNames, isRequired=true },
                new DynamicGridInfo{PropertyName="PaymentDate", Label = "Payment Date", GridType = DynamicGridType.DatetimePicker, isRequired=true },
                new DynamicGridInfo{PropertyName="Year", Label = "Year", decimalValue = true},
                new DynamicGridInfo{PropertyName="Qtrs", Label = "Quarter", GridType=DynamicGridType.ComboBox, ComboboxChoices = Quarter.ALL_QUARTER, isRequired=true },
                new DynamicGridInfo{PropertyName="Status", Label = "Status", GridType=DynamicGridType.ComboBox, ComboboxChoices = TaxStatus.STATUS, isRequired=true },
                new DynamicGridInfo{PropertyName="RequestingParty", Label = "Email Address" },
                new DynamicGridInfo{PropertyName="BussinessRemarks", Label = "Remarks"},
            };
            BusinessDynamicGridContainer = new DynamicGridContainer<Business>(DgBusAddUpdateForm, gridInfoArray, true, true);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSaveRecord_Click(object sender, EventArgs e)
        {

        }
    }
}
