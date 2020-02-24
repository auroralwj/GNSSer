using System;
using System.Collections.Generic;
using System.Text;

namespace Geo.Utils
{
    /// <summary>
    /// 转换
    /// </summary>
    public class EncodingConverter
    {
        /// <summary>
        /// UTF8 to GB2312.
        /// </summary>
        /// <param name="utf8str"></param>
        /// <returns></returns>
        public static string Utf8ToGB2312String(string utf8str)
        {
            byte[] utf8Bytes = Encoding.UTF8.GetBytes(utf8str);
            Encoding gb2312 = Encoding.GetEncoding("GB2312");
            byte[] gb2312Bytes = Encoding.Convert(Encoding.UTF8, gb2312, utf8Bytes);
            char[] newChar = new char[gb2312.GetCharCount(gb2312Bytes)];
            gb2312.GetChars(gb2312Bytes, 0, gb2312Bytes.Length, newChar, 0);
            string str = new string(newChar);
            return str;
        }
        /// <summary>
        /// 直接更改编码方式。.
        /// </summary>
        /// <param name="formStr"></param>
        /// <param name="fromEncoding"></param>
        /// <param name="toEncoding"></param>
        /// <returns></returns>
        public static string ChangeStringEncoding(string formStr, Encoding fromEncoding, Encoding toEncoding)
        {
            byte[] fromBytes = fromEncoding.GetBytes(formStr);
            return toEncoding.GetString(fromBytes);
        }

        /// <summary>
        /// 编码转换。
        /// </summary>
        /// <param name="formStr"></param>
        /// <param name="fromEncoding"></param>
        /// <param name="toEncoding"></param>
        /// <returns></returns>
        public static string ConvertStringEncoding(string formStr, Encoding fromEncoding, Encoding toEncoding)
        {
            byte[] fromBytes = fromEncoding.GetBytes(formStr);
            byte[] toBytes = Encoding.Convert(fromEncoding, toEncoding, fromBytes);
            char[] newChar = new char[toEncoding.GetCharCount(toBytes)];
            toEncoding.GetChars(toBytes, 0, toBytes.Length, newChar, 0);
            string str = new string(newChar);
            return str;
        }

        public static string Iso8859ToGb2312(string iso8859Str)
        {
            Encoding iso8859, gb2312;
            iso8859 = Encoding.GetEncoding("iso8859-1");
            gb2312 = Encoding.GetEncoding("Gb2312");

            byte[] iso8859Bytes = iso8859.GetBytes(iso8859Str);
            return gb2312.GetString(iso8859Bytes);
        }

    }
}
