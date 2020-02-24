  
//2017.08.02, czs, create in hongqing, AndroidGnssTimeCaculator

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Geo;
using Geo.Times;
using Gnsser.Times;
using System.IO;


namespace Gnsser
{

    /**
     * 2017/7/27,czs, create in hongqing, new class.
     */

    public class AnGnssUtil
    {



        /**
         *通过系统ID转换成易读的文字。
         * @param constellationId
         * @return
         */
        public static String getConstellationName(int constellationId)
        {
            switch (constellationId)
            {
                case 1:
                    return "GPS";
                case 2:
                    return "SBAS";
                case 3:
                    return "GLONASS";
                case 4:
                    return "QZSS";
                case 5:
                    return "BEIDOU";
                case 6:
                    return "GALILEO";
                default:
                    return "UNKNOWN";
            }
        } 

        public static SatelliteNumber getSatelliteNumbere(int constellationId, int svid)
        {
            var satType = getSatelliteType(constellationId);
            if(satType == SatelliteType.R && svid >= 100){
                svid = svid - 80;
            }
            return new SatelliteNumber(satType, svid);
        }

        public static SatelliteType getSatelliteType(int constellationId)
        {
            switch (constellationId)
            {
                case 1:
                    return SatelliteType.G;//"GPS";
                case 2:
                    return SatelliteType.S;//"SBAS";
                case 3:
                    return SatelliteType.R;//"GLONASS";
                case 4:
                    return SatelliteType.J;// "QZSS";
                case 5:
                    return SatelliteType.C;//"BEIDOU";
                case 6:
                    return SatelliteType.E;//"GALILEO";
                default:
                    return SatelliteType.U;//S;//"UNKNOWN";
            }
        }
    }
}