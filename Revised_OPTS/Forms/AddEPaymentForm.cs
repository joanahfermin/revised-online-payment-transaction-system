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
using Inventory_System.DAL;
using Revised_OPTS.DAL;

namespace Inventory_System.Forms
{
    public partial class AddEPaymentForm : Form
    {
        List<ElectronicPayment> data = new List<ElectronicPayment>();

        private Image originalBackgroundImageNonRpt;
        private Image originalBackgroundImageRpt;

        private DynamicGridContainer<Rpt> DynamicGridContainer;

        IRptService rptService = ServiceFactory.Instance.GetRptService();
        IBusinessMasterDetailTPNRepository busMasterdetailTPNRepository = RepositoryFactory.Instance.GetBusinessRetrieveTaxpayerNameRepository();


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
                    rptService.SaveAllEPayment(rptToSaveList, businessToSaveList, miscToSaveList);
                    NotificationHelper.notifyUserAndRefreshRecord(firstRecordSearchMainFormRef);
                    btnClose_Click(sender, e);
                }
                catch (DuplicateRecordException ex)
                {
                    MessageBox.Show(ex.Message);

                    ExistingRecordForm DuplicateForm = new ExistingRecordForm(ex.duplicateRptList, ex.duplicateBusList, ex.duplicateMiscList);
                    DuplicateForm.ShowDialog();
                    return;
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
                    firstRecordSearchMainFormRef = ep.BillerInfo3;
                }
                //MISC - ZONING
                else if (taxUniqueKeyFormat.isOPnumberFormatZoning(ep.BillerRef))
                {
                    miscToSaveList.Add(ProcessEPaymentMiscZoning(ep));
                    firstRecordSearchMainFormRef = ep.BillerRef;
                }
                //MISC - LIQUOR
                else if (taxUniqueKeyFormat.isOPnumberFormatLiquorLLRB(ep.BillerRef))
                {
                    miscToSaveList.Add(ProcessEPaymentMiscLiquor(ep));
                    firstRecordSearchMainFormRef = ep.BillerRef;
                }

            }

            List<string> billNumberList = businessToSaveList.Select(b => b.BillNumber).ToList();
            List<BusinessMasterDetailTPN> retrievedNames = busMasterdetailTPNRepository.retrieveByBillNumber(billNumberList);

            foreach (Business bus in businessToSaveList)
            {
                if (bus.TaxpayersName == null || bus.TaxpayersName == String.Empty)
                {
                    foreach (BusinessMasterDetailTPN busName in retrievedNames)
                    {
                        if (bus.BillNumber == busName.BillNo)
                        {
                            bus.TaxpayersName = busName.TaxpayerName;
                            bus.BusinessName = busName.BusinessName;
                        }
                    }
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

        private Miscellaneous ProcessEPaymentMiscLiquor(ElectronicPayment ep)
        {
            Miscellaneous misc = ConversionHelper.ConvertToMiscLiquor(ep);
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
            GenerateSheet(rptToSaveList, businessToSaveList, miscToSaveList);
        }

        private void GenerateSheet(List<Rpt> rptToSaveList, List<Business> businessToSaveList, List<Miscellaneous> miscToSaveList)
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
                Sheets sheets = spreadsheetDocument.WorkbookPart.Workbook.AppendChild(new Sheets());

                CreateRptSheet(sheets, workbookPart, spreadsheetDocument, rptToSaveList);
                CreateBusinessSheet(sheets, workbookPart, spreadsheetDocument, businessToSaveList);
                CreateMiscSheet(sheets, workbookPart, spreadsheetDocument, miscToSaveList);
            }

            //Open the excel file
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo { FileName = filePath, UseShellExecute = true });
        }

        private void CreateRptSheet(Sheets sheets, WorkbookPart workbookPart, SpreadsheetDocument spreadsheetDocument, List<Rpt> rptToSaveList)
        {
            // Add a WorksheetPart to the WorkbookPart
            var worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
            worksheetPart.Worksheet = new Worksheet(new SheetData());

            // Add styles to the cells
            WorkbookStylesPart stylePart = workbookPart.AddNewPart<WorkbookStylesPart>();
            stylePart.Stylesheet = GenerateStylesheet();
            stylePart.Stylesheet.Save();

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
            decimal totalAmountTransferred = 0;

            foreach (var rpt in rptToSaveList)
            {
                row = new Row();
                row.Append(CreateCell($"A{rowIndex}", count.ToString()));
                row.Append(CreateCell($"B{rowIndex}", rpt.Bank));
                row.Append(CreateCell($"C{rowIndex}", rpt.TaxDec));
                row.Append(CreateCell($"D{rowIndex}", rpt.YearQuarter));
                row.Append(CreateCell($"E{rowIndex}", rpt.TaxPayerName));
                row.Append(CreateDecimalCell($"F{rowIndex}", rpt.AmountTransferred ?? 0));
                row.Append(CreateCell($"G{rowIndex}", rpt.PaymentDate.ToString()));
                sheetData.AppendChild(row);

                totalAmountTransferred += rpt.AmountTransferred ?? 0;

                count++;
                rowIndex++;
            }
            // Add a row for the total
            row = new Row();
            row.Append(CreateCell($"A{rowIndex}", ""));
            row.Append(CreateCell($"B{rowIndex}", ""));
            row.Append(CreateCell($"C{rowIndex}", ""));
            row.Append(CreateCell($"D{rowIndex}", ""));
            row.Append(CreateCell($"E{rowIndex}", "TOTAL PAYMENT"));
            row.Append(CreateDecimalCell($"F{rowIndex}", totalAmountTransferred));
            row.Append(CreateCell($"G{rowIndex}", ""));
            sheetData.AppendChild(row);
        }

        private void CreateBusinessSheet(Sheets sheets, WorkbookPart workbookPart, SpreadsheetDocument spreadsheetDocument, List<Business> businessToSaveList)
        {
            // Add a WorksheetPart to the WorkbookPart
            var worksheetPartBusiness = workbookPart.AddNewPart<WorksheetPart>();
            worksheetPartBusiness.Worksheet = new Worksheet(new SheetData());

            //// Add styles to the cells
            //WorkbookStylesPart stylePart = workbookPart.AddNewPart<WorkbookStylesPart>();
            //stylePart.Stylesheet = GenerateStylesheet();
            //stylePart.Stylesheet.Save();

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
            bussinessRow.Append(CreateCell("C5", "TAXPAYER'S NAME"));
            bussinessRow.Append(CreateCell("D5", "SERVICE PROVIDER"));
            bussinessRow.Append(CreateCell("E5", "MP NUMBER"));
            bussinessRow.Append(CreateCell("F5", "AMOUNT DUE"));
            bussinessRow.Append(CreateCell("G5", "PAYMENT DATE"));
            businessSheetData.AppendChild(bussinessRow);

            int bussinessRowIndex = 6;
            int count = 1;
            decimal totalAmountTransferred = 0;

            foreach (Business bus in businessToSaveList)
            {
                bussinessRow = new Row(); // Create a new row instance for each iteration
                bussinessRow.Append(CreateCell($"A{bussinessRowIndex}", count.ToString()));
                bussinessRow.Append(CreateCell($"B{bussinessRowIndex}", bus.BillNumber));
                bussinessRow.Append(CreateCell($"C{bussinessRowIndex}", bus.TaxpayersName));
                bussinessRow.Append(CreateCell($"D{bussinessRowIndex}", bus.PaymentChannel));
                bussinessRow.Append(CreateCell($"E{bussinessRowIndex}", bus.MP_Number));
                bussinessRow.Append(CreateDecimalCell($"F{bussinessRowIndex}", bus.TotalAmount ?? 0));
                bussinessRow.Append(CreateCell($"G{bussinessRowIndex}", bus.DateOfPayment.ToString()));
                businessSheetData.AppendChild(bussinessRow);

                totalAmountTransferred += bus.TotalAmount ?? 0;

                count++;
                bussinessRowIndex++;
            }
            // Add a row for the total
            bussinessRow = new Row();
            bussinessRow.Append(CreateCell($"A{bussinessRowIndex}", ""));
            bussinessRow.Append(CreateCell($"B{bussinessRowIndex}", ""));
            bussinessRow.Append(CreateCell($"C{bussinessRowIndex}", ""));
            bussinessRow.Append(CreateCell($"D{bussinessRowIndex}", "TOTAL PAYMENT"));
            bussinessRow.Append(CreateDecimalCell($"E{bussinessRowIndex}", totalAmountTransferred));
            bussinessRow.Append(CreateCell($"F{bussinessRowIndex}", ""));
            bussinessRow.Append(CreateCell($"G{bussinessRowIndex}", ""));
            businessSheetData.AppendChild(bussinessRow);
        }

        private void CreateMiscSheet(Sheets sheets, WorkbookPart workbookPart, SpreadsheetDocument spreadsheetDocument, List<Miscellaneous> miscToSaveList)
        {
            UInt32 sheetCounter = 3;
            foreach (string miscType in TaxTypeUtil.ALL_MISC_TAX_TYPE)
            {
                // Add a WorksheetPart to the WorkbookPart
                var worksheetPartMisc = workbookPart.AddNewPart<WorksheetPart>();
                worksheetPartMisc.Worksheet = new Worksheet(new SheetData());

                //// Add styles to the cells
                //WorkbookStylesPart stylePart = workbookPart.AddNewPart<WorkbookStylesPart>();
                //stylePart.Stylesheet = GenerateStylesheet();
                //stylePart.Stylesheet.Save();

                sheets.Append(new Sheet() { Id = spreadsheetDocument.WorkbookPart.GetIdOfPart(worksheetPartMisc), SheetId = sheetCounter, Name = miscType });
                var miscSheetData = worksheetPartMisc.Worksheet.GetFirstChild<SheetData>();
                sheetCounter++;

                // Add data to the sheet
                miscSheetData.AppendChild(new Row());
                miscSheetData.AppendChild(new Row());
                miscSheetData.AppendChild(new Row());
                miscSheetData.AppendChild(new Row());

                var miscRow = new Row();
                miscRow.Append(CreateCell("A5", ""));
                miscRow.Append(CreateCell("B5", "BILL NUMBER"));
                miscRow.Append(CreateCell("C5", "SERVICE PROVIDER"));
                miscRow.Append(CreateCell("D5", "OPA TRACKING NUMBER"));
                miscRow.Append(CreateCell("E5", "TAXPAYER NAME"));
                miscRow.Append(CreateCell("F5", "AMOUNT DUE"));
                miscRow.Append(CreateCell("G5", "PAYMENT DATE"));
                miscSheetData.AppendChild(miscRow);

                int miscRowIndex = 6;
                int count = 1;
                decimal totalAmountTransferred = 0;

                foreach (Miscellaneous misc in miscToSaveList)
                {
                    if (miscType == misc.MiscType)
                    {
                        miscRow = new Row(); // Create a new row instance for each iteration
                        miscRow.Append(CreateCell($"A{miscRowIndex}", count.ToString()));
                        miscRow.Append(CreateCell($"B{miscRowIndex}", misc.OrderOfPaymentNum));
                        miscRow.Append(CreateCell($"C{miscRowIndex}", misc.ModeOfPayment));
                        miscRow.Append(CreateCell($"D{miscRowIndex}", misc.OPATrackingNum));
                        miscRow.Append(CreateCell($"E{miscRowIndex}", misc.TaxpayersName));
                        miscRow.Append(CreateDecimalCell($"F{miscRowIndex}", misc.TransferredAmount ?? 0));
                        miscRow.Append(CreateCell($"G{miscRowIndex}", misc.PaymentDate.ToString()));
                        miscSheetData.AppendChild(miscRow);

                        totalAmountTransferred += misc.TransferredAmount ?? 0;

                        count++;
                        miscRowIndex++;
                    }
                }
                // Add a row for the total
                miscRow = new Row();
                miscRow.Append(CreateCell($"A{miscRowIndex}", ""));
                miscRow.Append(CreateCell($"B{miscRowIndex}", ""));
                miscRow.Append(CreateCell($"C{miscRowIndex}", ""));
                miscRow.Append(CreateCell($"D{miscRowIndex}", ""));
                miscRow.Append(CreateCell($"E{miscRowIndex}", "TOTAL PAYMENT"));
                miscRow.Append(CreateDecimalCell($"F{miscRowIndex}", totalAmountTransferred));
                miscRow.Append(CreateCell($"G{miscRowIndex}", ""));
                miscSheetData.AppendChild(miscRow);
            }
        }

        private static Stylesheet GenerateStylesheet()
        {
            Stylesheet ss = new Stylesheet();

            Fonts fts = new Fonts();
            DocumentFormat.OpenXml.Spreadsheet.Font ft = new DocumentFormat.OpenXml.Spreadsheet.Font();
            FontName ftn = new FontName();
            ftn.Val = "Calibri";
            FontSize ftsz = new FontSize();
            ftsz.Val = 11;
            ft.FontName = ftn;
            ft.FontSize = ftsz;
            fts.Append(ft);
            fts.Count = (uint)fts.ChildElements.Count;

            Fills fills = new Fills();
            Fill fill;
            PatternFill patternFill;
            fill = new Fill();
            patternFill = new PatternFill();
            patternFill.PatternType = PatternValues.None;
            fill.PatternFill = patternFill;
            fills.Append(fill);
            fill = new Fill();
            patternFill = new PatternFill();
            patternFill.PatternType = PatternValues.Gray125;
            fill.PatternFill = patternFill;
            fills.Append(fill);
            fills.Count = (uint)fills.ChildElements.Count;

            Borders borders = new Borders();
            Border border = new Border();
            border.LeftBorder = new LeftBorder();
            border.RightBorder = new RightBorder();
            border.TopBorder = new TopBorder();
            border.BottomBorder = new BottomBorder();
            border.DiagonalBorder = new DiagonalBorder();
            borders.Append(border);
            borders.Count = (uint)borders.ChildElements.Count;

            CellStyleFormats csfs = new CellStyleFormats();
            CellFormat cf = new CellFormat();
            cf.NumberFormatId = 0;
            cf.FontId = 0;
            cf.FillId = 0;
            cf.BorderId = 0;
            csfs.Append(cf);
            csfs.Count = (uint)csfs.ChildElements.Count;

            uint iExcelIndex = 164;
            NumberingFormats nfs = new NumberingFormats();
            CellFormats cfs = new CellFormats();

            cf = new CellFormat();
            cf.NumberFormatId = 0;
            cf.FontId = 0;
            cf.FillId = 0;
            cf.BorderId = 0;
            cf.FormatId = 0;
            cfs.Append(cf);

            NumberingFormat nf;
            nf = new NumberingFormat();
            nf.NumberFormatId = iExcelIndex++;
            nf.FormatCode = "dd/mm/yyyy hh:mm:ss";
            nfs.Append(nf);
            cf = new CellFormat();
            cf.NumberFormatId = nf.NumberFormatId;
            cf.FontId = 0;
            cf.FillId = 0;
            cf.BorderId = 0;
            cf.FormatId = 0;
            cf.ApplyNumberFormat = true;
            cfs.Append(cf);

            nf = new NumberingFormat();
            nf.NumberFormatId = iExcelIndex++;
            nf.FormatCode = "#,##0.0000";
            nfs.Append(nf);
            cf = new CellFormat();
            cf.NumberFormatId = nf.NumberFormatId;
            cf.FontId = 0;
            cf.FillId = 0;
            cf.BorderId = 0;
            cf.FormatId = 0;
            cf.ApplyNumberFormat = true;
            cfs.Append(cf);

            // #,##0.00 is also Excel style index 4
            nf = new NumberingFormat();
            nf.NumberFormatId = iExcelIndex++;
            nf.FormatCode = "#,##0.00";
            nfs.Append(nf);
            cf = new CellFormat();
            cf.NumberFormatId = nf.NumberFormatId;
            cf.FontId = 0;
            cf.FillId = 0;
            cf.BorderId = 0;
            cf.FormatId = 0;
            cf.ApplyNumberFormat = true;
            cfs.Append(cf);

            // @ is also Excel style index 49
            nf = new NumberingFormat();
            nf.NumberFormatId = iExcelIndex++;
            nf.FormatCode = "@";
            nfs.Append(nf);
            cf = new CellFormat();
            cf.NumberFormatId = nf.NumberFormatId;
            cf.FontId = 0;
            cf.FillId = 0;
            cf.BorderId = 0;
            cf.FormatId = 0;
            cf.ApplyNumberFormat = true;
            cfs.Append(cf);

            nfs.Count = (uint)nfs.ChildElements.Count;
            cfs.Count = (uint)cfs.ChildElements.Count;

            ss.Append(nfs);
            ss.Append(fts);
            ss.Append(fills);
            ss.Append(borders);
            ss.Append(csfs);
            ss.Append(cfs);

            CellStyles css = new CellStyles();
            CellStyle cs = new CellStyle();
            cs.Name = "Normal";
            cs.FormatId = 0;
            cs.BuiltinId = 0;
            css.Append(cs);
            css.Count = (uint)css.ChildElements.Count;
            ss.Append(css);

            DifferentialFormats dfs = new DifferentialFormats();
            dfs.Count = 0;
            ss.Append(dfs);

            TableStyles tss = new TableStyles();
            tss.Count = 0;
            tss.DefaultTableStyle = "TableStyleMedium9";
            tss.DefaultPivotStyle = "PivotStyleLight16";
            ss.Append(tss);

            return ss;
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

        private static Cell CreateDecimalCell(string cellReference, decimal cellValue)
        {
            // Create a cell with specified reference and value
            var cell = new Cell()
            {
                CellReference = cellReference,
                DataType = CellValues.Number, // tell excel this is a number
                CellValue = new CellValue(cellValue),
                StyleIndex = 3 // Use #,##0.00
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
