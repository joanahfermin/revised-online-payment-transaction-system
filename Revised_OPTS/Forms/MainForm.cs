using Inventory_System.Exception;
using Inventory_System.Forms;
using Inventory_System.Service;
using Inventory_System.Utilities;
using Revised_OPTS.Forms;
using Revised_OPTS.Model;
using Revised_OPTS.Service;
using Revised_OPTS.Utilities;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Revised_OPTS
{
    public partial class MainForm : Form
    {
        IRptService rptService = ServiceFactory.Instance.GetRptService();
        IMiscService miscService = ServiceFactory.Instance.GetMiscService();
        IBusinessService businessService = ServiceFactory.Instance.GetBusinessService();
        ISecurityService securityService = ServiceFactory.Instance.GetSecurityService();

        private System.Windows.Forms.Timer autoUpdateScreenTimer = new System.Windows.Forms.Timer();

        private const int RPT_RECORD_TYPE = 1;
        private const int MISC_RECORD_TYPE = 2;
        private const int BUSINESS_RECORD_TYPE = 3;
        private int CURRENT_RECORD_TYPE = 0;

        private Image originalBackgroundImageNonRpt;
        private Image originalBackgroundImageRpt;

        public static MainForm Instance;

        string selectedRecordFormat = "";
        long selectedRecordId = 0;

        Dictionary<string, string> RPT_DG_COLUMNS = new Dictionary<string, string>
        {
            { "TaxDec", "TDN" }, { "TaxPayerName", "TaxPayer's Name" }, { "AmountToPay", "Bill Amount" },
            { "AmountTransferred", "Transferred Amount" }, { "TotalAmountTransferred", "Total Amount Transferred" }, { "ExcessShortAmount", "Excess/Short" },
            { "Bank", "Bank" }, { "YearQuarter", "Year" }, { "Quarter", "Quarter" },
            /*{ "PaymentType", "Payment Type" },*/ { "BillingSelection", "Billing Selection" }, { "Status", "Status" }, { "RefNum", "Reference No." },
            { "RequestingParty", "Email Address" }, { "EncodedBy", "Encoded By" }, { "EncodedDate", "Encoded Date" }, { "RPTremarks", "Remarks" },

            { "VerifiedBy", "VerifiedBy" }, { "PaymentDate", "Payment Date" }, { "VerifiedDate", "Verified Date" },
            { "ValidatedBy", "Validated By" }, { "ValidatedDate", "Validated Date" }, { "UploadedBy", "Uploaded By" },
            { "UploadedDate", "Uploaded Date" }, { "ReleasedBy", "Released By" }, { "ReleasedDate", "Released Date" },
            { "LocCode", "Location Code" }, { "RepName", "Representative Name" }, { "ContactNumber", "Contact Number" },
                    { "ORConfirmDate", "Receipt Confirm Date" }, { "ORAttachedDate", "Receipt Attachment Date" },
        };

        Dictionary<string, string> BUSINESS_DG_COLUMNS = new Dictionary<string, string>
        {
            { "Business_Type", "Bus. Type" }, { "MP_Number", "M.P Number" }, { "BillNumber", "Bill Number" },
            { "TaxpayersName", "TaxPayer's Name" }, { "BusinessName", "Business Name" }, { "BillAmount", "Bill Amount" },
            { "TotalAmount", "Transferred Amount" }, { "MiscFees", "Misc. Fees" }, { "Year", "Year" },
            { "Qtrs", "Quarter" }, { "Status", "Status" }, { "PaymentChannel", "Bank" }, { "RefNum", "Reference No." },
            { "VerifiedBy", "Verified By" }, { "VerifiedDate", "Verified Date" }, { "DateOfPayment", "Payment Date" },
            { "ValidatedBy", "Validated By" }, { "ValidatedDate", "Validated Date" },
            { "TransmittedBy", "Transmitted By" }, { "TransmittedDate", "Transmitted Date" }, { "RequestingParty", "Email Address" },
            { "EncodedDate", "Encoded Date" }, { "BussinessRemarks", "Remarks" }, { "EncodedBy", "Encoded By" },
            { "ContactNumber", "Contact Number" }, { "UploadedBy", "Uploaded By" }, { "UploadedDate", "Uploaded Date" },
                     { "ReleasedBy", "Released By" }, { "ReleasedDate", "Released Date" }, { "RepName", "Representative Name" }, { "RepContactNumber", "Rep. Contact Number" },
        };

        Dictionary<string, string> MISC_DG_COLUMNS = new Dictionary<string, string>
        {
            { "MiscType", "Misc Type" }, { "TaxpayersName", "TaxPayer's Name" }, { "OrderOfPaymentNum", "O.P Number" }, { "OPATrackingNum", "OPA Tracking No." },
            { "AmountToBePaid", "Bill Amount" }, { "TransferredAmount", "Transferred Amount" }, { "ExcessShort", "Excess/Short" }, { "Status", "Status" },
            { "ModeOfPayment", "Bank" }, { "PaymentDate", "Payment Date" },{ "VerifiedBy", "Verified By" }, { "VerifiedDate", "Verified Date" }, { "ValidatedBy", "Validated By" },
            { "ValidatedDate", "Validated Date" }, { "RequestingParty", "Email Address" }, { "RefNum", "Reference No." },
            { "EncodedDate", "Encoded Date" }, { "EncodedBy", "Encoded By" }, { "Remarks", "Remarks" }, { "ReleasedBy", "Released By" },
            { "ReleasedDate", "Released Date" }, { "RepName", "Representative Name" }, { "ContactNumber", "Contact Number" },
        };

        public MainForm()
        {
            InitializeComponent();
            DataGridUI();
            Instance = this;
            DgMainForm.CellFormatting += DgMainForm_CellFormatting;
            DgMainForm.SelectionChanged += DgMainForm_SelectionChanged;

            ContextMenuStrip contextMenuStrip1 = new ContextMenuStrip();

            ToolStripMenuItem menuItemDelete = new ToolStripMenuItem("Delete");
            ToolStripMenuItem menuItemAllRefNo = new ToolStripMenuItem("All items");
            ToolStripMenuItem menuItemItemsInTheListOfRefNo = new ToolStripMenuItem("Some items");

            menuItemDelete.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[]
            { menuItemAllRefNo, menuItemItemsInTheListOfRefNo });

            menuItemItemsInTheListOfRefNo.Click += MenuItemDelete_Click;
            menuItemAllRefNo.Click += MenuItemDeleteAllRefNo_Click;

            ToolStripMenuItem menuItemEdit = new ToolStripMenuItem("Edit");
            menuItemEdit.Click += MenuItemEdit_Click;

            ToolStripMenuItem menuItemValidatePayment = new ToolStripMenuItem("Validate Payment");
            menuItemValidatePayment.Click += MenuItemValidatePayment_Click;

            ToolStripMenuItem menuItemVerifyPayment = new ToolStripMenuItem("Verify Payment");
            menuItemVerifyPayment.Click += MenuItemVerifiedPayment_Click;
            ToolStripMenuItem menuItemReleaseReceipt = new ToolStripMenuItem("Release Receipt");
            menuItemReleaseReceipt.Click += MenuItemReleaseReceipt_Click;

            ToolStripMenuItem menuItemRevertStatus = new ToolStripMenuItem("Revert Status");
            ToolStripMenuItem menuItemRevertForVerificationStatus = new ToolStripMenuItem(TaxStatus.ForPaymentVerification);
            ToolStripMenuItem menuItemRevertForValidationStatus = new ToolStripMenuItem(TaxStatus.ForPaymentValidation);
            ToolStripMenuItem menuItemRevertForORUploadStatus = new ToolStripMenuItem(TaxStatus.ForORUpload);
            ToolStripMenuItem menuItemRevertForORPickUpStatus = new ToolStripMenuItem(TaxStatus.ForORPickup);

            menuItemRevertForVerificationStatus.Click += MenuItemRevertStatus_Click;
            menuItemRevertForValidationStatus.Click += MenuItemRevertStatus_Click;
            menuItemRevertForORUploadStatus.Click += MenuItemRevertStatus_Click;
            menuItemRevertForORPickUpStatus.Click += MenuItemRevertStatus_Click;

            menuItemRevertStatus.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[]
            { menuItemRevertForVerificationStatus, menuItemRevertForValidationStatus, menuItemRevertForORUploadStatus, menuItemRevertForORPickUpStatus });

            ToolStripMenuItem menuItemForTransmittal = new ToolStripMenuItem("Transmit Receipt");
            menuItemForTransmittal.Click += MenuItemTransmitReceipt_Click;

            contextMenuStrip1.Items.Add(menuItemDelete);
            contextMenuStrip1.Items.Add(menuItemEdit);
            contextMenuStrip1.Items.Add(menuItemReleaseReceipt);
            contextMenuStrip1.Items.Add(menuItemRevertStatus);
            contextMenuStrip1.Items.Add(menuItemValidatePayment);
            contextMenuStrip1.Items.Add(menuItemVerifyPayment);

            DgMainForm.ContextMenuStrip = contextMenuStrip1;

            InitializeAutoUpdateScreenTimer();
        }

        private void InitializeAutoUpdateScreenTimer()
        {
            autoUpdateScreenTimer.Interval = 300000; // 5 minutes
            autoUpdateScreenTimer.Tick += autoUpdateScreenTimer_Tick;
            autoUpdateScreenTimer.Start();
        }

        private void autoUpdateScreenTimer_Tick(object sender, EventArgs e)
        {
            string uploadedBy = securityService.getLoginUser().DisplayName;
            tbMailToSendCount.Text = rptService.CountORUploadRemainingToSend(uploadedBy).ToString();

        }


        private void DgMainForm_SelectionChanged(object? sender, EventArgs e)
        {
            tbRecordSelected.Text = DgMainForm.SelectedRows.Count.ToString();

            DataGridViewSelectedRowCollection selectedRows = DgMainForm.SelectedRows;
            decimal sumBillAmount = 0;
            decimal sumTotalTransferredAmount = 0;

            foreach (DataGridViewRow row in selectedRows)
            {
                if (CURRENT_RECORD_TYPE == RPT_RECORD_TYPE)
                {
                    //conversion of row to Rpt
                    Rpt selectedRptRecord = row.DataBoundItem as Rpt;
                    if (selectedRptRecord != null)
                    {
                        // Assuming AmountToPay is a property in your Rpt class and Sum is a property of AmountToPay
                        sumBillAmount = (decimal)(selectedRptRecord.AmountToPay + sumBillAmount);
                        sumTotalTransferredAmount = (decimal)(selectedRptRecord.TotalAmountTransferred ?? 0) + sumTotalTransferredAmount;
                    }
                }
                else if (CURRENT_RECORD_TYPE == BUSINESS_RECORD_TYPE)
                {
                    Business selectedBusinessRecord = row.DataBoundItem as Business;
                    if (selectedBusinessRecord != null)
                    {
                        // Assuming AmountToPay is a property in your Rpt class and Sum is a property of AmountToPay
                        sumBillAmount = (decimal)(selectedBusinessRecord.BillAmount ?? 0) + sumBillAmount;
                        sumTotalTransferredAmount = (decimal)(selectedBusinessRecord.TotalAmount ?? 0) + sumTotalTransferredAmount;
                    }
                }
                else if (CURRENT_RECORD_TYPE == MISC_RECORD_TYPE)
                {
                    Miscellaneous selectedMiscRecord = row.DataBoundItem as Miscellaneous;
                    if (selectedMiscRecord != null)
                    {
                        // Assuming AmountToPay is a property in your Rpt class and Sum is a property of AmountToPay
                        sumBillAmount = (decimal)(selectedMiscRecord.AmountToBePaid ?? 0) + sumBillAmount;
                        sumTotalTransferredAmount = (decimal)(selectedMiscRecord.TransferredAmount ?? 0) + sumTotalTransferredAmount;
                    }
                }
                tbTotalBillAmount.Text = sumBillAmount.ToString("N2");
                tbTotalAmountTransferred.Text = sumTotalTransferredAmount.ToString("N2");
            }
        }

        private void DeleteAllRptWithSameRefNo()
        {
            DataGridViewSelectedRowCollection selectedRows = DgMainForm.SelectedRows;
            if (selectedRows.Count > 0)
            {
                var sortedRptRows = selectedRows.Cast<DataGridViewRow>().OrderBy(row => row.Index);
                string firstRptReferenceNumber = sortedRptRows.Select(row => row.DataBoundItem).OfType<Rpt>().Select(rpt => rpt.RefNum).FirstOrDefault();
                if (firstRptReferenceNumber != null)
                {
                    List<Rpt> searchResultList = rptService.RetrieveBySameRefNum(firstRptReferenceNumber);
                    List<Rpt> listofRptsToDelete = searchResultList.Where(rpt => rpt.RefNum == firstRptReferenceNumber).ToList();
                    if (listofRptsToDelete.Count > 0)
                    {
                        DialogResult result = MessageBox.Show($"Please be informed that there are {listofRptsToDelete.Count} record(s) found in the selection. Are you sure you want to delete all the record(s) in the list of {firstRptReferenceNumber}? ", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (result == DialogResult.Yes)
                        {
                            rptService.DeleteAll(listofRptsToDelete);
                            Search(tbSearch.Text);
                            tbSearch.Clear();
                            MessageBox.Show("Operation completed successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
            }
        }

        private void DeleteAllBusinessWithSameRefNo()
        {
            DataGridViewSelectedRowCollection selectedRows = DgMainForm.SelectedRows;
            if (selectedRows.Count > 0)
            {
                var sortedBusRows = selectedRows.Cast<DataGridViewRow>().OrderBy(row => row.Index);
                string firstBusReferenceNumber = sortedBusRows.Select(row => row.DataBoundItem).OfType<Business>().Select(bus => bus.RefNum).FirstOrDefault();
                if (firstBusReferenceNumber != null)
                {
                    List<Business> searchResultList = businessService.RetrieveBySameRefNum(firstBusReferenceNumber);
                    List<Business> listofBusToDelete = searchResultList.Where(bus => bus.RefNum == firstBusReferenceNumber).ToList();
                    if (listofBusToDelete.Count > 0)
                    {
                        DialogResult result = MessageBox.Show($"Please be informed that there are {listofBusToDelete.Count} record(s) found in the selection. Are you sure you want to delete all the record(s) in the list of {firstBusReferenceNumber}? ", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (result == DialogResult.Yes)
                        {
                            businessService.DeleteAll(listofBusToDelete);
                            Search(tbSearch.Text);
                            tbSearch.Clear();
                            MessageBox.Show("Operation completed successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
            }
        }

        private void DeleteAllMiscWithSameRefNo()
        {
            DataGridViewSelectedRowCollection selectedRows = DgMainForm.SelectedRows;
            if (selectedRows.Count > 0)
            {
                var sortedMiscRows = selectedRows.Cast<DataGridViewRow>().OrderBy(row => row.Index);
                string firstMiscReferenceNumber = sortedMiscRows.Select(row => row.DataBoundItem).OfType<Miscellaneous>().Select(misc => misc.RefNum).FirstOrDefault();
                if (firstMiscReferenceNumber != null)
                {
                    List<Miscellaneous> searchResultList = miscService.RetrieveBySameRefNum(firstMiscReferenceNumber);
                    List<Miscellaneous> listofBusToDelete = searchResultList.Where(misc => misc.RefNum == firstMiscReferenceNumber).ToList();
                    if (listofBusToDelete.Count > 0)
                    {
                        DialogResult result = MessageBox.Show($"Please be informed that there are {listofBusToDelete.Count} record(s) found in the selection. Are you sure you want to delete all the record(s) in the list of {firstMiscReferenceNumber}? ", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (result == DialogResult.Yes)
                        {
                            miscService.DeleteAll(listofBusToDelete);
                            Search(tbSearch.Text);
                            tbSearch.Clear();
                            MessageBox.Show("Operation completed successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }

            }
        }

        private void MenuItemDeleteAllRefNo_Click(object? sender, EventArgs e)
        {
            DeleteAllRptWithSameRefNo();
            DeleteAllBusinessWithSameRefNo();
            DeleteAllMiscWithSameRefNo();
        }

        private void MenuItemDelete_Click(object? sender, EventArgs e)
        {
            DataGridViewSelectedRowCollection selectedRow = DgMainForm.SelectedRows;
            if (selectedRow != null && selectedRow.Count > 0)
            {
                Rpt selectedRptRecord = selectedRow[0].DataBoundItem as Rpt;
                if (selectedRptRecord.RefNum != null)
                {
                    new RPTMultipleAddUpdateRecordForm(selectedRptRecord.RefNum, selectedRptRecord.RequestingParty).Show();
                    MessageBox.Show("Right-click the record you want to delete to navigate the action you want to perform.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }


            //if (selectedRows.Count > 0)
            //{
            //    foreach (DataGridViewRow row in selectedRows)
            //    {
            //        Rpt selectedRptRecord = row.DataBoundItem as Rpt;
            //        if (selectedRptRecord.RefNum != null)
            //        {
            //            new RPTMultipleAddUpdateRecordForm(selectedRptRecord.RefNum, selectedRptRecord.RequestingParty).Show();
            //            MessageBox.Show("Right-click the record you want to delete to navigate the action you want to perform.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //        }
            //        else
            //        {
            //            DialogResult result = MessageBox.Show("Are you sure you want to delete the selected record(s)?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            //            if (result == DialogResult.Yes)
            //            {
            //                rptService.Delete(selectedRptRecord);
            //                Search(tbSearch.Text);
            //                tbSearch.Clear();
            //                MessageBox.Show("Operation completed successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //            }
            //        }
            //    }
            //}
        }

        private void MenuItemReleaseReceipt_Click(object? sender, EventArgs e)
        {
            DataGridViewSelectedRowCollection selectedRows = DgMainForm.SelectedRows;
            string forORUpload = TaxStatus.ForORUpload;
            string forORPickup = TaxStatus.ForORPickup;

            if (selectedRows != null)
            {
                List<Rpt> rptList = new List<Rpt>();

                int countInvalid = 0;

                foreach (DataGridViewRow row in selectedRows)
                {
                    object item = row.DataBoundItem;

                    if (item is Rpt)
                    {
                        Rpt rpt = item as Rpt;

                        if (rpt.Status == forORUpload || rpt.Status == forORPickup)
                        {
                            rptList.Add(rpt);
                        }
                        else
                        {
                            countInvalid++;
                        }
                    }
                }
                if (countInvalid > 0)
                {
                    MessageBox.Show($"Selected record(s) are not {forORUpload}/{forORPickup} and will not be executed.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                if (rptList.Count > 0)
                {
                    //DialogResult result = MessageBox.Show("Are you sure you want to update the status of the selected records?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    //if (result == DialogResult.Yes)
                    //{
                        //rptService.UpdateSelectedRecordsStatus(rptList, TaxStatus.Released);
                        //MessageBox.Show("Operation completed successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        //DgMainForm.Refresh();

                        ReleaseORForm releaseORform = new ReleaseORForm(rptList);
                        releaseORform.Show();
                    //}
                }
            }
        }

        private void MenuItemRevertStatus_Click(object? sender, EventArgs e)
        {
            DataGridViewSelectedRowCollection selectedRows = DgMainForm.SelectedRows;

            ToolStripMenuItem clickedSubMenuItem = (ToolStripMenuItem)sender;
            string selectedStatusInSubMenuItemText = clickedSubMenuItem.Text;

            if (selectedRows != null)
            {
                List<Rpt> rptList = new List<Rpt>();
                List<Business> businessList = new List<Business>();
                List<Miscellaneous> miscList = new List<Miscellaneous>();

                foreach (DataGridViewRow row in selectedRows)
                {
                    object item = row.DataBoundItem;

                    if (item is Rpt)
                    {
                        Rpt rpt = item as Rpt;
                        rptList.Add(rpt);
                    }
                    else if (item is Business)
                    {
                        Business business = item as Business;
                        businessList.Add(business);
                    }
                    else if (item is Miscellaneous)
                    {
                        Miscellaneous misc = item as Miscellaneous;
                        miscList.Add(misc);
                    }
                }

                if (rptList.Count > 0 || businessList.Count > 0 || miscList.Count > 0)
                {
                    try
                    {
                        rptService.CheckRevertStatus(rptList, selectedStatusInSubMenuItemText);
                        bool isAttachedOr = false;

                        foreach (Rpt rpt in rptList)
                        {
                            if (selectedStatusInSubMenuItemText == TaxStatus.ForORUpload)
                            {
                                isAttachedOr = true;
                            }
                        }
                        if (isAttachedOr)
                        {
                            MessageBox.Show("Attached receipt detected. Kindly note that if an attempt is made to revert a record containing an attached receipt picture, the system will automatically delete the associated image.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    catch (RptException ex)
                    {
                        MessageBox.Show(ex.Message);
                        return;
                    }

                    DialogResult result = MessageBox.Show("Are you sure you want to update the status of the selected records?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (result == DialogResult.Yes)
                    {
                        rptService.RevertSelectedRecordStatus(rptList, selectedStatusInSubMenuItemText);
                        businessService.RevertSelectedRecordStatus(businessList);
                        miscService.RevertSelectedRecordStatus(miscList);
                        DgMainForm.Refresh();
                        MessageBox.Show("Operation completed successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
        }

        private void MenuItemTransmitReceipt_Click(object? sender, EventArgs e)
        {
            DataGridViewSelectedRowCollection selectedRows = DgMainForm.SelectedRows;
            string forPaymentValidation = TaxStatus.ForPaymentValidation;
        }

        private void MenuItemValidatePayment_Click(object? sender, EventArgs e)
        {
            DataGridViewSelectedRowCollection selectedRows = DgMainForm.SelectedRows;
            string forPaymentValidation = TaxStatus.ForPaymentValidation;

            if (selectedRows != null)
            {
                List<Rpt> rptList = new List<Rpt>();
                List<Business> businessList = new List<Business>();
                List<Miscellaneous> miscList = new List<Miscellaneous>();

                int countInvalid = 0;

                foreach (DataGridViewRow row in selectedRows)
                {
                    object item = row.DataBoundItem;

                    if (item is Rpt)
                    {
                        Rpt rpt = item as Rpt;
                        if (rpt.Status == TaxStatus.ForPaymentValidation)
                        {
                            rptList.Add(rpt);
                        }
                        else
                        {
                            countInvalid++;
                        }
                    }
                    else if (item is Business)
                    {
                        Business business = item as Business;
                        if (business.Status == TaxStatus.ForPaymentValidation)
                        {
                            businessList.Add(business);
                        }
                        else
                        {
                            countInvalid++;
                        }
                    }
                    else if (item is Miscellaneous)
                    {
                        Miscellaneous misc = item as Miscellaneous;
                        if (misc.Status == TaxStatus.ForPaymentValidation)
                        {
                            miscList.Add(misc);
                        }
                        else
                        {
                            countInvalid++;
                        }
                    }
                }

                if (countInvalid > 0)
                {
                    MessageBox.Show($"Selected record(s) are not {forPaymentValidation} and will not be executed.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                if (rptList.Count > 0 || businessList.Count > 0 || miscList.Count > 0)
                {
                    DialogResult result = MessageBox.Show("Are you sure you want to update the status of the selected records?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (result == DialogResult.Yes)
                    {
                        rptService.UpdateSelectedRecordsStatus(rptList, TaxStatus.ForORUpload);
                        businessService.UpdateSelectedRecordsStatus(businessList, TaxStatus.ForTransmittal);
                        miscService.UpdateSelectedRecordsStatus(miscList, TaxStatus.ForORUpload);
                        MessageBox.Show("Operation completed successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        DgMainForm.Refresh();
                    }
                }
            }
        }

        private void MenuItemVerifiedPayment_Click(object? sender, EventArgs e)
        {
            DataGridViewSelectedRowCollection selectedRows = DgMainForm.SelectedRows;
            string forPaymentVerification = TaxStatus.ForPaymentVerification;

            if (selectedRows != null)
            {
                List<Rpt> rptList = new List<Rpt>();
                List<Business> businessList = new List<Business>();
                List<Miscellaneous> miscList = new List<Miscellaneous>();

                int invalidCount = 0;

                foreach (DataGridViewRow row in selectedRows)
                {
                    object item = row.DataBoundItem;

                    if (item is Rpt)
                    {
                        Rpt rpt = item as Rpt;
                        if (rpt.Status == forPaymentVerification)
                        {
                            rptList.Add(rpt);
                        }
                        else
                        {
                            invalidCount++;
                        }
                    }
                    else if (item is Business)
                    {
                        Business business = item as Business;
                        if (business.Status == forPaymentVerification)
                        {
                            businessList.Add(business);
                        }
                        else
                        {
                            invalidCount++;
                        }
                    }
                    else if (item is Miscellaneous)
                    {
                        Miscellaneous misc = item as Miscellaneous;
                        if (misc.Status == forPaymentVerification)
                        {
                            miscList.Add(misc);
                        }
                        else
                        {
                            invalidCount++;
                        }
                    }
                }

                if (invalidCount > 0)
                {
                    MessageBox.Show($"Selected record(s) are not {forPaymentVerification} and will not be executed.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                if (rptList.Count > 0 || businessList.Count > 0 || miscList.Count > 0)
                {
                    DialogResult result = MessageBox.Show("Are you sure you want to update the status of the selected records?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (result == DialogResult.Yes)
                    {
                        rptService.UpdateSelectedRecordsStatus(rptList, TaxStatus.ForPaymentValidation);
                        businessService.UpdateSelectedRecordsStatus(businessList, TaxStatus.ForPaymentValidation);
                        miscService.UpdateSelectedRecordsStatus(miscList, TaxStatus.ForPaymentValidation);
                        MessageBox.Show("Operation completed successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        DgMainForm.Refresh();
                    }
                }
            }
        }

        private void MenuItemEdit_Click(object? sender, EventArgs e)
        {
            DataGridViewRow selectedRow = DgMainForm.CurrentRow;
            if (selectedRow != null)
            {
                Rpt selectedRptRecord = selectedRow.DataBoundItem as Rpt;
                if (selectedRptRecord.RefNum != null)
                {
                    new RPTMultipleAddUpdateRecordForm(selectedRptRecord.RefNum, selectedRptRecord.RequestingParty).Show();
                }
                else
                {
                    Rpt retrieveRptRecord = rptService.Get(selectedRecordId);
                    string taxType = TaxTypeUtil.REALPROPERTYTAX;
                    AllTaxesAddUpdateRecordForm updateRecord = new AllTaxesAddUpdateRecordForm(retrieveRptRecord.RptID, taxType);
                    updateRecord.ShowDialog();
                }
            }
        }

        //all decimal columns shall have thousand separators and decimal points.
        private void DgMainForm_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.Value is decimal decimalValue)
            {
                e.Value = decimalValue.ToString("N2"); 
                e.FormattingApplied = true;
            }
        }

        public void DataGridUI()
        {
            DgMainForm.DefaultCellStyle.Font = new Font("Tahoma", 12, FontStyle.Regular);
            //DgMainForm.BackgroundColor = Color.White;
            this.WindowState = FormWindowState.Maximized;
            //DgMainForm.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCells;

            DgMainForm.DefaultCellStyle.ForeColor = Color.Black;

            //DgMainForm.ColumnHeadersDefaultCellStyle.ForeColor = Color.MidnightBlue;
            //DgMainForm.GridColor = Color.DarkGray;
            //DgMainForm.CellBorderStyle = DataGridViewCellBorderStyle.Single;
            //DgMainForm.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Raised;

            //DgMainForm.DefaultCellStyle.SelectionBackColor = Color.AliceBlue;
        }

        //search records based on keyvalueformat.
        private void tbSearch_KeyDown(object sender, KeyEventArgs e)
        {
            string searchedUniqueKey = tbSearch.Text;

            Search(searchedUniqueKey);
            DgMainForm.ClearSelection();
            int selectedRowCount = 0;
            int counter = 0;

            foreach (DataGridViewRow row in DgMainForm.Rows)
            {
                if (CURRENT_RECORD_TYPE == RPT_RECORD_TYPE)
                {
                    Rpt selectedRptRecord = row.DataBoundItem as Rpt;

                    if (selectedRptRecord.TaxDec.Equals(searchedUniqueKey, StringComparison.OrdinalIgnoreCase))
                    {
                        row.Selected = true;
                        selectedRowCount++;
                        DgMainForm.FirstDisplayedScrollingRowIndex = counter;
                    }
                }
                else if (CURRENT_RECORD_TYPE == BUSINESS_RECORD_TYPE)
                {
                    Business selectedBusinessRecord = row.DataBoundItem as Business;
                    if (selectedBusinessRecord.BillNumber.Equals(searchedUniqueKey, StringComparison.OrdinalIgnoreCase))
                    {
                        row.Selected = true;
                        selectedRowCount++;
                        DgMainForm.FirstDisplayedScrollingRowIndex = counter;
                    }
                }
                else if (CURRENT_RECORD_TYPE == MISC_RECORD_TYPE)
                {
                    Miscellaneous selectedMiscRecord = row.DataBoundItem as Miscellaneous;
                    if (selectedMiscRecord.OrderOfPaymentNum.Equals(searchedUniqueKey, StringComparison.OrdinalIgnoreCase))
                    {
                        row.Selected = true;
                        selectedRowCount++;
                        DgMainForm.FirstDisplayedScrollingRowIndex = counter;
                    }
                }
                counter++;
            }
            tbRecordSelected.Text = selectedRowCount.ToString();
        }

        //retrieve records based on keyvalueformat.
        public void Search(string searchRecordinDB)
        {
            if (SearchBusinessFormat.isTDN(searchRecordinDB))
            {
                CURRENT_RECORD_TYPE = RPT_RECORD_TYPE;
                ShowDataInDataGridView<Rpt>(RPT_DG_COLUMNS, rptService.RetrieveBySearchKeyword(searchRecordinDB));
            }
            else if (SearchBusinessFormat.isMiscOccuPermit(searchRecordinDB) || SearchBusinessFormat.isMiscOvrTtmd(searchRecordinDB)
                || SearchBusinessFormat.isMiscOvrDpos(searchRecordinDB) || SearchBusinessFormat.isMiscMarket(searchRecordinDB)
                || SearchBusinessFormat.isMiscZoning(searchRecordinDB) || SearchBusinessFormat.isMiscLiquor(searchRecordinDB))
            {
                CURRENT_RECORD_TYPE = MISC_RECORD_TYPE;
                ShowDataInDataGridView<Miscellaneous>(MISC_DG_COLUMNS, miscService.RetrieveBySearchKeyword(searchRecordinDB));
            }
            else if (SearchBusinessFormat.isBusiness(searchRecordinDB))
            {
                CURRENT_RECORD_TYPE = BUSINESS_RECORD_TYPE;
                ShowDataInDataGridView<Business>(BUSINESS_DG_COLUMNS, businessService.RetrieveBySearchKeyword(searchRecordinDB));
            }
        }

        //populate datagridview
        private void ShowDataInDataGridView<T>(Dictionary<string, string> columnMappings, List<T> dataList)
        {
            DgMainForm.Columns.Clear();
            DgMainForm.AutoGenerateColumns = false;

            foreach (var kvp in columnMappings)
            {
                DgMainForm.Columns.Add(kvp.Key, kvp.Value);
                DgMainForm.Columns[kvp.Key].DataPropertyName = kvp.Key;
            }
            DgMainForm.DataSource = dataList;

            //if (dataList.Count == 0)
            //{
            //    MessageBox.Show("No data found.");
            //}

            DgMainForm.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 12, FontStyle.Regular);
            DgMainForm.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
            DgMainForm.ColumnHeadersDefaultCellStyle.BackColor = Color.DarkSalmon;
            DgMainForm.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.DarkSalmon;
        }

        //get the selected record's keyvalueformat.
        private void DgMainForm_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow selectedRow = DgMainForm.Rows[e.RowIndex];
                if (selectedRow.DataBoundItem is Rpt selectedRecord)
                {
                    Rpt tdnRecord = selectedRow.DataBoundItem as Rpt;
                    selectedRecordFormat = tdnRecord.TaxDec;
                    selectedRecordId = tdnRecord.RptID;
                }
                else if (selectedRow.DataBoundItem is Business businessSelectedRecord)
                {
                    Business businessRecord = selectedRow.DataBoundItem as Business;
                    selectedRecordFormat = businessRecord.BillNumber;
                    selectedRecordId = businessRecord.BusinessID;

                }
                else if (selectedRow.DataBoundItem is Miscellaneous miscSelectedRecord)
                {
                    Miscellaneous miscRecord = selectedRow.DataBoundItem as Miscellaneous;
                    selectedRecordFormat = miscRecord.OrderOfPaymentNum;
                    selectedRecordId = miscRecord.MiscID;
                }
            }
        }

        //get the selected record's id then user can edit the record.
        private void DgMainForm_DoubleClick(object sender, EventArgs e)
        {
            bool isRptTDNFormatCorrect = SearchBusinessFormat.isTDN(selectedRecordFormat);
            bool isBusinessBillNumFormatCorrect = SearchBusinessFormat.isBusiness(selectedRecordFormat);
 
            if (isRptTDNFormatCorrect)
            {
                MenuItemEdit_Click(sender, e);
            }
            else if (isBusinessBillNumFormatCorrect)
            {
                Business retrieveBusinessRecord = businessService.Get(selectedRecordId);
                string taxType = TaxTypeUtil.BUSINESS;
                AllTaxesAddUpdateRecordForm updateRecord = new AllTaxesAddUpdateRecordForm(retrieveBusinessRecord.BusinessID, taxType);
                updateRecord.ShowDialog();
            }
            else
            {
                string taxType = SearchBusinessFormat.GetTaxTypeFromFormat(selectedRecordFormat);
                if (taxType != null)
                {
                    Miscellaneous retrieveMiscRecord = miscService.Get(selectedRecordId);
                    AllTaxesAddUpdateRecordForm updateRecord = new AllTaxesAddUpdateRecordForm(retrieveMiscRecord.MiscID, taxType);
                    updateRecord.ShowDialog();
                }
            }
        }

        private void MainForm_SizeChanged(object sender, EventArgs e)
        {
            DgMainForm.Size = new Size(this.ClientSize.Width - 50, this.ClientSize.Height - 170);
        }

        private void btnAddNewRecord_Click(object sender, EventArgs e)
        {
            new AllTaxesAddUpdateRecordForm().Show();
        }

        private void btnAddNewRecord_MouseEnter(object sender, EventArgs e)
        {
            originalBackgroundImageNonRpt = btnNonRptAddNewRecord.BackgroundImage;
            btnNonRptAddNewRecord.BackgroundImage = null;

            Color customColor = Color.FromArgb(23, 45, 74);
            btnNonRptAddNewRecord.BackColor = customColor;
        }

        private void btnAddNewRecord_MouseLeave(object sender, EventArgs e)
        {
            btnNonRptAddNewRecord.BackgroundImage = originalBackgroundImageNonRpt;
        }

        private void btnAddRptRecord_Click(object sender, EventArgs e)
        {
            new RPTMultipleAddUpdateRecordForm().Show();
        }

        private void btnAddRptRecord_MouseEnter(object sender, EventArgs e)
        {
            originalBackgroundImageRpt = btnAddRptRecord.BackgroundImage;
            btnAddRptRecord.BackgroundImage = null;

            Color customColor = Color.FromArgb(23, 45, 74);
            btnAddRptRecord.BackColor = customColor;
        }

        private void btnAddRptRecord_MouseLeave(object sender, EventArgs e)
        {
            btnAddRptRecord.BackgroundImage = originalBackgroundImageRpt;
        }

        private void btnORUpload_Click(object sender, EventArgs e)
        {
            ORUploadForm orUploadForm = new ORUploadForm();
            orUploadForm.ShowDialog();
        }

        private void btnORUpload_MouseEnter(object sender, EventArgs e)
        {
            originalBackgroundImageRpt = btnORUpload.BackgroundImage;
            btnORUpload.BackgroundImage = null;

            Color customColor = Color.FromArgb(23, 45, 74);
            btnORUpload.BackColor = customColor;
        }

        private void btnORUpload_MouseLeave(object sender, EventArgs e)
        {
            btnORUpload.BackgroundImage = originalBackgroundImageRpt;
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (LoginForm.INSTANCE != null)
            {
                LoginForm.INSTANCE.Close();
            }
        }

        private void btnAddEpayments_Click(object sender, EventArgs e)
        {
            AddEPaymentForm addEPaymentForm = new AddEPaymentForm();
            addEPaymentForm.ShowDialog();
        }

        private void btnAddEpayments_MouseEnter(object sender, EventArgs e)
        {
            originalBackgroundImageRpt = btnAddEpayments.BackgroundImage;
            btnAddEpayments.BackgroundImage = null;

            Color customColor = Color.FromArgb(23, 45, 74);
            btnAddEpayments.BackColor = customColor;
        }

        private void btnAddEpayments_MouseLeave(object sender, EventArgs e)
        {
            btnAddEpayments.BackgroundImage = originalBackgroundImageRpt;
        }

        private void btnReport_MouseEnter(object sender, EventArgs e)
        {
            originalBackgroundImageRpt = btnReport.BackgroundImage;
            btnReport.BackgroundImage = null;

            Color customColor = Color.FromArgb(23, 45, 74);
            btnReport.BackColor = customColor;
        }

        private void btnReport_MouseLeave(object sender, EventArgs e)
        {
            btnReport.BackgroundImage = originalBackgroundImageRpt;
        }

        private void btnReport_Click(object sender, EventArgs e)
        {
            ReportForm reportForm = new ReportForm();
            reportForm.ShowDialog();
        }

        private void DgMainForm_CellContextMenuStripNeeded(object sender, DataGridViewCellContextMenuStripNeededEventArgs e)
        {
            int rowSelected = e.RowIndex;

            if (e.RowIndex != -1 && DgMainForm.Rows[rowSelected].Selected == false)
            {
                DgMainForm.ClearSelection();
                DgMainForm.Rows[rowSelected].Selected = true;
            }
        }
    }
}