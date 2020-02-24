//2018.05.25, czs, create in HMX, CODE电离层球谐函数

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Geo;
using Geo.Algorithm;
using Geo.Common;
using Gnsser.Times;
using Geo.Times;
using Gnsser.Data.Rinex;

namespace Gnsser.Data
{
    /// <summary>
    /// 电离层头文件， 字符串
    /// </summary>
    public class IonoHarmonicHeaderLabel
    {
        public const string StartLine = "CODE'S GLOBAL IONOSPHERE MAPS";
        public const string MODEL_NUMBER_STATION_NAME = "MODEL NUMBER / STATION NAME";
        public const string MODEL_TYPE = "MODEL TYPE";
        public const string MAXIMUM_DEGREE_OF_SPHERICAL_HARMONICS = "MAXIMUM DEGREE OF SPHERICAL HARMONICS";
        public const string MAXIMUM_ORDER = "MAXIMUM ORDER";
        public const string EVELOPMENT_WITH_RESPECT_TO = "DEVELOPMENT WITH RESPECT TO";
        public const string MAPPING_FUNCTION = "MAPPING FUNCTION";
        public const string HEIGHT_OF_SINGLE_LAYER_AND_ITS_RMS_ERROR = "HEIGHT OF SINGLE LAYER AND ITS RMS ERROR";
        public const string COORDINATES_OF_EARTH_CENTERED_DIPOLE_AXIS = "COORDINATES OF EARTH-CENTERED DIPOLE AXIS";
        public const string PERIOD_OF_VALIDITY = "PERIOD OF VALIDITY";
        public const string LATITUDE_BAND_COVERED = "LATITUDE BAND COVERED "; 
        public const string ADDITIONAL_INFORMATION = "ADDITIONAL INFORMATION";
        public const string COMMENT_WARNING = "COMMENT / WARNING";
        public const string COEFFICIENTS = "COEFFICIENTS";
        public const string DEGREE = "DEGREE";

        public const string NUMBER_OF_CONTRIBUTING_STATIONS = "  NUMBER OF CONTRIBUTING STATIONS";
        public const string NUMBER_OF_CONTRIBUTING_SATELLITES = "  NUMBER OF CONTRIBUTING SATELLITES";
        public const string ELEVATION_CUT_OFF_ANGLE = "  ELEVATION CUT-OFF ANGLE (DEGREES)"; 
        public const string MAXIMUM_TEC_AND_ITS_RMS = "  MAXIMUM TEC AND ITS RMS ERROR (TECU)";



    } 

    public enum IonoMappingFunction
    {
        NONE = 0,
        COSZ=1,
        MSLM = 2,
        ESM=3
    }


    /// <summary>
    /// 电离层头文件
    /// </summary>
    public class IonoHarmonicHeader
    {
        /// <summary>
        /// 电离层头文件
        /// </summary>
        public IonoHarmonicHeader()
        {
            this.Comments = new List<string>();
        }
        public string ModelNumberStationName { get; set; }
        public string ModelType { get; set; }
        public int MaxDegree { get; set; }
        public int MaxOrder { get; set; }
        public bool IsGeographicalOrGeomeagnetic { get; set; }
        public bool IsMeanOrTruePosOfTheSun{ get; set; }
        public IonoMappingFunction MappingFunction { get; set; }
        public RmsedNumeral LayerHeight { get; set; }
        public NumerialSegment LatSpan { get; set; } 
        public TimePeriod ValidPeroid { get; set; }
        public int NumOfStations { get; set; }
        public int NumOfSatellites { get; set; }
        public double ElevationCutOff { get; set; }
        public RmsedNumeral MaxTec { get; set; } 
        public List<string> Comments { get; set; }
                  

        /// <summary>
        /// 文件名称
        /// </summary>
        public string FileName { get; set; }

    }
   
}
