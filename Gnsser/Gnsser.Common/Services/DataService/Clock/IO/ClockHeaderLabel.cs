//2015.05.10, czs, create in namu, 单独为钟差文件而设计

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Geo.Coordinates;

namespace Gnsser.Data.Rinex
{
    /// <summary>
    /// Rinex 头文件标签。
    /// </summary>
    public class ClockHeaderLabel
    {
        // GNSS OBSERVATION DATA FILE - HEADER SECTION DESCRIPTION  
        /// <summary>
        /// The "RINEX VERSION / TYPE" clk must be the prevObj clk in coefficient fileB
        /// </summary>
        public const string RINEX_VERSION_TYPE = "RINEX VERSION / TYPE";
        public const string PGM_RUN_BY_DATE = "PGM / RUN BY / DATE";
        public const string COMMENT = "COMMENT"; 
        public const string END_OF_HEADER = "END OF HEADER"; 
        public const string TIME_SYSTEM_ID = "TIME SYSTEM ID";
        public const string SYS_PCVS_APPLIED = "SYS / PCVS APPLIED";
        public const string SYS_DCBS_APPLIED = "SYS / DCBS APPLIED";
        public const string LEAP_SECONDS = "LEAP SECONDS";//2016.11.06,double add
        public const string TYPES_OF_DATA = "# / TYPES OF DATA";
        public const string ANALYSIS_CENTER = "ANALYSIS CENTER";
        public const string OF_CLK_REF = "# OF CLK REF";
        public const string ANALYSIS_CLK_REF = "ANALYSIS CLK REF";
        public const string OF_SOLN_STA_TRF = "# OF SOLN STA / TRF";
        public const string SOLN_STA_NAME_NUM = "SOLN STA NAME / NUM";
        public const string OF_SOLN_SATS = "# OF SOLN SATS";
        public const string PRN_LIST = "PRN LIST"; 

        //----------------rinex 3.0 nav file
        /// <summary>
        /// Ionospheric correction parameters
        /// - Correction type
        /// GAL = Galileo ai0 – ai2
        /// GPSA = GPS alpha0 - alpha3
        /// GPSB = GPS beta0 - beta3
        /// - Parameters
        /// GPS: alpha0-alpha3 or beta0-beta3 |
        /// GAL: ai0, ai1, ai2, zero
        /// </summary>
        public const string IONOSPHERIC_CORR = "IONOSPHERIC CORR";
        /// <summary>
        /// Corrections to transform the system time | |*
        ///to UTC or other time systems | |
        ///- Correction type | A4,1X, |
        ///GAUT = GAL to UTC a0, a1 | |
        ///GPUT = GPS to UTC a0, a1 | |
        ///SBUT = SBAS to UTC a0, a1 | |
        ///GLUT = GLO to UTC a0=TauC, a1=zero | |
        ///GPGA = GPS to GAL a0=A0G, a1=A1G | |
        ///GLGP = GLO to GPS a0=TauGPS, a1=zero | |
        ///- a0,a1 Coefficients of 1-deg polynomial | D17.10, |
        ///(a0 sec, a1 sec/sec) | D16.9, |
        ///CORR(s) = a0 + a1*DELTAT | |
        ///- TProduct Reference time for polynomial | I7, |
        ///(Fraction into GPS/GAL week) | |
        ///- W Reference week number | I5, |
        ///(GPS/GAL week, continuous number) | |
        ///TProduct and W zero for GLONASS. | |
        ///- S EGNOS, WAAS, or MSAS ... | 1X,A5,1X |
        ///(left-justified) | |
        ///Derived from MT17 service provider. | |
        ///If not known: Use Snn with | |
        ///nn = PRN-100 of satellite | |
        ///broadcasting the MT12 | |
        ///- U UTC Identifier (0 if unknown) | I2,1X |
        ///1=UTC(NIST), 2=UTC(USNO), 3=UTC(SU), | |
        ///4=UTC(BIPM), 5=UTC(Europe Lab), | |
        ///6=UTC(CRL), >6 = not assigned yet | |
        ///S and U for SBAS only. |
        /// </summary>
        public const string TIME_SYSTEM_CORR = "TIME SYSTEM CORR";

    }


}
