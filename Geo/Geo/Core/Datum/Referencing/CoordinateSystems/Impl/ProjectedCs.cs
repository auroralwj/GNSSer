//2014.05.24, czs, created 

using System;
using System.Collections.Generic;
using System.Text;
using Geo.Common;
using System.Globalization;

namespace Geo.Referencing
{
   
    /// <summary>
    /// 2D投影坐标系统
    /// </summary>
    public class ProjectedCs : HorizontalCs
    {
        /// <summary>
        /// 初始化一个  2D投影坐标系统 实例。
        /// </summary>
        /// <param name="datum">Horizontal datum</param>
        /// <param name="geographicCs">Geographic coordinate system</param>
        /// <param name="linearUnit">Linear unit</param>
        /// <param name="projection">Projection</param>
        /// <param name="Axis">Axis info</param>
        /// <param name="name">Name</param>
        /// <param name="id">Authority-specific identification code.</param>
        public ProjectedCs(
            HorizontalDatum datum, 
            GeographicCs geographicCs,
            LinearUnit linearUnit,
            Projection projection, 
            List<IAxis> Axis,
            string name=null,
            string id=null)
            : base(datum, Axis, name, id)
        {
            GeographicCs = geographicCs;
            LinearUnit = linearUnit;
            Projection = projection;
        }

        #region Predefined projected coordinate systems

        /// <summary>
        /// Universal Transverse Mercator - WGS84
        /// </summary>
        /// <param name="Zone">UTM zone</param>
        /// <param name="ZoneIsNorth">true of Northern hemisphere, false if southern</param>
        /// <returns>UTM/WGS84 coordsys</returns>
        public static ProjectedCs WGS84_UTM(int Zone, bool ZoneIsNorth)
        {
            List<ProjectionParameter> pInfo = new List<ProjectionParameter>();
            pInfo.Add(new ProjectionParameter("latitude_of_origin", 0));
            pInfo.Add(new ProjectionParameter("central_meridian", Zone * 6 - 183));
            pInfo.Add(new ProjectionParameter("scale_factor", 0.9996));
            pInfo.Add(new ProjectionParameter("false_easting", 500000));
            pInfo.Add(new ProjectionParameter("false_northing", ZoneIsNorth ? 0 : 10000000));
            //Projection projection = cFac.CreateProjection("UTM" + Zone.ToString() + (ZoneIsNorth ? "N" : "S"), "Transverse_Mercator", parameters);
            Projection proj = new Projection("Transverse_Mercator", pInfo, "UTM" + Zone.ToString(System.Globalization.CultureInfo.InvariantCulture) + (ZoneIsNorth ? "N" : "S"),
                ( 32600 + Zone + (ZoneIsNorth ? 0 : 100))+"");
            System.Collections.Generic.List<IAxis> axes = new List<IAxis>();
            axes.Add(new Axis("East", Direction.East));
            axes.Add(new Axis("North", Direction.North));
            return new ProjectedCs(HorizontalDatum.WGS84,
                GeographicCs.WGS84, LinearUnit.Metre, proj, axes,
                "WGS 84 / UTM zone " + Zone.ToString(System.Globalization.CultureInfo.InvariantCulture) + (ZoneIsNorth ? "N" : "S"), (32600 + Zone + (ZoneIsNorth ? 0 : 100))+"");
        }

        #endregion

        #region ProjectedCoordinateSystem Members
         

        /// <summary>
        /// Gets or sets the GeographicCoordinateSystem.
        /// </summary>
        public GeographicCs GeographicCs { get; set; } 

        /// <summary>
        /// Gets or sets the <see cref="LinearUnit">LinearUnits</see>. The linear unit must be the same as the <see cref="CoordinateSystem"/> units.
        /// </summary>
        public LinearUnit LinearUnit { get; set; }
        /// <summary>
        /// Gets units for dimension within coordinate system. Each dimension in 
        /// the coordinate system has corresponding units.
        /// </summary>
        /// <param name="dimension">Dimension</param>
        /// <returns>Unit</returns>
        public Unit GetUnits(int dimension)
        {
            return LinearUnit;
        } 

        /// <summary>
        /// Gets or sets the projection
        /// </summary>
        public Projection Projection { get; set; }


        /// <summary>
        /// 是否相等
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>True if equal</returns>
        public override bool Equals(object obj)
        {
            if (!(obj is ProjectedCs))
                return false;
            ProjectedCs pcs = obj as ProjectedCs;
            if (pcs.Dimension != this.Dimension)
                return false;
            for (int i = 0; i < pcs.Dimension; i++)
            {
                if (pcs.Axes[i].Orientation.Direction != this.Axes[i].Orientation.Direction)
                    return false;
                if (!pcs.Axes[i].Unit.Equals(this.Axes[i].Unit))
                    return false;
            }

            return pcs.GeographicCs.Equals(this.GeographicCs) &&
                    pcs.HorizontalDatum.Equals(this.HorizontalDatum) &&
                    pcs.LinearUnit.Equals(this.LinearUnit) &&
                    pcs.Projection.Equals(this.Projection);
        }
        /// <summary>
        /// 哈希数值
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        #endregion
    }


}
