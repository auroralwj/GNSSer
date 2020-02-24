//2014.05.22, Cui Yang, created
//2014.08.19, czs , edit, 将 载波频率采用 AntennaFrequency 进行了参数化，支持多系统计算（取决于天线文件）
//2014.09.16, cy, 面向对象
//2014.10.05, czs, edit in hailutu, 纠正天线的获取，原改正的天线采用的是测站天线，而这里应该用卫星天线
//2016.09.25, czs, edit in hongqing, 指定数据源

using System;
using System.Text;
using Geo.Coordinates;
using Gnsser.Data.Rinex;
using Geo.Algorithm;
using Geo.Algorithm.Adjust;
using Gnsser.Service;
using Geo.Utils;
using Gnsser.Domain;
using Gnsser.Times;
using Gnsser.Data;
using Geo;
using Geo.Times;

namespace Gnsser.Correction
{
    /// <summary>
    ///计算卫星天线相位中心改正数。
    ///需要提供天线文件和卫星信息文件。
    ///算法同时内置了GPS卫星的默认配置。
    /// </summary>
    public class SatAntennaPhaseCenterCorrector : AbstractRangeCorrector
    {
        /// <summary>
        /// 构造函数
        /// </summary> 
        public SatAntennaPhaseCenterCorrector(AntennaFileService AntennaDataSource, IErpFileService ErpDataService, SatInfoService SatInfoService, UniverseObjectProvider UniverseObjectProvider)
        {
            this.Name = "卫星天线相位距离改正";
            this.ErpDataService = ErpDataService;
            this.SatInfoService = SatInfoService;
            this.UniverseObjectProvider = UniverseObjectProvider;
            this.AntennaDataSource = AntennaDataSource;
            this.CorrectionType = CorrectionType.SatAntennaPhaseCenter;
        }
        public AntennaFileService AntennaDataSource { get; set; }
        public UniverseObjectProvider UniverseObjectProvider { get; set; }
        public IErpFileService ErpDataService { get; set; }
        public SatInfoService SatInfoService { get; set; }
        public override void Correct(EpochSatellite epochSatellite)
        {
            if (AntennaDataSource == null || SatInfoService == null) { return; }

            Time gpsTime = epochSatellite.RecevingTime;


            //  XYZ sunPosition = epochSatellite.EpochInfo.DataSouceProvider.UniverseObjectProvider.GetSunPosition(gpsTime);// 这个可以每个历元算一次，有待优化。？？？czs 2014.10.05
            //下面是新的计算太阳位置
            Time tutc = gpsTime.GpstToUtc();

            //查找地球自转信息
            Gnsser.Data.ErpItem erpv = null;
            if (ErpDataService != null)
            {
                erpv = ErpDataService.Get(tutc);
            }
            if (erpv == null) erpv = ErpItem.Zero;

            XYZ sunPosition = new XYZ();
            UniverseObjectProvider.GetSunPosition(gpsTime, erpv, ref sunPosition);


            IEphemeris sat = epochSatellite.Ephemeris;

            XYZ svPos = sat.XYZ;

            XYZ receiverPosition = epochSatellite.SiteInfo.EstimatedXyz;

            SatelliteNumber prn = epochSatellite.Prn;

            SatInfoFile periodSatInfoCollection = SatInfoService.SatInfoFile;
            IAntenna antenna = AntennaDataSource.Get(prn.ToString(), gpsTime);

            double correction = GetSatPhaseCenterCorectValue(prn, gpsTime, svPos, receiverPosition, sunPosition, periodSatInfoCollection, antenna);

            this.Correction = (correction);//
        }

        /// <summary>
        /// 计算卫星相位中心改正值。
        /// </summary>
        /// <param name="satelliteType">卫星编号</param>
        /// <param name="time">历元</param>
        /// <param name="satPos">卫星位置</param>
        /// <param name="sunPosition">太阳位置</param>
        /// <param name="satData">Name of "PRN_GPS"-like file containing satellite satData.</param>
        /// <returns></returns>
        public static double GetSatPhaseCenterCorectValue(SatelliteNumber prn, Time time, XYZ satPos, XYZ ReceiverPosition, XYZ sunPosition, SatInfoFile satData, IAntenna antenna = null)
        {
            if (antenna == null) return 0;

            // This variable that will hold the correction, 0.0 by default
            double svPCcorr = 0.0;

            // Check is Antex antenna information is available or not, and if
            // available, whether satellite phase center information is absolute or relative
            bool absoluteModel = false;


            if (antenna != null)
            {
                absoluteModel = antenna.Header.IsAbsolute;
            }

            if (absoluteModel)
            {
                //方法1：参考GPSTK
                // svPCcorr = GetPhaseCorrection(satelliteType, satPos, ReceiverPosition, sunPosition, svPCcorr, antenna);

                //方法2：参考RTKLIB
                svPCcorr = GetPhaseCorrection1(prn, satPos, ReceiverPosition, sunPosition, svPCcorr, antenna);

            }
            else// 采用一些默认的参数。
            {
                svPCcorr = DefautSatPhaseCorrector(prn, time, satPos, ReceiverPosition, sunPosition, satData);
            }
            // This correction is interpreted as an "advance" in the signal,
            // instead of a delay. Therefore, it has negative sign
            return svPCcorr;
        }

        /// <summary>
        /// 卫星天线相位中心改正，参照GPSTK模块
        /// </summary>
        /// <param name="satelliteType"></param>
        /// <param name="satPos"></param>
        /// <param name="ReceiverPosition"></param>
        /// <param name="sunPosition"></param>
        /// <param name="svPCcorr"></param>
        /// <param name="antenna"></param>
        /// <returns></returns>
        private static double GetPhaseCorrection(SatelliteNumber prn, XYZ satPos, XYZ ReceiverPosition, XYZ sunPosition, double svPCcorr, IAntenna antenna)
        {
            // Unitary vector from satellite to Earth mass center (ECEF)
            XYZ satToEarthUnit = (-1.0) * satPos.UnitVector();

            // Unitary vector from Earth mass center to Sun (ECEF)
            XYZ earthToSunUnit = sunPosition.UnitVector();
            // rj = rk x ri: Rotation axis of solar panels (ECEF)
            XYZ rj = satToEarthUnit.Cross(earthToSunUnit);

            // Redefine ri: ri = rj x rk (ECEF)
            earthToSunUnit = rj.Cross(satToEarthUnit);
            // Let's funcKeyToDouble ri to an unitary vector. (ECEF)
            earthToSunUnit = earthToSunUnit.UnitVector();

            // Get vector from Earth mass center to receiver
            XYZ receiverPos = ReceiverPosition;

            // Compute unitary vector vector from satellite to RECEIVER
            XYZ satToReceverUnit = (receiverPos - satPos).UnitVector();

            // When not using Antex information, if satellite belongs to block
            // "IIR" its correction is 0.0, else it will depend on satellite model.

            // We will need the elevation, in degrees. It is found using
            // dot product and the corresponding unitary angles

            double cosa = satToReceverUnit.Dot(satToEarthUnit);
            cosa = cosa < -1.0 ? -1.0 : (cosa > 1.0 ? 1.0 : cosa);
            double nadir = Math.Acos(cosa) * CoordConsts.RadToDegMultiplier;

            if (!DoubleUtil.IsValid(nadir)) return 0;

            // The nadir angle should always smaller than 14.0 deg, 
            // but some times it's a bit bigger than 14.0 deg, we 
            // force it to 14.0 deg to stop throwing an exception.
            // The Reference is available at:
            // http://igscb.jpl.nasa.gov/igscb/resource/pubs/02_ott/session_8.pdf
            nadir = (nadir > 14) ? 14.0 : nadir;

            double elev = 90.0 - nadir;



            // Get antenna eccentricity for frequency "G01" (L1), in
            // satellite reference system.
            // NOTE: It is NOT in ECEF, it is in UEN!!!
            RinexSatFrequency freq = new RinexSatFrequency(prn, 1);
            // NEU satAnt = antenna.GetAntennaEccentricity(AntennaFrequency.G01);
            NEU satAnt = antenna.GetPcoValue(freq);
            if (satAnt.Equals(NEU.Zero))
                return 0;




            //Now, get the phase center variation.
            NEU var = new NEU(0,0, antenna.GetPcvValue(freq, elev));


            // We must substract them
            satAnt = satAnt - var;

            // Change to ECEF
            // 原 satAnt t is in UEN!!!,本satAnt为NEU，分量相对应
            // satAnt[0] = U
            // Triple svAntenna( satAnt[2]*ri + satAnt[1]*rj + satAnt[0]*rk );


            //  XYZ svAntenna = satAnt.N * earthToSunUnit + satAnt.E * rj + satAnt.U * satToEarthUnit;

            XYZ svAntenna = -(var.N * earthToSunUnit + var.E * rj + var.U * satToEarthUnit); 

            // Projection of "svAntenna" vector to line of sight vector rrho
            svPCcorr = (satToReceverUnit.Dot(svAntenna));
            return svPCcorr;
        }


        /// <summary>
        /// 卫星天线相位中心改正，参照RTKLIB
        /// </summary>
        /// <param name="satelliteType"></param>
        /// <param name="satPos"></param>
        /// <param name="ReceiverPosition"></param>
        /// <param name="sunPosition"></param>
        /// <param name="svPCcorr"></param>
        /// <param name="antenna"></param>
        /// <returns></returns>
        private static double GetPhaseCorrection1(SatelliteNumber prn, XYZ satPos, XYZ ReceiverPosition, XYZ sunPosition, double svPCcorr, IAntenna antenna)
        {

            XYZ ru = ReceiverPosition - satPos;
            XYZ rz = -satPos;


            double[] eu = new double[3]; eu[0] = ru.X / ru.Length; eu[1] = ru.Y / ru.Length; eu[2] = ru.Z / ru.Length;
            double[] ez = new double[3]; ez[0] = rz.X / rz.Length; ez[1] = rz.Y / rz.Length; ez[2] = rz.Z / rz.Length;

            double cosa = eu[0] * ez[0] + eu[1] * ez[1] + eu[2] * ez[2];
            cosa = cosa < -1.0 ? -1.0 : (cosa > 1.0 ? 1.0 : cosa);

            double nadir = Math.Acos(cosa) * CoordConsts.RadToDegMultiplier;


            //// The nadir angle should always smaller than 14.0 deg, 
            // // but some times it's a bit bigger than 14.0 deg, we 
            // // force it to 14.0 deg to stop throwing an exception.
            // // The Reference is available at:
            // // http://igscb.jpl.nasa.gov/igscb/resource/pubs/02_ott/session_8.pdf
            // nadir = (nadir > 14) ? 14.0 : nadir;

            double elev = 90.0 - nadir;

            // Get antenna eccentricity for frequency "G01" (L1), in
            // satellite reference system.
            // NOTE: It is NOT in ECEF, it is in UEN!!!
            RinexSatFrequency freq = new RinexSatFrequency(prn, 1);
            // NEU satAnt = antenna.GetAntennaEccentricity(AntennaFrequency.G01);
            NEU satAnt = antenna.GetPcoValue(freq);
            if (satAnt.Equals(NEU.Zero))
                return 0;

            //Now, get the phase center variation.
            var var = antenna.GetPcvValue(freq, elev); //只有U方向有值

            // Projection of "svAntenna" vector to line of sight vector rrho
           // svPCcorr = var.U;

            return var;
        }





        /// <summary>
        /// 默认的卫星天线相位改正。
        /// </summary>
        /// <param name="satelliteType"></param>
        /// <param name="time"></param>
        /// <param name="satPos"></param>
        /// <param name="sunPosition"></param>
        /// <returns></returns>
        public static double DefautSatPhaseCorrector(SatelliteNumber prn, Time time, XYZ satPos, XYZ ReceiverPosition, XYZ sunPosition, SatInfoFile satData)
        {

            // Unitary vector from satellite to Earth mass center (ECEF)
            XYZ satToEarthUnit = (-1.0) * satPos.UnitVector();

            // Unitary vector from Earth mass center to Sun (ECEF)
            XYZ earthToSunUnit = sunPosition.UnitVector();
            // rj = rk x ri: Rotation axis of solar panels (ECEF)
            XYZ rj = satToEarthUnit.Cross(earthToSunUnit);

            // Redefine ri: ri = rj x rk (ECEF)
            earthToSunUnit = rj.Cross(satToEarthUnit);
            // Let's funcKeyToDouble ri to an unitary vector. (ECEF)
            earthToSunUnit = earthToSunUnit.UnitVector();


            // Get vector from Earth mass center to receiver
            XYZ receiverPos = ReceiverPosition;

            // Compute unitary vector vector from satellite to RECEIVER
            XYZ satToReceverUnit = (receiverPos - satPos).UnitVector();

            // When not using Antex information, if satellite belongs to block
            // "IIR" its correction is 0.0, else it will depend on satellite model.

            // This variable that will hold the correction, 0.0 by default
            double svPCcorr = 0.0;

            // If no Antex information is given, or if phase center information
            // uses a relative model, then use a simpler, older approach

            // Please note that in this case all GLONASS satellite are
            // considered as having phase center at (0.0, 0.0, 0.0). The former
            // is not true for 'GLONASS-M' satellites (-0.545, 0.0, 0.0 ), but
            // currently there is no simple way to take this into account.

            // For satellites II and IIA:
            if ((satData.GetBlock(prn, time) == "II") || (satData.GetBlock(prn, time) == "IIA"))
            {
                //First, build satellite antenna vector for models II/IIA
                XYZ svAntenna = 0.279 * earthToSunUnit + 1.023 * satToEarthUnit;
                // Projection of "svAntenna" vector to line of sight vector rrho
                svPCcorr = (satToReceverUnit.Dot(svAntenna));
            }
            else
            {
                // For satellites belonging to block "I"
                if ((satData.GetBlock(prn, time) == "I"))
                {

                    // First, build satellite antenna vector for model I
                    XYZ svAntenna = (0.210 * earthToSunUnit + 0.854 * satToEarthUnit);

                    // Projection of "svAntenna" to line of sight vector (rrho)
                    svPCcorr = (satToReceverUnit.Dot(svAntenna));
                }
            }
            return svPCcorr;
        }


    }
}
