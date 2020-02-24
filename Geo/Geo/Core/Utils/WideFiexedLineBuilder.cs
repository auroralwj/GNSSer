//2016.11.26, czs, create in hongqing ,宽度固定的文本

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Geo
{
    /// <summary>
    /// 宽度固定的文本
    /// </summary>
    public class WideFiexedLineBuilder : AbstractBuilder<List<String>, string>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="maxWidth"></param>
        public WideFiexedLineBuilder(int maxWidth)
        {
            this.MaxWidthCount = maxWidth;
        }
        /// <summary>
        /// 限宽字数
        /// </summary>
        public int MaxWidthCount { get; set; }

        public override List<string> Build(string material)
        {
            List<string> list = new List<string>();
            while (material.Length > 0)
            {
                var line = Geo.Utils.StringUtil.SubString(material, 0, MaxWidthCount);
                list.Add(line);

                material = Geo.Utils.StringUtil.SubString(material, MaxWidthCount);
            }

            return list;
        }
        /// <summary>
        /// 截断为宽度固定的文本
        /// </summary>
        /// <param name="str"></param>
        /// <param name="lineMaxWith"></param>
        /// <returns></returns>
        static  public List<string> GetWideFixedLines(string str, int lineMaxWith)
        {
            WideFiexedLineBuilder builder = new WideFiexedLineBuilder(lineMaxWith);
            return builder.Build(str);
        }
    }


}
