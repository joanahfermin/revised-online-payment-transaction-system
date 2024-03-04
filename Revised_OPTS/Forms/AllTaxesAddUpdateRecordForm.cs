using Inventory_System.DAL;
using Inventory_System.Exception;
using Inventory_System.Forms;
using Inventory_System.Model;
using Inventory_System.Service;
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
    public partial class AllTaxesAddUpdateRecordForm : Form
    {
        IRptService rptService = ServiceFactory.Instance.GetRptService();
        IBusinessService businessService = ServiceFactory.Instance.GetBusinessService();
        IMiscService miscService = ServiceFactory.Instance.GetMiscService();

        IBusinessMasterDetailTPNRepository businessRetrieveTaxpayerNameRep = RepositoryFactory.Instance.GetBusinessRetrieveTaxpayerNameRepository();
        IMiscDetailsBillingStageRepository miscRetrieveTaxpayerNameRep = RepositoryFactory.Instance.MiscRetrieveTaxpayerNameRepository();

        private Image originalBackgroundImage;
        private Image originalCloseBackgroundImage;

        Color customColor = Color.FromArgb(6, 19, 36);

        DynamicControlContainer dynamicControlContainer;

        Business business;
        Miscellaneous misc;
        string retrieveTaxTypeFromMainForm = "";

        long businessId = 0;
        long miscId = 0;

        public AllTaxesAddUpdateRecordForm()
        {
            dynamicControlContainer = new DynamicControlContainer(this);

            InitializeComponent();
            InitializeTaxType();
            InitializeDynamicMapping();

            panel1.BackColor = customColor;
            label1.BackColor = customColor;
            btnSaveRecord.BackColor = customColor;
            btnClose.BackColor = customColor;

            btnSaveRecord.Visible = false;
            cbTaxType.Text = TaxTypeUtil.BUSINESS;
        }

        //TO DO: IF USER CREATES A RECORD, STATUS SHOULD BE 'FOR PAYMENT VERIFICATION', IF NOT, STATUS CAN EITHER BE 'FOR ASSESSMENT, ASSESSMENT PRINTED, BILL SENT'
        public AllTaxesAddUpdateRecordForm(long id, string taxType)
        {
            dynamicControlContainer = new DynamicControlContainer(this);

            if (taxType == TaxTypeUtil.BUSINESS)
            {
                business = businessService.Get(id);
                businessId = business.BusinessID;
                retrieveTaxTypeFromMainForm = taxType;
            }

            else if (taxType == TaxTypeUtil.MISCELLANEOUS_OCCUPERMIT || taxType == TaxTypeUtil.MISCELLANEOUS_OVR || 
                taxType == TaxTypeUtil.MISCELLANEOUS_ZONING || taxType == TaxTypeUtil.MISCELLANEOUS_MARKET || taxType == TaxTypeUtil.MISCELLANEOUS_LIQUOR)
            {
                misc = miscService.Get(id);
                miscId = misc.MiscID;
                retrieveTaxTypeFromMainForm = taxType;
            }

            InitializeComponent();
            InitializeTaxType();
            InitializeDynamicMapping();
            InitializeRetrieveRecord();

            panel1.BackColor = customColor;
            label1.BackColor = customColor;
            btnSaveRecord.BackColor = customColor;
            btnClose.BackColor = customColor;
        }

        public void InitializeRetrieveRecord()
        {
            cbTaxType.Text = retrieveTaxTypeFromMainForm;
            cbTaxType.Enabled = false;

            Thread newThread = new Thread(RetrieveRecord);
            newThread.Start();

        }


        void RetrieveRecord()
        {
            Thread.Sleep(100);
            Invoke(new Action(() =>
            {
                if (retrieveTaxTypeFromMainForm == TaxTypeUtil.BUSINESS)
                {
                    business = businessService.Get(businessId);
                    dynamicControlContainer.PopulateDynamicControls(retrieveTaxTypeFromMainForm, business);
                }
                else if (retrieveTaxTypeFromMainForm == TaxTypeUtil.MISCELLANEOUS_OCCUPERMIT || retrieveTaxTypeFromMainForm == TaxTypeUtil.MISCELLANEOUS_OVR ||
                    retrieveTaxTypeFromMainForm == TaxTypeUtil.MISCELLANEOUS_MARKET || retrieveTaxTypeFromMainForm == TaxTypeUtil.MISCELLANEOUS_ZONING ||
                    retrieveTaxTypeFromMainForm == TaxTypeUtil.MISCELLANEOUS_LIQUOR)
                {
                    misc = miscService.Get(miscId);
                    dynamicControlContainer.PopulateDynamicControls(retrieveTaxTypeFromMainForm, misc);
                }
            }));
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
            string[] bankNames = bankList.Select(bank => bank.BankName.Trim()).ToList().ToArray();

            //COMMON LABEL AND CONTROL
            DynamicControlInfo[] commonInfo = new DynamicControlInfo[]
                {
                    new DynamicControlInfo{PropertyName = "RequestingParty", Label = "Email Address:", ControlType = DynamicControlType.TextBox},
                    new DynamicControlInfo{PropertyName = "Status", Label = "Status: ", ControlType = DynamicControlType.ComboBox, ComboboxChoices = TaxStatus.STATUS, Enabled = false, InitialValue = TaxStatus.ForPaymentVerification},
                };

            //BUSINESS
            dynamicControlContainer.AddDynamicPropertyMapping(TaxTypeUtil.BUSINESS,
                new DynamicControlInfo[]
                {
                    new DynamicControlInfo{PropertyName = "Business_Type", Label = "*Business Type: ", ControlType = DynamicControlType.ComboBox, ComboboxChoices = BusinessUtil.BUSINESS_TYPE, isRequired = true},
                    new DynamicControlInfo{PropertyName = "BillNumber", Label = "*Bill Number: ", ControlType = DynamicControlType.TextBox, isRequired = true, format = BusinessFormat.BUSINESS_BILLNUM_FORMAT},
                    new DynamicControlInfo{PropertyName = "TaxpayersName", Label = "*TaxPayer's Name:", ControlType = DynamicControlType.TextBox, isRequired = true},
                    new DynamicControlInfo{PropertyName = "MP_Number", Label = "*M.P Number:", ControlType = DynamicControlType.TextBox, isRequired = true, format = BusinessFormat.MP_FORMAT},
                    new DynamicControlInfo{PropertyName = "BusinessName", Label = "*Business Name: ", ControlType = DynamicControlType.TextBox, isRequired = true},
                    new DynamicControlInfo{PropertyName = "BillAmount", Label = "*Bill Amount:", ControlType = DynamicControlType.TextBox, InitialValue = "0.00", isRequired = true, decimalValue = true},
                    new DynamicControlInfo{PropertyName = "TotalAmount", Label = "*Transferred Amount:", ControlType = DynamicControlType.TextBox, InitialValue = "0.00", isRequired = true, decimalValue = true},
                    new DynamicControlInfo{PropertyName = "PaymentChannel", Label = "*Bank: ", ControlType = DynamicControlType.ComboBox, ComboboxChoices = bankNames, isRequired = true},
                    new DynamicControlInfo{PropertyName = "DateOfPayment", Label = "*Payment Date: ", ControlType = DynamicControlType.DatePicker, isRequired = true},
                    new DynamicControlInfo{PropertyName = "Year", Label = "*Year:", ControlType = DynamicControlType.TextBox, isRequired = true},
                    new DynamicControlInfo{PropertyName = "Qtrs", Label = "*Quarter: ", ControlType = DynamicControlType.ComboBox, ComboboxChoices = Quarter.ALL_QUARTER, isRequired = true},
                    new DynamicControlInfo{PropertyName = "MiscFees", Label = "Misc. Fees: ", ControlType = DynamicControlType.TextBox, InitialValue = "0.00"},
                    new DynamicControlInfo{PropertyName = "BussinessRemarks", Label = "Remarks:", ControlType = DynamicControlType.TextBox},
                }.Concat(commonInfo).ToArray());

            //MISC
            DynamicControlInfo[] miscInfo = new DynamicControlInfo[]
                {
                    new DynamicControlInfo{PropertyName = "OrderOfPaymentNum", Label = "*Bill Number:", ControlType = DynamicControlType.TextBox, isRequired = true},
                    new DynamicControlInfo{PropertyName = "TaxpayersName", Label = "*TaxPayer's Name:", ControlType = DynamicControlType.TextBox, isRequired = true},
                    new DynamicControlInfo{PropertyName = "OPATrackingNum", Label = "OPA Tracking No.: ", ControlType = DynamicControlType.TextBox},
                    new DynamicControlInfo{PropertyName = "AmountToBePaid", Label = "*Bill Amount:", ControlType = DynamicControlType.TextBox, InitialValue = "0.00", isRequired = true, decimalValue = true},
                    new DynamicControlInfo{PropertyName = "TransferredAmount", Label = "*Transferred Amount:", ControlType = DynamicControlType.TextBox, InitialValue = "0.00", isRequired = true, decimalValue = true},
                    new DynamicControlInfo{PropertyName = "ModeOfPayment", Label = "*Bank:", ControlType = DynamicControlType.ComboBox, ComboboxChoices = bankNames, isRequired = true},
                    new DynamicControlInfo{PropertyName = "PaymentDate", Label = "*Payment Date: ", ControlType = DynamicControlType.DatePicker, isRequired = true},
                    new DynamicControlInfo{PropertyName = "Remarks", Label = "Remarks: ", ControlType = DynamicControlType.TextBox},
                }.Concat(commonInfo).ToArray();

            dynamicControlContainer.AddDynamicPropertyMapping(TaxTypeUtil.MISCELLANEOUS_OCCUPERMIT, DynamicControlInfoUtil.Clone(miscInfo));
            dynamicControlContainer.AddDynamicPropertyMapping(TaxTypeUtil.MISCELLANEOUS_OVR, DynamicControlInfoUtil.Clone(miscInfo));
            dynamicControlContainer.AddDynamicPropertyMapping(TaxTypeUtil.MISCELLANEOUS_MARKET, DynamicControlInfoUtil.Clone(miscInfo));
            dynamicControlContainer.AddDynamicPropertyMapping(TaxTypeUtil.MISCELLANEOUS_ZONING, DynamicControlInfoUtil.Clone(miscInfo));
            dynamicControlContainer.AddDynamicPropertyMapping(TaxTypeUtil.MISCELLANEOUS_LIQUOR, DynamicControlInfoUtil.Clone(miscInfo));

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

            //MISCELLANEOUS_LIQUOR_CHANGELABELTO_MPNUM
            DynamicControlInfo LiquorLabelInfo = dynamicControlContainer.FindDynamicControlInfo(TaxTypeUtil.MISCELLANEOUS_LIQUOR, "OPATrackingNum");
            LiquorLabelInfo.Label = "MP Number:";
        }

        private void cbTaxType_SelectedIndexChanged(object sender, EventArgs e)
        {
            dynamicControlContainer.RemoveAllDynamicControls();
            string taxType = cbTaxType.Text;
            dynamicControlContainer.AddDynamicControls(taxType);

            btnSaveRecord.Visible = true;

            if (taxType == TaxTypeUtil.BUSINESS)
            {
                Control MP_NumberTextBox = dynamicControlContainer.FindControlByName(taxType, "BillNumber");
                MP_NumberTextBox.KeyDown += MP_NumberTextBox_KeyDown;
                MP_NumberTextBox.Leave += MP_NumberTextBox_Leave;
            }
            else if (taxType != TaxTypeUtil.BUSINESS)
            {
                foreach (string miscTaxType in TaxTypeUtil.ALL_MISC_TAX_TYPE)
                {
                    taxType = miscTaxType;

                    if (taxType == TaxTypeUtil.MISCELLANEOUS_OCCUPERMIT || taxType == TaxTypeUtil.MISCELLANEOUS_OVR ||
                        taxType == TaxTypeUtil.MISCELLANEOUS_MARKET || taxType == TaxTypeUtil.MISCELLANEOUS_ZONING ||
                        taxType == TaxTypeUtil.MISCELLANEOUS_LIQUOR)
                    {
                        Control MiscOrderOfPaymentNumTextBox = dynamicControlContainer.FindControlByName(taxType, "OrderOfPaymentNum");
                        MiscOrderOfPaymentNumTextBox.KeyDown += MiscOrderOfPaymentNumTextBox_Keydown;
                        MiscOrderOfPaymentNumTextBox.Leave += MiscOrderOfPaymentNumTextBox_Leave;
                    }
                }
            }
            else
            {
                MessageBox.Show("NOT YET IMPLEMENTED.");
            }
        }

        private void MiscOrderOfPaymentNumTextBox_Leave(object? sender, EventArgs e)
        {
            RetrieveTPNforAllMisc();
        }

        private void MiscOrderOfPaymentNumTextBox_Keydown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                RetrieveTPNforAllMisc();
            }
        }

        private void MP_NumberTextBox_Leave(object? sender, EventArgs e)
        {
            RetrieveTPNforBusiness();
        }

        private void MP_NumberTextBox_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                RetrieveTPNforBusiness();
            }
        }

        //based on MP Number.
        private void RetrieveTPNforBusiness()
        {
            string taxType = cbTaxType.Text;
            Control MP_NumberTextBox = dynamicControlContainer.FindControlByName(taxType, "BillNumber");
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

        //based on Bill Number.
        private void RetrieveTPNforAllMisc()
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
            if (taxType == TaxTypeUtil.BUSINESS)
            {
                List<Business> businessList = new List<Business>();

                try
                {
                    if (business != null)
                    {
                        dynamicControlContainer.CopyDynamicProperties(business, taxType);
                        businessService.Update(business);
                        searchKeyword = business.BillNumber;
                    }
                    else
                    {
                        Business business = new Business();
                        dynamicControlContainer.CopyDynamicProperties(business, taxType);

                        businessList.Add(business);
                        businessService.Insert(businessList);
                        searchKeyword = business.BillNumber;
                    }
                    notifyUserAndRefreshRecord(searchKeyword);

                }
                catch (DuplicateRecordException ex)
                {
                    DialogResult result = MessageBox.Show($"There is an existing record/s detected in the database.", "Duplicate Record Detected", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);

                    ExistingRecordForm DuplicateForm = new ExistingRecordForm(new List<Rpt>(), ex.duplicateBusList, new List<Miscellaneous>());
                    DuplicateForm.ShowDialog();

                    if (MessageBox.Show("Would you like to continue?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        businessService.Insert(businessList, false);
                        MessageBox.Show("Record(s) have been successfully saved.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    return;
                }
            }
            else
            {
                List<Miscellaneous> miscList = new List<Miscellaneous>();

                try
                {
                    if (misc != null)
                    {
                        dynamicControlContainer.CopyDynamicProperties(misc, taxType);
                        miscService.Update(misc);
                        searchKeyword = misc.OrderOfPaymentNum;
                    }
                    else
                    {
                        Miscellaneous misc = new Miscellaneous();
                        dynamicControlContainer.CopyDynamicProperties(misc, taxType);
                        misc.MiscType = taxType;
                        miscList.Add(misc);

                        miscService.Insert(miscList);
                        searchKeyword = misc.OrderOfPaymentNum;
                    }
                    notifyUserAndRefreshRecord(searchKeyword);
                }
                catch (DuplicateRecordException ex)
                {
                    DialogResult result = MessageBox.Show($"There is an existing record/s detected in the database.", "Duplicate Record Detected", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);

                    ExistingRecordForm DuplicateForm = new ExistingRecordForm(new List<Rpt>(), new List<Business>(), ex.duplicateMiscList);
                    DuplicateForm.ShowDialog();

                    if (MessageBox.Show("Would you like to continue?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        miscService.Insert(miscList, false);
                        MessageBox.Show("Record(s) have been successfully saved.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    return;
                }
            }
            btnClose_Click(sender, e);
        }

        public void notifyUserAndRefreshRecord(string keyWord)
        {
            MessageBox.Show("Record successfully saved.");
            MainForm.Instance.Search(keyWord);
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
