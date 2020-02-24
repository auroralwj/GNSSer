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
                //26����
                string gewei = GetColName(colNum % 26);//��λ           
                string shiwei = GetColName((int)(colNum / 26));//ʮλ
                return shiwei + gewei;
            }
            if (colNum > 26 *26 && colNum <= 26 * 26 *26)
            {
                //26����
                string gewei = GetColName(colNum % 26);//��λ           
                string shiwei = GetColName((int)(colNum / 26));//ʮλ
                string baiwei = GetColName((int)(colNum / (26 * 26)));//��λ
                return baiwei + shiwei + gewei;
            }
            throw new Exception("�������в�֧����˴��Excel���");

        }
        /// <summary>
        /// �г˷�
        /// </summary>
        /// <param name="worksheet"></param>
        /// <param name="rowNum"></param>
        /// <param name="resultCol"></param>
        public static void RowCellTimes(Excel.Application worksheet, int rowNum, int resultCol)
        {
            RowCellTimes(worksheet, rowNum, resultCol, resultCol - 1, resultCol - 2);
        }
        /// <summary>
        /// �г˷�
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
        /// �г˷�
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
            Range range = worksheet.get_Range(resultColName + rowNum); //��˻���
            range.Formula = Formula;
            //MessageBox.Show(Formula);
            range.Calculate();
        }
        /// <summary>
        /// ��ͨ��ʽ�����
        /// </summary>
        /// <param name="worksheet"></param>
        /// <param name="rowNum"></param>
        /// <param name="colNum"></param>
        public static void FormatTable(Excel.Application worksheet, int rowNum, int colNum)
        {
            string colName = ExcelUtil.GetColName(colNum);
            ////��ʽ�趨
            Range range = worksheet.get_Range("A2", colName + (rowNum - 1));
            range.Font.Bold = true;
            range.Borders.LineStyle = 1;     //���õ�Ԫ��߿�Ĵ�ϸ     
            range.BorderAround(
                XlLineStyle.xlContinuous,
                XlBorderWeight.xlThick,
                XlColorIndex.xlColorIndexAutomatic,
                System.Drawing.Color.Black.ToArgb());     //����Ԫ��ӱ߿�  

            range.HorizontalAlignment = XlHAlign.xlHAlignCenter;     //���������ڵ�Ԫ���ڵĶ��䷽ʽ
            range.VerticalAlignment = XlVAlign.xlVAlignCenter;    //�ı���ֱ���з�ʽ  
        }
        /// <summary>
        /// ��Excel��Ԫ����д���ַ��������ϲ�ָ���ĵ�Ԫ��
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
        /// д��ͷ��Ĭ��Ϊд�ڵ�һ�У����ϵ�ָ������
        /// </summary>
        /// <param name="worksheet"></param>
        /// <param name="Title"></param>
        /// <param name="toCol"></param>
        /// <returns></returns>
        public static void WritteExcelTitle(Excel.Application worksheet,string Title, string toCol)
        {
            //---------------��ͷ---------------------
            worksheet.Cells[1, 1] = Title ?? "����";
            Range range = (Range)worksheet.get_Range("A1", toCol + "1");     //��ȡExcel�����Ԫ�����򣺱�����ΪExcel��ͷ  
            range.Merge(0);     //��Ԫ��ϲ�����     
            range.Font.Size = 16;     //���������С   
            range.Font.Bold = true;
            range.HorizontalAlignment = XlHAlign.xlHAlignCenter;     //���������ڵ�Ԫ���ڵĶ��䷽ʽ
            range.VerticalAlignment = XlVAlign.xlVAlignCenter;    //�ı���ֱ���з�ʽ    
            range.Borders.LineStyle = 1;     //���õ�Ԫ��߿�Ĵ�ϸ     
            range.BorderAround(
                XlLineStyle.xlContinuous,
                XlBorderWeight.xlThick,
                XlColorIndex.xlColorIndexAutomatic,
                System.Drawing.Color.Black.ToArgb());     //����Ԫ��ӱ߿�     
            range.Borders.get_Item(XlBordersIndex.xlEdgeTop).LineStyle = XlLineStyle.xlLineStyleNone; //���õ�Ԫ���ϱ߿�Ϊ�ޱ߿�     
            range.Borders.get_Item(XlBordersIndex.xlEdgeLeft).LineStyle = XlLineStyle.xlLineStyleNone;
            range.Borders.get_Item(XlBordersIndex.xlEdgeRight).LineStyle = XlLineStyle.xlLineStyleNone;
        }
        /// <summary>
        /// ��ָ���е�ֵ[startRow:(resultRow-1)]�ӵ�ָ����Ԫ��colName + resultRow��
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
        /// ��ӱ��
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
                if (i != cellNames.Count - 1) Formula += "+";//���һ���⡰+����
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