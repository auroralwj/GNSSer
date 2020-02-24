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
    /// Gnsser 定义的参数，写在观测文件Comment里面。
    /// </summary>
    public class WidthFixedParamLineBuilder
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public WidthFixedParamLineBuilder()
        {
            Params = new Dictionary<string, string>();
            MaxLineWidth = 60;
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        public WidthFixedParamLineBuilder(Dictionary<string, string> paramDic)
        {
            Params = paramDic;
            MaxLineWidth = 60;
        }
        #region 基本属性
        /// <summary>
        /// 参数对名称和值
        /// </summary>
        public Dictionary<string, string> Params { get; set; }
        /// <summary>
        /// 内容行最大长度
        /// </summary>
        static public int MaxLineWidth { get; set; }
        /// <summary>
        /// 起始标记
        /// </summary>
        public static string StartMaker = "GNSSER_START:";
        /// <summary>
        /// 结束标记
        /// </summary>
        public static string EndMaker = "GNSSER_END:";

        #endregion

        /// <summary>
        /// 构建成指定宽度以换行符分开的字符串。
        /// </summary>
        /// <returns></returns>
        public string ToMarkedLineString()
        {
            var paramString = BuildParamString();

            return ToMarkedLineString(paramString);
        }
        /// <summary>
        /// 转换为参数字符串
        /// </summary>
        /// <returns></returns>
        public string BuildParamString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var item in Params)
            {
                var str = item.Key + "=" + item.Value;
                sb.Append(str);
                sb.Append(";");
            }
            var paramString = sb.ToString();
            return paramString;
        }
        /// <summary>
        /// 转换成以指定宽度的字符串列表
        /// </summary>
        /// <returns></returns>
        public List<string> ToMarkedLines()
        {
            return ToMarkedLines(BuildParamString());
        }

        #region 写成注释
        /// <summary>
        /// 将结果更新到RINEX头文件中，将覆盖以往的参数。
        /// </summary>
        /// <param name="existOFilePath"></param>
        /// <param name="newOfilePath"></param>
        public void UpdateToRinexOFileHeader(string existOFilePath, string newOfilePath)
        {
            RinexObsFileReader reader = new RinexObsFileReader(existOFilePath, false);
            var header = reader.GetHeader();
            var remainedComments = FilterParamComments(header.Comments);
            remainedComments.AddRange(this.ToMarkedLines());
            header.Comments = remainedComments;

            RinexObsFileReplacer replacer = new RinexObsFileReplacer(existOFilePath);
            replacer.ReplaceHeader(header, newOfilePath);
        }

        /// <summary>
        /// 为不超过指定字符数（60）的行的组成，一个整体的字符串，行采用换行符分隔。
        /// </summary>
        /// <param name="paramStringNoMarker"></param>
        /// <returns></returns>
        public static string ToMarkedLineString(string paramStringNoMarker)
        {
            List<string> lines = ToMarkedLines(paramStringNoMarker);
            return StringUtil.ToLineString(lines);
        }

        /// <summary>
        /// 转换成RINEX Comments 
        /// </summary>
        /// <param name="paramStringNoMarker"></param>
        /// <returns></returns>
        public static List<string> ToMarkedLines(string paramStringNoMarker)
        {
            var line = StartMaker + paramStringNoMarker;
            var lines = WideFiexedLineBuilder.GetWideFixedLines(line, MaxLineWidth);
            var lastLine = lines[lines.Count - 1];
            if (lines.Count > 1 && lastLine.Length + EndMaker.Length < MaxLineWidth)
            {
                lines[lines.Count - 1] = EndMaker + lastLine;
            }
            else
            {
                lines.Add(EndMaker);
            }

            return lines;
        }

        /// <summary>
        /// 过滤掉GNSSer参数。
        /// </summary>
        /// <param name="comments"></param>
        /// <returns></returns>
        public List<string> FilterParamComments(List<string> comments)
        {
            List<string> filteredComments = new List<string>();
            bool isStarted = false;
            bool isEnd = false;
            foreach (var comment in comments)
            {
                if (String.IsNullOrWhiteSpace(comment)) { continue; }

                var str = Geo.Utils.StringUtil.SubString(comment, 0, MaxLineWidth);

                if (comment.StartsWith(StartMaker)) { isStarted = true; str = str.Substring(StartMaker.Length); }
                if (comment.StartsWith(EndMaker)) { isEnd = true; str = str.Substring(EndMaker.Length); }
                if (isStarted && !isEnd)
                {
                    continue;
                }
                filteredComments.Add(comment);
            }
            return filteredComments;
        } 

        #endregion 
    } 

}
