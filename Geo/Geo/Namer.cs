//2019.03.02, czs, create in hongqing mengxiangshu, 命名器

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geo
{
    /// <summary>
    /// 命名器
    /// </summary>
    public class Namer
    {
        /// <summary>
        /// 获取第一部分
        /// </summary>
        /// <param name="name"></param>
        /// <param name="splitter"></param>
        /// <returns></returns>
        public static string GetFirstPart(string name, string splitter = "_")
        {
            var index = name.IndexOf(splitter);
            if (index == -1)
            {
                return name;
            }
            if (index == 0)
            {
                name = name.Substring(splitter.Length);
                return GetFirstPart(name);
            }
            return name.Substring(0, index);
        }
        /// <summary>
        /// 获取名称
        /// </summary>
        /// <param name="names"></param>
        /// <param name="maxLenOfEach"></param>
        /// <returns></returns>
        public static string GetName(IEnumerable<string> names, int maxLenOfEach = 8)
        {
            StringBuilder sb = new StringBuilder();
            int i = 0;
            foreach (var item in names)
            {
                if (i > 0) { sb.Append("_"); }
                if(i > 3)
                {
                    sb.Append("etc" + names.Count() + "Count");
                    break;
                }
                var name = Geo.Utils.StringUtil.SubString(item, 0, maxLenOfEach).Trim();
                sb.Append(name);
                i++;
            }
            return sb.ToString();
        }
    }
}
