//2017.08.17, czs, edit in hongqing, 考虑做成服务
//2017.09.11, czs, edit in hongqing, 按照 IS-GPS-200H.pdf 修改

using System;
using Geo.Coordinates;
using Gnsser.Domain;
using Geo;
using Geo.Times;
using Gnsser.Data;

namespace Gnsser.Correction
{

    //1.利用双频改正 这一方法能消除电离层影响的95%。
    //2.利用模型改正   使用单频接收机的用户，一般用导航电文中的电离层模型加以改正。但是，这类模型还不完善，改正的有效性可能低于75%。
    //3.同步观测  卫星信号到达相距较近的不同测站的路径相近，所经介质状况相似，所以通过不同测站对同一卫星的观测量组差后，误差将明显减弱电离层的影响， 其残差不会超过1ppm。
    //由于非真空，电离层造成延迟，使得时间变长，测距变长，需减去延迟部分。符号为负

    /// <summary>
    /// 电离层参数改正 克罗布歇Klobuchar模型,GPS导航电文改正模型，返回的是以米为单位的距离改正。
    /// 见 IS-GPS-200H.pdf
    /// </summary>
    public class IonoParamModelCorrector : AbstractRangeCorrector
    {
        /// <summary>
        /// 电离层参数模型改正，构造函数。
        /// </summary>
        /// <param name="IonoParamService"></param>
        /// <param name="IsCorrectionOnPhase"></param>
        public IonoParamModelCorrector(IonoParamService IonoParamService, bool IsCorrectionOnPhase = false)
        {
            this.Name = "电离层参数模型距离改正";
            this.IsCorrectionOnPhase = IsCorrectionOnPhase;
            this.IonoParamService = IonoParamService;
            this.CorrectionType = Gnsser.Correction.CorrectionType.IonoParam;
        }

        /// <summary>
        /// 是否改正到相位上。
        /// </summary>
        public bool IsCorrectionOnPhase { get; set; }
        IonoParamService IonoParamService { get; set; }
        /// <summary>
        /// 特定的类型不同的改正，目前只支持GPS
        /// </summary>
        public SatelliteType SatelliteType { get; set; }

        public override void Correct(EpochSatellite epochSatellite)
        {
            if (IonoParamService == null) return; 


            IIonoParam ionParam = IonoParamService.Get(epochSatellite.ReceiverTime);

            if (ionParam == null)
            {
                return;
            }

            double correction = GetCorrectorInDistance(epochSatellite, ionParam);

            this.Correction = correction;
            if (IsCorrectionOnPhase)
            {
                this.Correction = -this.Correction;
            }

            // epochSatellite.SetCorrection(ErrorType, correction);
        }
        private const double PI = 3.1415926535898e0;  //*** exactly
        //private static double RAD = 180.0 / Consts.PI;      //*** radians to deg
        private static double GAMMA = Math.Pow(77.0 / 60.0, 2.0);//=γ12 = (fL1/fL2)2 = (1575.42/1227.6)2 = (77/60)2.
        private static double GAMMA1 = 1.0 - GAMMA;

        /// <summary>
        /// 电离层模型改正。返回改正距离。 单位：米
        /// </summary>
        /// <param name="epochSatellite">卫星观测信息</param>
        /// <param name="ionParam">电离层参数</param>
        /// <returns></returns>
        public static double GetCorrectorInDistance(EpochSatellite epochSatellite, IIonoParam ionParam)
        {
            if (ionParam == null) return 0;

            double correction = GetCorrectorInDistance(epochSatellite.RecevingTime.SecondsOfWeek,
                epochSatellite.Ephemeris.XYZ,
                epochSatellite.SiteInfo.EstimatedXyz,
                ionParam);
            return correction;
        }

        /// <summary>
        /// 电离层模型改正。返回改正距离。 单位：米
        /// Tiono is referred to the L1 frequency; if the user is operating on the L2 frequency, 
        /// the correction term must be multiplied by γ (reference paragraph 20.3.3.3.3.2),
        /// </summary>
        /// <param name="weekSecond">周秒</param>
        /// <param name="satXyz">卫星位置</param>
        /// <param name="receiverXyz">接收机位置</param>
        /// <param name="ionParam">电离层参数</param>
        /// <returns> </returns>
        public static double GetCorrectorInDistance(double weekSecond, XYZ satXyz, XYZ receiverXyz, IIonoParam ionParam)
        {
            if (ionParam == null) return 0;
            
            return GnssConst.LIGHT_SPEED * IonoCorrection(weekSecond, satXyz, receiverXyz, ionParam);
        }

        /// <summary>
        /// 电离层模型改正。返回时间延迟，单位：秒。
        /// broadcast iono (coeff.v. icd-200)
        /// input time:    fraction of ut (or gps time of secondOfWeek)      (machts nicht)
        /// </summary>
        /// <param name="weekSecond">周秒</param>
        /// <param name="satXyz">卫星位置</param>
        /// <param name="rcvXyz">接收机位置</param>
        /// <returns>time delay in fraction</returns>
        public static double IonoCorrection(double weekSecond, XYZ satXyz, XYZ rcvXyz, IIonoParam ionParam)
        {
            //*** divide by pi to funcKeyToDouble radians to semicircles
            Polar p = CoordTransformer.XyzToGeoPolar(satXyz, rcvXyz, AngleUnit.Radian); //myV.GetLocalPolar(rxPos);
            //azimuth angle between the user and satellite, measured clockwise positive from the true North (semi-circles)
            double A = p.Azimuth;  // 只参与三角计算，故单位为弧度radians here
            //elevation angle between the user and satellite (semi-circles)
            double E = p.Elevation / PI; //半周

            GeoCoord geo = CoordTransformer.XyzToGeoCoord(rcvXyz, AngleUnit.Radian);
            double rcvLat = geo.Lat / PI;//半周
            double rcvLon = geo.Lon / PI;//半周

            //earth's central angle between the user position and the earth projection of ionospheric intersection point (semi-circles)
            double psi = GetEarthsCentralAngle(E);

            double inoLat = GetIonLat(A, rcvLat, psi);
            double inoLon = GetIonLon(A, rcvLon, psi, inoLat);
            double ionMLat = GetMeanIonLat(inoLat, inoLon);

            //*** local time from gps or ut time
            double tlocal = GetLocalTime(weekSecond, inoLon);

            //*** magnification factor, amplitude, and period
            double f = GetF(E);
            double amp = GetAMP(ionParam, ionMLat);
            double x = GetX(ionParam, ionMLat, tlocal);

            return GetTimeDelay(f, amp, x);
        }

        #region 子算法细节
        /// <summary>
        /// 计算电离层时间延迟
        /// </summary>
        /// <param name="f"></param>
        /// <param name="amp"></param>
        /// <param name="x"></param>
        /// <returns></returns>
        private static double GetTimeDelay(double f, double amp, double x)
        {
            double delay = f * 5.0e-9;

            if (Math.Abs(x) < 1.57)
            {
                delay = delay + f * amp * (1.0 - Math.Pow(x, 2) / 2.0 + Math.Pow(x, 4) / 24.0);
            }
            return delay;
        }
        /// <summary>
        /// 计算X，模型参数之一。phase (radians)
        /// </summary>
        /// <param name="ionParam"></param>
        /// <param name="ionMLat">电离层穿刺点平均纬度，单位：半周</param>
        /// <param name="tlocal"></param>
        /// <returns></returns>
        private static double GetX(IIonoParam ionParam, double ionMLat, double tlocal)
        {
            double per = GetPER(ionParam, ionMLat);

            double x = 2.0 * PI * (tlocal - 50400.0) / per;
            return x;
        }
        /// <summary>
        /// 计算AMP，模型参数之一。amplitude
        /// </summary>
        /// <param name="ionParam"></param>
        /// <param name="ionMLat">电离层穿刺点平均纬度，单位：半周</param>
        /// <returns></returns>
        private static double GetAMP(IIonoParam ionParam, double ionMLat)
        {
            double amp = ionParam.AlfaA0 +
                 ionParam.AlfaA1 * ionMLat +
                  ionParam.AlfaA2 * ionMLat * ionMLat +
                  ionParam.AlfaA3 * ionMLat * ionMLat * ionMLat;
            if (amp < 0.0)
            {
                //*******throw new Exception("amp limit="+amp);
                amp = 0.0;
            }
            return amp;
        }
        /// <summary>
        /// 计算F，模型参数之一。
        /// obliquity factor (dimensionless)
        /// </summary>
        /// <param name="E">elevation angle between the user and satellite (semi-circles)</param>
        /// <returns></returns>
        private static double GetF(double E)
        {
            double f = 1.0 + 16.0 * Math.Pow(0.53 - E, 3);     //*** obliquity
            return f;
        }
        /// <summary>
        /// geomagnetic latitude of the earth projection of the ionospheric intersection point (mean ionospheric height assumed 350 km) (semi-circles)
        /// </summary>
        /// <param name="inoLat"></param>
        /// <param name="inoLon"></param>
        /// <returns></returns>
        private static double GetMeanIonLat(double inoLat, double inoLon)
        {
            double glam = inoLat + 0.064 * Math.Cos((inoLon - 1.617) * PI);
            return glam;
        }
        /// <summary>
        /// earth's central angle between the user position and the earth projection of ionospheric intersection point (semi-circles)
        /// </summary>
        /// <param name="E">卫星高度角，单位半周elevation angle between the user and satellite (semi-circles)</param>
        /// <returns>单位半周</returns>
        private static double GetEarthsCentralAngle(double E)
        {
            double psi = 0.0137 / (E + 0.11) - 0.022;
            return psi;
        }
        /// <summary>
        /// 获取电离层穿刺点经度，单位：半周
        /// </summary>
        /// <param name="A">此处应该为弧度才能计算。azimuth angle between the user and satellite, measured clockwise positive from the true North (semi-circles)</param>
        /// <param name="rcvLon"></param>
        /// <param name="psi"></param>
        /// <param name="inoLat">单位：半周</param>
        /// <returns>单位：半周</returns>
        private static double GetIonLon(double A, double rcvLon, double psi, double inoLat)
        {
            //geodetic longitude of the earth projection of the ionospheric intersection point (semi-circles)
            double inoLon = rcvLon + psi * Math.Sin(A) / Math.Cos(inoLat * PI);
            return inoLon;
        }
        /// <summary>
        /// 获取电离层穿刺点纬度,单位：半周
        /// </summary>
        /// <param name="A">只参与三角计算，故单位为弧度</param>
        /// <param name="rcvLat"></param>
        /// <param name="psi"></param>
        /// <returns>单位：半周</returns>
        private static double GetIonLat(double A, double rcvLat, double psi)
        {
            //geodetic latitude of the earth projection of the ionospheric intersection point (semi-circles)
            double inoLat = rcvLat + psi * Math.Cos(A);
            if (inoLat < -0.416) { inoLat = -0.416; }
            if (inoLat > 0.416) {inoLat = 0.416;}
            return inoLat;
        }

        /// <summary>
        /// local time (sec)
        /// </summary>
        /// <param name="weekSecond">GPS time</param>
        /// <param name="inoLon">geodetic longitude of the earth projection of the ionospheric intersection point (semi-circles)</param>
        /// <returns></returns>
        private static double GetLocalTime(double weekSecond, double inoLon)
        {
            double tlocal = 43200.0 * inoLon + weekSecond;
            while (tlocal < 0.0) { tlocal = tlocal + 86400.0; }
            while (tlocal >= 86400.0) { tlocal = tlocal - 86400.0; }
            return tlocal;
        }

        /// <summary>
        /// 计算PER，模型参数之一。period
        /// </summary>
        /// <param name="ionParam"></param>
        /// <param name="ionMLat">电离层穿刺点平均纬度，单位：半周</param>
        /// <returns></returns>
        private static double GetPER(IIonoParam ionParam, double ionMLat)
        {
            double per = ionParam.BetaB0 +
                 ionParam.BetaB1 * ionMLat +
                ionParam.BetaB2 * ionMLat * ionMLat +
                ionParam.BetaB3 * ionMLat * ionMLat * ionMLat;
            if (per < 72000.0)
            {
                per = 72000.0;
            }
            return per;
        }

        #endregion
    }

}
