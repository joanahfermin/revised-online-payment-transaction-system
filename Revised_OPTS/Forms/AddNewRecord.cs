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

        private List<Label> dynamicLabelList = new List<Label>();
        private List<TextBox> dynamicTextboxList = new List<TextBox>();
        private List<ComboBox> dynamicComboBoxList = new List<ComboBox>();
        private List<DateTimePicker> dynamicDateTime = new List<DateTimePicker>();

        Dictionary<string, string[]> dynamicPropertyLabelMapping = new Dictionary<string, string[]>();
        Dictionary<string, string[]> dynamicPropertyNameMapping = new Dictionary<string, string[]>();
        Dictionary<string, string[]> dynamicPropertyComboBoxLabelMapping = new Dictionary<string, string[]>();
        Dictionary<string, string[]> dynamicPropertyComboBoxNameMapping = new Dictionary<string, string[]>();
        Dictionary<string, Dictionary<string, Type>> dynamicPropertyDateTimeLabelMapping = new Dictionary<string, Dictionary<string, Type>>();

        private int CONTROL_HEIGHT_INCREMENT = 37;
        private int CONTROL_START_Y = 145;

        private int LABEL_START_X = 130;
        private int TEXTBOX_START_X = 130;

        private int COMBOBOX_CONTROL_HEIGHT_INCREMENT = 37;
        private int COMBOBOX_CONTROL_START_Y = 145;

        private int COMBOBOX_LABEL_START_X = 630;
        private int COMBOBOX_TEXTBOX_START_X = 630;

        private int DATETIME_CONTROL_HEIGHT_INCREMENT = 37;
        private int DATETIME_CONTROL_START_Y = 370;

        private int DATETIME_LABEL_START_X = 630;
        private int DATETIME_PICKER_START_X = 630;

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


            //dynamicPropertyLabelMapping.Add(TaxTypeUtil.MISCELLANEOUS, new string[] { "O.P Number:", "OPA Tracking No.:", "Requesting Party:", "Remarks:" });
            //dynamicPropertyNameMapping.Add(Misc_Type.LIQUOR, new string[] { "OrderOfPaymentNum", "OPATrackingNum", "RequestingParty", "Remarks" });
        }

        private void RemoveAllDynamicControls()
        {
            foreach (Label label in dynamicLabelList)
            {
                Controls.Remove(label);
            }
            foreach (TextBox textBox in dynamicTextboxList)
            {
                Controls.Remove(textBox);
            }
            foreach (ComboBox comboBox in dynamicComboBoxList)
            {
                Controls.Remove(comboBox);
            }

            dynamicLabelList.Clear();
            dynamicTextboxList.Clear();
        }

        private void cbTaxType_SelectedIndexChanged(object sender, EventArgs e)
        {

            RemoveAllDynamicControls();
            string taxType = cbTaxType.Text;
            string[] dynamicPropertyLabels = dynamicPropertyLabelMapping[taxType];
            string[] dynamicPropertyComboBoxLabels = dynamicPropertyComboBoxLabelMapping[taxType];
            AddDynamicControls(dynamicPropertyLabels);

            List<Bank> bankList = rptService.GetAllBanks();
            List<string> status = TaxStatus.STATUS.ToList();
            List<string> quarter = Quarter.ALL_QUARTER.ToList();

            List<string> bankNames = bankList.Select(bank => bank.BankName).ToList();
            AddDynamicComboBoxControls(dynamicPropertyComboBoxLabels, bankNames, status, quarter);

            AddDynamicDateTimeControls(dynamicPropertyDateTimeLabelMapping);
        }

        private void AddDynamicControls(string[] dynamicPropertyLabels)
        {
            int y = CONTROL_START_Y;

            foreach (string propertyLabel in dynamicPropertyLabels)
            {
                // Create the label above the textbox
                Label label = new Label();
                label.Top = y;
                label.Left = LABEL_START_X;
                label.Text = propertyLabel;
                label.Width = 250;
                label.Font = new Font("Tahoma", 12, FontStyle.Regular);
                label.ForeColor = Color.White;
                label.BackColor = Color.Transparent;
                dynamicLabelList.Add(label);
                Controls.Add(label);

                // Adjust the Y position for the textbox to be below the label
                y += CONTROL_HEIGHT_INCREMENT;

                TextBox textBox = new TextBox();
                textBox.Top = y;  // Place the textbox below the label
                textBox.BorderStyle = BorderStyle.FixedSingle;
                textBox.Left = TEXTBOX_START_X;
                textBox.Width = 350;
                textBox.Height = 28;
                textBox.Font = new Font("Tahoma", 12, FontStyle.Regular);
                textBox.TextAlign = HorizontalAlignment.Center;
                textBox.BackColor = Color.AliceBlue;
                dynamicTextboxList.Add(textBox);
                Controls.Add(textBox);

                // Increment the Y position for the next label-textbox pair
                y += CONTROL_HEIGHT_INCREMENT;
            }
        }

        private void AddDynamicComboBoxControls(string[] dynamicPropertyComboBoxLabels, List<string> banks, List<string> status, List<string> quarter)
        {
            int y = COMBOBOX_CONTROL_START_Y;

            foreach (string propertyLabel in dynamicPropertyComboBoxLabels)
            {
                // Create the label above the textbox
                Label label = new Label();
                label.Top = y;
                label.Left = COMBOBOX_LABEL_START_X;
                label.Text = propertyLabel;
                label.Width = 250;
                label.Font = new Font("Tahoma", 12, FontStyle.Regular);
                label.ForeColor = Color.White;
                label.BackColor = Color.Transparent;
                Controls.Add(label);

                // Adjust the Y position for the textbox to be below the label
                y += COMBOBOX_CONTROL_HEIGHT_INCREMENT;

                ComboBox comboBox = new ComboBox();
                comboBox.Top = y;  // Place the textbox below the label
                comboBox.Left = COMBOBOX_TEXTBOX_START_X;
                comboBox.Width = 350;
                comboBox.Height = 28;
                comboBox.Font = new Font("Tahoma", 12, FontStyle.Regular);
                comboBox.BackColor = Color.AliceBlue;

                if (propertyLabel == "Bank: ")
                {
                    comboBox.DataSource = banks;
                }
                else if (propertyLabel == "Status: ")
                {
                    //comboBox.DataSource = status;
                    comboBox.Text = TaxStatus.ForPaymentVerification;
                    comboBox.Enabled = false;
                }
                else if (propertyLabel == "Quarter: ")
                {
                    comboBox.DataSource = quarter;
                    comboBox.Width = 150;
                }

                dynamicComboBoxList.Add(comboBox);
                Controls.Add(comboBox);

                // Increment the Y position for the next label-textbox pair
                y += CONTROL_HEIGHT_INCREMENT;
            }
        }

        private void AddDynamicDateTimeControls(Dictionary<string, Dictionary<string, Type>> dynamicPropertyDateTimeLabelMapping)
        {
            int y = DATETIME_CONTROL_START_Y;

            foreach (var (taxType, propertyMapping) in dynamicPropertyDateTimeLabelMapping)
            {
                if (taxType == TaxTypeUtil.REALPROPERTYTAX)
                {
                    foreach (var (propertyLabel, dataType) in propertyMapping)
                    {
                        if (dataType == typeof(DateTime))
                        {
                            // Create the label above the DateTimePicker
                            Label label = new Label
                            {
                                Top = y,
                                Left = DATETIME_LABEL_START_X,
                                Text = propertyLabel,
                                Width = 250,
                                Font = new Font("Tahoma", 12, FontStyle.Regular),
                                ForeColor = Color.White,
                                BackColor = Color.Transparent
                            };
                            Controls.Add(label);

                            // Create the DateTimePicker control
                            DateTimePicker dateTimePicker = new DateTimePicker
                            {
                                Top = y + DATETIME_CONTROL_HEIGHT_INCREMENT,
                                Left = DATETIME_PICKER_START_X,
                                Width = 150,
                                Font = new Font("Tahoma", 12, FontStyle.Regular),
                                Format = DateTimePickerFormat.Short,
                                BackColor = Color.AliceBlue
                            };
                            dynamicDateTime.Add(dateTimePicker);
                            Controls.Add(dateTimePicker);

                            // Increment the Y position for the next label-DateTimePicker pair
                            y += DATETIME_CONTROL_HEIGHT_INCREMENT;
                        }
                    }
                }
            }
        }

        private void btnSaveRecord_Click(object sender, EventArgs e)
        {
            Rpt rpt = new Rpt();

            string[] dynamicPropertyNames = dynamicPropertyNameMapping[TaxTypeUtil.REALPROPERTYTAX];
            CopyDynamicProperties(rpt, dynamicPropertyNames);

            rptService.Insert(rpt);
            MessageBox.Show("Record successfully saved.");
        }

        private void CopyDynamicProperties(Rpt rpt, string[] dynamicPropertyNames)
        {
            for (int i = 0; i < dynamicPropertyNames.Length; i++)
            {
                string propertyName = dynamicPropertyNames[i];
                string value = dynamicTextboxList[i].Text;
                PropertyInfo propertyInfo = rpt.GetType().GetProperty(propertyName);

                if (propertyInfo.PropertyType == typeof(decimal?))
                {
                    decimal? decimalValue = decimal.Parse(value);
                    propertyInfo.SetValue(rpt, decimalValue);
                }
                else
                {
                    propertyInfo.SetValue(rpt, value);
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
}
