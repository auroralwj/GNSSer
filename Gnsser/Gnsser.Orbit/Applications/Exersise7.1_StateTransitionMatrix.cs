
//2017.06.24, czs, funcKeyToDouble from c++ in hongqing, Exersise7.1_StateTransitionMatrix
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text; using Geo.Coordinates;

using Geo.Algorithm; using Geo.Utils; namespace Gnsser.Orbits
{

    public class AuxParam7
    {
        public double Mjd_0;     // Reference epoch
        public int n_a, m_a;   // Degree and order of gravity field for acceleration
        public int n_G, m_G;   // Degree and order of gravity field for gradient
    }

    class Program
    { 


        //------------------------------------------------------------------------------
        //
        // Accel
        //
        // Purpose:
        //
        //   Computes the acceleration of an Earth orbiting satellite due to 
        //   the Earth's harmonic gravity field up to degree and order 10
        //
        // Input/Output:
        //
        //   Mjd_UT      Modified Julian Date (Universal Time)
        //   r           Satellite position vector in the true-of-date system
        //   n,m         Gravity model degree and order 
        //   <return>    Acceleration (a=d^2r/dt^2) in the true-of-date system
        //
        //------------------------------------------------------------------------------

        static Geo.Algorithm.Vector Accel(double Mjd_UT, Geo.Algorithm.Vector r, int n, int m)
        {
            // Earth rotation matrix 

            Matrix U = Matrix.RotateZ3D(IERS.GetGmstRad(Mjd_UT));

            // Acceleration due to harmonic gravity field

            return Force.AccelerOfHarmonicGraviFiled(r, U, OrbitConsts.GM_Earth, Force.Grav.R_ref, Force.Grav.CS, n, m);

        }


        //------------------------------------------------------------------------------
        //
        // Gradient
        //
        // Purpose:
        //
        //   Computes the gradient of the Earth's harmonic gravity field 
        //
        // Input/Output:
        //
        //   Mjd_UT      Modified Julian Date (Universal Time)
        //   r           Satellite position vector in the true-of-date system
        //   n,m         Gravity model degree and order 
        //   <return>    Gradient (G=da/dr) in the true-of-date system
        //
        //------------------------------------------------------------------------------

        static Matrix Gradient(double Mjd_UT, Geo.Algorithm.Vector r, int n, int m)
        {

            // Constants

            const double d = 1.0;   // Position increment [m]

            // Variables 

            int i;
            Geo.Algorithm.Vector a = new Geo.Algorithm.Vector(3), da = new Geo.Algorithm.Vector(3), dr = new Geo.Algorithm.Vector(3);
            Matrix U = new Matrix(3, 3), G = new Matrix(3, 3);

            // Earth rotation matrix 

            U = Matrix.RotateZ3D(IERS.GetGmstRad(Mjd_UT));

            // Acceleration

            a = Force.AccelerOfHarmonicGraviFiled(r, U, OrbitConsts.GM_Earth, Force.Grav.R_ref, Force.Grav.CS, n, m);

            // Gradient

            for (i = 0; i <= 2; i++)
            {
                // Set offset in i-th component of the position vector
                //dr = 0.0;
                dr = new Geo.Algorithm.Vector(3);
                dr[i] = d;
                // Acceleration difference
                da = Force.AccelerOfHarmonicGraviFiled(r + dr / 2, U, OrbitConsts.GM_Earth, Force.Grav.R_ref, Force.Grav.CS, n, m) -
                     Force.AccelerOfHarmonicGraviFiled(r - dr / 2, U, OrbitConsts.GM_Earth, Force.Grav.R_ref, Force.Grav.CS, n, m);
                //  da = AccelHarmonic ( r+dr,U, SatConst.GM_Earth, Grav.R_ref,Grav.CS, n,m ) -  a;
                // Derivative with respect to i-th axis
                G.SetCol(i, da / d);
            }

            return G;

        }
         
        /// <summary>
        ///  Computes the variational equations, i.e. the derivative of the state vector
        ///   and the state transition matrix
        /// </summary>
        /// <param name="t">t           Time since epoch (*pAux).Mjd_0 in [s]</param>
        /// <param name="yPhi"> yPhi        (6+36)-dim vector comprising the state vector (y) and the
        ///               state transition matrix (Phi) in column wise storage order</param>
        /// <param name="yPhip"> yPhip       Derivative of yPhi</param>
        /// <param name="pAux">pAux        Pointer; pAux is expected to point to a variable of type
        ///               AuxDataRecord, which is used to communicate with the other
        ///               program sections and to hold satData between subsequent calls
        ///               of this function</param>
        static Geo.Algorithm.Vector VarEqn(double t, Geo.Algorithm.Vector yPhi,  Object pAux)// void* pAux )
        {
            // Variables
            AuxParam7 p;             // Auxiliary satData pointer
            int i, j;           // Loop counters
            double Mjd;           // Modified Julian Date
            Geo.Algorithm.Vector r = new Geo.Algorithm.Vector(3), v = new Geo.Algorithm.Vector(3);    // Position, velocity
            Geo.Algorithm.Vector a = new Geo.Algorithm.Vector(3);          // Acceleration
            Matrix G = new Matrix(3, 3);        // Gradient of acceleration
            Matrix Phi = new Matrix(6, 6);      // State transition matrix
            Matrix Phip = new Matrix(6, 6);     // Time derivative of state transition matrix
            Matrix dfdy = new Matrix(6, 6);     // 

            // Pointer to auxiliary satData record
            p = (AuxParam7)(pAux);

            // Time

            Mjd = p.Mjd_0 + t / 86400.0;

            // State vector components
            r = yPhi.Slice(0, 2);
            v = yPhi.Slice(3, 5);

            // State transition matrix
            for (j = 0; j <= 5; j++) Phi.SetCol(j, yPhi.Slice(6 * (j + 1), 6 * (j + 1) + 5));

            // Acceleration and gradient
            a = Accel(Mjd, r, p.n_a, p.m_a);
            G = Gradient(Mjd, r, p.n_G, p.m_G);

            // Time derivative of state transition matrix
            for (i = 0; i <= 2; i++)
            {
                for (j = 0; j <= 2; j++)
                {
                    dfdy[i, j] = 0.0;                  // dv/dr[i,j]
                    dfdy[i + 3, j] = G[i, j];               // da/dr[i,j]
                    dfdy[i, j + 3] = (i == j ? 1 : 0);   // dv/dv[i,j]
                    dfdy[i + 3, j + 3] = 0.0;                // da/dv[i,j]
                };
            };

            Phip = dfdy * Phi;

            // Derivative of combined state vector and state transition matrix
            var yPhip = new Geo.Algorithm.Vector(42);
            for (i = 0; i <= 2; i++)
            {
                yPhip[i] = v[i];                    // dr/dt[i]
                yPhip[i + 3] = a[i];                    // dv/dt[i]
            };
            for (i = 0; i <= 5; i++)
                for (j = 0; j <= 5; j++)
                    yPhip[6 * (j + 1) + i] = Phip[i, j];     // dPhi/dt[i,j]
            return yPhip;
        }


        //------------------------------------------------------------------------------
        //
        // Max: finds the largest element of a matrix
        //
        //------------------------------------------------------------------------------

        static double Max(Matrix M)
        {

            int n = M.RowCount;
            int m = M.ColCount;
            double max = 0.0;

            for (int i = 0; i < n; i++)
                for (int j = 0; j < m; j++)
                    if (fabs(M[i, j]) > max) max = fabs(M[i, j]);

            return max;
        }

        static double fabs(double val) { return Math.Abs(val); }

        //------------------------------------------------------------------------------
        //
        // InvSymp: 
        //
        // Purpose:
        //
        //   Inverts a symplectic matrix
        //
        // Input/output:
        //
        //   A         Sympletctic (even-dimensional square) matrix 
        //   <return>  Inverse of A
        //
        //                      ( +(A_22)^T  -(A_12)^T )            ( A_11  A_12 )
        //             A^(-1) = (                      )  with  A = (            )
        //                      ( -(A_21)^T  +(A_11)^T )            ( A_21  A_22 )
        //
        //------------------------------------------------------------------------------

        static Matrix InvSymp(Matrix A)
        {

            int N = A.RowCount;
            int n = N / 2;
            Matrix Inv = new Matrix(N, N);

            if (A.ColCount != N || 2 * n != N)
            {
                throw new ArgumentException("ERROR: Invalid shape in InvSymp(Matrix)");
                //  exit(1);
            };

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    Inv[i, j] = +A[j + n, i + n];
                    Inv[i, j + n] = -A[j, i + n];
                    Inv[i + n, j] = -A[j + n, i];
                    Inv[i + n, j + n] = +A[j, i];
                };
            };

            return Inv;

        }


        static void Main(string[] args)
        {

            // Constants

            const double relerr = 1.0e-13;                     // Relative and absolute
            const double abserr = 1.0e-6;                      // accuracy requirement

            const double a = OrbitConsts.RadiusOfEarth + 650.0e3;                // Semi-major axis [m]
            const double e = 0.001;                          // Eccentricity
            const double inc = OrbitConsts.RadPerDeg * 51.0;                       // Inclination [rad]

            double n = Math.Sqrt(OrbitConsts.GM_Earth / (a * a * a));         // Mean motion

            Matrix Scale = Matrix.CreateDiagonal(new Geo.Algorithm.Vector(1, 1, 1, 1 / n, 1 / n, 1 / n)); // Velocity
            Matrix scale = Matrix.CreateDiagonal(new Geo.Algorithm.Vector(1, 1, 1, n, n, n)); // normalization

            const double t_end = 86400.0;
            const double step = 300.0;

            // Variables

            int i, j;                     // Loop counters
            double Mjd0;                    // Epoch (Modified Julian Date)
            double t;                       // Integration time [s]
            double max, max_Kep;            // Matrix error norm
            AuxParam7 Aux1 = new AuxParam7(), Aux2 = new AuxParam7();               // Auxiliary parameter records
            // Model parameters for reference solution (full 10x10 gravity model) and
            // simplified variational equations

            // Epoch state and transition matrix

            Mjd0 = OrbitConsts.MJD_J2000;

            Aux1.Mjd_0 = Mjd0;  // Reference epoch
            Aux1.n_a = 10;    // Degree of gravity field for trajectory computation
            Aux1.m_a = 10;    // Order of gravity field for trajectory computation
            Aux1.n_G = 10;    // Degree of gravity field for variational equations
            Aux1.m_G = 10;    // Order of gravity field for variational equations

            Aux2.Mjd_0 = Mjd0;  // Reference epoch
            Aux2.n_a = 2;    // Degree of gravity field for trajectory computation
            Aux2.m_a = 0;    // Order of gravity field for trajectory computation
            Aux2.n_G = 2;    // Degree of gravity field for variational equations
            Aux2.m_G = 0;    // Order of gravity field for variational equations




            DeIntegrator Int1 = new DeIntegrator(VarEqn, 42, Aux1);   // Object for integrating the variat. eqns.
            DeIntegrator Int2 = new DeIntegrator(VarEqn, 42, Aux2);   // 
            Geo.Algorithm.Vector y0 = new Geo.Algorithm.Vector(6), y = new Geo.Algorithm.Vector(6);             // State vector
            Geo.Algorithm.Vector yPhi1 = new Geo.Algorithm.Vector(42), yPhi2 = new Geo.Algorithm.Vector(42);    // State and transition matrix vector
            Matrix Phi1 = new Matrix(6, 6), Phi2 = new Matrix(6, 6);    // State transition matrix
            Matrix Phi_Kep = new Matrix(6, 6);            // State transition matrix (Keplerian)


            y0 = Kepler.State(OrbitConsts.GM_Earth, new Geo.Algorithm.Vector(a, e, inc, 0.0, 0.0, 0.0));

            for (i = 0; i <= 5; i++)
            {
                yPhi1[i] = y0[i];
                for (j = 0; j <= 5; j++) yPhi1[6 * (j + 1) + i] = (i == j ? 1 : 0);
            }

            yPhi2 = new Geo.Algorithm.Vector(yPhi1);



            // Initialization

            t = 0.0;
            Int1.Init(t, relerr, abserr);
            Int2.Init(t, relerr, abserr);

            // Header
            var endl = "\r\n";
            var info = "Exercise 7-1: State transition matrix" + endl
                + endl
                + "  Time [s]  Error (J2)  Error(Kep)" + endl;

            // Steps

            while (t < t_end)
            {

                // New output time

                t += step;

                // Integration

                Int1.Integ(t, ref yPhi1); // Reference
                Int2.Integ(t, ref yPhi2); // Simplified

                //info= setprecision(5) + setw(12)+yPhi1 + endl;
                //info= setprecision(5) + setw(12) + yPhi2 + endl;


                // Extract and normalize state transition matrices

                for (j = 0; j <= 5; j++) Phi1.SetCol(j, yPhi1.Slice(6 * (j + 1), 6 * (j + 1) + 5));
                for (j = 0; j <= 5; j++) Phi2.SetCol(j, yPhi2.Slice(6 * (j + 1), 6 * (j + 1) + 5));

                Phi1 = Scale * Phi1 * scale;
                Phi2 = Scale * Phi2 * scale;

                // Keplerian state transition matrix (normalized)

                Kepler.TwoBody(OrbitConsts.GM_Earth, y0, t, ref y, ref Phi_Kep);
                Phi_Kep = Scale * Phi_Kep * scale;

                // Matrix error norms

                max = Max(Matrix.CreateIdentity(6) - Phi1 * InvSymp(Phi2));
                max_Kep = Max(Matrix.CreateIdentity(6) - Phi1 * InvSymp(Phi_Kep));

                // Output 
                //   info = String.Format( "{0, 10:F }{1, 10:F }{2, 10:F } ",  t,  max , max_Kep );
                info = String.Format("{0, 10:F2}{1, 10:F2}{2, 12:F2}", t, max, max_Kep);
                Console.WriteLine(info);
            };

            // Output

            info = endl
                + "Time since epoch [s]" + endl
                + endl
                + String.Format("{0, 10:F}", t) + endl
                + endl;
            Console.Write(info);
            info = "State transition matrix (reference)" + endl
                + endl
                + Phi1 + endl;
            Console.Write(info);
            info = "J2 State transition matrix" + endl
                + endl
                + Phi2 + endl;
            Console.Write(info);
            info = "Keplerian State transition matrix" + endl
                + endl
                + Phi_Kep + endl;
            Console.Write(info);

            Console.ReadKey();
        }
    }
}
