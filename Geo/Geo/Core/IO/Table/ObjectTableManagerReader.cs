//2017.08.01, czs, create in hongqing, 单文件多表对象读取器。

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
    /// 单文件多表对象读取器
    /// </summary>
    public class ObjectTableManagerReader : AbstractReader<ObjectTableManager>
    {
        /// <summary>
        /// 单文件多表对象读取器
        /// </summary>
        /// <param name="path"></param>
        /// <param name="encoding"></param>
        public ObjectTableManagerReader(string path, Encoding encoding = null)
            :base(path, encoding)
        {
            this.FilePath = path;
            this.ColTypes = new Dictionary<string, NamedValueTypeManager>();
            Appeared = new Dictionary<string, List<int>>();
            Spliters = new string[] { "\t" };
            HeaderMarkers = new string[] { "#"};
        }
        /// <summary>
        /// 单文件多表对象读取器
        /// </summary>
        /// <param name="path"></param>
        /// <param name="encoding"></param>
        public ObjectTableManagerReader(Stream path, Encoding encoding = null)
            :base(path, encoding)
        {
          //  this.FilePath = path;
            this.ColTypes = new Dictionary<string, NamedValueTypeManager>();
            Appeared = new Dictionary<string, List<int>>();
            Spliters = new string[] { "\t" };
            HeaderMarkers = new string[] { "#"};
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
        /// 分割器
        /// </summary>
        public string[] HeaderMarkers { get; set; }
        /// <summary>
        /// 列类型
        /// </summary>
        public Dictionary<string, NamedValueTypeManager> ColTypes { get; set; }

       
        public  Dictionary<string,List<int>> Appeared { get; set; }
        /// <summary>
        /// 优先解析
        /// </summary>
        public bool IsIntOrFloatFirst { get; set; }
        /// <summary>
        /// 读取
        /// </summary>
        /// <returns></returns>
        public override ObjectTableManager Read()
        {
            using (StreamReader reader = new StreamReader(Stream, this.Encoding))
            {
                //记录第一次出现的数据类型，后续依此转换
                var name = Path.GetFileNameWithoutExtension(FilePath);

                ObjectTableManager mgr = new ObjectTableManager() { Name = name }; 
               
                string  [] titles = null;
                var line = "";
                int index = -1;
                bool isHeaderEnd = false;
                while ((line = reader.ReadLine()) != null)
                {
                    if (String.IsNullOrWhiteSpace(line)) { continue; }

                    index++;

                    //处理头部
                    if(!isHeaderEnd){
                        string marker;
                        if (IsHeaderLine(line, out marker))
                            {
                                line = line.Replace(marker, "");
                                if (String.IsNullOrWhiteSpace(line)) { continue; }

                                titles = line.Split(Spliters, StringSplitOptions.RemoveEmptyEntries);
                                var colNameMgr = new NamedValueTypeManager( );
                                colNameMgr.InitNames(titles);
                                var key = titles[0].Trim() ;
                                ColTypes[key] = colNameMgr;

                                mgr.AddTable(key);
                                continue;
                            }
                        else
                        {
                            isHeaderEnd = true;         
                        }
                    }
                  
                    //处理内容
                    if (String.IsNullOrWhiteSpace(line)) { continue; }

                    var strs = line.Split(Spliters, StringSplitOptions.None);//, StringSplitOptions.RemoveEmptyEntries);

                    var keyName = strs[0].Trim();
                    var storage = mgr.Get(keyName);
                    List<int> appeard = null;
                    if (!Appeared.ContainsKey(keyName)) {  Appeared[keyName] = new List<int>(); }
                    appeard = Appeared[keyName];

                    var colTypes =  ColTypes[keyName];
                    titles = colTypes.Titles;
                    storage.NewRow();
                    int length = Math.Min( titles.Length, strs.Length);
                    for (int i = 1; i < length; i++)
                    {
                        var title = titles[i].Trim();
                        var type = colTypes[title];
                        var val = strs[i];
                        
                        object newVal = null;
                        if (!appeard.Contains(i))
                        {
                            type.ValueType = ValueTypeHelper.GetValueType(val, IsIntOrFloatFirst);
                            appeard.Add(i);
                        }
                        newVal = type.GetValue(val);

                        if (newVal != null)
                        {
                            storage.AddItem(title, newVal);
                        }
                    }
                    //storage.AddItem(titles, vals);
                    storage.EndRow();
                }
                return mgr;
            } 
        }

        public bool IsHeaderLine(string line, out string marker)
        {
            foreach (var item in HeaderMarkers)
            {
                if (line.StartsWith(item))
                {
                    marker = item;
                    return true;
                }
            }
            marker = "";
            return false;
        }
    }
}