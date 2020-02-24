//2015.09.28, czs, create in xi'an hongqing, Gnsser文件读取通用接口
//2016.12.28, czs, edit in hongqing, 修改属性读取，增加属性的字符串范围

using System;
using System.IO;
using System.Text;
using System.Data;
using Geo.Common;
using Geo.Coordinates;
using System.Collections;
using System.Collections.Generic;

namespace Geo.IO
{
    /// <summary>
    /// 一个位置。
    /// </summary>
    public class IntPosition
    {
        /// <summary>
        /// 属性
        /// </summary>
        /// <param name="startIndex"></param>
        /// <param name="count"></param>
        public IntPosition( int startIndex, int count)
        {
            this.StartIndex = startIndex;
            this.Count = count;
        }

        /// <summary>
        /// 起始编号
        /// </summary>
        public int StartIndex { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public int Count { get; set; }
    }

    /// <summary>
    /// 行文件读取通用接口。本类采用流式读取，实现了枚举接口 IEnumerable 和 IEnumerator。
    /// </summary>
    public  class LineFileReader<T> : Geo.IO.EntityFileReader<T> where T : IOrderedProperty
    {
        Log log = new Log(typeof(LineFileReader<T>));
        #region 构造函数与初始化
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public LineFileReader() { }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="filePath">数据文件路径。</param>
        /// <param name="metaFilePathOrDirectory">元数据文件或者元数据文件存放的目录。若不指定，则自动寻找文件目录下的元数据，若没有则调用默认的元数据，若还没有则报错。</param>
        public LineFileReader(string filePath, string metaFilePathOrDirectory = null)
            : base(filePath, metaFilePathOrDirectory)
        { 
        } 

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="filePath">数据文件路径。</param>
        /// <param name="Gmetadata"></param>
        public LineFileReader(string filePath, Gmetadata Gmetadata): base( filePath, Gmetadata )
        {
           
        }
        protected override void Init(Gmetadata Gmetadata)
        {
            Comments = new List<string>();
            CommentMarkers = Gmetadata.CommentMarkers;
            base.Init(Gmetadata);
        }

        /// <summary>
        /// 设置数据流
        /// </summary>
        protected override  void InitStreamReader()
        {
            StreamReader = new StreamReader(InputPath, System.Text.Encoding.Default, true);
            this.CurrentIndex = -1;
        }
       #endregion

        #region 属性
        #region 字符串分段方法,若不设置，默认为
        /// <summary>
        /// 采用固定字符串截取。此处采用字典而不采用列表，是因为若不需要解析的就可以不解析了。
        /// </summary>
        public Dictionary<string, IntPosition> PropertyPositions { get; set; }
        /// <summary>
        /// 项目长度集合，适用于以长度确定行的文件。
        /// 如果没有指定，则采用分隔符分隔。
        /// </summary>
        public List<int> ItemLengthes { get; set; }
        #endregion 
        /// <summary>
        /// 注释行起始字符
        /// </summary>
        public string[] CommentMarkers { get; set; }
        /// <summary>
        /// 数据流阅读器
        /// </summary>
        private StreamReader StreamReader { get; set; }  
        #endregion


        #region Read Convert

        /// <summary>
        /// 移动到下一个。
        /// </summary>
        /// <returns></returns>
        public override bool MoveNext()
        {
            #region 流程控制
            CurrentIndex++;
            if (CurrentIndex == StartIndex) { log.Debug("数据流 " + this.Name + " 开始读取数据。"); }
            if (this.IsCancel) { log.Info("数据流 " + this.Name + " 已被手动取消。"); return false; }
            if (CurrentIndex > this.MaxEnumIndex) { log.Info("数据流 " + this.Name + " 已达指定的最大编号 " + this.MaxEnumIndex); return false; }
            while (CurrentIndex < this.StartIndex) { ReadContentLine(); CurrentIndex++; }
            #endregion

            var content = ReadContentLine();
            if (!string.IsNullOrEmpty(content))
            {
                Current = ParseLine(content);
                return true;

            }
            return false;
        }

        /// <summary>
        /// 读取内容行
        /// </summary>
        /// <returns></returns>
        public string ReadContentLine()
        {
            var line = StreamReader.ReadLine();
            if (line == null) { return null; }
            if (IsEndLine(line)) { log.Info("读取到结尾符号END，结束读取 " + InputPath); return string.Empty; }

            //是否是注释行
            while (line != null && (IsCommentLine(line) || string.IsNullOrWhiteSpace(line)))
            { 
                line = StreamReader.ReadLine();
                if (line == null) { return null; }

                if(!string.IsNullOrWhiteSpace(line)){
                    Comments.Add(line.Substring(1));
                }
                if (IsEndLine(line)) { log.Info("读取到结尾符号END，结束读取 " + InputPath); return string.Empty; }
            }
            return GetContent(line);
        }
        public List<string> Comments { get; set; }

        /// <summary>
        /// 解析行为对象。包含分解行和解析两个内容。
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public virtual T ParseLine(string line)
        {
            string[] items =  SplitLine(line);
            return  this.Parse(items);
        } 

        #region 核心转换
        /// <summary>
        /// 字符串列表解析为属性
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>

        public virtual T Parse(string[] items)
        {
            T entity = Activator.CreateInstance<T>();
             
            var length = entity.OrderedProperties.Count;
                 
            for (int i = 0; i < length; i++)
            {
                 SetPropertyValue(items, entity, i);
            } 

            //更新上一个
            this.PreviousObject = entity;

            return entity;
        }

        /// <summary>
        /// 设置属性值
        /// </summary>
        /// <param name="items"></param>
        /// <param name="entity"></param>
        /// <param name="i"></param>
        public virtual void SetPropertyValue(string[] items, T entity, int i)
        {
            var propertyName = entity.OrderedProperties[i];
            var valString = GetPropertyValue(items, propertyName);
            var propertyInfo = EntityType.GetProperty(propertyName);
            var initVal = propertyInfo.GetValue(entity, null);
            object newValue = GetOrParseValueString(valString, propertyName);

            //赋值
            if (newValue != null && newValue != initVal)
            {
                if (newValue is Double && this.IsPropertyUnitChanged)
                {
                    newValue = Convert(propertyName, (Double)newValue);
                }
                propertyInfo.SetValue(entity, newValue, null);
            }

            //相对路径转绝对路径
            if (newValue is String)
            {
                var val = newValue as String;
                if (!String.IsNullOrWhiteSpace(val) && val.Contains("..\\")) //相对本地路径的标识
                {
                    val = Geo.Utils.PathUtil.GetAbsPath(val, BaseDirectory);

                    propertyInfo.SetValue(entity, val, null);
                }
            }
        }



        /// <summary>
        /// 单位转换
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public virtual double Convert(string propertyName, double val)
        {
            if (PropertyUnits.ContainsKey(propertyName) && DestPropertyUnits.ContainsKey(propertyName))
            {
                return UnitConverter.Convert(val, PropertyUnits[propertyName], DestPropertyUnits[propertyName]);
            }
            return val;
        }

        /// <summary>
        /// 分离
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public string[] SplitLine(string line)
        {
            if (line == null) { throw new ArgumentNullException("参数 line 不可为 null"); }

            string[] items = null;

            if (ItemLengthes != null && ItemLengthes.Count != 0)//采用紧密分段
            {
                List<string> contents = new List<string>();
                Geo.Utils.StringSequence sq = new Utils.StringSequence(line);
                foreach (var len in ItemLengthes)
                {
                    var val = sq.DeQueue(len);
                    contents.Add(val);
                }

                items = contents.ToArray();
            }
            else if (PropertyPositions != null && PropertyPositions.Count != 0) //采用固定字符串截取
            {
                List<string> contents = new List<string>();
                foreach (var item in this.Metadata.PropertyNames)
                {
                    var p = PropertyPositions[item];
                    var str = Geo.Utils.StringUtil.SubString(line, p.StartIndex, p.Count);
                    contents.Add(str);
                }
            }
            else // 采用分隔符
            {
                items = line.Trim().Split(ItemSpliters, StringSplitOptions.RemoveEmptyEntries);
            }

            return items;
        }
        /// <summary>
        /// 获取双精度浮点数，若果出错则抛出异常。
        /// </summary>
        /// <param name="items"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        protected double GetDoubleProperty(string[] items, string propertyName)
        {
            return Double.Parse(GetPropertyValue(items, propertyName));
        }
        /// <summary>
        /// 尝试获取指定索引的项目。若没有则返回null。
        /// </summary>
        /// <param name="items"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        protected string GetPropertyValue(string[] items, string propertyName)
        {
            return GetPropertyValue(items, PropertyIndexes[propertyName]);
        }
        /// <summary>
        /// 尝试获取指定索引的项目。若没有则返回null。
        /// </summary>
        /// <param name="items"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        protected string GetPropertyValue(string[] items, int index)
        {
            if (items.Length > index) { return items[index]; }
            return null;
        }
        #endregion

        #endregion

        /// <summary>
        /// 释放资源
        /// </summary>
        public override  void Dispose()
        {
            if (StreamReader != null && StreamReader != StreamReader.Null)
            {
                StreamReader.Close();
                StreamReader.Dispose();
            }
        }  
         

        #region tools

        /// <summary>
        /// 是否是注释行，即第一个字符,非如 # （空格除外），可以指定之
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public bool IsCommentLine(string line)
        {
            foreach (var item in CommentMarkers)
            {
                if (line.TrimStart().StartsWith(item))
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 是否为结果行
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public  bool IsEndLine(string line)
        {
            return line.Trim().Equals("End", StringComparison.CurrentCultureIgnoreCase);
        } 
        /// <summary>
        /// 返回内容行，如果没有内容，则返回 null 或 空白内容。
        /// 自动省略CommentMarkers号后面的内容。并替换转义字符。
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public  string GetContent(string line)
        {
            if (string.IsNullOrWhiteSpace(line)) return line;
            int indexOfComment = int.MaxValue;
            foreach (var item in this.CommentMarkers)
            {
                var convertedStr = @"\"+item;
                var index = line.IndexOf(item);
                var convertedIndex = line.IndexOf(convertedStr);//转义字符的位置
                if(convertedIndex == index - 1)
                {
                    line = line.Replace(convertedStr, item);
                    continue;
                }

                indexOfComment = Math.Min(indexOfComment, index);
            } 
            if (indexOfComment != -1)//find
            {
                return Geo.Utils.StringUtil.SubString( line, 0, indexOfComment);
            }
            return line;
        }
         
        #endregion
    }
}
