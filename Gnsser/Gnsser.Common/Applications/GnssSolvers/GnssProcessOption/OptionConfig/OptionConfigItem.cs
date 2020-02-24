//2015.01.18, czs, create in namu,  配置文件内容
//2018.03.19, czs, edit in hmx,  与分组配置文件合并，抽象为通用设置数据 
//2018.03.20, czs, edti in hmx, OptionConfig 与配置Option绑定

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

namespace Gnsser
{  
    
    /// <summary>
    /// 配置文件内容
    /// </summary>
    public class OptionConfigItem:   ObjectConfigItem<OptionName> //: TypedConfigItem<Tkey, Object>// IComparable<ConfigItem>
    {
        
        /// <summary>
        /// 构造函数
        /// </summary>
        public OptionConfigItem(OptionName Name, Object Value, string group = "Default", string Comment = "")
            :base(Name, Value, group, Comment)
        { 
        }
        /// <summary>
        /// 解析字符串
        /// </summary>
        /// <param name="str"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public override object ParseString(string str, OptionName name)
        { 
            return OptionManager.ParseValue( str, name);
        }

        /// <summary>
        /// 是否是枚举类型
        /// </summary>
        /// <returns></returns>
        public bool IsEnumType()
        {
            return OptionManager.IsEnumType(this.Name);
        }
    }
}
