using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;

using System.Runtime.InteropServices;
using System.IO;
using System.Windows.Forms; 

using Excel = Microsoft.Office.Interop.Excel;
using MSExcel = Microsoft.Office.Interop.Excel;
using Microsoft.Office.Interop.Excel; 
 

namespace Geo.Utils
{

    /// <summary>
    /// Represents the header and data of a column.
    /// </summary>
    public class ColumnData
    {
        /// <summary>
        /// Gets or sets the header.
        /// </summary>
        /// <value>The header.</value>
        public string Header { get; set; }

        /// <summary>
        /// Gets or sets the data.
        /// </summary> 
        public object[] Data { get; set; }
    }
    /// <summary>
    /// Excel帮助类
    /// </summary>
    public class ExcelHelper
    {
        /// <summary>
        /// Creates the excel file by column.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <param name="columns">The columns.</param>
        public static void CreateExcelFileByColumn(string filename, IEnumerable<ColumnData> columns)
        {
            createExcelFile(filename, excelApp =>
            {
                //Write data into the workbook by column.
                int columnIndex = 1;
                foreach (var column in columns)
                {
                    //Write the header.
                    excelApp.Cells[1, columnIndex].Value = column.Header;

                    //Write the following lines in this column.
                    int rowIndex = 2;
                    foreach (var cell in column.Data)
                    {
                        string val = "";
                        if (cell != null) val = cell.ToString();
                        excelApp.Cells[rowIndex++, columnIndex].Value = val;
                    }
                    columnIndex++;
                }
            });
        }

        /// <summary>
        /// Creates the excel file by row.
        /// 将选中的行，保存到文件。
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <param name="rows">The rows.</param>
        public static void CreateExcelFileByRow(string filename, IEnumerable<IEnumerable> rows)
        {
            createExcelFile(filename, excelApp =>
            {
                //Write data into the workbook by row.
                int rowIndex = 1;
                foreach (var row in rows)
                {
                    int columnIndex = 1;
                    foreach (var cell in row)
                    {
                        string val = "";
                        if (cell != null) val = cell.ToString();
                      //  excelApp.Cells[rowIndex, columnIndex++].Value = currentVal;
                    }
                    rowIndex++;
                }
            });
        }
        /// <summary>
        /// 保存到文件。
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="dataGridView1"></param>
        /// <param name="onlySelectedRows">是否只输出选中的行</param>
        public static void CreateExcelFile(string filename, DataGridView dataGridView1, bool onlySelectedRows = true)
        {
            createExcelFile(filename, excelApp =>
            {               
                //head
                int colIndex = 1;
                foreach (DataGridViewColumn v in dataGridView1.Columns)
                {
                    excelApp.Cells[1, colIndex++].Value = v.HeaderText;
                }

                //Write data into the workbook by row.
                int rowIndex = 2;

                IList rows = dataGridView1.Rows;
                if(onlySelectedRows)  rows = Utils.DataGridViewUtil.GetSelectedRows(dataGridView1);

                foreach (DataGridViewRow row in rows)
                {                   
                    int columnIndex = 1;
                    foreach (DataGridViewCell cell in  row.Cells)
                    {
                        string val = "";
                        if (cell != null && cell.Value != null) val = cell.Value.ToString();
                        excelApp.Cells[rowIndex, columnIndex++].Value = val; 
                    }
                    rowIndex++;
                }
            });
        }

        /// <summary>
        /// Creates the excel file and perform the specified action.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <param name="action">The action.</param>
        private static void createExcelFile(string filename, Action<MSExcel.Application> action)
        {
            //Create the excel application and set it to run in background.
            var excelApp = new MSExcel.Application();
            excelApp.Visible = false;

            //Add a new workbook.
            excelApp.Workbooks.Add();

            //Perform the action.
            action(excelApp);

            //Save the workbook then close the file.
            excelApp.ActiveWorkbook.SaveAs(Filename: filename,
                FileFormat: MSExcel.XlFileFormat.xlWorkbookNormal);
            excelApp.ActiveWorkbook.Close();

            //Exit the excel application.
            excelApp.Quit();
        }



    }
}