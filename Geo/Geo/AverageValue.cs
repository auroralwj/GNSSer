//2018.07.07, czs, create in HMX, 平均数

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Geo.Algorithm;
using Geo.Common;

namespace Geo
{
    /// <summary>
    /// 平均数
    /// </summary>
    public class AverageValue :  RmsedNumeral
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="data"></param>
        public AverageValue(double[] data):this(data[0],data[1],(int)data[2])
        { 
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="val"></param>
        /// <param name="rms"></param>
        /// <param name="count"></param>
        public AverageValue(double val, double rms, int count)
            :base(val, rms)
        {
            this.Count = count;
        }
        /// <summary>
        /// 求平均的数量
        /// </summary>
        public int Count { get; set; }

        public override string ToString()
        {
            return ToString("G");
        }
        /// <summary>
        /// 字符串
        /// </summary>
        /// <param name="format"></param>
        /// <returns></returns>
        public override string ToString(string format)
        {
            return Value.ToString(format) + "(" + Rms.ToString(format) + ")" + "Count:" +  Count.ToString();
        }
    }
}
