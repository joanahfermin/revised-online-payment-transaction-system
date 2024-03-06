using Inventory_System.DAL;
using Inventory_System.Model;
using Inventory_System.Utilities;
using Revised_OPTS.DAL;
using Revised_OPTS.Model;
using Revised_OPTS.Service;
using Revised_OPTS.Utilities;
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
    public partial class ReleaseORForm : Form
    {
        private Image originalBackgroundImage;

        IRptService rptService = ServiceFactory.Instance.GetRptService();
        IRPTAttachPictureRepository rptAttachPictureRepository = RepositoryFactory.Instance.GetRPTAttachPictureRepository();

        private DynamicGridContainer<Rpt> RptDynamicGridContainer;
        private long RptID = 0;

        public ReleaseORForm(List<Rpt> rptList)
        {
            InitializeComponent();
            InitializeRptDataGridView();

            RptDynamicGridContainer.PopulateData(rptList);

            DgRptAddUpdateForm.DefaultCellStyle.Font = new Font("Tahoma", 12, FontStyle.Regular);
            this.WindowState = FormWindowState.Maximized;

            DgRptAddUpdateForm.SelectionChanged += DgRptAddUpdateForm_SelectionChanged;
        }

        private void DgRptAddUpdateForm_SelectionChanged(object? sender, EventArgs e)
        {
            DataGridViewRow selectedRow = DgRptAddUpdateForm.CurrentRow;

            if (selectedRow != null)
            {
                Rpt selectedRpt = (Rpt)selectedRow.DataBoundItem;
                if (selectedRpt != null)
                {
                    RptID = selectedRpt.RptID;
                    loadRptReceipt(RptID);
                }
            }
        }

        private void InitializeRptDataGridView()
        {
            List<Bank> bankList = rptService.GetAllBanks();
            string[] bankNames = bankList.Select(bank => bank.BankName).ToList().ToArray();

            DynamicGridInfo[] gridInfoArray = new DynamicGridInfo[] {
                //new DynamicGridInfo{PropertyName="RptID", Label = "Rpt ID", isReadOnly = true },
                new DynamicGridInfo{PropertyName="LocCode", Label = "Location Code", isReadOnly = true },
                new DynamicGridInfo{PropertyName="TaxDec", Label = "TDN", isRequired=true, isReadOnly = true  },
                new DynamicGridInfo{PropertyName="TaxPayerName", Label = "TaxPayer's Name", isRequired=true, isReadOnly = true  },
                new DynamicGridInfo{PropertyName="AmountToPay", Label = "Bill Amount", isRequired=true, isReadOnly = true  },
                new DynamicGridInfo{PropertyName="TotalAmountTransferred", Label = "Total Amount Transferred", isRequired=true, isReadOnly = true  },
                new DynamicGridInfo{PropertyName="Bank", Label = "Bank", GridType = DynamicGridType.ComboBox, ComboboxChoices = bankNames, isRequired=true, isReadOnly = true  },
                new DynamicGridInfo{PropertyName="PaymentDate", Label = "Payment Date", GridType = DynamicGridType.DatetimePicker, isRequired=true, isReadOnly = true  },
                new DynamicGridInfo{PropertyName="YearQuarter", Label = "Year", decimalValue = true, isRequired=true, isReadOnly = true},
                new DynamicGridInfo{PropertyName="Quarter", Label = "Quarter", GridType=DynamicGridType.ComboBox, ComboboxChoices = Quarter.ALL_QUARTER, isRequired=true, isReadOnly = true },
                new DynamicGridInfo{PropertyName="Status", Label = "Status", GridType=DynamicGridType.ComboBox, ComboboxChoices = TaxStatus.STATUS, isReadOnly = true },
                new DynamicGridInfo{PropertyName="BillingSelection", Label = "Billing Selection", GridType=DynamicGridType.ComboBox, ComboboxChoices = BillingSelectionUtil.ALL_BILLING_SELECTION, isRequired=true, isReadOnly = true },
                new DynamicGridInfo{PropertyName="RequestingParty", Label = "Email Address", isReadOnly = true },
                new DynamicGridInfo{PropertyName="RPTremarks", Label = "Remarks", isReadOnly = true },
            };
            RptDynamicGridContainer = new DynamicGridContainer<Rpt>(DgRptAddUpdateForm, gridInfoArray, true, true);
        }

        public void loadRptReceipt(long rptId)
        {
            if (pbReceipt.Image != null)
            {
                pbReceipt.Image.Dispose();
            }
            pbReceipt.Image = null;

            RPTAttachPicture pix = rptService.getRptReceipt(rptId);
            if (pix != null)
            {
                pbReceipt.Image = Image.FromStream(new MemoryStream(pix.FileData));
                pbReceipt.SizeMode = PictureBoxSizeMode.StretchImage;
            }
        }

        private void btnSaveRecord_MouseEnter(object sender, EventArgs e)
        {
            originalBackgroundImage = btnSaveRecord.BackgroundImage;
            btnSaveRecord.BackgroundImage = null;

            Color customColor = Color.FromArgb(23, 45, 74);
            btnSaveRecord.BackColor = customColor;
        }

        private void btnSaveRecord_Click(object sender, EventArgs e)
        {
            string repName = textRepName.Text;
            string contactNum = textContactNum.Text;
            string releaser = textRelesedBy.Text;
            TextBox textbox = new TextBox();
            textbox.Text = repName;

            DataGridViewSelectedRowCollection selectedRows = DgRptAddUpdateForm.SelectedRows;
            if (selectedRows != null)
            {
                List<Rpt> rptList = new List<Rpt>();
                string firstTaxdec = null;

                foreach (DataGridViewRow row in selectedRows)
                {
                    object item = row.DataBoundItem;
                    if (item is Rpt)
                    {
                        Rpt rpt = item as Rpt;
                        rptList.Add(rpt);
                    }
                    if (repName.Length == 0 || contactNum.Length == 0 || releaser.Length == 0)
                    {
                        Validations.validateRequired(errorProvider1, textRepName, "Rep. Name");
                        Validations.validateRequired(errorProvider1, textContactNum, "Contact Number");
                        Validations.validateRequired(errorProvider1, textRelesedBy, "Released By");
                        return;
                    }
                }
                if (rptList.Count > 0)
                {
                    DialogResult result = MessageBox.Show("Are you sure you want to update the status of the selected records?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (result == DialogResult.Yes)
                    {
                        rptService.ReleaseReceipt(rptList, TaxStatus.Released, repName, contactNum, releaser);
                        DgRptAddUpdateForm.Refresh();
                        string rptListFirst = rptList[0].TaxDec.ToString();
                        NotificationHelper.notifyUserAndRefreshRecord(rptListFirst);
                        btnClose_Click(sender, e);
                    }
                }
            }
        }

        private void btnSaveRecord_MouseLeave(object sender, EventArgs e)
        {
            btnSaveRecord.BackgroundImage = originalBackgroundImage;
        }

        private void btnClose_MouseEnter(object sender, EventArgs e)
        {
            originalBackgroundImage = btnClose.BackgroundImage;
            btnClose.BackgroundImage = null;

            Color customColor = Color.FromArgb(23, 45, 74);
            btnClose.BackColor = customColor;
        }

        private void btnClose_MouseLeave(object sender, EventArgs e)
        {
            btnClose.BackgroundImage = originalBackgroundImage;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
