//2016.11.28, czs, create in hongqing, 写 GNSSER 特有信息，写在 RINEX 的注释里面。

using System;
using Geo.Coordinates; 
using System.Collections.Generic;
using System.Text;
using System.IO; 
using Geo.Utils;
using Gnsser.Times;
using Geo.Times;
using Geo;
using Gnsser.Data.Rinex;

namespace Gnsser
{ 
    /// <summary>
    /// 宽度固定的参数读取。
    /// </summary>
    public class WidthFixedParamReader
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="width"></param>
        public WidthFixedParamReader(int width = 60)
        {
            MaxLineWidth = width;
        }
        /// <summary>
        /// 内容行最大长度
        /// </summary>
        public int MaxLineWidth { get; set; }
        /// <summary>
        /// 起始标记
        /// </summary>
        public static string StartMaker = "GNSSER_START:";
        /// <summary>
        /// 结束标记
        /// </summary>
        public static string EndMaker = "GNSSER_END:";

        #region 读取，解析

        /// <summary>
        /// 解析，从观测文件。
        /// </summary>
        /// <param name="oFilePath"></param>
        /// <returns></returns>
        public Dictionary<string, string> ParseFromRinexOFile(string oFilePath)
        {
            RinexObsFileReader reader = new RinexObsFileReader(oFilePath, false);
            var header = reader.GetHeader();
            return GetParamFromHeader(header);
        }
        /// <summary>
        /// 从头部对象中提取参数
        /// </summary> 
        /// <param name="header"></param>
        /// <returns></returns>
        public Dictionary<string, string> GetParamFromHeader(RinexObsFileHeader header)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            var str = ExtractParamStringFromRinexComments(header.Comments);
            var paires = str.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var paramPair in paires)
            {
                if (String.IsNullOrWhiteSpace(paramPair)) { continue; }

                var strs = paramPair.Split('=');
                data.Add(strs[0], strs[1]);
            }
            return data;
        }
        /// <summary>
        /// 从RINEX注释行中，提取变量字符串。不包含起始和结束标记 Marker。
        /// </summary>
        /// <param name="comments"></param>
        /// <returns></returns>
        private  string ExtractParamStringFromRinexComments(IEnumerable<string> comments)
        {
            bool isStarted = false;
            bool isEnd = false;
            StringBuilder sb = new StringBuilder();
            foreach (var comment in comments)
            {
                if (String.IsNullOrWhiteSpace(comment)) { continue; }

                var str = Geo.Utils.StringUtil.SubString(comment, 0, MaxLineWidth);

                if (comment.StartsWith(StartMaker)) { isStarted = true; str = str.Substring(StartMaker.Length); }
                if (comment.StartsWith(EndMaker)) { isEnd = true; str = str.Substring(EndMaker.Length); }
                if (isStarted)
                {
                    sb.Append(str);
                }
                if (isEnd)
                {
                    break;
                }
            }
            var totalParamString = sb.ToString();
            return totalParamString;
        }

        #endregion

      


    }


}
