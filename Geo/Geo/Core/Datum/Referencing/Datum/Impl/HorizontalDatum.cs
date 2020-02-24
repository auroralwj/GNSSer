//2014.05.24, czs, created 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Geo.Common;

namespace Geo.Referencing
{
    /// <summary>
    /// 水平基准。
    /// </summary>
    public class HorizontalDatum : Datum
    {
        /// <summary>
        /// Initializes a new instance of a horizontal datum
        /// </summary>
        /// <param name="ellipsoid">Ellipsoid</param>
        /// <param name="toWgs84">Parameters for a Bursa Wolf transformation into WGS84</param>
        /// <param name="type">Datum type</param>
        /// <param name="name">Name</param> 
        /// <param name="id">Authority-specific identification code.</param>  
        public HorizontalDatum(
            Ellipsoid ellipsoid, 
            BursaTransParams toWgs84,
            DatumType type,
            string name = null,
            string id = null)
            : base(type, name,id )
        {
            Ellipsoid = ellipsoid;
            Wgs84Parameters = toWgs84;
        }

        #region Predefined datums
        /// <summary>
        /// EPSG's WGS 84 datum has been the then current realisation. No distinction is made between the original WGS 84 
        /// frame, WGS 84 (G730), WGS 84 (G873) and WGS 84 (G1150). Since 1997, WGS 84 has been maintained within 10cm of 
        /// the then current ITRF.
        /// </summary>
        /// <remarks>
        /// <para>Area of use: World</para>
        /// <para>Origin description: Defined through a consistent set of station coordinates. These have changed with time: by 0.7m 
        /// on 29/6/1994 [WGS 84 (G730)], a further 0.2m on 29/1/1997 [WGS 84 (G873)] and a further 0.06m on 
        /// 20/1/2002 [WGS 84 (G1150)].</para>
        /// </remarks>
        public static HorizontalDatum WGS84
        {
            get
            {
                return new HorizontalDatum( 
                    Referencing.Ellipsoid.WGS84,
                    null,
                    DatumType.HD_Geocentric,
                    "WGS84",  
                    "6326");
            }
        }

        /// <summary>
        /// World Geodetic System 1972
        /// </summary>
        /// <remarks>
        /// <para>Used by GPS before 1987. For Transit satellite positioning see also WGS 72BE. Datum code 6323 reserved for southern hemisphere ProjCS's.</para>
        /// <para>Area of use: World</para>
        /// <para>Origin description: Developed from a worldwide distribution of terrestrial and
        /// geodetic satellite observations and defined through a set of station coordinates.</para>
        /// </remarks>
        public static HorizontalDatum WGS72
        {
            get
            {
                HorizontalDatum datum =
                    new HorizontalDatum(
                         Referencing.Ellipsoid.WGS72,
                         null,
                         DatumType.HD_Geocentric,
                        "WGS72",
                        "6322");
                datum.Wgs84Parameters = new BursaTransParams(0, 0, 4.5, 0, 0, 0.554, 0.219);
                return datum;
            }
        }


        /// <summary>
        /// European Terrestrial Reference System 1989
        /// </summary>
        /// <remarks>
        /// <para>Area of use: 
        /// Europe: Albania; Andorra; Austria; Belgium; Bosnia and Herzegovina; Bulgaria; Croatia; 
        /// Cyprus; Czech Republic; Denmark; Estonia; Finland; Faroe Islands; France; Germany; Greece; 
        /// Hungary; Ireland; Italy; Latvia; Liechtenstein; Lithuania; Luxembourg; Malta; Netherlands; 
        /// Norway; Poland; Portugal; Romania; San Marino; Serbia and Montenegro; Slovakia; Slovenia; 
        /// Spain; Svalbard; Sweden; Switzerland; United Kingdom (UK) including Channel Islands and 
        /// Isle of Man; Vatican City State.</para>
        /// <para>Origin description: Fixed to the stable part of the Eurasian continental 
        /// plate and consistent with ITRS at the epoch 1989.0.</para>
        /// </remarks>
        public static HorizontalDatum ETRF89
        {
            get
            {
                HorizontalDatum datum =
                      new HorizontalDatum(
                           Referencing.Ellipsoid.GRS80,
                           null,
                           DatumType.HD_Geocentric,
                           "ETRF89",
                          "6258");

                datum.Wgs84Parameters = new BursaTransParams();
                return datum;
            }
        }

        /// <summary>
        /// European Datum 1950
        /// </summary>
        /// <remarks>
        /// <para>Area of use:
        /// Europe - west - Denmark; Faroe Islands; France offshore; Israel offshore; Italy including San 
        /// Marino and Vatican City State; Ireland offshore; Netherlands offshore; Germany; Greece (offshore);
        /// North Sea; Norway; Spain; Svalbard; Turkey; United Kingdom UKCS offshore. Egypt - Western Desert.
        /// </para>
        /// <para>Origin description: Fundamental point: Potsdam (Helmert Tower). 
        /// Latitude: 52 deg 22 min 51.4456 sec N; Longitude: 13 deg  3 min 58.9283 sec E (of Greenwich).</para>
        /// </remarks>
        public static HorizontalDatum ED50
        {
            get
            {

                HorizontalDatum datum =
                    new HorizontalDatum(
                         Referencing.Ellipsoid.International1924,
                         null,
                         DatumType.HD_Geocentric,
                        "ED50",
                        "6230");
                datum.Wgs84Parameters = new BursaTransParams(-87, -98, -121, 0, 0, 0, 0);
                return datum;
            }
        }
        #endregion

        #region HorizontalDatum Members
         
        /// <summary>
        /// Gets or sets the ellipsoid of the datum
        /// </summary>
        public Ellipsoid Ellipsoid { get; set; }
        /// <summary>
        /// Gets preferred parameters for a Bursa Wolf transformation into WGS84
        /// </summary>
        public BursaTransParams Wgs84Parameters { get; set; }

        #endregion

        /// <summary>
        /// Checks whether the values of this instance is equal to the values of another instance.
        /// Only parameters used for coordinate system are used for comparison.
        /// Name, abbreviation, authority, alias and remarks are ignored in the comparison.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>True if equal</returns>
        public override bool Equals(object obj)
        {
            if (!(obj is HorizontalDatum))
                return false;
            HorizontalDatum datum = obj as HorizontalDatum;
            if (datum.Wgs84Parameters == null && this.Wgs84Parameters != null) return false;
            if (datum.Wgs84Parameters != null && !datum.Wgs84Parameters.Equals(this.Wgs84Parameters))
                return false;
            return (datum != null && this.Ellipsoid != null &&
                datum.Ellipsoid.Equals(this.Ellipsoid) || datum == null && this.Ellipsoid == null) && this.DatumType == datum.DatumType;
        }

        public override int GetHashCode()
        {
            return Wgs84Parameters.GetHashCode();
        }

    }
}
