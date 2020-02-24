//2014.09.15, czs, create, 设置卫星星历。
//2014.10.12, czs, edit in hailutu, 对星历赋值进行了重新设计，分解为几个不同的子算法
//2015.02.08, 崔阳, 卫星钟差和精密星历若同时存在，则不可分割


using System;
using System.IO;
using System.Collections.Generic;
using Gnsser.Domain;
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

namespace Gnsser
{
     
    /// <summary>
    /// satellite antenna offset correction
    /// </summary>
    public class SatAntOffEphemerisReviser : EphemerisReviser
    {
        /// <summary>
        /// satellite antenna offset correction
        /// </summary>
        /// <param name="DataSouceProvider"></param>
        public SatAntOffEphemerisReviser(DataSourceContext DataSouceProvider)
        {
            this.Name = "卫星天线改正";
            this.DataSouceProvider = DataSouceProvider;
        }

        DataSourceContext DataSouceProvider;

        public override bool Revise(ref IEphemeris eph)
        {
            XYZ dant1 = GetSatAntOff(eph.Prn, eph, eph.Time); 
            eph.XYZ = eph.XYZ + dant1; 
            return true;
        }

        /// <summary>
        /// 根据太阳计算卫星偏差
        /// </summary>
        /// <param name="prn"></param>
        /// <param name="eph"></param>
        /// <param name="emissionTime"></param>
        /// <returns></returns>
        private XYZ GetSatAntOff(SatelliteNumber prn, IEphemeris eph, Time emissionTime)
        {
            ErpItem erpv = null;
            if (DataSouceProvider.ErpDataService != null)
            {
                erpv = DataSouceProvider.ErpDataService.Get(emissionTime);
            }
            if (erpv == null) erpv =   ErpItem.Zero;
            XYZ rsun = new XYZ();

            //sun position in ecef
            //  rsun = EpochSat.EpochInfo.DataSouceProvider.UniverseObjectProvider.GetSunPosition(emissionTime);
            this.DataSouceProvider.UniverseObjectProvider.GetSunPosition(emissionTime, erpv, ref rsun);


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
            if (DataSouceProvider.AntennaDataSource == null)
            {
                return new XYZ();
            }

            IAntenna antenna = DataSouceProvider.AntennaDataSource.Get(prn.ToString(), emissionTime);
            //如果为空，则返回 0 坐标
            if (antenna == null)
            {
                return new XYZ();
            }

            // Get antenna eccentricity for frequency "G01" (L1), in
            // satellite reference system.
            // NOTE: It is NOT in ECEF, it is in UEN!!!
            RinexSatFrequency freq = new RinexSatFrequency(prn, 1);
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
