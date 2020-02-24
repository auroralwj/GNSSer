//2015.11.10, czs, create in k166 成都到西安列车， ListViewUtil 类

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Reflection;

namespace Geo.Utils
{
    /// <summary>
    /// 窗口工具类
    /// </summary>
    public static class ListViewUtil
    {

        /// <summary>
        /// 获取选中的。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ListView"></param>
        /// <returns></returns>
        static public List<T> GetSelectedObjects<T>(ListView ListView)
        {
            List<T> list = new List<T>();
            if (ListView == null || ListView.Items.Count == 0)
            {
                return list;
            }
            foreach (ListViewItem item in ListView.SelectedItems)
            {
                if (item.Tag is T)
                {
                    var o = (T)(item.Tag);
                    list.Add(o);
                }
            }

            return list;
        }

        /// <summary>
        /// 获取选中的。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ListView"></param>
        /// <returns></returns>
        static  public T GetSelectedObject<T>(ListView ListView)
        {
            if (ListView == null || ListView.Items.Count == 0)
            {
                return default(T);
            }
            foreach (ListViewItem item in ListView.SelectedItems)
            {
                if (item.Tag is T)
                {
                    var o = (T)(item.Tag);
                    return o;
                }
            }

            return default(T);
        }

    }
}
