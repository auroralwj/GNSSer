//2014.05.22, Cui Yang, created
//2014.08.19, czs, edit, 将 载波频率采用 AntennaFrequency 进行了参数化，支持多系统计算（取决于天线文件）
//2014.09.16, cy, 面向对象
//2015.03.16, cy, 增加天线类型变量AntennaType

using System;
using System.Text;
using Geo.Coordinates;
using Gnsser.Data.Rinex;
using Geo.Algorithm;
using Geo.Algorithm.Adjust;
using Gnsser.Service;
using Geo.Utils;
using Gnsser.Domain;
using System.Collections.Generic;
using Gnsser.Times;
using Geo.Times;
using Gnsser.Data;
using Geo;

namespace Gnsser.Correction
{
    /// <summary>
    ///计算相位缠绕改正(phase wind-up)。可以带来厘米级别的差异，不影响基线，影响PPP，一个测站只应初始化一次。
    ///卫星发射右旋极化(RCP)的电磁波信号, GPS相位观测值的大小同卫星信号发射天线和GPS接收机接收天线间的方位有关。
    /// </summary>
    public class PhaseWindUpCorrector : AbstractPhaseCorrector
    {
        /// <summary>
        /// 构造函数
        /// </summary> 
        public PhaseWindUpCorrector(SatInfoFile satData, DataSourceContext DataSouceProvider)
        {
            this.Name = "相位缠绕距离改正";
            this.CorrectionType = CorrectionType.PhaseWindUp;
            this.SatInfoService = satData;
            this.PhaseManager = new PhaseManager();
            this.DataSouceProvider = DataSouceProvider;
        }
        DataSourceContext DataSouceProvider;
        /// <summary>
        /// 相位管理
        /// </summary>
        PhaseManager PhaseManager;
        /// <summary>
        /// 卫星状态集合。
        /// </summary>
        SatInfoFile SatInfoService;


        public override void Correct(EpochSatellite epochSatellite)
        {
            Time gpsTime = epochSatellite.RecevingTime;

            IEphemeris sat = epochSatellite.Ephemeris;
            SatelliteNumber prn = epochSatellite.Prn;

            //计算太阳位置方法
            //XYZ sunPosition = epochSatellite.EpochInfo.DataSouceProvider.UniverseObjectProvider.GetSunPosition(gpsTime);

            //新的计算太阳位置方法
            Time tutc = gpsTime.GpstToUtc();
            //查找地球自转信息
            Gnsser.Data.ErpItem erpv = null;
            if (DataSouceProvider.ErpDataService != null)
            {
                erpv = DataSouceProvider.ErpDataService.Get(tutc);
            }
            if (erpv == null) erpv = ErpItem.Zero;

            XYZ sunPosition = new XYZ();
            DataSouceProvider.UniverseObjectProvider.GetSunPosition(gpsTime, erpv, ref sunPosition);

            //use L1 value
            IAntenna antenna = DataSouceProvider.AntennaDataSource.Get(prn.ToString(), gpsTime);

            if (antenna == null)
            {
                return;
            }

            string AntennaType = antenna.AntennaType;

            XYZ svPos = sat.XYZ;

            XYZ receiverPosition = epochSatellite.SiteInfo.EstimatedXyz;
            if (receiverPosition.Equals(XYZ.Zero)) return;

            bool cycleSlip = epochSatellite.IsUnstable;

            if (cycleSlip || !PhaseManager.Contains(prn)) //a cycle slip happend
            {
                PhaseManager[prn] = new SatVectorPhase(); 
            }
             
            double windUpCorrection = GetSatPhaseWindUpCorectValue(prn, gpsTime, svPos, receiverPosition, sunPosition, AntennaType);

            //double windUpCorrection2 = GetSatPhaseWindUpCorectValue(satelliteType, gpsTime, svPos, receiverPosition, epochSatellite, sunPosition);
             
            this.Correction = (windUpCorrection);  
        }

        /// <summary>
        /// 天线缠绕改正 方法2 参考RTKLIB
        /// 李林阳添加 2015.01.01
        /// </summary>
        /// <param name="prn"></param>
        /// <param name="time"></param>
        /// <param name="satPos"></param>
        /// <param name="receiverPos"></param>
        /// <param name="epochSatellite"></param>
        /// <param name="sunPos"></param>
        /// <returns></returns>
        private double GetSatPhaseWindUpCorectValue(SatelliteNumber prn, Time time, XYZ satPos, XYZ receiverPos, EpochSatellite epochSatellite, XYZ sunPos)
        {
            #region
            XYZ rxPos = new XYZ(receiverPos.X, receiverPos.Y, receiverPos.Z);
            //李林阳加天线相位缠绕
            /* unit vector satellite to receiver */
            XYZ ek = (rxPos - satPos).UnitVector(); ;//接收机位置-卫星位置,单位化
            // for (time=0;time<3;time++) r[time]=rr[time]-rs[time];
            //if (!normv3(r, ek)) return; //单位化

            /* unit vectors of satellite antenna */
            //for (time = 0; time < 3; time++) r[time] = -rs[time];
            //if (!normv3(r, ezs)) return;
            XYZ ezs = (-1.0 * satPos).UnitVector();//卫星位置的单位向量

            //for (time = 0; time < 3; time++) r[time] = rsun[time] - rs[time];
            //if (!normv3(r, ess)) return;
            XYZ ess = (sunPos - satPos).UnitVector();

            //cross3(ezs, ess, r);
            //if (!normv3(r, eys)) return;
            XYZ eys = ezs.Cross(ess);
            eys = eys.UnitVector();

            //cross3(eys, ezs, exs);
            XYZ exs = eys.Cross(ezs);

            /* unit vectors of receiver antenna */
            GeoCoord geoCoord = epochSatellite.SiteInfo.EstimatedGeoCoord;
            //ecef2pos(rr, pos);
            //xyz2enu(pos, E);
            //exr[0] = E[1]; exr[1] = E[4]; exr[2] = E[7]; /* x = north */
            //eyr[0] = -E[0]; eyr[1] = -E[3]; eyr[2] = -E[6]; /* y = west  */
            double Lat = geoCoord.Lat * CoordConsts.DegToRadMultiplier;
            double Lon = geoCoord.Lon * CoordConsts.DegToRadMultiplier;
            //geoCoord.Lat = geoCoord.Lat / CoordConsts.RadToDegMultiplier;
            //geoCoord.Lon = geoCoord.Lon / CoordConsts.RadToDegMultiplier;
            double[] exr = new double[3];
            double[] eyr = new double[3];
            double cosB = Math.Cos(Lat);
            double sinB = Math.Sin(Lat);

            double cosL = Math.Cos(Lon);
            double sinL = Math.Sin(Lon);
            exr[0] = -sinB * cosL; exr[1] = -sinB * sinL; exr[2] = cosB; /* x = north */
            eyr[0] = sinL; eyr[1] = -cosL; eyr[2] = 0; /* y = west  */
            XYZ exr_ = new XYZ();
            exr_.X = exr[0]; exr_.Y = exr[1]; exr_.Z = exr[2];

            XYZ eyr_ = new XYZ();
            eyr_.X = eyr[0]; eyr_.Y = eyr[1]; eyr_.Z = eyr[2];

            /* phase windup effect */
            //cross3(ek, eys, eks);
            //cross3(ek, eyr, ekr);

            XYZ eks = ek.Cross(eys);
            XYZ ekr = ek.Cross(eyr_);

            double[] ds = new double[3];
            double[] dr = new double[3];
            for (int i = 0; i < 3; i++)
            {
                //ds[time] = exs[time] - ek[time] * dot(ek, exs, 3) - eks[time];
                //dr[time] = exr[time] - ek[time] * dot(ek, exr, 3) + ekr[time];
                ds[i] = exs[i] - ek[i] * ek.Dot(exs) - eks[i];
                dr[i] = exr[i] - ek[i] * ek.Dot(exr_) + ekr[i];
            }

            XYZ ds_ = new XYZ(ds);
            XYZ dr_ = new XYZ(dr); 

            //cosp = dot(ds, dr, 3) / norm(ds, 3) / norm(dr, 3);
            double cosp = ds_.Dot(dr_) / ds_.Length / dr_.Length;
            if (cosp < -1.0) cosp = -1.0;
            else if (cosp > 1.0) cosp = 1.0;

            //ph = acos(cosp) / 2.0 / PI;
            double ph = Math.Acos(cosp) / 2.0 / Math.PI;

            //cross3(ds, dr, drs);
            XYZ drs = ds_.Cross(dr_);
            //if (dot(ek, drs, 3) < 0.0) ph = -ph;
            if (ek.Dot(drs) < 0.0) ph = -ph;

            //*phw = ph + floor(*phw - ph + 0.5); /* in cycle */
            //double wind_up = ph + Math.Floor(wind_up - ph + 0.5);
            PhaseManager[prn].CorrectionValue = ph + Math.Floor(PhaseManager[prn].CorrectionValue - ph + 0.5);
            double wind_up = PhaseManager[prn].CorrectionValue * 2 * Math.PI;
            //要处理异常IIR卫星，参见GPSTK，2015.02.08
            //if (SatInfoService.GetBlock(satelliteType, time) == "IIR")
            //{
            //    if (wind_up > 0)
            //    {
            //        wind_up -= Math.PI;
            //    }

            //    else
            //    {
            //        wind_up += Math.PI;
            //    }
            //}
            //已验证这种方法的精度，但对于IIR型卫星，如果处理，还未实现，因此该模型暂时不能用。
            #endregion

            return wind_up;
        }

        /// <summary>
        /// 计算天线缠绕改正，单位：弧度。
        /// </summary> 
        /// <param name="prn">卫星编号</param>
        /// <param name="time">时间</param>
        /// <param name="satPos">卫星位置</param>
        /// <param name="receiverPos">接收机位置</param>
        /// <param name="sunPos">太阳位置</param> 
        /// <returns></returns>
        private double GetSatPhaseWindUpCorectValue(SatelliteNumber prn, Time time, XYZ satPos, XYZ receiverPos, XYZ sunPos, string AntennaType)
        {
            //Vector from SV to Sun center of mass
            XYZ gps_sun = sunPos - satPos;


            //Unitary vector from satellite to Earth mass center
            XYZ rk = (-1.0) * satPos.UnitVector();


            //rj=rk * gps_sun, then make sure it is unitary
            XYZ rj = (rk.Cross(gps_sun)).UnitVector();



            //Define ri: ri= rj * rk, then make sure it is unitary
            //Now, ri, rj, rk form a base in the satellite body reference frame, expressed in the ECEF reference frame
            XYZ ri = (rj.Cross(rk)).UnitVector();


            // Get satellite rotation angle

            // Get vector from Earth mass center to receiver
            XYZ rxPos = new XYZ(receiverPos.X, receiverPos.Y, receiverPos.Z);
            // Compute unitary vector vector from satellite to RECEIVER
            XYZ rrho = (rxPos - satPos).UnitVector();

            // Projection of "rk" vector to line of sight vector (rrho)
            double zk = rrho.Dot(rk);

            // Get a vector without components on rk (time.e., belonging
            // to ri, rj plane)
            XYZ dpp = (rrho - zk * rk);

            // Compute dpp components in ri, rj plane
            double xk = (dpp.Dot(ri));
            double yk = (dpp.Dot(rj));

            //Compute satellite rotation angle, in radians
            double alpha1 = Math.Atan2(yk, xk);
            // Get receiver rotation angle

            // Redefine rk: Unitary vector from Receiver to Earth mass center
            rk = (-1.0) * (rxPos.UnitVector());

            // Let's define a NORTH unitary vector in the Up, East, North
            // (UEN) topocentric reference frame
            XYZ delta = new XYZ(0.0, 0.0, 1.0);

            // Rotate delta to XYZ reference frame
            GeoCoord nomNEU = Geo.Coordinates.CoordTransformer.XyzToGeoCoord(receiverPos);
            delta = XYZ.RotateY(delta, nomNEU.Lat);
            delta = XYZ.RotateZ(delta, -nomNEU.Lon);


            // Computation of reference trame unitary vectors for receiver
            // rj = rk x delta, and make it unitary
            rj = (rk.Cross(delta)).UnitVector();

            // ri = rj x rk, and make it unitary
            ri = (rj.Cross(rk)).UnitVector();

            // Projection of "rk" vector to line of sight vector (rrho)
            zk = rrho.Dot(rk);

            // Get a vector without components on rk (time.e., belonging
            // to ri, rj plane)
            dpp = rrho - zk * rk;
            // Compute dpp components in ri, rj plane
            xk = dpp.Dot(ri);
            yk = dpp.Dot(rj);

            // Compute receiver rotation angle, in radians
            double alpha2 = Math.Atan2(yk, xk);

            double wind_up = 0.0;
            // Find out if satellite belongs to block "IIR", because
            // satellites of block IIR have a 180 phase shift
            if (SatInfoService.GetBlock(prn, time) == "IIR")
            {
                wind_up = Math.PI;// 3.1415926535898;//PI
            }
            //if (AntennaType.Contains("IIR") && SatInfoService.GetBlock(prn, time) != "IIR")
            //{
            //    wind_up += 0;
            //}


            alpha1 = alpha1 + wind_up;

            SatVectorPhase satVecPhase = PhaseManager[prn]; 


            double da1 = alpha1 - satVecPhase.PhaseOfSatellite;
            double da2 = (alpha2 - satVecPhase.PhaseOfReceiver);
            //double da1 = alpha1 - phase_satellite[satid].PreviousPhase;
            //double da2 = (alpha2 - phase_station[satid].PreviousPhase);


            // Let's avoid problems when passing from 359 to 0 degrees.
            double tmp1 = satVecPhase.PhaseOfSatellite;
            tmp1 += Math.Atan2(Math.Sin(da1), Math.Cos(da1));
            satVecPhase.PhaseOfSatellite = tmp1;

            double tmp2 = satVecPhase.PhaseOfReceiver;
            tmp2 += Math.Atan2(Math.Sin(da2), Math.Cos(da2));
            satVecPhase.PhaseOfReceiver = tmp2;

            // Compute wind up effect in radians
            wind_up = satVecPhase.PhaseOfSatellite -
                     satVecPhase.PhaseOfReceiver;

            // Let's avoid problems when passing from 359 to 0 degrees.
            //PhaseData tmp1 = phase_satellite[satid];
            //tmp1.PreviousPhase += Math.Atan2(Math.Sin(da1), Math.Cos(da1));
            //phase_satellite[satid] = tmp1;

            //PhaseData tmp2 = phase_station[satid];
            //tmp2.PreviousPhase += Math.Atan2(Math.Sin(da2), Math.Cos(da2));
            //phase_station[satid] = tmp2;

            //// Compute wind up effect in radians
            //wind_up = phase_satellite[satid].PreviousPhase -
            //          phase_station[satid].PreviousPhase;


            return wind_up;
        }


    }
}
