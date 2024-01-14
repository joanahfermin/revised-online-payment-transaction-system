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
            this.rbElectronic = new System.Windows.Forms.RadioButton();
            this.rbRegular = new System.Windows.Forms.RadioButton();
            this.tbElectronicTaxDec = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.dtRegularDate = new System.Windows.Forms.DateTimePicker();
            this.cbRegularBank = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.dgRptList = new System.Windows.Forms.DataGridView();
            this.cbRegularValBy = new System.Windows.Forms.ComboBox();
            this.pbVideoCapture = new System.Windows.Forms.PictureBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.pbReceipt = new System.Windows.Forms.PictureBox();
            this.btnSaveReceipt = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgRptList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbVideoCapture)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbReceipt)).BeginInit();
            this.SuspendLayout();
            // 
            // rbElectronic
            // 
            this.rbElectronic.AutoSize = true;
            this.rbElectronic.Location = new System.Drawing.Point(29, 21);
            this.rbElectronic.Name = "rbElectronic";
            this.rbElectronic.Size = new System.Drawing.Size(77, 19);
            this.rbElectronic.TabIndex = 0;
            this.rbElectronic.TabStop = true;
            this.rbElectronic.Text = "Electronic";
            this.rbElectronic.UseVisualStyleBackColor = true;
            this.rbElectronic.CheckedChanged += new System.EventHandler(this.rbElectronic_CheckedChanged);
            // 
            // rbRegular
            // 
            this.rbRegular.AutoSize = true;
            this.rbRegular.Location = new System.Drawing.Point(29, 88);
            this.rbRegular.Name = "rbRegular";
            this.rbRegular.Size = new System.Drawing.Size(65, 19);
            this.rbRegular.TabIndex = 1;
            this.rbRegular.TabStop = true;
            this.rbRegular.Text = "Regular";
            this.rbRegular.UseVisualStyleBackColor = true;
            this.rbRegular.CheckedChanged += new System.EventHandler(this.rbRegular_CheckedChanged);
            // 
            // tbElectronicTaxDec
            // 
            this.tbElectronicTaxDec.Location = new System.Drawing.Point(187, 17);
            this.tbElectronicTaxDec.Name = "tbElectronicTaxDec";
            this.tbElectronicTaxDec.Size = new System.Drawing.Size(154, 23);
            this.tbElectronicTaxDec.TabIndex = 2;
            this.tbElectronicTaxDec.KeyDown += new System.Windows.Forms.KeyEventHandler(this.SearchRecordsOnEnterKey);
            this.tbElectronicTaxDec.Leave += new System.EventHandler(this.SearchRecords);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(137, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 15);
            this.label1.TabIndex = 3;
            this.label1.Text = "TaxDec";
            // 
            // dtRegularDate
            // 
            this.dtRegularDate.CustomFormat = "MM/dd/yyyy";
            this.dtRegularDate.Location = new System.Drawing.Point(137, 84);
            this.dtRegularDate.Name = "dtRegularDate";
            this.dtRegularDate.Size = new System.Drawing.Size(200, 23);
            this.dtRegularDate.TabIndex = 4;
            this.dtRegularDate.ValueChanged += new System.EventHandler(this.SearchRecords);
            // 
            // cbRegularBank
            // 
            this.cbRegularBank.FormattingEnabled = true;
            this.cbRegularBank.Location = new System.Drawing.Point(370, 84);
            this.cbRegularBank.Name = "cbRegularBank";
            this.cbRegularBank.Size = new System.Drawing.Size(121, 23);
            this.cbRegularBank.TabIndex = 5;
            this.cbRegularBank.SelectedIndexChanged += new System.EventHandler(this.SearchRecords);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(137, 63);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(31, 15);
            this.label2.TabIndex = 6;
            this.label2.Text = "Date";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(370, 63);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(33, 15);
            this.label3.TabIndex = 7;
            this.label3.Text = "Bank";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(520, 63);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(35, 15);
            this.label4.TabIndex = 8;
            this.label4.Text = "ValBy";
            // 
            // dgRptList
            // 
            this.dgRptList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgRptList.Location = new System.Drawing.Point(29, 113);
            this.dgRptList.Name = "dgRptList";
            this.dgRptList.ReadOnly = true;
            this.dgRptList.RowTemplate.Height = 25;
            this.dgRptList.Size = new System.Drawing.Size(815, 717);
            this.dgRptList.TabIndex = 10;
            this.dgRptList.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgRptList_CellClick);
            this.dgRptList.CurrentCellChanged += new System.EventHandler(this.dgRptList_CellClick);
            // 
            // cbRegularValBy
            // 
            this.cbRegularValBy.FormattingEnabled = true;
            this.cbRegularValBy.Location = new System.Drawing.Point(520, 84);
            this.cbRegularValBy.Name = "cbRegularValBy";
            this.cbRegularValBy.Size = new System.Drawing.Size(121, 23);
            this.cbRegularValBy.TabIndex = 11;
            this.cbRegularValBy.SelectedIndexChanged += new System.EventHandler(this.SearchRecords);
            // 
            // pbVideoCapture
            // 
            this.pbVideoCapture.Location = new System.Drawing.Point(1046, 352);
            this.pbVideoCapture.Name = "pbVideoCapture";
            this.pbVideoCapture.Size = new System.Drawing.Size(721, 388);
            this.pbVideoCapture.TabIndex = 12;
            this.pbVideoCapture.TabStop = false;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(979, 352);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(48, 15);
            this.label5.TabIndex = 13;
            this.label5.Text = "Camera";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(865, 21);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(46, 15);
            this.label6.TabIndex = 15;
            this.label6.Text = "Receipt";
            // 
            // pbReceipt
            // 
            this.pbReceipt.Location = new System.Drawing.Point(1046, 21);
            this.pbReceipt.Name = "pbReceipt";
            this.pbReceipt.Size = new System.Drawing.Size(721, 311);
            this.pbReceipt.TabIndex = 14;
            this.pbReceipt.TabStop = false;
            // 
            // btnSaveReceipt
            // 
            this.btnSaveReceipt.Location = new System.Drawing.Point(1046, 767);
            this.btnSaveReceipt.Name = "btnSaveReceipt";
            this.btnSaveReceipt.Size = new System.Drawing.Size(105, 23);
            this.btnSaveReceipt.TabIndex = 16;
            this.btnSaveReceipt.Text = "Save Receipt";
            this.btnSaveReceipt.UseVisualStyleBackColor = true;
            this.btnSaveReceipt.Click += new System.EventHandler(this.btnSaveReceipt_Click);
            // 
            // ORUploadForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1796, 842);
            this.Controls.Add(this.btnSaveReceipt);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.pbReceipt);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.pbVideoCapture);
            this.Controls.Add(this.cbRegularValBy);
            this.Controls.Add(this.dgRptList);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cbRegularBank);
            this.Controls.Add(this.dtRegularDate);
            this.Controls.Add(this.label1);
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
        private Label label1;
        private DateTimePicker dtRegularDate;
        private ComboBox cbRegularBank;
        private Label label2;
        private Label label3;
        private Label label4;
        private DataGridView dgRptList;
        private ComboBox cbRegularValBy;
        private PictureBox pbVideoCapture;
        private Label label5;
        private Label label6;
        private PictureBox pbReceipt;
        private Button btnSaveReceipt;
    }
}