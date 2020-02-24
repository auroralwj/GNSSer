//2017.06.20, czs, funcKeyToDouble from c++ in hongqing, Extended Kalman IsSatisfied class 
//2017.06.26, czs, edit in hongqing, format and refactor codes

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text; using Geo.Coordinates;

using Geo.Algorithm; using Geo.Utils; namespace Gnsser.Orbits
{

    /// <summary>
    ///  Extended Kalman IsSatisfied class 
    /// </summary>
   public  class ExtendedKalmanFilter
    {

        private

          // Elements
          int n;                   // Number of state parameters
        double t;                   // Time 
        Geo.Algorithm.Vector x;                   // State parameters
        Matrix P;                   // Covariance matrix 


        /// <summary>
        ///  Constructor, EKF class (implementation)
        /// </summary>
        /// <param name="n_"></param>
        public ExtendedKalmanFilter(int n_)
        {
            n = (n_);             // Number of estimation parameters
            t = (0.0);           // Epoch
            // Allocate storage and initialize to zero
            x = new Geo.Algorithm.Vector(n);
            P = new Matrix(n, n);
        }

        //
        // Initialization of a new problem
        //

        public void Init(double t_, Geo.Algorithm.Vector x_, Matrix P_)
        {
            t = t_; x = x_; P = P_;
        }

        public void Init(double t_, Geo.Algorithm.Vector x_, Geo.Algorithm.Vector sigma)
        {
            t = t_; x = x_;
            //P = new Matrix(n,n);
            for (int i = 0; i < n; i++) P[i, i] = sigma[i] * sigma[i];
        }


        //
        // Access to filter parameters
        //

        public double Time() { return t; }     // Time    

        public Geo.Algorithm.Vector State() { return x; }     // State parameters

        public Matrix Cov() { return P; }     // Covariance matrix

        public Geo.Algorithm.Vector StdDev()
        {                 // Standard deviation
            Geo.Algorithm.Vector Sigma = new Geo.Algorithm.Vector(n);
            for (int i = 0; i < n; i++) Sigma[i] = Math.Sqrt(P[i, i]);
            return Sigma;
        }

        //
        // Time Update
        //

        public void TimeUpdate(double t_,    // New epoch
                          Geo.Algorithm.Vector x_,    // Propagetd state 
                          Matrix Phi)  // State transition matrix  
        {
            t = t_;                    // Next time step
            x = x_;                    // Propagated state
            P = Phi * P * Phi.Transpose();     // Propagated covariance 
        }


        /// <summary>
        ///  Update of filter parameters
        /// </summary>
        /// <param name="t_"></param>
        /// <param name="x_"></param>
        /// <param name="Phi"></param>
        /// <param name="Qdt"></param>
        public void TimeUpdate(double t_,    // New epoch
                         Geo.Algorithm.Vector x_,    // Propagetd state 
                         Matrix Phi,   // State transition matrix 
                         Matrix Qdt)  // Accumulated process noise 
        {
            t = t_;                          // Next time step
            x = x_;                          // Propagated state
            P = Phi * P * Phi.Transpose() + Qdt;     // Propagated covariance + noise
        }


        /// <summary>
        ///  Scalar Measurement Update 
        /// </summary>
        /// <param name="z"></param>
        /// <param name="g"></param>
        /// <param name="sigma"></param>
        /// <param name="G"></param>
        public void MeasUpdate(double z,     // Measurement at new epoch
                           double g,     // Modelled measurement
                           double sigma, // Standard deviation
                           Geo.Algorithm.Vector G)    // Partials dg/dx
        {
            Geo.Algorithm.Vector K = new Geo.Algorithm.Vector(n);                   // Kalman gain
            double Inv_W = sigma * sigma;    // Inverse weight (measurement covariance)

            // Kalman gain
            K = P * G / (Inv_W + G.Dot(P * G));

            // State update
            x = x + K * (z - g);

            // Covariance update
            P = (Matrix.CreateIdentity(n) - MatrixUtil.Dyadic(K, G)) * P;
        }
    }
}