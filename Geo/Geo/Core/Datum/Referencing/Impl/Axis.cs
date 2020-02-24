//2014.05.24, czs, created 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Geo.Common;

namespace Geo.Referencing
{
    /// <summary>
    /// 坐标轴。
    /// 空间参考系统的坐标轴方向，如果没有指定，就使用默认的，默认的指定方向如下：
    /// 地理坐标系统: AXIS[“Lon”,EAST],AXIS[“Lat”,NORTH]
    /// 投影坐标系统: AXIS[“X”,EAST],AXIS[“Y”,NORTH]
    /// 地心坐标系统: AXIS[“X”,OTHER],AXIS[“Y”,EAST],AXIS[“Z”,NORTH]
    /// </summary>
    public class Axis : Named, Geo.Referencing.IAxis
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public Axis() {} 
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="name"></param>
        /// <param name="ordinate"></param>
        public Axis(string name, Ordinate ordinate, Unit Unit = null)
            : this(name, new Orientation(Direction.Other), ordinate, Unit) { }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="name"></param>
        /// <param name="Direction"></param>
        public Axis(string name, Direction Direction = Direction.Other, Ordinate ordinate = Ordinate.Other, Unit Unit = null)
            : this(name, new Orientation(Direction), ordinate, Unit) { }
     
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="name"></param>
        /// <param name="Orientation"></param>
        /// <param name="ordinate"></param>
        /// <param name="Unit"></param>
        public Axis(string name, Orientation Orientation, Ordinate ordinate = Ordinate.Other, Unit Unit = null)
        {
            this.Name = name;
            this.Orientation = Orientation;
            this.Ordinate = ordinate;
            this.Unit = Unit;
        }
        /// <summary>
        /// 坐标轴尺度
        /// </summary>
        public Unit Unit { get; set; }

        /// <summary>
        /// 坐标轴类型。
        /// </summary>
        public Ordinate Ordinate { get; set; }
        /// <summary>
        ///指向，坐标轴的指向。
        /// </summary>
        public Orientation Orientation { get; set; }

        /// <summary>
        /// 比较其内容是否相等。
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            Axis ax = obj as Axis;
            if (ax == null) return false;

            if (ax.Ordinate != Ordinate) return false;
            if (ax.Name != Name) return false;
            if (!ax.Unit.Equals(Unit)) return false;
            if (!ax.Orientation.Equals(Orientation)) return false;
            return true;
        }
        public override int GetHashCode()
        { 
            return Ordinate.GetHashCode() + Unit.GetHashCode()*13;
        }

        #region 常用坐标轴
        /// <summary>
        /// X 轴，单位为米。
        /// </summary>
        public static Axis X= new Axis("X", Ordinate.X, LinearUnit.Metre);
        /// <summary>
        /// Y 轴，单位为米。
        /// </summary>
        public static Axis Y= new Axis("Y", Ordinate.Y, LinearUnit.Metre);
        /// <summary>
        /// Z 轴，单位为米。
        /// </summary>
        public static Axis Z= new Axis("Z", Ordinate.Z, LinearUnit.Metre);
        /// <summary>
        /// Height 轴，单位为米。
        /// </summary>
        public static Axis Height= new Axis("Height", Ordinate.Height, LinearUnit.Metre);
        /// <summary>
        /// Lon 轴，单位为度。
        /// </summary>
        public static Axis Lon= new Axis("Lon", Ordinate.Lon, AngularUnit.Degree);
        /// <summary>
        /// Lat 轴，单位为度。
        /// </summary>
        public static Axis Lat= new Axis("Lat", Ordinate.Lat, AngularUnit.Degree);
        /// <summary>
        /// Time 轴，单位为秒。
        /// </summary>
        public static Axis Time= new Axis("Time", Ordinate.Time, TimeUnit.Second);
        /// <summary>
        /// Radius 轴，主要是球面坐标中，单位米。
        /// </summary>
        public static Axis Radius= new Axis("Radius", Ordinate.Radius, LinearUnit.Metre);
        /// <summary>
        /// 上方向 轴 米
        /// </summary>
        public static Axis Up= new Axis("Up", Ordinate.Up, LinearUnit.Metre);
        /// <summary>
        /// 北方向轴 米
        /// </summary>
        public static Axis North= new Axis("North", Ordinate.North, LinearUnit.Metre);
        /// <summary>
        /// 东方向轴 米
        /// </summary>
        public static Axis East= new Axis("East", Ordinate.East, LinearUnit.Metre); 

 /// <summary>
        /// 东方向轴 米
        /// </summary>
        public static Axis Azimuth= new Axis("Azimuth", Ordinate.East, LinearUnit.Metre);
        /// <summary>
        /// 高度角 度
        /// </summary>
        public static Axis ElevatAngle= new Axis("ElevatAngle", Ordinate.Azimuth, AngularUnit.Degree);


        #endregion
    }
}
