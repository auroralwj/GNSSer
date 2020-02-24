//2017.06.20, czs, funcKeyToDouble from c++ in hongqing, RK4
//2017.06.26, czs, edit in hongqing, format and refactor codes

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text; using Geo.Coordinates;

using Geo.Algorithm; using Geo.Utils; namespace Gnsser.Orbits
{     
    /// <summary>
    /// 计算y值的微分。Derivative y'=f(x,y) Vector  Function prototype for prevObj order differential equations
    /// </summary>
    /// <param name="x">Independent variable，一般为时间</param>
    /// <param name="y"> State vector，待微分的向量 </param> 
    /// <param name="pAux">辅助数据。 Pointer to auxiliary satData used within f </param>
    /// <returns></returns>
    public delegate Geo.Algorithm.Vector DEfunct(double x, Geo.Algorithm.Vector y, Object pAux);

    /// <summary>
    ///  DE积分状态控制。
    ///  State codes 
    /// </summary>
    public enum DE_STATE
    {
        DE_INIT = 1,   // Restart integration
        DE_DONE = 2,   // Successful step
        DE_BADACC = 3,   // Accuracy requirement could not be achieved
        DE_NUMSTEPS = 4,   // Permitted number of steps exceeded
        DE_STIFF = 5,   // Stiff problem suspected
        DE_INVPARAM = 6    // Invalid input parameters
    }

    /// <summary>
    /// 常微分方程数值积分方法
    /// Numerical integration methods for ordinary differential equations
    ///  DE integrator class specification
    ///  
    /// Shampine, Gordon: "Computer solution of Ordinary Differential Equations",  Freeman and Comp., San Francisco (1975).
    /// </summary>
    public class DeIntegrator
    {
        // Elements
        public double relerr;      // Desired relative accuracy of the solution
        public double abserr;      // Desired absolute accuracy of the solution
        public DE_STATE State;       // State code (default = DE_INIT)
        public bool PermitTOUT;  // Flag for integrating past tout
        // (default = true)
        public double t;           // Value of independent variable


        // Elements
        DEfunct DeriFunc;
        int n_eqn;
        Object pAux;
        Geo.Algorithm.Vector yy { get; set; }
        Geo.Algorithm.Vector wt, p, yp, ypout;
        Matrix phi;
        //double   alpha[13],beta[13],v[13],w[13],psi[13];
        //double   sig[14],g[14];
        double[] alpha = new double[13], beta = new double[13], v = new double[13], w = new double[13], psi = new double[13];
        double[] sig = new double[14], g = new double[14];
        double x, h, hold, told, delsgn;
        int ns, k, kold;
        bool OldPermit, phase1, start, nornd;
      

        /// <summary>
        /// DE 构造函数
        /// </summary>
        /// <param name="f_">Differential equation</param>
        /// <param name="n_eqn_">Dimension</param>
        /// <param name="pAux_">Pointer to auxiliary satData</param>
        public DeIntegrator(DEfunct f_, int n_eqn_, Object pAux_ )
        {
            n_eqn = (n_eqn_);
            DeriFunc = (f_);
            this.pAux = (pAux_);
            yy = new Geo.Algorithm.Vector(n_eqn);   // Allocate vectors with proper dimension
            wt = new Geo.Algorithm.Vector(n_eqn);
            p = new Geo.Algorithm.Vector(n_eqn);
            yp = new Geo.Algorithm.Vector(n_eqn);
            ypout = new Geo.Algorithm.Vector(n_eqn);
            phi = new Matrix(n_eqn, 17);
            State = DE_STATE.DE_INVPARAM;     // Status flag 
            PermitTOUT = true;            // Allow integration past tout by default
            t = 0.0;
            relerr = 0.0;             // Accuracy requirements
            abserr = 0.0;
        }

        const int maxnum = 500;  // Maximum number of steps to take
        const double umach = 1.0e-15;// double.Epsilon;
        const double twou = 2.0 * umach;
        const double fouru = 4.0 * umach;


        #region Auxiliary functions (min, max, sign)
        private static double fabs(double val) { return Math.Abs(val); }
        private static double min(double a, double b) { return Math.Min(a, b); }
        private static double max(double a, double b) { return Math.Max(a, b); }
        // sign: returns absolute value of a with sign of b
        private static double sign(double a, double b) { return (b >= 0.0) ? fabs(a) : -fabs(a); }
        private static double pow(double val, double pow) { return Math.Pow(val, pow); }
        private static double sqrt(double val) { return Math.Sqrt(val); }
        #endregion

        //
        // Integration step
        //

        // Powers of two (two(n)=2**n)
        double[] two = 
                 { 1.0, 2.0, 4.0, 8.0, 16.0, 32.0, 64.0, 128.0,     
                   256.0, 512.0, 1024.0, 2048.0, 4096.0, 8192.0 };

        double[] gstr =  
                 {1.0, 0.5, 0.0833, 0.0417, 0.0264, 0.0188,
                  0.0143, 0.0114, 0.00936, 0.00789, 0.00679,   
                  0.00592, 0.00524, 0.00468 };

        public void Step(ref double x, Geo.Algorithm.Vector y, ref double eps, ref bool crash)
        {
            // Variables

            bool success;
            int i, ifail, im1, ip1, iq, j, km1, km2, knew, kp1, kp2;
            int l, limit1, limit2, nsm2, nsp1, nsp2;
            double absh, erk, erkm1, erkm2, erkp1, err, hnew;
            double p5eps, r, reali, realns, rho, round, sum, tau;
            double temp1, temp2, temp3, temp4, temp5, temp6, xold;
            
            #region  Begin block 0
            //                                                                   
            // Check if step aboutSize or error tolerance is too small for machine    
            // precision.  If prevObj step, initialize phi array and estimate a    
            // starting step aboutSize. If step aboutSize is too small, determine an       
            // acceptable one.                                                   
            //                                                                   

            if (fabs(h) < fouru * fabs(x))
            {
                h = sign(fouru * fabs(x), h);
                crash = true;
                return;                      // Exit 
            };

            p5eps = 0.5 * eps;
            crash = false;
            g[1] = 1.0;
            g[2] = 0.5;
            sig[1] = 1.0;

            ifail = 0;

            // If error tolerance is too small, increase it to an 
            // acceptable value.                                  

            round = 0.0;
            for (l = 0; l < n_eqn; l++) round += (y[l] * y[l]) / (wt[l] * wt[l]);
            round = twou * sqrt(round);
            if (p5eps < round)
            {
                eps = 2.0 * round * (1.0 + fouru);
                crash = true;
                return;
            };


            if (start)
            {
                // Initialize. Compute appropriate step aboutSize for prevObj step. 
                yp= DeriFunc(x, y, pAux);
                sum = 0.0;
                for (l = 0; l < n_eqn; l++)
                {
                    phi[l, 1] = yp[l];
                    phi[l, 2] = 0.0;
                    sum += (yp[l] * yp[l]) / (wt[l] * wt[l]);
                }
                sum = sqrt(sum);
                absh = fabs(h);
                if (eps < 16.0 * sum * h * h) absh = 0.25 * sqrt(eps / sum);
                h = sign(max(absh, fouru * fabs(x)), h);
                hold = 0.0;
                hnew = 0.0;
                k = 1;
                kold = 0;
                start = false;
                phase1 = true;
                nornd = true;
                if (p5eps <= 100.0 * round)
                {
                    nornd = false;
                    for (l = 0; l < n_eqn; l++) phi[l, 15] = 0.0;
                };
            };
            #endregion End block 0

            #region  Repeat blocks 1, 2 (and 3) until step is successful
            do
            {
                #region Begin block 1
                //                                                                 
                // Compute coefficients of formulas for this step. Avoid computing 
                // those quantities not changed when step aboutSize is not changed.     
                //                                                                 

                kp1 = k + 1;
                kp2 = k + 2;
                km1 = k - 1;
                km2 = k - 2;

                // ns is the number of steps taken with aboutSize h, including the 
                // current one. When k<ns, no coefficients change.           

                if (h != hold) ns = 0;
                if (ns <= kold) ns = ns + 1;
                nsp1 = ns + 1;

                if (k >= ns)
                {

                    // Compute those components of alpha[*],beta[*],psi[*],sig[*] 
                    // which are changed                                          
                    beta[ns] = 1.0;
                    realns = ns;
                    alpha[ns] = 1.0 / realns;
                    temp1 = h * realns;
                    sig[nsp1] = 1.0;
                    if (k >= nsp1)
                    {
                        for (i = nsp1; i <= k; i++)
                        {
                            im1 = i - 1;
                            temp2 = psi[im1];
                            psi[im1] = temp1;
                            beta[i] = beta[im1] * psi[im1] / temp2;
                            temp1 = temp2 + h;
                            alpha[i] = h / temp1;
                            reali = i;
                            sig[i + 1] = reali * alpha[i] * sig[i];
                        };
                    };
                    psi[k] = temp1;

                    // Compute coefficients g[*]; initialize v[*] and set w[*]. 
                    if (ns > 1)
                    {
                        // If order was raised, update diagonal part of v[*] 
                        if (k > kold)
                        {
                            temp4 = k * kp1;
                            v[k] = 1.0 / temp4;
                            nsm2 = ns - 2;
                            for (j = 1; j <= nsm2; j++)
                            {
                                i = k - j;
                                v[i] = v[i] - alpha[j + 1] * v[i + 1];
                            };
                        };
                        // Update V[*] and set W[*] 
                        limit1 = kp1 - ns;
                        temp5 = alpha[ns];
                        for (iq = 1; iq <= limit1; iq++)
                        {
                            v[iq] = v[iq] - temp5 * v[iq + 1];
                            w[iq] = v[iq];
                        };

                        g[nsp1] = w[1];
                    }
                    else
                    {
                        for (iq = 1; iq <= k; iq++)
                        {
                            temp3 = iq * (iq + 1);
                            v[iq] = 1.0 / temp3;
                            w[iq] = v[iq];
                        };
                    };

                    // Compute the g[*] in the work vector w[*] 
                    nsp2 = ns + 2;
                    if (kp1 >= nsp2)
                    {
                        for (i = nsp2; i <= kp1; i++)
                        {
                            limit2 = kp2 - i;
                            temp6 = alpha[i - 1];
                            for (iq = 1; iq <= limit2; iq++) w[iq] = w[iq] - temp6 * w[iq + 1];
                            g[i] = w[1];
                        };
                    };

                }; // if K>=NS  

                #endregion End block 1

                #region Begin block 2
                //                                                                 
                // Predict a solution p[*], evaluate derivatives using predicted   
                // solution, estimate local error at order k and errors at orders  
                // k, k-1, k-2 as if constant step aboutSize were used.                 
                //                                                                 

                // Change phi to phi star 
                if (k >= nsp1)
                {
                    for (i = nsp1; i <= k; i++)
                    {
                        temp1 = beta[i];
                        for (l = 0; l < n_eqn; l++) phi[l, i] = temp1 * phi[l, i];
                    }
                };

                // Predict solution and differences 
                for (l = 0; l < n_eqn; l++)
                {
                    phi[l, kp2] = phi[l, kp1];
                    phi[l, kp1] = 0.0;
                    p[l] = 0.0;
                };
                for (j = 1; j <= k; j++)
                {
                    i = kp1 - j;
                    ip1 = i + 1;
                    temp2 = g[i];
                    for (l = 0; l < n_eqn; l++)
                    {
                        p[l] = p[l] + temp2 * phi[l, i];
                        phi[l, i] = phi[l, i] + phi[l, ip1];
                    };
                };
                if (nornd)
                {
                    p = y + h * p;
                }
                else
                {
                    for (l = 0; l < n_eqn; l++)
                    {
                        tau = h * p[l] - phi[l, 15];
                        p[l] = y[l] + tau;
                        phi[l, 16] = (p[l] - y[l]) - tau;
                    };
                };
                xold = x;
                x = x + h;
                absh = fabs(h);
                 yp= DeriFunc(x, p, pAux);

                // Estimate errors at orders k, k-1, k-2 
                erkm2 = 0.0;
                erkm1 = 0.0;
                erk = 0.0;

                for (l = 0; l < n_eqn; l++)
                {
                    temp3 = 1.0 / wt[l];
                    temp4 = yp[l] - phi[l, 1];
                    if (km2 > 0) erkm2 = erkm2 + ((phi[l, km1] + temp4) * temp3)
                                                * ((phi[l, km1] + temp4) * temp3);
                    if (km2 >= 0) erkm1 = erkm1 + ((phi[l, k] + temp4) * temp3)
                                                 * ((phi[l, k] + temp4) * temp3);
                    erk = erk + (temp4 * temp3) * (temp4 * temp3);
                };

                if (km2 > 0) erkm2 = absh * sig[km1] * gstr[km2] * sqrt(erkm2);
                if (km2 >= 0) erkm1 = absh * sig[k] * gstr[km1] * sqrt(erkm1);

                temp5 = absh * sqrt(erk);
                err = temp5 * (g[k] - g[kp1]);
                erk = temp5 * sig[kp1] * gstr[k];
                knew = k;

                // Test if order should be lowered 
                if (km2 > 0) if (max(erkm1, erkm2) <= erk) knew = km1;
                if (km2 == 0) if (erkm1 <= 0.5 * erk) knew = km1;


                #endregion   End block 2

                //                                                                 
                // If step is successful continue with block 4, otherwise repeat  blocks 1 and 2 after executing block 3                          
                //                                                                 

                success = (err <= eps);

                if (!success)
                {
                    #region  block 3

                    // The step is unsuccessful. Restore x, phi[*,*], psi[*]. If   
                    // 3rd consecutive failure, set order to 1. If step fails more 
                    // than 3 times, consider an optimal step aboutSize. Double error   
                    // tolerance and return if estimated step aboutSize is too small    
                    // for machine precision.                                      
                    //                                                             

                    // Restore x, phi[*,*] and psi[*] 
                    phase1 = false;
                    x = xold;
                    for (i = 1; i <= k; i++)
                    {
                        temp1 = 1.0 / beta[i];
                        ip1 = i + 1;
                        for (l = 0; l < n_eqn; l++) phi[l, i] = temp1 * (phi[l, i] - phi[l, ip1]);
                    };

                    if (k >= 2)
                        for (i = 2; i <= k; i++) psi[i - 1] = psi[i] - h;

                    // On third failure, set order to one. 
                    // Thereafter, use optimal step aboutSize   
                    ifail++;
                    temp2 = 0.5;
                    if (ifail > 3)
                        if (p5eps < 0.25 * erk) temp2 = sqrt(p5eps / erk);
                    if (ifail >= 3) knew = 1;
                    h = temp2 * h;
                    k = knew;
                    if (fabs(h) < fouru * fabs(x))
                    {
                        crash = true;
                        h = sign(fouru * fabs(x), h);
                        eps *= 2.0;
                        return;                     // Exit 
                    }
                    #endregion End block 3, return to start of block 1
                };  // end if(success) 
            }
            while (!success);
            #endregion

            #region block 4
            //                                                                   
            // Begin block 4                                                     
            //                                                                   
            // The step is successful. Correct the predicted solution, evaluate  
            // the derivatives using the corrected solution and update the       
            // differences. Determine best order and step aboutSize for next step.    
            //  
            kold = k;
            hold = h;


            // Correct and evaluate 
            temp1 = h * g[kp1];
            if (nornd)
                for (l = 0; l < n_eqn; l++) y[l] = p[l] + temp1 * (yp[l] - phi[l, 1]);
            else
                for (l = 0; l < n_eqn; l++)
                {
                    rho = temp1 * (yp[l] - phi[l, 1]) - phi[l, 16];
                    y[l] = p[l] + rho;
                    phi[l, 15] = (y[l] - p[l]) - rho;
                };

           yp= DeriFunc(x, y, pAux);

            // Update differences for next step 
            for (l = 0; l < n_eqn; l++)
            {
                phi[l, kp1] = yp[l] - phi[l, 1];
                phi[l, kp2] = phi[l, kp1] - phi[l, kp2];
            };
            for (i = 1; i <= k; i++)
                for (l = 0; l < n_eqn; l++)
                    phi[l, i] = phi[l, i] + phi[l, kp1];


            // Estimate error at order k+1 unless               
            // - in prevObj phase when always raise order,        
            // - already decided to lower order,                
            // - step aboutSize not constant so estimate unreliable  
            erkp1 = 0.0;
            if ((knew == km1) || (k == 12)) phase1 = false;

            if (phase1)
            {
                k = kp1;
                erk = erkp1;
            }
            else
            {
                if (knew == km1)
                {
                    // lower order 
                    k = km1;
                    erk = erkm1;
                }
                else
                {

                    if (kp1 <= ns)
                    {
                        for (l = 0; l < n_eqn; l++)
                            erkp1 = erkp1 + (phi[l, kp2] / wt[l]) * (phi[l, kp2] / wt[l]);
                        erkp1 = absh * gstr[kp1] * sqrt(erkp1);

                        // Using estimated error at order k+1, determine 
                        // appropriate order for next step               
                        if (k > 1)
                        {
                            if (erkm1 <= min(erk, erkp1))
                            {
                                // lower order
                                k = km1; erk = erkm1;
                            }
                            else
                            {
                                if ((erkp1 < erk) && (k != 12))
                                {
                                    // raise order 
                                    k = kp1; erk = erkp1;
                                };
                            };
                        }
                        else
                        {
                            if (erkp1 < 0.5 * erk)
                            {
                                // raise order 
                                // Here erkp1 < erk < max(erkm1,ermk2) else    
                                // order would have been lowered in block 2.   
                                // Thus order is to be raised                  
                                k = kp1;
                                erk = erkp1;
                            };
                        };

                    }; // end if kp1<=ns 

                }; // end if knew!=km1 

            }; // end if !phase1 


            // With new order determine appropriate step aboutSize for next step 
            if (phase1 || (p5eps >= erk * two[k + 1]))
                hnew = 2.0 * h;
            else
            {
                if (p5eps < erk)
                {
                    temp2 = k + 1;
                    r = pow(p5eps / erk, 1.0 / temp2);
                    hnew = absh * max(0.5, min(0.9, r));
                    hnew = sign(max(hnew, fouru * fabs(x)), h);
                }
                else hnew = h;
            };

            h = hnew;
            #endregion End block 4
        }
         
        /// <summary>
        /// Interpolation
        /// </summary>
        /// <param name="xout"></param>
        /// <param name="yout"></param>
        /// <param name="ypout"></param>
        public void Interpolate(double xout, ref  Geo.Algorithm.Vector yout, ref Geo.Algorithm.Vector ypout)
        {
            // Variables
            int i, j, ki;
            double eta, gamma, hi, psijm1;
            double temp1, term;
            double[] g = new double[14], rho = new double[14], w = new double[14];


            g[1] = 1.0;
            rho[1] = 1.0;

            hi = xout - x;
            ki = kold + 1;

            // Initialize w[*] for computing g[*] 
            for (i = 1; i <= ki; i++)
            {
                temp1 = i;
                w[i] = 1.0 / temp1;
            }

            // Compute g[*] 
            term = 0.0;
            for (j = 2; j <= ki; j++)
            {
                psijm1 = psi[j - 1];
                gamma = (hi + term) / psijm1;
                eta = hi / psijm1;
                for (i = 1; i <= ki + 1 - j; i++) w[i] = gamma * w[i] - eta * w[i + 1];
                g[j] = w[1];
                rho[j] = gamma * rho[j - 1];
                term = psijm1;
            };

            // Interpolate for the solution yout and for 
            // the derivative of the solution ypout      
            ypout = new Geo.Algorithm.Vector(yout.Dimension);
            yout = new Geo.Algorithm.Vector(yout.Dimension);
            for (j = 1; j <= ki; j++)
            {
                i = ki + 1 - j;
                yout = yout + g[i] * phi.Col(i);
                ypout = ypout + rho[i] * phi.Col(i);
            };
            yout = yy + hi * yout;
            //  return yy + hi * yout;


            //   Console.WriteLine(yout); 
        }


        //
        // DE integration
        // (with full control of warnings and errros status codes)
        //

        public void Integ_(
                   ref double t,          // Value of independent variable
                   double tout,       // Desired output point
                   ref Geo.Algorithm.Vector y           // Solution vector
                 )
        {

            // Variables

            bool stiff, crash = false;           // Flags
            int nostep;                 // Step count
            int kle4 = 0;
            double releps, abseps, tend;
            double absdel, del, eps;


            // Return, if output time equals input time

            if (t == tout) return;    // No integration


            // Test for improper parameters

            eps = max(relerr, abserr);

            if ((relerr < 0.0) ||      // Negative relative error bound
                 (abserr < 0.0) ||      // Negative absolute error bound
                 (eps <= 0.0) ||      // Both error bounds are non-positive
                 (State > DE_STATE.DE_INVPARAM) ||      // Invalid status flag
                 ((State != DE_STATE.DE_INIT) &&
                   (t != told)))
            {
                State = DE_STATE.DE_INVPARAM;                 // Set error code
                return;                              // Exit
            };


            // On each call set interval of integration and counter for
            // number of steps. Adjust input error tolerances to define
            // weight vector for subroutine STEP.

            del = tout - t;
            absdel = fabs(del);

            tend = t + 100.0 * del;
            if (!PermitTOUT) tend = tout;

            nostep = 0;
            kle4 = 0;
            stiff = false;
            releps = relerr / eps;
            abseps = abserr / eps;

            if ((State == DE_STATE.DE_INIT) || (!OldPermit) || (delsgn * del <= 0.0))
            {
                // On start and restart also set the work variables x and yy(*),
                // store the direction of integration and initialize the step aboutSize
                start = true;
                x = t;
                yy = new Geo.Algorithm.Vector(y);
                delsgn = sign(1.0, del);
                h = sign(max(fouru * fabs(x), fabs(tout - x)), tout - x);
            }

            while (true)
            {  // Start step loop

                // If already past output point, interpolate solution and return
                if (fabs(x - t) >= absdel)
                {
                    Interpolate(tout, ref y, ref ypout);
                    State = DE_STATE.DE_DONE;          // Set return code
                    t = tout;             // Set independent variable
                    told = t;                // Store independent variable
                    OldPermit = PermitTOUT;
                    return;                       // Normal exit
                };

                // If cannot go past output point and sufficiently close,
                // extrapolate and return
                if (!PermitTOUT && (fabs(tout - x) < fouru * fabs(x)))
                {
                    h = tout - x;
                   yp = DeriFunc(x, yy, pAux);              // Compute derivative yp(x)
                    y = (yy) + h * yp;                // Extrapolate vector from x to tout
                    State = DE_STATE.DE_DONE;          // Set return code
                    t = tout;             // Set independent variable
                    told = t;                // Store independent variable
                    OldPermit = PermitTOUT;
                    return;                       // Normal exit
                };

                // Test for too much work
                if (nostep >= maxnum)
                {
                    State = DE_STATE.DE_NUMSTEPS;          // Too many steps
                    if (stiff) State = DE_STATE.DE_STIFF;  // Stiffness suspected
                    y = new Geo.Algorithm.Vector(yy);               // Copy last step
                    t = x;
                    told = t;
                    OldPermit = true;
                    return;                        // Weak failure exit
                };

                // Limit step aboutSize, set weight vector and take a step
                h = sign(min(fabs(h), fabs(tend - x)), h);
                for (int l = 0; l < n_eqn; l++)
                    wt[l] = releps * fabs(yy[l]) + abseps;

                Step(ref x, yy, ref eps, ref crash);

                //Console.WriteLine(yy);

                // Test for too small tolerances
                if (crash)
                {
                    State = DE_STATE.DE_BADACC;
                    relerr = eps * releps;       // Modify relative and absolute
                    abserr = eps * abseps;       // accuracy requirements
                    y = new Geo.Algorithm.Vector(yy);               // Copy last step
                    t = x;
                    told = t;
                    OldPermit = true;
                    return;                       // Weak failure exit
                }

                nostep++;  // Count total number of steps

                // Count number of consecutive steps taken with the order of
                // the method being less or equal to four and test for stiffness
                kle4++;
                if (kold > 4) kle4 = 0;
                if (kle4 >= 50) stiff = true;

            } // End step loop 
        }


    
        /// <summary>
        /// Initialization
        /// </summary>
        /// <param name="t0">Initial value of the independent variable</param>
        /// <param name="rel">Relative accuracy requirement</param>
        /// <param name="abs"> Absolute accuracy requirement</param>
        public void Init(double t0, double rel, double abs)
        {
            t = t0;
            relerr = rel;
            abserr = abs;
            State = DE_STATE.DE_INIT;
        } 
        /// <summary> 
        /// DE integration with simplified state code handling
        /// (skips over warnings, aborts in case of error)
        /// </summary>
        /// <param name="tout"> Desired output point</param>
        /// <param name="y"> Solution vector</param>
        public Geo.Algorithm.Vector Integ(double tout, Geo.Algorithm.Vector y)
        {
            do
            {
                Integ_(ref t, tout, ref y);

                if (State == DE_STATE.DE_INVPARAM)
                {
                    var info = "ERROR: invalid parameters in public Integ";
                    throw new ArgumentException(info);
                    // << std::endl; exit(1); 
                }
                if (State == DE_STATE.DE_BADACC)
                {
                    var info = "WARNING: Accuracy requirement not achieved in public Integ";
                    Console.WriteLine(info);
                }
                if (State == DE_STATE.DE_STIFF)
                {
                    var info = "WARNING: Stiff problem suspected in public Integ";
                    Console.WriteLine(info);
                }
            }
            while (State > DE_STATE.DE_DONE);
            return y;
        }
        public void Integ( double tout, ref  Geo.Algorithm.Vector y )
        {
            do
            {
                Integ_(ref t, tout, ref y);


                if (State == DE_STATE.DE_INVPARAM)
                {
                    var info = "ERROR: invalid parameters in public Integ";
                    throw new ArgumentException(info);
                    // << std::endl; exit(1); 
                }
                if (State == DE_STATE.DE_BADACC)
                {
                    var info = "WARNING: Accuracy requirement not achieved in public Integ";
                    Console.WriteLine(info);
                }
                if (State == DE_STATE.DE_STIFF)
                {
                    var info = "WARNING: Stiff problem suspected in public Integ";
                    Console.WriteLine(info);
                }
            }
            while (State > DE_STATE.DE_DONE);
        }


        //
        // Interpolation
        //

        public Geo.Algorithm.Vector Intrp(
             double tout,           // Desired output point
            ref  Geo.Algorithm.Vector y               // Solution vector
           )
        {
            Interpolate(tout, ref y, ref ypout);     // Interpolate and discard interpolated
            return ypout;// derivative ypout
        }

    }
}