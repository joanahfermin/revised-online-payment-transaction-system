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
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Revised_OPTS.Forms
{
    public partial class AddNewRecord : Form
    {
        IRptService rptService = ServiceFactory.Instance.GetRptService();
        IBusinessService businessService = ServiceFactory.Instance.GetBusinessService();

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
            foreach (string miscType in TaxTypeUtil.ALL_TAX_TYPE)
            {
                cbTaxType.Items.Add(miscType);
            }
        }

        public void InitializeDynamicMapping()
        {
            List<Bank> bankList = rptService.GetAllBanks();
            string[] bankNames = bankList.Select(bank => bank.BankName).ToList().ToArray();

            DynamicControlInfo[] commonInfo = new DynamicControlInfo[]
                {
                    new DynamicControlInfo{PropertyName = "RequestingParty", Label = "Email Address:", ControlType = ControlType.TextBox},
                    new DynamicControlInfo{PropertyName = "Status", Label = "Status: ", ControlType = ControlType.ComboBox, ComboboxChoices = TaxStatus.STATUS, Enabled = false, InitialValue = TaxStatus.ForPaymentVerification},
                };

            dynamicPropertyMapping.Add(TaxTypeUtil.REALPROPERTYTAX, new DynamicControlInfo[]
                {
                    new DynamicControlInfo{PropertyName = "TaxDec", Label = "TDN:", ControlType = ControlType.TextBox},
                    new DynamicControlInfo{PropertyName = "TaxPayerName", Label = "TaxPayer's Name:", ControlType = ControlType.TextBox},
                    new DynamicControlInfo{PropertyName = "AmountToPay", Label = "Bill Amount:", ControlType = ControlType.TextBox},
                    new DynamicControlInfo{PropertyName = "RPTremarks", Label = "Remarks:", ControlType = ControlType.TextBox},
                    new DynamicControlInfo{PropertyName = "YearQuarter", Label = "Year:", ControlType = ControlType.TextBox},
                    new DynamicControlInfo{PropertyName = "Bank", Label = "Bank: ", ControlType = ControlType.ComboBox, ComboboxChoices = bankNames},
                    new DynamicControlInfo{PropertyName = "Quarter", Label = "Quarter: ", ControlType = ControlType.ComboBox, ComboboxChoices = Quarter.ALL_QUARTER},
                    new DynamicControlInfo{PropertyName = "PaymentDate", Label = "Payment Date: ", ControlType = ControlType.DatePicker},
                    new DynamicControlInfo{PropertyName = "AmountTransferred", Label = "Transferred Amount:", ControlType = ControlType.TextBox},
                }.Concat(commonInfo).ToArray());

            dynamicPropertyMapping.Add(TaxTypeUtil.BUSINESS,
                new DynamicControlInfo[]
                {
                    new DynamicControlInfo{PropertyName = "Business_Type", Label = "Business Type: ", ControlType = ControlType.ComboBox, ComboboxChoices = BusinessUtil.BUSINESS_TYPE },
                    new DynamicControlInfo{PropertyName = "TaxpayersName", Label = "TaxPayer's Name:", ControlType = ControlType.TextBox},
                    new DynamicControlInfo{PropertyName = "MP_Number", Label = "M.P Number:", ControlType = ControlType.TextBox},
                    new DynamicControlInfo{PropertyName = "BusinessName", Label = "Business Name: ", ControlType = ControlType.TextBox},
                    new DynamicControlInfo{PropertyName = "BillNumber", Label = "Bill Number: ", ControlType = ControlType.TextBox},
                    new DynamicControlInfo{PropertyName = "BillAmount", Label = "Bill Amount:", ControlType = ControlType.TextBox},
                    new DynamicControlInfo{PropertyName = "TotalAmount", Label = "Transferred Amount:", ControlType = ControlType.TextBox},
                    new DynamicControlInfo{PropertyName = "Year", Label = "Year:", ControlType = ControlType.TextBox},
                    new DynamicControlInfo{PropertyName = "MiscFees", Label = "Misc. Fees: ", ControlType = ControlType.TextBox},
                    new DynamicControlInfo{PropertyName = "PaymentChannel", Label = "Bank: ", ControlType = ControlType.ComboBox, ComboboxChoices = bankNames},
                    new DynamicControlInfo{PropertyName = "BussinessRemarks", Label = "Remarks:", ControlType = ControlType.TextBox},
                    new DynamicControlInfo{PropertyName = "Qtrs", Label = "Quarter: ", ControlType = ControlType.ComboBox, ComboboxChoices = Quarter.ALL_QUARTER},
                    new DynamicControlInfo{PropertyName = "DateOfPayment", Label = "Payment Date: ", ControlType = ControlType.DatePicker},
                }.Concat(commonInfo).ToArray());


            /*
            dynamicPropertyLabelMapping.Add(TaxTypeUtil.REALPROPERTYTAX, new string[] { "TDN: ", "TaxPayer's Name: ", "Bill Amount: ",
                "Transferred Amount:", "Year:", "Email Address:", "Remarks:" });
            dynamicPropertyNameMapping.Add(TaxTypeUtil.REALPROPERTYTAX, new string[] { "TaxDec", "TaxPayerName", "AmountToPay",
                "AmountTransferred", "YearQuarter", "RequestingParty", "RPTremarks"});

            dynamicPropertyComboBoxLabelMapping.Add(TaxTypeUtil.REALPROPERTYTAX, new string[] { "Bank: ", "Status: ", "Quarter: " });
            dynamicPropertyComboBoxNameMapping.Add(TaxTypeUtil.REALPROPERTYTAX, new string[] { "Bank", "Status", "Quarter" });
            dynamicPropertyDateTimeLabelMapping.Add(TaxTypeUtil.REALPROPERTYTAX, new Dictionary<string, Type> {{ "Payment Date: ", typeof(DateTime) }});

            dynamicPropertyLabelMapping.Add(TaxTypeUtil.BUSINESS, new string[] { "M.P Number", "TaxPayer's Name: ", "Business Name: ",
                "Bill Number: ", "Bill Amount: ", "Misc. Fees: ", "Transferred Amount: ", "Email Address: ", "Contact Number: ", "Remarks:" });
            dynamicPropertyNameMapping.Add(TaxTypeUtil.BUSINESS, new string[] { "MP_Number", "TaxpayersName", "BusinessName", "BillNumber",
                "BillAmount", "MiscFees", "TotalAmount", "RequestingParty", "ContactNumber", "BussinessRemarks"});
            dynamicPropertyComboBoxLabelMapping.Add(TaxTypeUtil.BUSINESS, new string[] { "Bank: ", "Status: ", "Quarter: " });
            dynamicPropertyComboBoxNameMapping.Add(TaxTypeUtil.BUSINESS, new string[] { "Bank", "Status", "Quarter" });
            dynamicPropertyDateTimeLabelMapping.Add(TaxTypeUtil.BUSINESS, new Dictionary<string, Type> { { "Payment Date: ", typeof(DateTime) } });
            */

            //dynamicPropertyLabelMapping.Add(TaxTypeUtil.MISCELLANEOUS, new string[] { "O.P Number:", "OPA Tracking No.:", "Requesting Party:", "Remarks:" });
            //dynamicPropertyNameMapping.Add(Misc_Type.LIQUOR, new string[] { "OrderOfPaymentNum", "OPATrackingNum", "RequestingParty", "Remarks" });
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

        private void cbTaxType_SelectedIndexChanged(object sender, EventArgs e)
        {

            RemoveAllDynamicControls();
            string taxType = cbTaxType.Text;
            DynamicControlInfo[] dynamicPropertyInfos = dynamicPropertyMapping[taxType];
            AddDynamicControls(dynamicPropertyInfos);
        }

        private void AddDynamicControls(DynamicControlInfo[] dynamicPropertyInfos)
        {
            int y = CONTROL_START_Y;
            int x = LABEL_START_X;
            int controlCounter = 0;

            foreach (DynamicControlInfo propertyInfo in dynamicPropertyInfos)
            {
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
                    TextBox textBox = new TextBox();
                    textBox.BorderStyle = BorderStyle.FixedSingle;
                    textBox.TextAlign = HorizontalAlignment.Center;
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

                control.Enabled = propertyInfo.Enabled;
                if (propertyInfo.InitialValue != null)
                {
                    control.Text = propertyInfo.InitialValue;
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

        private void btnSaveRecord_Click(object sender, EventArgs e)
        {
            string taxType = cbTaxType.Text;
            string searchKeyword = "";

            DynamicControlInfo[] dynamicPropertyInfos = dynamicPropertyMapping[taxType];

            if (taxType == TaxTypeUtil.REALPROPERTYTAX)
            {
                Rpt rpt = new Rpt();
                CopyDynamicProperties(rpt, dynamicPropertyInfos);

                rptService.Insert(rpt);
                searchKeyword = rpt.TaxDec;
                MainForm.Instance.Search(searchKeyword);
            }
            else
            {
                Business business = new Business();
                CopyDynamicProperties(business, dynamicPropertyInfos);

                businessService.Insert(business);
                searchKeyword = business.MP_Number;
                MainForm.Instance.Search(searchKeyword);
            }
            MessageBox.Show("Record successfully saved.");
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
    }

    public enum ControlType
    {
        TextBox,
        ComboBox,
        DatePicker
    }
}
