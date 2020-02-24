using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Office.Interop.Word;
using Word = Microsoft.Office.Interop.Word;

using System.Data;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace Geo.Utils
{
    /// <summary>
    /// 报表输出工具。
    /// </summary>
    public static class ReportUtil
    {
        /// <summary>
        /// 将 DataGridView 数据保存到磁盘。过程中会提示路径选择。
        /// </summary>
        /// <param name="dataGridView1">DataGridView对象</param>
        public static void SaveToExcel(DataGridView dataGridView1)
        {
            try
            {
                SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                saveFileDialog1.Filter = "Excel 文件|*.xls";
                saveFileDialog1.FileName = "数据导出";
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    string path = saveFileDialog1.FileName;
                    ExcelHelper.CreateExcelFile(path, dataGridView1);
                    if (MessageBox.Show("生成成功，是否打开？", "提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        FileUtil.OpenFile(path);
                    }
                }
            }
            catch (Exception ex) { FormUtil.ShowErrorMessageBox("出错了：" +ex.Message + ",注意：Excel 不允许路径中有'['、']'等字符。"); }

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataGridView1"></param>
        /// <param name="title"></param>
        public static void SaveToWord(DataGridView dataGridView1, string title = "数据报表")
        {
            ReportRows(dataGridView1, title);
        }

        #region 打印列表
        /// <summary>
        /// 报表打印
        /// </summary>
        /// <param name="dataGridView1"></param>
        /// <param name="title"></param>
        public static void ReportRows(DataGridView dataGridView1, string title = "数据报表")
        {
            //检查是否选中列
            if (dataGridView1.SelectedRows == null || dataGridView1.SelectedRows.Count < 1)
            {
                MessageBox.Show("请选择要导出的行！");
                return;
            }

            //选择需要打印的列。
            List<string> titleList = new List<string>();
            foreach (DataGridViewColumn col in dataGridView1.Columns)  titleList.Add(col.HeaderText); 
            //弹出对话框选择
            SelectMultiNameForm form = new SelectMultiNameForm(titleList.ToArray());
            if (form.ShowDialog() == DialogResult.OK)  titleList = form.SelectedNames;  
            else return;

            if (titleList.Count < 1)  {  MessageBox.Show("你的选择为空！");   return;  }

            //着手打印了
            Geo.Utils.FormUtil.ShowWaittingForm("正在努力处理Word程序，请稍后……");
            Report(dataGridView1, titleList, title);
        }


        /// <summary>
        /// 启动Word输出表格数据。
        /// </summary>
        /// <param name="dataGridView1"></param>
        /// <param name="tableName">数据报表</param>
        /// <param name="titleList"></param>
        private static void Report(DataGridView dataGridView1, List<string> titleList, string tableName = "数据报表")//string fileName, 
        {
            Word.Application wordApp = new Word.Application();
            wordApp.Visible = true;
            Word.Document wordDoc = wordApp.Documents.Add();
            //写入标题 
            WordUtil.InsertText(wordApp, tableName, 20, WdColor.wdColorBlack, 2, "宋体", WdParagraphAlignment.wdAlignParagraphCenter);
            WordUtil.ToNextLine(wordApp);
           

            object mis = Type.Missing;

            //Selection.TypeParagraph
            //定义好行和列的数量
            int rowCount = dataGridView1.SelectedRows.Count + 1;//加标题
            int colCount = titleList.Count;

            //在wordDoc中插入一个表格 
            Word.Range tableLocation = wordDoc.Paragraphs.Last.Range; 
            object autoFitBehavior = Word.WdAutoFitBehavior.wdAutoFitWindow;
            Word.Table wordTable = wordDoc.Tables.Add(tableLocation, rowCount, colCount, ref mis, ref autoFitBehavior);

            WordUtil.SetTableStyle(wordTable);

            int colInx = 0;
            foreach (string title in titleList)
            {
                wordTable.Cell(1, colInx + 1).Range.InsertAfter(title);
                colInx++;
            }

            for (int rowIndex = 1; rowIndex < rowCount; rowIndex++)//每行,注意：Word Table 列编号起始点为1.
            {
                DataGridViewRow row = dataGridView1.SelectedRows[rowIndex - 1];
                for (int colIndex = 0, wordColIndex = 1; colIndex < row.Cells.Count; colIndex++)//每列
                {
                    //第一行为标题
                    if (titleList.Contains(dataGridView1.Columns[colIndex].HeaderText))
                    {
                        wordTable.Cell(rowIndex + 1, wordColIndex++).Range.InsertAfter(row.Cells[colIndex].Value+"");
                    }
                }
            }
            WordUtil.SortAscending(wordTable);

            FormUtil.ShowOkMessageBox("Word报表输出完毕！请注意保存。");
        }
        #endregion

    }
}
