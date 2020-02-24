//2014.05.24, czs, created 

using System;
using System.Collections.Generic;
using System.Text;
using Geo.Common;

namespace Geo.Referencing
{

    /// <summary>
    /// 椭球体。 同 Spheroid
    /// </summary>
    public interface IEllipsoid
    {
        /// <summary>
        /// Gets or sets the value of the axis unit.
        /// </summary>
        LinearUnit AxisUnit { get; set; }
        /// <summary>
        /// 长半轴
        /// </summary>
        double SemiMajorAxis { get; set; }
        /// <summary>
        /// 短半轴
        /// </summary>
        double SemiMinorAxis { get; set; }

        /// <summary>
        /// 扁率的倒数
        /// </summary>
        double InverseFlattening { get; set; }
        /// <summary>
        /// 扁率
        /// </summary>
        double Flattening { get; set; }
        /// <summary>
        /// 极曲率半径
        /// </summary>
        double PolarCurvatureSemiAxis { get; set; }
        /// <summary>
        /// 第一偏心率
        /// </summary>
        double FirstEccentricity { get; set; }
        /// <summary>
        /// 第二偏心率
        /// </summary>
        double SecondEccentricity { get; set; }
        /// <summary>
        /// gravitational constant
        /// </summary>
        double GM { get; set; }

        /// <summary>
        /// earth angular velocity 
        /// </summary>
        double AngleVelocity { get; set; }
    }
}