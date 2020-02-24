//2014.10.18, czs, edit in beijing, 抽离 ClockBias，为钟差而设计


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Gnsser.Times;

namespace Gnsser.Data.Rinex
{
    /// <summary>
    /// 原子钟载体类型（接收机或卫星）。
    /// </summary>
    public enum ClockType { Receiver, Satellite }

    /// <summary>
    /// 原子钟钟差领域类，参照钟差文件模型设计。文件类型。
    /// </summary>
    public class AtomicClock : SatClockBias, IComparable<AtomicClock>
    {
        /// <summary>
        /// 钟的名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 钟的类型
        /// </summary>
        public ClockType ClockType { get; set; }
        /// <summary>
        /// 状态码
        /// </summary>
        public int StateCode { get; set; }

        #region override
        /// <summary>
        /// 比较
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public int CompareTo(AtomicClock other)
        {
            return (int)Time.CompareTo(other.Time);
        }
        /// <summary>
        /// 等于否
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            AtomicClock o = obj as AtomicClock;
            if (o == null) return false;

            return ClockType == o.ClockType
                && base.Equals(obj)
                ;
        }
        /// <summary>
        /// 哈希数
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return ClockType.GetHashCode() * 3
                + base.GetHashCode()
               ;
        }
        /// <summary>
        /// 字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Time + " " + ClockType + " " + Prn + " " + ClockBias + " " + ClockDrift;
        }
        #endregion


        /// <summary>
        /// 已制表位分割的标题
        /// </summary>
        /// <returns></returns>
        public override  string GetTabTitles()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("名称");
            sb.Append("\t");
            sb.Append("Time");
            sb.Append("\t");
            sb.Append("钟差");
            sb.Append("\t");
            sb.Append("钟漂");

            return sb.ToString();
        }

        /// <summary>
        /// 表行内容
        /// </summary>
        /// <returns></returns>
        public override string GetTabValues()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(this.Name);
            sb.Append("\t");
            sb.Append(Time);

            sb.Append("\t");
            if (ClockBias == 0) { sb.Append(" "); }
            else { sb.Append(ClockBias.ToString("g")); }

            sb.Append("\t");
            if (ClockDrift == 0) { sb.Append(" "); }
            else { sb.Append(ClockDrift.ToString("g")); }
             
            return sb.ToString();
        }
    }
}