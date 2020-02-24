
//2017.06.21, czs, funcKeyToDouble from c++ in hongqing, Exersise5.3_GeodeticCoordinates
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text; using Geo.Coordinates;

using Geo.Algorithm; using Geo.Utils; namespace Gnsser.Orbits
{

    // 
    // Purpose: 
    //
    //   Satellite Orbits - Models, Methods, and Applications
    //   Exersise5.3_GeodeticCoordinates
    class Exersise5_3_GeodeticCoordinates
    {
        static void Main0(string[] args)
        {
            // WGS84 geoid parameters
            const double R_WGS84 = 6378.137e3;      // Radius Earth [m]
            const double f_WGS84 = 1.0 / 298.257223563; // Flattening   

            // Variables

            Geo.Algorithm.Vector R_Sta = new Geo.Algorithm.Vector(3);   // Cartesian station coordinates
            GeoCoord Sta;     // Station coordinates

            // Header 
            var endl = "\r\n";
            var info = "Exercise 5-3: Geodetic coordinates" + endl;
            Console.WriteLine(info);
            // Coordinates of NIMA GPS station at Diego Garcia (WGS84(G873); epoch 1997.0)
            R_Sta = new Geo.Algorithm.Vector(1917032.190, 6029782.349, -801376.113);   // [m]

            // Geodetic coordinates

            Sta = CoordTransformer.XyzToGeoCoord(new XYZ(R_Sta.OneDimArray));//, R_WGS84, f_WGS84);

            // Output
            info = "Cartesian station coordinates (WGS84) [m]" + endl + endl
                 + Sta.ToXyzVector(R_WGS84, f_WGS84) + endl + endl;
            Console.WriteLine(info);

            info = "Geodetic station coordinates (WGS84)" + endl + endl
                 + " longitude " + String.Format("{0, 12:F8}", OrbitConsts.DegPerRad * Sta.Lon) + " deg" + endl
                 + " latitude  " + String.Format("{0, 12:F8}", OrbitConsts.DegPerRad * Sta.Lat) + " deg" + endl
                 + " height    " + String.Format("{0, 12:F5}", Sta.Height) + " m" + endl;
            Console.WriteLine(info);

            Console.ReadKey();
        }
    }
}