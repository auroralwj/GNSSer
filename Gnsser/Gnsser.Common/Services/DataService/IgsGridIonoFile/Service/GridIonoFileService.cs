 //2017.08.17, czs, create in hongqing, 电离层文件的读取与服务
 //2018.05.13, czs, edit in hmx, 修复倾斜电离层延迟计算算法


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
    public class GridIonoFileService : IGridIonoFileService
    {
        Log log = new Log(typeof(GridIonoFileService));
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="filePath"></param>
        public GridIonoFileService(string filePath)
            : this(new IonoReader(filePath).ReadAll())
        {
        }
        /// <summary>
        /// 以文件初始化
        /// </summary>
        /// <param name="IonoFile"></param>
        public GridIonoFileService(IonoFile IonoFile)
        {
            SetFile(IonoFile);
        }
        /// <summary>
        /// 初始化插值器
        /// </summary>
        private void Init()
        {
            HeightOfModel = 450000;
            this.Name = this.IonoFile.Name;
        }

        /// <summary>
        /// 修改数据源，重新初始化。
        /// </summary>
        /// <param name="IonoFile"></param>
        public void SetFile(IonoFile IonoFile)
        {
            this.IonoFile = IonoFile;
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
        public IonoFile IonoFile { get; private set; }
        /// <summary>
        /// 有效服务时段
        /// </summary>
        public BufferedTimePeriod TimePeriod { get { return IonoFile.TimePeriod; } }
 
        /// <summary>
        /// 电离层薄层模型的高度 450 000 M
        /// </summary>
        public double HeightOfModel { get; set; }
        /// <summary>
        /// DCB of Site
        /// </summary>
        public Dictionary<string, RmsedNumeral> DcbsOfSites { get { return IonoFile.DcbsOfSites; } }
        /// <summary>
        /// DCB of PRN
        /// </summary>
        public Dictionary<SatelliteNumber, RmsedNumeral> DcbsOfSats { get { return IonoFile.DcbsOfSats; } }



        #endregion
        /// <summary>
        /// 获取当天测站DCB
        /// </summary>
        /// <param name="time"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public RmsedNumeral GetDcb(Time time, string name)
        {
            if (DcbsOfSites.ContainsKey(name))
            {
                return DcbsOfSites[name];
            }
            var key = "G_" + name.ToLower();
            if (DcbsOfSites.ContainsKey(key))
            {
                return DcbsOfSites[key];
            }
            key = "R_" + name.ToUpper();
            if (DcbsOfSites.ContainsKey(key))
            {
                return DcbsOfSites[key];
            }
            return RmsedNumeral.Zero;
        }

        /// <summary>
        /// 获取当天卫星DCB
        /// </summary>
        /// <param name="time"></param>
        /// <param name="prn"></param>
        /// <returns></returns>
        public RmsedNumeral GetDcb(Time time, SatelliteNumber prn)
        {
            if (DcbsOfSats.ContainsKey(prn))
            {
                return DcbsOfSats[prn];
            }
            return RmsedNumeral.Zero;
        } 



        /// <summary>
        /// 获取斜距方向!这是一种错误的做法！！！！！！！！！
        /// </summary>
        /// <param name="time">时间</param>
        /// <param name="geocentricLonLatOfIntersection">穿刺点球坐标（经纬度，单位：度）</param>
        /// <param name="satElevationOfPuncturePointDeg">穿刺点的卫星高度角，单位：度</param>
        /// <returns></returns>
        public RmsedNumeral GetSlope2(Time time, LonLat geocentricLonLatOfIntersection, double satElevationOfPuncturePointDeg)
        {
            var tec = Get(time, geocentricLonLatOfIntersection);
            if (tec == null)
            {
                return null;
            }
            // var factor1 = GetVerticalToSlopeFactor(satElevationDeg, Geo.Referencing.Ellipsoid.MeanRaduis, HeightOfModel);
            var factor = 1 / Math.Sin(satElevationOfPuncturePointDeg * Geo.Coordinates.AngularConvert.DegToRadMultiplier);
            //var includeAngle = GetIncludedAngleOfPuncturePointRad(satElevationOfSiteDeg);
            //var factor = 1 / Math.Cos(satElevationOfSiteDeg);

            return new RmsedNumeral()
            {
                Value = tec.Value * factor,
                Rms = tec.Rms * factor
            };
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
        /// 计算电离层穿刺点到测站与地球球心的夹角,也是穿刺点的卫星天顶距,返回弧度
        /// </summary>
        /// <param name="satElevationOfSiteDeg"></param>
        /// <returns></returns>
        public static double GetSatZenithAngleOfPuncturePointInSphereRad(double satElevationOfSiteDeg, double HeightOfModel)
        {
            var EarthRadius = Geo.Referencing.Ellipsoid.MeanRaduis;
            var cosEle = EarthRadius * Math.Cos(satElevationOfSiteDeg * Geo.Coordinates.AngularConvert.DegToRadMultiplier);
            var angle = Math.Asin(cosEle / (EarthRadius + HeightOfModel));
            return angle;
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

        ///// <summary>
        ///// ．斜ＴＥＣ转垂直ＴＥＣ的乘法因子。
        ///// </summary>
        ///// <param name="satElevation">卫星高度角,角度degree</param>
        ///// <param name="radiusOfMeanEarth">地球平均半径</param>
        ///// <param name="heightOfIono">电离层模型高度</param>
        ///// <returns></returns>
        //private static double GetSlopeToVerticalFactor(double satElevation, double radiusOfMeanEarth, double heightOfIono)
        //{
        //    return Math.Sqrt(1.0 - Math.Pow(radiusOfMeanEarth * Math.Cos(satElevation * Geo.Coordinates.AngularConvert.DegToRadMultiplier) / (radiusOfMeanEarth + heightOfIono), 2.0));
        //}
        /// <summary>
        /// 垂直ＴＥＣ转斜ＴＥＣ的乘法因子。
        /// </summary>
        /// <param name="satElevation">卫星高度角,角度degree</param>
        /// <param name="radiusOfMeanEarth">地球平均半径</param>
        /// <param name="heightOfIono">电离层模型高度</param>
        /// <returns></returns>
        //private static double GetVerticalToSlopeFactor(double satElevation, double radiusOfMeanEarth, double heightOfIono)
        //{
        //    return 1.0 /  GetSlopeToVerticalFactor(satElevation, radiusOfMeanEarth, heightOfIono);
        //}

        /// <summary>
        /// 获取服务为，原始数据。单位 1e16.
        /// </summary>
        /// <param name="time"></param>
        /// <param name="geocentricLonlatDeg"></param>
        /// <returns></returns>
        public RmsedNumeral Get(Time time, LonLat geocentricLonlatDeg)
        {
            double bufferedTime = 2 * 3600;
            this.TimePeriod.SetSameBuffer(bufferedTime);
            if (!TimePeriod.BufferedContains(time))
            {
                log.Error(Name + " 不在有效服务时段内。 " + TimePeriod + ", " + time);
                return null;
                throw new ApplicationException();
            }

            List<Time> times = TimeUtil.GetNearst(this.IonoFile.Keys, time);
            if (times.Count == 1)//刚好，或在边界，无需拟合
            {
                return Interpolate(this.IonoFile.Get(times[0]), geocentricLonlatDeg);
            }
            else if (times.Count >= 3)//需要拟合
            {
                var smallTime = times[1];
                var largerTime = times[2];
                IonoSection regoin1 = this.IonoFile.Get(smallTime);
                IonoSection regoin2 = this.IonoFile.Get(largerTime);


                var val1 = Interpolate(regoin1, geocentricLonlatDeg);
                var val2 = Interpolate(regoin2, geocentricLonlatDeg);
                var t = time.TickTime.SecondsOfWeek;
                var t1 = smallTime.TickTime.SecondsOfWeek;
                var t2 = largerTime.TickTime.SecondsOfWeek;

                if (t2 == 0)
                {
                    t2 = SecondTime.SECOND_PER_WEEK;// 3600 * 24 * 7;
                }

                var val = Geo.Utils.DoubleUtil.Interpolate(t, t1, t2, val1.Value, val2.Value);
                var rms = Geo.Utils.DoubleUtil.Interpolate(t, t1, t2, val1.Rms, val2.Rms);

                return new RmsedNumeral(val, rms);
            }

            IonoSection regoin = this.IonoFile.Get(times[0]);
            List<double> lats = Geo.Utils.DoubleUtil.GetNearst(regoin.Keys, geocentricLonlatDeg.Lat, false);

            IonoRecord record = regoin.Get(lats[0]);
            List<double> lons = Geo.Utils.DoubleUtil.GetNearst(record.Keys, geocentricLonlatDeg.Lon, false);
            return record[lons[0]];
        }
     

        /// <summary>
        /// 内插不同经纬度的值。
        /// </summary>
        /// <param name="IonoSection"></param>
        /// <param name="geoCoord">待内插坐标</param>
        /// <returns></returns>
        public RmsedNumeral Interpolate(IonoSection IonoSection, LonLat geoCoord)
        {
            List<double> lats = Geo.Utils.DoubleUtil.GetNearst(IonoSection.Keys, geoCoord.Lat, false);
            if (lats.Count == 1)//在指定的纬度圈上面
            {
                var lat = lats[0];
                IonoRecord record = IonoSection.Get(lat);
                List<double> lons = Geo.Utils.DoubleUtil.GetNearst(record.Keys, geoCoord.Lon, false);
                if (lons.Count == 1) //刚好，找到你了，不用差值
                {
                    var lon = lons[0];
                    return record[lon];
                }
                else if (lons.Count == 3)//差值
                {
                    var lon1 = lons[1];
                    var lon2 = lons[2];
                    var val1 = record[lon1];
                    var val2 = record[lon2];
                    var val = Geo.Utils.DoubleUtil.Interpolate(geoCoord.Lon, lon1, lon2, val1.Value, val2.Value);
                    var rms = Geo.Utils.DoubleUtil.Interpolate(geoCoord.Lon, lon1, lon2, val1.Rms, val2.Rms);
                    return new RmsedNumeral(val, rms);
                }
                else
                {
                    throw new Exception("内插错误，获取经度值失败");
                }
            }
            else if (lats.Count == 3)
            {
                var lat1 = lats[1];
                var lat2 = lats[2];

                IonoRecord record1 = IonoSection.Get(lat1);
                IonoRecord record2 = IonoSection.Get(lat2);

                var lat = lats[0];
                IonoRecord record = IonoSection.Get(lat);
                List<double> lons = Geo.Utils.DoubleUtil.GetNearst(record1.Keys, geoCoord.Lon, false);
      
                if (lons.Count == 1)
                {
                    var lon = lons[0];
                    var val0 = record1[lon];
                    var val1 = record2[lon];
                    var val = Geo.Utils.DoubleUtil.Interpolate(geoCoord.Lat, lat1, lat2, val0.Value, val1.Value);
                    var rms = Geo.Utils.DoubleUtil.Interpolate(geoCoord.Lat, lat1, lat2, val0.Rms, val1.Rms);
                    return new RmsedNumeral(val, rms);
                }
                else if (lons.Count >= 3)
                {
                    var lon1 = lons[1];
                    var lon2 = lons[2];

                    var val11 = record1[lon1];
                    var val12 = record1[lon2];
                    var val21 = record2[lon1];
                    var val22 = record2[lon2];


                    var val = Geo.Utils.DoubleUtil.Interpolate(geoCoord.Lon, geoCoord.Lat, lon1, lon2, lat1, lat2, val11.Value, val12.Value, val21.Value, val22.Value);
                    var rms = Geo.Utils.DoubleUtil.Interpolate(geoCoord.Lon, geoCoord.Lat, lon1, lon2, lat1, lat2, val11.Rms, val12.Rms, val21.Rms, val22.Rms);
                    return new RmsedNumeral(val, rms);
                }
            }
            else
            {
                throw new Exception("内插错误，获取值纬度失败");
            }
            throw new Exception("内插错误，不可能出现");
        }
    }
}