//2016.02.10, czs, 表格读取器

using System;
using System.Data;
using System.Collections.Generic;

namespace Geo.IO
{
    /// <summary>
    /// 文件读取器，表格。
    /// </summary>
    public interface ITableReader
    { 
        /// <summary>
        /// 读取为模型列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        List<T> ReadToModels<T>() where T : class;
        /// <summary>
        /// 读取
        /// </summary>
        /// <returns></returns>
        DataTable ReadToRawTable();
    }
     

    /// <summary>
    /// 文件读取器，表格。
    /// </summary>
    public interface ITableFileReader : ITableReader
    {
        /// <summary>
        /// 文件/网络等输入路径
        /// </summary>
        string InputPath { get; set; } 
    }
     

    /// <summary>
    /// 数据表读取器
    /// </summary>
    public abstract class TableFileReader : ITableFileReader
    {
        protected Log log = new Log(typeof(TableFileReader));
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="path"></param>
        public TableFileReader(string path)
        {
            this.InputPath = path;
        }
        /// <summary>
        /// 文件路径
        /// </summary>
        public string InputPath { get; set; }

        /// <summary>
        /// 读取数据表。
        /// </summary>
        /// <returns></returns>
        public DataTable ReadToRawTable()
        {
            List<string[]> table = ReadToTable();

            int colCount = table[0].Length;

            DataTable dataTable = new DataTable();
            for (int i = 0; i < colCount; i++)
            {
                dataTable.Columns.Add("列" + i);
            }

            foreach (var item in table)
            {
                while (dataTable.Columns.Count < item.Length)
                {
                    dataTable.Columns.Add("列" + dataTable.Columns.Count);
                }

                //dataTable.Rows.Add(key.ToArray());
                dataTable.Rows.Add(item);
            }
            return dataTable;
        }
        /// <summary>
        /// 解析为原始数据表
        /// </summary>
        /// <returns></returns>
        protected abstract List<string[]> ReadToTable();
        /// <summary>
        /// 解析为模型
        /// </summary>
        /// <returns></returns>
        public abstract List<T> ReadToModels<T>() where T : class;
    }
     
}
