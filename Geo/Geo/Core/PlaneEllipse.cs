//2016.06.04, czs,create in hongqing, 椭圆轨道

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Geo.Coordinates;

namespace Geo
{ 
    /// <summary>
    /// 平面椭圆
    /// </summary>
    public class PlaneEllipse
    {
        public const double PI = 3.14159265358979323846;
        public const double GM = 3.9860050E14;
 
        /// <summary>
        /// 平面椭圆
        /// </summary>
        /// <param name="a">椭圆长半径</param>
        /// <param name="e">离心率</param>
        public PlaneEllipse(double a, double e)
        {
            this.a = a;
            this.e = e;
        }

        #region 基本参数
        /// <summary>
        /// 离心率 Eccentricity
        /// </summary>
        public double e { get; set; }

        /// <summary> 
        /// 长半轴 SemiMajor
        /// </summary>
        public double a { get; set; }
        #endregion
        /// <summary>
        /// e*e
        /// </summary>
        public double ee { get { return e * e; } }
        /// <summary>
        /// 短半轴 SemiMinor
        /// </summary>
        public double b { get { return a * Math.Sqrt( 1 - e * e ); } }
        /// <summary>
        /// 焦点长 FocalLength
        /// </summary>
        public double c { get { return e * a; } }
        /// <summary>
        /// 椭圆面积 Area
        /// </summary>
        public double Area { get { return PI * a * b; } }
        /// <summary>
        /// 椭圆的半通径 P
        /// </summary>
        public double P { get { return b * b / a; } }
        /// <summary>
        /// 平均角速度 n，弧度/秒
        /// </summary>
        public double n { get { return Math.Sqrt(GM) / (a * SqrtSemiMajor); } }
       /// <summary>
        /// 周期，一圈所费时间,单位秒
        /// </summary>
        public double OrbitCycleTime { get { return 2 * PI / n; } }

        #region 具有意义的属性
        /// <summary>
        /// 平均角速度。弧度/秒
        /// </summary>
        public double MeanAngularVelocity { get { return n; } }
        /// <summary>
        /// 轨道长半径平方根
        /// </summary>
        public double SqrtSemiMajor { get { return Math.Sqrt(a); } }
        /// <summary>
        /// 椭圆的半通径，米
        /// </summary>
        public double SemiChordDiameter { get { return P; } }
        /// <summary>
        /// 离心率
        /// </summary>
        public double FocalLength { get { return c; } }
        /// <summary>
        /// 轨道短半径，b
        /// </summary>
        public double SemiMinor { get { return b; } }
        /// <summary>
        /// 离心率
        /// </summary>
        public double Eccentricity { get { return e; } set { e = value; } }
        /// <summary>
        /// 离心率
        /// </summary>
        public double SemiMajor { get { return a; } set { a = value; } }


        #endregion

        public override bool Equals(object obj)
        {
             var el = obj as PlaneEllipse;
             if (el == null) return false;

             return el.a == this.a && el.e == this.e;
        }

        public override int GetHashCode()
        {
            return a.GetHashCode() + e.GetHashCode();
        }
        public override string ToString()
        {
            return a.ToString() + ", " +e.ToString();
        }
    }



}
