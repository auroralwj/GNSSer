//2014.05.24, czs, created 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Geo.Common;

namespace Geo.Referencing
{
    
    /// <summary>
    /// 以角度为单位。
    /// 默认以弧度作为转换基准。
    /// </summary>
    public class AngularUnit : Unit
    { 
        /// <summary>
        /// 创建一个实例。
        /// </summary>
        /// <param name="RadiansPerUnit">尺度转换因子</param>
        public AngularUnit(double RadiansPerUnit = 1) : base(RadiansPerUnit) {}
        /// <summary>
        /// 以弧度为基准。
        /// 一个单位具有多少弧度。
        /// </summary>
        public double RadiansPerUnit { get { return ConversionFactor; } set { ConversionFactor = value; } }

        /// <summary>
        /// 弧度。以弧度作为转换基准。
        /// </summary>
        public static AngularUnit Radian
        {
            get
            {
                return new AngularUnit()
                {
                    RadiansPerUnit = 1,
                    Name = "radian",
                    Id = "9101",
                    Abbreviation = "rad"
                };
            }
        }

        /// <summary>
        /// 度。°以弧度作为转换基准。
        /// </summary>
        public static AngularUnit Degree
        {
            get
            {
                return new AngularUnit()
                {
                    RadiansPerUnit = 0.017453292519943295769236907684886,
                    Name = "degree",
                    Id = "9102",
                    Abbreviation = "deg"
                };
            }
        }

        /// <summary>
        ///以弧度作为转换基准。百分度制角度单位 定义 Gon，百分度制(GRAD)角度单位，用G表示。1G = 直角的1%。 
        ///与其他角度单位制换算关系 角度制(DEG) 1G = 0.9° 弧度制(RAD) 
        /// Pi / 200 = 0.015707963267948966192313216916398 radians
        /// </summary>
        public static AngularUnit Grad
        { get
            {
                return new AngularUnit()
                {
                    RadiansPerUnit = 0.015707963267948966192313216916398,
                    Name = "grad",
                    Id = "9105",
                    Abbreviation = "gr"
                };
            }
         }

        /// <summary>
        ///以弧度作为转换基准。百分度制角度单位 定义 Gon，百分度制(GRAD)角度单位，用G表示。1G = 直角的1%。 
        ///与其他角度单位制换算关系 角度制(DEG) 1G = 0.9° 弧度制(RAD) 
        /// Pi / 200 = 0.015707963267948966192313216916398 radians
        /// </summary>		
        public static AngularUnit Gon
        {get
            {
                return new AngularUnit()
                {
                    RadiansPerUnit = 0.015707963267948966192313216916398,
                    Name = "gon",
                    Id = "9106",
                    Abbreviation = "g"
                };
            }
         }
    }




}
