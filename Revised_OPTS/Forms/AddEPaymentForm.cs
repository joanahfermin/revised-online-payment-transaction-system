using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml;
using Inventory_System.Exception;
using Inventory_System.Model;
using Inventory_System.Utilities;
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
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Inventory_System.Forms
{
    public partial class AddEPaymentForm : Form
    {
        List<ElectronicPayment> data = new List<ElectronicPayment>();

        private Image originalBackgroundImageNonRpt;
        private Image originalBackgroundImageRpt;

        private DynamicGridContainer<Rpt> DynamicGridContainer;

        IRptService rptService = ServiceFactory.Instance.GetRptService();

        ElectronicPayment ep = new ElectronicPayment();
        TaxUniqueKeyFormat taxUniqueKeyFormat = new TaxUniqueKeyFormat();
        List<Rpt> rptToSaveList = new List<Rpt>();
        List<Miscellaneous> miscToSaveList = new List<Miscellaneous>();
        List<Business> businessToSaveList = new List<Business>();

        //Rpt rpt = new Rpt();

        decimal totalAmountTransferred = 0;
        string initialValueOfQuarter = Quarter.FULL_YEAR;
        string initialValueBillingSelection = BillingSelectionUtil.CLASS1;
        string firstRecordSearchMainFormRef = null;

        private NotificationHelper NotificationHelper = new NotificationHelper();

        public AddEPaymentForm()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;

            dgRptList.KeyUp += dgRptList_KeyDown;
        }

        private void dgRptList_KeyDown(object? sender, KeyEventArgs e)
        {
            PasteDataFromClipboard();
            //totalAmountTransferred = Convert.ToDecimal(tbTotalAmountTransferred.Text);
        }

        /// <summary>
        /// Transfer data from clipboard.
        /// </summary>
        private void PasteDataFromClipboard()
        {
            string dataAsString = Clipboard.GetText();

            List<int> IgnoreColumnList = new List<int>();
            IgnoreColumnList.Add(0);
            IgnoreColumnList.Add(1);
            IgnoreColumnList.Add(2);
            IgnoreColumnList.Add(8);
            IgnoreColumnList.Add(10);
            IgnoreColumnList.Add(11);

            //For every record copied, .split splits strings into several lines. r and n = new line, RemoveEmptyEnries, kapag walang laman ang row, iignore lang nya.
            string[] rowArray = dataAsString.Split(new Char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

            if (rowArray.Length > 0)
            {
                dgRptList.ClearSelection();

                foreach (string row in rowArray)
                {
                    //Splits the records through TAB. \t = tab.
                    string[] columnArray = row.Split(new char[] { '\t' });

                    if (columnArray.Length >= 13)
                    {
                        //ElectronicPayment ep = new ElectronicPayment();//xxx
                        ep.EpaymentRef = columnArray[0];
                        ep.EpaymentTransactionRef = columnArray[1];
                        ep.BillerRef = columnArray[2];
                        ep.ServiceProvider = columnArray[3];
                        ep.BillerId = columnArray[4];
                        ep.BillerInfo1 = columnArray[5]; //YEAR
                        ep.Quarter = Quarter.FULL_YEAR; //CREATE QUARTER BECAUSE THERE'S NO QUARTER IN THE GCASH/PAYMAYA EXCEL FILE.
                        ep.BillerInfo2 = columnArray[6];
                        ep.BillingSelection = BillingSelectionUtil.CLASS1; //CREATE BILLING SELECTION BECAUSE THERE'S NO B.SELECTION IN THE GCASH/PAYMAYA EXCEL FILE.
                        ep.BillerInfo3 = columnArray[7];
                        ep.AmountDue = Convert.ToDecimal(columnArray[9]);

                        if (DateTime.TryParse(columnArray[12], out DateTime date))
                        {
                            ep.Date = date;
                        }
                        data.Add(ep);
                    }
                    else
                    {
                        MessageBox.Show("Invalid copy of data.");
                    }
                }
                dgRptList.DataSource = data;
            }
        }

        private void btnSaveAll_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are your sure?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                GenerateTemporaryTaxesList();

                try
                {
                    rptService.SaveAllEPayment(rptToSaveList, miscToSaveList, businessToSaveList);
                    NotificationHelper.notifyUserAndRefreshRecord(firstRecordSearchMainFormRef);
                    btnClose_Click(sender, e);
                }
                catch (RptException ex)
                {
                    MessageBox.Show(ex.Message);
                    return;
                }
            }
        }

        private void GenerateTemporaryTaxesList()
        {
            rptToSaveList.Clear();
            businessToSaveList.Clear();
            miscToSaveList.Clear();

            foreach (DataGridViewRow row in dgRptList.Rows)
            {
                ElectronicPayment ep = (ElectronicPayment)row.DataBoundItem;
                //RPT
                if (taxUniqueKeyFormat.isRPTTaxDecFormat(ep.BillerId))
                {
                    rptToSaveList.Add(ProcessEPaymentRpt(ep));
                    firstRecordSearchMainFormRef = ep.BillerId;
                }
                //BUSINESS
                else if (taxUniqueKeyFormat.isOPnumberFormatBusiness(ep.BillerRef))
                {
                    businessToSaveList.Add(ProcessEPaymentBusiness(ep));
                    firstRecordSearchMainFormRef = ep.BillerRef;
                }
                //MISC - OCCUPATIONAL PERMIT
                else if (taxUniqueKeyFormat.isOPnumberFormatOccuPermit(ep.BillerRef))
                {
                    miscToSaveList.Add(ProcessEPaymentMiscOccuPermit(ep));
                    firstRecordSearchMainFormRef = ep.BillerRef;
                }
                //MISC - OVR TTMD
                else if (taxUniqueKeyFormat.isOPnumberFormatOvrTTMD(ep.BillerRef))
                {
                    miscToSaveList.Add(ProcessEPaymentMiscOvrTtmd(ep));
                    firstRecordSearchMainFormRef = ep.BillerRef;
                }
                //MISC - OVR DPOS
                else if (taxUniqueKeyFormat.isOPnumberFormatOvrDPOS(ep.BillerRef))
                {
                    miscToSaveList.Add(ProcessEPaymentMiscOvrDpos(ep));
                    firstRecordSearchMainFormRef = ep.BillerRef;
                }
                //MISC - MARKET
                else if (taxUniqueKeyFormat.isOPnumberFormatMarketMDAD(ep.BillerInfo3))
                {
                    miscToSaveList.Add(ProcessEPaymentMiscMarket(ep));
                    firstRecordSearchMainFormRef = ep.BillerRef;
                }
                //MISC - ZONING
                else if (taxUniqueKeyFormat.isOPnumberFormatZoning(ep.BillerRef))
                {
                    miscToSaveList.Add(ProcessEPaymentMiscZoning(ep));
                    firstRecordSearchMainFormRef = ep.BillerRef;
                }
            }
        }

        private Rpt ProcessEPaymentRpt(ElectronicPayment ep)
        {
            Rpt rpt = ConversionHelper.ConvertToRpt(ep);
            return rpt;
        }

        private Miscellaneous ProcessEPaymentMiscOccuPermit(ElectronicPayment ep)
        {
            Miscellaneous misc = ConversionHelper.ConvertToMiscOccuPermit(ep);
            return misc;
        }

        private Miscellaneous ProcessEPaymentMiscOvrTtmd(ElectronicPayment ep)
        {
            Miscellaneous misc = ConversionHelper.ConvertToMiscOvrTtmd(ep);
            return misc;
        }

        private Miscellaneous ProcessEPaymentMiscOvrDpos(ElectronicPayment ep)
        {
            Miscellaneous misc = ConversionHelper.ConvertToMiscOvrDpos(ep);
            return misc;
        }
        private Miscellaneous ProcessEPaymentMiscMarket(ElectronicPayment ep)
        {
            Miscellaneous misc = ConversionHelper.ConvertToMiscMarket(ep);
            return misc;
        }

        private Miscellaneous ProcessEPaymentMiscZoning(ElectronicPayment ep)
        {
            Miscellaneous misc = ConversionHelper.ConvertToMiscZoning(ep);
            return misc;
        }

        private Business ProcessEPaymentBusiness(ElectronicPayment ep)
        {
            Business bus = ConversionHelper.ConvertToBusiness(ep);
            return bus;
        }

        private void btnPrintReport_Click(object sender, EventArgs e)
        {
            GenerateTemporaryTaxesList();

            GenerateSheet(rptToSaveList);

            /*
            object Nothing = System.Reflection.Missing.Value;
            var app = new Microsoft.Office.Interop.Excel.Application();
            app.Visible = true;
            Microsoft.Office.Interop.Excel.Workbook workBook = app.Workbooks.Add(Nothing);
            Microsoft.Office.Interop.Excel.Worksheet worksheet = (Microsoft.Office.Interop.Excel.Worksheet)workBook.Sheets[1];
            worksheet.Name = "WorkSheet";

            GenerateTemporaryTaxesList();
            GenerateSheet(workBook, "RPT", rptToSaveList);
            */
        }

        private void GenerateSheet(List<Rpt>  rptToSaveList)
        {
            // Generate a filename in My Documents folder.
            string fileName = $"GcashPayMaya_{DateTime.Now:yyyyMMdd_HHmmss}_{Guid.NewGuid()}.xlsx";
            string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), fileName);

            // Create the excel file
            using (var spreadsheetDocument = SpreadsheetDocument.Create(filePath, SpreadsheetDocumentType.Workbook))
            {
                // Add a WorkbookPart to the document
                var workbookPart = spreadsheetDocument.AddWorkbookPart();
                workbookPart.Workbook = new Workbook();

                // Add a WorksheetPart to the WorkbookPart
                var worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
                worksheetPart.Worksheet = new Worksheet(new SheetData());

                // Add a Sheets to the Workbook
                var sheets = spreadsheetDocument.WorkbookPart.Workbook.AppendChild(new Sheets());
                sheets.Append(new Sheet() { Id = spreadsheetDocument.WorkbookPart.GetIdOfPart(worksheetPart), SheetId = 1, Name = "Sheet1" });

                // Get the sheet data
                var sheetData = worksheetPart.Worksheet.GetFirstChild<SheetData>();

                // Add data to the sheet
                var row = new Row();
                row.Append(CreateCell("A1", "Header1"));
                row.Append(CreateCell("B1", "Header2"));
                sheetData.AppendChild(row);

                for (int i = 1; i <= 5; i++)
                {
                    row = new Row();
                    row.Append(CreateCell($"A{i + 1}", $"Data{i}_Column1"));
                    row.Append(CreateCell($"B{i + 1}", $"Data{i}_Column2"));
                    sheetData.AppendChild(row);
                }
            }

            //Open the excel file
            System.Diagnostics.Process.Start("excel.exe", filePath);
        }

        private static Cell CreateCell(string cellReference, string cellValue)
        {
            // Create a cell with specified reference and value
            var cell = new Cell()
            {
                CellReference = cellReference,
                DataType = CellValues.String,
                CellValue = new CellValue(cellValue)
            };

            return cell;
        }

        /*
        private void GenerateSheet(Microsoft.Office.Interop.Excel.Workbook workBook, string sheetName, List<Rpt> rptToSaveList)
        {
            Microsoft.Office.Interop.Excel.Worksheet worksheet = (Microsoft.Office.Interop.Excel.Worksheet)workBook.Sheets.Add(Type.Missing, workBook.Sheets[workBook.Sheets.Count], 1, Microsoft.Office.Interop.Excel.XlSheetType.xlWorksheet);
            worksheet.Name = sheetName;

            if (taxUniqueKeyFormat.isRPTTaxDecFormat(ep.BillerId))
            {
                int row = 5;
                int counter = 1;

                foreach (Rpt rpt in rptToSaveList)
                {
                    worksheet.Cells[row, 1] = rpt.Bank; 
                    worksheet.Cells[row, 2] = rpt.Bank; 
                    worksheet.Cells[row, 3] = rpt.TaxDec; 
                    worksheet.Cells[row, 4] = rpt.YearQuarter; 
                    worksheet.Cells[row, 5] = rpt.TaxPayerName; 
                    worksheet.Cells[row, 6] = rpt.AmountTransferred; 
                    worksheet.Cells[row, 7] = rpt.PaymentDate; 
                    row++;
                    counter++;
                }

                worksheet.Cells[row, 5] = "Net Amount:";
                //worksheet.Cells[row, 5].Style.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;
                worksheet.Range[worksheet.Cells[row, 5], worksheet.Cells[row, 5]].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;
                worksheet.Cells[row, 6] = $"=sum(F5:F{row - 1})";
            }
        }
        */

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSaveAll_MouseEnter(object sender, EventArgs e)
        {
            originalBackgroundImageNonRpt = btnSaveAll.BackgroundImage;
            btnSaveAll.BackgroundImage = null;

            Color customColor = Color.FromArgb(23, 45, 74);
            btnSaveAll.BackColor = customColor;
        }

        private void btnSaveAll_MouseLeave(object sender, EventArgs e)
        {
            btnSaveAll.BackgroundImage = originalBackgroundImageNonRpt;
        }

    }
}
