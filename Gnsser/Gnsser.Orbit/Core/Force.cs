//2017.06.20, czs, funcKeyToDouble from c++ in hongqing, Force
//2017.06.26, czs, edit in hongqing, format and refactor codes

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text; using Geo.Coordinates;

using Geo.Algorithm; using Geo.Utils; namespace Gnsser.Orbits
{
   
    /// <summary>
    /// 力
    /// </summary>
    public class Force
    {
        public const double MJD_J2000 = OrbitConsts.MJD_J2000;// 51544.5;             // Modif. Julian Date of J2000.0
        public const double AU = OrbitConsts.AU;// 149597870000.0;      // Astronomical unit [m]; IAU 1976  

        // Local funtions 
        // Fractional part of a number (y=x-[x])
        private static double Frac(double x) { return MathUtil.Fraction(x); }
         
        private  static double sin(double val) { return Math.Sin(val); }
        private static double cos(double val) { return Math.Cos(val); }

        public static GravityModel Grav = GravityModel.Grav;

        /// <summary>
        /// 计算地球重力场引起的加速度。
        /// Computes the acceleration due to the harmonic gravity field of the central body
        /// </summary>
        /// <param name="satXyz"> r Satellite position vector in the inertial system</param>
        /// <param name="earthRotationMatrix"> E Transformation matrix to body-fixed system</param>
        /// <param name="GM"> GM Gravitational coefficient</param>
        /// <param name="R_ref">R_ref Reference radius </param>
        /// <param name="CS">CS Spherical harmonics coefficients (un-normalized)</param>
        /// <param name="n_max">n_max Maximum degree </param>
        /// <param name="m_max"> m_max Maximum order (m_max<=n_max; m_max=0 for zonals, only)</param>
        /// <returns>Acceleration (a=d^2r/dt^2)</returns>
        public static Geo.Algorithm.Vector AccelerOfHarmonicGraviFiled(Geo.Algorithm.Vector satXyz, Matrix earthRotationMatrix,
                              double GM, double radiusOfRefer, Matrix spherHarmoCoeff,
                              int maxDegree, int maxOrder)
        {
            // Local variables
            int n, m;                           // Loop counters
            double r_sqr, rho, Fac;               // Auxiliary quantities
            double x0, y0, z0;                      // Normalized coordinates
            double ax, ay, az;                      // Acceleration vector 
            double C, S;                           // Gravitational coefficients
            Geo.Algorithm.Vector r_bf = new Geo.Algorithm.Vector(3);                       // Body-fixed position
            Geo.Algorithm.Vector a_bf = new Geo.Algorithm.Vector(3);                       // Body-fixed acceleration

            Matrix V = new Matrix(maxDegree + 2, maxDegree + 2);            // Harmonic functions
            Matrix W = new Matrix(maxDegree + 2, maxDegree + 2);            // work array (0..n_max+1,0..n_max+1)

            // Body-fixed position 
            r_bf = earthRotationMatrix * satXyz;

            // Auxiliary quantities
            r_sqr = r_bf.Dot(r_bf);               // Square of distance
            rho = radiusOfRefer * radiusOfRefer / r_sqr;

            x0 = radiusOfRefer * r_bf[0] / r_sqr;          // Normalized
            y0 = radiusOfRefer * r_bf[1] / r_sqr;          // coordinates
            z0 = radiusOfRefer * r_bf[2] / r_sqr;

            // Evaluate harmonic functions 
            //   V_nm = (R_ref/r)^(n+1) * P_nm(sin(phi)) * cos(m*lambda)
            // and 
            //   W_nm = (R_ref/r)^(n+1) * P_nm(sin(phi)) * sin(m*lambda)
            // up to degree and order n_max+1
            //

            // Calculate zonal terms V(n,0); set W(n,0)=0.0
            V[0, 0] = radiusOfRefer / Math.Sqrt(r_sqr);
            W[0, 0] = 0.0;

            V[1, 0] = z0 * V[0, 0];
            W[1, 0] = 0.0;

            for (n = 2; n <= maxDegree + 1; n++)
            {
                V[n, 0] = ((2 * n - 1) * z0 * V[n - 1, 0] - (n - 1) * rho * V[n - 2, 0]) / n;
                W[n, 0] = 0.0;
            }

            // Calculate tesseral and sectorial terms 
            for (m = 1; m <= maxOrder + 1; m++)
            {
                // Calculate V(m,m) .. V(n_max+1,m)
                V[m, m] = (2 * m - 1) * (x0 * V[m - 1, m - 1] - y0 * W[m - 1, m - 1]);
                W[m, m] = (2 * m - 1) * (x0 * W[m - 1, m - 1] + y0 * V[m - 1, m - 1]);

                if (m <= maxDegree)
                {
                    V[m + 1, m] = (2 * m + 1) * z0 * V[m, m];
                    W[m + 1, m] = (2 * m + 1) * z0 * W[m, m];
                }

                for (n = m + 2; n <= maxDegree + 1; n++)
                {
                    V[n, m] = ((2 * n - 1) * z0 * V[n - 1, m] - (n + m - 1) * rho * V[n - 2, m]) / (n - m);
                    W[n, m] = ((2 * n - 1) * z0 * W[n - 1, m] - (n + m - 1) * rho * W[n - 2, m]) / (n - m);
                }
            }

            //
            // Calculate accelerations ax,ay,az
            //
            ax = ay = az = 0.0;

            for (m = 0; m <= maxOrder; m++)
                for (n = m; n <= maxDegree; n++)
                    if (m == 0)
                    {
                        C = spherHarmoCoeff[n, 0];   // = C_n,0
                        ax -= C * V[n + 1, 1];
                        ay -= C * W[n + 1, 1];
                        az -= (n + 1) * C * V[n + 1, 0];
                    }
                    else
                    {
                        C = spherHarmoCoeff[n, m];   // = C_n,m 
                        S = spherHarmoCoeff[m - 1, n]; // = S_n,m 
                        Fac = 0.5 * (n - m + 1) * (n - m + 2);
                        ax += +0.5 * (-C * V[n + 1, m + 1] - S * W[n + 1, m + 1])
                                + Fac * (+C * V[n + 1, m - 1] + S * W[n + 1, m - 1]);
                        ay += +0.5 * (-C * W[n + 1, m + 1] + S * V[n + 1, m + 1])
                                + Fac * (-C * W[n + 1, m - 1] + S * V[n + 1, m - 1]);
                        az += (n - m + 1) * (-C * V[n + 1, m] - S * W[n + 1, m]);
                    }

            // Body-fixed acceleration
            a_bf = (GM / (radiusOfRefer * radiusOfRefer)) * new Geo.Algorithm.Vector(ax, ay, az);

            // Inertial acceleration 
            return earthRotationMatrix.Transpose() * a_bf;
        }

        /// <summary>
        /// 计算由质点万有引力引起的扰动加速度。 Computes the perturbational acceleration due to a point mass
        /// </summary>
        /// <param name="satXyz"> r           Satellite position vector </param>
        /// <param name="massXyz"> s           Point mass position vector</param>
        /// <param name="GM"> GM          Gravitational coefficient of point mass</param>
        /// <returns> Acceleration (a=d^2r/dt^2)</returns>
        public static Geo.Algorithm.Vector AccelerOfPointMass(Geo.Algorithm.Vector satXyz, Geo.Algorithm.Vector massXyz, double GM)
        {
            //Relative position vector of satellite w.r.t. point mass 
            Geo.Algorithm.Vector d = satXyz - massXyz;

            // Acceleration 
            return (-GM) * (d / Math.Pow(d.Norm(), 3) + massXyz / Math.Pow(massXyz.Norm(), 3));
        }

        /// <summary>
        ///  计算由太阳光压引起的加速度。  Computes the acceleration due to solar radiation pressure assuming 
        ///   the spacecraft surface normal to the Sun direction
        ///   Notes:   r, r_sun, Area, mass, P0 and AU must be given in consistent units,
        ///   e.g. m, m^2, kg and N/m^2. 
        /// </summary>
        /// <param name="satXyz">r Spacecraft position vector </param>
        /// <param name="sunXyz"> r_Sun Sun position vector </param>
        /// <param name="Area">Area Cross-section </param>
        /// <param name="mass"> mass Spacecraft mass</param>
        /// <param name="solarRadiPressCoef"> coefOfRadiation Solar radiation pressure coefficient</param>
        /// <param name="solarRadiPressurePerAU">P0 Solar radiation pressure at 1 AU </param>
        /// <param name="AstroUnit">AU Length of one Astronomical Unit </param>
        /// <returns>Acceleration (a=d^2r/dt^2)</returns>
        public static Geo.Algorithm.Vector AccelerOfSolarRadiPressure(Geo.Algorithm.Vector satXyz, Geo.Algorithm.Vector sunXyz,
                            double Area, double mass, double solarRadiPressCoef,
                            double solarRadiPressurePerAU, double AstroUnit)
        {
            //Relative position vector of spacecraft w.r.t. Sun
            Geo.Algorithm.Vector d = satXyz - sunXyz;

            //Acceleration 
            return solarRadiPressCoef * (Area / mass) * solarRadiPressurePerAU * (AstroUnit * AstroUnit) * d / Math.Pow(d.Norm(), 3);
        }


        /// <summary>
        /// 计算大气延迟。 Computes the acceleration due to the atmospheric drag.
        /// </summary>
        /// <param name="Mjd_TT">Mjd_TT      Terrestrial Time (Modified Julian Date)</param>
        /// <param name="satXyz">r           Satellite position vector in the inertial system [m]</param>
        /// <param name="satVelocity"> v           Satellite velocity vector in the inertial system [m/s]</param>
        /// <param name="transMatrix"> T           Transformation matrix to true-of-date inertial system</param>
        /// <param name="Area">Area        Cross-section [m^2]</param>
        /// <param name="mass"> mass        Spacecraft mass [kg]</param>
        /// <param name="atmosDragCoeff"> coefOfDrag          Drag coefficient</param>
        /// <returns>Acceleration (a=d^2r/dt^2) [m/s^2]</returns>
        public static Geo.Algorithm.Vector AccelerDragOfAtmos(double Mjd_TT, Geo.Algorithm.Vector satXyz, Geo.Algorithm.Vector satVelocity,
                            Matrix transMatrix,  double Area, double mass, double atmosDragCoeff)
        {
            // Earth angular velocity vector [rad/s]
            double[] Data_omega = { 0.0, 0.0, 7.29212e-5 };
            Geo.Algorithm.Vector omega = new Geo.Algorithm.Vector(Data_omega, 3);

            // Variables
            double v_abs, dens;
            Geo.Algorithm.Vector r_tod = new Geo.Algorithm.Vector(3), v_tod = new Geo.Algorithm.Vector(3);
            Geo.Algorithm.Vector v_rel = new Geo.Algorithm.Vector(3), a_tod = new Geo.Algorithm.Vector(3);
            Matrix T_trp = new Matrix(3, 3);

            // Transformation matrix to ICRF/EME2000 system
            T_trp = transMatrix.Transpose();

            // Position and velocity in true-of-date system
            r_tod = transMatrix * satXyz;
            v_tod = transMatrix * satVelocity;

            // Velocity relative to the Earth's atmosphere
            v_rel = v_tod - omega.Cross3D(r_tod);
            v_abs = v_rel.Norm();

            // Atmospheric density due to modified Harris-Priester model
            dens = TerrestrialUtil. AtmosDensity_HP(Mjd_TT, r_tod);

            // Acceleration 
            a_tod = -0.5 * atmosDragCoeff * (Area / mass) * dens * v_abs * v_rel;

            return T_trp * a_tod;
        }

        


        /// <summary>
        /// 计算绕地航天器主要的加速度。
        ///   Computes the acceleration of an Earth orbiting satellite due to 
        ///    - the Earth's harmonic gravity field, 
        ///    - the gravitational perturbations of the Sun and Moon
        ///    - the solar radiation pressure and
        ///    - the atmospheric drag
        /// </summary>
        /// <param name="Mjd_TT"> Mjd_TT  Terrestrial Time (Modified Julian Date)</param>
        /// <param name="satXyzIcrf"> r 卫星在国际天球参考框架的位置  Satellite position vector in the ICRF/EME2000 system</param>
        /// <param name="satVelocityIcrf"> v 卫星在国际天球参考框架的速度 Satellite velocity vector in the ICRF/EME2000 system</param>
        /// <param name="Area">  Area    Cross-section </param>
        /// <param name="mass">  mass 质量  Spacecraft mass</param>
        /// <param name="solarRadiPresCoeff"> coefOfRadiation  光压系数  Radiation pressure coefficient</param>
        /// <param name="atmDragCoefficient"> coefOfDrag  大气阻力系数  Drag coefficient</param>
        /// <returns> Acceleration (a=d^2r/dt^2) in the ICRF/EME2000 system</returns> 
        public static Geo.Algorithm.Vector AccelerOfMainForces(double Mjd_TT, Geo.Algorithm.Vector satXyzIcrf, Geo.Algorithm.Vector satVelocityIcrf, double Area, double mass, double solarRadiPresCoeff, double atmDragCoefficient)
        {
            double Mjd_UT1;
            Geo.Algorithm.Vector a = new Geo.Algorithm.Vector(3), r_Sun = new Geo.Algorithm.Vector(3), r_Moon = new Geo.Algorithm.Vector(3);
            Matrix T = new Matrix(3, 3), E = new Matrix(3, 3);

            // Acceleration due to harmonic gravity field
            Mjd_UT1 = Mjd_TT;

            T = IERS.NutMatrix(Mjd_TT) * IERS.PrecessionMatrix(MJD_J2000, Mjd_TT);
            E = IERS.GreenwichHourAngleMatrix(Mjd_UT1) * T;

            a = AccelerOfHarmonicGraviFiled(satXyzIcrf, E, Grav.GM, Grav.R_ref, Grav.CS, Grav.n_max, Grav.m_max);

            // Luni-solar perturbations 
            r_Sun = CelestialUtil.Sun(Mjd_TT);
            r_Moon = CelestialUtil.Moon(Mjd_TT);

            a += AccelerOfPointMass(satXyzIcrf, r_Sun, OrbitConsts.GM_Sun);
            a += AccelerOfPointMass(satXyzIcrf, r_Moon, OrbitConsts.GM_Moon);

            // Solar radiation pressure
            a += CelestialUtil.Illumination(satXyzIcrf, r_Sun) * AccelerOfSolarRadiPressure(satXyzIcrf, r_Sun, Area, mass, solarRadiPresCoeff, OrbitConsts.PressureOfSolarRadiationPerAU, AU);

            // Atmospheric drag
            a += AccelerDragOfAtmos(Mjd_TT, satXyzIcrf, satVelocityIcrf, T, Area, mass, atmDragCoefficient);

            // Acceleration
            return a;
        }
    }
}