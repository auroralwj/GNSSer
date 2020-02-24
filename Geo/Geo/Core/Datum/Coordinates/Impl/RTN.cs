//2018.10.21, czs, create in hmx, 轨道坐标RTN

using System;
using System.Collections.Generic;

using System.Text;
using Geo.Utils;

namespace Geo.Coordinates
{
    /// <summary>
    /// 轨道坐标RTN
    /// R 径向
    /// T 迹向
    /// N 法向
    /// </summary>
    public class RTN : IEquatable<RTN>
    {
        /// <summary>
        /// 北东天坐标。卫星坐标，局部坐标，测站坐标。
        /// </summary>
        public RTN()
        { }
        /// <summary>
        /// 北东天坐标。卫星坐标，局部坐标，测站坐标。
        /// </summary>
        /// <param name="n"></param>
        /// <param name="r"></param>
        /// <param name="t"></param>
        public RTN(double r, double t, double n) : this() { this.N = n; this.R = r; this.T = t; }

        /// <summary>
        /// 径向
        /// </summary>
        public double R { get; set; }
        /// <summary>
        /// 迹向
        /// </summary>
        public double T { get; set; }
        /// <summary>
        /// 法向
        /// </summary>
        public double N { get; set; }

        public static RTN operator +(RTN a, RTN b) { return new RTN(a.R + b.R, a.T + b.T,a.N + b.N ); }
        public static RTN operator -(RTN a, RTN b) { return new RTN(a.R + b.R, a.T - b.T, a.N - b.N); }
        /// <summary>
        /// 相等否
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            RTN g = obj as RTN;
            if (g == null) return false;

            return Equals(g);
        }
        /// <summary>
        /// 相等否
        /// </summary>
        /// <param name="g"></param>
        /// <returns></returns>
        public  bool  Equals(RTN g)
        {
            return
              N == g.N
                && R == g.R
                && T == g.T;
        }
        /// <summary>
        /// 哈希数
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return (int)(N) * 13 + (int)(R) + 17 + (int)(T) * 13;
        }

        /// <summary>
        /// 字符串显示
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "(" +R.ToString("0.000000") + "," + T.ToString("0.000000") + "," + N.ToString("0.000000") + ")";
        }

        /// <summare>
        /// 欧式距离。
        /// </summare>
        /// <param name="neu"></param>
        /// <returns></returns>
        public double Distance(RTN neu)
        {
            return Math.Sqrt((N - neu.N) * (N - neu.N) + (R - neu.R) * (R - neu.R) + (T - neu.T) * (T - neu.T));
        }
        /// <summary>
        /// 方向余弦
        /// </summary>
        public double CosN { get { return N / this.Length; } }
        /// <summary>
        /// 方向余弦
        /// </summary>
        public double CosR { get { return R / this.Length; } }
        /// <summary>
        /// 方向余弦
        /// </summary>
        public double CosT { get { return T / this.Length; } }
        /// <summary>
        /// 距离原点的长度
        /// </summary>
        public double Length { get { return Math.Sqrt((N) * (N) + (R) * (R) + (T) * (T)); } }

    }
}
