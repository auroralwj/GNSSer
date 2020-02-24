using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Geo.Utils;
using System.Windows.Forms;
using System.Collections.Specialized; 

namespace Geo.Winform
{
    /// <summary>
    /// 提供一个基础实现。
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BaseSimpleTableManager<T> : ISimpleTableManager where T : class
    {
        public BaseSimpleTableManager(bool UseDisplayName = false)
        {
            this.UseDisplayName = UseDisplayName;
            this.ClassType = typeof(T);
            this.TableName = this.ClassType.Name; 
        }

        #region 属性
        /// <summary>
        /// 实体类型
        /// </summary>
        public Type ClassType { get; set; }
        /// <summary>
        /// 数据表名称
        /// </summary>
        public string TableName { get; set; } 
        /// <summary>
        /// 是否使用属性的Display属性显示
        /// </summary>
        public bool UseDisplayName { get; set; }
        /// <summary>
        /// 需要隐藏的属性名称
        /// </summary>
        public List<string> HiddenAttributes { get; set; }
        #endregion

        #region 方法
        /// <summary>
        /// 获取列
        /// </summary>
        /// <returns></returns>
        public virtual  DataGridViewColumn[] GetDataGridViewColumns()
        {
            return DataGridViewUtil.BuildDataGridViewCols(ClassType, UseDisplayName,HiddenAttributes);
        }
        /// <summary>
        /// 获取列
        /// </summary>
        /// <param name="hiddenAttributes"></param>
        /// <param name="useDisplayName"></param>
        /// <returns></returns>
        public virtual DataGridViewColumn[] GetDataGridViewColumns(List<string> hiddenAttributes, bool useDisplayName = true, string [] orderedColNames = null)
        {
            return DataGridViewUtil.BuildDataGridViewCols(ClassType, useDisplayName, hiddenAttributes, orderedColNames);
           
        }
        #endregion 
    } 
}
