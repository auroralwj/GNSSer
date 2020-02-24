//2016.08.19, czs, create in 江西上饶火车站, 数组工具


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text; 

namespace Geo.Utils
{
    /// <summary>
    /// 数组工具类。
    /// </summary>
    public class ArrayUtil
    {


        /// <summary>
        /// 检查并重置行数量
        /// </summary>
        public static bool CheckAndResizeArrayTo(ref string[] inputArray, int MaxAppendLineCount = 10000)
        {
            if (inputArray.Length > MaxAppendLineCount)
            {
                var newLinesCount = MaxAppendLineCount / 2;
                int startIndexToRemain = inputArray.Length - newLinesCount;
                Array.Copy(inputArray, startIndexToRemain, inputArray, 0, newLinesCount);
                Array.Resize(ref inputArray, newLinesCount);
                return false;
            }
            return true;
        }

        /// <summary>
        /// 返回字符串以指定的符号间隔
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <param name="spliter"></param>
        /// <returns></returns>
        public static string ToString<T>(IEnumerable<T> array, string spliter = ", ")
        {
            StringBuilder sb = new StringBuilder();
            foreach (var item in array)
            {
                sb.Append(item);
                sb.Append(spliter);
            }

            return sb.ToString();

        }
        

        /// <summary>
        /// 返回子数组，如果不满足要求，则返回空数组。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <param name="fromIndex"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        static public List<T> GetSubArray<T>(IEnumerable<T> array, int fromIndex, int len)
        { 
            List<T> list = new List<T>();
            int i = 0;
            int endIndexNotInclude = fromIndex + len;
            foreach (var item in array)
            {
                if (i >= fromIndex && i < endIndexNotInclude) { list.Add(item); }

                if (i > endIndexNotInclude) break;
                i++;
            }
            return list;
        }
        /// <summary>
        /// 返回尾部子数组，如果不足，则返回原数组。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        static public List<T> GetEnds<T>(IEnumerable<T> array, int len)
        {
            int totalLen = array.Count();
            if (len >= totalLen) { return new List<T>( array); }

            int fromIndex = totalLen - len; 
            
            return GetSubArray<T>(array, fromIndex, len);
        }

        /// <summary>
        /// 按照老顺序排序
        /// </summary>
        /// <param name="names"></param>
        /// <param name="oldOrders"></param>
        /// <returns></returns>
        public static List<string> GetNamesInOldOrders(List<string> names, string[] oldOrders)
        {
            List<string> list = new List<string>();
            if (oldOrders != null && oldOrders.Length > 0)
            {
                foreach (var item in oldOrders)
                {
                    if (names.Contains(item)) { list.Add(item); }
                }
            }

            foreach (var item in names)
            {
                if (!list.Contains(item)) list.Add(item);
            }
            return list;
        }

    }
}
