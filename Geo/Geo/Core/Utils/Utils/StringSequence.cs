//2015.02.07, czs, create in pengzhou, 字符串转运站

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geo.Utils
{ 
    /// <summary>
    /// 字符串转运站,是一个队列数据结构。
    /// </summary>
    public class StringSequence
    {
        /// <summary>
        /// 构造函数。
        /// </summary>
        public StringSequence()
        {
            this.Builder = new StringBuilder();
        }
        /// <summary>
        /// 构造函数。
        /// </summary>
        public StringSequence(string str)
        {
            this.Builder = new StringBuilder(str);
        }  

        /// <summary>
        /// 库存容量，超出则出货。
        /// </summary>
        public int Capacity { get { return Builder.Length; } }
        /// <summary>
        /// 数据存储。
        /// </summary>
       private StringBuilder Builder { get;  set; }
        /// <summary>
        /// 插入队列
        /// </summary>
        /// <param name="index"></param>
        /// <param name="str"></param>
        public void Insert(int index, string str)
        {
            Builder.Insert(index, str);
        }

        /// <summary>
        /// 入队,接收数据。
        /// </summary>
        /// <param name="str"></param>
        public void EnQuence(string str)
        {
            this.Builder.Append(str); 
        }

        /// <summary>
        ///出队， 将先入的推出来。
        /// </summary>
        public string DeQueue(int count = Int32.MaxValue)
        {
            int len = Math.Min(Builder.Length, count);

            string str = Builder.ToString(0, len).ToString();
            Builder.Remove(0, len);
            return str;
        }
        /// <summary>
        ///出队，并不删除。
        /// </summary>
        public string GetQueue(int count = Int32.MaxValue)
        {
            int len = Math.Min(Builder.Length, count);

            string str = Builder.ToString(0, len).ToString();
            return str;
        }

        /// <summary>
        /// 根据输入的字节，获取本次将处理的二进制字符串
        /// </summary>
        /// <param name="inBytes"></param>
        /// <returns></returns>
        private string GetBinaryString(byte[] inBytes)
        {
            //转换为2进制字符串
            List<string> btStrs = new List<string>();
            foreach (var bt in inBytes)
            {
                //如果不足8位则补充
                var bit = Convert.ToString(bt, 2);
                while (bit.Length < 8)
                {
                    bit = "0" + bit;
                }
                //反序
                bit = Reverse(bit);

                btStrs.Add(bit);
            }


            //组成一条整的字符串 
            StringBuilder sb = new StringBuilder();
            foreach (var item in btStrs)
            {
                sb.Append(item);
            }
            var binString = sb.ToString();
            return binString;
        }

        /// <summary>
        /// 字符串顺序调换，第一位和最后一位调换。相当于反向排序。
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns></returns>
        private static String Reverse(string str)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = str.Length - 1; i >= 0; i--)
            {
                sb.Append(str[i]);
            }
            return sb.ToString();
        }
         
        public void Clear()
        {
            this.Builder.Clear();
        }
    }
}
