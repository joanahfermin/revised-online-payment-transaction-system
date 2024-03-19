namespace Inventory_System.Forms
{
    partial class ReleaseORForm
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ReleaseORForm));
            this.DgRptAddUpdateForm = new System.Windows.Forms.DataGridView();
            this.pbReceipt = new System.Windows.Forms.PictureBox();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnSaveRecord = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textRepName = new System.Windows.Forms.TextBox();
            this.textContactNum = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textRelesedBy = new System.Windows.Forms.TextBox();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.SearchLabel = new System.Windows.Forms.Label();
            this.tbSearch = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.DgRptAddUpdateForm)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbReceipt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // DgRptAddUpdateForm
            // 
            this.DgRptAddUpdateForm.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.DgRptAddUpdateForm.BackgroundColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.LightSalmon;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.MistyRose;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.Desktop;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.DgRptAddUpdateForm.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.DgRptAddUpdateForm.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(198)))), ((int)(((byte)(247)))));
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.DgRptAddUpdateForm.DefaultCellStyle = dataGridViewCellStyle2;
            this.DgRptAddUpdateForm.EnableHeadersVisualStyles = false;
            this.DgRptAddUpdateForm.Location = new System.Drawing.Point(12, 146);
            this.DgRptAddUpdateForm.Name = "DgRptAddUpdateForm";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.LightSalmon;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.MistyRose;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.DgRptAddUpdateForm.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.DgRptAddUpdateForm.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.LightBlue;
            this.DgRptAddUpdateForm.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.DgRptAddUpdateForm.RowTemplate.Height = 25;
            this.DgRptAddUpdateForm.Size = new System.Drawing.Size(1230, 836);
            this.DgRptAddUpdateForm.TabIndex = 205;
            // 
            // pbReceipt
            // 
            this.pbReceipt.BackColor = System.Drawing.Color.Transparent;
            this.pbReceipt.Location = new System.Drawing.Point(1255, 12);
            this.pbReceipt.Name = "pbReceipt";
            this.pbReceipt.Size = new System.Drawing.Size(650, 970);
            this.pbReceipt.TabIndex = 206;
            this.pbReceipt.TabStop = false;
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
            this.btnClose.Location = new System.Drawing.Point(1123, 29);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(126, 73);
            this.btnClose.TabIndex = 5;
            this.btnClose.Text = "Close";
            this.btnClose.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            this.btnClose.MouseEnter += new System.EventHandler(this.btnClose_MouseEnter);
            this.btnClose.MouseLeave += new System.EventHandler(this.btnClose_MouseLeave);
            // 
            // btnSaveRecord
            // 
            this.btnSaveRecord.BackColor = System.Drawing.Color.Transparent;
            this.btnSaveRecord.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnSaveRecord.BackgroundImage")));
            this.btnSaveRecord.FlatAppearance.BorderSize = 0;
            this.btnSaveRecord.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSaveRecord.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnSaveRecord.ForeColor = System.Drawing.Color.White;
            this.btnSaveRecord.Image = ((System.Drawing.Image)(resources.GetObject("btnSaveRecord.Image")));
            this.btnSaveRecord.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSaveRecord.Location = new System.Drawing.Point(933, 29);
            this.btnSaveRecord.Name = "btnSaveRecord";
            this.btnSaveRecord.Size = new System.Drawing.Size(184, 73);
            this.btnSaveRecord.TabIndex = 4;
            this.btnSaveRecord.Text = "Save Record";
            this.btnSaveRecord.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnSaveRecord.UseVisualStyleBackColor = false;
            this.btnSaveRecord.Click += new System.EventHandler(this.btnSaveRecord_Click);
            this.btnSaveRecord.MouseEnter += new System.EventHandler(this.btnSaveRecord_MouseEnter);
            this.btnSaveRecord.MouseLeave += new System.EventHandler(this.btnSaveRecord_MouseLeave);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(12, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(111, 125);
            this.pictureBox1.TabIndex = 209;
            this.pictureBox1.TabStop = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label3.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label3.Location = new System.Drawing.Point(128, 64);
            this.label3.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.label3.Name = "label3";
            this.label3.Padding = new System.Windows.Forms.Padding(10);
            this.label3.Size = new System.Drawing.Size(199, 38);
            this.label3.TabIndex = 212;
            this.label3.Text = "Representative Name: ";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // textRepName
            // 
            this.textRepName.BackColor = System.Drawing.Color.OldLace;
            this.textRepName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textRepName.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.textRepName.ForeColor = System.Drawing.Color.Black;
            this.textRepName.Location = new System.Drawing.Point(140, 104);
            this.textRepName.Name = "textRepName";
            this.textRepName.Size = new System.Drawing.Size(207, 26);
            this.textRepName.TabIndex = 1;
            this.textRepName.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textContactNum
            // 
            this.textContactNum.BackColor = System.Drawing.Color.OldLace;
            this.textContactNum.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textContactNum.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.textContactNum.ForeColor = System.Drawing.Color.Black;
            this.textContactNum.Location = new System.Drawing.Point(385, 104);
            this.textContactNum.Name = "textContactNum";
            this.textContactNum.Size = new System.Drawing.Size(207, 26);
            this.textContactNum.TabIndex = 2;
            this.textContactNum.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label1.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label1.Location = new System.Drawing.Point(373, 64);
            this.label1.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.label1.Name = "label1";
            this.label1.Padding = new System.Windows.Forms.Padding(10);
            this.label1.Size = new System.Drawing.Size(157, 38);
            this.label1.TabIndex = 212;
            this.label1.Text = "Contact Number: ";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label2.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label2.Location = new System.Drawing.Point(620, 64);
            this.label2.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.label2.Name = "label2";
            this.label2.Padding = new System.Windows.Forms.Padding(10);
            this.label2.Size = new System.Drawing.Size(129, 38);
            this.label2.TabIndex = 212;
            this.label2.Text = "Released By: ";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // textRelesedBy
            // 
            this.textRelesedBy.BackColor = System.Drawing.Color.OldLace;
            this.textRelesedBy.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textRelesedBy.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.textRelesedBy.ForeColor = System.Drawing.Color.Black;
            this.textRelesedBy.Location = new System.Drawing.Point(632, 104);
            this.textRelesedBy.Name = "textRelesedBy";
            this.textRelesedBy.Size = new System.Drawing.Size(207, 26);
            this.textRelesedBy.TabIndex = 3;
            this.textRelesedBy.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // errorProvider1
            // 
            this.errorProvider1.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
            this.errorProvider1.ContainerControl = this;
            this.errorProvider1.Icon = ((System.Drawing.Icon)(resources.GetObject("errorProvider1.Icon")));
            // 
            // SearchLabel
            // 
            this.SearchLabel.AutoSize = true;
            this.SearchLabel.BackColor = System.Drawing.Color.Transparent;
            this.SearchLabel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.SearchLabel.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.SearchLabel.ForeColor = System.Drawing.Color.White;
            this.SearchLabel.Image = ((System.Drawing.Image)(resources.GetObject("SearchLabel.Image")));
            this.SearchLabel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.SearchLabel.Location = new System.Drawing.Point(132, 18);
            this.SearchLabel.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.SearchLabel.Name = "SearchLabel";
            this.SearchLabel.Padding = new System.Windows.Forms.Padding(10);
            this.SearchLabel.Size = new System.Drawing.Size(125, 39);
            this.SearchLabel.TabIndex = 214;
            this.SearchLabel.Text = "       Search:";
            this.SearchLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbSearch
            // 
            this.tbSearch.BackColor = System.Drawing.Color.AliceBlue;
            this.tbSearch.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbSearch.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.tbSearch.Location = new System.Drawing.Point(263, 24);
            this.tbSearch.Name = "tbSearch";
            this.tbSearch.Size = new System.Drawing.Size(445, 27);
            this.tbSearch.TabIndex = 213;
            this.tbSearch.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // ReleaseORForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.ClientSize = new System.Drawing.Size(1924, 1004);
            this.Controls.Add(this.SearchLabel);
            this.Controls.Add(this.tbSearch);
            this.Controls.Add(this.textRepName);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textRelesedBy);
            this.Controls.Add(this.textContactNum);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnSaveRecord);
            this.Controls.Add(this.pbReceipt);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.DgRptAddUpdateForm);
            this.Controls.Add(this.label1);
            this.Name = "ReleaseORForm";
            this.Text = "ReleaseORForm";
            ((System.ComponentModel.ISupportInitialize)(this.DgRptAddUpdateForm)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbReceipt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DataGridView DgRptAddUpdateForm;
        private PictureBox pbReceipt;
        private Button btnClose;
        private Button btnSaveRecord;
        private PictureBox pictureBox1;
        private Label label3;
        private TextBox textRepName;
        private TextBox textContactNum;
        private Label label1;
        private Label label2;
        private TextBox textRelesedBy;
        private ErrorProvider errorProvider1;
        private Label SearchLabel;
        private TextBox tbSearch;
    }
}