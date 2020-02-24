//2014.10.26, czs, create in numu, 为表格而设计
//2014.11.29, czs, edit in jinxinliangmao shuangliao, 接口从 IToTabString 改为IToTabRow， 增加 GetTabTitles， ToTabString 改为 GetTabValues

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data; 
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.IO;
using System.Threading;
using Geo.Algorithm;
using Geo.Algorithm.Adjust;


namespace Geo
{
    /// <summary>
    /// 为表格而设计
    /// </summary>
    public interface IToTabRow
    {
        /// <summary>
        /// 每个元素的标题
        /// </summary>
        /// <returns></returns>
        string GetTabTitles();

        /// <summary>
        /// 转换为以Tab为分隔符的字符串，易于粘贴到Excel等表格中。
        /// </summary>
        /// <returns></returns>
        string GetTabValues();
    }

    /// <summary>
    /// 表数据接口
    /// </summary>
    public interface ITableData : IToTabRow
    {
        /// <summary>
        /// 表标题完整列表
        /// </summary>
        List<string> WholeColTitles { get; } 
    }

    //2014.12.15, czs, create in namu shuangliao, 非紧凑表行数据，在统一的列下面将行数据进行填充。
    /// <summary>
    /// 表行数据。
    /// </summary>
    /// <typeparam name="T">类型</typeparam>
    public class TableRowData<T> : Named, ITableData
    {
        /// <summary>
        /// 表数据构造函数
        /// </summary>
        /// <param name="valDic">名称与值字典</param>
        /// <param name="WholeColTitles">标题</param>
        /// <param name="keyNames">关键标题（非关键标题将被忽略，如平差的RMS值）</param>
        public TableRowData(Dictionary<string, T> valDic, List<string> WholeColTitles, List<string> keyNames = null, string Placeholder = " ")
        {
            this.WholeColTitles = WholeColTitles;
            this.valDic = valDic;
            this.KeyNames = keyNames;
            this.Placeholder = Placeholder;
        }
        /// <summary>
        /// 索引标题，关键标题。
        /// </summary>
        public List<string> KeyNames { get; protected set; }
        /// <summary>
        /// 完整的表头，必须设置。
        /// </summary>
        public List<string> WholeColTitles { get; protected set; }
        /// <summary>
        /// 列数据字典
        /// </summary>
        public Dictionary<string, T> valDic { get; set; }

        public string GetTabTitles()
        {
            StringBuilder sb = new StringBuilder();
            int i = 0;
            foreach (var item in WholeColTitles)
            {
                if (i != 0) sb.Append("\t");
                sb.Append(item);
                i++;
            }
            return sb.ToString();
        }
        /// <summary>
        /// 获取以制表符分开的行，按照表对应的列进行填充。
        /// </summary>
        /// <param name="valDic"></param>
        /// <returns></returns>
        public string GetTabValues(Dictionary<string, T> valDic)
        {
            StringBuilder sb = new StringBuilder();
            int i = -1;
            foreach (var item in WholeColTitles)
            {   
                i++;             
                //非关键标题则跳过
                if (  KeyNames != null && !KeyNames.Contains(item)) continue;
                
                if (i != 0) sb.Append("\t");

                if (valDic.ContainsKey(item))
                    sb.Append(String.Format(new NumeralFormatProvider(), "{0}", valDic[item]));
                else sb.Append(String.Format(new NumeralFormatProvider(), "{0}", Placeholder)); ;//占位
            }
            return sb.ToString();
        }
        /// <summary>
        /// 占位符号
        /// </summary>
        public string Placeholder { get; set; }
        /// <summary>
        /// 获取表行
        /// </summary>
        /// <returns></returns>
        public string GetTabValues() { return GetTabValues(valDic); }
    }

    /// <summary>
    /// 表数据.非紧凑表行数据，在统一的列下面将行数据进行填充。
    /// </summary>
    public class TableRowData : TableRowData<Double>
    {
        /// <summary>
        /// 表数据构造函数
        /// </summary>
        public TableRowData(Dictionary<string, double> valDic, List<string> WholeColTitles, List<string> keyTitles = null, string Placeholder =" ")
            : base(valDic, WholeColTitles, keyTitles, Placeholder)
        { 
        }
    }

    public class IntTableRowData : TableRowData<Int32>
    {
        /// <summary>
        /// 表数据构造函数
        /// </summary>
        public IntTableRowData(Dictionary<string, int> valDic, List<string> WholeColTitles, List<string> keyTitles = null, string Placeholder = " ")
            : base(valDic, WholeColTitles, keyTitles, Placeholder)
        { 
        }
    }

    public class TableRowUtil
    {


        /// <summary>
        /// 用于具有多列的对象
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="dic">字典</param>
        /// <param name="FullParamNames">列的全部名称</param>
        /// <param name="Placeholder">默认数值</param>
        /// <param name="keyNames">列关键字</param>
        /// <returns></returns>
        public static TableRowData<T> GetTableRow<T>(Dictionary<string, T> dic, List<string> FullParamNames, Object Placeholder, List<string> keyNames = null)
        {
            if (Placeholder == null) Placeholder = " ";

            string placeHolder = Placeholder.ToString();

            if (Placeholder is IToTabRow) placeHolder = (Placeholder as IToTabRow).GetTabValues();

            TableRowData<T> row = new TableRowData<T>(dic, FullParamNames, keyNames, placeHolder);
            return row;
        }

    }
}
