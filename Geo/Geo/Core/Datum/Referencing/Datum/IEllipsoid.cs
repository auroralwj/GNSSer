//2014.05.24, czs, created 

using System;
using System.Collections.Generic;
using System.Text;
using Geo.Common;

namespace Geo.Referencing
{

    /// <summary>
    /// �����塣 ͬ Spheroid
    /// </summary>
    public interface IEllipsoid
    {
        /// <summary>
        /// Gets or sets the value of the axis unit.
        /// </summary>
        LinearUnit AxisUnit { get; set; }
        /// <summary>
        /// ������
        /// </summary>
        double SemiMajorAxis { get; set; }
        /// <summary>
        /// �̰���
        /// </summary>
        double SemiMinorAxis { get; set; }

        /// <summary>
        /// ���ʵĵ���
        /// </summary>
        double InverseFlattening { get; set; }
        /// <summary>
        /// ����
        /// </summary>
        double Flattening { get; set; }
        /// <summary>
        /// �����ʰ뾶
        /// </summary>
        double PolarCurvatureSemiAxis { get; set; }
        /// <summary>
        /// ��һƫ����
        /// </summary>
        double FirstEccentricity { get; set; }
        /// <summary>
        /// �ڶ�ƫ����
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