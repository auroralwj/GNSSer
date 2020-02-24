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
using   Microsoft.Office.Interop.Excel;

namespace Geo.Utils
{

    /// <summary>
    /// ExcelUtil
    /// </summary>
    public class ExcelUtil
    {
        /// <summary>
        /// 1 = A, 2 = B 
        /// </summary>
        /// <param name="colNum"></param>
        /// <returns></returns>
        public static string GetColName(int colNum)
        {
            if(1<=colNum && colNum<=26)
                return ((char)(64 + colNum)).ToString();
            if (colNum > 26 && colNum <= 26 * 26)
            {
                //26进制
                string gewei = GetColName(colNum % 26);//个位           
                string shiwei = GetColName((int)(colNum / 26));//十位
                return shiwei + gewei;
            }
            if (colNum > 26 *26 && colNum <= 26 * 26 *26)
            {
                //26进制
                string gewei = GetColName(colNum % 26);//个位           
                string shiwei = GetColName((int)(colNum / 26));//十位
                string baiwei = GetColName((int)(colNum / (26 * 26)));//百位
                return baiwei + shiwei + gewei;
            }
            throw new Exception("本程序尚不支持如此大的Excel表格！");

        }
        /// <summary>
        /// 行乘法
        /// </summary>
        /// <param name="worksheet"></param>
        /// <param name="rowNum"></param>
        /// <param name="resultCol"></param>
        public static void RowCellTimes(Excel.Application worksheet, int rowNum, int resultCol)
        {
            RowCellTimes(worksheet, rowNum, resultCol, resultCol - 1, resultCol - 2);
        }
        /// <summary>
        /// 行乘法
        /// </summary>
        /// <param name="worksheet"></param>
        /// <param name="rowNum"></param>
        /// <param name="resultCol"></param>
        /// <param name="multierCol"></param>
        public static void RowCellTimes(Excel.Application worksheet, int rowNum, int resultCol, int multierCol)
        {
            RowCellTimes(worksheet, rowNum, resultCol, resultCol - 1, multierCol);
        }
        /// <summary>
        /// 行乘法
        /// </summary>
        /// <param name="worksheet"></param>
        /// <param name="rowNum"></param>
        /// <param name="resultCol"></param>
        /// <param name="aCol"></param>
        /// <param name="bCol"></param>
        public static void RowCellTimes(Excel.Application worksheet, int rowNum, int resultCol, int aCol, int bCol)
        {
            string resultColName = ExcelUtil.GetColName(resultCol);
            string aColName = ExcelUtil.GetColName(aCol);
            string bColName = ExcelUtil.GetColName(bCol);

            string Formula = "=" + aColName + rowNum + "*" + bColName + rowNum;
            Range range = worksheet.get_Range(resultColName + rowNum); //求乘积：
            range.Formula = Formula;
            //MessageBox.Show(Formula);
            range.Calculate();
        }
        /// <summary>
        /// 普通格式化表格
        /// </summary>
        /// <param name="worksheet"></param>
        /// <param name="rowNum"></param>
        /// <param name="colNum"></param>
        public static void FormatTable(Excel.Application worksheet, int rowNum, int colNum)
        {
            string colName = ExcelUtil.GetColName(colNum);
            ////样式设定
            Range range = worksheet.get_Range("A2", colName + (rowNum - 1));
            range.Font.Bold = true;
            range.Borders.LineStyle = 1;     //设置单元格边框的粗细     
            range.BorderAround(
                XlLineStyle.xlContinuous,
                XlBorderWeight.xlThick,
                XlColorIndex.xlColorIndexAutomatic,
                System.Drawing.Color.Black.ToArgb());     //给单元格加边框  

            range.HorizontalAlignment = XlHAlign.xlHAlignCenter;     //设置字体在单元格内的对其方式
            range.VerticalAlignment = XlVAlign.xlVAlignCenter;    //文本垂直居中方式  
        }
        /// <summary>
        /// 在Excel单元格中写入字符串，并合并指定的单元格。
        /// </summary>
        /// <param name="worksheet"></param>
        /// <param name="val"></param>
        /// <param name="rowNum"></param>
        /// <param name="colNum"></param>
        /// <param name="rowSpan"></param>
        /// <param name="colSpan"></param>
        public static void Write(Excel.Application worksheet, string val, int rowNum, int colNum, int rowSpan = 1, int colSpan = 1)
        {
            worksheet.Cells[rowNum, colNum] = val;

            if (rowSpan != 1 || colSpan != 1)
            {
                string fromColName = ExcelUtil.GetColName(colNum);
                string toColName = ExcelUtil.GetColName(colNum + colSpan - 1);
                int toRowNum = rowNum + rowSpan - 1;

                //  MessageBox.Show(fromColName + ": " + colNum);
                worksheet.get_Range(fromColName + rowNum, toColName + toRowNum).Merge(0);
            }
        }

        /// <summary>
        /// 写表头，默认为写在第一行，并合到指定的列
        /// </summary>
        /// <param name="worksheet"></param>
        /// <param name="Title"></param>
        /// <param name="toCol"></param>
        /// <returns></returns>
        public static void WritteExcelTitle(Excel.Application worksheet,string Title, string toCol)
        {
            //---------------表头---------------------
            worksheet.Cells[1, 1] = Title ?? "门市";
            Range range = (Range)worksheet.get_Range("A1", toCol + "1");     //获取Excel多个单元格区域：本例做为Excel表头  
            range.Merge(0);     //单元格合并动作     
            range.Font.Size = 16;     //设置字体大小   
            range.Font.Bold = true;
            range.HorizontalAlignment = XlHAlign.xlHAlignCenter;     //设置字体在单元格内的对其方式
            range.VerticalAlignment = XlVAlign.xlVAlignCenter;    //文本垂直居中方式    
            range.Borders.LineStyle = 1;     //设置单元格边框的粗细     
            range.BorderAround(
                XlLineStyle.xlContinuous,
                XlBorderWeight.xlThick,
                XlColorIndex.xlColorIndexAutomatic,
                System.Drawing.Color.Black.ToArgb());     //给单元格加边框     
            range.Borders.get_Item(XlBordersIndex.xlEdgeTop).LineStyle = XlLineStyle.xlLineStyleNone; //设置单元格上边框为无边框     
            range.Borders.get_Item(XlBordersIndex.xlEdgeLeft).LineStyle = XlLineStyle.xlLineStyleNone;
            range.Borders.get_Item(XlBordersIndex.xlEdgeRight).LineStyle = XlLineStyle.xlLineStyleNone;
        }
        /// <summary>
        /// 将指定列的值[startRow:(resultRow-1)]加到指定单元格（colName + resultRow）
        /// </summary>
        /// <param name="worksheet"></param>
        /// <param name="colName"></param>
        /// <param name="startRow"></param>
        /// <param name="resultRow"></param>
        /// <returns></returns>
        public static Range SumRows(Excel.Application worksheet, string colName, int startRow, int resultRow)
        {
            Range range = worksheet.get_Range(colName + resultRow);
            range.Formula = "=SUM(" + colName + startRow + ":" + colName + (resultRow - 1) + ")";
            range.Calculate();
            return range;
        }
        /// <summary>
        /// 添加表格
        /// </summary>
        /// <param name="worksheet"></param>
        /// <param name="resultCellName"></param>
        /// <param name="cellNames"></param>
        /// <returns></returns>
        public static Range AddCells(Excel.Application worksheet, string resultCellName, List<string> cellNames)
        {
            string Formula = "=";
            int i = 0;
            foreach (string n in cellNames)
            {
                Formula += n;
                if (i != cellNames.Count - 1) Formula += "+";//最后一个免“+”号
                i++;
            }
            // MessageBox.Show(Formula);

            Range range = worksheet.get_Range(resultCellName);
            range.Formula = Formula;
            range.Calculate();
            return range;
        }

    }
}