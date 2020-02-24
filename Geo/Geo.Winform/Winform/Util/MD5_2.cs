using System;
using System.Collections.Generic; 
using System.Text; 

namespace  Geo.Winform
{
    public class MD5_2
    {
        // 格式化md5 hash 字节数组所用的格式（两位小写16进制数字）
        private static readonly string m_strHexFormat = "x2";
        private MD5_2() { }
        /// <summary>  
        /// 使用当前缺省的字符编码对字符串进行加密。 189 0819 6180 廖
        /// </summary>
        /// <param name="str">需要进行md5演算的字符串</param>
        /// <returns>用小写字母表示的32位16进制数字字符串</returns>
        public static string md5(string str)
        {
            return (string)md5(str, false, Encoding.UTF8);
        }
        /// <summary>
        /// 对字符串进行md5 hash计算
        /// </summary>
        /// <param name="str">需要进行md5演算的字符串</param>
        /// <param name="raw_output">
        /// false则返回经过格式化的加密字符串(等同于 string md5(string) )
        /// true则返回原始的md5 hash 长度16 的 byte[] 数组
        /// </param>
        /// <returns>
        /// byte[] 数组或者经过格式化的 string 字符串
        /// </returns>
        public static object md5(string str, bool raw_output)
        {
            return md5(str, raw_output, Encoding.UTF8);
        }
        /// <summary>
        /// 对字符串进行md5 hash计算
        /// </summary>
        /// <param name="str">需要进行md5演算的字符串</param>
        /// <param name="raw_output">
        /// false则返回经过格式化的加密字符串(等同于 string md5(string) )
        /// true则返回原始的md5 hash 长度16 的 byte[] 数组
        /// </param>
        /// <param name="charEncoder">
        /// 用来指定对输入字符串进行编解码的 Encoding 类型，
        /// 当输入字符串中包含多字节文字（比如中文）的时候
        /// 必须保证进行匹配的 md5 hash 所使用的字符编码相同，
        /// 否则计算出来的 md5 将不匹配。
        /// </param>
        /// <returns>
        /// byte[] 数组或者经过格式化的 string 字符串
        /// </returns>
        public static object md5(string str, bool raw_output, 
                                                    Encoding charEncoder)
        {
            if (!raw_output)
                return md5str(str, charEncoder);
            else
                return md5raw(str, charEncoder);
        }

        /// <summary>
        /// 使用当前缺省的字符编码对字符串进行加密
        /// </summary>
        /// <param name="str">需要进行md5演算的字符串</param>
        /// <returns>用小写字母表示的32位16进制数字字符串</returns>
        protected static string md5str(string str)
        {
            return md5str(str, Encoding.UTF8);
        }
        /// <summary>
        /// 对字符串进行md5加密
        /// </summary>
        /// <param name="str">需要进行md5演算的字符串</param>
        /// <param name="charEncoder">
        /// 指定对输入字符串进行编解码的 Encoding 类型
        /// </param>
        /// <returns>用小写字母表示的32位16进制数字字符串</returns>
        protected static string md5str(string str, Encoding charEncoder)
        {
            byte[] bytesOfStr = md5raw(str, charEncoder);
            int bLen = bytesOfStr.Length;
            StringBuilder pwdBuilder = new StringBuilder(32);
            for (int i = 0; i < bLen; i++ )
            {
                pwdBuilder.Append(bytesOfStr[i].ToString(m_strHexFormat));
            }
            return pwdBuilder.ToString();
        }
        /// <summary>
        /// 使用当前缺省的字符编码对字符串进行加密
        /// </summary>
        /// <param name="str">需要进行md5演算的字符串</param>
        /// <returns>长度16 的 byte[] 数组</returns>
        protected static byte[] md5raw(string str)
        {
            return md5raw(str, Encoding.UTF8);
        }
        /// <summary>
        /// 对字符串进行md5加密
        /// </summary>
        /// <param name="str">需要进行md5演算的字符串</param>
        /// <param name="charEncoder">
        /// 指定对输入字符串进行编解码的 Encoding 类型
        /// </param>
        /// <returns>长度16 的 byte[] 数组</returns>
        protected static byte[] md5raw(string str, Encoding charEncoder)
        {
            System.Security.Cryptography.MD5 md5 = 
                System.Security.Cryptography.MD5.Create();
            return md5.ComputeHash(charEncoder.GetBytes(str));
        }
    }
}
