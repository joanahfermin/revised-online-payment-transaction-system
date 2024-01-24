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
                        ElectronicPayment ep = new ElectronicPayment();//xxx
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
            //add data to each taxes list to be able to utilize later for the report
            GenerateTemporaryTaxesList();
            GenerateSheet(rptToSaveList, businessToSaveList);
        }

        private void GenerateSheet(List<Rpt>  rptToSaveList, List<Business> businessToSaveList)
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

                // Add a Sheets to the Workbook
                var sheets = spreadsheetDocument.WorkbookPart.Workbook.AppendChild(new Sheets());

                // Add a WorksheetPart to the WorkbookPart
                var worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
                worksheetPart.Worksheet = new Worksheet(new SheetData());

                // Add sheet to the document
                sheets.Append(new Sheet() { Id = spreadsheetDocument.WorkbookPart.GetIdOfPart(worksheetPart), SheetId = 1, Name = "RPT" });
                // Get the sheet data
                var sheetData = worksheetPart.Worksheet.GetFirstChild<SheetData>();

                // Add data to the sheet
                sheetData.AppendChild(new Row());
                sheetData.AppendChild(new Row());
                sheetData.AppendChild(new Row());
                sheetData.AppendChild(new Row());

                var row = new Row();
                row.Append(CreateCell("A5", ""));
                row.Append(CreateCell("B5", "BANK"));
                row.Append(CreateCell("C5", "TAX DEC. NO."));
                row.Append(CreateCell("D5", "YEAR"));
                row.Append(CreateCell("E5", "TAXPAYER NAME"));
                row.Append(CreateCell("F5", "AMOUNT DUE"));
                row.Append(CreateCell("G5", "PAYMENT DATE"));
                sheetData.AppendChild(row);

                int rowIndex = 6;
                int count = 1;

                foreach (var rpt in rptToSaveList)
                {
                    row = new Row();
                    row.Append(CreateCell($"A{rowIndex}", count.ToString()));
                    row.Append(CreateCell($"B{rowIndex}", rpt.Bank));
                    row.Append(CreateCell($"C{rowIndex}", rpt.TaxDec));
                    row.Append(CreateCell($"D{rowIndex}", rpt.YearQuarter));
                    row.Append(CreateCell($"E{rowIndex}", rpt.TaxPayerName));
                    row.Append(CreateCell($"F{rowIndex}", rpt.AmountTransferred?.ToString(("N2"))));
                    row.Append(CreateCell($"G{rowIndex}", rpt.PaymentDate.ToString()));
                    sheetData.AppendChild(row);
                    count++;
                    rowIndex++;
                }

                // Add a WorksheetPart to the WorkbookPart
                var worksheetPartBusiness = workbookPart.AddNewPart<WorksheetPart>();
                worksheetPartBusiness.Worksheet = new Worksheet(new SheetData());

                sheets.Append(new Sheet() { Id = spreadsheetDocument.WorkbookPart.GetIdOfPart(worksheetPartBusiness), SheetId = 2, Name = "BUSINESS" });
                var businessSheetData = worksheetPartBusiness.Worksheet.GetFirstChild<SheetData>();

                // Add data to the sheet
                businessSheetData.AppendChild(new Row());
                businessSheetData.AppendChild(new Row());
                businessSheetData.AppendChild(new Row());
                businessSheetData.AppendChild(new Row());

                var bussinessRow = new Row();
                bussinessRow.Append(CreateCell("A5", ""));
                bussinessRow.Append(CreateCell("B5", "BILL NUMBER"));
                bussinessRow.Append(CreateCell("C5", "SERVICE PROVIDER"));
                bussinessRow.Append(CreateCell("D5", "MP NUMBER"));
                bussinessRow.Append(CreateCell("E5", "AMOUNT DUE"));
                bussinessRow.Append(CreateCell("F5", "PAYMENT DATE"));
                businessSheetData.AppendChild(bussinessRow);

                int bussinessRowIndex = 6;
                count = 1;

                foreach (Business bus in businessToSaveList)
                {
                    bussinessRow = new Row(); // Create a new row instance for each iteration
                    bussinessRow.Append(CreateCell($"A{bussinessRowIndex}", count.ToString()));
                    bussinessRow.Append(CreateCell($"B{bussinessRowIndex}", bus.BillNumber));
                    bussinessRow.Append(CreateCell($"C{bussinessRowIndex}", bus.PaymentChannel));
                    bussinessRow.Append(CreateCell($"D{bussinessRowIndex}", bus.MP_Number));
                    bussinessRow.Append(CreateCell($"E{bussinessRowIndex}", bus.TotalAmount?.ToString(("N2"))));
                    bussinessRow.Append(CreateCell($"F{bussinessRowIndex}", bus.DateOfPayment.ToString()));
                    businessSheetData.AppendChild(bussinessRow);
                    count++;
                    bussinessRowIndex++;
                }
            }

            //Open the excel file
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo { FileName = filePath, UseShellExecute = true });
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

        private void btnSaveAll_MouseEnter(object sender, EventArgs e)
        {
            originalBackgroundImageNonRpt = btnSaveAll.BackgroundImage;
            btnSaveAll.BackgroundImage = null;

            System.Drawing.Color customColor = System.Drawing.Color.FromArgb(23, 45, 74);
            btnSaveAll.BackColor = customColor;
        }

        private void btnSaveAll_MouseLeave(object sender, EventArgs e)
        {
            btnSaveAll.BackgroundImage = originalBackgroundImageNonRpt;
        }

        private void btnClose_MouseEnter(object sender, EventArgs e)
        {
            originalBackgroundImageNonRpt = btnClose.BackgroundImage;
            btnClose.BackgroundImage = null;

            System.Drawing.Color customColor = System.Drawing.Color.FromArgb(23, 45, 74);
            btnClose.BackColor = customColor;
        }

        private void btnClose_MouseLeave(object sender, EventArgs e)
        {
            btnClose.BackgroundImage = originalBackgroundImageNonRpt;
        }

        private void btnPrintReport_MouseEnter(object sender, EventArgs e)
        {
            originalBackgroundImageNonRpt = btnPrintReport.BackgroundImage;
            btnPrintReport.BackgroundImage = null;

            System.Drawing.Color customColor = System.Drawing.Color.FromArgb(23, 45, 74);
            btnPrintReport.BackColor = customColor;
        }

        private void btnPrintReport_MouseLeave(object sender, EventArgs e)
        {
            btnPrintReport.BackgroundImage = originalBackgroundImageNonRpt;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
