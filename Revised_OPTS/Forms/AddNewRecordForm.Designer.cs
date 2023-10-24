namespace Revised_OPTS.Forms
{
    partial class AddNewRecordForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AddNewRecordForm));
            this.cbTaxType = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnSaveRecord = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.OPLogo = new System.Windows.Forms.PictureBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.OPLogo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // cbTaxType
            // 
            this.cbTaxType.BackColor = System.Drawing.Color.AliceBlue;
            this.cbTaxType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbTaxType.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbTaxType.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.cbTaxType.FormattingEnabled = true;
            this.cbTaxType.Location = new System.Drawing.Point(237, 51);
            this.cbTaxType.Name = "cbTaxType";
            this.cbTaxType.Size = new System.Drawing.Size(247, 27);
            this.cbTaxType.TabIndex = 1;
            this.cbTaxType.SelectedIndexChanged += new System.EventHandler(this.cbTaxType_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label1.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(98, 59);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(133, 19);
            this.label1.TabIndex = 1;
            this.label1.Text = "Choose tax type: ";
            // 
            // btnSaveRecord
            // 
            this.btnSaveRecord.BackColor = System.Drawing.Color.Transparent;
            this.btnSaveRecord.FlatAppearance.BorderSize = 0;
            this.btnSaveRecord.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSaveRecord.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnSaveRecord.ForeColor = System.Drawing.Color.White;
            this.btnSaveRecord.Image = ((System.Drawing.Image)(resources.GetObject("btnSaveRecord.Image")));
            this.btnSaveRecord.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSaveRecord.Location = new System.Drawing.Point(552, 37);
            this.btnSaveRecord.Name = "btnSaveRecord";
            this.btnSaveRecord.Size = new System.Drawing.Size(184, 53);
            this.btnSaveRecord.TabIndex = 4;
            this.btnSaveRecord.Text = "Save Record";
            this.btnSaveRecord.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnSaveRecord.UseVisualStyleBackColor = false;
            this.btnSaveRecord.Click += new System.EventHandler(this.btnSaveRecord_Click);
            this.btnSaveRecord.MouseEnter += new System.EventHandler(this.btnSaveRecord_MouseEnter);
            this.btnSaveRecord.MouseLeave += new System.EventHandler(this.btnSaveRecord_MouseLeave);
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.Transparent;
            this.btnClose.FlatAppearance.BorderSize = 0;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.Image = ((System.Drawing.Image)(resources.GetObject("btnClose.Image")));
            this.btnClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnClose.Location = new System.Drawing.Point(742, 37);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(126, 53);
            this.btnClose.TabIndex = 4;
            this.btnClose.Text = "Close";
            this.btnClose.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            this.btnClose.MouseEnter += new System.EventHandler(this.btnClose_MouseEnter);
            this.btnClose.MouseLeave += new System.EventHandler(this.btnClose_MouseLeave);
            // 
            // OPLogo
            // 
            this.OPLogo.BackColor = System.Drawing.Color.Transparent;
            this.OPLogo.Image = ((System.Drawing.Image)(resources.GetObject("OPLogo.Image")));
            this.OPLogo.Location = new System.Drawing.Point(976, 753);
            this.OPLogo.Name = "OPLogo";
            this.OPLogo.Size = new System.Drawing.Size(162, 186);
            this.OPLogo.TabIndex = 5;
            this.OPLogo.TabStop = false;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Transparent;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1150, 127);
            this.panel1.TabIndex = 6;
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            this.errorProvider1.Icon = ((System.Drawing.Icon)(resources.GetObject("errorProvider1.Icon")));
            // 
            // AddNewRecord
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.ClientSize = new System.Drawing.Size(1150, 951);
            this.Controls.Add(this.OPLogo);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnSaveRecord);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cbTaxType);
            this.Controls.Add(this.panel1);
            this.Name = "AddNewRecord";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "AddNewRecord";
            ((System.ComponentModel.ISupportInitialize)(this.OPLogo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ComboBox cbTaxType;
        private Label label1;
        private Button btnSaveRecord;
        private Button btnClose;
        private PictureBox OPLogo;
        private Panel panel1;
        private ErrorProvider errorProvider1;
    }
}