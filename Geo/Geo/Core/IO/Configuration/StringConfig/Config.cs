//2015.01.18, czs, create in namu,  配置文件内容
//2015.09.26, czs, edit in hongqing, BaseDictionary<string, ConfigItem>
//2016.02.10, czs, edit in hongqing,  增加默认数据，应对变化
//2018.03.19, czs, edti in hmx, 与分组配置文件合并，提取抽象类

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Geo.Common;
using System.IO;
using Geo.IO;

namespace Geo.IO
{
    /// <summary>
    /// 配置文件内容.
    /// 通常以#为注释，以=为赋值符号，以行为单位的配置文件。
    /// </summary>
    public class Config : TypedConfig<ConfigItem, string, string> // BaseDictionary<string, ConfigItem>
    {
        protected const string  Common= "Common";

        /// <summary>
        ///配置文件内容
        /// </summary> 
        public Config()
        {
            Version = 1.0;
            Author = "Gnsser";
            this.CreationTime = DateTime.Now;
        }

        /// <summary>
        /// 采用已有列表初始化
        /// </summary>
        /// <param name="ConfigItems"></param>
        /// <param name="Comments"></param>
        public Config(IDictionary<String, ConfigItem> ConfigItems, List<String> Comments) :base (ConfigItems, Comments)
        {
            Version = 1.0;
            Author = "Gnsser";
            this.CreationTime = DateTime.Now;
        }

        protected override ConfigItem CreateItem(string Name, string Value, string group = "Default", string Comment = "")
        {
            return new ConfigItem(Name, Value, group, Comment);
        }

        #region 配置文件公共属性

        /// <summary>
        /// 版本
        /// </summary>
        public double Version { get { return GetDouble(VariableNames.Version); } set { SetObj(VariableNames.Version, value, Common); } }
        /// <summary>
        /// 作者
        /// </summary>
        public string Author { get { return GetString(VariableNames.Author); } set { SetObj(VariableNames.Author, value, Common); } }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreationTime { get { return GetDateTime(VariableNames.CreationTime); } set { SetObj(VariableNames.CreationTime, value, Common); } }

        #endregion
        /// <summary>
        /// 解析
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        protected override string ParseString(string str)
        {
            return str;
        }
    }
     
}
