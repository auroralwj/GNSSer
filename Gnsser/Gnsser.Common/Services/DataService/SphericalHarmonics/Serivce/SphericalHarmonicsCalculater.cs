//2017.11.06, czs, create, 球谐系数
//2018.05.26, czs, edit in HMX, 修改属性支持电离层球谐函数

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Gnsser.Service;
using Geo.Coordinates;
using Gnsser.Data.Rinex;
using Geo.Algorithm;
using Gnsser.Times;
using Geo.Utils;
using Gnsser;
using Geo.Times;
using Geo;
using Geo.IO;

namespace Gnsser.Data
{
    /// <summary>
    /// 球谐函数计算器
    /// </summary>
    public class SphericalHarmonicsCalculater
    {
        Log log = new Log(typeof(SphericalHarmonicsCalculater));
        /// <summary>
        /// 球谐函数计算器
        /// </summary>
        /// <param name="File"></param>
        public SphericalHarmonicsCalculater(SphericalHarmonicsFile File)
        {
            this.File = File;
            if (this.File.Contains(0))//包含0，非重力，为高程
            {
                IsCommon2DSphericalHarmonics = true;
            }
        }

        /// <summary>
        /// 是否是从0开始的普通球谐函数，与高程无关，二维，如电离层、高程和深度数据，即没有高程
        /// </summary>
        public bool IsCommon2DSphericalHarmonics { get; set; }

        /// <summary>
        /// 文件
        /// </summary>
        private SphericalHarmonicsFile File { get; set; }
        /// <summary>
        /// m
        /// </summary>
        public const double a = 6378136.3;
        public const double R = 6378136.3;
        /// <summary>
        /// const
        /// </summary>
        public const double  GM =3986004.415E8;



        #region 算法工具 

        /// <summary>
        /// 获取值
        /// </summary>
        /// <param name="order">最大阶次</param>
        /// <param name="coord">经纬度坐标，球面坐标</param>
        /// <param name="radius">球半径</param>
        /// <returns></returns>
        public double GetValue(int order, LonLat coord, double radius)
        { 
            var lon_rad = Geo.Coordinates.AngularConvert.ToRad( coord.Lon, coord.Unit);
            var lat_rad = Geo.Coordinates.AngularConvert.ToRad(coord.Lat, coord.Unit);
            
            return GetValue(order, lon_rad, lat_rad, radius);
        }
        /// <summary>
        /// 获取值
        /// </summary>
        /// <param name="order"></param>
        /// <param name="coord">球坐标</param>
        /// <returns></returns>
        public double GetValue(int order, Polar coord)
        {
            var lon_rad = Geo.Coordinates.AngularConvert.ToRad(coord.Azimuth, coord.Unit);
            var lat_rad = Geo.Coordinates.AngularConvert.ToRad(coord.Elevation, coord.Unit);

            return GetValue(order, lon_rad, lat_rad, coord.Range); 
        }

        /// <summary>
        /// 计算值，单位：弧度
        /// </summary>
        /// <param name="nMax">最大阶次</param>
        /// <param name="r">球半径</param>
        /// <param name="phi_rad">球坐标的纬度，高度角，弧度</param>
        /// <param name="lambda_rad">球坐标的经度，方位角，弧度</param>
        /// <returns></returns>
        public Double GetValue(int nMax, double lambda_rad, double phi_rad,double r)
        {
            //高程水深数据
            if (IsCommon2DSphericalHarmonics)
            {
               return  GetCommon2DSphericalHarmonicsValue(nMax, lambda_rad, phi_rad);
            }
            //重力场数据
            double former = GM / r;
            double latter = 1 + FirstSuperposition(nMax, lambda_rad,phi_rad, r);
            return former * latter;
        }
        /// <summary>
        /// 前面的迭代
        /// </summary>
        /// <param name="nMax"></param>
        /// <param name="lambda_rad"></param>
        /// <param name="phi_rad">地心纬度</param>
        /// <param name="r"></param>
        /// <returns></returns>
        private double FirstSuperposition(int nMax, double lambda_rad, double phi_rad, double r)
        {
            //转换为天顶距
            double val = 0;
            double adivr = a / r;
            
            double theta_rad = Math.PI / 2 - phi_rad;

            FastLegendreCalculater leg = new FastLegendreCalculater(theta_rad);

            for (int n = File.FirstKey; n <= nMax; n++)
            {
                var former = Math.Pow(adivr, n);
                var latter = HarmonicsSuperposition(n, lambda_rad, theta_rad, leg);
                val += former * latter;
            }
            return val;
        }
        /// <summary>
        /// 常规高程无关球谐函数，如获取高程或深度，电离层电子数等。
        /// </summary>
        /// <param name="nMax"></param>
        /// <param name="lambda_rad"></param>
        /// <param name="phi_rad"> 地心纬度</param>
        /// <returns></returns>
        private double GetCommon2DSphericalHarmonicsValue(int nMax, double lambda_rad, double phi_rad)
        {
            //转换为天顶距
           // double theta_rad = Math.PI / 2 - phi_rad;
            double theta_rad = Math.PI / 2 - phi_rad;
            FastLegendreCalculater leg = new FastLegendreCalculater(theta_rad);
            double val = 0; 
            for (int n = File.FirstKey; n <= nMax; n++)
            {
                var latter = HarmonicsSuperposition(n, lambda_rad, theta_rad, leg);
                val += latter;
            }
            return val;
        }

        /// <summary>
        /// 球谐函数迭代
        /// </summary>
        /// <param name="theta_rad">rad</param>
        /// <param name="n"></param>
        /// <param name="lambda">rad</param>
        /// <param name="leg">rad，勒让德直接取值</param>
        /// <returns></returns>
        private double HarmonicsSuperposition(int n, double lambda, double theta_rad, FastLegendreCalculater leg)
        {
            double val = 0;
            var nStep = File[n];

            double[] legs = FastLegendreCalculater.Leg(n, theta_rad);

            for (int m = 0; m <= n; m++)
            {
                double Cnm = nStep.C[m].Value;
                double Snm = nStep.S[m].Value;
                var former = GetItemValue(Cnm, Snm, lambda, m);
                var latter = legs[m]; 
                val += former * latter;
            } 
            return val;
        }



        /// <summary>
        /// 计算叠加项
        /// </summary>
        /// <param name="Cnm"></param>
        /// <param name="Snm"></param>
        /// <param name="lambda">rad</param>
        /// <param name="m"></param>
        /// <returns></returns>
        private static double GetItemValue(double Cnm, double Snm, double lambda, int m)
        {
            double mLambda = lambda * m;
            return Cnm * Math.Cos(mLambda) + Snm * Math.Sin(mLambda);
        }
#endregion

    }
}
