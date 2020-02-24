using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Geo.Winform
{ 
    /// <summary>
    /// 搜索选项
    /// </summary>
    public class EntitySearchOption : DbSearchItemOption
    {
        /// <summary>
        /// 查询生成器。
        /// </summary>
        public EntityQueryBuilder QueryManager { get; set; } 
    }


    /// <summary>
    /// 数据库搜索选项
    /// </summary>
    public class DbSearchItemOption
    { 
        /// <summary>
        /// 需要隐藏（不显示）的属性名称
        /// </summary>
        public List<string> InvisibleAttributes { get; set; }
        /// <summary>
        /// 类类型
        /// </summary>
        public Type ClassType { get; set; }
        /// <summary>
        /// 代选对象
        /// </summary>
        public Dictionary<String, Object> PropertiesDic { get; set; }
        /// <summary>
        /// 是否使用属性的Display属性值
        /// </summary>
        public bool UseDisplayName { get; set; }

    }
}
