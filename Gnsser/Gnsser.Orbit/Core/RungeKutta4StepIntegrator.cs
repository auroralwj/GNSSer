//2017.06.20, czs, funcKeyToDouble from c++ in hongqing, RK4
//2017.06.26, czs, edit in hongqing, format and refactor codes

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text; using Geo.Coordinates;

using Geo.Algorithm; using Geo.Utils; namespace Gnsser.Orbits
{

    //------------------------------------------------------------------------------
    //
    // RK4 class (specification)
    //
    //------------------------------------------------------------------------------

    // Function prototype for prevObj order differential equations
    // void f (double x, const Geo.Algorithm.Vector& y, Geo.Algorithm.Vector& yp )
    /// <summary>
    ///  Derivative y'=f(x,y)
    /// </summary>
    /// <param name="x">Independent variable</param>
    /// <param name="y">State vector  </param> 
    /// <param name="pAux"> Pointer to auxiliary satData used within f</param>
    /// <returns></returns>
    public delegate Geo.Algorithm.Vector RK4funct(  double x,  Geo.Algorithm.Vector y,   Object pAux    );


    /// <summary>
    /// 4阶龙格库塔法积分。
    /// RK4 class specification
    /// </summary> 
    public class RungeKutta4StepIntegrator
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="f_">Differential equation</param>
        /// <param name="n_eqn_">Dimension</param>
        /// <param name="pAux_">Pointer to auxiliary satData</param>
        public RungeKutta4StepIntegrator(RK4funct f_, int n_eqn_, Object pAux_)
        {
            n_eqn = (n_eqn_);
            DevFunc = (f_);
            pAux = (pAux_);
        }


        private RK4funct DevFunc;
        int n_eqn;
        Object pAux;
        Geo.Algorithm.Vector k_1, k_2, k_3, k_4;

        /// <summary>
        ///  Integration step
        /// </summary>
        /// <param name="t">Value of the independent variable; updated by t+h</param>
        /// <param name="y">Value of y(t); updated by y(t+h)</param>
        /// <param name="h">Step aboutSize</param>
        public void Step(ref double t, ref Geo.Algorithm.Vector y, double h)
        {
            // Elementary RK4 step
            k_1 = DevFunc(t, y, pAux);
            k_2 = DevFunc(t + h / 2.0, (y + (h / 2.0) * k_1), pAux);
            k_3 = DevFunc(t + h / 2.0, (y + (h / 2.0) * k_2), pAux);
            k_4 = DevFunc(t + h, (y + h * k_3), pAux);

            y = y + (h / 6.0) * (k_1 + 2.0 * k_2 + 2.0 * k_3 + k_4);

            // Update independent variable
            t = t + h;
        }
    }


}