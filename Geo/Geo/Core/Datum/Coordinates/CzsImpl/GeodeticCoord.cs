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
    public class GeodeticCoord : LonLatCoord, IGeodeticCoord
    {
        /// <summary>
        /// 默认构造函数。初始化为 Coordinate.Empty。
        /// </summary>
        public GeodeticCoord():this(null) {}

        /// <summary>
        /// 将一个隐含有 Lon, Lat, height的坐标转换为大地坐标。
        /// </summary>
        /// <param name="coord">含有 Lon, Lat, height的坐标转换</param>
        public GeodeticCoord(ICoordinate coord) 
            :this(coord.ReferenceSystem, coord[Ordinate.Lon], coord[Ordinate.Lat], coord[Ordinate.Height], coord.Weight)
        {
            //if (!coord.ContainsOrdinate(Ordinate.Lon)
            //   || !coord.ContainsOrdinate(Ordinate.Lat)
            //   || !coord.ContainsOrdinate(Ordinate.Height))
            //    throw new ArgumentException("不是大地坐标", "coord");

        }
        /// <summary>
        /// 由参考系统实例化坐标。
        /// </summary>
        /// <param name="referenceSystem">参考系统</param>
        public GeodeticCoord(ICoordinateReferenceSystem referenceSystem, double lon = 0, double lat = 0, double height = 0,double weight = 0)
            : base(referenceSystem, lon, lat, weight)
        {
            if (!ReferenceSystem.CoordinateSystem.Contains(Ordinate.Height) )
                throw new ArgumentException("参考系中没有 Height 轴", "referenceSystem");
            this.Height = height;
        }

        /// <summary>
        /// Lon 轴分量值。
        /// </summary>
        public double Height
        {
            get { return this[Ordinate.Height]; }
            set { this[Ordinate.Height] = value; }
        } 
    }
}
