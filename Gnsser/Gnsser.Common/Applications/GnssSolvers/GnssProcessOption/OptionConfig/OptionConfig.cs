//2015.01.18, czs, create in namu,  配置文件内容
//2015.09.26, czs, edit in hongqing, BaseDictionary<string, ConfigItem>
//2016.02.10, czs, edit in hongqing,  增加默认数据，应对变化
//2018.03.19, czs, edti in hmx, 与分组配置文件合并，提取抽象类
//2018.03.20, czs, edti in hmx, OptionConfig 与配置Option绑定

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Geo.Common;
using System.IO;
using Geo.IO;


namespace Gnsser
{
    /// <summary>
    /// 配置文件内容.
    /// 通常以#为注释，以=为赋值符号，以行为单位的配置文件。
    /// </summary>
    public class OptionConfig :  ObjectConfig<OptionConfigItem, OptionName>// TypedConfig<ObjectConfigItem<Tkey>, Tkey, Object> // BaseDictionary<string, ConfigItem>
    {
        /// <summary>
        ///配置文件内容
        /// </summary> 
        public OptionConfig() 
        { 
            var type = typeof(OptionName);
            var collection = Enum.GetNames(type);
            foreach (var item in collection)
            {
                var key = (OptionName)Enum.Parse(type, item);
                var val = GetOrCreate(key);
                this[key] = val;
            }

            var itm = GetOrCreate(OptionName.Version);
            itm.Value = 1;
            itm.Group = "Basic";
            itm.Comment = "自动生成";
            itm = GetOrCreate(OptionName.CreationTime);
            itm.Value = DateTime.Now;
            itm.Group = "Basic";
            itm.Comment = "自动生成";
            itm = GetOrCreate(OptionName.Author);
            itm.Value = "GNSSer, www.gnsser.com";
            itm.Group = "Basic";
            itm.Comment = "自动生成";
        }

        /// <summary>
        /// 采用已有列表初始化
        /// </summary>
        /// <param name="ConfigItems"></param>
        /// <param name="Comments"></param>
        public OptionConfig(IDictionary<OptionName, OptionConfigItem> ConfigItems, List<String> Comments) :base (ConfigItems, Comments)
        { 
        } 
        /// <summary>
        /// 创建一个新对象
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="Value"></param>
        /// <param name="group"></param>
        /// <param name="Comment"></param>
        /// <returns></returns>
        protected override OptionConfigItem CreateItem(OptionName Name, object Value, string group = "Default", string Comment = "")
        {
            if(Value == null)
            {
                Value = OptionManager.GetDefaultValue(Name);
            }

            return new OptionConfigItem(Name, Value, group, Comment);
        }

        /// <summary>
        /// 解析
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        protected override Object ParseString(string str)
        {
            return str;
        }
    }
     
}
