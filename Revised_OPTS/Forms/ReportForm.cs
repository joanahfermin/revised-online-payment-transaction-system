﻿using Inventory_System.Model;
using Inventory_System.Utilities;
using Revised_OPTS.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Inventory_System.Forms
{
    public partial class ReportForm : Form
    {
        private Image originalBackgroundImageRpt;
        DynamicControlContainer dynamicControlContainer;

        private DynamicGridContainer<AllTaxTypeReport> DynamicGridContainer;

        IRptService rptService = ServiceFactory.Instance.GetRptService();

        public ReportForm()
        {
            InitializeComponent();
            InitializeReportType();
            //InitializeDynamicMapping();

            dynamicControlContainer = new DynamicControlContainer(this);

            DgReportForm.DefaultCellStyle.Font = new Font("Tahoma", 12, FontStyle.Regular);
            this.WindowState = FormWindowState.Maximized;
        }
        public void InitializeReportType()
        {
            foreach (string allReport in AllTaxTypeReportUtil.ALL_REPORT.OrderBy(reportType => reportType))
            {
                cbTaxTypeReport.Items.Add(allReport);
            }
        }

        public void InitializeDynamicMapping()
        {
            DynamicGridInfo[] gridInfoArray = new DynamicGridInfo[] {
                new DynamicGridInfo{PropertyName="TaxType", Label = "Tax Type", isReadOnly = true },
                new DynamicGridInfo{PropertyName="BillNumber", Label = "Bill Number", isRequired=true },
                new DynamicGridInfo{PropertyName="TaxpayerName", Label = "Taxpayer's Name", isRequired=true },
                new DynamicGridInfo{PropertyName="BillAmount", Label = "Bill Amount", isRequired=true },
                new DynamicGridInfo{PropertyName="TotalAmountTransferred", Label = "Total Amount Transferred" },
                new DynamicGridInfo{PropertyName="ExcessShort", Label = "Excess/Short"},
                new DynamicGridInfo{PropertyName="Remarks", Label = "Remarks"},
            };
            DynamicGridContainer = new DynamicGridContainer<AllTaxTypeReport>(DgReportForm, gridInfoArray, true, true);
        }

        private void cbTaxType_SelectedIndexChanged(object sender, EventArgs e)
        {
            string taxType = cbTaxTypeReport.Text;
            DgReportForm.Rows.Clear();
            DgReportForm.Columns.Clear();
            
            if (taxType == AllTaxTypeReportUtil.COLLECTORS_REPORT)
            {
                InitializeDynamicMapping();
                DgReportForm.DataSource = rptService.RetrieveByValidatedDate(dtFrom.Value, dtTo.Value);
            }
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {

        }

        private void btnClose_MouseEnter(object sender, EventArgs e)
        {
            originalBackgroundImageRpt = btnClose.BackgroundImage;
            btnClose.BackgroundImage = null;

            Color customColor = Color.FromArgb(23, 45, 74);
            btnClose.BackColor = customColor;
        }

        private void btnClose_MouseLeave(object sender, EventArgs e)
        {
            btnClose.BackgroundImage = originalBackgroundImageRpt;
        }

        private void btnGenerate_MouseEnter(object sender, EventArgs e)
        {
            originalBackgroundImageRpt = btnGenerate.BackgroundImage;
            btnGenerate.BackgroundImage = null;

            Color customColor = Color.FromArgb(23, 45, 74);
            btnGenerate.BackColor = customColor;
        }

        private void btnGenerate_MouseLeave(object sender, EventArgs e)
        {
            btnGenerate.BackgroundImage = originalBackgroundImageRpt;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
