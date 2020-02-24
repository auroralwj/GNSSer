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
    public class NEU : IEquatable<NEU>, IToTabRow//Coordinate, 
    {
        /// <summary>
        /// 北东天坐标。卫星坐标，局部坐标，测站坐标。当地左手笛卡尔坐标系
        /// </summary>
        public NEU()
    //    :base(Referencing.CoordinateReferenceSystem.NeuCrs, 0, Referencing.CoordinateType.NEU)
        { }
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="hen"></param>
        public NEU(HEN hen):this( hen.N, hen.E,hen.H)
        {

        }
        /// <summary>
        /// 北东天坐标。卫星坐标，局部坐标，测站坐标。当地左手笛卡尔坐标系
        /// </summary>
        /// <param name="n">北方向，X</param>
        /// <param name="e">东方向，Y</param>
        /// <param name="u">向上，Z</param>
        public NEU(double n, double e, double u) :this(){ this.N = n; this.E = e; this.U = u; }
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
        /// 站心坐标系
        /// </summary>
        public ENU ENU { get { return new ENU(E, N, U); } }
        public static NEU operator +(NEU a, NEU b) { return new NEU(a.N + b.N, a.E + b.E, a.U + b.U); }
        public static NEU operator -(NEU a, NEU b) { return new NEU(a.N - b.N, a.E - b.E, a.U - b.U); }        
        public static NEU operator -(NEU neu) { return new NEU(-neu.N, -neu.E, -neu.U); }

        public override bool Equals(object obj)
        {
            NEU g = obj as NEU;
            if (g == null) return false;

            return Equals(g);
        }

        public  bool Equals(NEU g)
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
        /// <param name="info"></param>
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
        public double Distance(NEU neu)
        {
            return Math.Sqrt((N - neu.N) * (N - neu.N) + (E - neu.E) * (E - neu.E) + (U - neu.U) * (U - neu.U));
        }
        public double CosN { get { return N / this.Length; } }
        public double CosE { get { return E / this.Length; } }
        public double CosU { get { return U / this.Length; } }
        public double Length { get { return Math.Sqrt((N) * (N) + (E) * (E) + (U) * (U)); } }

        /// <summare>
        /// 按权拟合。
        /// </summare>
        /// <param name="neuA"></param>
        /// <param name="weightA"></param>
        /// <param name="neuB"></param>
        /// <param name="weightB"></param>
        /// <returns></returns>
        public static NEU GetNEU(NEU neuA, double weightA, NEU neuB, double weightB)
        {
            return new NEU()
            {
                N = (neuA.N * weightA + neuB.N * weightB) / (weightA + weightB),
                E = (neuA.E * weightA + neuB.E * weightB) / (weightA + weightB),
                U = (neuA.U * weightA + neuB.U * weightB) / (weightA + weightB)
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
            double a = N * T.X + E * T.Y + U * T.Z;
            return a;
        }

        /// <summary>
        /// T是单位向量
        /// </summary>
        /// <param name="T"></param>
        /// <returns></returns>
        public double Dot(NEU T)
        {
            //
            double a = N * T.N + E * T.E + U * T.U;
            return a;
        }

        public NEU UnitNeuVector()
        {
            double mag = this.Length;
            if (mag <= Math.Pow(10, -14))
            {
                throw new Exception("Divide by Zero Error");
            }
            NEU retArg = new NEU(this.N/ mag, this.E / mag, this.U / mag);
            return retArg;
        }
        /// <summary>
        /// 0 0 0
        /// </summary>
        public new static NEU Zero
        {
            get
            {
                return new NEU(0,0,0);
            }
        }
        /// <summary>
        /// 解析字符串，可以解析空格、分号、换行符、回车符、Tab为分隔符的字符串
        /// </summary>
        /// <param name="toString"></param>
        /// <returns></returns>
        public static new NEU Parse(string toString) { return Parse(toString, new char[] { ',', ';', ' ', '\t', '\n', '\r' }); }

        /// <summary>
        /// (x,y) (x,y,z) (x y z) x y z
        /// </summary>
        /// <param name="toString"></param>
        /// <param name="separaters"></param>
        /// <returns></returns>
        public static NEU Parse(string toString, char[] separaters)
        {
            toString = toString.Replace("(", "").Replace(")", "");
            string[] strs = toString.Split(separaters, StringSplitOptions.RemoveEmptyEntries);
            return new NEU(Double.Parse(strs[0]), Double.Parse(strs[1]), Double.Parse(strs[2]));
        }

        /// <summary>
        /// 从一维数组中解析。
        /// </summary>
        /// <param name="result">计算结果</param>
        /// <returns></returns>
        public static NEU Parse(double[] result)
        {
            return new NEU(result[0], result[1], result[2]);
        }
        /// <summary>
        /// 转为一维数组
        /// </summary>
        public double[] Array { get { return new Double[] { N, E, U }; } }

        public virtual string GetTabTitles()
        {
           return "N\tE\tU";
        }
       
       static   NumeralFormatProvider FormatProvider = new NumeralFormatProvider();

        public virtual string GetTabValues()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(String.Format(FormatProvider, "{0:,6.4}", N));
            sb.Append("\t");
            sb.Append(String.Format(FormatProvider, "{0:,6.4}", E));
            sb.Append("\t");
            sb.Append(String.Format(FormatProvider, "{0:,6.4}", U)); 
            return sb.ToString();
        }

        public override string ToString()
        {
            return N + "," + E + "," + U;
        }
    }
}
