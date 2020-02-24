//2015.01.18, czs, create in namu,  配置文件内容
//2018.03.19, czs, edit in hmx,  与分组配置文件合并，抽象为通用设置数据 


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Geo.Common;
using System.ComponentModel.DataAnnotations;
//using EF::System.ComponentModel.DataAnnotations.Schema;
//using System.Data.Entity;
using System.Globalization;
using Geo.IO;

namespace Geo.IO
{  
    
    /// <summary>
    /// 配置文件内容
    /// </summary>
    public abstract class ObjectConfigItem<TKey> : TypedConfigItem<TKey, Object>// IComparable<ConfigItem>
    {
        
        /// <summary>
        /// 构造函数
        /// </summary>
        public ObjectConfigItem(TKey Name, Object Value, string group = "Default", string Comment = "")
            :base(Name, Value, group, Comment)
        { 
        }

    }
}
