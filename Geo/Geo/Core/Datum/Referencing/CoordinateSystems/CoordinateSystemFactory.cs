//2014.05.30, czs, added

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Geo.Referencing
{
    /// <summary>
    /// 坐标系统创建工厂。
    /// </summary>
    /// <remarks>
    /// 工厂设计模式。
    /// </remarks>
    public class CoordinateSystemFactory
    { 
        #region ICoordinateSystemFactory Members

        /// <summary>
        /// Creates a spatial reference object given its Well-known text representation.
        /// The output object may be either a <see cref="GeographicCs"/> or
        /// a <see cref="ProjectedCs"/>.
        /// </summary>
        /// <param name="WKT">The Well-known text representation for the spatial reference</param>
        /// <returns>The resulting spatial reference object</returns>
        //public ICoordinateSystem CreateFromWkt(string WKT)
        //{
        //    return CoordinateSystemWktReader.Parse(WKT) as ICoordinateSystem;
        //}

        /// <summary>
        /// Creates an <see cref="Ellipsoid"/> from radius values.
        /// </summary>
        /// <seealso cref="CreateFlattenedSphere"/>
        /// <param name="name">Name of ellipsoid</param>
        /// <param name="semiMajorAxis"></param>
        /// <param name="semiMinorAxis"></param>
        /// <param name="linearUnit"></param>
        /// <returns>Ellipsoid</returns>
        public IEllipsoid CreateEllipsoid(string name, double semiMajorAxis, double semiMinorAxis, LinearUnit linearUnit)
        {
            double ivf = 0;
            if (semiMajorAxis != semiMinorAxis)
                ivf = semiMajorAxis / (semiMajorAxis - semiMinorAxis);
            return new Ellipsoid(semiMajorAxis, ivf, linearUnit)
            {
                Name = name
            };
        }

        /// <summary>
        /// Creates an <see cref="Ellipsoid"/> from an major radius, and inverse flattening.
        /// </summary>
        /// <seealso cref="CreateEllipsoid"/>
        /// <param name="name">Name of ellipsoid</param>
        /// <param name="semiMajorAxis">Semi major-axis</param>
        /// <param name="inverseFlattening">Inverse flattening</param>
        /// <param name="linearUnit">Linear unit</param>
        /// <returns>Ellipsoid</returns>
        public Ellipsoid CreateFlattenedSphere(string name, double semiMajorAxis, double inverseFlattening, LinearUnit linearUnit)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException("Invalid name");

            return new Ellipsoid(semiMajorAxis, inverseFlattening, linearUnit, name);
        }

        /// <summary>
        /// Creates a <see cref="ProjectedCs"/> using a projection object.
        /// </summary>
        /// <param name="name">Name of projected coordinate system</param>
        /// <param name="gcs">Geographic coordinate system</param>
        /// <param name="projection">Projection</param>
        /// <param name="linearUnit">Linear unit</param>
        /// <param name="axis0">Primary axis</param>
        /// <param name="axis1">Secondary axis</param>
        /// <returns>Projected coordinate system</returns>
        public ProjectedCs CreateProjectedCs(string name, GeographicCs gcs, Projection projection, LinearUnit linearUnit, Axis axis0, Axis axis1)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException("Invalid name");
            if (gcs == null)
                throw new ArgumentException("Geographic coordinate system was null");
            if (projection == null)
                throw new ArgumentException("Projection was null");
            if (linearUnit == null)
                throw new ArgumentException("Linear unit was null");
            List<IAxis> info = new List<IAxis>(2);
            info.Add(axis0);
            info.Add(axis1);
            return new ProjectedCs(null, gcs, linearUnit, projection, info, name);
        }

        /// <summary>
        /// Creates a <see cref="Projection"/>.
        /// </summary>
        /// <param name="name">Name of projection</param>
        /// <param name="wktProjectionClass">Projection class</param>
        /// <param name="parameters">Projection parameters</param>
        /// <returns>Projection</returns>
        public Projection CreateProjection(string name, string wktProjectionClass, List<ProjectionParameter> parameters)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException("Invalid name");
            if (parameters == null || parameters.Count == 0)
                throw new ArgumentException("Invalid projection parameters");
            return new Projection(wktProjectionClass, parameters, name);
        }

        /// <summary>
        /// Creates <see cref="HorizontalDatum"/> from ellipsoid and Bursa-World parameters.
        /// </summary>
        /// <remarks>
        /// Since this method contains a set of Bursa-Wolf parameters, the created 
        /// datum will always have a relationship to WGS84. If you wish to create a
        /// horizontal datum that has no relationship with WGS84, then you can 
        /// either specify a <see cref="DatumType">horizontalDatumType</see> of <see cref="DatumType.HD_Other"/>, or create it via WKT.
        /// </remarks>
        /// <param name="name">Name of ellipsoid</param>
        /// <param name="datumType">Type of datum</param>
        /// <param name="ellipsoid">Ellipsoid</param>
        /// <param name="toWgs84">Wgs84 conversion parameters</param>
        /// <returns>Horizontal datum</returns>
        public HorizontalDatum CreateHorizontalDatum(string name, DatumType datumType, Ellipsoid ellipsoid, BursaTransParams toWgs84)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException("Invalid name");
            if (ellipsoid == null)
                throw new ArgumentException("Ellipsoid was null");

            return new HorizontalDatum(ellipsoid, toWgs84, datumType, name);
        }

        /// <summary>
        /// Creates a <see cref="PrimeMeridian"/>, relative to Greenwich.
        /// </summary>
        /// <param name="name">Name of prime meridian</param>
        /// <param name="angularUnit">Angular unit</param>
        /// <param name="longitude">Longitude</param>
        /// <returns>Prime meridian</returns>
        public PrimeMeridian CreatePrimeMeridian(string name, AngularUnit angularUnit, double longitude)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException("Invalid name");
            return new PrimeMeridian(longitude, angularUnit, name);
        }

        /// <summary>
        /// Creates a <see cref="GeographicCs"/>, which could be Lat/Lon or Lon/Lat.
        /// </summary>
        /// <param name="name">Name of geographical coordinate system</param>
        /// <param name="angularUnit">Angular units</param>
        /// <param name="datum">Horizontal datum</param>
        /// <param name="primeMeridian">Prime meridian</param>
        /// <param name="axis0">First axis</param>
        /// <param name="axis1">Second axis</param>
        /// <returns>Geographic coordinate system</returns>
        public GeographicCs CreateGeographicCs(string name, AngularUnit angularUnit, HorizontalDatum datum, PrimeMeridian primeMeridian, Axis axis0, Axis axis1)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException("Invalid name");
            List<IAxis> info = new List<IAxis>(2);
            info.Add(axis0);
            info.Add(axis1);
            return new GeographicCs(angularUnit, datum, primeMeridian, info, name);
        }


        /// <summary>
        /// Creates a <see cref="CreateGeocentricCs"/> from a <see cref="HorizontalDatum">datum</see>, 
        /// <see cref="LinearUnit">linear unit</see> and <see cref="PrimeMeridian"/>.
        /// </summary>
        /// <param name="name">Name of geocentric coordinate system</param>
        /// <param name="datum">Horizontal datum</param>
        /// <param name="linearUnit">Linear unit</param>
        /// <param name="primeMeridian">Prime meridian</param>
        /// <returns>Geocentric Coordinate System</returns>
        public GeocentricCs CreateGeocentricCs(string name, HorizontalDatum datum, LinearUnit linearUnit, PrimeMeridian primeMeridian)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException("Invalid name");
            List<IAxis> info = new List<IAxis>(3);

            info.Add(new Axis("X", Direction.Other));
            info.Add(new Axis("Y", Direction.Other));
            info.Add(new Axis("Z", Direction.Other));
            return new GeocentricCs()
            {
                HorizontalDatum = datum,
                Name = name,
                Axes = info,
                PrimeMeridian = primeMeridian,
                LinearUnit = linearUnit
            };
        }

        #endregion
    }
}
