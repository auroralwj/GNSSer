using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Collections.Generic;

namespace Geo.Referencing
{
    /// <summary>
    /// Well Known Text (WKT) 坐标系统读取器。
    /// </summary>
    public class CoordinateSystemWktReader
    {
        /// <summary>
        /// 读取并解析 WKT格式的空间参考系统字符串。
        /// 
        /// Reads and parses a WKT-formatted projection string.
        /// </summary>
        /// <param name="wkt">String containing WKT.</param>
        /// <returns>Object representation of the WKT.</returns>
        /// <exception cref="System.ArgumentException">If a token is not recognised.</exception>
        public static IdentifiedObject Parse(string wkt)
        {
            IdentifiedObject returnObject = null;
            StringReader reader = new StringReader(wkt);
            WktStreamTokenizer tokenizer = new WktStreamTokenizer(reader);
            tokenizer.NextToken();
            string objectName = tokenizer.GetStringValue();
            switch (objectName)
            {
                case "UNIT":
                    returnObject = ReadUnit(tokenizer);
                    break;
                //case "VERT_DATUM":
                //    VerticalDatum verticalDatum = ReadVerticalDatum(tokenizer);
                //    returnObject = verticalDatum;
                //    break;
                case "SPHEROID":
                    returnObject = ReadEllipsoid(tokenizer);
                    break;
                case "DATUM":
                    returnObject = ReadHorizontalDatum(tokenizer); ;
                    break;
                case "PRIMEM":
                    returnObject = ReadPrimeMeridian(tokenizer);
                    break;
                case "VERT_CS":
                case "GEOGCS":
                case "PROJCS":
                case "COMPD_CS":
                case "GEOCCS":
                case "FITTED_CS":
                case "LOCAL_CS":
                    returnObject = ReadCoordinateSystem(wkt, tokenizer);
                    break;
                default:
                    throw new ArgumentException(String.Format("'{0}' is not recognized.", objectName));

            }
            reader.Close();
            return returnObject;
        }

        #region 以下是读取细节
        /// <summary>
        /// Returns a IUnit given a piece of WKT.
        /// </summary>
        /// <param name="tokenizer">WktStreamTokenizer that has the WKT.</param>
        /// <returns>An object that implements the IUnit interface.</returns>
        private static Unit ReadUnit(WktStreamTokenizer tokenizer)
        {
            tokenizer.ReadToken("[");
            string unitName = tokenizer.ReadDoubleQuotedWord();
            tokenizer.ReadToken(",");
            tokenizer.NextToken();
            double unitsPerUnit = tokenizer.GetNumericValue();
            string authority = String.Empty;
            long authorityCode = -1;
            tokenizer.NextToken();
            if (tokenizer.GetStringValue() == ",")
            {
                tokenizer.ReadAuthority(ref authority, ref authorityCode);
                tokenizer.ReadToken("]");
            }
            return new Unit(){ Id =authorityCode+"", Name = unitName, Abbreviation = unitName, ConversionFactor = unitsPerUnit};
        }
        /// <summary>
        /// Returns a <see cref="LinearUnit"/> given a piece of WKT.
        /// </summary>
        /// <param name="tokenizer">WktStreamTokenizer that has the WKT.</param>
        /// <returns>An object that implements the IUnit interface.</returns>
        private static LinearUnit ReadLinearUnit(WktStreamTokenizer tokenizer)
        {
            tokenizer.ReadToken("[");
            string unitName = tokenizer.ReadDoubleQuotedWord();
            tokenizer.ReadToken(",");
            tokenizer.NextToken();
            double unitsPerUnit = tokenizer.GetNumericValue();
            string authority = String.Empty;
            long authorityCode = -1;
            tokenizer.NextToken();
            if (tokenizer.GetStringValue() == ",")
            {
                tokenizer.ReadAuthority(ref authority, ref authorityCode);
                tokenizer.ReadToken("]");
            }
            return new LinearUnit() { Id = authorityCode + "", Name = unitName, Abbreviation = unitName, 
                 MetersPerUnit = unitsPerUnit};
        }
        /// <summary>
        /// Returns a <see cref="AngularUnit"/> given a piece of WKT.
        /// </summary>
        /// <param name="tokenizer">WktStreamTokenizer that has the WKT.</param>
        /// <returns>An object that implements the IUnit interface.</returns>
        private static AngularUnit ReadAngularUnit(WktStreamTokenizer tokenizer)
        {
            tokenizer.ReadToken("[");
            string unitName = tokenizer.ReadDoubleQuotedWord();
            tokenizer.ReadToken(",");
            tokenizer.NextToken();
            double unitsPerUnit = tokenizer.GetNumericValue();
            string authority = String.Empty;
            long authorityCode = -1;
            tokenizer.NextToken();
            if (tokenizer.GetStringValue() == ",")
            {
                tokenizer.ReadAuthority(ref authority, ref authorityCode);
                tokenizer.ReadToken("]");
            }

            return new AngularUnit()
            {
                Id = authorityCode + "",
                Name = unitName,
                Abbreviation = unitName,
                 RadiansPerUnit = unitsPerUnit
            };
          }

        /// <summary>
        /// Returns a <see cref="Axis"/> given a piece of WKT.
        /// </summary>
        /// <param name="tokenizer">WktStreamTokenizer that has the WKT.</param>
        /// <returns>An Axis object.</returns>
        private static Axis ReadAxis(WktStreamTokenizer tokenizer)
        {
            if (tokenizer.GetStringValue() != "AXIS")
                tokenizer.ReadToken("AXIS");
            tokenizer.ReadToken("[");
            string axisName = tokenizer.ReadDoubleQuotedWord();
            tokenizer.ReadToken(",");
            tokenizer.NextToken();
            string unitname = tokenizer.GetStringValue();
            tokenizer.ReadToken("]");
            switch (unitname.ToUpper(CultureInfo.InvariantCulture))
            {
                case "DOWN": return new Axis(axisName, Direction.Down);
                case "EAST": return new Axis(axisName, Direction.East);
                case "NORTH": return new Axis(axisName, Direction.North);
                case "OTHER": return new Axis(axisName, Direction.Other);
                case "SOUTH": return new Axis(axisName, Direction.South);
                case "UP": return new Axis(axisName, Direction.Up);
                case "WEST": return new Axis(axisName, Direction.West);
                default:
                    throw new ArgumentException("Invalid axis name '" + unitname + "' in WKT");
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="coordinateSystem"></param>
        /// <param name="tokenizer"></param>
        /// <returns></returns>
        private static CoordinateSystem ReadCoordinateSystem(string coordinateSystem, WktStreamTokenizer tokenizer)
        {
            switch (tokenizer.GetStringValue())
            {
                case "GEOGCS":
                    return ReadGeographicCoordinateSystem(tokenizer);
                case "PROJCS":
                    return ReadProjectedCoordinateSystem(tokenizer);
                case "COMPD_CS":
                /*	ICompoundCoordinateSystem compoundCS = ReadCompoundCoordinateSystem(tokenizer);
                    returnCS = compoundCS;
                    break;*/
                case "VERT_CS":
                /*	VerticalCoordinateSystem verticalCS = ReadVerticalCoordinateSystem(tokenizer);
                    returnCS = verticalCS;
                    break;*/
                case "GEOCCS":
                case "FITTED_CS":
                case "LOCAL_CS":
                    throw new NotSupportedException(String.Format("{0} coordinate system is not supported.", coordinateSystem));
                default:
                    throw new InvalidOperationException(String.Format("{0} coordinate system is not recognized.", coordinateSystem));
            }
        }

        /// <summary>
        /// Reads either 3, 6 or 7 parameter Bursa-Wolf values from TOWGS84 token
        /// </summary>
        /// <param name="tokenizer"></param>
        /// <returns></returns>
        private static BursaTransParams ReadWGS84ConversionInfo(WktStreamTokenizer tokenizer)
        {
            //TOWGS84[0,0,0,0,0,0,0]
            tokenizer.ReadToken("[");
            BursaTransParams info = new BursaTransParams();
            tokenizer.NextToken();
            info.Dx = tokenizer.GetNumericValue();
            tokenizer.ReadToken(",");

            tokenizer.NextToken();
            info.Dy = tokenizer.GetNumericValue();
            tokenizer.ReadToken(",");

            tokenizer.NextToken();
            info.Dz = tokenizer.GetNumericValue();
            tokenizer.NextToken();
            if (tokenizer.GetStringValue() == ",")
            {
                tokenizer.NextToken();
                info.Ex = tokenizer.GetNumericValue();

                tokenizer.ReadToken(",");
                tokenizer.NextToken();
                info.Ey = tokenizer.GetNumericValue();

                tokenizer.ReadToken(",");
                tokenizer.NextToken();
                info.Ez = tokenizer.GetNumericValue();

                tokenizer.NextToken();
                if (tokenizer.GetStringValue() == ",")
                {
                    tokenizer.NextToken();
                    info.Scale_ppm = tokenizer.GetNumericValue();
                }
            }
            if (tokenizer.GetStringValue() != "]")
                tokenizer.ReadToken("]");
            return info;
        }
         

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tokenizer"></param>
        /// <returns></returns>
        private static Ellipsoid ReadEllipsoid(WktStreamTokenizer tokenizer)
        {
            //SPHEROID["Airy 1830",6377563.396,299.3249646,AUTHORITY["EPSG","7001"]]
            tokenizer.ReadToken("[");
            string name = tokenizer.ReadDoubleQuotedWord();
            tokenizer.ReadToken(",");
            tokenizer.NextToken();
            double majorAxis = tokenizer.GetNumericValue();
            tokenizer.ReadToken(",");
            tokenizer.NextToken();
            double e = tokenizer.GetNumericValue();
            //

            //tokenizer.ReadToken(",");
            tokenizer.NextToken();
            string authority = String.Empty;
            long authorityCode = -1;
            if (tokenizer.GetStringValue() == ",") //Read authority
            {
                tokenizer.ReadAuthority(ref authority, ref authorityCode);
                tokenizer.ReadToken("]");
            }
            Ellipsoid ellipsoid = new Ellipsoid(majorAxis, e, LinearUnit.Metre)
            {
                Name = name,
                Id = authorityCode +""
            };
            return ellipsoid;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tokenizer"></param>
        /// <returns></returns>
        private static Projection ReadProjection(WktStreamTokenizer tokenizer)
        {
            //tokenizer.NextToken();// PROJECTION
            tokenizer.ReadToken("PROJECTION");
            tokenizer.ReadToken("[");//[
            string projectionName = tokenizer.ReadDoubleQuotedWord();
            tokenizer.ReadToken("]");//]
            tokenizer.ReadToken(",");//,
            tokenizer.ReadToken("PARAMETER");
            List<ProjectionParameter> paramList = new List<ProjectionParameter>();
            while (tokenizer.GetStringValue() == "PARAMETER")
            {
                tokenizer.ReadToken("[");
                string paramName = tokenizer.ReadDoubleQuotedWord();
                tokenizer.ReadToken(",");
                tokenizer.NextToken();
                double paramValue = tokenizer.GetNumericValue();
                tokenizer.ReadToken("]");
                tokenizer.ReadToken(",");
                paramList.Add(new ProjectionParameter(paramName, paramValue));
                tokenizer.NextToken();
            }
            string authority = String.Empty;
            long authorityCode = -1;
            Projection projection = new Projection(                 
                projectionName, paramList,authorityCode+"", projectionName);
            return projection;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tokenizer"></param>
        /// <returns></returns>
        private static ProjectedCs ReadProjectedCoordinateSystem(WktStreamTokenizer tokenizer)
        {
            /*PROJCS[
                "OSGB 1936 / British National Grid",
                GEOGCS[
                    "OSGB 1936",
                    DATUM[...]
                    PRIMEM[...]
                    AXIS["Geodetic latitude","NORTH"]
                    AXIS["Geodetic longitude","EAST"]
                    AUTHORITY["EPSG","4277"]
                ],
                PROJECTION["Transverse Mercator"],
                PARAMETER["latitude_of_natural_origin",49],
                PARAMETER["longitude_of_natural_origin",-2],
                PARAMETER["scale_factor_at_natural_origin",0.999601272],
                PARAMETER["false_easting",400000],
                PARAMETER["false_northing",-100000],
                AXIS["Easting","EAST"],
                AXIS["Northing","NORTH"],
                AUTHORITY["EPSG","27700"]
            ]
            */
            tokenizer.ReadToken("[");
            string name = tokenizer.ReadDoubleQuotedWord();
            tokenizer.ReadToken(",");
            tokenizer.ReadToken("GEOGCS");
            GeographicCs geographicCS = ReadGeographicCoordinateSystem(tokenizer);
            tokenizer.ReadToken(",");
            Projection projection = ReadProjection(tokenizer);
            Unit unit = ReadLinearUnit(tokenizer);

            string authority = String.Empty;
            long authorityCode = -1;
            tokenizer.NextToken();
            List<IAxis> axes = new List<IAxis>(2);
            if (tokenizer.GetStringValue() == ",")
            {
                tokenizer.NextToken();
                while (tokenizer.GetStringValue() == "AXIS")
                {
                    axes.Add(ReadAxis(tokenizer));
                    tokenizer.NextToken();
                    if (tokenizer.GetStringValue() == ",") tokenizer.NextToken();
                }
                if (tokenizer.GetStringValue() == ",") tokenizer.NextToken();
                if (tokenizer.GetStringValue() == "AUTHORITY")
                {
                    tokenizer.ReadAuthority(ref authority, ref authorityCode);
                    tokenizer.ReadToken("]");
                }
            }
            //This is default axis values if not specified.
            if (axes.Count == 0)
            {
                axes.Add(new Axis("X", Direction.East));
                axes.Add(new Axis("Y", Direction.North));
            }
            ProjectedCs projectedCS = new ProjectedCs(geographicCS.HorizontalDatum, geographicCS, unit as LinearUnit, projection, axes, name,  authorityCode+"");
            return projectedCS;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tokenizer"></param>
        /// <returns></returns>
        private static GeographicCs ReadGeographicCoordinateSystem(WktStreamTokenizer tokenizer)
        {
            /*
            GEOGCS["OSGB 1936",
            DATUM["OSGB 1936",SPHEROID["Airy 1830",6377563.396,299.3249646,AUTHORITY["EPSG","7001"]],TOWGS84[0,0,0,0,0,0,0],AUTHORITY["EPSG","6277"]]
            PRIMEM["Greenwich",0,AUTHORITY["EPSG","8901"]]
            AXIS["Geodetic latitude","NORTH"]
            AXIS["Geodetic longitude","EAST"]
            AUTHORITY["EPSG","4277"]
            ]
            */
            tokenizer.ReadToken("[");
            string name = tokenizer.ReadDoubleQuotedWord();
            tokenizer.ReadToken(",");
            tokenizer.ReadToken("DATUM");
            HorizontalDatum horizontalDatum = ReadHorizontalDatum(tokenizer);
            tokenizer.ReadToken(",");
            tokenizer.ReadToken("PRIMEM");
            PrimeMeridian primeMeridian = ReadPrimeMeridian(tokenizer);
            tokenizer.ReadToken(",");
            tokenizer.ReadToken("UNIT");
            AngularUnit angularUnit = ReadAngularUnit(tokenizer);

            string authority = String.Empty;
            long authorityCode = -1;
            tokenizer.NextToken();
            List<IAxis> info = new List<IAxis>(2);
            if (tokenizer.GetStringValue() == ",")
            {
                tokenizer.NextToken();
                while (tokenizer.GetStringValue() == "AXIS")
                {
                    info.Add(ReadAxis(tokenizer));
                    tokenizer.NextToken();
                    if (tokenizer.GetStringValue() == ",") tokenizer.NextToken();
                }
                if (tokenizer.GetStringValue() == ",") tokenizer.NextToken();
                if (tokenizer.GetStringValue() == "AUTHORITY")
                {
                    tokenizer.ReadAuthority(ref authority, ref authorityCode);
                    tokenizer.ReadToken("]");
                }
            }
            //This is default axis values if not specified.
            if (info.Count == 0)
            {
                info.Add(new Axis("Lon", Direction.East));
                info.Add(new Axis("Lat", Direction.North));
            }
            GeographicCs geographicCS = new GeographicCs(angularUnit, horizontalDatum,
                    primeMeridian, info, name, authorityCode+"");
            return geographicCS;
        }

        /// <summary>
        /// 读取 HorizontalDatum
        /// </summary>
        /// <param name="tokenizer"></param>
        /// <returns></returns>
        private static HorizontalDatum ReadHorizontalDatum(WktStreamTokenizer tokenizer)
        {
            //DATUM["OSGB 1936",SPHEROID["Airy 1830",6377563.396,299.3249646,AUTHORITY["EPSG","7001"]],TOWGS84[0,0,0,0,0,0,0],AUTHORITY["EPSG","6277"]]
            BursaTransParams wgsInfo = null;
            string authority = String.Empty;
            long authorityCode = -1;

            tokenizer.ReadToken("[");
            string name = tokenizer.ReadDoubleQuotedWord();
            tokenizer.ReadToken(",");
            tokenizer.ReadToken("SPHEROID");
            Ellipsoid ellipsoid = ReadEllipsoid(tokenizer);
            tokenizer.NextToken();
            while (tokenizer.GetStringValue() == ",")
            {
                tokenizer.NextToken();
                if (tokenizer.GetStringValue() == "TOWGS84")
                {
                    wgsInfo = ReadWGS84ConversionInfo(tokenizer);
                    tokenizer.NextToken();
                }
                else if (tokenizer.GetStringValue() == "AUTHORITY")
                {
                    tokenizer.ReadAuthority(ref authority, ref authorityCode);
                    tokenizer.ReadToken("]");
                }
            }
            // make an assumption about the datum type.
            HorizontalDatum horizontalDatum = new HorizontalDatum(
                ellipsoid, 
                wgsInfo, 
                DatumType.HD_Geocentric,
                name,
                authorityCode+"");

            return horizontalDatum;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tokenizer"></param>
        /// <returns></returns>
        private static PrimeMeridian ReadPrimeMeridian(WktStreamTokenizer tokenizer)
        {
            //PRIMEM["Greenwich",0,AUTHORITY["EPSG","8901"]]
            tokenizer.ReadToken("[");
            string name = tokenizer.ReadDoubleQuotedWord();
            tokenizer.ReadToken(",");
            tokenizer.NextToken();
            double longitude = tokenizer.GetNumericValue();

            tokenizer.NextToken();
            string authority = String.Empty;
            long authorityCode = -1;
            if (tokenizer.GetStringValue() == ",")
            {
                tokenizer.ReadAuthority(ref authority, ref authorityCode);
                tokenizer.ReadToken("]");
            }
            // make an assumption about the Angular units - degrees.
            PrimeMeridian primeMeridian = new PrimeMeridian(longitude, AngularUnit.Degree, name, authorityCode+"");

            return primeMeridian;
        }
        #endregion

    }
}
