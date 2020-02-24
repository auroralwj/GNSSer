//2016.01.17, czs, create in hongqing,
//2016.02.07, czs, edit in hongqing, 修改，增加了节工具
//2017.06.30, czs, edit in hongqing, 增加 StripTagsCharArray 输入为空的判断

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Security.Cryptography;
using  System.Text.RegularExpressions;

namespace Geo.Utils
{
    /// <summary>
    /// Html 工具
    /// </summary>
    public class HtmlUtil
    {
        /// <summary>
        /// 清除标签
        /// </summary>
        /// <param name="html"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string ClearHtmlTag(string html, int length = 0)
        {
            string strText = System.Text.RegularExpressions.Regex.Replace(html, "<[^>]+>", "");
            strText = System.Text.RegularExpressions.Regex.Replace(strText, "&[^;]+;", "");
            strText = strText.Replace("<", "");
            strText = strText.Replace(">", ""); 

            if (length > 0 && strText.Length > length)
                return strText.Substring(0, length);

            return strText;
        }

         public static string RemoveHTMLTags(string htmlStream)
        {
            if (htmlStream == null)
            {
                throw new Exception("Your input html stream is null!");
                return null;
            }
            /*
             * 最好把所有的特殊HTML标记都找出来，然后把与其相对应的Unicode字符一起影射到Hash表内，最后一起都替换掉
             */
            //先单独测试,成功后,再把所有模式合并
            //注:这两个必须单独处理
            //去掉嵌套了HTML标记的JavaScript:(<script)[\\s\\S]*(</script>)
            //去掉css标记:(<style)[\\s\\S]*(</style>)
            //去掉css标记:\\..*\\{[\\s\\S]*\\}
            htmlStream = Regex.Replace(htmlStream, "(<script)[\\s\\S]*?(</script>)|(<style)[\\s\\S]*?(</style>)", " ", RegexOptions.IgnoreCase);
            //htmlStream = RemoveTag(htmlStream, "script");
            //htmlStream = RemoveTag(htmlStream, "style");
            //去掉普通HTML标记:<[^>]+>
            //替换空格:&nbsp;|&amp;|&shy;|&#160;|&#173;
            htmlStream = Regex.Replace(htmlStream, "<[^>]+>|&nbsp;|&amp;|&shy;|&#160;|&#173;|&bull;|&lt;|&gt;", " ", RegexOptions.IgnoreCase);
            //htmlStream = RemoveTag(htmlStream);
            //替换左尖括号
            //htmlStream = Regex.Replace(htmlStream, "&lt;", "<");
            //替换右尖括号
            //htmlStream = Regex.Replace(htmlStream, "&gt;", ">");
            //替换空行
            //htmlStream = Regex.Replace(htmlStream, "[\n|\r|\t]", " ");//[\n|\r][\t*| *]*[\n|\r]
            htmlStream = Regex.Replace(htmlStream, "(\r\n[\r|\n|\t| ]*\r\n)|(\n[\r|\n|\t| ]*\n)", "\r\n");

            htmlStream = Regex.Replace(htmlStream, "[\t| ]{1,}", " ");
            return htmlStream.Trim();
        }

         /// <summary>
         /// Remove HTML from string with Regex.
         /// </summary>
         public static string StripTagsRegex(string source)
         {
             return Regex.Replace(source, "<.*?>", string.Empty);
         }

         /// <summary>
         /// Compiled regular expression for performance.
         /// </summary>
         static Regex _htmlRegex = new Regex("<.*?>", RegexOptions.Compiled);

         /// <summary>
         /// Remove HTML from string with compiled Regex.
         /// </summary>
         public static string StripTagsRegexCompiled(string source)
         {
             return _htmlRegex.Replace(source, string.Empty);
         }

         /// <summary>
         /// Remove HTML tags from string using char array.
         /// </summary>
         public static string StripTagsCharArray(string source, int maxLen)
         {
             if (String.IsNullOrWhiteSpace(source)) { return ""; }

             char[] array = new char[source.Length];
             int arrayIndex = 0;
             bool inside = false;

             for (int i = 0; i < source.Length; i++)
             {
                 char let = source[i];
                 if (let == '<')
                 {
                     inside = true;
                     continue;
                 }
                 if (let == '>')
                 {
                     inside = false;
                     continue;
                 }
                 if (!inside)
                 {
                     array[arrayIndex] = let;
                     arrayIndex++;
                 }
                 if (arrayIndex >= maxLen) break;
             }
             return new string(array, 0, arrayIndex);
         }

 
    }
}
