//2016.09.10, czs, create in hongqing, ENU 对应 XYZ

using System;
using System.Collections.Generic;
using System.Text;
using Geo.Utils;

namespace Geo.Coordinates
{
    /// <summary>
    /// 站心坐标系.左手坐标系。或卫星坐标系。地方坐标系。当地左手笛卡尔坐标系
    /// N 北方
    /// E 东方
    /// U 头顶
    /// </summary>
    public class ENU //: Coordinate, IEquatable<ENU>, IToTabRow
    {
        /// <summary>
        /// 北东天坐标。卫星坐标，局部坐标，测站坐标。当地左手笛卡尔坐标系
        /// </summary>
        public ENU()
        //:base(Referencing.CoordinateReferenceSystem.EnuCrs, 0, Referencing.CoordinateType.ENU)
        { }

        /// <summary>
        /// 北东天坐标
        /// </summary>
        /// <param name="hen"></param>
        public ENU(HEN hen)
            : this(hen.E, hen.N, hen.H)
        {

        }
        /// <summary>
        /// 北东天坐标
        /// </summary>
        /// <param name="neu"></param>
        public ENU(NEU neu)
            : this(neu.E, neu.N, neu.U)
        {

        }

        /// <summary>
        /// 北东天坐标。卫星坐标，局部坐标，测站坐标。当地左手笛卡尔坐标系
        /// </summary>
        /// <param name="n">北方向，X</param>
        /// <param name="e">东方向，Y</param>
        /// <param name="u">向上，Z</param>
        public ENU( double e,double n, double u) :this(){ this.N = n; this.E = e; this.U = u; }
        /// <summary>
        /// 北方向
        /// </summary>
        public double N { get; set; }
        /// <summary>
        /// 东方向
        /// </summary>
        public double E { get; set; }
        /// <summary>
        /// 上方向
        /// </summary>
        public double U { get; set; }
        /// <summary>
        /// 水平方向的长度
        /// </summary>
        public double LevelLength { get => Math.Sqrt(E*E + N * N); }

        public static ENU operator +(ENU a, ENU b) { return new ENU(a.E + b.E,a.N + b.N,  a.U + b.U); }
        public static ENU operator -(ENU a, ENU b) { return new ENU(a.E - b.E, a.N - b.N, a.U - b.U); }        
        public static ENU operator -(ENU ENU) { return new ENU( -ENU.E, -ENU.N,-ENU.U); }

        public override bool Equals(object obj)
        {
            ENU g = obj as ENU;
            if (g == null) return false;

            return Equals(g);
        }

        public  bool Equals(ENU g)
        {
            return
              N == g.N
                && E == g.E
                && U == g.U;
        }
        public override int GetHashCode()
        {
            return (int)(N) * 13 + (int)(E) + 17 + (int)(U) * 3;
        }

         
        /// <summary>
        /// 0.0780 0.0000 0.0000
        /// 1X,F6.4
        /// </summary>
        /// <returns></returns>
        public string ToRnxString()
        {
            return U.ToString("0.0000").Replace("-0.", "-.") + " " + N.ToString("0.0000").Replace("-0.", "-.") + " " + E.ToString("0.0000").Replace("-0.", "-.");
        }
        /// <summary>
        /// 8.2 = 12345.78
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public string ToRnxString(double p)
        {
            int interger = (int)p;
            int fractional = (int)((p - interger) * 10);
            string formate = "0.0";
            while (formate.Length < fractional + 2) formate += "0";

            return StringUtil.FillSpaceLeft(U.ToString(formate).Replace("-0.", "-."), interger)
                + " " + StringUtil.FillSpaceLeft(N.ToString(formate).Replace("-0.", "-."), interger)
                + " " + StringUtil.FillSpaceLeft(E.ToString(formate).Replace("-0.", "-."), interger);
        }


        /// <summare>
        /// 欧式距离。
        /// </summare>
        /// <param name="info"></param>
        /// <returns></returns>
        public double Distance(ENU ENU)
        {
            return Math.Sqrt( (E - ENU.E) * (E - ENU.E) + (N - ENU.N) * (N - ENU.N) +(U - ENU.U) * (U - ENU.U));
        }
        public double CosN { get { return N / this.Length; } }
        public double CosE { get { return E / this.Length; } }
        public double CosU { get { return U / this.Length; } }
        public double Length { get { return Math.Sqrt((E) * (E) +(N) * (N) +  (U) * (U)); } }

        /// <summare>
        /// 按权拟合。
        /// </summare>
        /// <param name="ENUA"></param>
        /// <param name="weightA"></param>
        /// <param name="ENUB"></param>
        /// <param name="weightB"></param>
        /// <returns></returns>
        public static ENU GetENU(ENU ENUA, double weightA, ENU ENUB, double weightB)
        {
            return new ENU()
            {
                E = (ENUA.E * weightA + ENUB.E * weightB) / (weightA + weightB),
                N = (ENUA.N * weightA + ENUB.N * weightB) / (weightA + weightB),
                U = (ENUA.U * weightA + ENUB.U * weightB) / (weightA + weightB)
            };
        }

        /// <summary>
        /// T是单位向量
        /// </summary>
        /// <param name="T"></param>
        /// <returns></returns>
        public double Dot(XYZ T)
        {
            //
            double a = E * T.X + N * T.Y + U * T.Z;
            return a;
        }

        /// <summary>
        /// T是单位向量
        /// </summary>
        /// <param name="T"></param>
        /// <returns></returns>
        public double Dot(ENU T)
        {
            //
            double a =  E * T.E + N * T.N +U * T.U;
            return a;
        }

        public ENU UnitENUVector()
        {
            double mag = this.Length;
            if (mag <= Math.Pow(10, -14))
            {
                throw new Exception("Divide by Zero Error");
            }
            ENU retArg = new ENU(this.E / mag, this.N/ mag, this.U / mag);
            return retArg;
        }
        /// <summary>
        /// 0 0 0
        /// </summary>
        public new static ENU Zero
        {
            get
            {
                return new ENU(0,0,0);
            }
        }
        /// <summary>
        /// 解析字符串，可以解析空格、分号、换行符、回车符、Tab为分隔符的字符串
        /// </summary>
        /// <param name="toString"></param>
        /// <returns></returns>
        public static new ENU Parse(string toString) { return Parse(toString, new char[] { ',', ';', ' ', '\t', '\n', '\r' }); }

        /// <summary>
        /// (x,y) (x,y,z) (x y z) x y z
        /// </summary>
        /// <param name="toString"></param>
        /// <param name="separaters"></param>
        /// <returns></returns>
        public static ENU Parse(string toString, char[] separaters)
        {
            toString = toString.Replace("(", "").Replace(")", "");
            string[] strs = toString.Split(separaters, StringSplitOptions.RemoveEmptyEntries);
            return new ENU(Double.Parse(strs[0]), Double.Parse(strs[1]), Double.Parse(strs[2]));
        }

        /// <summary>
        /// 从一维数组中解析。
        /// </summary>
        /// <param name="result">计算结果</param>
        /// <returns></returns>
        public static ENU Parse(double[] result)
        {
            return new ENU(result[0], result[1], result[2]);
        }
        /// <summary>
        /// 转为一维数组
        /// </summary>
        public double[] Array { get { return new Double[] { E,N,  U }; } }

        //public override string GetTabTitles()
        //{
        //   return "E\tN\tU";
        //}

        //public override string GetTabValues()
        //{
        //    StringBuilder sb = new StringBuilder();
        //    sb.Append(String.Format(FormatProvider, "{0:,6.4}", E));
        //    sb.Append("\t");
        //    sb.Append(String.Format(FormatProvider, "{0:,6.4}", N));
        //    sb.Append("\t");
        //    sb.Append(String.Format(FormatProvider, "{0:,6.4}", U)); 
        //    return sb.ToString();
        //}

        public override string ToString()
        {
            return E.ToString("0.00000") + ", " +N.ToString("0.00000") + ", " +  U.ToString("0.00000");
        }
    }
}
