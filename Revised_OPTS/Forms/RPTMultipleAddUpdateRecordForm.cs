using Inventory_System.DAL;
using Inventory_System.Exception;
using Inventory_System.Model;
using Inventory_System.Utilities;
using Revised_OPTS;
using Revised_OPTS.DAL;
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
    public partial class RPTMultipleAddUpdateRecordForm : Form
    {
        IRptService rptService = ServiceFactory.Instance.GetRptService();
        IRptTaxbillTPNRepository rptRetrieveTaxpayerNameRep = RepositoryFactory.Instance.GetRptRetrieveTaxpayerNameRepository();

        private Image originalBackgroundImage;
        private Image originalCloseBackgroundImage;

        Color customColor = Color.FromArgb(6, 19, 36);

        private DynamicGridContainer<Rpt> DynamicGridContainer;
        private NotificationHelper NotificationHelper = new NotificationHelper();

        public RPTMultipleAddUpdateRecordForm()
        {
            InitializeComponent();
            InitializeDataGridView();
            DataGridUI();

            DgRptAddUpdateForm.CellFormatting += DgRptAddUpdateForm_CellFormatting;
            DgRptAddUpdateForm.CellValueChanged += DgRptAddUpdateForm_CellValueChanged;
            DgRptAddUpdateForm.RowsRemoved += DgRptAddUpdateForm_RowsRemoved;
            DgRptAddUpdateForm.RowsAdded += DgRptAddUpdateForm_RowsAdded;
            DgRptAddUpdateForm.DefaultValuesNeeded += DgRptAddUpdateForm_DefaultValuesNeeded;
            DgRptAddUpdateForm.CellClick += DgRptAddUpdateForm_CellClick;

            panel1.BackColor = customColor;
            btnSaveRecord.BackColor = customColor;
            btnClose.BackColor = customColor;
        }

        private void DgRptAddUpdateForm_CellClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                // Access the clicked cell's value
                object cellValue = DgRptAddUpdateForm.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;

                // Copy the text to the clipboard
                if (cellValue != null)
                {
                    Clipboard.SetText(cellValue.ToString());
                }
            }
        }

        public RPTMultipleAddUpdateRecordForm(string refNum, string reqParty)
        {
            InitializeComponent();
            InitializeDataGridView();
            DataGridUI();

            DgRptAddUpdateForm.CellFormatting += DgRptAddUpdateForm_CellFormatting;
            DgRptAddUpdateForm.CellValueChanged += DgRptAddUpdateForm_CellValueChanged;
            DgRptAddUpdateForm.RowsRemoved += DgRptAddUpdateForm_RowsRemoved;
            DgRptAddUpdateForm.RowsAdded += DgRptAddUpdateForm_RowsAdded;
            DgRptAddUpdateForm.DefaultValuesNeeded += DgRptAddUpdateForm_DefaultValuesNeeded;
            DgRptAddUpdateForm.CellClick += DgRptAddUpdateForm_CellClick;

            panel1.BackColor = customColor;
            btnSaveRecord.BackColor = customColor;
            btnClose.BackColor = customColor;

            List<Rpt> allRptSameRefnum = rptService.RetrieveBySameRefNumAndReqParty(refNum, reqParty);
            DynamicGridContainer.PopulateData(allRptSameRefnum);
        }

        private void DgRptAddUpdateForm_DefaultValuesNeeded(object sender, DataGridViewRowEventArgs e)
        {
            DataGridViewRow selectedRow = DgRptAddUpdateForm.CurrentRow;
            if (selectedRow != null)
            {
                Rpt selectedRpt = (Rpt)selectedRow.DataBoundItem;
                selectedRpt.PaymentDate = DateTime.Now;
            }
        }

        private void DgRptAddUpdateForm_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            DataGridViewRow selectedRow = DgRptAddUpdateForm.CurrentRow;

            // Check if a row is selected
            if (selectedRow != null)
            {
                Rpt selectedRpt = (Rpt)selectedRow.DataBoundItem;

                if (selectedRpt != null)
                {
                    if (selectedRpt.Bank == null)
                    {
                        selectedRpt.Bank = "BANK TRANSFER";
                    }
                    if (selectedRpt.Quarter == null)
                    {
                        selectedRpt.Quarter = Quarter.FULL_YEAR;
                    }
                    if (selectedRpt.BillingSelection == null)
                    {
                        selectedRpt.BillingSelection = BillingSelectionUtil.CLASS1;
                    }
                }
            }
            UpdateTotalAmount();
        }

        private void DgRptAddUpdateForm_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            UpdateTotalAmount();
        }

        private void DgRptAddUpdateForm_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            UpdateTotalAmount();

            //Retrieve TPN using TDN.
            if (e.RowIndex >= 0 && e.ColumnIndex == 1)
            {
                // Access the cell value
                DataGridViewRow selectedRow = DgRptAddUpdateForm.CurrentRow;

                if (selectedRow != null)
                {
                    Rpt selectedRpt = (Rpt)selectedRow.DataBoundItem;
                    RptTaxbillTPN retrievedTPN = rptRetrieveTaxpayerNameRep.retrieveByTDN(selectedRpt.TaxDec);

                    if (retrievedTPN != null)
                    {
                        selectedRpt.TaxPayerName = retrievedTPN.ONAME;
                    }
                    //else
                    //{
                    //    selectedRpt.TaxPayerName = Validations.NO_RETRIEVED_NAME;
                    //}
                }
            }
        }

        private void UpdateTotalAmount()
        {
            List<Rpt> listOfRptsToSave = DynamicGridContainer.GetData();

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
                new DynamicGridInfo{PropertyName="Bank", Label = "Bank", GridType = DynamicGridType.ComboBox, ComboboxChoices = bankNames, isRequired=true },
                new DynamicGridInfo{PropertyName="PaymentDate", Label = "Payment Date", GridType = DynamicGridType.DatetimePicker, isRequired=true },
                new DynamicGridInfo{PropertyName="YearQuarter", Label = "Year", decimalValue = true},
                new DynamicGridInfo{PropertyName="Quarter", Label = "Quarter", GridType=DynamicGridType.ComboBox, ComboboxChoices = Quarter.ALL_QUARTER, isRequired=true },
                new DynamicGridInfo{PropertyName="BillingSelection", Label = "Billing Selection", GridType=DynamicGridType.ComboBox, ComboboxChoices = BillingSelectionUtil.ALL_BILLING_SELECTION, isRequired=true },
                new DynamicGridInfo{PropertyName="RequestingParty", Label = "Email Address" },
                new DynamicGridInfo{PropertyName="RPTremarks", Label = "Remarks"},
            };
            DynamicGridContainer = new DynamicGridContainer<Rpt>(DgRptAddUpdateForm, gridInfoArray, true, true);
        }

        public void DataGridUI()
        {   
            DgRptAddUpdateForm.DefaultCellStyle.Font = new Font("Tahoma", 12, FontStyle.Regular);
            DgRptAddUpdateForm.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCells;

            DgRptAddUpdateForm.GridColor = Color.DarkGray;
            DgRptAddUpdateForm.CellBorderStyle = DataGridViewCellBorderStyle.Single;
            //DgRptAddUpdateForm.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Raised;

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

            if (!DynamicGridContainer.HaveNoErrors())
            {
                MessageBox.Show("Data contains error.");
                return;
            }

            try
            {
                rptService.SaveAll(listOfRptsToSave, listOfRptsToDelete, totalAmountTransferred);
                if (firstTaxdecRecord != null)
                {
                    NotificationHelper.notifyUserAndRefreshRecord(firstTaxdecRecord);
                }
                btnClose_Click(sender, e);

            }
            catch (DuplicateRecordException ex)
            {
                DialogResult result = MessageBox.Show($"There is an existing record/s detected in the database.", "Duplicate Record Detected", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);

                ExistingRecordForm DuplicateForm = new ExistingRecordForm(ex.duplicateRptList, new List<Business>(), new List<Miscellaneous>());
                DuplicateForm.ShowDialog();

                if (MessageBox.Show("Would you like to continue?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    rptService.SaveAll(listOfRptsToSave, listOfRptsToDelete, totalAmountTransferred, false);
                    MessageBox.Show("Record(s) have been successfully saved.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                return;
            }
            catch (RptException ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
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
