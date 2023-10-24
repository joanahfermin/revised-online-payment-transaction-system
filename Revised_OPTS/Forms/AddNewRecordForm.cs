using Inventory_System.DAL;
using Inventory_System.Exception;
using Inventory_System.Model;
using Inventory_System.Utilities;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
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
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Revised_OPTS.Forms
{
    public partial class AddNewRecordForm : Form
    {

        IRptService rptService = ServiceFactory.Instance.GetRptService();
        IBusinessService businessService = ServiceFactory.Instance.GetBusinessService();
        IMiscService miscService = ServiceFactory.Instance.GetMiscService();
        IRptTaxbillTPNRepository rptRetrieveTaxpayerNameRep = RepositoryFactory.Instance.GetRptRetrieveTaxpayerNameRepository();
        IBusinessMasterDetailTPNRepository businessRetrieveTaxpayerNameRep = RepositoryFactory.Instance.GetBusinessRetrieveTaxpayerNameRepository();
        IMiscDetailsBillingStageRepository miscRetrieveTaxpayerNameRep = RepositoryFactory.Instance.MiscRetrieveTaxpayerNameRepository();

        private Image originalBackgroundImage;
        private Image originalCloseBackgroundImage;

        Color customColor = Color.FromArgb(6, 19, 36);

        DynamicControlContainer dynamicControlContainer;

        public AddNewRecordForm()
        {
            dynamicControlContainer = new DynamicControlContainer(this);

            InitializeComponent();
            InitializeTaxType();
            InitializeDynamicMapping();

            panel1.BackColor = customColor;
            label1.BackColor = customColor;
            btnSaveRecord.BackColor = customColor;
            btnClose.BackColor = customColor;
        }

        public void InitializeTaxType()
        {
            foreach (string miscType in TaxTypeUtil.ALL_TAX_TYPE.OrderBy(taxType => taxType))
            {
                cbTaxType.Items.Add(miscType);
            }
        }

        public void InitializeDynamicMapping()
        {
            List<Bank> bankList = rptService.GetAllBanks();
            string[] bankNames = bankList.Select(bank => bank.BankName).ToList().ToArray();

            //COMMON LABEL AND CONTROL
            DynamicControlInfo[] commonInfo = new DynamicControlInfo[]
                {
                    new DynamicControlInfo{PropertyName = "RequestingParty", Label = "Email Address:", ControlType = DynamicControlType.TextBox},
                    new DynamicControlInfo{PropertyName = "Status", Label = "Status: ", ControlType = DynamicControlType.ComboBox, ComboboxChoices = TaxStatus.STATUS, Enabled = false, InitialValue = TaxStatus.ForPaymentVerification},
                };

            //RPT
            dynamicControlContainer.AddDynamicPropertyMapping(TaxTypeUtil.REALPROPERTYTAX, new DynamicControlInfo[]
                {
                    new DynamicControlInfo{PropertyName = "TaxDec", Label = "*TDN:", ControlType = DynamicControlType.TextBox, isRequired = true, format = BusinessFormat.TAXDEC_FORMAT},
                    new DynamicControlInfo{PropertyName = "TaxPayerName", Label = "*TaxPayer's Name:", ControlType = DynamicControlType.TextBox, isRequired = true},
                    new DynamicControlInfo{PropertyName = "AmountToPay", Label = "*Bill Amount:", ControlType = DynamicControlType.TextBox, InitialValue = "0.00", isRequired = true},
                    new DynamicControlInfo{PropertyName = "AmountTransferred", Label = "*Transferred Amount:", ControlType = DynamicControlType.TextBox, InitialValue = "0.00", isRequired = true},
                    new DynamicControlInfo{PropertyName = "Bank", Label = "*Bank: ", ControlType = DynamicControlType.ComboBox, ComboboxChoices = bankNames, isRequired = true},
                    new DynamicControlInfo{PropertyName = "PaymentDate", Label = "Payment Date: ", ControlType = DynamicControlType.DatePicker, isRequired = true},
                    new DynamicControlInfo{PropertyName = "YearQuarter", Label = "*Year:", ControlType = DynamicControlType.TextBox, isRequired = true},
                    new DynamicControlInfo{PropertyName = "Quarter", Label = "Quarter: ", ControlType = DynamicControlType.ComboBox, ComboboxChoices = Quarter.ALL_QUARTER},
                    new DynamicControlInfo{PropertyName = "RPTremarks", Label = "Remarks:", ControlType = DynamicControlType.TextBox},
                }.Concat(commonInfo).ToArray()); ;

            //BUSINESS
            dynamicControlContainer.AddDynamicPropertyMapping(TaxTypeUtil.BUSINESS,
                new DynamicControlInfo[]
                {
                    new DynamicControlInfo{PropertyName = "Business_Type", Label = "*Business Type: ", ControlType = DynamicControlType.ComboBox, ComboboxChoices = BusinessUtil.BUSINESS_TYPE, isRequired = true},
                    new DynamicControlInfo{PropertyName = "BillNumber", Label = "*Bill Number: ", ControlType = DynamicControlType.TextBox, isRequired = true, format = BusinessFormat.BUSINESS_BILLNUM_FORMAT},
                    new DynamicControlInfo{PropertyName = "TaxpayersName", Label = "*TaxPayer's Name:", ControlType = DynamicControlType.TextBox, isRequired = true},
                    new DynamicControlInfo{PropertyName = "MP_Number", Label = "*M.P Number:", ControlType = DynamicControlType.TextBox, isRequired = true, format = BusinessFormat.MP_FORMAT},
                    new DynamicControlInfo{PropertyName = "BusinessName", Label = "*Business Name: ", ControlType = DynamicControlType.TextBox, isRequired = true},
                    new DynamicControlInfo{PropertyName = "BillAmount", Label = "*Bill Amount:", ControlType = DynamicControlType.TextBox, InitialValue = "0.00", isRequired = true},
                    new DynamicControlInfo{PropertyName = "TotalAmount", Label = "*Transferred Amount:", ControlType = DynamicControlType.TextBox, InitialValue = "0.00", isRequired = true},
                    new DynamicControlInfo{PropertyName = "PaymentChannel", Label = "*Bank: ", ControlType = DynamicControlType.ComboBox, ComboboxChoices = bankNames, isRequired = true},
                    new DynamicControlInfo{PropertyName = "DateOfPayment", Label = "*Payment Date: ", ControlType = DynamicControlType.DatePicker, isRequired = true},
                    new DynamicControlInfo{PropertyName = "Year", Label = "*Year:", ControlType = DynamicControlType.TextBox, isRequired = true},
                    new DynamicControlInfo{PropertyName = "Qtrs", Label = "*Quarter: ", ControlType = DynamicControlType.ComboBox, ComboboxChoices = Quarter.ALL_QUARTER, isRequired = true},
                    new DynamicControlInfo{PropertyName = "MiscFees", Label = "Misc. Fees: ", ControlType = DynamicControlType.TextBox},
                    new DynamicControlInfo{PropertyName = "BussinessRemarks", Label = "Remarks:", ControlType = DynamicControlType.TextBox},
                }.Concat(commonInfo).ToArray());

            //MISC
            DynamicControlInfo[] miscInfo = new DynamicControlInfo[]
                {
                    new DynamicControlInfo{PropertyName = "OrderOfPaymentNum", Label = "*Bill Number:", ControlType = DynamicControlType.TextBox, isRequired = true},
                    new DynamicControlInfo{PropertyName = "TaxpayersName", Label = "*TaxPayer's Name:", ControlType = DynamicControlType.TextBox, isRequired = true},
                    new DynamicControlInfo{PropertyName = "OPATrackingNum", Label = "OPA Tracking No.: ", ControlType = DynamicControlType.TextBox},
                    new DynamicControlInfo{PropertyName = "AmountToBePaid", Label = "*Bill Amount:", ControlType = DynamicControlType.TextBox, InitialValue = "0.00", isRequired = true},
                    new DynamicControlInfo{PropertyName = "TransferredAmount", Label = "*Transferred Amount:", ControlType = DynamicControlType.TextBox, InitialValue = "0.00", isRequired = true},
                    new DynamicControlInfo{PropertyName = "ModeOfPayment", Label = "*Bank:", ControlType = DynamicControlType.ComboBox, ComboboxChoices = bankNames, isRequired = true},
                    new DynamicControlInfo{PropertyName = "PaymentDate", Label = "*Payment Date: ", ControlType = DynamicControlType.DatePicker, isRequired = true},
                    new DynamicControlInfo{PropertyName = "Remarks", Label = "Remarks: ", ControlType = DynamicControlType.TextBox},
                }.Concat(commonInfo).ToArray();

            dynamicControlContainer.AddDynamicPropertyMapping(TaxTypeUtil.MISCELLANEOUS_OCCUPERMIT, DynamicControlInfoUtil.Clone(miscInfo));
            dynamicControlContainer.AddDynamicPropertyMapping(TaxTypeUtil.MISCELLANEOUS_OVR, DynamicControlInfoUtil.Clone(miscInfo));
            dynamicControlContainer.AddDynamicPropertyMapping(TaxTypeUtil.MISCELLANEOUS_MARKET, DynamicControlInfoUtil.Clone(miscInfo));
            dynamicControlContainer.AddDynamicPropertyMapping(TaxTypeUtil.MISCELLANEOUS_ZONING, DynamicControlInfoUtil.Clone(miscInfo));

            //LIQUOR
            dynamicControlContainer.AddDynamicPropertyMapping(TaxTypeUtil.MISCELLANEOUS_LIQUOR,
                new DynamicControlInfo[]
                {
                    new DynamicControlInfo{PropertyName = "BillNumber", Label = "*Bill Number: ", ControlType = DynamicControlType.TextBox, isRequired = true, format = BusinessFormat.LIQUOR_FORMAT},
                    new DynamicControlInfo{PropertyName = "MP_Number", Label = "*M.P Number:", ControlType = DynamicControlType.TextBox, isRequired = true, format = BusinessFormat.MP_FORMAT},
                    new DynamicControlInfo{PropertyName = "TaxpayersName", Label = "*TaxPayer's Name:", ControlType = DynamicControlType.TextBox, isRequired = true},
                    new DynamicControlInfo{PropertyName = "BillAmount", Label = "*Bill Amount:", ControlType = DynamicControlType.TextBox, InitialValue = "0.00", isRequired = true},
                    new DynamicControlInfo{PropertyName = "TotalAmount", Label = "*Transferred Amount:", ControlType = DynamicControlType.TextBox, InitialValue = "0.00", isRequired = true},
                    new DynamicControlInfo{PropertyName = "PaymentChannel", Label = "*Bank: ", ControlType = DynamicControlType.ComboBox, ComboboxChoices = bankNames, isRequired = true},
                    new DynamicControlInfo{PropertyName = "DateOfPayment", Label = "*Payment Date: ", ControlType = DynamicControlType.DatePicker, isRequired = true},
                    new DynamicControlInfo{PropertyName = "Year", Label = "*Year:", ControlType = DynamicControlType.TextBox, isRequired = true},
                    new DynamicControlInfo{PropertyName = "Qtrs", Label = "*Quarter: ", ControlType = DynamicControlType.ComboBox, ComboboxChoices = Quarter.ALL_QUARTER, isRequired = true},
                    new DynamicControlInfo{PropertyName = "BussinessRemarks", Label = "Remarks:", ControlType = DynamicControlType.TextBox},
                }.Concat(commonInfo).ToArray());

            ApplyDynamicMappingSpecialRules();
        }

        private void ApplyDynamicMappingSpecialRules()
        {
            //MISCELLANEOUS_OCCUPERMIT
            DynamicControlInfo OccuPermitOrderOfPaymentNumInfo = dynamicControlContainer.FindDynamicControlInfo(TaxTypeUtil.MISCELLANEOUS_OCCUPERMIT, "OrderOfPaymentNum");
            OccuPermitOrderOfPaymentNumInfo.format = BusinessFormat.OCCUPERMIT_FORMAT;

            //MISCELLANEOUS_OVR
            DynamicControlInfo OvrOrderOfPaymentNumInfo = dynamicControlContainer.FindDynamicControlInfo(TaxTypeUtil.MISCELLANEOUS_OVR, "OrderOfPaymentNum");
            OvrOrderOfPaymentNumInfo.format = BusinessFormat.OVR_TTMD_FORMAT;

            //MISCELLANEOUS_MARKET
            DynamicControlInfo MarketOrderOfPaymentNumInfo = dynamicControlContainer.FindDynamicControlInfo(TaxTypeUtil.MISCELLANEOUS_MARKET, "OrderOfPaymentNum");
            MarketOrderOfPaymentNumInfo.format = BusinessFormat.MARKET_FORMAT;

            DynamicControlInfo MarketOPATrackingNumInfo = dynamicControlContainer.FindDynamicControlInfo(TaxTypeUtil.MISCELLANEOUS_MARKET, "OPATrackingNum");
            MarketOPATrackingNumInfo.Enabled = false;
            MarketOPATrackingNumInfo.InitialValue = "NOT APPLICABLE";

            //MISCELLANEOUS_ZONING
            DynamicControlInfo ZoningOrderOfPaymentNumInfo = dynamicControlContainer.FindDynamicControlInfo(TaxTypeUtil.MISCELLANEOUS_ZONING, "OrderOfPaymentNum");
            ZoningOrderOfPaymentNumInfo.format = BusinessFormat.ZONING_FORMAT;

            DynamicControlInfo ZoningOPATrackingNumInfo = dynamicControlContainer.FindDynamicControlInfo(TaxTypeUtil.MISCELLANEOUS_ZONING, "OPATrackingNum");
            ZoningOPATrackingNumInfo.Enabled = false;
            ZoningOPATrackingNumInfo.InitialValue = "NOT APPLICABLE";

        }


        private void cbTaxType_SelectedIndexChanged(object sender, EventArgs e)
        {
            dynamicControlContainer.RemoveAllDynamicControls();
            string taxType = cbTaxType.Text;
            dynamicControlContainer.AddDynamicControls(taxType);

            if (taxType == TaxTypeUtil.REALPROPERTYTAX)
            {
                Control TaxDecTextBox = dynamicControlContainer.FindControlByName(taxType, "TaxDec");
                TaxDecTextBox.TextChanged += TaxDecTextBox_TextChanged;
            }
            else if (taxType == TaxTypeUtil.BUSINESS)
            {
                Control MP_NumberTextBox = dynamicControlContainer.FindControlByName(taxType, "MP_Number");
                MP_NumberTextBox.TextChanged += MP_NumberTextBox_TextChanged;
            }
            else if (taxType == TaxTypeUtil.MISCELLANEOUS_OCCUPERMIT)
            {
                Control MiscOrderOfPaymentNumTextBox = dynamicControlContainer.FindControlByName(taxType, "OrderOfPaymentNum");
                MiscOrderOfPaymentNumTextBox.TextChanged += MiscOrderOfPaymentNumTextBox_TextChanged;
            }
        }

        private void TaxDecTextBox_TextChanged(object sender, EventArgs e)
        {
            string taxType = cbTaxType.Text;
            Control TaxdecTextBox = dynamicControlContainer.FindControlByName(taxType, "TaxDec");
            Control TaxPayerNameTextBox = dynamicControlContainer.FindControlByName(taxType, "TaxPayerName");

            RptTaxbillTPN rptRetrieveTaxpayerName = rptRetrieveTaxpayerNameRep.retrieveByTDN(TaxdecTextBox.Text);

            if (rptRetrieveTaxpayerName != null)
            {
                TaxPayerNameTextBox.Text = rptRetrieveTaxpayerName.ONAME;
            }
            else
            {
                TaxPayerNameTextBox.Text = Validations.NO_RETRIEVED_NAME;
            }
        }

        private void MP_NumberTextBox_TextChanged(object sender, EventArgs e)
        {
            string taxType = cbTaxType.Text;
            Control MP_NumberTextBox = dynamicControlContainer.FindControlByName(taxType, "MP_Number");
            Control BusinessNameTextBox = dynamicControlContainer.FindControlByName(taxType, "BusinessName");

            BusinessMasterDetailTPN businessRetrieveTaxpayerName = businessRetrieveTaxpayerNameRep.retrieveByMpNo(MP_NumberTextBox.Text);

            if (businessRetrieveTaxpayerName != null)
            {
                BusinessNameTextBox.Text = businessRetrieveTaxpayerName.BusinessName;
            }
            else
            {
                BusinessNameTextBox.Text = Validations.NO_RETRIEVED_NAME;
            }
        }

        private void MiscOrderOfPaymentNumTextBox_TextChanged(object sender, EventArgs e)
        {
            string taxType = cbTaxType.Text;
            Control OrderOfPaymentNum = dynamicControlContainer.FindControlByName(taxType, "OrderOfPaymentNum");
            Control TaxPayerNameTextBox = dynamicControlContainer.FindControlByName(taxType, "TaxpayersName");

            MiscDetailsBillingStage miscRetrieveTaxpayerName = miscRetrieveTaxpayerNameRep.retrieveByBillNum(OrderOfPaymentNum.Text);

            if (miscRetrieveTaxpayerName != null)
            {
                TaxPayerNameTextBox.Text = miscRetrieveTaxpayerName.TaxpayerLName;
            }
            else
            {
                TaxPayerNameTextBox.Text = Validations.NO_RETRIEVED_NAME;
            }
        }

        private void btnSaveRecord_Click(object sender, EventArgs e)
        {
            string taxType = cbTaxType.Text;
            string searchKeyword = "";

            dynamicControlContainer.validateForm(taxType, errorProvider1);
            if (Validations.HaveErrors(errorProvider1))
            {
                return;
            }

            if (taxType == TaxTypeUtil.REALPROPERTYTAX)
            {
                Rpt rpt = new Rpt();
                dynamicControlContainer.CopyDynamicProperties(rpt, taxType);
                rptService.Insert(rpt);
                MessageBox.Show("Record successfully saved.");
                searchKeyword = rpt.TaxDec;
                MainForm.Instance.Search(searchKeyword);
            }

            else if (taxType == TaxTypeUtil.BUSINESS)
            {
                Business business = new Business();
                dynamicControlContainer.CopyDynamicProperties(business, taxType);
                businessService.Insert(business);
                MessageBox.Show("Record successfully saved.");
                searchKeyword = business.MP_Number;
                MainForm.Instance.Search(searchKeyword);
            }

            else
            {
                Miscellaneous misc = new Miscellaneous();
                dynamicControlContainer.CopyDynamicProperties(misc, taxType);
                string miscType = cbTaxType.Text;
                misc.MiscType = miscType;
                miscService.Insert(misc);
                MessageBox.Show("Record successfully saved.");
                searchKeyword = misc.OrderOfPaymentNum;
                MainForm.Instance.Search(searchKeyword);
            }
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
            btnSaveRecord.BackColor = customColor;
        }

        private void btnClose_MouseEnter(object sender, EventArgs e)
        {
            originalCloseBackgroundImage = btnClose.BackgroundImage;
            btnClose.BackgroundImage = null;

            Color customColor = Color.FromArgb(23, 45, 74);
            btnClose.BackColor = customColor;
        }

        private void btnClose_MouseLeave(object sender, EventArgs e)
        {
            btnClose.BackgroundImage = originalCloseBackgroundImage;
            btnClose.BackColor = customColor;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }


}
