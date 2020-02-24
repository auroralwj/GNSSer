//2014.05.22, Cui Yang, created
//2014.07.22, czs, Refactoring.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Geo.Utils;
using Gnsser.Data.Rinex;

namespace Gnsser
{
    /// <summary>
    /// This is a class to read and parse antenna satData in Antex file format
    /// rcvr_ant.tab: Offical IGS naming conventions for GNSS equipment
    /// antex13.txt: ANTEX format definition
    /// igs05_wwww.atx: Absolute IGS phase center corrections for satellite and receiver antennas. Field 'wwww'represents GPS week of last file change
    /// igs05.atx: Link to latest igs05_wwww.atx file
    /// igs01.atx: Relative IGS phase center corrections for satellite and receiver antennas
    /// </summary>
    public class AntexLabel
    { 
        #region 天线文件标识符字符串常量
        //Antex Formatting Strings
        public const string ANTEX_VERSION_SYST = "ANTEX VERSION / SYST";//"ANTEX VERSION / SYST"
        public const string PCV_TYPE_REFANT = "PCV TYPE / REFANT";//"PCV TYPE / REFANT "
        public const string COMMENT = "COMMENT";//"COMMENT"
        public const string END_OF_HEADER = "END OF HEADER";//"END OF HEADER"

        //                                                            START OF ANTENNA    
        //BLOCK IIA           G01                 G032      1992-079A TYPE / SERIAL NO    
        //                    GFZ/TUM                  0    20-APR-05 METH / BY / # / DATE
        //     0.0                                                    DAZI                
        //     0.0  14.0   1.0                                        ZEN1 / ZEN2 / DZEN  
        //     2                                                      # OF FREQUENCIES    
        //  1992    11    22     0     0    0.0000000                 VALID FROM          
        //  2008    10    16    23    59   59.9999999                 VALID UNTIL         
        //IGS05_1568                                                  SINEX CODE          
        //   G01                                                      START OF FREQUENCY  
        //    279.00      0.00   2201.00                              NORTH / EAST / UP   
        //   NOAZI   -0.80   -0.90   -0.90   -0.80   -0.40    0.20    0.80    1.30    1.40    1.20    0.70    0.00   -0.40   -0.70   -0.90
        //   G01                                                      END OF FREQUENCY    
        //   G02                                                      START OF FREQUENCY  
        //    279.00      0.00   2201.00                              NORTH / EAST / UP   
        //   NOAZI   -0.80   -0.90   -0.90   -0.80   -0.40    0.20    0.80    1.30    1.40    1.20    0.70    0.00   -0.40   -0.70   -0.90
        //   G02                                                      END OF FREQUENCY    
        //                                                            END OF ANTENNA      
        public const string START_OF_ANTENNA = "START OF ANTENNA";//"START OF ANTENNA "
        public const string TYPE_ERIAL_NO = "TYPE / SERIAL NO";//"TYPE / SERIAL NO"
        public const string METH_BY_DATE = "METH / BY / # / DATE";//calibrationMethod
        public const string DAZI = "DAZI";//"incrementAzimuth"
        public const string ZEN1_ZEN2_DZEN = "ZEN1 / ZEN2 / DZEN";//"ZEN1 / ZEN2 / DZEN"
        public const string NUM_OF_FREQUENCIES = "# OF FREQUENCIES";//"# OF FREQUENCIES "
        public const string VALID_FROM = "VALID FROM";//"VALID FROM"
        public const string VALID_UNTIL = "VALID UNTIL";//"VALID UNTIL"
        public const string SINEX_CODE = "SINEX CODE";//"SINEX CODE "
        public const string START_OF_FREQUENCY = "START OF FREQUENCY";//"START OF FREQUENCY"startOfFreq
        public const string NORTH_EAST_UP = "NORTH / EAST / UP";//"NORTH / EAST / UP"antennaEcc
        public const string END_OF_FREQUENCY = "END OF FREQUENCY";//"END OF FREQUENCY"
        public const string START_OF_FREQ_RMS = "START OF FREQ RMS";//"START OF FREQ RMS"startOfFreqRMS
        public const string RMS_NORTH_EAST_UP = "NORTH / EAST / UP";//antennaEccRMS/"NORTH / EAST / UP"
        public const string END_OF_FREQ_RMS = "END OF FREQ RMS";//"END OF FREQ RMS"
        public const string END_OF_ANTENNA = "END OF ANTENNA";//"END OF ANTENNA"
        #endregion       

    }
}
