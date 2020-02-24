
//2014.06.24，czs ，create

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;

namespace Geo.Utils
{
    /// <summary>
    /// DataTable 工具类。
    /// </summary>
    public class DataTableUtil
    {

        static char[] Spliters = new char[] { ' ', '\t', ';', ',' };

        /// <summary>
        /// 将文本行打伞解析为数据表。
        /// </summary>
        /// <param name="lines"></param>
        /// <returns></returns>
        public static DataTable LinesToTable(string[] lines)
        {
            DataTable table = new DataTable();
            foreach (var line in lines)
            {
                var items = line.Split(Spliters, StringSplitOptions.RemoveEmptyEntries);
                table.Rows.Add(items);
            }
            return table;
        }


        /// <summary>
        /// 创建一个DataTable
        /// </summary>
        /// <param name="name"></param>
        /// <param name="headerText"></param>
        /// <param name="cells"></param>
        /// <param name="colTypes"></param>
        /// <returns></returns>
        public static DataTable Create(string name, string[] headerText, string[][] cells, Type [] colTypes = null)
        {
            if (colTypes != null && colTypes.Length != headerText.Length) throw new ArgumentException("标题和标题类型长度应该相同");
            if (headerText.Length != cells[0].Length) throw new ArgumentException("标题和内容长度应该相同");

            int col = headerText.Length;
            int row = cells.Length;

            DataTable table = new DataTable(name);
           // table.Columns = new DataColumnCollection();
            //header
            for (int i = 0; i < col; i++)
            {
                Type type = colTypes==null? typeof(string): colTypes[i];
                table.Columns.Add(headerText[i], type);
            }
             
            //content
            for (int i = 0; i < row; i++)
            {
                DataRow dataRow = table.NewRow();
                dataRow.ItemArray = cells[i];
                //for (int j = 0; j < col; j++)
                //{
                //    dataRow[j] = cells[i][j];
                //}

                table.Rows.Add(dataRow);
            }

            return table;
        }
    }
}
