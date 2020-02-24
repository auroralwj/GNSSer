//2015.01.18, czs, create in namu,  配置文件内容读取器
//2018.03.19, czs, edti in hmx, 与分组配置文件合并，提取抽象类
//2018.03.20, czs, edti in hmx, OptionConfig 与配置Option绑定

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using Geo.Common;
using Geo.IO;

namespace Gnsser
{
    /// <summary>
    /// 配置文件内容。
    /// 通常为一行一个变量和值。采用分隔符分开。
    /// </summary>
    public  class OptionConfigReader :  ObjectConfigReader<OptionConfig, OptionConfigItem, OptionName> //: TypedConfigReader<ObjectConfig<TKey>, ObjectConfigItem<TKey>, TKey, Object>// AbstractReader<Config>
    {
        /// <summary>
        ///配置文件内容
        /// </summary> 
        /// <param name="path">路径</param> 
        /// <param name="ValueSplitter">数值分隔符</param> 
        /// <param name="CommentSplitter">注释分隔符</param> 
        public OptionConfigReader(string path, string ValueSplitter = "=", string CommentSplitter = "#") : this(new FileStream(path, FileMode.Open), Encoding.UTF8, ValueSplitter, CommentSplitter) { }

        /// <summary>
        ///配置文件内容
        /// </summary> 
        /// <param name="stream">数据流</param>
        /// <param name="encoding">编码</param>
        /// <param name="ValueSplitter">数值分隔符</param> 
        /// <param name="CommentSplitter">注释分隔符</param> 
        public OptionConfigReader(Stream stream, Encoding encoding = null, string ValueSplitter = "=", string CommentSplitter = "#")
            : base(stream, encoding?? Encoding.UTF8, ValueSplitter, CommentSplitter)
        { 
        }
         
         
        /// <summary>
        /// 创建配置表
        /// </summary>
        /// <returns></returns>
        protected override OptionConfig CreateConfig()
        {
            return new OptionConfig();
        }

        /// <summary>
        /// 解析字符串
        /// </summary>
        /// <param name="str"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        protected override Object ParseValue(string str, OptionName key)
        {
        return OptionManager.ParseValue(str, key);
    }

        /// <summary>
        /// 创建配置项目
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="Value"></param>
        /// <param name="group"></param>
        /// <param name="Comment"></param>
        /// <returns></returns>
        protected override OptionConfigItem CreateItem(OptionName Name, Object Value, string group = "Default", string Comment = "")
        {
            return new OptionConfigItem(Name, Value, group, Comment);
        }

        /// <summary>
        /// 解析关键字
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        protected override OptionName ParseKey(string str)
        {
            return (OptionName)Enum.Parse(typeof(OptionName), str);
        }
         
    }
}
