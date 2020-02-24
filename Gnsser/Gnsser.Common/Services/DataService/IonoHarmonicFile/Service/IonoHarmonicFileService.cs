//2018.05.25, czs, create in HMX, CODE电离层球谐函数

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Geo.Coordinates;//
using Gnsser.Times;
using Gnsser.Service;
using Geo.Times;
using Geo;
using Geo.IO;
using Gnsser.Data.Rinex;

namespace Gnsser.Data
{
  
    /// <summary>
    /// 电离层文件的读取与服务
    /// </summary>
    public class IonoHarmonicFileService : IIonoService
    {
        Log log = new Log(typeof(GridIonoFileService));
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="filePath"></param>
        public IonoHarmonicFileService(string filePath)
            : this(new IonoHarmonicReader(filePath).ReadAll())
        {
        }
        /// <summary>
        /// 以文件初始化
        /// </summary>
        /// <param name="IonoHarmonicFile"></param>
        public IonoHarmonicFileService(IonoHarmonicFile IonoHarmonicFile)
        {
            SetFile(IonoHarmonicFile);
        }
        /// <summary>
        /// 初始化插值器
        /// </summary>
        private void Init()
        {
            HeightOfModel = 450000;
            this.Name = this.IonoHarmonicFile.Name;
        }

        /// <summary>
        /// 修改数据源，重新初始化。
        /// </summary>
        /// <param name="IonoHarmonicFile"></param>
        public void SetFile(IonoHarmonicFile IonoHarmonicFile)
        {
            this.IonoHarmonicFile = IonoHarmonicFile;
            this.Init();
        }

        #region 属性
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        ///   文件 
        /// </summary>
        public IonoHarmonicFile IonoHarmonicFile { get; private set; }
        /// <summary>
        /// 有效服务时段
        /// </summary>
        public BufferedTimePeriod TimePeriod { get { return IonoHarmonicFile.TimePeriod; } }
 
        /// <summary>
        /// 电离层薄层模型的高度 450 000 M
        /// </summary>
        public double HeightOfModel { get; set; }

        #endregion

        /// <summary>
        /// 垂直方向的电子数量，获取服务，为原始计算和拟合数据。单位 1e16.
        /// 失败返回 NaN
        /// </summary>
        /// <param name="time"></param>
        /// <param name="geocentricLonlatDeg">以度为单位的</param>
        /// <returns>失败返回 NaN</returns>
        public RmsedNumeral Get(Time time, LonLat geocentricLonlatDeg)
        {
            if (!this.TimePeriod.Contains(time)) { return RmsedNumeral.NaN; }

            //首先查找附近的时间。
            if (this.IonoHarmonicFile.Contains(time))
            {
                var data = this.IonoHarmonicFile.Get(time);
                return GetValue(time, geocentricLonlatDeg, data);
            }

            //如果在之前或之后，则直接采用一个
            if (time < TimePeriod.Start)
            {
                var data = this.IonoHarmonicFile.Get(TimePeriod.Start);
                return GetValue(time, geocentricLonlatDeg, data);
            }
            if (time > TimePeriod.End)
            {
                var data = this.IonoHarmonicFile.Get(TimePeriod.End);
                return GetValue(time, geocentricLonlatDeg, data);
            }
            //在中间
            var times = TimeUtil.GetNearst(IonoHarmonicFile.OrderedKeys, time);
            if(times.Count == 1)
            {
                var data = this.IonoHarmonicFile.Get(times[0]);
                return GetValue(time, geocentricLonlatDeg, data);
            }
            //一定是三个
            var prevTime = times[1];
            var nextTime = times[2];
            var preVal = GetValue(time, geocentricLonlatDeg, this.IonoHarmonicFile.Get(prevTime));
            var nextVal = GetValue(time, geocentricLonlatDeg, this.IonoHarmonicFile.Get(nextTime));
            double val = Geo.Utils.DoubleUtil.WeightedAverage(preVal.Value, nextVal.Value, Math.Abs(time - prevTime), Math.Abs(time - nextTime));
            
            return new RmsedNumeral(val, 0);
        }

        /// <summary>
        /// 垂直方向的电子数量
        /// </summary>
        /// <param name="time"></param>
        /// <param name="geocentricLonlatDeg"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        private static RmsedNumeral GetValue(Time time, LonLat geocentricLonlatDeg, IonoHarmonicSection data)
        {
            SphericalHarmonicsCalculater calculater = new SphericalHarmonicsCalculater(data);
            var radius = Geo.Referencing.Ellipsoid.MeanRaduis;// HeightOnSphere + AveRadiusOfEarch;                 
            LonLat lonLatSE = geocentricLonlatDeg.GetSeLonLat(time.DateTime);//转换为日固坐标系

            var val = calculater.GetValue(data.MaxDegree, lonLatSE, radius);
            return new RmsedNumeral(val, 0);
        }

        /// <summary>
        /// 获取倾斜延迟距离
        /// </summary>
        /// <param name="time">历元</param>
        /// <param name="siteXyz">测站坐标</param>
        /// <param name="satXyz">卫星坐标</param>
        /// <param name="freq">频率</param>
        /// <returns></returns>
        public double GetSlopeDelayRange(Time time, XYZ siteXyz, XYZ satXyz, double freq)
        {
            var tec = GetSlope(time, siteXyz, satXyz);
            return GetIonoDelayRange(tec.Value, freq);
        }
        /// <summary>
        /// 获取倾斜电离层电子数
        /// </summary>
        /// <param name="time">历元</param>
        /// <param name="siteXyz">测站坐标</param>
        /// <param name="satXyz">卫星坐标</param>
        /// <returns></returns>
        public RmsedNumeral GetSlope(Time time, XYZ siteXyz, XYZ satXyz)
        {
            var punctPoint = XyzUtil.GetIntersectionXyz(siteXyz, satXyz, HeightOfModel);
            var geocentricLonLat = Geo.Coordinates.CoordTransformer.XyzToSphere(punctPoint);
            var SpherePolar = CoordTransformer.XyzToSpherePolar(satXyz, siteXyz, AngleUnit.Degree);

            var tec = GetSlope(time, geocentricLonLat, SpherePolar.Elevation);
            return tec;
        }  




        /// <summary>
        /// 获取斜距方向
        /// </summary>
        /// <param name="time">时间</param>
        /// <param name="geocentricLonLatOfIntersection">穿刺点球坐标（经纬度，单位：度）</param>
        /// <param name="satElevationOfSiteDeg">测站的卫星高度角，单位：度</param>
        /// <returns></returns>
        public RmsedNumeral GetSlope(Time time, LonLat geocentricLonLatOfIntersection, double satElevationOfSiteDeg)
        {
            var tec = Get(time, geocentricLonLatOfIntersection);
            if (tec == null)
            {
                return null;
            }
            // var factor1 = GetVerticalToSlopeFactor(satElevationDeg, Geo.Referencing.Ellipsoid.MeanRaduis, HeightOfModel);
            // var factor = 1 / Math.Sin(satElevationOfSiteDeg * Geo.Coordinates.AngularConvert.DegToRadMultiplier);
            var includeAngle = GetSatZenithAngleOfPuncturePointInSphereRad(satElevationOfSiteDeg, HeightOfModel);
            var factor = 1 / Math.Cos(includeAngle);

            return new RmsedNumeral()
            {
                Value = tec.Value * factor,
                Rms = tec.Rms * factor
            };
        }


        /// <summary>
        /// 斜距影响
        /// </summary>
        /// <param name="time"></param>
        /// <param name="geoCoordOfIntersection">电离层坐标</param>
        /// <param name="satElevationDeg"></param>
        /// <param name="freq">频率，单位 10^6</param>
        /// <returns></returns>
        public double GetSlopeLen(Time time, GeoCoord geoCoordOfIntersection, double satElevationDeg, double freq)
        {
            var val = GetSlope(time, geoCoordOfIntersection, satElevationDeg);
            return GetIonoDelayRange(val.Value, freq);
        }

        /// <summary>
        /// 电离层对于伪距的延迟距离
        /// </summary>
        /// <param name="tec">单位 1e16.</param>
        /// <param name="freq">频率，单位 10^6</param>
        /// <returns></returns>
        public static double GetIonoDelayRange(double tec, double freq)
        {
            return tec * 40.28 / (freq * freq) * 1e4;//斜方向延迟             
        }
        /// <summary>
        /// 计算电离层穿刺点到测站与地球球心的夹角,也是穿刺点的卫星天顶距,返回弧度
        /// </summary>
        /// <param name="satElevationOfSiteDeg"></param>
        /// <param name="HeightOfModel"></param>
        /// <returns></returns>
        public static double GetSatZenithAngleOfPuncturePointInSphereRad(double satElevationOfSiteDeg, double HeightOfModel)
        {
            var EarthRadius = Geo.Referencing.Ellipsoid.MeanRaduis;
            var cosEle = EarthRadius * Math.Cos(satElevationOfSiteDeg * Geo.Coordinates.AngularConvert.DegToRadMultiplier);
            var angle = Math.Asin(cosEle / (EarthRadius + HeightOfModel));
            return angle;
        } 


        #region 计算细节，暂时未用

        /// <summary>
        /// 计算球谐函数值
        /// </summary>
        /// <param name="lonOfSE"></param>
        /// <param name="sinlatRad"></param>
        /// <param name="maxDegreeAndOrder"></param>
        /// <returns></returns>
        public double GetHarmounicValue(double lonOfSE, double sinlatRad, int maxDegreeAndOrder)
        {
            return GetHarmounicValue(  lonOfSE,   sinlatRad, maxDegreeAndOrder, maxDegreeAndOrder);
        }

        /// <summary>
        /// 计算球谐函数值
        /// </summary>
        /// <param name="lonOfSE"></param>
        /// <param name="sinlatRad"></param>
        /// <param name="maxDegree"></param>
        /// <param name="maxOrder"></param>
        /// <returns></returns>
        public double GetHarmounicValue( double lonOfSE, double sinlatRad, int maxDegree, int maxOrder)
        {
            if(maxOrder > maxDegree) { maxOrder = maxDegree; }
            double val = 0;
            for (int degree = 0; degree < maxDegree; degree++)
            {
                
                for (int order = 0; order < maxOrder && order<= degree; order++)
                {
                    double anm = 0;
                    double bnm = 0;

                    var leg = LegendrePolynomials(degree, order, sinlatRad);
                    double lon = order * lonOfSE;
                    var second = anm * Math.Cos(lon) + bnm * Math.Sin(lon);
                    var itemVal = second  * leg;
                    val += itemVal;
                }
            }
            return val;
        }

        public double LegendrePolynomials(int degree, int order, double sinlatRad)
        {
            return 0;
        }

        /// <summary>
        /// The normalization function Λ
        /// </summary>
        /// <param name="n"></param>
        /// <param name="m"></param>
        /// <param name="kronecherDelta"></param>
        /// <returns></returns>
        private double NormalizationFunction(int n, int m, double kronecherDelta)
        {
            if(n==0 && m == 0)
            {
                return 1.0;
            }

            double inner = 2.0 * (2.0 * n + 1.0) * Factorial(n - m) / (1.0 + kronecherDelta) / Factorial(n + m);
            double val = Math.Sqrt(inner);
            return val;
        }

        /// <summary>
        /// 阶乘
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public int Factorial(int num)
        {
            int val = 1;
            for (int i = 1; i <= num; i++)
            {
                val += i;
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