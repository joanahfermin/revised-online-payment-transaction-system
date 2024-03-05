namespace Inventory_System.Forms
{
    partial class EmailTemplateForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EmailTemplateForm));
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.tbEditorEmailTemp = new DG.MiniHTMLTextBox.MiniHTMLTextBox();
            this.btnSaveRecord = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
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
            // tbEditorEmailTemp
            // 
            this.tbEditorEmailTemp.IllegalPatterns = new string[] {
        "<script.*?>",
        "<\\w+\\s+.*?(j|java|vb|ecma)script:.*?>",
        "<\\w+(\\s+|\\s+.*?\\s+)on\\w+\\s*=.+?>",
        "</?input.*?>"};
            this.tbEditorEmailTemp.Location = new System.Drawing.Point(13, 143);
            this.tbEditorEmailTemp.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.tbEditorEmailTemp.Name = "tbEditorEmailTemp";
            this.tbEditorEmailTemp.Padding = new System.Windows.Forms.Padding(1);
            this.tbEditorEmailTemp.Size = new System.Drawing.Size(1005, 655);
            this.tbEditorEmailTemp.TabIndex = 203;
            this.tbEditorEmailTemp.Text = null;
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
            this.btnSaveRecord.Location = new System.Drawing.Point(702, 38);
            this.btnSaveRecord.Name = "btnSaveRecord";
            this.btnSaveRecord.Size = new System.Drawing.Size(184, 73);
            this.btnSaveRecord.TabIndex = 205;
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
            this.btnClose.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnClose.BackgroundImage")));
            this.btnClose.FlatAppearance.BorderSize = 0;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.Image = ((System.Drawing.Image)(resources.GetObject("btnClose.Image")));
            this.btnClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnClose.Location = new System.Drawing.Point(892, 38);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(126, 73);
            this.btnClose.TabIndex = 204;
            this.btnClose.Text = "Close";
            this.btnClose.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            this.btnClose.MouseEnter += new System.EventHandler(this.btnClose_MouseEnter);
            this.btnClose.MouseLeave += new System.EventHandler(this.btnClose_MouseLeave);
            // 
            // EmailTemplateForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.ClientSize = new System.Drawing.Size(1031, 810);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnSaveRecord);
            this.Controls.Add(this.tbEditorEmailTemp);
            this.Controls.Add(this.pictureBox1);
            this.ForeColor = System.Drawing.SystemColors.Control;
            this.Name = "EmailTemplateForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "EmailTemplateForm";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private PictureBox pictureBox1;
        private DG.MiniHTMLTextBox.MiniHTMLTextBox tbEditorEmailTemp;
        private Button btnSaveRecord;
        private Button btnClose;
    }
}