// 2014.11.30, czs, create in jinxingliaomao shaungliao, 距离单位。

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Geo.Referencing
{
    /// <summary>
    /// 距离的单位
    /// </summary>
    public struct DistanceUnit 
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="scale">尺度</param>
        /// <param name="name">名称，通常为简称</param>
        /// <param name="fullName">名称</param>
        public DistanceUnit(double scale, string name, string fullName = "")
        {
            Scale = scale;
            Name = name;
            FullName = fullName;
        }
        /// <summary>
        /// 尺度，以米为基准。
        /// </summary>
        public double Scale;
        /// <summary>
        /// 名称
        /// </summary>
        public string Name;
        /// <summary>
        /// 全称，英文，如 KiloMeter。
        /// </summary>
        public string FullName;
        /// <summary>
        /// 名称，单位简称如 m。
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Name;
        }
        /// <summary>
        /// 是否相等
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (!(obj is DistanceUnit)) return false;
            DistanceUnit unit = (DistanceUnit)obj;
            return unit.Scale == Scale;
        }
        /// <summary>
        /// 哈希数
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return Scale.GetHashCode();
        }  
        /// <summary>
        /// 千米
        /// </summary>
        public static DistanceUnit Kilometer { get { return new DistanceUnit(1000, "km", "Kilometer"); } }

        /// <summary>
        /// 米
        /// </summary>
        public static DistanceUnit Meter { get { return new DistanceUnit(1, "m", "Meter"); } }
        /// <summary>
        /// 分米
        /// </summary>
        public static DistanceUnit Decimeter { get { return new DistanceUnit(1e-1, "dm", "Decimeter"); } }
        /// <summary>
        /// 厘米
        /// </summary>
        public static DistanceUnit Centimeter { get { return new DistanceUnit(1e-2, "cm", "Centimeter"); } }
        /// <summary>
        /// 毫米
        /// </summary>
        public static DistanceUnit Millimeter { get { return new DistanceUnit(1e-3, "mm", "Millimeter"); } }
        /// <summary>
        /// 微米
        /// </summary>
        public static DistanceUnit MicroMeter { get { return new DistanceUnit(1e-6, "μm", "MicroMeter"); } }
        /// <summary>
        /// 纳米
        /// </summary>
        public static DistanceUnit NanoMeter { get { return new DistanceUnit(1e-9, "nm", "NanoMeter"); } }
        /// <summary>
        /// 皮米
        /// </summary>
        public static DistanceUnit Picometer { get { return new DistanceUnit(1e-12, "pm", "Picometer"); } }
        /// <summary>
        /// 飞米
        /// </summary>
        public static DistanceUnit Femtometer { get { return new DistanceUnit(1e-15, "fm", "Femtometer"); } }
        /// <summary>
        /// 阿米（英文待更正）
        /// </summary>
        public static DistanceUnit Ameter { get { return new DistanceUnit(1e-18, "am", "Ameter"); } }
      
 
  }
}
