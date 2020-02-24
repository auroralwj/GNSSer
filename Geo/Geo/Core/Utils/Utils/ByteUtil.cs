//2015.03.23， czs, create in namu, 提供一些字节遍历工具
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Geo.Utils
{
    /// <summary>
    /// 提供一些字节遍历工具
    /// </summary>
    public class ByteUtil
    {


        /// <summary>
        /// 通过移位的方法获取前几个字节代表的数字。
        /// </summary>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public int GetFirstBits(List<byte> data, int count)
        {
            int byteCount = count / 8;
            int byteInnerCount = count % 8;

            int result = 0;
            for (int i = 0; i < byteCount; i++)
            {
                var leftShiftCount = (byteCount - 1 - i) * 8 + byteInnerCount;
                result += GetLeftShiftedByte(data[i], leftShiftCount);
            }

            //字节内部
            var bt = data[byteCount];
            int small = GetLeftBits(bt, byteInnerCount);

            result = result + small;
            return result;
        }

        /// <summary>
        /// 从高位截取字节。
        /// </summary>
        /// <param name="count"></param>
        /// <param name="bt"></param>
        /// <returns></returns>
        public static int GetLeftBits(byte bt, int count)
        {
            string str = BitConvertUtil.GetBinString(bt);

            str = str.Substring(0, count);
            int small = Convert.ToInt32(str, 2);
            return small;
        }

        /// <summary>
        /// 获取左移后的二进制。
        /// </summary>
        /// <param name="bt"></param>
        /// <param name="leftShiftCount"></param>
        /// <returns></returns>
        public static int GetLeftShiftedByte(byte bt, int leftShiftCount)
        {
            return bt << leftShiftCount;
        }
        /// <summary>
        ///  返回人们能读懂的文件大小的字符串。如 “5MB”
        /// </summary>
        /// <param name="Length">文件大小（字节）</param>
        /// <returns></returns>
        public static string GetReadableFileSize(double Length)
        {
            if (Length < 1024)
            {
                return Length + " B";
            }
            else if (Length < 1024 * 1024)
            {
                return (Length / 1024.0).ToString("##.00") + " KB";
            }
            else if (Length < 1024 * 1024 * 1024)
            {
                return (Length / 1024.0 / 1024).ToString("#.00") + " MB";
            }
            else if (Length < 1024 * 1024 * 1024 * 1024L)
            {
                return (Length / 1024.0 / 1024 / 1024).ToString("#.00") + " GB";
            }
            return Length + " B";
        }

    }
}
