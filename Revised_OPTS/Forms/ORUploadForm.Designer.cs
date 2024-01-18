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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ORUploadForm));
            this.rbElectronic = new System.Windows.Forms.RadioButton();
            this.rbRegular = new System.Windows.Forms.RadioButton();
            this.tbElectronicTaxDec = new System.Windows.Forms.TextBox();
            this.dtRegularDate = new System.Windows.Forms.DateTimePicker();
            this.dgRptList = new System.Windows.Forms.DataGridView();
            this.cbRegularValBy = new System.Windows.Forms.ComboBox();
            this.pbVideoCapture = new System.Windows.Forms.PictureBox();
            this.pbReceipt = new System.Windows.Forms.PictureBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnUploadReceipt = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.cbRegularBank = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgRptList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbVideoCapture)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbReceipt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // rbElectronic
            // 
            this.rbElectronic.AutoSize = true;
            this.rbElectronic.BackColor = System.Drawing.Color.Transparent;
            this.rbElectronic.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.rbElectronic.ForeColor = System.Drawing.Color.White;
            this.rbElectronic.Location = new System.Drawing.Point(155, 43);
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
            this.rbRegular.Location = new System.Drawing.Point(155, 82);
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
            this.tbElectronicTaxDec.Location = new System.Drawing.Point(283, 44);
            this.tbElectronicTaxDec.Name = "tbElectronicTaxDec";
            this.tbElectronicTaxDec.Size = new System.Drawing.Size(200, 27);
            this.tbElectronicTaxDec.TabIndex = 2;
            this.tbElectronicTaxDec.KeyDown += new System.Windows.Forms.KeyEventHandler(this.SearchRecordsOnEnterKey);
            // 
            // dtRegularDate
            // 
            this.dtRegularDate.CustomFormat = "MM/dd/yyyy";
            this.dtRegularDate.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.dtRegularDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtRegularDate.Location = new System.Drawing.Point(283, 110);
            this.dtRegularDate.Name = "dtRegularDate";
            this.dtRegularDate.Size = new System.Drawing.Size(136, 27);
            this.dtRegularDate.TabIndex = 4;
            this.dtRegularDate.ValueChanged += new System.EventHandler(this.SearchRecords);
            // 
            // dgRptList
            // 
            this.dgRptList.AllowUserToAddRows = false;
            this.dgRptList.AllowUserToDeleteRows = false;
            this.dgRptList.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgRptList.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCells;
            this.dgRptList.BackgroundColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.DarkSalmon;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.LightSalmon;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgRptList.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgRptList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(136)))), ((int)(((byte)(136)))), ((int)(((byte)(136)))));
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(198)))), ((int)(((byte)(247)))));
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgRptList.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgRptList.EnableHeadersVisualStyles = false;
            this.dgRptList.GridColor = System.Drawing.Color.DarkGray;
            this.dgRptList.Location = new System.Drawing.Point(12, 150);
            this.dgRptList.Name = "dgRptList";
            this.dgRptList.ReadOnly = true;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(174)))), ((int)(((byte)(219)))));
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(198)))), ((int)(((byte)(247)))));
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgRptList.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dgRptList.RowHeadersVisible = false;
            this.dgRptList.RowTemplate.DefaultCellStyle.BackColor = System.Drawing.Color.White;
            this.dgRptList.RowTemplate.DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
            this.dgRptList.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.LightBlue;
            this.dgRptList.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.dgRptList.RowTemplate.Height = 25;
            this.dgRptList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgRptList.Size = new System.Drawing.Size(992, 846);
            this.dgRptList.TabIndex = 10;
            this.dgRptList.CurrentCellChanged += new System.EventHandler(this.dgRptList_CellClick);
            // 
            // cbRegularValBy
            // 
            this.cbRegularValBy.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.cbRegularValBy.FormattingEnabled = true;
            this.cbRegularValBy.Location = new System.Drawing.Point(444, 110);
            this.cbRegularValBy.Name = "cbRegularValBy";
            this.cbRegularValBy.Size = new System.Drawing.Size(187, 27);
            this.cbRegularValBy.TabIndex = 11;
            this.cbRegularValBy.SelectedIndexChanged += new System.EventHandler(this.SearchRecords);
            // 
            // pbVideoCapture
            // 
            this.pbVideoCapture.BackColor = System.Drawing.Color.Transparent;
            this.pbVideoCapture.Location = new System.Drawing.Point(1026, 584);
            this.pbVideoCapture.Name = "pbVideoCapture";
            this.pbVideoCapture.Size = new System.Drawing.Size(875, 412);
            this.pbVideoCapture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbVideoCapture.TabIndex = 12;
            this.pbVideoCapture.TabStop = false;
            // 
            // pbReceipt
            // 
            this.pbReceipt.BackColor = System.Drawing.Color.Transparent;
            this.pbReceipt.Location = new System.Drawing.Point(1026, 61);
            this.pbReceipt.Name = "pbReceipt";
            this.pbReceipt.Size = new System.Drawing.Size(875, 468);
            this.pbReceipt.TabIndex = 14;
            this.pbReceipt.TabStop = false;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label7.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label7.ForeColor = System.Drawing.Color.White;
            this.label7.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label7.Location = new System.Drawing.Point(268, 8);
            this.label7.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.label7.Name = "label7";
            this.label7.Padding = new System.Windows.Forms.Padding(10);
            this.label7.Size = new System.Drawing.Size(218, 38);
            this.label7.TabIndex = 201;
            this.label7.Text = "Tax Declaration Number: ";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label1.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label1.Location = new System.Drawing.Point(268, 76);
            this.label1.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.label1.Name = "label1";
            this.label1.Padding = new System.Windows.Forms.Padding(10);
            this.label1.Size = new System.Drawing.Size(72, 38);
            this.label1.TabIndex = 201;
            this.label1.Text = "Date: ";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label3.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label3.Location = new System.Drawing.Point(428, 76);
            this.label3.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.label3.Name = "label3";
            this.label3.Padding = new System.Windows.Forms.Padding(10);
            this.label3.Size = new System.Drawing.Size(127, 38);
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
            this.label6.Location = new System.Drawing.Point(1010, 537);
            this.label6.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.label6.Name = "label6";
            this.label6.Padding = new System.Windows.Forms.Padding(10);
            this.label6.Size = new System.Drawing.Size(169, 39);
            this.label6.TabIndex = 201;
            this.label6.Text = "Camera Preview:";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(12, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(111, 125);
            this.pictureBox1.TabIndex = 202;
            this.pictureBox1.TabStop = false;
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.Transparent;
            this.btnClose.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnClose.BackgroundImage")));
            this.btnClose.FlatAppearance.BorderSize = 0;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.Image = ((System.Drawing.Image)(resources.GetObject("btnClose.Image")));
            this.btnClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnClose.Location = new System.Drawing.Point(878, 71);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(126, 73);
            this.btnClose.TabIndex = 203;
            this.btnClose.Text = "Close";
            this.btnClose.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            this.btnClose.MouseEnter += new System.EventHandler(this.btnClose_MouseEnter);
            this.btnClose.MouseLeave += new System.EventHandler(this.btnClose_MouseLeave);
            // 
            // btnUploadReceipt
            // 
            this.btnUploadReceipt.BackColor = System.Drawing.Color.Transparent;
            this.btnUploadReceipt.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnUploadReceipt.BackgroundImage")));
            this.btnUploadReceipt.FlatAppearance.BorderSize = 0;
            this.btnUploadReceipt.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUploadReceipt.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnUploadReceipt.ForeColor = System.Drawing.Color.White;
            this.btnUploadReceipt.Image = ((System.Drawing.Image)(resources.GetObject("btnUploadReceipt.Image")));
            this.btnUploadReceipt.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnUploadReceipt.Location = new System.Drawing.Point(671, 71);
            this.btnUploadReceipt.Name = "btnUploadReceipt";
            this.btnUploadReceipt.Size = new System.Drawing.Size(201, 73);
            this.btnUploadReceipt.TabIndex = 204;
            this.btnUploadReceipt.Text = "Upload Receipt";
            this.btnUploadReceipt.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnUploadReceipt.UseVisualStyleBackColor = false;
            this.btnUploadReceipt.Click += new System.EventHandler(this.btnUploadReceipt_Click);
            this.btnUploadReceipt.MouseEnter += new System.EventHandler(this.btnUploadReceipt_MouseEnter);
            this.btnUploadReceipt.MouseLeave += new System.EventHandler(this.btnUploadReceipt_MouseLeave);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label2.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label2.Location = new System.Drawing.Point(456, 229);
            this.label2.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.label2.Name = "label2";
            this.label2.Padding = new System.Windows.Forms.Padding(10);
            this.label2.Size = new System.Drawing.Size(70, 38);
            this.label2.TabIndex = 201;
            this.label2.Text = "Bank:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cbRegularBank
            // 
            this.cbRegularBank.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.cbRegularBank.FormattingEnabled = true;
            this.cbRegularBank.Location = new System.Drawing.Point(467, 263);
            this.cbRegularBank.Name = "cbRegularBank";
            this.cbRegularBank.Size = new System.Drawing.Size(319, 27);
            this.cbRegularBank.TabIndex = 5;
            // 
            // ORUploadForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.ClientSize = new System.Drawing.Size(1924, 1024);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnUploadReceipt);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.pbReceipt);
            this.Controls.Add(this.pbVideoCapture);
            this.Controls.Add(this.cbRegularValBy);
            this.Controls.Add(this.dgRptList);
            this.Controls.Add(this.cbRegularBank);
            this.Controls.Add(this.dtRegularDate);
            this.Controls.Add(this.tbElectronicTaxDec);
            this.Controls.Add(this.rbRegular);
            this.Controls.Add(this.rbElectronic);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label7);
            this.Name = "ORUploadForm";
            this.Text = "ORUploadForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ORUploadForm_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.dgRptList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbVideoCapture)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbReceipt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private RadioButton rbElectronic;
        private RadioButton rbRegular;
        private TextBox tbElectronicTaxDec;
        private DateTimePicker dtRegularDate;
        private DataGridView dgRptList;
        private ComboBox cbRegularValBy;
        private PictureBox pbVideoCapture;
        private PictureBox pbReceipt;
        private Label label7;
        private Label label1;
        private Label label3;
        private Label label4;
        private Label label6;
        private PictureBox pictureBox1;
        private Button btnClose;
        private Button btnUploadReceipt;
        private Label label2;
        private ComboBox cbRegularBank;
    }
}