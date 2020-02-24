using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace GeodeticX
{
    /// <summary>
    /// 大地坐标集合
    /// </summary>
    [ClassInterface(ClassInterfaceType.None)]
    public class GeoPoints : List<GeoPoint>
    {
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="B"></param>
        /// <param name="L"></param>
        public void Add(double B, double L)
        {
            GeoPoint point = new GeoPoint(B, L);
            this.Add(point);
        }
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="B"></param>
        /// <param name="L"></param>
        /// <param name="H"></param>
        public void Add(double B, double L, double H)
        {
            GeoPoint point = new GeoPoint(B, L, H);
            this.Add(point);
        }
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="BLH"></param>

        public void Add(GeodeticCoordinate BLH)
        {
            GeoPoint point = new GeoPoint(BLH);
            this.Add(point);
        }
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="XYZ"></param>
        public void Add(SpatialRectCoordinate XYZ)
        {
            GeoPoint point = new GeoPoint(XYZ);
            this.Add(point);
        }
        /// <summary>
        /// 添加 
        /// </summary>
        /// <param name="xyh"></param>
        public void Add(GaussCoordinate xyh)
        {
            GeoPoint point = new GeoPoint(xyh);            
            this.Add(point);
        }
    }
}
