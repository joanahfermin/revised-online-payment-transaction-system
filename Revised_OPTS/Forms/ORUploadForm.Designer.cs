namespace Inventory_System.Forms
{
    partial class ORUploadForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ORUploadForm));
            this.rbElectronic = new System.Windows.Forms.RadioButton();
            this.rbRegular = new System.Windows.Forms.RadioButton();
            this.tbElectronicTaxDec = new System.Windows.Forms.TextBox();
            this.dtRegularDate = new System.Windows.Forms.DateTimePicker();
            this.cbRegularBank = new System.Windows.Forms.ComboBox();
            this.dgRptList = new System.Windows.Forms.DataGridView();
            this.cbRegularValBy = new System.Windows.Forms.ComboBox();
            this.pbVideoCapture = new System.Windows.Forms.PictureBox();
            this.pbReceipt = new System.Windows.Forms.PictureBox();
            this.btnSaveReceipt = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgRptList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbVideoCapture)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbReceipt)).BeginInit();
            this.SuspendLayout();
            // 
            // rbElectronic
            // 
            this.rbElectronic.AutoSize = true;
            this.rbElectronic.BackColor = System.Drawing.Color.Transparent;
            this.rbElectronic.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.rbElectronic.ForeColor = System.Drawing.Color.White;
            this.rbElectronic.Location = new System.Drawing.Point(236, 45);
            this.rbElectronic.Name = "rbElectronic";
            this.rbElectronic.Size = new System.Drawing.Size(107, 23);
            this.rbElectronic.TabIndex = 0;
            this.rbElectronic.TabStop = true;
            this.rbElectronic.Text = "Electronic";
            this.rbElectronic.UseVisualStyleBackColor = false;
            this.rbElectronic.CheckedChanged += new System.EventHandler(this.rbElectronic_CheckedChanged);
            // 
            // rbRegular
            // 
            this.rbRegular.AutoSize = true;
            this.rbRegular.BackColor = System.Drawing.Color.Transparent;
            this.rbRegular.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.rbRegular.ForeColor = System.Drawing.Color.White;
            this.rbRegular.Location = new System.Drawing.Point(236, 125);
            this.rbRegular.Name = "rbRegular";
            this.rbRegular.Size = new System.Drawing.Size(91, 23);
            this.rbRegular.TabIndex = 1;
            this.rbRegular.TabStop = true;
            this.rbRegular.Text = "Regular";
            this.rbRegular.UseVisualStyleBackColor = false;
            this.rbRegular.CheckedChanged += new System.EventHandler(this.rbRegular_CheckedChanged);
            // 
            // tbElectronicTaxDec
            // 
            this.tbElectronicTaxDec.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.tbElectronicTaxDec.Location = new System.Drawing.Point(349, 45);
            this.tbElectronicTaxDec.Name = "tbElectronicTaxDec";
            this.tbElectronicTaxDec.Size = new System.Drawing.Size(200, 27);
            this.tbElectronicTaxDec.TabIndex = 2;
            this.tbElectronicTaxDec.KeyDown += new System.Windows.Forms.KeyEventHandler(this.SearchRecordsOnEnterKey);
            this.tbElectronicTaxDec.Leave += new System.EventHandler(this.SearchRecords);
            // 
            // dtRegularDate
            // 
            this.dtRegularDate.CustomFormat = "MM/dd/yyyy";
            this.dtRegularDate.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.dtRegularDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtRegularDate.Location = new System.Drawing.Point(349, 125);
            this.dtRegularDate.Name = "dtRegularDate";
            this.dtRegularDate.Size = new System.Drawing.Size(136, 27);
            this.dtRegularDate.TabIndex = 4;
            this.dtRegularDate.ValueChanged += new System.EventHandler(this.SearchRecords);
            // 
            // cbRegularBank
            // 
            this.cbRegularBank.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.cbRegularBank.FormattingEnabled = true;
            this.cbRegularBank.Location = new System.Drawing.Point(508, 125);
            this.cbRegularBank.Name = "cbRegularBank";
            this.cbRegularBank.Size = new System.Drawing.Size(319, 27);
            this.cbRegularBank.TabIndex = 5;
            this.cbRegularBank.SelectedIndexChanged += new System.EventHandler(this.SearchRecords);
            // 
            // dgRptList
            // 
            this.dgRptList.BackgroundColor = System.Drawing.Color.AliceBlue;
            this.dgRptList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgRptList.Location = new System.Drawing.Point(29, 174);
            this.dgRptList.Name = "dgRptList";
            this.dgRptList.ReadOnly = true;
            this.dgRptList.RowTemplate.Height = 25;
            this.dgRptList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgRptList.Size = new System.Drawing.Size(971, 822);
            this.dgRptList.TabIndex = 10;
            this.dgRptList.CurrentCellChanged += new System.EventHandler(this.dgRptList_CellClick);
            // 
            // cbRegularValBy
            // 
            this.cbRegularValBy.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.cbRegularValBy.FormattingEnabled = true;
            this.cbRegularValBy.Location = new System.Drawing.Point(849, 125);
            this.cbRegularValBy.Name = "cbRegularValBy";
            this.cbRegularValBy.Size = new System.Drawing.Size(151, 27);
            this.cbRegularValBy.TabIndex = 11;
            this.cbRegularValBy.SelectedIndexChanged += new System.EventHandler(this.SearchRecords);
            // 
            // pbVideoCapture
            // 
            this.pbVideoCapture.BackColor = System.Drawing.Color.Transparent;
            this.pbVideoCapture.Location = new System.Drawing.Point(1026, 535);
            this.pbVideoCapture.Name = "pbVideoCapture";
            this.pbVideoCapture.Size = new System.Drawing.Size(741, 461);
            this.pbVideoCapture.TabIndex = 12;
            this.pbVideoCapture.TabStop = false;
            // 
            // pbReceipt
            // 
            this.pbReceipt.BackColor = System.Drawing.Color.Transparent;
            this.pbReceipt.Location = new System.Drawing.Point(1026, 61);
            this.pbReceipt.Name = "pbReceipt";
            this.pbReceipt.Size = new System.Drawing.Size(741, 420);
            this.pbReceipt.TabIndex = 14;
            this.pbReceipt.TabStop = false;
            // 
            // btnSaveReceipt
            // 
            this.btnSaveReceipt.Location = new System.Drawing.Point(659, 47);
            this.btnSaveReceipt.Name = "btnSaveReceipt";
            this.btnSaveReceipt.Size = new System.Drawing.Size(105, 23);
            this.btnSaveReceipt.TabIndex = 16;
            this.btnSaveReceipt.Text = "Save Receipt";
            this.btnSaveReceipt.UseVisualStyleBackColor = true;
            this.btnSaveReceipt.Click += new System.EventHandler(this.btnSaveReceipt_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label7.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label7.ForeColor = System.Drawing.Color.White;
            this.label7.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label7.Location = new System.Drawing.Point(334, 6);
            this.label7.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.label7.Name = "label7";
            this.label7.Padding = new System.Windows.Forms.Padding(10);
            this.label7.Size = new System.Drawing.Size(239, 39);
            this.label7.TabIndex = 201;
            this.label7.Text = "Tax Declaration Number: ";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label1.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label1.Location = new System.Drawing.Point(334, 86);
            this.label1.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.label1.Name = "label1";
            this.label1.Padding = new System.Windows.Forms.Padding(10);
            this.label1.Size = new System.Drawing.Size(79, 39);
            this.label1.TabIndex = 201;
            this.label1.Text = "Date: ";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label2.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label2.Location = new System.Drawing.Point(495, 86);
            this.label2.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.label2.Name = "label2";
            this.label2.Padding = new System.Windows.Forms.Padding(10);
            this.label2.Size = new System.Drawing.Size(76, 39);
            this.label2.TabIndex = 201;
            this.label2.Text = "Bank:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label3.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label3.Location = new System.Drawing.Point(833, 86);
            this.label3.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.label3.Name = "label3";
            this.label3.Padding = new System.Windows.Forms.Padding(10);
            this.label3.Size = new System.Drawing.Size(138, 39);
            this.label3.TabIndex = 201;
            this.label3.Text = "Validated By:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label4.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label4.Location = new System.Drawing.Point(1010, 14);
            this.label4.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.label4.Name = "label4";
            this.label4.Padding = new System.Windows.Forms.Padding(10);
            this.label4.Size = new System.Drawing.Size(168, 39);
            this.label4.TabIndex = 201;
            this.label4.Text = "Receipt Preview:";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label6.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label6.ForeColor = System.Drawing.Color.White;
            this.label6.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label6.Location = new System.Drawing.Point(1010, 488);
            this.label6.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.label6.Name = "label6";
            this.label6.Padding = new System.Windows.Forms.Padding(10);
            this.label6.Size = new System.Drawing.Size(169, 39);
            this.label6.TabIndex = 201;
            this.label6.Text = "Camera Preview:";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ORUploadForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.ClientSize = new System.Drawing.Size(1796, 1024);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.btnSaveReceipt);
            this.Controls.Add(this.pbReceipt);
            this.Controls.Add(this.pbVideoCapture);
            this.Controls.Add(this.cbRegularValBy);
            this.Controls.Add(this.dgRptList);
            this.Controls.Add(this.cbRegularBank);
            this.Controls.Add(this.dtRegularDate);
            this.Controls.Add(this.tbElectronicTaxDec);
            this.Controls.Add(this.rbRegular);
            this.Controls.Add(this.rbElectronic);
            this.Name = "ORUploadForm";
            this.Text = "ORUploadForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ORUploadForm_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.dgRptList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbVideoCapture)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbReceipt)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private RadioButton rbElectronic;
        private RadioButton rbRegular;
        private TextBox tbElectronicTaxDec;
        private DateTimePicker dtRegularDate;
        private ComboBox cbRegularBank;
        private DataGridView dgRptList;
        private ComboBox cbRegularValBy;
        private PictureBox pbVideoCapture;
        private PictureBox pbReceipt;
        private Button btnSaveReceipt;
        private Label label7;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label6;
    }
}