﻿using Inventory_System.Utilities;
using Revised_OPTS;
using Revised_OPTS.Model;
using Revised_OPTS.Service;
using Revised_OPTS.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Inventory_System.Forms
{
    public partial class RPTAddUpdateRecordForm : Form
    {
        IRptService rptService = ServiceFactory.Instance.GetRptService();

        private Image originalBackgroundImage;
        private Image originalCloseBackgroundImage;

        Color customColor = Color.FromArgb(6, 19, 36);

        private DynamicGridContainer<Rpt> DynamicGridContainer;

        public RPTAddUpdateRecordForm()
        {
            InitializeComponent();
            InitializeDataGridView();
            DataGridUI();
            DgRptAddUpdateForm.CellFormatting += DgRptAddUpdateForm_CellFormatting;
            DgRptAddUpdateForm.CellValueChanged += DgRptAddUpdateForm_CellValueChanged;
            DgRptAddUpdateForm.RowsRemoved += DgRptAddUpdateForm_RowsRemoved;

            panel1.BackColor = customColor;
            btnSaveRecord.BackColor = customColor;
            btnClose.BackColor = customColor;
        }

        private void DgRptAddUpdateForm_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            UpdateTotalAmount();
        }

        private void DgRptAddUpdateForm_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            UpdateTotalAmount();
        }

        private void UpdateTotalAmount()
        {
            List<Rpt> listOfRptsToSave = DynamicGridContainer.GetData();

            //decimal? totalAmountToPayComputed = 0;

            //foreach (Rpt rpt in listOfRptsToSave)
            //{
            //    totalAmountToPayComputed += rpt.AmountToPay;
            //}

            decimal totalAmountToPayComputed = listOfRptsToSave.Sum(rpt => rpt.AmountToPay ?? 0);

            tbTotalAmountTransferred.Text = totalAmountToPayComputed.ToString("N", CultureInfo.InvariantCulture);
        }

        private void DgRptAddUpdateForm_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.Value is decimal decimalValue)
            {
                e.Value = decimalValue.ToString("N2");
                e.FormattingApplied = true;
            }
        }

        private void InitializeDataGridView()
        {
            List<Bank> bankList = rptService.GetAllBanks();
            string[] bankNames = bankList.Select(bank => bank.BankName).ToList().ToArray();

            DynamicGridInfo[] gridInfoArray = new DynamicGridInfo[] {
                new DynamicGridInfo{PropertyName="RptID", Label = "ID", isReadOnly = true },
                new DynamicGridInfo{PropertyName="TaxDec", Label = "TDN", isRequired=true },
                new DynamicGridInfo{PropertyName="TaxPayerName", Label = "TaxPayer's Name", isRequired=true },
                new DynamicGridInfo{PropertyName="AmountToPay", Label = "Bill Amount", isRequired=true },
                //new DynamicGridInfo{PropertyName="AmountTransferred", Label = "Transferred Amount", isRequired=true },
                new DynamicGridInfo{PropertyName="Bank", Label = "Bank", GridType = DynamicGridType.ComboBox, ComboboxChoices = bankNames, isRequired=true },

                new DynamicGridInfo{PropertyName="YearQuarter", Label = "Year", decimalValue = true},
                new DynamicGridInfo{PropertyName="Quarter", Label = "Quarter", GridType=DynamicGridType.ComboBox, ComboboxChoices = Quarter.ALL_QUARTER, isRequired=true },
                new DynamicGridInfo{PropertyName="RequestingParty", Label = "Email Address" },

                new DynamicGridInfo{PropertyName="RPTremarks", Label = "Remarks"},
            };
            DynamicGridContainer = new DynamicGridContainer<Rpt>(DgRptAddUpdateForm, gridInfoArray, true);
        }

        public void DataGridUI()
        {   
            DgRptAddUpdateForm.DefaultCellStyle.Font = new Font("Tahoma", 12, FontStyle.Regular);
            DgRptAddUpdateForm.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCells;
            DgRptAddUpdateForm.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            DgRptAddUpdateForm.GridColor = Color.DarkGray;
            DgRptAddUpdateForm.CellBorderStyle = DataGridViewCellBorderStyle.Single;
            DgRptAddUpdateForm.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Raised;

            DgRptAddUpdateForm.ColumnHeadersDefaultCellStyle.BackColor = Color.DarkSalmon;
        }

        private void btnSaveRecord_Click(object sender, EventArgs e)
        {
            string firstTaxdecRecord = null;
            decimal totalAmountTransferred = Convert.ToDecimal(tbTotalAmountTransferred.Text);

            List<Rpt> listOfRptsToSave = DynamicGridContainer.GetData();
            List<Rpt> listOfRptsToDelete = DynamicGridContainer.GetDataToDelete();

            if (listOfRptsToSave.Count > 0)
            {
                firstTaxdecRecord = listOfRptsToSave[0].TaxDec.ToString();
            }
            else
            {
                MessageBox.Show("No items in the grid.");
                return;
            }

            rptService.SaveAll(listOfRptsToSave, listOfRptsToDelete, totalAmountTransferred);
            notifyUserAndRefreshRecord(firstTaxdecRecord);
            btnClose_Click(sender, e);
        }

        public void notifyUserAndRefreshRecord(string keyWord)
        {
            MessageBox.Show("Record successfully saved.");
            MainForm.Instance.Search(keyWord);
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
