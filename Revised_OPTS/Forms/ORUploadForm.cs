using Inventory_System.Model;
using Inventory_System.Service;
using Revised_OPTS.Model;
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
using OpenCvSharp;
using OpenCvSharp.Extensions;
using Inventory_System.Utilities;

namespace Inventory_System.Forms
{
    public partial class ORUploadForm : Form
    {
        IRptService rptService = ServiceFactory.Instance.GetRptService();
        ISecurityService securityService = ServiceFactory.Instance.GetSecurityService();

        VideoCapture videoCapture;
        private Thread cameraThread;
        private bool stopCameraThread = false;

        Dictionary<string, string> RPT_DG_COLUMNS = new Dictionary<string, string>
        {
            { "TaxDec", "TDN" }, { "TaxPayerName", "TaxPayer's Name" }, { "AmountToPay", "Bill Amount" },
            { "AmountTransferred", "Transferred Amount" }, { "Bank", "Bank" }, { "YearQuarter", "Year" },
            { "ValidatedBy", "Validated By" },
        };

        public ORUploadForm()
        {
            InitializeComponent();
            WindowState = FormWindowState.Maximized;
            rbElectronic.Checked = true;

            InitializeCombobox();
            InitializeGrid();
            InitializeCamera();
        }

        private void InitializeCombobox()
        {
            List<Bank> banks = rptService.GetRegularBanks();
            cbRegularBank.Items.AddRange(banks.Select(bank => bank.BankName).ToArray());

            List<UserAccount> validators = securityService.GetValidators();
            cbRegularValBy.Items.AddRange(validators.Select(v => v.DisplayName).ToArray());
        }

        private void InitializeGrid()
        {
            dgRptList.Columns.Clear();
            dgRptList.AutoGenerateColumns = false;
            foreach (var kvp in RPT_DG_COLUMNS)
            {
                dgRptList.Columns.Add(kvp.Key, kvp.Value);
                dgRptList.Columns[kvp.Key].DataPropertyName = kvp.Key;
            }
            dgRptList.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 12, FontStyle.Regular);
            dgRptList.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
            dgRptList.ColumnHeadersDefaultCellStyle.BackColor = Color.DarkSalmon;
            dgRptList.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.DarkSalmon;
        }

        private void InitializeCamera()
        {
            stopCameraThread = false;
            cameraThread = new Thread(new ThreadStart(CaptureCameraCallback));
            cameraThread.Start();
        }

        private void CaptureCameraCallback()
        {
            videoCapture = new VideoCapture(0);
            videoCapture.Open(0);

            Mat frame = new Mat();
            if (videoCapture.IsOpened())
            {
                while(!stopCameraThread)
                {
                    videoCapture.Read(frame);
                    Bitmap image = BitmapConverter.ToBitmap(frame);
                    if (pbVideoCapture.Image != null)
                    {
                        pbVideoCapture.Image.Dispose();
                    }
                    pbVideoCapture.Image = image;
                }
                videoCapture.Release();
                lock (pbVideoCapture)
                {
                    if (pbVideoCapture.Image != null)
                    {
                        pbVideoCapture.Image.Dispose();
                    }
                    pbVideoCapture.Image = null;
                }
            }
        }
        private void rbElectronic_CheckedChanged(object sender, EventArgs e)
        {
            tbElectronicTaxDec.Enabled = true;
            dtRegularDate.Enabled = false;
            cbRegularBank.Enabled = false;
            cbRegularValBy.Enabled = false;
        }

        private void rbRegular_CheckedChanged(object sender, EventArgs e)
        {
            tbElectronicTaxDec.Enabled = false;
            dtRegularDate.Enabled = true;
            cbRegularBank.Enabled = true;
            cbRegularValBy.Enabled = true;
        }

        private void SearchRecordsOnEnterKey(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SearchRecords(sender, e);
            }
        }

        private void SearchRecords(object sender, EventArgs e)
        {
            if (rbElectronic.Checked)
            {
                dgRptList.DataSource = rptService.RetrieveBySearchKeyword(tbElectronicTaxDec.Text);
            }
            else
            {
                dgRptList.DataSource = rptService.RetrieveForORUploadRegular(dtRegularDate.Value, cbRegularBank.Text, cbRegularValBy.Text);
            }
        }

        private void ORUploadForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            stopCameraThread = true;
            cameraThread.Join();
        }

        private void btnSaveReceipt_Click(object sender, EventArgs e)
        {
            DataGridViewRow selectedRow = dgRptList.CurrentRow;
            if (selectedRow != null && videoCapture!= null && videoCapture.IsOpened())
            {
                // prepare photo, save, and then show
                Rpt rpt = selectedRow.DataBoundItem as Rpt;
                RPTAttachPicture rptAttachPicture = new RPTAttachPicture();
                rptAttachPicture.RptId = rpt.RptID;
                rptAttachPicture.DocumentType = DocumentType.RECEIPT;
                rptAttachPicture.FileName = "or.jpg";
                byte[] FileData = ImageUtil.ImageToByteArray(pbVideoCapture.Image);
                byte[] resizeFileData = ImageUtil.resizeJpg(FileData);
                rptAttachPicture.FileData = resizeFileData;
                rptService.InsertPicture(rptAttachPicture);
                loadRptReceipt(rpt.RptID);

                // Give user few second to see the photo and then select the next record
                Task.Delay(3000).ContinueWith(_ =>
                {
                    int currentRowIndex = dgRptList.CurrentCell.RowIndex;
                    if (currentRowIndex < dgRptList.Rows.Count - 1)
                    {
                        dgRptList.CurrentCell = dgRptList.Rows[currentRowIndex + 1].Cells[dgRptList.CurrentCell.ColumnIndex];
                    }
                }, TaskScheduler.FromCurrentSynchronizationContext());
            }
        }

        /*private void dgRptList_CellClick(object sender, DataGridViewCellEventArgs e)
        {

        }*/

        private void loadRptReceipt(long rptId)
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
            }
        }

        private void dgRptList_CellClick(object sender, EventArgs e)
        {
            DataGridViewRow selectedRow = dgRptList.CurrentRow;
            if (selectedRow != null)
            {
                Rpt rpt = selectedRow.DataBoundItem as Rpt;
                loadRptReceipt(rpt.RptID);
            }
        }
    }
}
