
//2017.06.20, czs, funcKeyToDouble from c++ in hongqing, Exersise3.4_OrbitPerturbations
//2017.06.27, czs, edit in hongqing, format and refactor codes

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text; using Geo.Coordinates;

using Geo.Algorithm; using Geo.Utils; namespace Gnsser.Orbits
{

    /// <summary>
    /// Record for passing global satData between Deriv and the calling program 
    /// </summary> 
    public class ForceModelOption
    {
        public ForceModelOption()
        {

        }
        /// <summary>
        /// 历元
        /// </summary>
        public double Mjd0_TT { get; set; }
        /// <summary>
        /// [m^2]  Remote sensing satellite
        /// </summary>
        public double Area { get; set; }
        /// <summary>
        /// 质量
        /// </summary>
        public double Mass { get; set; }
        /// <summary>
        /// 太阳光压系数
        /// </summary>
        public double CoefOfRadiation { get; set; }
        /// <summary>
        /// 大气延迟系数
        /// </summary>
        public double CoefOfDrag { get; set; }
        /// <summary>
        /// 最大维度
        /// </summary>
        public int MaxDegree { get; set; }
        /// <summary>
        /// 最大阶次
        /// </summary>
        public int MaxOrder { get; set; }
        /// <summary>
        /// 是否启用太阳质量影响
        /// </summary>
        public bool EnableSun { get; set; }
        /// <summary>
        /// 是否启用月亮质量影响
        /// </summary>
        public bool EnableMoon { get; set; }
        /// <summary>
        /// 是否启用太阳光压影响
        /// </summary>
        public bool EnableSolarRadiation { get; set; }
        /// <summary>
        /// 是否考虑大气影响
        /// </summary>
        public bool EnableDrag { get; set; }
    }

    /// <summary>
    /// 卫星加速度计算器。
    /// </summary>
    public class AccelerationCalculator
    {
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="option"></param>
        /// <param name="IERS"></param>
        public AccelerationCalculator(ForceModelOption option, IERS IERS)
        {
            this.IERS = IERS;
            this.Option = option;
        }
        /// <summary>
        /// 选项
        /// </summary>
        public ForceModelOption Option { get; set; }
        /// <summary>
        /// 时间、参考架转换
        /// </summary>
        public  IERS IERS { get; set; }

        /// <summary>
        ///   Computes the acceleration of an Earth orbiting satellite due to 
        ///    - the Earth's harmonic gravity field, 
        ///    - the gravitational perturbations of the Sun and Moon
        ///    - the solar radiation pressure and
        ///    - the atmospheric drag
        /// </summary>
        /// <param name="Mjd_TT">Mjd_TT      Terrestrial Time (Modified Julian Date)</param>
        /// <param name="r">r           Satellite position vector in the ICRF/EME2000 system</param>
        /// <param name="v">v           Satellite velocity vector in the ICRF/EME2000 system</param>
        /// <param name="Area"> Area        Cross-section </param>
        /// <param name="mass"> mass        Spacecraft mass</param>
        /// <param name="coefOfRadiation">coefOfRadiation          Radiation pressure coefficient</param>
        /// <param name="coefOfDrag">coefOfDrag          Drag coefficient</param>
        /// <param name="n"></param>
        /// <param name="m"></param>
        /// <param name="isConsiderSun">是否启用太阳质量影响</param>
        /// <param name="isConsiderMoon">是否启用月亮质量影响</param>
        /// <param name="isConsiderSolarRadi">是否启用太阳光压影响</param>
        /// <param name="isConsiderDrag">是否考虑大气影响</param>
        /// <returns>Acceleration (a=d^2r/dt^2) in the ICRF/EME2000 system</returns>
        public Geo.Algorithm.Vector GetAcceleration(double Mjd_TT, Geo.Algorithm.Vector r, Geo.Algorithm.Vector v)
        {
            // Acceleration due to harmonic gravity field
            var Mjd_UT1 = Mjd_TT + (IERS.GetUT1_UTC(Mjd_TT) - IERS.GetTT_UTC(Mjd_TT)) / 86400.0;

            var T = IERS.NutMatrix(Mjd_TT) * IERS.PrecessionMatrix(OrbitConsts.MJD_J2000, Mjd_TT);
            var E = IERS.GreenwichHourAngleMatrix(Mjd_UT1) * T;

            var a = Force.AccelerOfHarmonicGraviFiled(r, E, Force.Grav.GM, Force.Grav.R_ref, Force.Grav.CS, Option.MaxDegree, Option.MaxOrder);

            // Luni-solar perturbations 
            var r_Sun = CelestialUtil.Sun(Mjd_TT);

            if (Option.EnableSun)
            {
                a += Force.AccelerOfPointMass(r, r_Sun, OrbitConsts.GM_Sun);
            }
            if (Option.EnableMoon)
            {
                var r_Moon = CelestialUtil.Moon(Mjd_TT);
                a += Force.AccelerOfPointMass(r, r_Moon, OrbitConsts.GM_Moon);
            }

            // Solar radiation pressure
            if (Option.EnableSolarRadiation)
            {
                a += Force.AccelerOfSolarRadiPressure(r, r_Sun, Option.Area, Option.Mass, Option.CoefOfRadiation, OrbitConsts.PressureOfSolarRadiationPerAU, OrbitConsts.AU);
            }
            // Atmospheric drag
            if (Option.EnableDrag)
            {
                a += Force.AccelerDragOfAtmos(Mjd_TT, r, v, T, Option.Area, Option.Mass, Option.CoefOfDrag);
            }
            // Acceleration
            return a;
        }
    }

}