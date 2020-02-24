using System;
using System.Collections.Generic;

using System.Text;
using Geo.Utils;

namespace Geo.Coordinates
{
    /// <summary>
    /// 仪器坐标
    /// </summary>
    public class HEN //: Coordinate
    {
        /// <summary>
        /// 构造
        /// </summary>
        public HEN()// : base(Referencing.CoordinateReferenceSystem.HenCrs, 0, Referencing.CoordinateType.HEN)
        { }
        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="h"></param>
        /// <param name="n"></param>
        /// <param name="e"></param>
        public HEN(double h, double n, double e)
            :this ()
        {
            this.H = h;
            this.E = e;
            this.N = n;
        }
        /// <summary>
        /// 高度
        /// </summary>
        public double H { get; set; }
        /// <summary>
        /// 东方
        /// </summary>
        public double E { get; set; }
        /// <summary>
        /// 北方
        /// </summary>
        public double N { get; set; }

        /// <summary>
        /// +
        /// </summary>
        /// <param name="first"></param>
        /// <returns></returns>
        public static HEN operator +(HEN first)
        {
            return first;
        }
        /// <summary>
        /// -
        /// </summary>
        /// <param name="first"></param>
        /// <returns></returns>
        public static HEN operator -(HEN first)
        {
            return new HEN(-first.H, -first.N, -first.E);
        }
        /// <summary>
        /// +
        /// </summary>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <returns></returns>
        public static HEN operator +(HEN first, HEN second)
        {
            return new HEN(first.H + second.H, first.N + second.N, first.E + second.E);
        }
        /// <summary>
        /// 坐标平移。
        /// </summary>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <returns></returns>
        public static HEN operator -(HEN first, HEN second)
        {
            return new HEN(first.H - second.H, first.N - second.N, first.E - second.E);
        }
        /// <summary>
        /// 字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return ToRnxString();
        }
        /// <summary>
        /// 是否为 0
        /// </summary>
        public bool IsZero { get => H == 0 && N == 0 && E == 0; }

        /// <summary>
        /// 解析
        /// </summary>
        /// <param name="hStr"></param>
        /// <param name="nStr"></param>
        /// <param name="eStr"></param>
        /// <returns></returns>
        public static HEN TryParse(string hStr, string nStr, string eStr)
        {
            double h, e, n;
            double.TryParse(hStr, out h);
            double.TryParse(nStr, out n);
            double.TryParse(eStr, out e);
            return new HEN(h, n, e);
        }

        /// <summary>
        /// 0.0780 0.0000 0.0000
        /// 1X,F6.4
        /// </summary>
        /// <returns></returns>
        public string ToRnxString()
        {
            return H.ToString("0.0000").Replace("-0.", "-.") + " " + N.ToString("0.0000").Replace("-0.", "-.") + " " + E.ToString("0.0000").Replace("-0.", "-.");
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

            return StringUtil.FillSpaceLeft(H.ToString(formate).Replace("-0.", "-."), interger)
                + " " + StringUtil.FillSpaceLeft(N.ToString(formate).Replace("-0.", "-."), interger)
                + " " + StringUtil.FillSpaceLeft(E.ToString(formate).Replace("-0.", "-."), interger);
        }
        /// <summary>
        /// 题目
        /// </summary>
        /// <returns></returns>
        public  string GetTabTitles()
        {
            return "H\tE\tN";
        }
        /// <summary>
        /// 表格值
        /// </summary>
        /// <returns></returns>
        public  string GetTabValues()
        {
            return String.Format(new NumeralFormatProvider(), "{0}\t{1}\t{2}", H, E, N);
        }
    }
}
