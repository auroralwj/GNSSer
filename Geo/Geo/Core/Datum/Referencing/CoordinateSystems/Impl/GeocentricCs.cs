//2014.05.24, czs, created 

using System;
using System.Collections.Generic;
using System.Text;
using Geo.Common;
using System.Globalization;

namespace Geo.Referencing
{
    
    /// <summary>
    /// 地心三维坐标系。
    /// </summary>
    public class GeocentricCs : CoordinateSystem
    { 
        /// <summary>
        /// 构造函数
        /// </summary>
        public GeocentricCs():base() { }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="PrimeMeridian">首子午线</param>
        /// <param name="HorizontalDatum">基准</param>
        /// <param name="LinearUnit">尺度计量单位</param>
        /// <param name="axes">坐标轴</param>
        /// <param name="name">坐标系统名称</param>
        /// <param name="id">坐标系统编号</param>
        public GeocentricCs( 
            PrimeMeridian PrimeMeridian, 
            HorizontalDatum HorizontalDatum,
            LinearUnit LinearUnit,
            List<IAxis> axes,
            string name = null,
            string id = null)
            :base(axes,  name,  id)
        {
            this.HorizontalDatum = HorizontalDatum;
            this.HorizontalDatum = HorizontalDatum;
            this.LinearUnit = LinearUnit;
        } 
        /// <summary>
        /// Returns the HorizontalDatum. The horizontal datum is used to determine where
        /// the centre of the Earth is considered to be. All coordinate points will be 
        /// measured from the centre of the Earth, and not the surface.
        /// </summary>
        public HorizontalDatum HorizontalDatum { get; set; }
        /// <summary>
        /// 坐标轴的线性计量单位.
        /// </summary>
        public LinearUnit LinearUnit { get; set; }
        /// <summary>
        /// 初始子午线。
        /// </summary>
        public PrimeMeridian PrimeMeridian { get; set; }

        #region Predefined geographic coordinate systems

        /// <summary>
        /// Creates a geocentric coordinate system based on the WGS84 ellipsoid, suitable for GPS measurements
        /// </summary>
        public static GeocentricCs WGS84
        {
            get
            {
                return new CoordinateSystemFactory().CreateGeocentricCs("WGS84 Geocentric",
                    HorizontalDatum.WGS84, LinearUnit.Metre,
                    PrimeMeridian.Greenwich);
            }
        }

        #endregion

        /// <summary>
        /// 哈希数
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return HorizontalDatum.GetHashCode() * 9 + PrimeMeridian.GetHashCode() * 13;
        }

        /// <summary>
        /// 是否相等
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>True if equal</returns>
        public override bool Equals(object obj)
        {
            if (!(obj is GeocentricCs))
                return false;
            GeocentricCs gcc = obj as GeocentricCs;
            return gcc.HorizontalDatum.Equals(this.HorizontalDatum) &&
                gcc.LinearUnit.Equals(this.LinearUnit) &&
                gcc.PrimeMeridian.Equals(this.PrimeMeridian);
        }

    }

     
}
