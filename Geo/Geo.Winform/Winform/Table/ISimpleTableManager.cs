//2015.05.31, czs, 增加注释 in namu，为数据表的显示而创建

using System;
using System.Collections.Generic;

namespace Geo.Winform
{
    /// <summary>
    /// 为数据表的显示而创建
    /// </summary>
    public interface ISimpleTableManager
    {
        /// <summary>
        /// 主题对象类型
        /// </summary>
        Type ClassType { get; set; }
        /// <summary>
        /// 不显示的列名称
        /// </summary>
        System.Collections.Generic.List<string> HiddenAttributes { get; set; }
        /// <summary>
        /// 数据表名称
        /// </summary>
        string TableName { get; set; }
        /// <summary>
        /// 是否使用Display名称
        /// </summary>
        bool UseDisplayName { get; set; }
        /// <summary>
        /// 获取表的列
        /// </summary>
        /// <returns></returns>
        System.Windows.Forms.DataGridViewColumn[] GetDataGridViewColumns();
        /// <summary>
        /// 获取表的列
        /// </summary>
        /// <param name="hiddenAttributes"></param>
        /// <param name="useDisplayName"></param>
        /// <returns></returns>
        System.Windows.Forms.DataGridViewColumn[] GetDataGridViewColumns(List<string> hiddenAttributes, bool useDisplayName, string [] colOrders=null);
     
    }
}
