//2015.01.19, czs, create in numu, 配置文件的写
//2018.03.19, czs, edti in hmx, 与分组配置文件合并
//2018.03.20, czs, edti in hmx, OptionConfig 与配置Option绑定

using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using Geo.IO;

namespace Gnsser
{
    /// <summary>
    /// 包含Stream, Encoding属性，用于输入输出。
    /// </summary>
    public class OptionConfigWriter: ObjectConfigWriter<OptionConfigItem, OptionName>// : TypedConfigWriter<ObjectConfig<TKey>, ObjectConfigItem<TKey>, TKey, Object>//AbstractWriter<Config>
    {
        public OptionConfigWriter()// : base(ValueSplitter, CommentSplitter)
        {

        }
        /// <summary>
        ///配置文件内容
        /// </summary> 
        /// <param name="path">路径</param> 
        /// <param name="ValueSplitter">数值分隔符</param> 
        /// <param name="CommentSplitter">注释分隔符</param> 
        public OptionConfigWriter(string path, string ValueSplitter = "=", string CommentSplitter = "#") : this(new FileStream(path, FileMode.Create), Encoding.UTF8, ValueSplitter, CommentSplitter) { }

        /// <summary>
        ///配置文件内容
        /// </summary> 
        /// <param name="stream">数据流</param>
        /// <param name="encoding">编码</param>
        /// <param name="ValueSplitter">数值分隔符</param> 
        /// <param name="CommentSplitter">注释分隔符</param> 
        public OptionConfigWriter(Stream stream, Encoding encoding, string ValueSplitter = "=", string CommentSplitter = "#")
            : base(stream, encoding, ValueSplitter, CommentSplitter)
        { 
        }

        /// <summary>
        /// 值转换为字符串
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        protected override string ValueToString(Object val)
        {
            return OptionManager.ObjToString( val );
        }
        /// <summary>
        /// 键转换为字符串
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        protected override string KeyToString(OptionName key)
        {
            return key + "";
        }
    }
}
