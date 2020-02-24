
//2017.06.20, czs, funcKeyToDouble from c++ in hongqing, Exersise3.2_LunarEphemerides

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
    //   Exersise3.2_LunarEphemerides
    class Exersise3_2_LunarEphemerides
    {
        static void Main0(string[] args)
        { 

            // Constants

            const int N_Step = 8;
            const double Step = 0.5; // [d]


            // Sample Chebyshev coefficients from JPL DE405 for geocentric lunar 
            // coordinates in the ITRF(EME2000) frame valid from 2006/03/14 TDB 
            // to 2006/03/18 TDB 
             
            const double t1 = 53808.0;          // MJD at start of interval 
            const double t2 = 53812.0;          // MJD at end   of interval 
            double[] Cx_moon = {   // Coefficients for x-coordinate N_coeff
                     -0.383089044877016277e+06, 0.218158411754834669e+05, 0.179067292901463843e+05,
                     -0.836928063411765777e+02,-0.628266733052023696e+02,-0.459274434235101225e+00,
                      0.491167202819885532e-01, 0.770804039287614762e-03,-0.125935992206166816e-03,
                      0.500271026610991370e-05, 0.107044869185752331e-05, 0.172472464343636242e-08,
                     -0.269667589576924680e-08                                                   };
            double[] Cy_moon = {   // Coefficients for y-coordinate N_coeff
                     -0.379891721705081436e+05,-0.143611643157166138e+06, 0.187126702787245881e+04, 
                      0.112734362473135207e+04, 0.932891213817359177e+00,-0.191932684130578513e+01, 
                     -0.266517663331897990e-01, 0.104558913448630337e-02,-0.359077689123857890e-04, 
                     -0.123405162037249834e-04, 0.180479239596339495e-06, 0.525522632333670539e-07, 
                      0.543313967008773005e-09                                                   };
            double[] Cz_moon = {   // Coefficients for z-coordinate
                     -0.178496690739133737e+05,-0.788257550331743259e+05, 0.880684692614081882e+03, 
                      0.618395886330471512e+03, 0.103331218594995988e+01,-0.104949867328178592e+01,
                     -0.150337371962561087e-01, 0.569056416308259317e-03,-0.186297523286550968e-04,
                     -0.680012420653791955e-05, 0.902057208454410917e-07, 0.287891446432139173e-07, 
                      0.319822827699973363e-09                                                   };

            // Chebyshev approximation of lunar coordinates

            MultiDimChebshevFitter MoonCheb = new MultiDimChebshevFitter(t1, t2,                 // Order and interval
                                  new Vector(Cx_moon),    // Coefficients
                                  new Vector(Cy_moon),
                                  new Vector(Cz_moon));

            // Variables
            int i;
            double Mjd0, Mjd_TT;
            Vector r = new Vector(3);

            // Epoch
            Mjd0 = DateUtil.DateToMjd(2006, 03, 14, 00, 00, 0.0);

            // Output
            var info = "Exercise 3-2: Lunar Ephemerides " + "\r\n";
            info += " Moon position from low precision analytical theory" + "\r\n";
            info += " Date [TT]                 " + " Position [km] " + "\r\n";
            Console.WriteLine(info);
            for (i = 0; i <= N_Step; i++)
            {
                Mjd_TT = Mjd0 + i * Step;
                r = CelestialUtil.Moon(Mjd_TT) / 1000.0;

                info = " " + DateUtil.MjdToDateTimeString(Mjd_TT) +  " " +  r;

                Console.WriteLine(info);
            }

            info = " Moon position from DE405" + "\r\n";
            info += " Date [TT]                 " + " Position [km] " + "\r\n";
            Console.WriteLine(info);
            for (i = 0; i <= N_Step; i++)
            {
                Mjd_TT = Mjd0 + i * Step;
                r = MoonCheb.Fit(Mjd_TT);
                info = " " + DateUtil.MjdToDateTimeString(Mjd_TT) + " " + r;
                Console.WriteLine(info);
            }

            Console.ReadKey();
        }
    }
}