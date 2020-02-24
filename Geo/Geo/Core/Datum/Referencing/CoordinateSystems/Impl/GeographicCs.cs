//2014.05.24, czs, created 

using System;
using System.Collections.Generic;
using System.Text;
using Geo.Common;
using System.Globalization;

namespace Geo.Referencing
{
   
    /// <summary>
    /// 基于经纬度的地理坐标系统。
    /// </summary>
    /// <remarks>
    /// 一些坐标系统为 Lat/Lon 格式，而一些事 Lon/Lat，另外，有些是角度，而有些是弧度。
    /// 使用前需要判断确定。
    /// </remarks>
    public class GeographicCs : HorizontalCs
    {
        /// <summary>
        /// 建立一个实例。
        /// </summary>
        /// <param name="angularUnit">Angular units</param>
        /// <param name="horizontalDatum">Horizontal datum</param>
        /// <param name="primeMeridian">Prime meridian</param>
        /// <param name="Axis">Axis info</param>
        /// <param name="name">Name</param> 
        /// <param name="authorityCode">Authority-specific identification code.</param>
        public GeographicCs(
            AngularUnit angularUnit, 
            HorizontalDatum horizontalDatum,
            PrimeMeridian primeMeridian,
            List<IAxis> Axis, 
            string name = null,
            string authorityCode = null)
            :
            base(horizontalDatum, Axis, name,  authorityCode+"")
        {
            AngularUnit = angularUnit;
            PrimeMeridian = primeMeridian;
        }

        #region Predefined geographic coordinate systems

        /// <summary>
        /// Creates a decimal degrees geographic coordinate system based on the WGS84 ellipsoid, suitable for GPS measurements
        /// </summary>
        public static GeographicCs WGS84
        {
            get
            {
                List<IAxis> axes = new List<IAxis>(2);
                axes.Add(new Axis("Lon", Direction.East));
                axes.Add(new Axis("Lat", Direction.North));
                return new GeographicCs(AngularUnit.Degree,
                    HorizontalDatum.WGS84, PrimeMeridian.Greenwich, axes,
                    "WGS 84",  "4326");
            }
        }

        #endregion

        #region GeographicCoordinateSystem Members
         

        /// <summary>
        /// Gets or sets the angular units of the geographic coordinate system.
        /// </summary>
        public AngularUnit AngularUnit { get; set; }

        /// <summary>
        /// 首子午线。
        /// </summary>
        public PrimeMeridian PrimeMeridian { get; set; }

        /// <summary>
        /// Gets the number of available conversions to WGS84 coordinates.
        /// </summary>
        public int NumConversionToWGS84
        {
            get { return WGS84ConversionInfo.Count; }
        }
         
        public List<BursaTransParams> WGS84ConversionInfo { get; set; }

        /// <summary>
        /// Gets details on a conversion to WGS84.
        /// </summary>
        public BursaTransParams Wgs84ConversionInfo(int index)
        {
            return WGS84ConversionInfo[index];
        }
        /// <summary>
        /// 哈希代码。
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return PrimeMeridian.GetHashCode() * 3 + AngularUnit.GetHashCode() * 13 ;
        }

        /// <summary>
        /// 是否相等。
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>True if equal</returns>
        public override bool Equals(object obj)
        {
            if (!(obj is GeographicCs))
                return false;
            GeographicCs gcs = obj as GeographicCs;
            if (gcs.Dimension != this.Dimension) return false;
            if (this.WGS84ConversionInfo != null && gcs.WGS84ConversionInfo == null) return false;
            if (this.WGS84ConversionInfo == null && gcs.WGS84ConversionInfo != null) return false;
            if (this.WGS84ConversionInfo != null && gcs.WGS84ConversionInfo != null)
            {
                if (this.WGS84ConversionInfo.Count != gcs.WGS84ConversionInfo.Count) return false;
                for (int i = 0; i < this.WGS84ConversionInfo.Count; i++)
                    if (!gcs.WGS84ConversionInfo[i].Equals(this.WGS84ConversionInfo[i]))
                        return false;
            }
            if (this.Axes.Count != gcs.Axes.Count) return false;
            for (int i = 0; i < gcs.Axes.Count; i++)
                if (gcs.Axes[i].Orientation != this.Axes[i].Orientation)
                    return false;
            return gcs.AngularUnit.Equals(this.AngularUnit) &&
                    gcs.HorizontalDatum.Equals(this.HorizontalDatum) &&
                    gcs.PrimeMeridian.Equals(this.PrimeMeridian);
        }
        #endregion
    }


}
