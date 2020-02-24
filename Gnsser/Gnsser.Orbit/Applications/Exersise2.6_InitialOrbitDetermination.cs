//2017.06.19, czs, funcKeyToDouble from c++ in hongqing, Exersise2_6_InitialOrbitDetermination
//2017.06.27, czs, edit in hongqing, format and refactor codes

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text; using Geo.Coordinates;

using Geo.Algorithm; using Geo.Utils; namespace Gnsser.Orbits
{
    /// <summary>
    /// 站点观测类型
    /// </summary>
    public struct ObsType
    {
        /// <summary>
        /// 站点观测极坐标数值类型。
        /// </summary>
        /// <param name="Mjd_UTC">历元</param>
        /// <param name="Azimuth">方位角</param>
        /// <param name="Elevation">高程，角度</param>
        /// <param name="Range">长度，距离</param>
        public ObsType(double Mjd_UTC, double Azim, double Elev, double Range)
        {
            this.Mjd_UTC = Mjd_UTC;
            this.Azimuth = Azim;
            this.Elevation = Elev;
            this.Range = Range;
        }
        /// <summary>
        /// 历元
        /// </summary>
        public double Mjd_UTC;
        /// <summary>
        /// 方位角
        /// </summary>
        public double Azimuth;
        /// <summary>
        /// 高程，角度
        /// </summary>
        public double Elevation;
        /// <summary>
        /// 长度，距离
        /// </summary>
        public double Range;
    }

    // 
    // Purpose: 
    //
    //   Satellite Orbits - Models, Methods, and Applications
    //   Exercise 2-6: Initial orbit determination
    class Exersise2_6_InitialOrbitDetermination
    {

        static void Main0(string[] args)
        {
            string endl = "\r\n";

            // Ground station

            Vector R_Sta = new Geo.Algorithm.Vector(+1344.143e3, +6068.601e3, +1429.311e3);  // Position vector
            GeoCoord Sta = CoordTransformer.XyzToGeoCoord(new XYZ(R_Sta.OneDimArray));//new GeoCoord(R_Sta, OrbitConsts.RadiusOfEarth, OrbitConsts.FlatteningOfEarth);// Geodetic coordinates

            // Observations
            ObsType[] Obs = new ObsType[]  {
                new ObsType(  DateUtil.DateToMjd(1999, 04, 02, 00, 30, 00.0), 132.67*OrbitConsts.RadPerDeg, 32.44*OrbitConsts.RadPerDeg, 16945.450e3 ),
		        new ObsType(  DateUtil.DateToMjd(1999, 04, 02, 03, 00, 00.0), 123.08*OrbitConsts.RadPerDeg, 50.06*OrbitConsts.RadPerDeg, 37350.340e3 )
            };

            // Variables

            int i, j;
            double Az, El, d;
            Geo.Algorithm.Vector s = new Geo.Algorithm.Vector(3);
            Geo.Algorithm.Vector[] r = new Geo.Algorithm.Vector[2];

            // Transformation to local tangent coordinates
            Matrix E = Sta.ToLocalNez_Matrix();

            // Convert observations
            for (i = 0; i < 2; i++)
            {
                // Earth rotation
                Matrix U = Matrix.RotateZ3D(IERS.GetGmstRad(Obs[i].Mjd_UTC));
                // Topocentric position vector
                Az = Obs[i].Azimuth; El = Obs[i].Elevation; d = Obs[i].Range;
                s = d * Geo.Algorithm.Vector.VecPolar(OrbitConsts.PI / 2 - Az, El);
                // Inertial position vector
                r[i] = U.Transpose() * (E.Transpose() * s + R_Sta);
            }

            // Orbital elements
            Geo.Algorithm.Vector Kep = Kepler.Elements(OrbitConsts.GM_Earth, Obs[0].Mjd_UTC, Obs[1].Mjd_UTC, r[0], r[1]);

            // Output
            var info = "Exercise 2-6: Initial orbit determination" + "\r\n";
            info += "Inertial positions:" + "\r\n";
            info += "                             ";
            info += "[km]" + "          [km]" + "          [km]";
            Console.WriteLine(info);
            for (i = 0; i < 2; i++)
            {
                info = "  " + DateUtil.MjdToDateTimeString(Obs[i].Mjd_UTC);

                for (j = 0; j < 3; j++) { info += " " + String.Format("{0, 12:F3}", r[i][j] / 1000.0); };
                Console.WriteLine(info);
            }
            Console.WriteLine();

            info = "Orbital elements:" + "\r\n"
                + "  Epoch (1st obs.)  " + DateUtil.MjdToDateTimeString(Obs[0].Mjd_UTC) + endl
                + "  Semimajor axis   " + String.Format("{0, 10:F3}", Kep[0] / 1000.0) + " km" + endl
                + "  Eccentricity     " + String.Format("{0, 10:F3}", Kep[1]) + endl
                + "  Inclination      " + String.Format("{0, 10:F3}", Kep[2] * OrbitConsts.DegPerRad) + " deg" + endl
                + "  RA ascend. node  " + String.Format("{0, 10:F3}", Kep[3] * OrbitConsts.DegPerRad) + " deg" + endl
                + "  Arg. of perigee  " + String.Format("{0, 10:F3}", Kep[4] * OrbitConsts.DegPerRad) + " deg" + endl
                + "  Mean anomaly     " + String.Format("{0, 10:F3}", Kep[5] * OrbitConsts.DegPerRad) + " deg" + endl;
            Console.WriteLine(info);
            Console.ReadKey();
        }
    }
}