//2014.06.06,czs,created

using System;
using System.Collections.Generic;
using System.Text;
using Geo.Utils;
using Geo.Referencing;


namespace Geo.Coordinates
{
    /// <summary>
    /// 具有 X Y 分量的坐标。
    /// </summary>
    [Serializable]
    public class LonLatCoord : Coordinate, ILonLatCoord
    {
        /// <summary>
        /// 默认构造函数。初始化为 Coordinate.Empty。
        /// </summary>
        public LonLatCoord():this(null) {}

        
        /// <summary>
        /// 将一个隐含有 Lon, Lat 的坐标转换为 Lonlat 坐标。
        /// </summary>
        /// <param name="coord">含有 Lon, Lat的坐标转换</param>
        public LonLatCoord(ICoordinate coord) 
            :this(coord.ReferenceSystem, coord[Ordinate.Lon], coord[Ordinate.Lat], coord.Weight)
        {
         }
        /// <summary>
        /// 由参考系统实例化坐标。
        /// </summary>
        /// <param name="referenceSystem">参考系统</param>
        public LonLatCoord(ICoordinateReferenceSystem referenceSystem, double lon = 0, double lat = 0, double weight = 0)
            : base(referenceSystem, weight)
        {
            if (!ReferenceSystem.CoordinateSystem.Contains(Ordinate.Lon)
                || !ReferenceSystem.CoordinateSystem.Contains(Ordinate.Lat))
                throw new ArgumentException("参考系中没有 Lon Lat 轴", "referenceSystem");
            this.Lon = lon;
            this.Lat = lat;
        }

        /// <summary>
        /// Lon 轴分量值。
        /// </summary>
        public double Lon
        {
            get { return this[Ordinate.Lon]; }
            set { this[Ordinate.Lon] = value; }
        }

        /// <summary>
        /// Lat 轴分量值。
        /// </summary>
        public double Lat
        {
            get { return this[Ordinate.Lat]; }
            set { this[Ordinate.Lat] = value; }
        }
        /// <summary>
        /// 角度单位
        /// </summary>
        public AngleUnit Unit { get; set; }
    }
}
