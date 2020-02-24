//2015.10.20, czs, create in xi'an hongqing, Gnsser 行文件写入通用接口

using System;
using System.IO;
using System.Text;
using System.Data;
using Geo.Common;
using Geo.Coordinates;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace Geo.IO
{

    /// <summary>
    /// 行文件写入通用接口
    /// </summary>
    public class LineFileWriter<TLineClass> : Geo.IO.EntityWriter<TLineClass>, IDisposable
        where TLineClass : IOrderedProperty
    {
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public LineFileWriter() { ItemSpliter = '\t'; }

        #region 构造函数与初始化
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="filePath">数据文件路径。</param>
        /// <param name="metaFilePathOrDirectory">元数据文件或者元数据文件存放的目录。若不指定，则自动寻找文件目录下的元数据，若没有则调用默认的元数据，若还没有则报错。</param>
        public LineFileWriter(string filePath, string metaFilePathOrDirectory = null, Encoding Encoding = null, FileMode FileMode = FileMode.Create)
            : base(filePath, metaFilePathOrDirectory, Encoding, FileMode)
        {
            ItemSpliter = '\t';
        }


        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="filePath">数据文件路径。</param>
        /// <param name="Gmetadata"></param>
        public LineFileWriter(string filePath, Gmetadata Gmetadata, Encoding Encoding = null, FileMode FileMode = FileMode.Create)
            : base(filePath, Gmetadata, Encoding, FileMode)
        {
            ItemSpliter = '\t';
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="Stream">数据流。</param>
        /// <param name="Gmetadata"></param>
        public LineFileWriter(Stream Stream, Gmetadata Gmetadata, Encoding Encoding =null)
            : base(Stream, Gmetadata, Encoding)
        {
            ItemSpliter = '\t';
        }
           
       #endregion

        #region 属性
        /// <summary>
        /// 属性之间的分隔符号
        /// </summary>
        public char ItemSpliter { get; set; }
        /// <summary>
        /// 项目宽度是否固定，若是，分别判断 PropertyPositions 和 ItemLengthes 是否为空，决定采用
        /// </summary>
        public bool IsItemWidthFixed { get; set; }

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

        #endregion
        /// <summary>
        /// 写头部信息，具有注释
        /// </summary>
        public virtual void WriteHeaderLineWithComment()
        {
            StreamWriter.Write("#");
            WriteHeaderLine();
        }
        /// <summary>
        /// 写入一行头部信息
        /// </summary> 
        public virtual void WriteHeaderLine()
        {
            if (!IsItemWidthFixed)
            {
                if (PropertieNames != null && PropertieNames.Count != 0)
                {
                    foreach (var item in PropertieNames)
                    {
                        if (item != null)
                        {
                            StreamWriter.Write(item);
                        }
                        StreamWriter.Write(ItemSpliter);
                    }

                }
                else if (Properties != null && Properties.Count != 0)
                {
                    foreach (var item in Properties)
                    {
                        if (item != null)
                        {
                            StreamWriter.Write(item.Name);
                        }
                        StreamWriter.Write(ItemSpliter);
                    }
                }
            }
            else if (PropertyPositions != null && PropertyPositions.Count != 0)
            {
                StringBuilder sb = new StringBuilder();
                foreach (var item in Properties)
                {
                    var pos = PropertyPositions[item.Name];
                    var name = Geo.Utils.StringUtil.GetFixedLength(item.Name, pos.Count);
                    for (int i = 0; i < pos.StartIndex; i++)
                    {
                        sb.Append(" ");
                    }
                    sb.Append(name);
                }
                StreamWriter.Write(sb.ToString());
            }
            else if (ItemLengthes != null && ItemLengthes.Count != 0)
            {
                StringBuilder sb = new StringBuilder();
                int i = 0;
                foreach (var item in Properties)
                {
                    string name = item.Name;
                    name = Geo.Utils.StringUtil.GetFixedLength(name, ItemLengthes[i]);
                    if (ItemLengthes.Count > i + 1)//如果最后几位不指定，则采用最后可用的长度
                    {
                        i++;
                    }
                    sb.Append(name);
                }
                StreamWriter.Write(sb.ToString());
            }
            StreamWriter.WriteLine();
        }
        /// <summary>
        /// 写入一行注释
        /// </summary>
        /// <param name="comment"></param>
        public override void WriteCommentLine(string comment)
        {
            StreamWriter.WriteLine("# " + comment);
        }
        /// <summary>
        /// 写入一行注释
        /// </summary>
        /// <param name="comment"></param>
        public void WriteCommentLine(IEnumerable<string> collection)
        {
            foreach (var item in collection)
            {
                StreamWriter.WriteLine("# " + item);
            }         
        }
        /// <summary>
        /// 在当前行追加注释
        /// </summary>
        /// <param name="comment"></param>
        public override void AppendComment(string comment)
        {
            StreamWriter.Write("# " + comment);
        } 

        /// <summary>
        /// 写入
        /// </summary>
        /// <param name="obj"></param>
        public override void Write(TLineClass obj)
        {
            var line = EntityToLine(obj);

            StreamWriter.WriteLine(line);

            CurrentIndex++;
        }
        /// <summary>
        /// 对象序列化为字符串行
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public virtual string EntityToLine(TLineClass obj)
        {
            StringBuilder sb = new StringBuilder();  
            if (!IsItemWidthFixed)
            {
                int i = 0;
                foreach (var item in Properties)
                {
                    if (i != 0) { sb.Append(this.ItemSpliter); }

                    var str = PropertyToString(obj, item);

                    sb.Append(str);//???格式？？2015.10.20

                    i++;
                }

            }
            else if (PropertyPositions != null && PropertyPositions.Count != 0)
            { 
                foreach (var item in Properties)
                {
                    var pos = PropertyPositions[item.Name];
                    var str = PropertyToString(obj, item);
                    var name = Geo.Utils.StringUtil.GetFixedLength(str, pos.Count);
                    for (int i = 0; i < pos.StartIndex; i++)
                    {
                        sb.Append(" ");
                    }
                    sb.Append(name);
                } 
            }
            else if (ItemLengthes != null && ItemLengthes.Count != 0)
            { 
                int i = 0;
                foreach (var item in Properties)
                { 
                    var str = PropertyToString(obj, item); 
                   str = Geo.Utils.StringUtil.GetFixedLength(str, ItemLengthes[i]);
                    if (ItemLengthes.Count > i+1)//如果最后几位不指定，则采用最后可用的长度
                    {
                        i++;
                    }
                    sb.Append(str); 
                } 
            } 

            var line = sb.ToString();
            return line; 
        }

        /// <summary>
        /// 属性转换为字符串
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public virtual string PropertyToString(TLineClass obj, PropertyInfo item)
        {  
            var val = item.GetValue(obj, null);
            return PropertyToString(val);
        }
        /// <summary>
        /// 属性转为字符串
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public virtual string PropertyToString(object val)
        {
            var str = val.ToString();
            return str;
        } 
    }
}
