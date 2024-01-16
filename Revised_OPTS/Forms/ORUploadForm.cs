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
        private IRptService rptService = ServiceFactory.Instance.GetRptService();
        private ISecurityService securityService = ServiceFactory.Instance.GetSecurityService();

        private VideoCapture videoCapture;
        private Mat frame = new Mat();
        private Thread cameraThread;
        private bool stopCameraThread = false;

        Dictionary<string, string> RPT_DG_COLUMNS = new Dictionary<string, string>
        {
            { "TaxDec", "TDN" }, { "TaxPayerName", "TaxPayer's Name" }, { "AmountToPay", "Bill Amount" },
            { "AmountTransferred", "Transferred Amount" }, { "Bank", "Bank" }, { "YearQuarter", "Year" }, { "Quarter", "Quarter" },
            { "BillingSelection", "Billing Selection" }, { "ValidatedBy", "Validated By" },
        };

        private Image originalBackgroundImageRpt;

        public ORUploadForm()
        {
            InitializeComponent();
            rbElectronic.Checked = true;

            InitializeCombobox();
            InitializeGrid();
            InitializeCamera();
            this.WindowState = FormWindowState.Maximized;

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
            //dgRptList.Refresh();
            /*
            dgRptList.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 12, FontStyle.Regular);
            dgRptList.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
            dgRptList.ColumnHeadersDefaultCellStyle.BackColor = Color.DarkSalmon;
            dgRptList.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.DarkSalmon;

            foreach (DataGridViewColumn column in dgRptList.Columns)
            {
                column.HeaderCell.Style.Font = dgRptList.ColumnHeadersDefaultCellStyle.Font;
                column.HeaderCell.Style.ForeColor = dgRptList.ColumnHeadersDefaultCellStyle.ForeColor;
                column.HeaderCell.Style.BackColor = dgRptList.ColumnHeadersDefaultCellStyle.BackColor;
                column.HeaderCell.Style.SelectionBackColor = dgRptList.ColumnHeadersDefaultCellStyle.SelectionBackColor;
            }
            */
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

            
            if (videoCapture.IsOpened())
            {
                videoCapture.FrameHeight = 1920;
                videoCapture.FrameWidth = 1080;

                while (!stopCameraThread)
                {
                    videoCapture.Read(frame);
                    Bitmap image;
                    Image tempImage = pbVideoCapture.Image;
                    lock (videoCapture)
                    {
                        image = BitmapConverter.ToBitmap(frame);
                    }
                    Thread.Sleep(5);
                    pbVideoCapture.Image = image;
                    if (tempImage != null)
                    {
                        tempImage.Dispose();
                    }
                    Thread.Sleep(5);
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
                dgRptList.DataSource = rptService.RetrieveBySameRefNumInUploadingEpayment(tbElectronicTaxDec.Text);

                dgRptList.ClearSelection();
                int counter = 0;
                foreach (DataGridViewRow row in dgRptList.Rows)
                {
                    Rpt selectedRptRecord = row.DataBoundItem as Rpt;

                    if (selectedRptRecord.TaxDec.Equals(tbElectronicTaxDec.Text, StringComparison.OrdinalIgnoreCase))
                    {
                        row.Selected = true;
                        loadRptReceipt(selectedRptRecord.RptID);
                        dgRptList.FirstDisplayedScrollingRowIndex = counter;
                    }
                    counter++;
                }
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
                pbReceipt.SizeMode = PictureBoxSizeMode.StretchImage;
                //File.WriteAllBytes($"c:/temp/{rptId}.jpg", pix.FileData);
            }
        }

        private void dgRptList_CellClick(object sender, EventArgs e)
        {
            DataGridViewRow selectedRow = dgRptList.CurrentRow;
            if (selectedRow != null)
            {
                Rpt rpt = selectedRow.DataBoundItem as Rpt;
                if (rpt != null)
                {
                    loadRptReceipt(rpt.RptID);
                }
            }
        }

        private void btnUploadReceipt_Click(object sender, EventArgs e)
        {
            DataGridViewRow selectedRow = dgRptList.CurrentRow;
            if (selectedRow != null && videoCapture != null && videoCapture.IsOpened())
            {
                // prepare photo, save, and then show
                Rpt rpt = selectedRow.DataBoundItem as Rpt;
                RPTAttachPicture rptAttachPicture = new RPTAttachPicture();
                rptAttachPicture.RptId = rpt.RptID;
                rptAttachPicture.DocumentType = DocumentType.RECEIPT;
                rptAttachPicture.FileName = "or.jpg";

                Bitmap image;
                lock (videoCapture)
                {
                    image = BitmapConverter.ToBitmap(frame);
                }
                byte[] FileData = ImageUtil.ImageToByteArray(image);
                image.Dispose();

                //byte[] FileData = ImageUtil.ImageToByteArray(pbVideoCapture.Image);
                byte[] resizeFileData = ImageUtil.resizeJpg(FileData);
                rptAttachPicture.FileData = resizeFileData;
                rptService.InsertPicture(rptAttachPicture);
                loadRptReceipt(rpt.RptID);

                // Give user few second to see the photo and then select the next record
                Task.Delay(1000).ContinueWith(_ =>
                {
                    int currentRowIndex = dgRptList.CurrentCell.RowIndex;
                    if (currentRowIndex < dgRptList.Rows.Count - 1)
                    {
                        dgRptList.CurrentCell = dgRptList.Rows[currentRowIndex + 1].Cells[dgRptList.CurrentCell.ColumnIndex];
                    }
                }, TaskScheduler.FromCurrentSynchronizationContext());
            }
        }

        private void btnUploadReceipt_MouseEnter(object sender, EventArgs e)
        {
            originalBackgroundImageRpt = btnUploadReceipt.BackgroundImage;
            btnUploadReceipt.BackgroundImage = null;

            Color customColor = Color.FromArgb(23, 45, 74);
            btnUploadReceipt.BackColor = customColor;
        }

        private void btnUploadReceipt_MouseLeave(object sender, EventArgs e)
        {
            btnUploadReceipt.BackgroundImage = originalBackgroundImageRpt;
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

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}
