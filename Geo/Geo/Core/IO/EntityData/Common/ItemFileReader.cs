//2015.06.05, czs, craete in namu, 文本文件

 using System;   
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Geo.IO;

namespace Geo.Data
{
    /// <summary>
    /// 标记读取器在文件中的位置。
    /// </summary>
    public enum PositionOfFileReader
    {
        /// <summary>
        /// 头部段。即还没有正式读取内容。
        /// </summary>
        Header,
        /// <summary>
        /// 数据内容
        /// </summary>
        Content,
        /// <summary>
        /// 结束段。已经结束数据内容的读取。
        /// </summary>
        End
    }
     
    /// <summary>
    /// 文件读取
    /// </summary>
    /// <typeparam name="TItem"></typeparam>
    public abstract class ItemReader<TItem, TFile> : TableFileReader
        where TItem : Geo.IStringId 
        where TFile : ItemFile<TItem>,  new()
    {

        /// <summary>
        /// 构造函数。可以指定文件路径，但此处并不读取，需要调用Read()方法才读取。
        /// </summary>
        /// <param name="filePath"></param>
        public ItemReader(string filePath) : base(filePath)
        {
        }


        public static char[] Spliters = new char[] { ' ', '\t', ';', ',' };
        
        #region 文本段落起始与结束的标记
        public PositionOfFileReader PositionOfFileReader { get; set; }
        /// <summary>
        /// 起始行标记到内容的多余行数量
        /// </summary>
        public int InvalidLineCountAfterStartToken { get; set; }
        /// <summary>
        /// 起始行标记，其下 InvalidLineCountAfterStartToken 行为数据。
        /// </summary>
        public string TokenOfStartLine { get; set; }
        /// <summary>
        /// 结束行标记。
        /// </summary>
        public string TokenOfEndLine { get; set; }
        /// <summary>
        /// 是否有起始行标记。
        /// </summary>
        public bool HasTokenOfStartLine { get { return !String.IsNullOrEmpty(TokenOfStartLine); } }
        /// <summary>
        /// 是否有结束行标记
        /// </summary>
        public bool HasTokenOfEndLine { get { return !String.IsNullOrEmpty(TokenOfEndLine); } }

        #endregion

        #region 内容行的标记
        /// <summary>
        /// 内容行的起始标记。即之读取含有标记的行，如gamit globk org 文件。
        /// </summary>
        public string StartTokenOfContentLine { get; set; }
        /// <summary>
        /// 是否有内容行的起始标记。
        /// </summary>
        public bool HasStartTokenOfContentLine { get { return !String.IsNullOrEmpty(StartTokenOfContentLine); } }

        #endregion
        /// <summary>
        /// 读取文件为实体对象集合。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public override List<T> ReadToModels<T>()
        {
            List<T> list = new List<T>();
            var items = Read().Items;
            foreach (var item in items)
            {
                if (item is T)
                { list.Add(item as T); }
            }
            return list;
        }
        /// <summary>
        /// 读取
        /// </summary> 
        /// <returns></returns>
        public virtual TFile Read()
        {
            List<string[]> table = ReadToTable();

            TFile file = new TFile();
            foreach (var row in table)
            {
                var item = ParseRow(row);
                if (item != null) { file.Items.Add(item); }
            }
            return file;
        }



        /// <summary>
        /// 读取文件，解析为数据表
        /// </summary>
        /// <returns></returns>
        protected override List<string[]> ReadToTable()
        { 
            List<string[]> table = new List<string[]>();
            using (StreamReader sr = new StreamReader(InputPath, Encoding.Default))
            {
                PositionOfFileReader = Data.PositionOfFileReader.Header;
                string line = null;
                int i = 0;
                int StartLineNumber = 0;
                while ((line = sr.ReadLine()) != null)
                {
                    i++;
                    //忽略空行
                    if (String.IsNullOrWhiteSpace(line)) { continue; }

                    #region 判断起始和结束
                    if (PositionOfFileReader == Data.PositionOfFileReader.Header)
                    { 
                        StartLineNumber = i;
                        if (HasTokenOfStartLine) //如果有起始标记，则逐行进行判断
                        {
                            if (line.Contains(TokenOfStartLine)) {
                             
                                PositionOfFileReader = Data.PositionOfFileReader.Content; continue; }
                            else { continue; }
                        }//如果没有起始标记，则直接跳跃到内容
                        else {
                            PositionOfFileReader = Data.PositionOfFileReader.Content; }
                    }
                    //起始行到内容行之间忽略的部分
                    if (i - StartLineNumber < InvalidLineCountAfterStartToken)
                    {
                        continue;
                    }

                    if (PositionOfFileReader == Data.PositionOfFileReader.Content && HasTokenOfEndLine)
                    {
                        if (line.Contains(TokenOfEndLine)) { PositionOfFileReader = Data.PositionOfFileReader.End; break; }
                    }
                    #endregion

                    //判断是否内容行，若设置了起始标记。如Globk文件。
                    if (HasStartTokenOfContentLine)
                    {
                        if (!line.StartsWith(this.StartTokenOfContentLine))
                        { continue; }
                    }

                    if (!ExtraCheck(line)) { continue; }

                    //内容解析
                    string[] row = SplitLine(line);
                    table.Add(row);
                }
            }
            return table;
        }

        /// <summary>
        /// 分离行
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        protected virtual string[] SplitLine(string line)
        {
            string[] row = line.Split(Spliters, StringSplitOptions.RemoveEmptyEntries);
            return row;
        }

        /// <summary>
        /// 其它检查情况，判断是否是合格的内容行。
        /// 主要用于子对象的扩展。
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        protected virtual bool ExtraCheck(string line)
        {
            return true;
        }

        /// <summary>
        /// 解析
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        public abstract TItem ParseRow(string[] row);
    }

}
