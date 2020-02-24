//2015.04.27， czs, create in namu,  List 功能扩展
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Geo.Helper
{
    /// <summary>
    /// List 功能扩展
    /// </summary>
    public static  class ListHelper
    {
        /// <summary>
        /// 序列化显示
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="colName"></param>
        /// <returns></returns>
        public static string ToString<T>(this List<T> list)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("[");
            int i = 0;
            foreach (var item in list)
            {
                if (i != 0) sb.Append(",");
                sb.Append(item.ToString());
                i++;
            }
            sb.Append("]");
            return sb.ToString();
        }
        
    }
}
