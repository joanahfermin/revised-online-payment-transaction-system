using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
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

        //List<Rpt> rptToSaveList = new List<Rpt>();
        //List<Miscellaneous> miscToSaveList = new List<Miscellaneous>();
        //List<Business> businessToSaveList = new List<Business>();

        TaxUniqueKeyFormat taxUniqueKeyFormat = new TaxUniqueKeyFormat();

        List<CollectorsReport> data = new List<CollectorsReport>();

        public ReportForm()
        {
            InitializeComponent();
            InitializeReportType();

            dynamicControlContainer = new DynamicControlContainer(this);

            DgReportForm.DefaultCellStyle.Font = new System.Drawing.Font("Tahoma", 12, FontStyle.Regular);
            this.WindowState = FormWindowState.Maximized;

            DgReportForm.CellFormatting += DgReportForm_CellFormatting;
        }
        public void InitializeReportType()
        {
            foreach (string allReport in AllTaxTypeReportUtil.ALL_REPORT.OrderBy(reportType => reportType))
            {
                cbTaxTypeReport.Items.Add(allReport);
            }
            cbTaxTypeReport.SelectedIndex = 0;
        }

        public void InitializeDynamicMappingAllTaxesType()
        {
            DynamicGridInfo[] gridInfoArray = new DynamicGridInfo[] {
                new DynamicGridInfo{PropertyName="TaxType", Label = "Tax Type" },
                new DynamicGridInfo{PropertyName="BillNumber", Label = "Bill Number" },
                //new DynamicGridInfo{PropertyName="TaxpayerName", Label = "Taxpayer's Name" },
                new DynamicGridInfo{PropertyName="Collection", Label = "Bill Amount", decimalValue = true },
                new DynamicGridInfo{PropertyName="Billing", Label = "Total Amount Transferred", decimalValue = true },
                new DynamicGridInfo{PropertyName="ExcessShort", Label = "Excess/Short", decimalValue = true},
                new DynamicGridInfo{PropertyName="Remarks", Label = "Remarks"},
            };
            DynamicGridContainer = new DynamicGridContainer<AllTaxTypeReport>(DgReportForm, gridInfoArray, true, true);
        }

        private void DgReportForm_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.Value is decimal decimalValue)
            {
                e.Value = decimalValue.ToString("N2");
                e.FormattingApplied = true;
            }
        }

        private void loadCollectorsReport()
        {
            InitializeDynamicMappingAllTaxesType();

            List<AllTaxTypeReport> allTaxesValidated = rptService.RetrieveByValidatedDate(dtFrom.Value, dtTo.Value);
            DgReportForm.DataSource = allTaxesValidated;
        }

        private void dtFrom_ValueChanged(object sender, EventArgs e)
        {
            loadCollectorsReport();
        }

        private void dtTo_ValueChanged(object sender, EventArgs e)
        {
            loadCollectorsReport();
        }

        private void cbTaxType_SelectedIndexChanged(object sender, EventArgs e)
        {
            string taxType = cbTaxTypeReport.Text;
            DgReportForm.Rows.Clear();
            DgReportForm.Columns.Clear();

            if (taxType == AllTaxTypeReportUtil.COLLECTORS_REPORT)
            {
                dtFrom.Visible = true;
                dtTo.Visible = true;

                labelEnterRefNo.Visible = false;
                tbRefNo.Visible = false;
                labelDateFrom.Visible = true;
                labelDateTo.Visible = true;
            }
            else if (taxType == AllTaxTypeReportUtil.REGENERATE_EPAYMENTS)
            {
                labelEnterRefNo.Visible = true;
                tbRefNo.Visible = true;

                labelDateFrom.Visible = false;
                labelDateTo.Visible = false;
                dtFrom.Visible = false;
                dtTo.Visible = false;
            }
            else if (taxType == AllTaxTypeReportUtil.USER_ACTIVITY)
            {
                labelEnterRefNo.Visible = false;
                tbRefNo.Visible = false;

                labelDateFrom.Visible = true;
                labelDateTo.Visible = true;
                dtFrom.Visible = true;
                dtTo.Visible = true;
            }
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            GenerateSheet();
        }

        private void GenerateSheet(/*List<Rpt> rptToSaveList, List<Business> businessToSaveList, List<Miscellaneous> miscToSaveList*/)
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

                CreateRptSheet(sheets, workbookPart, spreadsheetDocument/*, rptToSaveList*/);
                CreateBusinessSheet(sheets, workbookPart, spreadsheetDocument/*, businessToSaveList*/);
                CreateMiscSheet(sheets, workbookPart, spreadsheetDocument/*, miscToSaveList*/);
            }

            //Open the excel file
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo { FileName = filePath, UseShellExecute = true });
        }
        
        private void CreateRptSheet(Sheets sheets, WorkbookPart workbookPart, SpreadsheetDocument spreadsheetDocument/*, List<Rpt> rptToSaveList*/)
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

            row.Append(CreateCell("B5", "BILL NUMBER"));
            row.Append(CreateCell("C5", "BILL AMOUNT"));
            row.Append(CreateCell("D5", "TOTAL TRANSFERRED AMOUNT"));
            row.Append(CreateCell("E5", "EXCESS/SHORT"));
            row.Append(CreateCell("F5", "REMARKS"));
            sheetData.AppendChild(row);

            int rowIndex = 6;
            int count = 1;
            decimal totalBillAmount = 0;
            decimal totalAmountTransferred = 0;
            decimal totalExcessShort = 0;

            foreach (DataGridViewRow gridrow in DgReportForm.Rows)
            {
                AllTaxTypeReport taxRow = (AllTaxTypeReport)gridrow.DataBoundItem;
            //foreach (var rpt in rptToSaveList)
            //{
                if (taxRow.TaxType != "RPT")
                {
                    continue;
                }
                row = new Row();
                row.Append(CreateCell($"A{rowIndex}", count.ToString()));
                row.Append(CreateCell($"B{rowIndex}", taxRow.BillNumber));
                row.Append(CreateDecimalCell($"C{rowIndex}", taxRow.Billing ?? 0));
                row.Append(CreateDecimalCell($"D{rowIndex}", taxRow.Collection?? 0));
                row.Append(CreateDecimalCell($"E{rowIndex}", taxRow.ExcessShort ?? 0));
                row.Append(CreateCell($"F{rowIndex}", taxRow.Remarks));
                sheetData.AppendChild(row);

                totalBillAmount += taxRow.Billing ?? 0;
                totalAmountTransferred += taxRow.Collection ?? 0;
                totalExcessShort += taxRow.ExcessShort ?? 0;

                count++;
                rowIndex++;
            }
            // Add a row for the total
            row = new Row();
            row.Append(CreateCell($"A{rowIndex}", ""));
            row.Append(CreateCell($"B{rowIndex}", ""));
            row.Append(CreateDecimalCell($"C{rowIndex}", totalBillAmount));
            row.Append(CreateDecimalCell($"D{rowIndex}", totalAmountTransferred));
            row.Append(CreateDecimalCell($"E{rowIndex}", totalExcessShort));
            row.Append(CreateCell($"F{rowIndex}", ""));
            sheetData.AppendChild(row);
        }

        private void CreateBusinessSheet(Sheets sheets, WorkbookPart workbookPart, SpreadsheetDocument spreadsheetDocument/*, List<Business> businessToSaveList*/)
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
            bussinessRow.Append(CreateCell("D5", "BILL AMOUNT"));
            bussinessRow.Append(CreateCell("E5", "AMOUNT DUE"));
            bussinessRow.Append(CreateCell("F5", "REMARKS"));
            businessSheetData.AppendChild(bussinessRow);

            int bussinessRowIndex = 6;
            int count = 1;
            decimal totalBillAmount = 0;
            decimal totalAmountTransferred = 0;

            foreach (DataGridViewRow gridrow in DgReportForm.Rows)
            {
                AllTaxTypeReport taxRow = (AllTaxTypeReport)gridrow.DataBoundItem;
                if (taxRow.TaxType != "BUSINESS")
                {
                    continue;
                }

                bussinessRow = new Row(); // Create a new row instance for each iteration
                bussinessRow.Append(CreateCell($"A{bussinessRowIndex}", count.ToString()));
                bussinessRow.Append(CreateCell($"B{bussinessRowIndex}", taxRow.BillNumber));
                bussinessRow.Append(CreateDecimalCell($"C{bussinessRowIndex}", taxRow.Billing ?? 0));
                bussinessRow.Append(CreateDecimalCell($"D{bussinessRowIndex}", taxRow.Collection ?? 0));
                bussinessRow.Append(CreateCell($"E{bussinessRowIndex}", taxRow.Remarks));
                businessSheetData.AppendChild(bussinessRow);

                totalBillAmount += taxRow.Billing ?? 0;
                totalAmountTransferred += taxRow.Collection ?? 0;

                count++;
                bussinessRowIndex++;
            }
            // Add a row for the total
            bussinessRow = new Row();
            bussinessRow.Append(CreateCell($"A{bussinessRowIndex}", ""));
            bussinessRow.Append(CreateCell($"B{bussinessRowIndex}", ""));
            bussinessRow.Append(CreateDecimalCell($"C{bussinessRowIndex}", totalBillAmount));
            bussinessRow.Append(CreateDecimalCell($"D{bussinessRowIndex}", totalAmountTransferred));
            bussinessRow.Append(CreateCell($"E{bussinessRowIndex}", ""));
            businessSheetData.AppendChild(bussinessRow);
        }

        private void CreateMiscSheet(Sheets sheets, WorkbookPart workbookPart, SpreadsheetDocument spreadsheetDocument/*, List<Miscellaneous> miscToSaveList*/)
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

                foreach (DataGridViewRow gridrow in DgReportForm.Rows)
                {
                    AllTaxTypeReport taxRow = (AllTaxTypeReport)gridrow.DataBoundItem;
                    if (taxRow.TaxType != "MISC")
                    {
                        continue;
                    }
                    if (
                        (miscType == TaxTypeUtil.MISCELLANEOUS_OCCUPERMIT && !SearchBusinessFormat.isMiscOccuPermit(taxRow.BillNumber)) ||
                        //(miscType == TaxTypeUtil.MISCELLANEOUS_ && !SearchBusinessFormat.isMiscOvrDpos(taxRow.BillNumber)) ||
                        //(miscType == TaxTypeUtil.MISCELLANEOUS_ && !SearchBusinessFormat.isMiscOvrTtmd(taxRow.BillNumber)) ||
                        (miscType == TaxTypeUtil.MISCELLANEOUS_MARKET && !SearchBusinessFormat.isMiscMarket(taxRow.BillNumber)) ||
                        (miscType == TaxTypeUtil.MISCELLANEOUS_ZONING && !SearchBusinessFormat.isMiscZoning(taxRow.BillNumber)) ||
                        (miscType == TaxTypeUtil.MISCELLANEOUS_LIQUOR && !SearchBusinessFormat.isMiscLiquor(taxRow.BillNumber)) 
                        )
                    {
                        continue;
                    }

                    //foreach (Miscellaneous misc in miscToSaveList)
                    //{
                        miscRow = new Row(); // Create a new row instance for each iteration
                        miscRow.Append(CreateCell($"A{miscRowIndex}", count.ToString()));
                        miscRow.Append(CreateCell($"B{miscRowIndex}", taxRow.BillNumber));
                        miscRow.Append(CreateCell($"C{miscRowIndex}", ""/*taxRow.ModeOfPayment*/));
                        miscRow.Append(CreateCell($"D{miscRowIndex}", ""/*taxRow.OPATrackingNum*/));
                        miscRow.Append(CreateCell($"E{miscRowIndex}", ""/*taxRow.TaxpayersName*/));
                        miscRow.Append(CreateDecimalCell($"F{miscRowIndex}", taxRow.Collection ?? 0));
                        //miscRow.Append(CreateCell($"G{miscRowIndex}", taxRow.PaymentDate.ToString()));
                        miscSheetData.AppendChild(miscRow);

                        totalAmountTransferred += taxRow.Collection ?? 0;

                        count++;
                        miscRowIndex++;
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

        private void btnClose_MouseEnter(object sender, EventArgs e)
        {
            originalBackgroundImageRpt = btnClose.BackgroundImage;
            btnClose.BackgroundImage = null;

            System.Drawing.Color customColor = System.Drawing.Color.FromArgb(23, 45, 74);
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

            System.Drawing.Color customColor = System.Drawing.Color.FromArgb(23, 45, 74);
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
