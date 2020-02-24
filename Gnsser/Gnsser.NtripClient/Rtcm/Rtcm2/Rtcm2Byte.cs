using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gnsser.Ntrip.Rtcm
{
    /// <summary>
    /// 一个 RTCM 字节，共 30 位。
    /// </summary>
    public class Rtcm2Byte
    {
        public Rtcm2Byte(string charString = "")
        {
            this.CharString = charString;
        }

        /// <summary>
        /// 30 比特字组成
        /// </summary>
        public int Length = 30; 


        /// <summary>
        /// 字节字符串
        /// </summary>
        public string CharString{get;set;}

        /// <summary>
        /// 利用上一个字节的末尾两个数字进行校验。
        /// </summary>
        /// <param name="bit29"></param>
        /// <param name="bit30"></param>
        /// <returns></returns>
        internal bool Check(bool bit29, bool bit30)
        {
            List<bool> list = GetBools(this.CharString);
            return Check(bit29, bit30, list);
        }

        /// <summary>
        /// 奇偶校验 6 bit
        /// </summary>
        public string SixCheckBits { get { return CharString.Substring(24,6); } }

        public int CheckBit { get; set; }

        #region 校验相关 2.3 版本？？
        static List<int> D25Indexes = new List<int>()
        {
            1,2,3,5,6,10,11,12,13,14,17,18,20,23
        };
        static List<int> D26Indexes = new List<int>()
        {
            2,3,4,6,7,11,12,13,14,15,18,19,21,24
        };
        static List<int> D27Indexes = new List<int>()
        {
            1,3,4,5,7,8,12,13,14,15,16,19,20,22
        };
        static List<int> D28Indexes = new List<int>()
        {
           2,4,5,6,8,9,13,14,15,16,17,20,21,23
        };
        static List<int> D29Indexes = new List<int>()
        {
           1,3,5,6,7,9,10,14,15,16,17,18,21,22,24
        };

        static List<int> D30Indexes = new List<int>()
        {
            3,5,6,8,9,10,11,13,19,22,23,24
        };
        /// <summary>
        /// 按照指定编码算法进行检核
        /// </summary>
        /// <param name="bit29"></param>
        /// <param name="bit30"></param>
        /// <param name="colName"></param>
        /// <returns></returns>
        public static bool Check(bool bit29, bool bit30, List<bool> list)
        {
//            􀝃􀯜gi = 1 for i = 0, 1, 3, 4, 5, 6, 7, 10, 11, 14, 17, 18, 23, 24
//= 0 otherwise
//This obsCodeode is called CRC-24Q (Q for Qualcomm Corporation




            //d25
            bool d25 = bit29;
            foreach (var item in D25Indexes)
            {
                d25 = d25 & list[item - 1];
            }
            if (d25 != list[24])
                return false;
            //d26
            bool d26 = bit30;
            foreach (var item in D26Indexes)
            {
                d26 = d26 & list[item - 1];
            }
            if (d26 != list[25])
                return false;
            //d27
            bool d27 = bit29;
            foreach (var item in D27Indexes)
            {
                d27 = d27 & list[item - 1];
            }
            if (d27 != list[26])
                return false;
            //d28
            bool d28 = bit30;
            foreach (var item in D28Indexes)
            {
                d28 = d28 & list[item - 1];
            }
            if (d28 != list[27])
                return false;
            //d29
            bool d29 = bit30;
            foreach (var item in D29Indexes)
            {
                d29 = d29 & list[item - 1];
            }
            if (d29 != list[28])
                return false;
            //d30
            bool d30 = bit29;
            foreach (var item in D30Indexes)
            {
                d30 = d30 & list[item - 1];
            }
            if (d30 != list[29])
                return false;
            return true;
        }

        /// <summary>
        /// 返回布尔值表示的二进制列表，编号从 0 开始。
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public List<bool> GetBools(string line)
        {
            List<bool> bools = new List<bool>();
            foreach (var item in line)
            {
                var bb = int.Parse(item.ToString());
                // var bb = (prn + "") == "1";
                bools.Add(bb == 1);
            }
            return bools;
        }
        #endregion
    }
   
}
