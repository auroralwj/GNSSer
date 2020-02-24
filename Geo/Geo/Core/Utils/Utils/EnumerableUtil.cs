//2017.10.10, czs, create in hongqing, ���飬�ɱ�������

using System;
using System.Collections.Generic;
using System.Text;

namespace Geo.Utils
{
    /// <summary>
    /// ���飬�ɱ�������
    /// </summary>
    public class EnumerableUtil
    {
        /// <summary>
        /// �����б�
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        static public String ToString<T>(IEnumerable<T> list, string splliter = ",")
        {
            StringBuilder sb = new StringBuilder();
            int i = 0;
            foreach (var item in list)
            {
                if (i != 0) { sb.Append(splliter); }
                sb.Append(item); 
                i++;
            }
            return sb.ToString();
        }
         
    }
}
