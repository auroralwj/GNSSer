//2017.06.18, czs, funcKeyToDouble from c++ in hongqing, Kepler
//2017.06.25, czs, edit in hongqing, format and refactor codes
//2018.10.14, czs, edit in hmx, 进行了简单的整理


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text; using Geo.Coordinates;
using Geo.Algorithm;
using Geo.Utils;

namespace Gnsser.Orbits
{
    /// <summary>
    /// 开普勒轨道计算。
    /// </summary>
    public class Kepler
    {
        // Machine accuracy

        const double eps_mach = Geo.Utils. OrbitConsts.MinDigitalResolution;// 1.0e-15; /* smallest such that 1.0+DBL_EPSILON != 1.0 *///double.Epsilon;// 1.0e-15;
 
        /// <summary>
        /// 解算计算开普勒方程，求偏近点角。Computes the eccentric anomaly for elliptic orbits
        /// </summary>
        /// <param name="M">M         Mean anomaly in [rad]</param>
        /// <param name="e"> e         Eccentricity of the orbit [0,1[</param>
        /// <returns> Eccentric anomaly in [rad]</returns>
        public static double EccAnom(double M, double e)
        {
            // Constants
            const int maxit = 20;
            const double eps = 100.0 * eps_mach;

            // Variables
            int i = 0;
            double E, f;

            // Starting value
            M = MathUtil.Modulo(M, 2.0 * OrbitConsts.PI);
            if (e < 0.8) E = M; else E = OrbitConsts.PI;

            // Iteration
            do
            {
                f = E - e * Math.Sin(E) - M;
                E = E - f / (1.0 - e * Math.Cos(E));
                ++i;
                if (i == maxit)
                {
                    Console.Error.WriteLine(" convergence problems in EccAnom");
                    break;
                }
            }
            while (Math.Abs(f) > eps);

            return E;
        }

        #region  计算细节
    
        /// <summary>
        /// 从两个位置向量和中间时间计算扇区三角形比率    
        ///   Computes the sector-triangle ratio from two position vectors and 
        ///   the intermediate time 
        /// </summary>
        /// <param name="r_a">r_a        Position at time t_a</param>
        /// <param name="r_b">Position at time t_b</param>
        /// <param name="tau">   Normalized time (Math.Sqrt(GM)*(t_a-t_b))</param>
        /// <returns> Sector-triangle ratio</returns>
        private static double FindEta(Geo.Algorithm.Vector r_a, Geo.Algorithm.Vector r_b, double tau)
        {
            // Constants
            const int maxit = 30;
            const double delta = 100.0 * eps_mach;

            // Variables
            int i;
            double kappa, m, l, s_a, s_b, eta_min, eta1, eta2, F1, F2, d_eta;


            // Auxiliary quantities
            s_a = r_a.Norm();
            s_b = r_b.Norm();

            kappa = Math.Sqrt(2.0 * (s_a * s_b + r_a.Dot(r_b)));

            m = tau * tau / Math.Pow(kappa, 3);
            l = (s_a + s_b) / (2.0 * kappa) - 0.5;

            eta_min = Math.Sqrt(m / (l + 1.0));

            // Start with Hansen's approximation
            eta2 = (12.0 + 10.0 * Math.Sqrt(1.0 + (44.0 / 9.0) * m / (l + 5.0 / 6.0))) / 22.0;
            eta1 = eta2 + 0.1;

            // Secant method
            F1 = F(eta1, m, l);
            F2 = F(eta2, m, l);

            i = 0;
            while (Math.Abs(F2 - F1) > delta)
            {
                d_eta = -F2 * (eta2 - eta1) / (F2 - F1);
                eta1 = eta2; F1 = F2;
                while (eta2 + d_eta <= eta_min) d_eta *= 0.5;
                eta2 += d_eta;
                F2 = F(eta2, m, l); ++i;

                if (i == maxit)
                {
                    throw new ArgumentException("WARNING: Convergence problems in FindEta");
                    break;
                }
            }

            return eta2;
        }

        /// <summary>
        /// local function for use by FindEta() F = 1 - eta +(m/eta**2)*W(m/eta**2-l)
        /// </summary>
        /// <param name="eta"></param>
        /// <param name="m"></param>
        /// <param name="l"></param>
        /// <returns></returns>
       static  private double F(double eta, double m, double l)
        {
            // Constants
            const double eps = 100.0 * eps_mach;

            // Variables
            double w, W, a, n, g;

            w = m / (eta * eta) - l;

            if (Math.Abs(w) < 0.1)
            { // Series expansion
                W = a = 4.0 / 3.0; n = 0.0;
                do
                {
                    n += 1.0; a *= w * (n + 2.0) / (n + 1.5); W += a;
                }
                while (Math.Abs(a) >= eps);
            }
            else
            {
                if (w > 0.0)
                {
                    g = 2.0 * Math.Asin(Math.Sqrt(w));
                    W = (2.0 * g - Math.Sin(2.0 * g)) / Math.Pow(Math.Sin(g), 3);
                }
                else
                {
                    g = 2.0 * Math.Log(Math.Sqrt(-w) + Math.Sqrt(1.0 - w));  // =2.0*arsinh(Math.Sqrt(-w))
                    W = (Math.Sinh(2.0 * g) - 2.0 * g) / Math.Pow(Math.Sinh(g), 3);
                }
            }

            return (1.0 - eta + (w + l) * W);
        }
        #endregion
         
        /// <summary>
        /// 由开普勒卫星轨道根数计算卫星状态,前三位置，后三速度。Computes the satellite state vector from osculating Keplerian elements 
        ///   for elliptic orbits  
        /// Notes:
        ///   The semimajor axis a=Kep(0), dt and GM must be given in consistent units, 
        ///   e.g. [m], [s] and [m^3/s^2]. The resulting units of length and velocity  
        ///   are implied by the units of GM, e.g. [m] and [m/s].
        /// </summary>
        /// <param name="GM"> GM        Gravitational coefficient    (gravitational constant * mass of central body)</param>
        /// <param name="kepElements">Kep       Keplerian elements (a,e,i,Omega,omega,M) with 
        /// a      Semimajor axis  
        /// e      Eccentricity 
        /// i      Inclination [rad] 
        /// Omega  Longitude of the ascending node [rad]   
        /// omega  Argument of pericenter  [rad]
        ///   M      Mean anomaly at epoch [rad]
        ///    dt        Time since epoch
        ///    
        /// </param>
        /// <param name="dt"></param>
        /// <returns>State vector (x,y,z,vx,vy,vz)</returns>
        public static Geo.Algorithm.Vector State(double GM, Geo.Algorithm.Vector kepElements, double dt = 0)
        {
            // Variables
            double a, e, i, Omega, omega, M, M0, n;
            double E, cosE, sinE, fac, R, V;
            Geo.Algorithm.Vector r = new Geo.Algorithm.Vector(3), v = new Geo.Algorithm.Vector(3);
            Matrix PQW = new Matrix(3, 3);

            // Keplerian elements at epoch
            a = kepElements[0]; Omega = kepElements[3];
            e = kepElements[1]; omega = kepElements[4];
            i = kepElements[2]; M0 = kepElements[5];

            // Mean anomaly
            if (dt == 0.0)
            {
                M = M0;
            }
            else
            {
                n = Math.Sqrt(GM / (a * a * a));
                M = M0 + n * dt;
            };

            // Eccentric anomaly
            E = EccAnom(M, e);

            cosE = Math.Cos(E);
            sinE = Math.Sin(E);

            // Perifocal coordinates
            fac = Math.Sqrt((1.0 - e) * (1.0 + e));

            R = a * (1.0 - e * cosE);  // Distance
            V = Math.Sqrt(GM * a) / R;    // Velocity

            r = new Geo.Algorithm.Vector(a * (cosE - e), a * fac * sinE, 0.0);
            v = new Geo.Algorithm.Vector(-V * sinE, +V * fac * cosE, 0.0);

            // Transformation to reference system (Gaussian vectors)
            PQW = Matrix.RotateZ3D(-Omega) * Matrix.RotateX3D(-i) * Matrix.RotateZ3D(-omega);

            r = PQW * r;
            v = PQW * v;

            // State vector 
            return r.Stack(v);
        }


        //------------------------------------------------------------------------------
        //
        // StatePartials
        //
        // Purpose:
        //
        //   Computes the partial derivatives of the satellite state vector with respect
        //   to the orbital elements for elliptic, Keplerian orbits
        //
        // Input/Output:
        //
        //   GM        Gravitational coefficient
        //             (gravitational constant * mass of central body)
        //   Kep       Keplerian elements (a,e,i,Omega,omega,M) at epoch with
        //               a      Semimajor axis 
        //               e      Eccentricity 
        //               i      Inclination [rad]
        //               Omega  Longitude of the ascending node [rad]
        //               omega  Argument of pericenter  [rad]
        //               M      Mean anomaly at epoch [rad]
        //   dt        Time since epoch
        //   <return>  Partials derivatives of the state vector (x,y,z,vx,vy,vz) at time
        //             dt with respect to the epoch orbital elements
        //
        // Notes:
        //
        //   The semimajor axis a=Kep(0), dt and GM must be given in consistent units, 
        //   e.g. [m], [s] and [m^3/s^2]. The resulting units of length and velocity  
        //   are implied by the units of GM, e.g. [m] and [m/s].
        //
        //   The function cannot be used with circular or non-inclined orbit.
        //
        //------------------------------------------------------------------------------
        /// <summary>
        /// 计算卫星状态向量相对于椭圆轨道元素的偏导数
        /// </summary>
        /// <param name="GM"></param>
        /// <param name="kepElements"></param>
        /// <param name="dt"></param>
        /// <returns></returns>
        private static Matrix StatePartials(double GM, Geo.Algorithm.Vector kepElements, double dt)
        {
            // Variables
            int k;
            double a, e, i, Omega, omega, M, M0, n, dMda;
            double E, cosE, sinE, fac, r, v, x, y, vx, vy;
            Matrix PQW = new Matrix(3, 3);
            Geo.Algorithm.Vector P = new Geo.Algorithm.Vector(3), Q = new Geo.Algorithm.Vector(3), W = new Geo.Algorithm.Vector(3), e_z = new Geo.Algorithm.Vector(3), N = new Geo.Algorithm.Vector(3);
            Geo.Algorithm.Vector dPdi = new Geo.Algorithm.Vector(3), dPdO = new Geo.Algorithm.Vector(3), dPdo = new Geo.Algorithm.Vector(3), dQdi = new Geo.Algorithm.Vector(3), dQdO = new Geo.Algorithm.Vector(3), dQdo = new Geo.Algorithm.Vector(3);
            Geo.Algorithm.Vector dYda = new Geo.Algorithm.Vector(6), dYde = new Geo.Algorithm.Vector(6), dYdi = new Geo.Algorithm.Vector(6), dYdO = new Geo.Algorithm.Vector(6), dYdo = new Geo.Algorithm.Vector(6), dYdM = new Geo.Algorithm.Vector(6);
            Matrix dYdA = new Matrix(6, 6);

            // Keplerian elements at epoch
            a = kepElements[0]; Omega = kepElements[3];
            e = kepElements[1]; omega = kepElements[4];
            i = kepElements[2]; M0 = kepElements[5];

            // Mean and eccentric anomaly

            n = Math.Sqrt(GM / (a * a * a));
            M = M0 + n * dt;
            E = EccAnom(M, e);

            // Perifocal coordinates
            cosE = Math.Cos(E);
            sinE = Math.Sin(E);
            fac = Math.Sqrt((1.0 - e) * (1.0 + e));

            r = a * (1.0 - e * cosE);  // Distance
            v = Math.Sqrt(GM * a) / r;    // Velocity

            x = +a * (cosE - e); y = +a * fac * sinE;
            vx = -v * sinE; vy = +v * fac * cosE;

            // Transformation to reference system (Gaussian vectors) and partials

            PQW = Matrix.RotateZ3D(-Omega) * Matrix.RotateX3D(-i) * Matrix.RotateZ3D(-omega);

            P = PQW.Col(0); Q = PQW.Col(1); W = PQW.Col(2);

            e_z = new Geo.Algorithm.Vector(0, 0, 1); N = e_z.Cross3D(W); N = N / N.Norm();

            dPdi = N.Cross3D(P); dPdO = e_z.Cross3D(P); dPdo = Q;
            dQdi = N.Cross3D(Q); dQdO = e_z.Cross3D(Q); dQdo = -P;

            // Partials w.r.t. semimajor axis, eccentricity and mean anomaly at time dt
            dYda = ((x / a) * P + (y / a) * Q).Stack((-vx / (2 * a)) * P + (-vy / (2 * a)) * Q);

            dYde = (((-a - Math.Pow(y / fac, 2) / r) * P + (x * y / (r * fac * fac)) * Q).Stack(
                           (vx * (2 * a * x + e * Math.Pow(y / fac, 2)) / (r * r)) * P
                           + ((n / fac) * Math.Pow(a / r, 2) * (x * x / r - Math.Pow(y / fac, 2) / a)) * Q));

            dYdM = ((vx * P + vy * Q) / n).Stack((-n * Math.Pow(a / r, 3)) * (x * P + y * Q));

            // Partials w.r.t. inlcination, node and argument of pericenter
            dYdi = (x * dPdi + y * dQdi).Stack(vx * dPdi + vy * dQdi);
            dYdO = (x * dPdO + y * dQdO).Stack(vx * dPdO + vy * dQdO);
            dYdo = (x * dPdo + y * dQdo).Stack(vx * dPdo + vy * dQdo);

            // Derivative of mean anomaly at time dt w.r.t. the semimajor axis at epoch
            dMda = -1.5 * (n / a) * dt;

            // Combined partial derivative matrix of state with respect to epoch elements
            for (k = 0; k < 6; k++)
            {
                dYdA[k, 0] = dYda[k] + dYdM[k] * dMda;
                dYdA[k, 1] = dYde[k];
                dYdA[k, 2] = dYdi[k];
                dYdA[k, 3] = dYdO[k];
                dYdA[k, 4] = dYdo[k];
                dYdA[k, 5] = dYdM[k];
            }

            return dYdA;
        }



        //------------------------------------------------------------------------------
        //
        // Elements
        //
        // Purpose:
        //
        //   Computes the osculating Keplerian elements from the satellite state vector
        //   for elliptic orbits
        //
        // Input/Output:
        //
        //   GM        Gravitational coefficient
        //             (gravitational constant * mass of central body)
        //   y         State vector (x,y,z,vx,vy,vz)
        //   <return>  Keplerian elements (a,e,i,Omega,omega,M) with
        //               a      Semimajor axis 
        //               e      Eccentricity 
        //               i      Inclination [rad]
        //               Omega  Longitude of the ascending node [rad]
        //               omega  Argument of pericenter  [rad]
        //               M      Mean anomaly  [rad]
        //
        // Notes:
        //
        //   The state vector and GM must be given in consistent units, 
        //   e.g. [m], [m/s] and [m^3/s^2]. The resulting unit of the semimajor
        //   axis is implied by the unity of y, e.g. [m].
        //
        //   The function cannot be used with state vectors describing a circular
        //   or non-inclined orbit.
        //
        //------------------------------------------------------------------------------
        /// <summary>
        /// 由某点的位置和速度，求解开普勒轨道根数。
        /// </summary>
        /// <param name="GM"> Gravitational coefficien<t/param>
        /// <param name="y"> State vector (x,y,z,vx,vy,vz) </param>
        /// <returns></returns>
        public static  Geo.Algorithm.Vector Elements(double GM, Geo.Algorithm.Vector stateVector)
        {
            // Variables
            Geo.Algorithm.Vector r = new Geo.Algorithm.Vector(3), v = new Geo.Algorithm.Vector(3), h = new Geo.Algorithm.Vector(3);
            double H, u, R;
            double eCosE, eSinE, e2, E, nu;
            double a, e, i, Omega, omega, M;

            r = stateVector.Slice(0, 2);                                  // Position
            v = stateVector.Slice(3, 5);                                  // Velocity

            h = r.Cross3D(v);                                    // Areal velocity
            H = h.Norm();

            Omega = Math.Atan2(h[0], -h[1]);                     // Long. ascend. node 
            Omega = MathUtil.Modulo(Omega, OrbitConsts.TwoPI);
            i = Math.Atan2(Math.Sqrt(h[0] * h[0] + h[1] * h[1]), h[2]); // Inclination        
            u = Math.Atan2(r[2] * H, -r[0] * h[1] + r[1] * h[0]);    // Arg. of latitude   

            R = r.Norm();                                      // Distance 
            a = 1.0 / (2.0 / R - v.Dot(v) / GM);                     // Semi-major axis  
            eCosE = 1.0 - R / a;                                   // e*cos(E)           
            eSinE = r.Dot(v) / Math.Sqrt(GM * a);                       // e*Math.Sin(E)          

            e2 = eCosE * eCosE + eSinE * eSinE;
            e = Math.Sqrt(e2);                                     // Eccentricity 
            E = Math.Atan2(eSinE, eCosE);                           // Eccentric anomaly  

            M = MathUtil.Modulo(E - eSinE, OrbitConsts.TwoPI);                          // Mean anomaly
            nu = Math.Atan2(Math.Sqrt(1.0 - e2) * eSinE, eCosE - e2);          // True anomaly
            omega = MathUtil.Modulo(u - nu, OrbitConsts.TwoPI);                          // Arg. of perihelion 

            // Keplerian elements vector
            return new Geo.Algorithm.Vector(a, e, i, Omega, omega, M);
        }


        //------------------------------------------------------------------------------
        //
        // Elements 
        //
        // Purpose:
        //
        //   Computes orbital elements from two given position vectors and 
        //   associated times 
        //
        // Input/Output:
        //
        //   GM        Gravitational coefficient
        //             (gravitational constant * mass of central body)
        //   Mjd_a     Time t_a (Modified Julian Date)
        //   Mjd_b     Time t_b (Modified Julian Date)
        //   r_a       Position vector at time t_a
        //   r_b       Position vector at time t_b
        //   <return>  Keplerian elements (a,e,i,Omega,omega,M)
        //               a      Semimajor axis 
        //               e      Eccentricity 
        //               i      Inclination [rad]
        //               Omega  Longitude of the ascending node [rad]
        //               omega  Argument of pericenter  [rad]
        //               M      Mean anomaly  [rad]
        //             at time t_a 
        //
        // Notes:
        //
        //   The function cannot be used with state vectors describing a circular
        //   or non-inclined orbit.
        //
        //------------------------------------------------------------------------------
        /// <summary>
        /// 计算轨道根数，由两个已知位置。
        /// </summary>
        /// <param name="GM"></param>
        /// <param name="Mjd_a"></param>
        /// <param name="Mjd_b"></param>
        /// <param name="r_a"></param>
        /// <param name="r_b"></param>
        /// <returns></returns>
        public static Geo.Algorithm.Vector Elements(double GM, double Mjd_a, double Mjd_b,
                            Geo.Algorithm.Vector r_a, Geo.Algorithm.Vector r_b)
        {

            // Variables

            double tau, eta, p;
            double n, nu, E, u;
            double s_a, s_b, s_0, fac, sinhH;
            double cos_dnu, sin_dnu, ecos_nu, esin_nu;
            double a, e, i, Omega, omega, M;
            Geo.Algorithm.Vector e_a = new Geo.Algorithm.Vector(3), r_0 = new Geo.Algorithm.Vector(3), e_0 = new Geo.Algorithm.Vector(3), W = new Geo.Algorithm.Vector(3);

            // Calculate vector r_0 (fraction of r_b perpendicular to r_a) 
            // and the magnitudes of r_a,r_b and r_0

            s_a = r_a.Norm(); e_a = r_a / s_a;
            s_b = r_b.Norm();
            fac = r_b.Dot(e_a); r_0 = r_b - fac * e_a;
            s_0 = r_0.Norm(); e_0 = r_0 / s_0;

            // Inclination and ascending node 

            W = e_a.Cross3D(e_0);
            Omega = Math.Atan2(W[0], -W[1]);                     // Long. ascend. node 
            Omega = MathUtil.Modulo(Omega, OrbitConsts.TwoPI);
            i = Math.Atan2(Math.Sqrt(W[0] * W[0] + W[1] * W[1]), W[2]); // Inclination        
            if (i == 0.0)
                u = Math.Atan2(r_a[1], r_a[0]);
            else
                u = Math.Atan2(+e_a[2], -e_a[0] * W[1] + e_a[1] * W[0]);

            // Semilatus rectum

            tau = Math.Sqrt(GM) * 86400.0 * Math.Abs(Mjd_b - Mjd_a);
            eta = FindEta(r_a, r_b, tau);
            p = Math.Pow(s_a * s_0 * eta / tau, 2);

            // Eccentricity, true anomaly and argument of perihelion

            cos_dnu = fac / s_b;
            sin_dnu = s_0 / s_b;

            ecos_nu = p / s_a - 1.0;
            esin_nu = (ecos_nu * cos_dnu - (p / s_b - 1.0)) / sin_dnu;

            e = Math.Sqrt(ecos_nu * ecos_nu + esin_nu * esin_nu);
            nu = Math.Atan2(esin_nu, ecos_nu);

            omega = MathUtil.Modulo(u - nu, OrbitConsts.TwoPI);

            // Perihelion distance, semimajor axis and mean motion

            a = p / (1.0 - e * e);
            n = Math.Sqrt(GM / Math.Abs(a * a * a));

            // Mean anomaly and time of perihelion passage

            if (e < 1.0)
            {
                E = Math.Atan2(Math.Sqrt((1.0 - e) * (1.0 + e)) * esin_nu, ecos_nu + e * e);
                M = MathUtil.Modulo(E - e * Math.Sin(E), OrbitConsts.TwoPI);
            }
            else
            {
                sinhH = Math.Sqrt((e - 1.0) * (e + 1.0)) * esin_nu / (e + e * ecos_nu);
                M = e * sinhH - Math.Log(sinhH + Math.Sqrt(1.0 + sinhH * sinhH));
            }

            // Keplerian elements vector

            return new Geo.Algorithm.Vector(a, e, i, Omega, omega, M);

        }


        //------------------------------------------------------------------------------
        //
        // TwoBody
        //
        // Purpose:
        //
        //   Propagates a given state vector and computes the state transition matrix 
        //   for elliptical Keplerian orbits
        //
        // Input/Output:
        //
        //   GM        Gravitational coefficient
        //             (gravitational constant * mass of central body)
        //   Y0        Epoch state vector (x,y,z,vx,vy,vz)_0
        //   dt        Time since epoch
        //   Y         State vector (x,y,z,vx,vy,vz)
        //   dYdY0     State transition matrix d(x,y,z,vx,vy,vz)/d(x,y,z,vx,vy,vz)_0
        //
        // Notes:
        //
        //   The state vector, dt and GM must be given in consistent units, 
        //   e.g. [m], [m/s] and [m^3/s^2]. The resulting units of length and velocity  
        //   are implied by the units of GM, e.g. [m] and [m/s].
        //
        //   Due to the internal use of Keplerian elements, the function cannot be 
        //   used with epoch state vectors describing a circular or non-inclined orbit.
        //
        //------------------------------------------------------------------------------
        /// <summary>
        /// 二体问题计算Propagates a given state vector and computes the state transition matrix 
        ///   for elliptical Keplerian orbits
        /// </summary>
        /// <param name="GM">Gravitational coefficient (gravitational constant * mass of central body) </param>
        /// <param name="Y0">Epoch state vector (x,y,z,vx,vy,vz)_0</param>
        /// <param name="dt">  Time since epoch</param>
        /// <param name="Y">State vector (x,y,z,vx,vy,vz)</param>
        /// <param name="dYdY0"> State transition matrix d(x,y,z,vx,vy,vz)/d(x,y,z,vx,vy,vz)_0</param>
        public static void TwoBody(double GM, Geo.Algorithm.Vector Y0, double dt,
                      ref Geo.Algorithm.Vector Y, ref Matrix dYdY0)
        {
            // Variables
            int k;
            double a, e, i, n, sqe2, naa;
            double P_aM, P_eM, P_eo, P_io, P_iO;
            Geo.Algorithm.Vector A0 = new Geo.Algorithm.Vector(6);
            Matrix dY0dA0 = new Matrix(6, 6), dYdA0 = new Matrix(6, 6), dA0dY0 = new Matrix(6, 6);

            // Orbital elements at epoch
            A0 = Elements(GM, Y0);

            a = A0[0]; e = A0[1]; i = A0[2];

            n = Math.Sqrt(GM / (a * a * a));

            // Propagated state 
            Y = State(GM, A0, dt);

            // State vector partials w.r.t epoch elements
            dY0dA0 = StatePartials(GM, A0, 0.0);
            dYdA0 = StatePartials(GM, A0, dt);

            // Poisson brackets
            sqe2 = Math.Sqrt((1.0 - e) * (1.0 + e));
            naa = n * a * a;

            P_aM = -2.0 / (n * a);                   // P(a,M)     = -P(M,a)
            P_eM = -(1.0 - e) * (1.0 + e) / (naa * e);     // P(e,M)     = -P(M,e)
            P_eo = +sqe2 / (naa * e);                // P(e,omega) = -P(omega,e)
            P_io = -1.0 / (naa * sqe2 * Math.Tan(i));       // P(i,omega) = -P(omega,i)
            P_iO = +1.0 / (naa * sqe2 * Math.Sin(i));       // P(i,Omega) = -P(Omega,i)

            // Partials of epoch elements w.r.t. epoch state
            for (k = 0; k < 3; k++)
            {
                dA0dY0[0, k] = +P_aM * dY0dA0[k + 3, 5];
                dA0dY0[0, k + 3] = -P_aM * dY0dA0[k, 5];

                dA0dY0[1, k] = +P_eo * dY0dA0[k + 3, 4] + P_eM * dY0dA0[k + 3, 5];
                dA0dY0[1, k + 3] = -P_eo * dY0dA0[k, 4] - P_eM * dY0dA0[k, 5];

                dA0dY0[2, k] = +P_iO * dY0dA0[k + 3, 3] + P_io * dY0dA0[k + 3, 4];
                dA0dY0[2, k + 3] = -P_iO * dY0dA0[k, 3] - P_io * dY0dA0[k, 4];

                dA0dY0[3, k] = -P_iO * dY0dA0[k + 3, 2];
                dA0dY0[3, k + 3] = +P_iO * dY0dA0[k, 2];

                dA0dY0[4, k] = -P_eo * dY0dA0[k + 3, 1] - P_io * dY0dA0[k + 3, 2];
                dA0dY0[4, k + 3] = +P_eo * dY0dA0[k, 1] + P_io * dY0dA0[k, 2];

                dA0dY0[5, k] = -P_aM * dY0dA0[k + 3, 0] - P_eM * dY0dA0[k + 3, 1];
                dA0dY0[5, k + 3] = +P_aM * dY0dA0[k, 0] + P_eM * dY0dA0[k, 1];

            };
            // State transition matrix
            dYdY0 = dYdA0 * dA0dY0;
        }

    }
}
