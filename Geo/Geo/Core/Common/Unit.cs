//2016.02.09, czs, create in xi'an hongqing, 属性名称字符串

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using  Geo.Common;
using Geo.IO;

namespace Geo
{
    /// <summary>
    /// 单位枚举
    /// </summary>
    public enum Units
    {
        /// <summary>
        /// 米
        /// </summary>
        M,
        Degree,
        Minute,
        Second,
        DMS_S,
        HMS_S,
        Rad,
        Meter,
        
        Centimeter,
        CM,
        DM,
        MM,
        UM,
        NM,


    }

    /// <summary>
    /// 帮助类
    /// </summary>
    public class UnitHelper
    { 

        /// <summary>
        /// 解析为枚举
        /// </summary>
        /// <param name="unit"></param>
        /// <returns></returns>
        static public Units ParseUnitEnum(string unit)
        {
            if (unit == "°") return Units.Degree;
            if (unit == "′") return Units.Minute;
            if (unit == "″") return Units.Second;

            var name = (Units)Enum.Parse(typeof(Units), unit, true);
            return name;
        }
        /// <summary>
        /// 解析为枚举
        /// </summary>
        /// <param name="unit"></param>
        /// <returns></returns>
        static public Units ParseUnitEnum(Unit unit)
        {
            var name = ParseUnitEnum(unit.Name);
            return name;
        }
        /// <summary>
        /// 通过名称获取单位对象。
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        internal static Unit GetUnitByName(string name)
        {
            Units u = ParseUnitEnum(name);
            switch (u)
            {
                case Units.Degree: return Unit.Degree;
                case Units.Centimeter: return Unit.Centimeter;
                case Units.DMS_S: return Unit.DMS_S;
                case Units.M: 
                case Units.Meter: return Unit.Meter;
                case Units.DM:
                    return Unit.Decimeter;
                case Units.NM:
                    return Unit.Nanometer;
                case Units.MM:
                    return Unit.Millimeter;
                case Units.CM:
                    return  Unit.Centimeter;
                case  Units.UM:
                    return Unit.Micrometer;
                case Units.Minute: return Unit.Minute;
                case Units.Rad: return Unit.Rad;
                case Units.Second: return Unit.Second;
                case Units.HMS_S: return Unit.HMS_S;
                default:
                    throw new NotSupportedException("不支持的解析：" + name);

            }

        }
    }

    /// <summary>
    /// 单位类型
    /// </summary>
    public enum UnitType
    {
        Unkown,
        /// <summary>
        /// 长度
        /// </summary>
        Length,
        /// <summary>
        /// 角度
        /// </summary>
        Angle,
        /// <summary>
        /// 时间
        /// </summary>
        Time
    }


    /// <summary>
    /// 单位,如米
    /// </summary>
    public class Unit : Named
    {
        /// <summary>
        /// 构造方法,默认为单位简称
        /// </summary>
        /// <param name="name"></param>
        public Unit(string name, UnitType UnitType= Geo.UnitType.Unkown, double Scale = 0)
            : this(name, "", UnitType, Scale)
        {

        }
        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="name"></param>
        /// <param name="Abbreviation"></param>
        /// <param name="UnitType"></param>
        public Unit(string name, string Abbreviation, UnitType UnitType, double Scale)
            : base(name)
        {
            this.Gauge = Scale;
            this.Abbreviation = Abbreviation;
            this.UnitType = UnitType;
        }
        #region 属性
        /// <summary>
        /// 转换因子，选择一个基本的转换单位，以其为中心，进行转换。
        ///  [geɪdʒ]   美 [gedʒ]  习n. 计量器；标准尺寸；容量规格vt. 测量；估计；给…定规格
        /// </summary>
        public double Gauge { get; set; }

        /// <summary>
        /// 单位类型
        /// </summary>
        public UnitType UnitType { get; set; }

        /// <summary>
        /// 名称,获取则为第一个匹配的。设置则添加。
        /// </summary>
        public override string Name
        {
            get {    if (Names == null || Names.Count == 0)  return ""; return Names[0]; }
            set
            {
                if (String.IsNullOrWhiteSpace(value)) return;

                if (Names == null) { Names = new List<string>(); }

                if (!Names.Contains(value))
                {
                    Names.Add(value);
                }
            }
        }

        /// <summary>
        /// 简称,获取则为第一个匹配的。设置则添加。
        /// </summary>
        public string Abbreviation
        {
            get { if (Abbreviations == null || Abbreviations.Count == 0) return "";  return Abbreviations[0]; }
            set
            {
                if (String.IsNullOrWhiteSpace(value)) return;

                if (Abbreviations == null) { Abbreviations = new List<string>(); }

                if (!Abbreviations.Contains(value))
                {
                    Abbreviations.Add(value);
                }
            }
        }

        /// <summary>
        /// 名称
        /// </summary>
        public List<string> Names { get; set; }

        /// <summary>
        /// 简称，如M
        /// </summary>
        public List<string> Abbreviations { get; set; }
        #endregion

        #region 对象方法
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            if (!String.IsNullOrWhiteSpace(Name))
            {
                sb.Append(Name);
            }
            if (!String.IsNullOrWhiteSpace(Abbreviation))
            {
                if (sb.Length == 0)
                {
                    sb.Append(Abbreviation);
                }
                else
                {
                    sb.Append("(" + Abbreviation + ")");
                }
            }
            
            return sb.ToString();
        }
        
        /// <summary>
        /// 只要有个全称相等，则相等。否则比较简称，有一个则相等。
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            Unit unit = obj as Unit;
            if (unit == null) return false;
            //类型
            if (this.UnitType != unit.UnitType) return false;
            if (this.Gauge != unit.Gauge) return false;

            foreach (var item in Names)
            {
                if (unit.Names.Contains(item))
                {
                    return true;
                }
            }
            foreach (var item in Abbreviations)
            {
                if (unit.Abbreviations.Contains(item))
                {
                    return true;
                }
            }

            return false;
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode() + 13 * Abbreviation.GetHashCode();
        }
        #endregion
         

        #region 常见单位
        /// <summary>
        /// 米 Meter
        /// </summary>
        public static Unit Meter
        {
            get
            {
                return new Unit("Meter", "m", UnitType.Length, 1.0);
            }
        }
        /// <summary>
        /// 分米 Decimeter
        /// </summary>
        public static Unit Decimeter
        {
            get
            {
                return new Unit("Decimeter", "dm", UnitType.Length, 0.1);
            }
        }
        /// <summary>
        /// 厘米 Meter
        /// </summary>
        public static Unit Centimeter
        {
            get
            {
                return new Unit("Centimeter", "cm", UnitType.Length, 0.01);
            }
        }
        /// <summary>
        /// 毫米 Millimeter
        /// </summary>
        public static Unit Millimeter
        {
            get
            {
                return new Unit("Millimeter", "mm", UnitType.Length, 0.001);
            }
        }
        /// <summary>
        /// 微米 Micrometer
        /// </summary>
        public static Unit Micrometer
        {
            get
            {
                return new Unit("Micrometer", "um", UnitType.Length, 1e-6);
            }
        }
        /// <summary>
        /// 纳米 Nanometer
        /// </summary>
        public static Unit Nanometer
        {
            get
            {
                return new Unit("Nanometer", "nm", UnitType.Length, 1e-9);
            }
        }
        #region 时间


        /// <summary>
        /// 天，时间单位
        /// </summary>
        public static Unit Day
        {
            get
            {
                return new Unit("Day", "d", UnitType.Time, 86400.0);
            }
        }

        /// <summary>
        /// 小时，时间单位
        /// </summary>
        public static Unit Hour
        {
            get
            {
                return new Unit("Hour", "h", UnitType.Time, 3600.0);
            }
        }
        /// <summary>
        /// 分，分钟，时间单位
        /// </summary>
        public static Unit Minute
        {
            get
            {
                return new Unit("Minute", "′", UnitType.Time, 60.0);
            }
        }
        /// <summary>
        /// 单位为秒，时间
        /// </summary>
        public static Unit Second
        {
            get
            {
                var unit = new Unit("Second", "″", UnitType.Time, 1.0);
                unit.Abbreviations.Add("s");
                return unit;
            }
        }
        #endregion

        #region 角度
        /// <summary>
        /// 弧度 Rad，角度单位
        /// </summary>
        public static Unit Rad
        {
            get
            {
                return new Unit("Rad", "R", UnitType.Angle, 2035752.0395261860185237929123651);
            }
        }
        /// <summary>
        /// 度 Deree，角度单位
        /// </summary>
        public static Unit Degree
        {
            get
            {
                return new Unit("Degree", "°", UnitType.Angle, 3600.0);
            }
        }
        ///// <summary>
        ///// 角度分
        ///// </summary>
        //public static Unit AngleMinute
        //{
        //    get
        //    {
        //        return new Unit("Minute", "′", UnitType.Angle, 60.0);
        //    }
        //}
        ///// <summary>
        ///// 单位为秒，角度
        ///// </summary>
        //public static Unit AngleSecond
        //{
        //    get
        //    {
        //        return new Unit("Second", "″", UnitType.Angle, 1.0);
        //    }
        //}
        /// <summary>
        /// 度分秒小数，如123456.78为123°45′56.78″
        /// </summary>
        public static Unit DMS_S
        {
            get
            {
                return new Unit("DMS_S", "DMS_S", UnitType.Angle, 0);//0 代表需要其他转换
            }
        }

        /// <summary>
        /// 度.分秒小数，如123.45678为123°45′56.78″
        /// </summary>
        public static Unit D_MS
        {
            get
            {
                return new Unit("D_MS", "D_MS", UnitType.Angle, 0);//0 代表需要其他转换
            }
        }
        /// <summary>
        /// 时角，天文单位
        /// </summary>
        public static Unit HMS_S
        {
            get
            {
                return new Unit("HMS_S", "HMS_S", UnitType.Angle, 0);
            }
        }

        #endregion
        #endregion

    }
}
