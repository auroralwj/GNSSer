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
    public abstract class ObjectConfig<TItem, TKey> : TypedConfig<TItem, TKey, Object> // BaseDictionary<string, ConfigItem>
        where TItem  : ObjectConfigItem<TKey>
      //    where TKey : IComparable<TKey>
    {
        /// <summary>
        ///配置文件内容
        /// </summary> 
        public ObjectConfig() 
        {  
        }

        /// <summary>
        /// 采用已有列表初始化
        /// </summary>
        /// <param name="ConfigItems"></param>
        /// <param name="Comments"></param>
        public ObjectConfig(IDictionary<TKey, TItem> ConfigItems, List<String> Comments) :base (ConfigItems, Comments)
        { 
        }

        //protected override ObjectConfigItem<Tkey> CreateItem(Tkey Name, Object Value, string group = "Default", string Comment = "")
        //{
        //    return new ObjectConfigItem<Tkey>(Name, Value, group, Comment);
        //}

        /// <summary>
        /// 解析
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        //protected override Object ParseString(string str)
        //{
        //    return str;
        //}
    }
     
}
