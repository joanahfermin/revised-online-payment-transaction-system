namespace Revised_OPTS
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            this.MainDGRightClick = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.testToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SearchLabel = new System.Windows.Forms.Label();
            this.tbSearch = new System.Windows.Forms.TextBox();
            this.btnNonRptAddNewRecord = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.btnAddRptRecord = new System.Windows.Forms.Button();
            this.tbTotalAmountTransferred = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tbRecordSelected = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tbTotalBillAmount = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnORUpload = new System.Windows.Forms.Button();
            this.btnAddEpayments = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.btnReport = new System.Windows.Forms.Button();
            this.DgMainForm = new System.Windows.Forms.DataGridView();
            this.label5 = new System.Windows.Forms.Label();
            this.tbMailToSendCount = new System.Windows.Forms.TextBox();
            this.btnAssignLocCode = new System.Windows.Forms.Button();
            this.btnEmailTemp = new System.Windows.Forms.Button();
            this.checkSearchByEmailAdd = new System.Windows.Forms.CheckBox();
            this.MainDGRightClick.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DgMainForm)).BeginInit();
            this.SuspendLayout();
            // 
            // MainDGRightClick
            // 
            this.MainDGRightClick.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.testToolStripMenuItem});
            this.MainDGRightClick.Name = "contextMenuStrip1";
            this.MainDGRightClick.Size = new System.Drawing.Size(94, 26);
            // 
            // testToolStripMenuItem
            // 
            this.testToolStripMenuItem.Name = "testToolStripMenuItem";
            this.testToolStripMenuItem.Size = new System.Drawing.Size(93, 22);
            this.testToolStripMenuItem.Text = "test";
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
            this.SearchLabel.Location = new System.Drawing.Point(140, 38);
            this.SearchLabel.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.SearchLabel.Name = "SearchLabel";
            this.SearchLabel.Padding = new System.Windows.Forms.Padding(10);
            this.SearchLabel.Size = new System.Drawing.Size(125, 39);
            this.SearchLabel.TabIndex = 200;
            this.SearchLabel.Text = "       Search:";
            this.SearchLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbSearch
            // 
            this.tbSearch.BackColor = System.Drawing.Color.AliceBlue;
            this.tbSearch.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbSearch.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.tbSearch.Location = new System.Drawing.Point(271, 46);
            this.tbSearch.Name = "tbSearch";
            this.tbSearch.Size = new System.Drawing.Size(445, 27);
            this.tbSearch.TabIndex = 1;
            this.tbSearch.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.tbSearch.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbSearch_KeyDown);
            // 
            // btnNonRptAddNewRecord
            // 
            this.btnNonRptAddNewRecord.BackColor = System.Drawing.Color.Transparent;
            this.btnNonRptAddNewRecord.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnNonRptAddNewRecord.BackgroundImage")));
            this.btnNonRptAddNewRecord.FlatAppearance.BorderSize = 0;
            this.btnNonRptAddNewRecord.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNonRptAddNewRecord.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnNonRptAddNewRecord.ForeColor = System.Drawing.Color.White;
            this.btnNonRptAddNewRecord.Image = ((System.Drawing.Image)(resources.GetObject("btnNonRptAddNewRecord.Image")));
            this.btnNonRptAddNewRecord.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnNonRptAddNewRecord.Location = new System.Drawing.Point(722, 12);
            this.btnNonRptAddNewRecord.Name = "btnNonRptAddNewRecord";
            this.btnNonRptAddNewRecord.Size = new System.Drawing.Size(138, 87);
            this.btnNonRptAddNewRecord.TabIndex = 3;
            this.btnNonRptAddNewRecord.Text = "Add Record";
            this.btnNonRptAddNewRecord.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnNonRptAddNewRecord.UseVisualStyleBackColor = false;
            this.btnNonRptAddNewRecord.Click += new System.EventHandler(this.btnAddNewRecord_Click);
            this.btnNonRptAddNewRecord.MouseEnter += new System.EventHandler(this.btnAddNewRecord_MouseEnter);
            this.btnNonRptAddNewRecord.MouseLeave += new System.EventHandler(this.btnAddNewRecord_MouseLeave);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(22, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(111, 125);
            this.pictureBox1.TabIndex = 201;
            this.pictureBox1.TabStop = false;
            // 
            // btnAddRptRecord
            // 
            this.btnAddRptRecord.BackColor = System.Drawing.Color.Transparent;
            this.btnAddRptRecord.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnAddRptRecord.BackgroundImage")));
            this.btnAddRptRecord.FlatAppearance.BorderSize = 0;
            this.btnAddRptRecord.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddRptRecord.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnAddRptRecord.Image = ((System.Drawing.Image)(resources.GetObject("btnAddRptRecord.Image")));
            this.btnAddRptRecord.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnAddRptRecord.Location = new System.Drawing.Point(859, 12);
            this.btnAddRptRecord.Name = "btnAddRptRecord";
            this.btnAddRptRecord.Size = new System.Drawing.Size(180, 87);
            this.btnAddRptRecord.TabIndex = 4;
            this.btnAddRptRecord.Text = "Add RPT Record(s) ";
            this.btnAddRptRecord.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnAddRptRecord.UseVisualStyleBackColor = false;
            this.btnAddRptRecord.Click += new System.EventHandler(this.btnAddRptRecord_Click);
            this.btnAddRptRecord.MouseEnter += new System.EventHandler(this.btnAddRptRecord_MouseEnter);
            this.btnAddRptRecord.MouseLeave += new System.EventHandler(this.btnAddRptRecord_MouseLeave);
            // 
            // tbTotalAmountTransferred
            // 
            this.tbTotalAmountTransferred.BackColor = System.Drawing.Color.OldLace;
            this.tbTotalAmountTransferred.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbTotalAmountTransferred.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.tbTotalAmountTransferred.ForeColor = System.Drawing.Color.Black;
            this.tbTotalAmountTransferred.Location = new System.Drawing.Point(706, 105);
            this.tbTotalAmountTransferred.Name = "tbTotalAmountTransferred";
            this.tbTotalAmountTransferred.Size = new System.Drawing.Size(136, 26);
            this.tbTotalAmountTransferred.TabIndex = 3;
            this.tbTotalAmountTransferred.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.tbTotalAmountTransferred.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbSearch_KeyDown);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label1.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label1.Location = new System.Drawing.Point(496, 104);
            this.label1.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.label1.Name = "label1";
            this.label1.Padding = new System.Windows.Forms.Padding(10);
            this.label1.Size = new System.Drawing.Size(204, 36);
            this.label1.TabIndex = 200;
            this.label1.Text = "Total Amount Transferred: ";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tbRecordSelected
            // 
            this.tbRecordSelected.BackColor = System.Drawing.Color.OldLace;
            this.tbRecordSelected.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbRecordSelected.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.tbRecordSelected.ForeColor = System.Drawing.Color.Black;
            this.tbRecordSelected.Location = new System.Drawing.Point(1011, 105);
            this.tbRecordSelected.Name = "tbRecordSelected";
            this.tbRecordSelected.Size = new System.Drawing.Size(60, 26);
            this.tbRecordSelected.TabIndex = 4;
            this.tbRecordSelected.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.tbRecordSelected.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbSearch_KeyDown);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label2.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label2.Location = new System.Drawing.Point(860, 104);
            this.label2.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.label2.Name = "label2";
            this.label2.Padding = new System.Windows.Forms.Padding(10);
            this.label2.Size = new System.Drawing.Size(145, 36);
            this.label2.TabIndex = 200;
            this.label2.Text = "Records Selected:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tbTotalBillAmount
            // 
            this.tbTotalBillAmount.BackColor = System.Drawing.Color.OldLace;
            this.tbTotalBillAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbTotalBillAmount.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.tbTotalBillAmount.ForeColor = System.Drawing.Color.Black;
            this.tbTotalBillAmount.Location = new System.Drawing.Point(353, 104);
            this.tbTotalBillAmount.Name = "tbTotalBillAmount";
            this.tbTotalBillAmount.Size = new System.Drawing.Size(136, 26);
            this.tbTotalBillAmount.TabIndex = 2;
            this.tbTotalBillAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.tbTotalBillAmount.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbSearch_KeyDown);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label3.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label3.Location = new System.Drawing.Point(207, 104);
            this.label3.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.label3.Name = "label3";
            this.label3.Padding = new System.Windows.Forms.Padding(10);
            this.label3.Size = new System.Drawing.Size(140, 36);
            this.label3.TabIndex = 200;
            this.label3.Text = "Total Bill Amount:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnORUpload
            // 
            this.btnORUpload.BackColor = System.Drawing.Color.Transparent;
            this.btnORUpload.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnORUpload.BackgroundImage")));
            this.btnORUpload.FlatAppearance.BorderSize = 0;
            this.btnORUpload.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnORUpload.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnORUpload.Image = ((System.Drawing.Image)(resources.GetObject("btnORUpload.Image")));
            this.btnORUpload.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnORUpload.Location = new System.Drawing.Point(1220, 5);
            this.btnORUpload.Name = "btnORUpload";
            this.btnORUpload.Size = new System.Drawing.Size(119, 94);
            this.btnORUpload.TabIndex = 202;
            this.btnORUpload.Text = "OR Upload";
            this.btnORUpload.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnORUpload.UseVisualStyleBackColor = false;
            this.btnORUpload.Click += new System.EventHandler(this.btnORUpload_Click);
            this.btnORUpload.MouseEnter += new System.EventHandler(this.btnORUpload_MouseEnter);
            this.btnORUpload.MouseLeave += new System.EventHandler(this.btnORUpload_MouseLeave);
            // 
            // btnAddEpayments
            // 
            this.btnAddEpayments.BackColor = System.Drawing.Color.Transparent;
            this.btnAddEpayments.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnAddEpayments.BackgroundImage")));
            this.btnAddEpayments.FlatAppearance.BorderSize = 0;
            this.btnAddEpayments.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddEpayments.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnAddEpayments.Image = ((System.Drawing.Image)(resources.GetObject("btnAddEpayments.Image")));
            this.btnAddEpayments.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnAddEpayments.Location = new System.Drawing.Point(1045, 12);
            this.btnAddEpayments.Name = "btnAddEpayments";
            this.btnAddEpayments.Size = new System.Drawing.Size(169, 87);
            this.btnAddEpayments.TabIndex = 4;
            this.btnAddEpayments.Text = "Add E-payment(s)";
            this.btnAddEpayments.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnAddEpayments.UseVisualStyleBackColor = false;
            this.btnAddEpayments.Click += new System.EventHandler(this.btnAddEpayments_Click);
            this.btnAddEpayments.MouseEnter += new System.EventHandler(this.btnAddEpayments_MouseEnter);
            this.btnAddEpayments.MouseLeave += new System.EventHandler(this.btnAddEpayments_MouseLeave);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label4.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point);
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label4.Location = new System.Drawing.Point(254, 11);
            this.label4.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.label4.Name = "label4";
            this.label4.Padding = new System.Windows.Forms.Padding(10);
            this.label4.Size = new System.Drawing.Size(425, 34);
            this.label4.TabIndex = 206;
            this.label4.Text = "*Search the unique key (RPT: TDN, BUSINESS and MISC: BILL NUMBER)";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnReport
            // 
            this.btnReport.BackColor = System.Drawing.Color.Transparent;
            this.btnReport.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnReport.BackgroundImage")));
            this.btnReport.FlatAppearance.BorderSize = 0;
            this.btnReport.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnReport.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnReport.Image = ((System.Drawing.Image)(resources.GetObject("btnReport.Image")));
            this.btnReport.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnReport.Location = new System.Drawing.Point(1454, 5);
            this.btnReport.Name = "btnReport";
            this.btnReport.Size = new System.Drawing.Size(85, 94);
            this.btnReport.TabIndex = 202;
            this.btnReport.Text = "Report";
            this.btnReport.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnReport.UseVisualStyleBackColor = false;
            this.btnReport.Click += new System.EventHandler(this.btnReport_Click);
            this.btnReport.MouseEnter += new System.EventHandler(this.btnReport_MouseEnter);
            this.btnReport.MouseLeave += new System.EventHandler(this.btnReport_MouseLeave);
            // 
            // DgMainForm
            // 
            this.DgMainForm.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.DgMainForm.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCells;
            this.DgMainForm.BackgroundColor = System.Drawing.Color.White;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.LightSalmon;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.Color.MistyRose;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.Desktop;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.DgMainForm.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.DgMainForm.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            dataGridViewCellStyle5.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(198)))), ((int)(((byte)(247)))));
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.DgMainForm.DefaultCellStyle = dataGridViewCellStyle5;
            this.DgMainForm.EnableHeadersVisualStyles = false;
            this.DgMainForm.Location = new System.Drawing.Point(22, 143);
            this.DgMainForm.Name = "DgMainForm";
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.Color.LightSalmon;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            dataGridViewCellStyle6.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.Color.MistyRose;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.DgMainForm.RowHeadersDefaultCellStyle = dataGridViewCellStyle6;
            this.DgMainForm.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.LightBlue;
            this.DgMainForm.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.DgMainForm.RowTemplate.Height = 25;
            this.DgMainForm.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.DgMainForm.Size = new System.Drawing.Size(1771, 694);
            this.DgMainForm.TabIndex = 207;
            this.DgMainForm.CellContextMenuStripNeeded += new System.Windows.Forms.DataGridViewCellContextMenuStripNeededEventHandler(this.DgMainForm_CellContextMenuStripNeeded);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label5.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label5.ForeColor = System.Drawing.Color.White;
            this.label5.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label5.Location = new System.Drawing.Point(1092, 104);
            this.label5.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.label5.Name = "label5";
            this.label5.Padding = new System.Windows.Forms.Padding(10);
            this.label5.Size = new System.Drawing.Size(175, 36);
            this.label5.TabIndex = 209;
            this.label5.Text = "Remaining OR To Send:";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tbMailToSendCount
            // 
            this.tbMailToSendCount.BackColor = System.Drawing.Color.OldLace;
            this.tbMailToSendCount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbMailToSendCount.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.tbMailToSendCount.ForeColor = System.Drawing.Color.Black;
            this.tbMailToSendCount.Location = new System.Drawing.Point(1273, 104);
            this.tbMailToSendCount.Name = "tbMailToSendCount";
            this.tbMailToSendCount.Size = new System.Drawing.Size(51, 26);
            this.tbMailToSendCount.TabIndex = 208;
            this.tbMailToSendCount.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // btnAssignLocCode
            // 
            this.btnAssignLocCode.BackColor = System.Drawing.Color.Transparent;
            this.btnAssignLocCode.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnAssignLocCode.BackgroundImage")));
            this.btnAssignLocCode.FlatAppearance.BorderSize = 0;
            this.btnAssignLocCode.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAssignLocCode.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnAssignLocCode.Image = ((System.Drawing.Image)(resources.GetObject("btnAssignLocCode.Image")));
            this.btnAssignLocCode.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnAssignLocCode.Location = new System.Drawing.Point(1345, 5);
            this.btnAssignLocCode.Name = "btnAssignLocCode";
            this.btnAssignLocCode.Size = new System.Drawing.Size(103, 94);
            this.btnAssignLocCode.TabIndex = 210;
            this.btnAssignLocCode.Text = "Loc. Code";
            this.btnAssignLocCode.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnAssignLocCode.UseVisualStyleBackColor = false;
            this.btnAssignLocCode.Click += new System.EventHandler(this.btnAssignLocCode_Click);
            this.btnAssignLocCode.MouseEnter += new System.EventHandler(this.btnAssignLocCode_MouseEnter);
            this.btnAssignLocCode.MouseLeave += new System.EventHandler(this.btnAssignLocCode_MouseLeave);
            // 
            // btnEmailTemp
            // 
            this.btnEmailTemp.BackColor = System.Drawing.Color.Transparent;
            this.btnEmailTemp.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnEmailTemp.BackgroundImage")));
            this.btnEmailTemp.FlatAppearance.BorderSize = 0;
            this.btnEmailTemp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEmailTemp.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnEmailTemp.Image = ((System.Drawing.Image)(resources.GetObject("btnEmailTemp.Image")));
            this.btnEmailTemp.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnEmailTemp.Location = new System.Drawing.Point(1545, 5);
            this.btnEmailTemp.Name = "btnEmailTemp";
            this.btnEmailTemp.Size = new System.Drawing.Size(155, 94);
            this.btnEmailTemp.TabIndex = 202;
            this.btnEmailTemp.Text = "Email Template";
            this.btnEmailTemp.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnEmailTemp.UseVisualStyleBackColor = false;
            this.btnEmailTemp.Click += new System.EventHandler(this.btnEmailTemp_Click);
            this.btnEmailTemp.MouseEnter += new System.EventHandler(this.btnEmailTemp_MouseEnter);
            this.btnEmailTemp.MouseLeave += new System.EventHandler(this.btnEmailTemp_MouseLeave);
            // 
            // checkSearchByEmailAdd
            // 
            this.checkSearchByEmailAdd.AutoSize = true;
            this.checkSearchByEmailAdd.BackColor = System.Drawing.Color.Transparent;
            this.checkSearchByEmailAdd.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point);
            this.checkSearchByEmailAdd.Location = new System.Drawing.Point(271, 79);
            this.checkSearchByEmailAdd.Name = "checkSearchByEmailAdd";
            this.checkSearchByEmailAdd.Size = new System.Drawing.Size(158, 18);
            this.checkSearchByEmailAdd.TabIndex = 211;
            this.checkSearchByEmailAdd.Text = "Search by Email Address";
            this.checkSearchByEmailAdd.UseVisualStyleBackColor = false;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Gainsboro;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.ClientSize = new System.Drawing.Size(1880, 758);
            this.Controls.Add(this.checkSearchByEmailAdd);
            this.Controls.Add(this.btnAssignLocCode);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.tbMailToSendCount);
            this.Controls.Add(this.DgMainForm);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.btnAddEpayments);
            this.Controls.Add(this.btnAddRptRecord);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.btnNonRptAddNewRecord);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.SearchLabel);
            this.Controls.Add(this.tbRecordSelected);
            this.Controls.Add(this.tbTotalBillAmount);
            this.Controls.Add(this.tbTotalAmountTransferred);
            this.Controls.Add(this.tbSearch);
            this.Controls.Add(this.btnReport);
            this.Controls.Add(this.btnEmailTemp);
            this.Controls.Add(this.btnORUpload);
            this.ForeColor = System.Drawing.Color.White;
            this.Name = "MainForm";
            this.Text = "Online Payment Transaction System - Main Form";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.SizeChanged += new System.EventHandler(this.MainForm_SizeChanged);
            this.MainDGRightClick.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DgMainForm)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private ContextMenuStrip MainDGRightClick;
        private ToolStripMenuItem testToolStripMenuItem;
        private Label SearchLabel;
        private TextBox tbSearch;
        private Button btnNonRptAddNewRecord;
        private PictureBox pictureBox1;
        private Button btnAddRptRecord;
        private TextBox tbTotalAmountTransferred;
        private Label label1;
        private TextBox tbRecordSelected;
        private Label label2;
        private TextBox tbTotalBillAmount;
        private Label label3;
        private Button btnORUpload;
        private Button btnAddEpayments;
        private Label label4;
        private Button btnReport;
        private DataGridView DgMainForm;
        private Label label5;
        private TextBox tbMailToSendCount;
        private Button btnAssignLocCode;
        private Button btnEmailTemp;
        private CheckBox checkSearchByEmailAdd;
    }
}