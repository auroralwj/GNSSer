//2017.06.20, czs, funcKeyToDouble from c++ in hongqing, Exersise3.4_OrbitPerturbations
//2017.06.27, czs, edit in hongqing, format and refactor codes


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text; using Geo.Coordinates;

using Geo.Algorithm; using Geo.Utils; namespace Gnsser.Orbits
{
    // Purpose: 
    //
    //   Satellite Orbits - Models, Methods, and Applications
    //   Exersise3.4_OrbitPerturbations
    class Exersise3_4_OrbitPerturbations
    {
        /// <summary>
        /// 导数、微分
        /// Computes the derivative of the state vector .
        ///   pAux is expected to point to a variable of type AuxDataRecord, which is
        ///   used to communicate with the other program sections and to hold satData 
        ///   between subsequent calls of this function  
        /// </summary>
        /// <param name="t">历元</param>
        /// <param name="y">卫星状态，坐标和速度</param> 
        /// <param name="pAux">附加选项</param>
        static Geo.Algorithm.Vector Deriv(double t, Geo.Algorithm.Vector y, Object pAux)
        {
            // Pointer to auxiliary satData record  
            ForceModelOption p = (ForceModelOption)pAux;// static_cast<ForceModelOption*>(pAux);

            // Time
            double Mjd_TT = p.Mjd0_TT + t / 86400.0;

            // State vector components
            Geo.Algorithm.Vector r = y.Slice(0, 2);
            Geo.Algorithm.Vector v = y.Slice(3, 5);

            // Acceleration 
            AccelerationCalculator calculator = new AccelerationCalculator(p, IERS);

            var a = calculator.GetAcceleration(Mjd_TT, r, v);

            // State vector derivative  
            Geo.Algorithm.Vector yp = v.Stack(a);
            return yp;
        }

        /// <summary>
        /// Ephemeris computation
        /// </summary>
        /// <param name="Y0"></param>
        /// <param name="N_Step"></param>
        /// <param name="Step"></param>
        /// <param name="p"></param>
        /// <param name="Eph"></param>
        static void Ephemeris(Geo.Algorithm.Vector Y0, int N_Step, double Step, ForceModelOption p, Geo.Algorithm.Vector[] Eph)
        {
            int i;
            double t, t_end;
            double relerr, abserr;        // Accuracy requirements
            DeIntegrator Orb = new DeIntegrator(Deriv, 6, p);// Object for integrating the eq. of motion
            Geo.Algorithm.Vector Y = new Geo.Algorithm.Vector(Y0);

            relerr = 1.0e-13;
            abserr = 1.0e-6;
            t = 0.0;
           // Y = Y0;
            Orb.Init(t, relerr, abserr);
            for (i = 0; i <= N_Step; i++)
            {
                t_end = Step * i;
               var Yresult=  Orb.Integ(t_end,  Y);
               Eph[i] = Yresult;
            };
            //var prevObj = Eph[0];
            //Console.WriteLine(prevObj);
            //var last = Eph[Eph.Length -1];
            //Console.WriteLine(last);
        }

        static double max(double a, double b) { return Math.Max(a, b); }
        static double Norm(Geo.Algorithm.Vector a) { return a.Norm(); }

        static IERS IERS;

        static void Main0(string[] args)
        {
            // Constants
            const int N_Step = 720;   // Maximum number of steps

            // Variables
            int i;
            int N_Step1;
            int N_Step2;
            double Step;
            double Mjd0_UTC;
            Geo.Algorithm.Vector Y0 = new Geo.Algorithm.Vector(6);
            Geo.Algorithm.Vector Kep = new Geo.Algorithm.Vector(6);
            ForceModelOption Aux_ref = new ForceModelOption(), Aux = new ForceModelOption();              // Auxiliary parameters
            double Max_J20, Max_J22, Max_J44, Max_J1010;
            double Max_Sun, Max_Moon, Max_SRad, Max_Drag;
            Geo.Algorithm.Vector[] Eph_Ref = new Geo.Algorithm.Vector[N_Step + 1];
            Geo.Algorithm.Vector[] Eph_J20 = new Geo.Algorithm.Vector[N_Step + 1];
            Geo.Algorithm.Vector[] Eph_J22 = new Geo.Algorithm.Vector[N_Step + 1];
            Geo.Algorithm.Vector[] Eph_J44 = new Geo.Algorithm.Vector[N_Step + 1];
            Geo.Algorithm.Vector[] Eph_J1010 = new Geo.Algorithm.Vector[N_Step + 1];
            Geo.Algorithm.Vector[] Eph_Sun = new Geo.Algorithm.Vector[N_Step + 1];
            Geo.Algorithm.Vector[] Eph_Moon = new Geo.Algorithm.Vector[N_Step + 1];
            Geo.Algorithm.Vector[] Eph_SRad = new Geo.Algorithm.Vector[N_Step + 1];
            Geo.Algorithm.Vector[] Eph_Drag = new Geo.Algorithm.Vector[N_Step + 1];

            // Initialize UT1-UTC and UTC-TAI time difference
            IERS = new IERS(-0.05, -30.00, 0.0, 0.0);

            // Epoch state (remote sensing satellite)
            Mjd0_UTC = DateUtil.DateToMjd(1999, 03, 01, 00, 00, 0.0);
            Kep = new Vector(7178.0e3, 0.0010, 98.57 * OrbitConsts.RadPerDeg, 0.0, 0.0, 0.0);
            Y0 = Kepler.State(OrbitConsts.GM_Earth, Kep, 0.0);

            // Model parameters
            Aux_ref.Mjd0_TT = Mjd0_UTC - IERS.GetTT_UTC(Mjd0_UTC) / 86400.0;
            Aux_ref.Area = 5.0;     // [m^2]  Remote sensing satellite
            Aux_ref.Mass = 1000.0;  // [kg]
            Aux_ref.CoefOfRadiation = 1.3;
            Aux_ref.CoefOfDrag = 2.3;
            Aux_ref.MaxDegree = 20;
            Aux_ref.MaxOrder = 20;
            Aux_ref.EnableSun = true;
            Aux_ref.EnableMoon = true;
            Aux_ref.EnableSolarRadiation = true;
            Aux_ref.EnableDrag = true;

            // Reference orbit

            Step = 120.0; // [s]
            N_Step1 = 50; // 100 mins
            N_Step2 = 720; // 1 day

            Aux = Aux_ref;
            Ephemeris(Y0, N_Step2, Step, Aux, Eph_Ref);

            // J2,0 perturbations
            Aux.MaxDegree = 2; Aux.MaxOrder = 0;
            
            Ephemeris(Y0, N_Step2, Step, Aux, Eph_J20);

            // J2,2 perturbations
            Aux.MaxDegree = 2; Aux.MaxOrder = 2;
            Ephemeris(Y0, N_Step2, Step, Aux, Eph_J22);

            // J4,4 perturbations
            Aux.MaxDegree = 4; Aux.MaxOrder = 4;
            Ephemeris(Y0, N_Step2, Step, Aux, Eph_J44);

            // J10,10 perturbations
            Aux.MaxDegree = 10; Aux.MaxOrder = 10;
            Ephemeris(Y0, N_Step2, Step, Aux, Eph_J1010);
            Aux.MaxDegree = 20; Aux.MaxOrder = 20;

            // Solar perturbations
            Aux.EnableSun = false;
            Ephemeris(Y0, N_Step2, Step, Aux, Eph_Sun);
            Aux.EnableSun = true;

            // Lunar perturbations
            Aux.EnableMoon = false;
            Ephemeris(Y0, N_Step2, Step, Aux, Eph_Moon);
            Aux.EnableMoon = true;

            // Solar radiation pressure perturbations
            Aux.EnableSolarRadiation = false;
            Ephemeris(Y0, N_Step2, Step, Aux, Eph_SRad);
            Aux.EnableSolarRadiation = true;

            // Drag perturbations
            Aux.EnableDrag = false;
            Ephemeris(Y0, N_Step2, Step, Aux, Eph_Drag);
            Aux.EnableDrag = true;

            // Find maximum over N_Step1 steps
            Max_J20 = Max_J22 = Max_J44 = Max_J1010 = Max_Sun = Max_Moon = Max_SRad = Max_Drag = 0.0;
            for (i = 0; i <= N_Step1; i++)
            {
                var refEph = Eph_Ref[i].Slice(0, 2);
                Max_J20 = max(Norm(Eph_J20[i].Slice(0, 2) - refEph), Max_J20);
                Max_J22 = max(Norm(Eph_J22[i].Slice(0, 2) - refEph), Max_J22);
                Max_J44 = max(Norm(Eph_J44[i].Slice(0, 2) - refEph), Max_J44);
                Max_J1010 = max(Norm(Eph_J1010[i].Slice(0, 2) - refEph), Max_J1010);
                Max_Sun = max(Norm(Eph_Sun[i].Slice(0, 2) - refEph), Max_Sun);
                Max_Moon = max(Norm(Eph_Moon[i].Slice(0, 2) - refEph), Max_Moon);
                Max_SRad = max(Norm(Eph_SRad[i].Slice(0, 2) - refEph), Max_SRad);
                Max_Drag = max(Norm(Eph_Drag[i].Slice(0, 2) - refEph), Max_Drag);
            };

            // Output
            var info = "Exercise 3-4: Orbit Perturbations " + "\r\n" + "\r\n";
            info += "Remote sensing satellite: " + "\r\n" + "\r\n";
            info += "  Maximum position errors within ";
            Console.Write(info);
            info = String.Format("{0, 8:F}", N_Step1 * Step / 60.0);
            info += " min propagation interval " + "\r\n";
            info += "    J2,0    J2,2    J4,4  J10,10" + "     Sun    Moon  SolRad    Drag" + "\r\n";
            info += "     [m]     [m]     [m]     [m]" + "     [m]     [m]     [m]     [m]";
            Console.WriteLine(info);
            info = String.Format("{0, 8:F}{1, 8:F}{2, 8:F}{3, 8:F}{4, 8:F}{5, 8:F}{6, 8:F}{7, 8:F}", Max_J20, Max_J22, Max_J44, Max_J1010, Max_Sun, Max_Moon, Max_SRad, Max_Drag);
            Console.WriteLine(info);
            Console.WriteLine();

            // Find maximum over N_Step2 steps
            for (i = N_Step1 + 1; i <= N_Step2; i++)
            {
                var refEph = Eph_Ref[i].Slice(0, 2);
                Max_J20 = max(Norm(Eph_J20[i].Slice(0, 2) - refEph), Max_J20);
                Max_J22 = max(Norm(Eph_J22[i].Slice(0, 2) - refEph), Max_J22);
                Max_J44 = max(Norm(Eph_J44[i].Slice(0, 2) - refEph), Max_J44);
                Max_J1010 = max(Norm(Eph_J1010[i].Slice(0, 2) - refEph), Max_J1010);
                Max_Sun = max(Norm(Eph_Sun[i].Slice(0, 2) - refEph), Max_Sun);
                Max_Moon = max(Norm(Eph_Moon[i].Slice(0, 2) - refEph), Max_Moon);
                Max_SRad = max(Norm(Eph_SRad[i].Slice(0, 2) - refEph), Max_SRad);
                Max_Drag = max(Norm(Eph_Drag[i].Slice(0, 2) - refEph), Max_Drag);
            };

            // Output
            info = "  Maximum position errors within ";
            Console.Write(info);
            info = String.Format("{0, 8:F}", N_Step2 * Step / 60.0);
            info += " min propagation interval " + "\r\n";
            info += "    J2,0    J2,2    J4,4  J10,10" + "     Sun    Moon  SolRad    Drag" + "\r\n";
            info += "     [m]     [m]     [m]     [m]" + "     [m]     [m]     [m]     [m]";
            Console.WriteLine(info);
            info = String.Format("{0, 8:F}{1, 8:F}{2, 8:F}{3, 8:F}{4, 8:F}{5, 8:F}{6, 8:F}{7, 8:F}", Max_J20, Max_J22, Max_J44, Max_J1010, Max_Sun, Max_Moon, Max_SRad, Max_Drag);
            Console.WriteLine(info);
            Console.WriteLine();             

            // Epoch state (geostationary satellite)
            Kep = new Vector(42166.0e3, 0.0004, 0.02 * OrbitConsts.RadPerDeg, 0.0, 0.0, 0.0);
            Y0 = Kepler.State(OrbitConsts.GM_Earth, Kep, 0.0);

            // Model parameters
            Aux_ref.Area = 10.0;    // [m^2]  Geostationary satellite

            // Reference orbit
            Step = 1200.0; // [s]
            N_Step1 = 72; // 1 day
            N_Step2 = 144; // 2 days 

            Aux = Aux_ref;
            Ephemeris(Y0, N_Step2, Step, Aux, Eph_Ref);

            // J2,0 perturbations
            Aux.MaxDegree = 2; Aux.MaxOrder = 0;
            Ephemeris(Y0, N_Step2, Step, Aux, Eph_J20);

            // J2,2 perturbations
            Aux.MaxDegree = 2; Aux.MaxOrder = 2;
            Ephemeris(Y0, N_Step2, Step, Aux, Eph_J22);

            // J4,4 perturbations
            Aux.MaxDegree = 4; Aux.MaxOrder = 4;
            Ephemeris(Y0, N_Step2, Step, Aux, Eph_J44);

            // J10,10 perturbations
            Aux.MaxDegree = 10; Aux.MaxOrder = 10;
            Ephemeris(Y0, N_Step2, Step, Aux, Eph_J1010);
            Aux.MaxDegree = 20; Aux.MaxOrder = 20;

            // Solar perturbations
            Aux.EnableSun = false;
            Ephemeris(Y0, N_Step2, Step, Aux, Eph_Sun);
            Aux.EnableSun = true;

            // Lunar perturbations
            Aux.EnableMoon = false;
            Ephemeris(Y0, N_Step2, Step, Aux, Eph_Moon);
            Aux.EnableMoon = true;

            // Solar radiation pressure perturbations
            Aux.EnableSolarRadiation = false;
            Ephemeris(Y0, N_Step2, Step, Aux, Eph_SRad);
            Aux.EnableSolarRadiation = true;

            // Drag perturbations
            Aux.EnableDrag = false;
            Ephemeris(Y0, N_Step2, Step, Aux, Eph_Drag);
            Aux.EnableDrag = true;

            // Find maximum over N_Step1 steps

            Max_J20 = Max_J22 = Max_J44 = Max_J1010 = Max_Sun = Max_Moon = Max_SRad = Max_Drag = 0.0;
            for (i = 0; i <= N_Step1; i++)
            {
                var refEph = Eph_Ref[i].Slice(0, 2);
                Max_J20 = max(Norm(Eph_J20[i].Slice(0, 2) - refEph), Max_J20);
                Max_J22 = max(Norm(Eph_J22[i].Slice(0, 2) - refEph), Max_J22);
                Max_J44 = max(Norm(Eph_J44[i].Slice(0, 2) - refEph), Max_J44);
                Max_J1010 = max(Norm(Eph_J1010[i].Slice(0, 2) - refEph), Max_J1010);
                Max_Sun = max(Norm(Eph_Sun[i].Slice(0, 2) - refEph), Max_Sun);
                Max_Moon = max(Norm(Eph_Moon[i].Slice(0, 2) - refEph), Max_Moon);
                Max_SRad = max(Norm(Eph_SRad[i].Slice(0, 2) - refEph), Max_SRad);
                Max_Drag = max(Norm(Eph_Drag[i].Slice(0, 2) - refEph), Max_Drag);
            };

            // Output
            // Output
            info = "Geostationary satellite: " + "\r\n";
            info += "  Maximum position errors within ";
            Console.Write(info);
            info = String.Format("{0, 8:F}", N_Step1 * Step / 60.0);
            info += " min propagation interval " + "\r\n";
            info += "    J2,0    J2,2    J4,4  J10,10" + "     Sun    Moon  SolRad    Drag" + "\r\n";
            info += "     [m]     [m]     [m]     [m]" + "     [m]     [m]     [m]     [m]";
            Console.WriteLine(info);
            info = String.Format("{0, 8:F}{1, 8:F}{2, 8:F}{3, 8:F}{4, 8:F}{5, 8:F}{6, 8:F}{7, 8:F}", Max_J20, Max_J22, Max_J44, Max_J1010, Max_Sun, Max_Moon, Max_SRad, Max_Drag);
            Console.WriteLine(info);
            Console.WriteLine();

            // Find maximum over N_Step2 steps

            for (i = N_Step1 + 1; i <= N_Step2; i++)
            {
                var refEph = Eph_Ref[i].Slice(0, 2);
                Max_J20 = max(Norm(Eph_J20[i].Slice(0, 2) - refEph), Max_J20);
                Max_J22 = max(Norm(Eph_J22[i].Slice(0, 2) - refEph), Max_J22);
                Max_J44 = max(Norm(Eph_J44[i].Slice(0, 2) - refEph), Max_J44);
                Max_J1010 = max(Norm(Eph_J1010[i].Slice(0, 2) - refEph), Max_J1010);
                Max_Sun = max(Norm(Eph_Sun[i].Slice(0, 2) - refEph), Max_Sun);
                Max_Moon = max(Norm(Eph_Moon[i].Slice(0, 2) - refEph), Max_Moon);
                Max_SRad = max(Norm(Eph_SRad[i].Slice(0, 2) - refEph), Max_SRad);
                Max_Drag = max(Norm(Eph_Drag[i].Slice(0, 2) - refEph), Max_Drag);
            };

            // Output
            info = "  Maximum position errors within ";
            Console.Write(info);
            info = String.Format("{0, 8:F}", N_Step2 * Step / 60.0);
            info += " min propagation interval " + "\r\n";
            info += "    J2,0    J2,2    J4,4  J10,10" + "     Sun    Moon  SolRad    Drag" + "\r\n";
            info += "     [m]     [m]     [m]     [m]" + "     [m]     [m]     [m]     [m]";
            Console.WriteLine(info);
            info = String.Format("{0, 8:F}{1, 8:F}{2, 8:F}{3, 8:F}{4, 8:F}{5, 8:F}{6, 8:F}{7, 8:F}", Max_J20, Max_J22, Max_J44, Max_J1010, Max_Sun, Max_Moon, Max_SRad, Max_Drag);
            Console.WriteLine(info);
            Console.WriteLine();


            Console.ReadKey();
        }
    }
}