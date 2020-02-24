//2017.06.20, czs, funcKeyToDouble from c++ in hongqing, Exersise3.2_LunarEphemerides
//2017.06.24, czs, edit in hongqing, format and refactor codes

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text; using Geo.Coordinates;

using Geo.Algorithm; using Geo.Utils; namespace Gnsser.Orbits
{ 
    /// <summary>
    /// 简易 多维 切比雪夫 拟合。将所有输入数据进行拟合。
    /// Chebyshev approximation of 3-dimensional vectors
    /// Cheb3D class (specification and implementation)
    /// </summary>
    public class MultiDimChebshevFitter
    {
        private  int Order;       // Number of coefficients
        double StartEpoch;      // Begin interval
        double EndEpoch;      // End interval
        Geo.Algorithm.Vector[] Inputs;
        /// <summary>
        /// 拟合向量的维数
        /// </summary>
        public int Dimension { get { return Inputs.Length; } } 

        /// <summary>
        /// Chebyshev approximation of 3-dimensional vectors
        /// </summary> 
        /// <param name="startEpoch">Begin interval</param>
        /// <param name="endEpoch">End interval</param>
        /// <param name="cx">Coefficients of Chebyshev polyomial (x-coordinate)</param>
        /// <param name="cy">Coefficients of Chebyshev polyomial (y-coordinate)</param>
        /// <param name="cz">Coefficients of Chebyshev polyomial (z-coordinate)</param>
        public MultiDimChebshevFitter(double startEpoch, double endEpoch,
                  params Geo.Algorithm.Vector[] inputs)
        {
            Inputs = inputs;
            Order = Inputs[0].Dimension; StartEpoch = (startEpoch); EndEpoch = (endEpoch); 
        }        

       /// <summary>
        ///   Evaluation of the Chebyshev approximation of a three-dimensional vector
       /// </summary>
       /// <param name="epoch"></param>
       /// <returns></returns>
        public Geo.Algorithm.Vector Fit(double epoch)
        {
            // Variables 
            Geo.Algorithm.Vector f1 = new Geo.Algorithm.Vector(Dimension), f2 = new Geo.Algorithm.Vector(Dimension);
            double tau;

            // Check validity
            if ((epoch < StartEpoch) || (EndEpoch < epoch))
            {
                throw new ArgumentException("Time out of range in Cheb3D::Value");
            }

            // Clenshaw algorithm
            tau = (2.0 * epoch - StartEpoch - EndEpoch) / (EndEpoch - StartEpoch);
            for (int i = Order - 1; i >= 1; i--)
            {
                var  old_f1 = f1;
                f1 = 2.0 * tau * f1 - f2 + GetRawValue(i);// new Vector(Cx[i], Cy[i], Cz[i]);
                f2 = old_f1;
            }

            return tau * f1 - f2 + GetRawValue(0);// new Vector(Cx[0], Cy[0], Cz[0]);
        }
        /// <summary>
        /// 获取原始值
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        protected Geo.Algorithm.Vector GetRawValue(int index)
        {
            Geo.Algorithm.Vector v = new Geo.Algorithm.Vector(Dimension);
            for (int i = 0; i < Dimension; i++)
            {
                v[i] = Inputs[i][index];
            }
            return v;
        }
    }
}