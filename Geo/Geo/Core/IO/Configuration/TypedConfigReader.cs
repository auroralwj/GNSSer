//2015.01.18, czs, create in namu,  配置文件内容读取器
//2018.03.19, czs, edti in hmx, 与分组配置文件合并

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using Geo.Common;

namespace Geo.IO
{
    /// <summary>
    /// 配置文件内容。
    /// 通常为一行一个变量和值。采用分隔符分开。
    /// </summary>
    public abstract  class TypedConfigReader<TTable, TItem,TKey, TValue>  : AbstractReader<TTable> 
        where TTable: TypedConfig<TItem, TKey, TValue>
        where TItem : TypedConfigItem<TKey, TValue>
       //   where TKey : IComparable<TKey>
    {
        /// <summary>
        ///配置文件内容
        /// </summary> 
        /// <param name="path">路径</param> 
        /// <param name="ValueSplitter">数值分隔符</param> 
        /// <param name="CommentSplitter">注释分隔符</param> 
        public TypedConfigReader(string path, string ValueSplitter = "=", string CommentSplitter = "#") : this(new FileStream(path, FileMode.Open), Encoding.UTF8, ValueSplitter, CommentSplitter) { }

        /// <summary>
        ///配置文件内容
        /// </summary> 
        /// <param name="stream">数据流</param>
        /// <param name="encoding">编码</param>
        /// <param name="ValueSplitter">数值分隔符</param> 
        /// <param name="CommentSplitter">注释分隔符</param> 
        public TypedConfigReader(Stream stream, Encoding encoding, string ValueSplitter = "=", string CommentSplitter = "#")
            : base(stream, encoding)
        {
            this.CommentSplitter = CommentSplitter;
            this.ValueSplitter = ValueSplitter;
        }

        /// <summary>
        /// 用于分割名称和值
        /// </summary>
        public string ValueSplitter { get; set; }
        /// <summary>
        /// 用于标记和分割注释
        /// </summary>
        public string CommentSplitter { get; set; }

        /// <summary>
        /// 读取配置文件
        /// </summary>
        /// <returns></returns>
        public override TTable Read()
        {
            TTable config = CreateConfig();
            using (StreamReader reader = new StreamReader(this.Stream))
            {
                string line = null;
                string group = "Default";
                while ((line = reader.ReadLine()) != null)
                {
                    //空白行
                    if (String.IsNullOrWhiteSpace(line)) continue;
                    //全文注释
                    var trimed = line.Trim();
                    if (trimed.StartsWith(CommentSplitter)) // 为注释行，
                    {
                        var words = trimed.Replace(CommentSplitter, "").Trim();
                        //查看是否为分组名称
                        if (words.StartsWith("["))
                        {
                            words = words.Replace("[", "");
                            var len = words.Length;
                            if (words.Contains("]"))
                            {
                                len = words.IndexOf("]");
                            }

                            group = words.Substring(0, len).Trim();
                        }
                        else
                        {
                            config.AddComment(words);
                        }
                        continue;
                    }
                    //下面具体内容了。
                    var item = ParseLine(line, group);
                    config.Set(item);
                }
            }
            return config;
        }

        /// <summary>
        /// 创建表
        /// </summary>
        /// <returns></returns>
        protected abstract TTable CreateConfig();


        /// <summary>
        /// 解析一行字符串。
        /// </summary>
        /// <param name="line">字符串行</param>
        /// <param name="group">分组</param>
        /// <returns></returns>
        public TItem ParseLine(string line, string group)
        {
            string val = "";
            string comment = "";
            //名称
            var valSplitterIndex = line.IndexOf(ValueSplitter);
            var name = line.Substring(0, valSplitterIndex).Trim();
            var key = ParseKey(name);

            //数值起始位置
            var valStartIndex = valSplitterIndex + ValueSplitter.Length;
            //注释
            if (line.Contains(CommentSplitter))
            {
                var CommentSplitterIndex = line.IndexOf(CommentSplitter);
                var valLen = CommentSplitterIndex - valStartIndex;
                val = line.Substring(valStartIndex, valLen).Trim();

                var commentStart = CommentSplitterIndex + CommentSplitter.Length;
                comment = line.Substring(commentStart).Trim();
            }
            else //没有注释
            {
                val = line.Substring(valStartIndex).Trim();
            }
            var Tval = ParseValue(val, key);

            return CreateItem(key, Tval, group, comment);
        }
        /// <summary>
        /// 解析字符串
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        protected abstract TKey ParseKey(string str);

        /// <summary>
        /// 解析字符串
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="key">关键字</param>
        /// <returns></returns>
        protected abstract TValue ParseValue(string str, TKey key);

        /// <summary>
        /// 创建项目
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="Value"></param>
        /// <param name="group"></param>
        /// <param name="Comment"></param>
        /// <returns></returns>
        protected abstract TItem CreateItem(TKey Name, TValue Value, string group = "Default", string Comment = "");
    }
}
