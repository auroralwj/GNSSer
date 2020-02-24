using System;
using System.Collections.Generic;

using System.Text;
using Geo.Utils;

namespace Geo.Coordinates
{
    /// <summary>
    /// 站心坐标系.左手坐标系。或卫星坐标系。
    /// N 北方
    /// E 东方
    /// U 头顶
    /// </summary>
    public class UEN: IEquatable<UEN>// : Coordinate,
    {
        public double N { get; set; }
        public double E { get; set; }
        public double U { get; set; }
        /// <summary>
        /// 北东天坐标。卫星坐标，局部坐标，测站坐标。
        /// </summary>
        public UEN()
      //  :base(Referencing.CoordinateReferenceSystem.NeuCrs, 0, Referencing.CoordinateType.UEN)
        { }
        /// <summary>
        /// 北东天坐标。卫星坐标，局部坐标，测站坐标。
        /// </summary>
        /// <param name="n"></param>
        /// <param name="e"></param>
        /// <param name="u"></param>
        public UEN(double n, double e, double u) :this(){ this.N = n; this.E = e; this.U = u; }

        public static UEN operator +(UEN a, UEN b) { return new UEN(a.U + b.U, a.E + b.E,a.N + b.N ); }
        public static UEN operator -(UEN a, UEN b) { return new UEN(a.U + b.U , a.E - b.E, a.N - b.N); }
        
        public override bool Equals(object obj)
        {
            UEN g = obj as UEN;
            if (g == null) return false;

            return Equals(g);
        }

        public  bool  Equals(UEN g)
        {
            return
              N == g.N
                && E == g.E
                && U == g.U;
        }
        public override int GetHashCode()
        {
            return (int)(N) * 13 + (int)(E) + 17 + (int)(U) * 13;
        }


        public override string ToString()
        {
            return "(" + U.ToString("0.000000") + "," + E.ToString("0.000000") + "," + N.ToString("0.000000") + ")";
        }
        
        /// <summare>
        /// 欧式距离。
        /// </summare>
        /// <param name="info"></param>
        /// <returns></returns>
        public double Distance(UEN neu)
        {
            return Math.Sqrt((N - neu.N) * (N - neu.N) + (E - neu.E) * (E - neu.E) + (U - neu.U) * (U - neu.U));
        }
        public double CosN { get { return N / this.Length; } }
        public double CosE { get { return E / this.Length; } }
        public double CosU { get { return U / this.Length; } }
        public double Length { get { return Math.Sqrt((N) * (N) + (E) * (E) + (U) * (U)); } }
    }
}
