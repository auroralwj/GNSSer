
//2017.06.21, czs, funcKeyToDouble from c++ in hongqing,Exersise4.2_Gauss-Jackson4th-orderPredictor
//2017.06.26, czs, edit in hongqing, format and refactor codes

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text; using Geo.Coordinates;

using Geo.Algorithm; using Geo.Utils; namespace Gnsser.Orbits
{
    //------------------------------------------------------------------------------
    //
    // GJ4P class 
    //
    //------------------------------------------------------------------------------

    // Function prototype for second order differential equations
    // void f (double t, const Vector& r, const Vector& v, Vector& a)

    public delegate void GJ4Pfunct(
      double t,     // Independent variable
      Geo.Algorithm.Vector r,     // Position vector 
      Geo.Algorithm.Vector v,     // Velocity vector  r'=v
      ref Geo.Algorithm.Vector a,     // Acceleration     r''=a=f(t,r,v)
      Action pAux   // Pointer to auxiliary satData used within f
    );
     
    /// <summary>
    /// Gauss Jackson第四阶预测器.
    /// 定步长多步积分法。
    /// Gauss-Jackson4th-orderPredictor
    /// </summary>
    public class GaussJackson4OrderPredictor
    {
        /// <summary>
        /// Gauss Jackson第四阶预测器
        /// </summary>
        /// <param name="f_">Differential equation</param>
        /// <param name="n_eqn_">Dimension</param>
        /// <param name="pAux_">Pointer to auxiliary satData</param>
        public GaussJackson4OrderPredictor(
            GJ4Pfunct f_, 
            int n_eqn_,
            Action pAux_ 
            )
        {
            n_eqn = (n_eqn_);
            DevFunc = (f_);
            pAux = (pAux_);

            for (int i = 0; i < 4; i++)
            {
                D[i] = new Geo.Algorithm.Vector(n_eqn);
                d[i] = new Geo.Algorithm.Vector(n_eqn);
            }
        }

        // Elements
        GJ4Pfunct DevFunc;           // Differential equation
        int n_eqn;       // Dimension
        double StepSize;           // Step aboutSize
        Action pAux;        // Pointer to auxiliary satData requird by f
        Geo.Algorithm.Vector S2, S1;       // First and second sum of acceleration
        Geo.Algorithm.Vector[] D = new Geo.Algorithm.Vector[4];//[4];        // Backward differences of acceleration at t
        Geo.Algorithm.Vector[] d = new Geo.Algorithm.Vector[4];//[4];        // Backward differences of acceleration at t+h
        Geo.Algorithm.Vector r_p, v_p;     // Predictor
        

        /// <summary>
        /// 获取状态
        /// </summary>
        /// <param name="e"></param>
        /// <param name="t"></param>
        /// <param name="h"></param>
        /// <param name="step"></param>
        /// <returns></returns>
        public  Geo.Algorithm.Vector GetState(double e, double t, double h,   int step)
        {
            var r = new Geo.Algorithm.Vector(1.0 - e, 0.0, 0.0);
            var v = new Geo.Algorithm.Vector(0.0, Math.Sqrt((1 + e) / (1 - e)), 0.0);
           
            // Integration from t=t to t=t_end
            this.Init(t, r, v, h);
            for (int i = 1; i <= step; i++)
            {
                Step(ref t, out r, out v);
            }
            var y = r.Stack(v);
            return y;
        }

        /// <summary>
        ///  4th order Runge-Kutta step for 2nd order differential equation
        /// </summary>
        /// <param name="t"></param>
        /// <param name="r"></param>
        /// <param name="v"></param>
        /// <param name="h"></param>
        public void RK4(ref double t, ref Geo.Algorithm.Vector r, ref Geo.Algorithm.Vector v, double h)
        {
            Geo.Algorithm.Vector v_1, v_2, v_3, v_4;
            Geo.Algorithm.Vector a_1 = new Geo.Algorithm.Vector(r.Dimension), a_2 = new Geo.Algorithm.Vector(r.Dimension), a_3 = new Geo.Algorithm.Vector(r.Dimension), a_4 = new Geo.Algorithm.Vector(r.Dimension);

            v_1 = v; DevFunc(t, r, v_1, ref a_1, pAux);
            v_2 = v + (h / 2.0) * a_1; DevFunc(t + h / 2.0, r + (h / 2.0) * v_1, v_2, ref a_2, pAux);
            v_3 = v + (h / 2.0) * a_2; DevFunc(t + h / 2.0, r + (h / 2.0) * v_2, v_3, ref a_3, pAux);
            v_4 = v + h * a_3; DevFunc(t + h, r + h * v_3, v_4, ref a_4, pAux);

            t = t + h;
            r = r + (h / 6.0) * (v_1 + 2.0 * v_2 + 2.0 * v_3 + v_4);
            v = v + (h / 6.0) * (a_1 + 2.0 * a_2 + 2.0 * a_3 + a_4);

        }

        /// <summary>
        /// Initialization of backwards differences from initial conditions
        /// </summary>
        /// <param name="t_0"></param>
        /// <param name="r_0"></param>
        /// <param name="v_0"></param>
        /// <param name="h_"></param>
        public void Init(double t_0, Geo.Algorithm.Vector r_0, Geo.Algorithm.Vector v_0, double h_)
        {
            // Order of method

            const int m = 4;

            // Coefficients gamma/delta of 1st/2nd order Moulton/Cowell corrector method

            double[] gc = { +1.0, -1 / 2.0, -1 / 12.0, -1 / 24.0, -19 / 720.0 };
            double[] dc = { +1.0, -1.0, +1 / 12.0, 0.0, -1 / 240.0, -1 / 240.0 };

            int i, j;
            double t = t_0;
            Geo.Algorithm.Vector r = r_0;
            Geo.Algorithm.Vector v = v_0;

            // Save step aboutSize  

            this.StepSize = h_;

            // Create table of accelerations at past times t-3h, t-2h, and t-h using
            // RK4 steps

            DevFunc(t, r, v, ref D[0], pAux);     // D[i]=a(t-ih)
            for (i = 1; i <= m - 1; i++)
            {
                RK4(ref t, ref r, ref v, -StepSize); DevFunc(t, r, v, ref D[i], pAux);
            };

            // Compute backwards differences

            for (i = 1; i <= m - 1; i++)
                for (j = m - 1; j >= i; j--) D[j] = D[j - 1] - D[j];

            // Initialize backwards sums using 4th order GJ corrector

            S1 = v_0 / StepSize; for (i = 1; i <= m; i++) S1 -= gc[i] * D[i - 1];
            S2 = r_0 / (StepSize * StepSize) - dc[1] * S1; for (i = 2; i <= m + 1; i++) S2 -= dc[i] * D[i - 2];

        }

        /// <summary>
        /// Step from t to t+h
        /// </summary>
        /// <param name="t"></param>
        /// <param name="r"></param>
        /// <param name="v"></param>
        public void Step(ref double t, out Geo.Algorithm.Vector r, out Geo.Algorithm.Vector v)
        {
            // Order of method
            const int m = 4;

            // Coefficients gamma/delta of 1st/2nd order Bashforth/Stoermr predictor
            double[] gp = { +1.0, +1 / 2.0, +5 / 12.0, +3 / 8.0, +251 / 720.0 };
            double[] dp = { +1.0, 0.0, +1 / 12.0, +1 / 12.0, +19 / 240.0, +3 / 40.0 };

            int i;

            // 4th order predictor
            r_p = dp[0] * S2; for (i = 2; i <= m + 1; i++) r_p += dp[i] * D[i - 2]; r_p = (StepSize * StepSize) * r_p;
            v_p = gp[0] * S1; for (i = 1; i <= m; i++) v_p += gp[i] * D[i - 1]; v_p = StepSize * v_p;

            // Update backwards difference table

            DevFunc(t + StepSize, r_p, v_p, ref d[0], pAux);               // Acceleration at t+h
            for (i = 1; i <= m - 1; i++) d[i] = d[i - 1] - D[i - 1];      // New differences at t+h
            for (i = 0; i <= m - 1; i++) D[i] = d[i];               // Update differences 
            S1 += d[0]; S2 += S1;                        // Update sums

            // Update independent variable and solution

            t = t + StepSize;
            r = new Geo.Algorithm.Vector(r_p);
            v = new Geo.Algorithm.Vector(v_p);

        }
    }
}