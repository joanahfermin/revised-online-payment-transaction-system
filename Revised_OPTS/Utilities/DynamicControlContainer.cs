using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Inventory_System.Utilities
{
    internal class DynamicControlContainer
    {
        private const int CONTROL_HEIGHT_INCREMENT = 37;
        
        private const int CONTROL_START_Y = 185;
        
        private const int LABEL_START_X = 130;
        
        private const int LABEL_COL2_START_X = 630;

        private List<Label> dynamicLabelList = new List<Label>();
        
        private List<Control> dynamicControlList = new List<Control>();

        Dictionary<string, DynamicControlInfo[]> dynamicPropertyMapping = new Dictionary<string, DynamicControlInfo[]>();

        private Form containerForm;

        public DynamicControlContainer(Form containerForm)
        {
            this.containerForm = containerForm;
        }

        public void AddDynamicPropertyMapping(String key, DynamicControlInfo[] mappingInfoArrat)
        {
            dynamicPropertyMapping.Add(key, mappingInfoArrat);
        }

        public DynamicControlInfo[] GetDynamicPropertyMapping(String key)
        {
            return dynamicPropertyMapping[key];
        }

        public void RemoveAllDynamicControls()
        {
            foreach (Label label in dynamicLabelList)
            {
                containerForm.Controls.Remove(label);
            }
            foreach (Control textBox in dynamicControlList)
            {
                containerForm.Controls.Remove(textBox);
            }
        }

        public Control FindControlByName(String key, String propertyName)
        {
            DynamicControlInfo[] dynamicPropertyInfos = GetDynamicPropertyMapping(key);

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

        public DynamicControlInfo FindDynamicControlInfo(String key, String propertyName)
        {
            DynamicControlInfo[] dynamicPropertyInfos = GetDynamicPropertyMapping(key);
            for (int i = 0; i < dynamicPropertyInfos.Length; i++)
            {
                DynamicControlInfo dynamicPropertyInfo = dynamicPropertyInfos[i];
                if (propertyName == dynamicPropertyInfo.PropertyName)
                {
                    return dynamicPropertyInfo;
                }
            }
            return null;

        }

        public void AddDynamicControls(String key)
        {
            DynamicControlInfo[] dynamicPropertyInfos = GetDynamicPropertyMapping(key);

            int y = CONTROL_START_Y;
            int x = LABEL_START_X;
            int controlCounter = 0;

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
                containerForm.Controls.Add(label);

                // Adjust the Y position for the textbox to be below the label
                y += CONTROL_HEIGHT_INCREMENT;

                Control control = null;
                if (propertyInfo.ControlType == DynamicControlType.TextBox)
                {
                    textBox.BorderStyle = BorderStyle.FixedSingle;
                    textBox.TextAlign = HorizontalAlignment.Center;
                    control = textBox;
                }
                else if (propertyInfo.ControlType == DynamicControlType.ComboBox)
                {
                    ComboBox comboBox = new ComboBox();
                    comboBox.DataSource = propertyInfo.ComboboxChoices;
                    control = comboBox;
                }
                else if (propertyInfo.ControlType == DynamicControlType.DatePicker)
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
                containerForm.Controls.Add(control);

                // Increment the Y position for the next label-textbox pair
                y += CONTROL_HEIGHT_INCREMENT;
            }
        }

        public void validateForm(string key, ErrorProvider errorProvider1)
        {
            errorProvider1.Clear();

            DynamicControlInfo[] dynamicPropertyInfos = GetDynamicPropertyMapping(key);
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

        public void CopyDynamicProperties(Object obj, string key)
        {
            DynamicControlInfo[] dynamicPropertyInfos = GetDynamicPropertyMapping(key);

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

        public void PopulateDynamicControls(string key, Object obj)
        {
            DynamicControlInfo[] dynamicPropertyInfos = GetDynamicPropertyMapping(key);

            for (int i = 0; i < dynamicPropertyInfos.Length; i++)
            {
                DynamicControlInfo dynamicPropertyInfo = dynamicPropertyInfos[i];
                PropertyInfo propertyInfo = obj.GetType().GetProperty(dynamicPropertyInfo.PropertyName);
                object value = propertyInfo.GetValue(obj);

                if (propertyInfo.PropertyType == typeof(decimal?) || propertyInfo.PropertyType == typeof(decimal))
                {
                    dynamicControlList[i].Text = ((decimal)value).ToString();
                }
                else if (propertyInfo.PropertyType == typeof(DateTime?) || propertyInfo.PropertyType == typeof(DateTime))
                {
                    if (dynamicControlList[i] is DateTimePicker dateTimePicker)
                    {
                        dateTimePicker.Value = (DateTime)value;
                    }
                }
                else
                {
                    dynamicControlList[i].Text = value.ToString();
                }
            }
        }
    }
}
