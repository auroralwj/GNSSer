using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gnsser.Data
{
    public class Gpt2Value
    {
        public Gpt2Value(double lat, double lon, pressure pre, T T, Q Q, dT dT, double undu, double Hs, ah ah, aw aw)
        {
            this.lat = lat;
            this.lon = lon;
            this.pre = pre;
            this.T = T;
            this.Q = Q;
            this.dT = dT;
            this.undu = undu;
            this.Hs = Hs;
            this.ah = ah;
            this.aw = aw;
        }
        /// <summary>
        /// 纬度
        /// </summary>
        public double lat;
        /// <summary>
        /// 经度
        /// </summary>
        public double lon;
        /// <summary>
        /// Pressure pressure in Pascal
        /// </summary>
        public pressure pre;
        /// <summary>
        /// temperature in Kelvin
        /// </summary>
        public T T;
        /// <summary>
        /// temperature lapse rate in Kelvin/m
        /// </summary>
        public dT dT;
        /// <summary>
        /// specific humidity in kg/kg
        /// </summary>
        public Q Q;
        /// <summary>
        /// 大地水准面起伏geoid undulation in m
        /// </summary>
        public double undu;
        /// <summary>
        /// orthometric grid height in m
        /// </summary>
        public double Hs;
        /// <summary>
        /// hydrostatic mapping function coefficient, dimensionless
        /// </summary>
        public ah ah;
        /// <summary>
        /// wet mapping function coefficient, dimensionless
        /// </summary>
        public aw aw;

    }
    /// <summary>
    /// Pressure pressure in Pascal
    /// </summary>
    public class pressure
    {
        public double a0;
        public double A1;
        public double B1;
        public double A2;
        public double B2;
    }
    /// <summary>
    /// 温度 temperature in Kelvin
    /// </summary>
     public class T
    {
         public double a0;
         public double A1;
         public double B1;
         public double A2;
         public double B2;
    }

    /// <summary>
    /// specific humidity in kg/kg
    /// </summary>
    public class Q
    {
        public double a0;
        public double A1;
        public double B1;
        public double A2;
        public double B2;
    }

    /// <summary>
    /// temperature lapse rate in Kelvin/m
    /// </summary>
    public class dT
    {
        public double a0;
        public double A1;
        public double B1;
        public double A2;
        public double B2;
    }

    /// <summary>
    /// hydrostatic mapping function coefficient, dimensionless
    /// </summary>
    public class ah
    {
        public double a0;
        public double A1;
        public double B1;
        public double A2;
        public double B2;
    }

    /// <summary>
    /// wet mapping function coefficient, dimensionless
    /// </summary>
    public class aw
    {
        public double a0;
        public double A1;
        public double B1;
        public double A2;
        public double B2;
    }
}
