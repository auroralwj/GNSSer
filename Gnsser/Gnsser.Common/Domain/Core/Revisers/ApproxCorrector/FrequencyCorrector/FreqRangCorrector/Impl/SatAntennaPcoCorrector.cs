//2014.05.22, Cui Yang, created
//2018.05, lly, edit in zz, 增加多系统支持
//2018.08.03, czs, edit in hmx, 整理
//2018.09.27, czs, edit in hmx, 去掉了RINEX编号转换统一到 ObsCodeConvert 中。


using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Geo.Coordinates;
using Gnsser.Data.Rinex;
using Geo.Algorithm;
using Geo.Algorithm.Adjust;
using Gnsser.Service;
using Geo.Utils;
using Gnsser.Checkers;
using Geo.Common;
using Gnsser.Times;
using Gnsser.Data;
using Geo.Times;
using Gnsser.Correction;
using Gnsser.Domain;

namespace Gnsser.Correction
{
    /// <summary>
    /// 卫星PCO改正
    /// </summary>
    public class SatAntennaPcoCorrector : AbstractFreqBasedRangeCorrector
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="DataSouceProvider"></param>
        public SatAntennaPcoCorrector(DataSourceContext DataSouceProvider)
        {
            this.Name = "卫星PCO改正";
            this.Context = DataSouceProvider;
        }
        /// <summary>
        /// 上下文
        /// </summary>
        DataSourceContext Context { get; set; }
        /// <summary>
        /// 改正
        /// </summary>
        /// <param name="epochSatellite"></param>
        public override void Correct(EpochSatellite epochSatellite)
        {
            Dictionary<RinexSatFrequency, double> correction = new Dictionary<RinexSatFrequency, double>();

            IEphemeris sat = epochSatellite.Ephemeris;
            XYZ satPos = sat.XYZ;
            XYZ receiverPosition = epochSatellite.SiteInfo.EstimatedXyz;
            XYZ ray = satPos - receiverPosition;
            GeoCoord rcvGeoCoord = epochSatellite.SiteInfo.ApproxGeoCoord;

            int i = 0;

            List<RinexSatFrequency> frequences = epochSatellite.RinexSatFrequences;
            foreach (var item in frequences)
            {
                XYZ dant1 = GetSatAntOff(epochSatellite.Prn, item, epochSatellite.Ephemeris, epochSatellite.Ephemeris.Time);

                //Compute vector station-satellite, in ECEF

                //Rotate vector ray to UEN reference frame
                //此处修改为 NEU 坐标系。
                //NEU rayNeu = CoordTransformer.XyzToNeu(ray, rcvGeoCoord, AngleUnit.Degree);
                //double rangeCorretion = CoordUtil.GetDirectionLength(dant1, epochSatellite.Polar);
                //ray = XYZ.RotateZ(ray, lon);
                //ray = XYZ.RotateY(ray, -lat); 
                //Convert ray to an unitary vector
                //  XYZ xyzNeu = new XYZ(rayNeu.N, rayNeu.E, rayNeu.U);
                //NEU unit = rayNeu.UnitNeuVector();
                //计算沿着射线方向的改正数。Compute corrections = displacement vectors components along ray direction.
                XYZ unit2 = ray.UnitVector();
                double range2 = dant1.Dot(unit2);
                //double correctForL = dant1.Dot(unit);
                //double rang = dant1.Dot(unit);

                if (range2 != 0 )
                {
                    correction.Add(item, range2);
                }
                i++;
            }            
            this.Correction = (correction);

        }

        /// <summary>
        /// 根据太阳计算卫星偏差
        /// </summary>
        /// <param name="prn"></param>
        /// <param name="eph"></param>
        /// <param name="emissionTime"></param>
        /// <returns></returns>
        private XYZ GetSatAntOff(SatelliteNumber prn, RinexSatFrequency freq, IEphemeris eph, Time emissionTime)
        {
            ErpItem erpv = null;
            if (Context.ErpDataService != null)
            {
                erpv = Context.ErpDataService.Get(emissionTime);
            }
            if (erpv == null) erpv = ErpItem.Zero;
            XYZ rsun = new XYZ();

            //sun position in ecef
            //  rsun = EpochSat.EpochInfo.DataSouceProvider.UniverseObjectProvider.GetSunPosition(emissionTime);
            this.Context.UniverseObjectProvider.GetSunPosition(emissionTime, erpv, ref rsun);


            //unit vetcors of satellite fixed coordinates

            XYZ ez = -1 * eph.XYZ.UnitVector();

            XYZ es = (rsun - eph.XYZ).UnitVector();
            //outer product of 3D vectors
            XYZ r = new XYZ();
            r.X = ez.Y * es.Z - ez.Z * es.Y;
            r.Y = ez.Z * es.X - ez.X * es.Z;
            r.Z = ez.X * es.Y - ez.Y * es.X;



            XYZ r0 = new XYZ();
            r0.X = r.Y * ez.Z - r.Z * ez.Y;
            r0.Y = r.Z * ez.X - r.X * ez.Z;
            r0.Z = r.X * ez.Y - r.Y * ez.X;

            XYZ ex = r0.UnitVector();


            XYZ ey = r.UnitVector();


            //XYZ ex = new XYZ();

            //ex.X = ey.Y * ez.Z - ey.Z * ez.Y;
            //ex.Y = ey.Z * ez.X - ey.X * ez.Z;
            //ex.Z = ey.X * ez.Y - ey.Y * ez.X;


            //use L1 value
            if (Context.AntennaDataSource == null)
            {
                return new XYZ();
            }

            IAntenna antenna = Context.AntennaDataSource.Get(prn.ToString(), emissionTime);
            //如果为空，则返回 0 坐标
            if (antenna == null)
            {
                return new XYZ();
            }

            // Get antenna eccentricity for frequency "G01" (L1), in
            // satellite reference system.
            // NOTE: It is NOT in ECEF, it is in UEN!!!
            //lly注释
            //SatelliteFrequency freq = new SatelliteFrequency(prn, 1);
            // NEU satAnt = antenna.GetAntennaEccentricity(AntennaFrequency.G01);
            NEU satAnt = antenna.GetPcoValue(freq);

            XYZ dant = new XYZ();
            dant.X = satAnt.E * ex.X + satAnt.N * ey.X + satAnt.U * ez.X;
            dant.Y = satAnt.E * ex.Y + satAnt.N * ey.Y + satAnt.U * ez.Y;
            dant.Z = satAnt.E * ex.Z + satAnt.N * ey.Z + satAnt.U * ez.Z;


            // Unitary vector from satellite to Earth mass center (ECEF)
            XYZ satToEarthUnit = (-1.0) * eph.XYZ.UnitVector();

            // Unitary vector from Earth mass center to Sun (ECEF)
            XYZ earthToSunUnit = rsun.UnitVector();
            // rj = rk x ri: Rotation axis of solar panels (ECEF)
            XYZ rj = satToEarthUnit.Cross(earthToSunUnit);

            // Redefine ri: ri = rj x rk (ECEF)
            earthToSunUnit = rj.Cross(satToEarthUnit);
            // Let's funcKeyToDouble ri to an unitary vector. (ECEF)
            earthToSunUnit = earthToSunUnit.UnitVector();

            XYZ dant1 = new XYZ();

            dant1.X = satAnt.E * rj.X + satAnt.N * earthToSunUnit.X + satAnt.U * satToEarthUnit.X;
            dant1.Y = satAnt.E * rj.Y + satAnt.N * earthToSunUnit.Y + satAnt.U * satToEarthUnit.Y;
            dant1.Z = satAnt.E * rj.Z + satAnt.N * earthToSunUnit.Z + satAnt.U * satToEarthUnit.Z;

            return dant1;
        }
    }
}