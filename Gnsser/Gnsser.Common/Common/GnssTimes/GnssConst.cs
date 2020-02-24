//新的常量
//2017.05.10, lly, add in zz, 非差非组合的电离层系数。

using System;
using System.Collections.Generic;
using System.Text;
using Geo.Coordinates;
using Geo.Times;
using Gnsser.Data;


namespace Gnsser
{
    /// <summary>
    /// GNSS 常用常量。
    /// </summary>
    public class GnssConst
    {
        #region 系统最值的初始
        /// <summary>
        /// max satellite PRN number of GPS
        /// </summary>
        public const int MaxPrnGPS = 32;

        /// <summary>
        /// min satellite PRN number of GPS
        /// </summary>
        public const int MinPrnGPS = 1;

        /// <summary>
        /// number of GPS satellite
        /// </summary>
        public const int NumSatGPS = MaxPrnGPS - MinPrnGPS + 1;



        /// <summary>
        /// max satellite PRN number of GLONASS
        /// </summary>
        public const int MaxPrnGLONASS = 24;

        /// <summary>
        /// min satellite PRN number of GLONASS
        /// </summary>
        public const int MinPrnGLONASS = 1;

        /// <summary>
        /// number of GLONASS satellite
        /// </summary>
        public const int NumSatGLONASS = MaxPrnGLONASS - MinPrnGLONASS + 1; 

        /// <summary>
        /// max satellite PRN number of Galileo
        /// </summary>
        public const int MaxPrnGalileo = 24;

        /// <summary>
        /// min satellite PRN number of Galileo
        /// </summary>
        public const int MinPrnGalileo = 1;

        /// <summary>
        /// number of Galileo satellite
        /// </summary>
        public const int NumSatGalileo = MaxPrnGalileo - MinPrnGalileo + 1;


        /// <summary>
        /// max satellite PRN number of Beidou
        /// </summary>
        public const int MaxPrnCompass = 24;

        /// <summary>
        /// min satellite PRN number of Beidou
        /// </summary>
        public const int MinPrnCompass = 1;

        /// <summary>
        /// number of Beidou satellite
        /// </summary>
        public const int NumSatCompass = MaxPrnCompass - MinPrnCompass + 1;


        /// <summary>
        /// max satellite number(目前仅考虑四大系统，其实还有更多，如QZSS SBAS等增强系统
        /// </summary>
        public const int MaxSat = NumSatGPS + NumSatGLONASS + NumSatGalileo + NumSatCompass;


        #endregion

        /// <summary>
        /// 地球自转常数 (rad/s)
        /// </summary>
        public const double RotateVelocityOfEarth = 7.2921151467e-5;    /* earth angular velocity (IS-GPS) (rad/s) */

        /// <summary>
        /// PI*2
        /// </summary>
        public const double TWO_PI = 6.2831853071796;
        //--------GPS 常量---------
        /// <summary>
        /// L1 carrier frequency in Hz.
        /// </summary>
        public const double GPS_L1_FREQ = 1575.42e6;
        /// <summary>
        ///  L1 carrier wavelength in meters.
        /// </summary>
        public const double GPS_L1_WAVELENGTH = 0.190293672798;
        /// <summary>
        /// L2 carrier frequency in Hz.
        /// </summary>
        public const double GPS_L2_FREQ = 1227.60e6;
        /// <summary>
        ///  L2 carrier wavelength in meters.
        /// </summary>
        public const double GPS_L2_WAVELENGTH = 0.244210213425;
        /// <summary>
        /// GPS基础频率
        /// </summary>
        public const double GPS_BASE_FREQUENCY = 10.23e+6;
        /// <summary>
        /// GPS L1的频率 154倍基础频率 = 1.57542e9;
        /// </summary>
        public const double GPS_FREQUENCY_L1 = 154 * GPS_BASE_FREQUENCY;
        /// <summary>
        /// GPS L2的频率 120倍基础频率 = 1.2276e9;
        /// </summary>
        public const double GPS_FREQUENCY_L2 = 120 * GPS_BASE_FREQUENCY;//


        /// <summary>
        /// GPS L5的频率 
        /// </summary>
        public const double GPS_FREQUENCY_L5 = 1.17645E9;//


        /// <summary>
        /// GPS L1 波长 0.19029367279836488047631742646405
        /// </summary>
        public const double GPS_WAVE_LENGTH_L1 = LIGHT_SPEED / GPS_FREQUENCY_L1;
        /// <summary>
        /// GPS L2 波长 0.24421021342456826327794069729554
        /// </summary>
        public const double GPS_WAVE_LENGTH_L2 = LIGHT_SPEED / GPS_FREQUENCY_L2;
        /// <summary>
        /// GPS L1 波长的倒数
        /// </summary>
        public const double GPS_ANT_WAVE_LENGTH_L1 = GPS_FREQUENCY_L1 / LIGHT_SPEED;
        /// <summary>
        /// 地球自转速度（Rad/s）
        /// </summary>
        public const double EARTH_ROTATE_SPEED = 7.2921151467e-5;    //*** rad/sec
        /// <summary>
        /// 地球半径估值（米）6371,000
        /// </summary>
        public const double EARTH_RADIUS_APPROX = 6371000;
        /// <summary>
        /// 光速
        /// </summary>
        public const double LIGHT_SPEED = 299792458;
        /// <summary>
        /// 1米等于多少纳米
        /// </summary>
        public const int NanoPerUnit = 1000000000;

        /// <summary>
        /// 真空中光在一个纳秒所经过的距离。
        /// </summary>
        public const double MeterPerNano =  (1e-9) * LIGHT_SPEED;
        /// <summary>
        /// GPS L1频率的平方
        /// </summary>
        public const double SquaredL1FreqOfGPS = GnssConst.GPS_FREQUENCY_L1 * GnssConst.GPS_FREQUENCY_L1;
        /// <summary>
        /// GPS L2频率的平方
        /// </summary>
        public const double SquaredL2FreqOfGPS = GnssConst.GPS_FREQUENCY_L2 * GnssConst.GPS_FREQUENCY_L2;
        /// <summary>
        /// 2.55 无电离层组合下，单频DCB改正，直接与DCB（m）值相乘即可
        /// </summary>
        public const double DcbMultiplierOfGPSL1 = SquaredL2FreqOfGPS / (SquaredL1FreqOfGPS - SquaredL2FreqOfGPS);
        /// <summary>
        ///  1.55 无电离层组合下，单频DCB改正，直接与DCB（m）值相乘即可
        /// </summary>
        public const double DcbMultiplierOfGPSL2 =  SquaredL1FreqOfGPS / (SquaredL1FreqOfGPS - SquaredL2FreqOfGPS);
        

        /// <summary>
        /// GPS 双频 L1载波的C1电离层组合乘法因子
        /// 2.545727780163160154572778016316;
        /// </summary>
        public const double GPS_L1C1_COM_FACTOR = 1.646944444 / 0.646944444;// 2.545727780163160154572778016316;
        /// <summary>
        /// GPS 双频 L1载波的C1电离层组合乘法因子
        /// 2.545727780163160154572778016316;
        /// </summary>
        public const double GPS_L1P1_FACTOR = GPS_L1C1_COM_FACTOR;//2.545727780163160154572778016316;
        /// <summary>
        /// GPS 双频 L1载波的C1电离层组合乘法因子
        /// -1.545727780163160154572778016316
        /// </summary>
        public const double GPS_L2P2_COM_FACTOR = -1.0 / 0.646944444;//- 1.545727780163160154572778016316;
        /// <summary>
        /// 北斗 双频 L1载波的C1电离层组合乘法因子
        /// -2.4871683136169249238472416253327
        /// </summary>
        public const double BD_L1C1_FACTOR = 2.4871683136169249238472416253327;

        /// <summary>
        /// 北斗 双频 L2载波的P2电离层组合乘法因子
        /// -1.4871683136169249238472416253327
        /// </summary>
        public const double BD_L2P2_FACTOR = -1.4871683136169249238472416253327;


        #region 一些常数

        //参考某 matlab PPP
        //Kalman initialization constants
        /// <summary>
        /// (m) used in Q(dynamic model)
        /// </summary>
        public const double StdOfCoord = 0.0003;
        /// <summary>
        /// (m) used in Q(dynamic model) 30
        /// </summary>
        public const double StdOfClockError = 30;
        /// <summary>
        /// (m) STD of carrier phase(R) 0.02
        /// </summary>
        public const double StdOfCarrierPhase = 0.02;
        /// <summary>
        ///  (m) STD of pseudorange(R) 0,2
        /// </summary>
        public const double StdOfPsedoRange = 0.2;
        /// <summary>
        /// (m) STD of initial parameters(P) 1.0
        /// </summary>
        public const double StdOfInitialCoord = 1;
        /// <summary>
        /// (m) STD of initial ambiguities(P) 1E10
        /// </summary>
        public const double StdOfInitialAmbiguity = 1E10;// Math.Pow(10, 10);
        /// <summary>
        /// (m) STD of initial clock(P) 500
        /// </summary>
        public const double StdOfInitialClockError = 500;
        /// <summary>
        ///  (m) STD of initial tropo(P) 1
        /// </summary>
        public const double StdOfInitialTropo = 1;//0.1;
        #endregion
        /// <summary>
        /// 钟差最大值
        /// </summary>
        public static double MaxClockError = 99999999.9;

        #region RTCM 需要的一些常数
        //参考rtklib
        /// <summary>
        /// OneLightmillisecond
        /// </summary>
        public const double LightSpeedPerMillisecond = 299792.458;
        /// <summary>
        /// semi-circle to radian (IS-GPS)
        /// </summary>
        public const double SC2RAD = 3.1415926535898;
        /// <summary>
        /// 2^-5
        /// </summary>
        public const double P2_5 = 0.03125; 
        /// <summary>
        ///  2^-6
        /// </summary>  
        public const double P2_6 = 0.015625;
        /// <summary>
        ///  2^-11
        /// </summary>
        public const double P2_11 = 4.882812500000000E-04;
        /// <summary>
        ///  2^-15
        /// </summary>
        public const double P2_15 = 3.051757812500000E-05; 
        /// <summary>
        /// 2^-17 
        /// </summary>
        public const double P2_17 = 7.629394531250000E-06;
        /// <summary>
        /// 2^-19
        /// </summary>
        public const double P2_19 = 1.907348632812500E-06; 
        /// <summary>
        ///  2^-20
        /// </summary>
        public const double P2_20 = 9.536743164062500E-07;
        /// <summary>
        ///  2^-21 
        /// </summary>
        public const double P2_21 = 4.768371582031250E-07; 
        /// <summary>
        ///  2^-23
        /// </summary>
        public const double P2_23 = 1.192092895507810E-07;
        /// <summary>
        /// 2^-24
        /// </summary>
        public const double P2_24 = 5.960464477539063E-08; 
        /// <summary>
        /// 2^-27
        /// </summary>
        public const double P2_27 = 7.450580596923828E-09; 
        /// <summary>
        /// 2^-29
        /// </summary>
        public const double P2_29 = 1.862645149230957E-09; 
        /// <summary>
        /// 2^-30
        /// </summary>
        public const double P2_30 = 9.313225746154785E-10; 
        /// <summary>
        /// 2^-31
        /// </summary>
        public const double P2_31 = 4.656612873077393E-10; 
        /// <summary>
        /// 2^-32
        /// </summary>
        public const double P2_32 = 2.328306436538696E-10;
        /// <summary>
        /// 2^-33
        /// </summary>
        public const double P2_33 = 1.164153218269348E-10; 
        /// <summary>
        /// 2^-35
        /// </summary>
        public const double P2_35 = 2.910383045673370E-11; 
        /// <summary>
        ///  2^-38
        /// </summary>
        public const double P2_38 = 3.637978807091710E-12; 
        /// <summary>
        /// 2^-39
        /// </summary>
        public const double P2_39 = 1.818989403545856E-12;
        /// <summary>
        /// 2^-40
        /// </summary>
        public const double P2_40 = 9.094947017729280E-13; 
        /// <summary>
        /// 2^-43
        /// </summary>
        public const double P2_43 = 1.136868377216160E-13; 
        /// <summary>
        /// 2^-48
        /// </summary>
        public const double P2_48 = 3.552713678800501E-15; 
        /// <summary>
        /// 2^-50
        /// </summary>
        public const double P2_50 = 8.881784197001252E-16; 
        /// <summary>
        /// 2^-55
        /// </summary>
        public const double P2_55 = 2.775557561562891E-17;
        #endregion

        #region GEOID 参数 暂时放在这里
        public const int GEOID_EMBEDDED = 0; //geoid model: embedded geoid
        public const int GEOID_EGM96_M150 = 1; //geoid model: EGM96 15 * 15"
        public const int GEOID_EGM2008_M25 = 2;//geoid model: EGM2008 2.5 * 2.5"
        public const int GEOID_EGM2008_M10 = 3;//geoid model: EGM2008 1.0 * 1.0"
        public const int GEOID_GSI2000_M15 = 4;//geoid model: GSI geoid 2000 1.0 * 1.5"
        #endregion

        #region 非差非组合的电离层系数
        /// <summary>
        ///GPS电离层系数 f1^2 / f2^2
        /// </summary>
        public const double CoeOfGPSIono = SquaredL1FreqOfGPS / SquaredL2FreqOfGPS;// 1.64694444445658;//

        /// <summary>
        ///Galileo电离层系数  f1^2 / f2^2
        /// </summary>
        public const double CoeOfGalileoIono = 1.793270321361059;//

        /// <summary>
        /// BD电离层系数 f1^2 / f2^2
        /// </summary>
        public const double CoeOfBDIono = 1.672418845159437;//
        #endregion
    }

}