
using System;
using System.Text;
using Geo.Coordinates;
using Gnsser.Data.Rinex;
using Geo.Algorithm;
using Geo.Algorithm.Adjust;
using Gnsser.Service;
using Gnsser.Times;
using Geo.Utils;
using Gnsser.Domain;
using Geo.Times; 

namespace Gnsser.Correction
{
    /// <summary>
    /// 对流程改正， 伪距改正数
    /// </summary>
    public class RangeTropoCorrector : AbstractRangeCorrector
    { 
        /// <summary>
        /// 伪距改正。对流程。
        /// </summary>
        public RangeTropoCorrector()
        {
            this.Name = "对流层距离改正";
            this.CorrectionType = CorrectionType.RangeTropo;
        }
         

        public  override void Correct(EpochSatellite epochSatellite) { 
            double correction = - GetTropoCorrectValue(
                epochSatellite.RecevingTime,
                epochSatellite.Ephemeris.XYZ,
                epochSatellite.SiteInfo.EstimatedXyz);

            this.Correction = -1.0 * (correction);
        } 


        /// <summary>
        ///  对流程改正。 对流程的改正符号为负数。
        /// </summary>
        /// <param name="gpsTime"></param>
        /// <param name="satPos"></param>
        /// <param name="receiverXyz"></param>
        /// <returns></returns>
        public static double GetTropoCorrectValue(Time gpsTime, XYZ satPos, XYZ receiverXyz)
        {
            Polar p = CoordTransformer.XyzToGeoPolar(satPos, receiverXyz, Geo.Coordinates.AngleUnit.Radian);
            Geo.Coordinates.GeoCoord geoCoord = Geo.Coordinates.CoordTransformer.XyzToGeoCoord(receiverXyz, Geo.Coordinates.AngleUnit.Radian);
            double troCorect = MeteorologyInfluence.TroposphereDelay(geoCoord.Lat, geoCoord.Height, p.Elevation, gpsTime);
            return troCorect;
        }
    }
}
