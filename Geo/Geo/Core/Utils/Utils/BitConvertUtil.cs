//2016.01.23， czs & double, create in 洪庆, 提供二进制转换工具

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Geo.Utils
{
    /// <summary>
    /// 提供二进制转换工具
    /// </summary>
    public class BitConvertUtil
    {

        /// <summary>
        /// 获取字符字符串。
        /// </summary>
        /// <param name="sequence">序列</param>
        /// <param name="charCount">字符数量</param>
        /// <returns></returns>
        public static string GetCharString(StringSequence sequence, int charCount)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < charCount; i++)
            {
                var ch = GetChar(sequence.DeQueue(8));//uint8  
                sb.Append(ch);
            }
            var str = sb.ToString();
            return str;
        }
        /// <summary>
        /// 将二进制字符串转化为布尔型。
        /// </summary>
        /// <param name="binStr">二进制字符串,0,1</param>
        /// <returns></returns>
        public static bool GetBoolean(string binStr)
        {
            return binStr == "1";
            //var num = Convert.ToBoolean(binStr);
            //return num;
        }
        /// <summary>
        /// 将转换成连续的二进制字符串。
        /// </summary>
        /// <param name="data">字节数组</param>
        /// <returns></returns>
        public static string GetBinString(List<byte> data)
        {
            StringBuilder sb = new StringBuilder();

            foreach (var item in data)
            {
                string str = GetBinString(item);
                sb.Append(str);
            }

            return sb.ToString();
        }
        public static string GetBinString(byte bt)
        {
            string str = Convert.ToString(bt, 2);
            while (str.Length < 8)
            {
                str = "0" + str;
            }
            return str;
        }

        /// <summary>
        /// 将二进制字符串转化为整型。全部是正数
        /// </summary>
        /// <param name="binStr">二进制字符串</param>
        /// <returns></returns>
        public static uint GetUInt(string binStr)
        {
            if (binStr.Length > 32)
                throw new ArgumentException("GetUInt型不支持32位以上数据");
            var num = Convert.ToUInt32(binStr, 2);
            return num;
        }
        /// <summary>
        /// 将Int型二进制字符串转化为整型，以补码的形式存储   2016.01.22  double,czs  create in Hongqing
        /// </summary>
        /// <param name="binStr">二进制字符串</param>
        /// <returns></returns>
        public static int GetInt(string binStr)
        {
            if (binStr.Length > 32)
                throw new ArgumentException("GetInt型不支持32位以上数据");
            int num = 0;

            if (binStr.Length == 1)
            {
                num = int.Parse(binStr);
            }
            else if (binStr.Length == 16)
            {
                num = Convert.ToInt16(binStr, 2);
            }
            else if (binStr.Length == 32)
            {
                num = Convert.ToInt32(binStr, 2);
            }
            else
            {
                num = ConvertToInt32WithComplementalCode(binStr);
            }

            return num;
        }
        /// <summary>
        /// 二进制字符串转换为有符号的32位整型，采用补码编码。
        /// </summary>
        /// <param name="binStr"></param>
        /// <returns></returns>
        public static int ConvertToInt32WithComplementalCode(string binStr)
        {
            bool isPositive = binStr[0] == '0';
            var sub = binStr.Substring(1);
            if (!isPositive)
            {
                sub = Inverse(sub);
            }

            int num = Convert.ToInt32(sub, 2);

            if (!isPositive)
            {
                num = (num + 1) * (-1);
            }
            return num;
        }
        /// <summary>
        /// 二进制字符串转换为有符号的64位整型，采用补码编码。
        /// </summary>
        /// <param name="binStr"></param>
        /// <returns></returns>
        public static long ConvertToInt64WithComplementalCode(string binStr)
        {
            bool isPositive = binStr[0] == '0';
            var sub = binStr.Substring(1);
            if (!isPositive)
            {
                sub = Inverse(sub);
            }

            var num = Convert.ToInt64(sub, 2);

            if (!isPositive)
            {
                num = (num + 1) * (-1);
            }
            return num;
        }
        /// <summary>
        /// IntS型二进制，以反码的形式存储   2016.01.22  double  create in Hongqing
        /// </summary>
        /// <param name="binStr"></param>
        /// <returns></returns>
        public static int ConvertToInt32WithInverseCode(string binStr)
        {
            if (binStr.Length > 32)
                throw new ArgumentException("GetIntS型不支持32位以上数据");
            bool isPositive = binStr[0] == '0';
            var sub = binStr.Substring(1);
            if (!isPositive)
            {
                sub = Inverse(sub);
            }
            var num = Convert.ToInt32(sub, 2);
            if (!isPositive)
            {
                num *= -1;
            }

            return num;
        }
        /// <summary>
        /// IntS型二进制，以反码的形式存储   2016.01.22  double  create in Hongqing
        /// </summary>
        /// <param name="binStr"></param>
        /// <returns></returns>
        public static long ConvertToInt64WithInverseCode(string binStr)
        {
            if (binStr.Length > 64)
                throw new ArgumentException("GetIntS型不支持64位以上数据");
            bool isPositive = binStr[0] == '0';
            var sub = binStr.Substring(1);
            if (!isPositive)
            {
                sub = Inverse(sub);
            }
            long num;
            num = Convert.ToInt64(sub, 2);
            if (!isPositive)
            {
                num *= -1;
            }

            return num;
        }
        /// <summary>
        /// 二进制字符串取反。
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string Inverse(string str)
        {
            StringBuilder sb = new StringBuilder();

            foreach (var item in str)
            {
                if (item == '1') sb.Append('0');
                else sb.Append('1');
            }
            return sb.ToString();
        }

        /// <summary>
        /// 将二进制字符串转化为长整型
        /// </summary>
        /// <param name="binStr">二进制字符串</param>
        /// <returns></returns>
        public static long GetLong(string binStr)
        {
            var len = binStr.Length;
            if (len > 64)
                throw new ArgumentException("GetIntS型不支持64位以上数据");
            if (len <= 32)
            {
                return GetInt(binStr);
            }

            var num = ConvertToInt64WithComplementalCode(binStr);
            return num;
        }
        /// <summary>
        /// 将二进制字符串转化为单个字符。
        /// </summary>
        /// <param name="binStr">二进制字符串</param>
        /// <returns></returns>
        public static char GetChar(string binStr)
        {
            var bt = Convert.ToByte(binStr, 2);
            char num = Convert.ToChar(bt);
            return num;
        }


    }
}
