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
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Revised_OPTS.Forms
{
    public partial class AddNewRecord : Form
    {
        private const string TAXDEC_FORMAT = "^[A|B|C|D|E|F|G]-[0-9]{3}-[0-9]{5}( / [A|B|C|D|E|F|G]-[0-9]{3}-[0-9]{5})*$";
        private const string MP_FORMAT = "^[0-9]{2}-[0-9]{6}$";
        private const string BUSINESS_BILLNUM_FORMAT = "^[B]-[0-9]{4}-[0-9]{2}-[0-9]{2}-[N][X]-[0-9]{6}$";
        //private const string OCCUPERMIT_FORMAT = "^[M]-[0-9]{4}-[0-9]{2}-[0-9]{2}-[B][P][L][O]-[A-Z,0-9]{4}-[0-9]{6}$";
        //private const string OVR_TTMD_FORMAT = "^[M]-[0-9]{4}-[0-9]{2}-[0-9]{2}-[T][T][M][D]-[A-Z,0-9]{4}-[0-9]{6}$";
        //private const string OVR_DPOS_FORMAT = "^[M]-[0-9]{4}-[0-9]{2}-[0-9]{2}-[D][P][O][S]-[A-Z,0-9]{4}-[0-9]{6}$";
        //private const string MARKET_FORMAT = "^[M]-[0-9]{4}-[0-9]{2}-[0-9]{2}-[M][D][A][D]-[A-Z,0-9]{4}-[0-9]{6}$";
        //private const string ZONING_FORMAT = "^[M]-[0-9]{4}-[0-9]{2}-[0-9]{2}-[C][P][D][O]-[A-Z,0-9]{4}-[0-9]{6}$";
        private const string LIQUOR_FORMAT = "^[M]-[0-9]{4}-[0-9]{2}-[0-9]{2}-[L][L][R][B]-[A-Z,0-9]{4}-[0-9]{6}$";

        IRptService rptService = ServiceFactory.Instance.GetRptService();
        IBusinessService businessService = ServiceFactory.Instance.GetBusinessService();
        IMiscService miscService = ServiceFactory.Instance.GetMiscService();
        IRptTaxbillTPNRepository rptRetrieveTaxpayerNameRep = RepositoryFactory.Instance.GetRptRetrieveTaxpayerNameRepository();
        IBusinessMasterDetailTPNRepository businessRetrieveTaxpayerNameRep = RepositoryFactory.Instance.GetBusinessRetrieveTaxpayerNameRepository();
        IMiscDetailsBillingStageRepository miscRetrieveTaxpayerNameRep = RepositoryFactory.Instance.MiscRetrieveTaxpayerNameRepository();

        private List<Label> dynamicLabelList = new List<Label>();
        private List<Control> dynamicControlList = new List<Control>();

        Dictionary<string, DynamicControlInfo[]> dynamicPropertyMapping = new Dictionary<string, DynamicControlInfo[]>();

        private int CONTROL_HEIGHT_INCREMENT = 37;
        private int CONTROL_START_Y = 185;

        private int LABEL_START_X = 130; 

        private int LABEL_COL2_START_X = 630; 

        private Image originalBackgroundImage;
        private Image originalCloseBackgroundImage;

        Color customColor = Color.FromArgb(6, 19, 36);

        public AddNewRecord()
        {
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
                    new DynamicControlInfo{PropertyName = "RequestingParty", Label = "Email Address:", ControlType = ControlType.TextBox},
                    new DynamicControlInfo{PropertyName = "Status", Label = "Status: ", ControlType = ControlType.ComboBox, ComboboxChoices = TaxStatus.STATUS, Enabled = false, InitialValue = TaxStatus.ForPaymentVerification},
                };

            //RPT
            dynamicPropertyMapping.Add(TaxTypeUtil.REALPROPERTYTAX, new DynamicControlInfo[]
                {
                    new DynamicControlInfo{PropertyName = "TaxDec", Label = "*TDN:", ControlType = ControlType.TextBox, isRequired = true, format = TAXDEC_FORMAT},
                    new DynamicControlInfo{PropertyName = "TaxPayerName", Label = "*TaxPayer's Name:", ControlType = ControlType.TextBox, isRequired = true},
                    new DynamicControlInfo{PropertyName = "AmountToPay", Label = "*Bill Amount:", ControlType = ControlType.TextBox, InitialValue = "0.00", isRequired = true},
                    new DynamicControlInfo{PropertyName = "AmountTransferred", Label = "*Transferred Amount:", ControlType = ControlType.TextBox, InitialValue = "0.00", isRequired = true},
                    new DynamicControlInfo{PropertyName = "Bank", Label = "*Bank: ", ControlType = ControlType.ComboBox, ComboboxChoices = bankNames, isRequired = true},
                    new DynamicControlInfo{PropertyName = "PaymentDate", Label = "Payment Date: ", ControlType = ControlType.DatePicker, isRequired = true},
                    new DynamicControlInfo{PropertyName = "YearQuarter", Label = "*Year:", ControlType = ControlType.TextBox, isRequired = true},
                    new DynamicControlInfo{PropertyName = "Quarter", Label = "Quarter: ", ControlType = ControlType.ComboBox, ComboboxChoices = Quarter.ALL_QUARTER},
                    new DynamicControlInfo{PropertyName = "RPTremarks", Label = "Remarks:", ControlType = ControlType.TextBox},
                }.Concat(commonInfo).ToArray()); ;

            //BUSINESS
            dynamicPropertyMapping.Add(TaxTypeUtil.BUSINESS,
                new DynamicControlInfo[]
                {
                    new DynamicControlInfo{PropertyName = "Business_Type", Label = "*Business Type: ", ControlType = ControlType.ComboBox, ComboboxChoices = BusinessUtil.BUSINESS_TYPE, isRequired = true},
                    new DynamicControlInfo{PropertyName = "BillNumber", Label = "*Bill Number: ", ControlType = ControlType.TextBox, isRequired = true, format = BUSINESS_BILLNUM_FORMAT},
                    new DynamicControlInfo{PropertyName = "TaxpayersName", Label = "*TaxPayer's Name:", ControlType = ControlType.TextBox, isRequired = true},
                    new DynamicControlInfo{PropertyName = "MP_Number", Label = "*M.P Number:", ControlType = ControlType.TextBox, isRequired = true, format = MP_FORMAT},
                    new DynamicControlInfo{PropertyName = "BusinessName", Label = "*Business Name: ", ControlType = ControlType.TextBox, isRequired = true},
                    new DynamicControlInfo{PropertyName = "BillAmount", Label = "*Bill Amount:", ControlType = ControlType.TextBox, InitialValue = "0.00", isRequired = true},
                    new DynamicControlInfo{PropertyName = "TotalAmount", Label = "*Transferred Amount:", ControlType = ControlType.TextBox, InitialValue = "0.00", isRequired = true},
                    new DynamicControlInfo{PropertyName = "PaymentChannel", Label = "*Bank: ", ControlType = ControlType.ComboBox, ComboboxChoices = bankNames, isRequired = true},
                    new DynamicControlInfo{PropertyName = "DateOfPayment", Label = "*Payment Date: ", ControlType = ControlType.DatePicker, isRequired = true},
                    new DynamicControlInfo{PropertyName = "Year", Label = "*Year:", ControlType = ControlType.TextBox, isRequired = true},
                    new DynamicControlInfo{PropertyName = "Qtrs", Label = "*Quarter: ", ControlType = ControlType.ComboBox, ComboboxChoices = Quarter.ALL_QUARTER, isRequired = true},
                    new DynamicControlInfo{PropertyName = "MiscFees", Label = "Misc. Fees: ", ControlType = ControlType.TextBox},
                    new DynamicControlInfo{PropertyName = "BussinessRemarks", Label = "Remarks:", ControlType = ControlType.TextBox},
                }.Concat(commonInfo).ToArray());

            //MISC
            DynamicControlInfo[] miscInfo = new DynamicControlInfo[]
                {
                    new DynamicControlInfo{PropertyName = "OrderOfPaymentNum", Label = "*Bill Number:", ControlType = ControlType.TextBox, isRequired = true},
                    new DynamicControlInfo{PropertyName = "TaxpayersName", Label = "*TaxPayer's Name:", ControlType = ControlType.TextBox, isRequired = true},
                    new DynamicControlInfo{PropertyName = "OPATrackingNum", Label = "OPA Tracking No.: ", ControlType = ControlType.TextBox},
                    new DynamicControlInfo{PropertyName = "AmountToBePaid", Label = "*Bill Amount:", ControlType = ControlType.TextBox, InitialValue = "0.00", isRequired = true},
                    new DynamicControlInfo{PropertyName = "TransferredAmount", Label = "*Transferred Amount:", ControlType = ControlType.TextBox, InitialValue = "0.00", isRequired = true},
                    new DynamicControlInfo{PropertyName = "ModeOfPayment", Label = "*Bank:", ControlType = ControlType.ComboBox, ComboboxChoices = bankNames, isRequired = true},
                    new DynamicControlInfo{PropertyName = "PaymentDate", Label = "*Payment Date: ", ControlType = ControlType.DatePicker, isRequired = true},
                    new DynamicControlInfo{PropertyName = "Remarks", Label = "Remarks: ", ControlType = ControlType.TextBox},
                }.Concat(commonInfo).ToArray();

            dynamicPropertyMapping.Add(TaxTypeUtil.MISCELLANEOUS_OCCUPERMIT, miscInfo);
            dynamicPropertyMapping.Add(TaxTypeUtil.MISCELLANEOUS_OVR, miscInfo);
            dynamicPropertyMapping.Add(TaxTypeUtil.MISCELLANEOUS_MARKET, miscInfo);
            dynamicPropertyMapping.Add(TaxTypeUtil.MISCELLANEOUS_ZONING, miscInfo);

            //LIQUOR
            dynamicPropertyMapping.Add(TaxTypeUtil.MISCELLANEOUS_LIQUOR,
                new DynamicControlInfo[]
                {
                    new DynamicControlInfo{PropertyName = "BillNumber", Label = "*Bill Number: ", ControlType = ControlType.TextBox, isRequired = true, format = LIQUOR_FORMAT},
                    new DynamicControlInfo{PropertyName = "MP_Number", Label = "*M.P Number:", ControlType = ControlType.TextBox, isRequired = true, format = MP_FORMAT},
                    new DynamicControlInfo{PropertyName = "TaxpayersName", Label = "*TaxPayer's Name:", ControlType = ControlType.TextBox, isRequired = true},
                    new DynamicControlInfo{PropertyName = "BillAmount", Label = "*Bill Amount:", ControlType = ControlType.TextBox, InitialValue = "0.00", isRequired = true},
                    new DynamicControlInfo{PropertyName = "TotalAmount", Label = "*Transferred Amount:", ControlType = ControlType.TextBox, InitialValue = "0.00", isRequired = true},
                    new DynamicControlInfo{PropertyName = "PaymentChannel", Label = "*Bank: ", ControlType = ControlType.ComboBox, ComboboxChoices = bankNames, isRequired = true},
                    new DynamicControlInfo{PropertyName = "DateOfPayment", Label = "*Payment Date: ", ControlType = ControlType.DatePicker, isRequired = true},
                    new DynamicControlInfo{PropertyName = "Year", Label = "*Year:", ControlType = ControlType.TextBox, isRequired = true},
                    new DynamicControlInfo{PropertyName = "Qtrs", Label = "*Quarter: ", ControlType = ControlType.ComboBox, ComboboxChoices = Quarter.ALL_QUARTER, isRequired = true},
                    new DynamicControlInfo{PropertyName = "BussinessRemarks", Label = "Remarks:", ControlType = ControlType.TextBox},
                }.Concat(commonInfo).ToArray());
        }

        private void RemoveAllDynamicControls()
        {
            foreach (Label label in dynamicLabelList)
            {
                Controls.Remove(label);
            }
            foreach (Control textBox in dynamicControlList)
            {
                Controls.Remove(textBox);
            }
        }

        private Control FindControlByName(String propertyName)
        {
            string taxType = cbTaxType.Text;
            DynamicControlInfo[] dynamicPropertyInfos = dynamicPropertyMapping[taxType];

            for (int i = 0; i < dynamicPropertyInfos.Length; i++)
            {
                DynamicControlInfo dynamicPropertyInfo = dynamicPropertyInfos[i];
                Control control = dynamicControlList[i];
                if (propertyName == dynamicPropertyInfo.PropertyName)
                {
                    return control;
                }
            }
            return null;
        }

        private void cbTaxType_SelectedIndexChanged(object sender, EventArgs e)
        {
            RemoveAllDynamicControls();
            string taxType = cbTaxType.Text;
            DynamicControlInfo[] dynamicPropertyInfos = dynamicPropertyMapping[taxType];
            AddDynamicControls(dynamicPropertyInfos);

            if (taxType == TaxTypeUtil.REALPROPERTYTAX)
            {
                Control TaxDecTextBox = FindControlByName("TaxDec");
                TaxDecTextBox.TextChanged += TaxDecTextBox_TextChanged;
            }
            else if (taxType == TaxTypeUtil.BUSINESS)
            {
                Control MP_NumberTextBox = FindControlByName("MP_Number");
                MP_NumberTextBox.TextChanged += MP_NumberTextBox_TextChanged;
            }
            else if (taxType == TaxTypeUtil.MISCELLANEOUS_OCCUPERMIT)
            {
                Control MiscOrderOfPaymentNumTextBox = FindControlByName("OrderOfPaymentNum");
                MiscOrderOfPaymentNumTextBox.TextChanged += MiscOrderOfPaymentNumTextBox_TextChanged;
            }
        }

        private void TaxDecTextBox_TextChanged(object sender, EventArgs e)
        {
            Control TaxdecTextBox = FindControlByName("TaxDec");
            Control TaxPayerNameTextBox = FindControlByName("TaxPayerName");

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
            Control MP_NumberTextBox = FindControlByName("MP_Number");
            Control BusinessNameTextBox = FindControlByName("BusinessName");

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
            Control OrderOfPaymentNum = FindControlByName("OrderOfPaymentNum");
            Control TaxPayerNameTextBox = FindControlByName("TaxpayersName");

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

        private void AddDynamicControls(DynamicControlInfo[] dynamicPropertyInfos)
        {
            int y = CONTROL_START_Y;
            int x = LABEL_START_X;
            int controlCounter = 0;

            string taxType = cbTaxType.Text;

            foreach (DynamicControlInfo propertyInfo in dynamicPropertyInfos)
            {
                TextBox textBox = new TextBox();
                controlCounter++;
                if (controlCounter == 8)
                {
                    y = CONTROL_START_Y;
                    x = LABEL_COL2_START_X;
                }

                // Create the label above the textbox
                Label label = new Label();
                label.Top = y;
                label.Left = x;
                label.Text = propertyInfo.Label;
                label.Width = 250;
                label.Font = new Font("Tahoma", 12, FontStyle.Regular);
                label.ForeColor = Color.White;
                label.BackColor = Color.Transparent;
                dynamicLabelList.Add(label);
                Controls.Add(label);

                // Adjust the Y position for the textbox to be below the label
                y += CONTROL_HEIGHT_INCREMENT;

                Control control = null;
                if (propertyInfo.ControlType == ControlType.TextBox)
                {
                    textBox.BorderStyle = BorderStyle.FixedSingle;
                    textBox.TextAlign = HorizontalAlignment.Center;

                    if (taxType != TaxTypeUtil.MISCELLANEOUS_OCCUPERMIT && taxType != TaxTypeUtil.MISCELLANEOUS_OVR)
                    {
                        if (propertyInfo.PropertyName == "OPATrackingNum")
                        {
                            textBox.Enabled = false;
                        }
                    }
                    control = textBox;
                }
                else if (propertyInfo.ControlType == ControlType.ComboBox)
                {
                    ComboBox comboBox = new ComboBox();
                    comboBox.DataSource = propertyInfo.ComboboxChoices;
                    control = comboBox;
                }
                else if (propertyInfo.ControlType == ControlType.DatePicker)
                {
                    DateTimePicker dateTimePicker = new DateTimePicker
                    {
                        Format = DateTimePickerFormat.Short,
                    };
                    control = dateTimePicker;
                }

                if (taxType != TaxTypeUtil.MISCELLANEOUS_OCCUPERMIT && taxType != TaxTypeUtil.MISCELLANEOUS_OVR)
                {
                    if (propertyInfo.PropertyName == "OPATrackingNum")
                    {
                        control.Enabled = false;
                        textBox.Text = "NOT APPLICABLE";
                    }
                    else
                    {
                        control.Enabled = propertyInfo.Enabled;
                    }
                }

                if (propertyInfo.InitialValue != null)
                {
                    control.Text = propertyInfo.InitialValue;
                    control.Enabled = false;
                }

                control.Top = y;  // Place the textbox below the label
                control.Left = x;
                control.Width = 350;
                control.Height = 28;
                control.Font = new Font("Tahoma", 12, FontStyle.Regular);
                control.BackColor = Color.AliceBlue;
                dynamicControlList.Add(control);
                Controls.Add(control);

                // Increment the Y position for the next label-textbox pair
                y += CONTROL_HEIGHT_INCREMENT;
            }
        }

        public void validateForm()
        {
            errorProvider1.Clear();
            string taxType = cbTaxType.Text;

            DynamicControlInfo[] dynamicPropertyInfos = dynamicPropertyMapping[taxType];
            for (int i = 0; i < dynamicPropertyInfos.Length; i++)
            {
                DynamicControlInfo dynamicPropertyInfo = dynamicPropertyInfos[i];
                Control control = dynamicControlList[i];

                if (dynamicPropertyInfo.isRequired)
                {
                    Validations.ValidateRequired(errorProvider1, control, dynamicPropertyInfo.Label);
                }

                if (dynamicPropertyInfo.format.Length > 0)
                {
                    Validations.ValidateFormat(errorProvider1, control, dynamicPropertyInfo.Label, dynamicPropertyInfo.format);
                }
            }
        }

        private void btnSaveRecord_Click(object sender, EventArgs e)
        {
            string taxType = cbTaxType.Text;
            string searchKeyword = "";

            DynamicControlInfo[] dynamicPropertyInfos = dynamicPropertyMapping[taxType];

            validateForm();
            if (Validations.HaveErrors(errorProvider1))
            {
                return;
            }

            if (taxType == TaxTypeUtil.REALPROPERTYTAX)
            {
                Rpt rpt = new Rpt();
                CopyDynamicProperties(rpt, dynamicPropertyInfos);
                rptService.Insert(rpt);
                MessageBox.Show("Record successfully saved.");
                searchKeyword = rpt.TaxDec;
                MainForm.Instance.Search(searchKeyword);
            }

            else if (taxType == TaxTypeUtil.BUSINESS)
            {
                Business business = new Business();
                CopyDynamicProperties(business, dynamicPropertyInfos);
                businessService.Insert(business);
                MessageBox.Show("Record successfully saved.");
                searchKeyword = business.MP_Number;
                MainForm.Instance.Search(searchKeyword);
            }

            else
            {
                Miscellaneous misc = new Miscellaneous();
                CopyDynamicProperties(misc, dynamicPropertyInfos);
                string miscType = cbTaxType.Text;
                misc.MiscType = miscType;
                miscService.Insert(misc);
                MessageBox.Show("Record successfully saved.");
                searchKeyword = misc.OrderOfPaymentNum;
                MainForm.Instance.Search(searchKeyword);
            }
        }

        private void CopyDynamicProperties(Object obj, DynamicControlInfo[] dynamicPropertyInfos)
        {
            for (int i = 0; i < dynamicPropertyInfos.Length; i++)
            {
                DynamicControlInfo dynamicPropertyInfo = dynamicPropertyInfos[i];
                string value = dynamicControlList[i].Text;
                PropertyInfo propertyInfo = obj.GetType().GetProperty(dynamicPropertyInfo.PropertyName);

                if (propertyInfo.PropertyType == typeof(decimal?) || propertyInfo.PropertyType == typeof(decimal))
                {
                    decimal decimalValue = 0;

                    if (value.Length > 0)
                    {
                        decimalValue = decimal.Parse(value);
                    }
                    propertyInfo.SetValue(obj, decimalValue);

                }
                else if (propertyInfo.PropertyType == typeof(DateTime?) || propertyInfo.PropertyType == typeof(DateTime))
                {
                    DateTime? dateTimeValue = DateTime.Parse(value);
                    propertyInfo.SetValue(obj, dateTimeValue);
                }
                else
                {
                    propertyInfo.SetValue(obj, value);
                }
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
    public class DynamicControlInfo
    {
        public string PropertyName;
        public string Label;
        public ControlType ControlType;
        public string[] ComboboxChoices;
        public bool Enabled = true;
        public string InitialValue = null;
        public bool isRequired = false;
        public string format = string.Empty;
    }

    public enum ControlType
    {
        TextBox,
        ComboBox,
        DatePicker
    }
}
