using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gnsser.Data
{
    public class Gpt2Value1Degree:Gpt2Value
    {
        public Gpt2Value1Degree(double lat, double lon, pressure pre, T T, Q Q, dT dT, double undu, double Hs, ah ah, aw aw, la la, Tm Tm)
            : base(lat, lon, pre, T, Q, dT, undu, Hs, ah, aw)
        {
            this.Tm = Tm;
            this.la = la;
        }
          /// <summary>
        /// % water vapor decrease factor, dimensionless
        /// </summary>
        public la la;
        /// <summary>
        /// % mean temperature in Kelvin
        /// </summary>
        public Tm Tm;      

    }
    //water vapor decrease factor
    public class la
    {
        public double a0;
        public double A1;
        public double B1;
        public double A2;
        public double B2;
    }

    /// <summary>
    ///   mean temperature of the water vapor in degrees Kelvin
    /// </summary>
    public class Tm
    {
        public double a0;
        public double A1;
        public double B1;
        public double A2;
        public double B2;
    }
}
