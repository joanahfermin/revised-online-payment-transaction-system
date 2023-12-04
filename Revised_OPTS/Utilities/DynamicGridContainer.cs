using Revised_OPTS.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Inventory_System.Utilities
{
    internal class DynamicGridContainer<T> where T : ICloneable
    {
        private DataGridView dataGridView;

        private DynamicGridInfo[] gridInfoArray;

        private BindingList<T> BindingDataList = new BindingList<T>();

        private List<T> DataToDeleteList = new List<T>();

        //public DynamicGridContainer(DataGridView dataGridView, DynamicGridInfo[] gridInfoArray, bool allowDelete, bool allowDuplicate)
        //{
        //    this.dataGridView = dataGridView;
        //    this.gridInfoArray = gridInfoArray;

        //    wag mag auto generate ng columns, manual natin lalagay
        //    dataGridView.AutoGenerateColumns = false;

        //    attach validation
        //    dataGridView.CellValidating += dataGridView_CellValidating;

        //    foreach (DynamicGridInfo info in gridInfoArray)
        //    {
        //        if (info.GridType == DynamicGridType.Text)
        //        {
        //            dataGridView.Columns.Add(info.PropertyName, info.Label);
        //            dataGridView.Columns[info.PropertyName].DataPropertyName = info.PropertyName;
        //        }
        //        else
        //        {
        //            DataGridViewComboBoxColumn comboBoxColumn = new DataGridViewComboBoxColumn();
        //            comboBoxColumn.DataPropertyName = info.PropertyName;
        //            comboBoxColumn.HeaderText = info.Label;
        //            comboBoxColumn.Name = info.PropertyName;
        //            comboBoxColumn.Items.AddRange(info.ComboboxChoices);
        //            dataGridView.Columns.Add(comboBoxColumn);
        //        }
        //        if (info.isReadOnly)
        //        {
        //            dataGridView.Columns[info.PropertyName].ReadOnly = true;
        //        }

        //        Add Delete Menu
        //        if (allowDelete)
        //        {
        //            ContextMenuStrip contextMenuStrip1 = new ContextMenuStrip();
        //            ToolStripMenuItem menuItem = new ToolStripMenuItem("Delete");
        //            menuItem.Click += DeleteItem_Click;
        //            contextMenuStrip1.Items.Add(menuItem);
        //            dataGridView.ContextMenuStrip = contextMenuStrip1;
        //            dataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        //        }

        //        if (allowDuplicate)
        //        {
        //            ContextMenuStrip contextMenuStrip1 = new ContextMenuStrip();
        //            ToolStripMenuItem menuItem = new ToolStripMenuItem("Duplicate");
        //            menuItem.Click += DuplicateRecord_Click;
        //            contextMenuStrip1.Items.Add(menuItem);
        //            dataGridView.ContextMenuStrip = contextMenuStrip1;
        //            dataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        //        }
        //        Bind the data
        //        dataGridView.DataSource = BindingDataList;
        //    }
        //}

        public DynamicGridContainer(DataGridView dataGridView, DynamicGridInfo[] gridInfoArray, bool allowDelete, bool allowDuplicate)
        {
            this.dataGridView = dataGridView;
            this.gridInfoArray = gridInfoArray;

            // wag mag auto generate ng columns, manual natin lalagay
            dataGridView.AutoGenerateColumns = false;

            // attach validation
            dataGridView.CellValidating += dataGridView_CellValidating;

            // Create a single ContextMenuStrip
            ContextMenuStrip contextMenuStrip1 = new ContextMenuStrip();

            foreach (DynamicGridInfo info in gridInfoArray)
            {
                if (info.GridType == DynamicGridType.Text)
                {
                    dataGridView.Columns.Add(info.PropertyName, info.Label);
                    dataGridView.Columns[info.PropertyName].DataPropertyName = info.PropertyName;
                }
                else
                {
                    DataGridViewComboBoxColumn comboBoxColumn = new DataGridViewComboBoxColumn();
                    comboBoxColumn.DataPropertyName = info.PropertyName;
                    comboBoxColumn.HeaderText = info.Label;
                    comboBoxColumn.Name = info.PropertyName;
                    comboBoxColumn.Items.AddRange(info.ComboboxChoices);
                    dataGridView.Columns.Add(comboBoxColumn);
                }

                if (info.isReadOnly)
                {
                    dataGridView.Columns[info.PropertyName].ReadOnly = true;
                }
            }

            // Add Delete Menu if allowed
            if (allowDelete)
            {
                ToolStripMenuItem deleteMenuItem = new ToolStripMenuItem("Delete");
                deleteMenuItem.Click += DeleteItem_Click;
                contextMenuStrip1.Items.Add(deleteMenuItem);
                dataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            }

            // Add Duplicate Menu if allowed
            if (allowDuplicate)
            {
                ToolStripMenuItem duplicateMenuItem = new ToolStripMenuItem("Duplicate");
                duplicateMenuItem.Click += DuplicateRecord_Click;
                contextMenuStrip1.Items.Add(duplicateMenuItem);
                dataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            }

            // Bind the data (moved outside the loop)
            dataGridView.DataSource = BindingDataList;

            // Set the context menu to the DataGridView
            dataGridView.ContextMenuStrip = contextMenuStrip1;
        }

        private void dataGridView_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            Boolean errorFound = false;
            DynamicGridInfo info = gridInfoArray[e.ColumnIndex];
            string cellValue = e.FormattedValue.ToString();

            // clear previous errors            
            dataGridView.Rows[e.RowIndex].ErrorText = string.Empty;

            //validate required field
            if (info.isRequired)
            {
                if (string.IsNullOrEmpty(cellValue))
                {
                    dataGridView.Rows[e.RowIndex].ErrorText = $"{info.Label} is required.";
                    //e.Cancel = true;
                    errorFound = true;
                }
            }

            // validate decimal format
            if (info.decimalValue && !errorFound)
            {
                if (!decimal.TryParse(cellValue, NumberStyles.Number, CultureInfo.InvariantCulture, out decimal value))
                {
                    dataGridView.Rows[e.RowIndex].ErrorText = $"{info.Label} is not in decimal format.";
                    //e.Cancel = true;
                    errorFound = true;
                }
            }

            // validate format
            if (!string.IsNullOrEmpty(info.format) && !errorFound)
            {
                if (!string.IsNullOrEmpty(cellValue))
                {
                    Regex re = new Regex(info.format);
                    if (!re.IsMatch(cellValue))
                    {
                        dataGridView.Rows[e.RowIndex].ErrorText = $"{info.Label} is not in correct format.";
                        //e.Cancel = true;
                        errorFound = true;
                    }
                }
            }
        }

        private void DeleteItem_Click(object sender, EventArgs e)
        {
            DataGridViewRow selectedRow = dataGridView.CurrentRow;
            if (selectedRow != null)
            {
                T selectedItem = (T)selectedRow.DataBoundItem;
                if (selectedItem != null)
                {
                    //remove sa datagridview para dina makita
                    BindingDataList.Remove(selectedItem);

                    //tandaan natin sino idedelete, later ipapadala sa service in 1 go delete
                    DataToDeleteList.Add(selectedItem);
                }
            }
        }

        private void DuplicateRecord_Click(object sender, EventArgs e)
        {
            // Get the selected row
            DataGridViewRow selectedRow = dataGridView.CurrentRow;

            // Check if a row is selected
            if (selectedRow != null)
            {
                T selectedDataObject = (T)selectedRow.DataBoundItem;

                if (selectedDataObject != null)
                {
                    // Create a new data object for the new row
                    T newDataObject = (T)selectedDataObject.Clone();

                    if (newDataObject is Rpt)
                    {
                        Rpt rpt = newDataObject as Rpt;

                        rpt.RptID = 0;
                    }
                    // Add the duplicated data object to your data source
                    BindingDataList.Add(newDataObject);

                    // Refresh the DataGridView to reflect the changes
                    dataGridView.Refresh();
                }
            }

        }

        public void PopulateData(List<T> data)
        {
            BindingDataList.Clear();
            foreach (T item in data)
            {
                BindingDataList.Add(item);
            }
        }

        public List<T> GetData()
        {
            return BindingDataList.ToList();
        }

        public List<T> GetDataToDelete()
        {
            return DataToDeleteList;
        }
    }
}

