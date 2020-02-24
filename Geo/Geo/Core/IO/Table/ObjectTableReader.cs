//2017.02.06, czs, create in hongqing, 表对象读取器。

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Data;
using System.Threading;
using Gnsser; 
using Geo.Utils;
using Geo.Coordinates;
using Geo.Referencing;
using Geo.Algorithm.Adjust;
using Geo.Common;
using Geo;
using Geo.Algorithm;
using Geo.IO;

namespace Geo
{
    /// <summary>
    /// 表对象读取器
    /// </summary>
    public class ObjectTableReader : AbstractReader<ObjectTableStorage>
    {
        /// <summary>
        /// 表对象读取器
        /// </summary>
        /// <param name="path"></param>
        /// <param name="encoding"></param>
        public ObjectTableReader(string path, Encoding encoding = null)
            : base(path, encoding)
        {
            this.FilePath = path;
            this.ColTypes = new NamedValueTypeManager();
            Appeared = new List<int>();
            this.StringSplitOptions = StringSplitOptions.None;
            Spliters = Setting.DefaultTextTableSpliter;// new string[] { "\t", "," };
        }
        /// <summary>
        /// 表对象读取器
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="encoding"></param>
        public ObjectTableReader(Stream stream, string[] spliters = null, Encoding encoding = null)
            : base(stream, encoding)
        {
            this.FilePath = "";
            this.ColTypes = new NamedValueTypeManager();
            Appeared = new List<int>();
            if (spliters == null)
            {
                spliters = Setting.DefaultTextTableSpliter;// new string[] { "\t", "," ,";","，"};
            }
            this.StringSplitOptions = StringSplitOptions  .None;
            this.Spliters = spliters;
        }
        /// <summary>
        /// 文件完整路径
        /// </summary>
        public string FilePath { get; set; }
             
        /// <summary>
        /// 分割器
        /// </summary>
        public string[] Spliters { get; set; }
        /// <summary>
        /// 列类型
        /// </summary>
        public NamedValueTypeManager  ColTypes { get; set; }
        /// <summary>
        /// 已出现
        /// </summary>
        public List<int> Appeared { get; set; }
        public StringSplitOptions StringSplitOptions { get; set; }
        /// <summary>
        /// 读取
        /// </summary>
        /// <returns></returns>
        public override ObjectTableStorage Read()
        {
            using (StreamReader reader = new StreamReader(Stream, this.Encoding))
            {
                //记录第一次出现的数据类型，后续依此转换
                var name = Path.GetFileNameWithoutExtension(FilePath);

                ObjectTableStorage storage = new ObjectTableStorage(name);
               
                string  [] titles = null;
                var line = "";
                int index = -1;
                while ((line = reader.ReadLine()) != null)
                {
                    if (String.IsNullOrWhiteSpace(line)) { continue; }

                    index++;
                    if (index == 0)//tittle
                    {
                        titles = line.Split(Spliters, StringSplitOptions);
                        ColTypes.InitNames(titles);
                        continue;
                    }

                    var vals = line.Split(Spliters, StringSplitOptions);//, StringSplitOptions.RemoveEmptyEntries);
                    storage.NewRow();
                    int length = Math.Min( titles.Length, vals.Length);
                    for (int i = 0; i < length; i++)
                    {
                        var title = titles[i].Trim();
                        var type = ColTypes[title];
                        var val = vals[i];
                        object newVal = null;
                        if (!Appeared.Contains(i))
                        {
                            type.ValueType = ValueTypeHelper.GetValueType(val);
                            Appeared.Add(i);
                        }
                        if (type.ValueType == ValueType.Unknown)
                        {
                            type.ValueType = ValueTypeHelper.GetValueType(val);
                        }

                        newVal = type.GetValue(val);  

                        if (newVal != null)//更新类型
                        { 
                           // type.ValueType = ValueTypeHelper.GetValueType(currentVal);
                            storage.AddItem(title, newVal);
                        }
                    }
                    //storage.AddItem(titles, vals);
                    storage.EndRow();
                }
                return storage;
            } 
        }

        /// <summary>
        /// 静态读取方法，读完就释放资源。
        /// </summary>
        /// <param name="path"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static ObjectTableStorage Read(string path, Encoding encoding = null)
        {
            using(ObjectTableReader reader = new ObjectTableReader(path, encoding))
            {
                return reader.Read();
            }

        }
    }
}