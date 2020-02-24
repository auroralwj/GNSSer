//2015.01.19, czs, create in numu, 配置文件的写
//2018.03.19, czs, edti in hmx, 与分组配置文件合并

using System;
using System.IO;
using System.Text;
using System.Collections.Generic;

namespace Geo.IO
{
    /// <summary>
    /// 包含Stream, Encoding属性，用于输入输出。
    /// </summary>
    public class TypedConfigWriter<TTable, TItem,  TKey, TValue>  : AbstractWriter<TTable>
        where TTable: TypedConfig<TItem, TKey, TValue>
        where TItem : TypedConfigItem<TKey, TValue>
          //where TKey : IComparable<TKey>
    {
        public TypedConfigWriter(string ValueSplitter = "=", string CommentSplitter = "#")
        {
            this.CommentSplitter = CommentSplitter;
            this.ValueSplitter = ValueSplitter;
            this.IsSort = true;
        }
        /// <summary>
        ///配置文件内容
        /// </summary> 
        /// <param name="path">路径</param> 
        /// <param name="ValueSplitter">数值分隔符</param> 
        /// <param name="CommentSplitter">注释分隔符</param> 
        public TypedConfigWriter(string path, string ValueSplitter = "=", string CommentSplitter = "#") : this(new FileStream(path, FileMode.Create), Encoding.UTF8, ValueSplitter, CommentSplitter) { }

        /// <summary>
        ///配置文件内容
        /// </summary> 
        /// <param name="stream">数据流</param>
        /// <param name="encoding">编码</param>
        /// <param name="ValueSplitter">数值分隔符</param> 
        /// <param name="CommentSplitter">注释分隔符</param> 
        public TypedConfigWriter(Stream stream, Encoding encoding, string ValueSplitter = "=", string CommentSplitter = "#")
            : base(stream, encoding)
        {
            this.CommentSplitter = CommentSplitter;
            this.ValueSplitter = ValueSplitter;
            this.IsSort = true;
        }
        /// <summary>
        /// 是否排序
        /// </summary>
        public bool IsSort { get; set; }

        /// <summary>
        /// 用于分割名称和值
        /// </summary>
        public string ValueSplitter { get; set; }
        /// <summary>
        /// 用于标记和分割注释
        /// </summary>
        public string CommentSplitter { get; set; }
        /// <summary>
        /// 写入一个到数据流。
        /// </summary>
        /// <returns></returns>
        public override void Write(TTable product)
        {
            if (product.Count == 0) return;
            string configText = BuildConfigText(product);

            using (StreamWriter sw = new StreamWriter(this.Stream, this.Encoding))
            {
                sw.Write(configText);
            }
        }

        /// <summary>
        /// 构建文本
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        public virtual string BuildConfigText(TTable product)
        {
            StringBuilder sb = new StringBuilder();
            //首先写注释
            foreach (var item in product.Comments)
            {
                sb.AppendLine(CommentSplitter + " " + item);
            }

            var list = product.Values;
            if (IsSort)//安装分组排序
            {
                list.Sort(new Comparison<TItem>((m1,m2)=>m1.Group.CompareTo(m2.Group)));
            }

            string lastGroup = null;

            foreach (var item in list)
            {
                var gitem = item;// as ConfigItem;

                //分组输出
                if ( String.IsNullOrWhiteSpace( lastGroup ) || lastGroup != gitem.Group)
                {
                    sb.AppendLine();
                    sb.AppendLine(CommentSplitter + " [" + gitem.Group + "]");
                }
                lastGroup = gitem.Group ?? "Default";

                sb.Append(Geo.Utils.StringUtil.FillSpaceRight(KeyToString(item.Name), 30) + this.ValueSplitter + ValueToString(item.Value));//"\t" + 

                if (item.Comment != null && !String.IsNullOrEmpty(item.Comment.Trim()))
                {
                    sb.Append("\t" + CommentSplitter + " " + item.Comment);
                }
                sb.AppendLine();
            }

            string configText = sb.ToString();
            return configText;
        }
        /// <summary>
        /// 值转换为字符串
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        protected virtual string ValueToString(TValue val)
        {
            return val + "";
        }
        /// <summary>
        /// 键转换为字符串
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        protected virtual string KeyToString(TKey key)
        {
            return key + "";
        }
    }
}
