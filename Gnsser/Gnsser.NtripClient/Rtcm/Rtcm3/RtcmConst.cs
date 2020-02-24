//2015.11.04, czs & double, create in erpao 招待所,  RTCM 常用常量

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gnsser.Domain;
using Gnsser.Data.Rinex;

namespace Gnsser.Ntrip.Rtcm
{ 
    /// <summary>
    /// RTCM 常用常量
    /// </summary>
    public class RtcmConst
    {
        /// <summary>
        /// 载波对应伪距的分辨率
        /// </summary>
        public static double PhaseRangeResolution = 0.0005;
        /// <summary>
        /// 伪距的分辨率
        /// </summary>
        public static double PseoudoRangeResolution = 0.02;

        public static uint MaxL2MinusL1PseudorangeDifference = 0xFFFFE000;

        public static uint MaxGpsL1PhaseRangeMinusPseudorange = 0xFFF80000;
        public static double DeltaClockC0solution = 0.1 / (1000);//0.1mm

        public static double DeltaClockC1solution = 0.001 / (1000);//0.001mm/s

        public static double DeltaClockC2solution = 0.00002 / (1000);//0.00002mm/(s^2)
        public static double DeltaRadial = 0.1 / (1000);//0.1mm

        public static double DeltaAlongTrack = 0.4 / (1000);//0.4mm

        public static double DeltaCrossTrack = 0.4 / (1000);//0.4mm
        public static double DotDeltaRadial = 0.001 / (1000);//0.001mm/s

        public static double DotDeltaAlongTrack = 0.004 / (1000);//0.004mm/s

        public static double DotDeltaCrossTrack = 0.004 / (1000);//0.004mm/s
        /// <summary>
        /// OneLightmillisecond
        /// </summary>
        public static double LightSpeedPerMillisecond = 299792.458;
        /// <summary>
        /// semi-circle to radian (IS-GPS)
        /// </summary>
        public static double SemiCircleToRad = Math.PI;// //3.1415926535898;
        /// <summary>
        /// 2^-5
        /// </summary>
        public static double P2_5 = Math.Pow(2, -5);//0.03125;
        /// <summary>
        ///  2^-6
        /// </summary>  
        public static double P2_6 = Math.Pow(2, -6);//0.015625;
        /// <summary>
        ///  2^-11
        /// </summary>
        public static double P2_11 = Math.Pow(2, -11);// 4.882812500000000E-04;
        /// <summary>
        ///  2^-15
        /// </summary>
        public static double P2_15 = Math.Pow(2, -15);//3.051757812500000E-05;
        /// <summary>
        /// 2^-17 
        /// </summary>
        public static double P2_17 = Math.Pow(2, -17);//7.629394531250000E-06;
        /// <summary>
        /// 2^-19
        /// </summary>
        public static double P2_19 = Math.Pow(2, -19);//1.907348632812500E-06;
        /// <summary>
        ///  2^-20
        /// </summary>
        public static double P2_20 = Math.Pow(2, -20);// 9.536743164062500E-07;
        /// <summary>
        ///  2^-21 
        /// </summary>
        public static double P2_21 = Math.Pow(2, -21);// 4.768371582031250E-07;
        /// <summary>
        ///  2^-23
        /// </summary>
        public static double P2_23 = Math.Pow(2, -23);//1.192092895507810E-07;
        /// <summary>
        /// 2^-24
        /// </summary>
        public static double P2_24 = Math.Pow(2, -24);//5.960464477539063E-08;
        /// <summary>
        /// 2^-27
        /// </summary>
        public static double P2_27 = Math.Pow(2, -27);//7.450580596923828E-09;
        /// <summary>
        /// 2^-29
        /// </summary>
        public static double P2_29 = Math.Pow(2, -29);//1.862645149230957E-09;
        /// <summary>
        /// 2^-30
        /// </summary>
        public static double P2_30 = Math.Pow(2, -30);//9.313225746154785E-10;
        /// <summary>
        /// 2^-31
        /// </summary>
        public static double P2_31 = Math.Pow(2, -31);// 4.656612873077393E-10;
        /// <summary>
        /// 2^-32
        /// </summary>
        public static double P2_32 = Math.Pow(2, -32);//2.328306436538696E-10;
        /// <summary>
        /// 2^-33
        /// </summary>
        public static double P2_33 = Math.Pow(2, -33);//1.164153218269348E-10;
        /// <summary>
        /// 2^-34
        /// </summary>
        public static double P2_34 = Math.Pow(2, -34);
        /// <summary>
        /// 2^-35
        /// </summary>
        public static double P2_35 = Math.Pow(2, -35);//2.910383045673370E-11;
        /// <summary>
        ///  2^-38
        /// </summary>
        public static double P2_38 = Math.Pow(2, -38);//3.637978807091710E-12;
        /// <summary>
        /// 2^-39
        /// </summary>
        public static double P2_39 = Math.Pow(2, -39);//1.818989403545856E-12;
        /// <summary>
        /// 2^-40
        /// </summary>
        public static double P2_40 = Math.Pow(2, -40);// 9.094947017729280E-13;
        /// <summary>
        /// 2^-43
        /// </summary>
        public static double P2_43 = Math.Pow(2, -43);//1.136868377216160E-13;
        /// <summary>
        /// 2^-46
        /// </summary>
        public static double P2_46 = Math.Pow(2, -46);
        /// <summary>
        /// 2^-48
        /// </summary>
        public static double P2_48 = Math.Pow(2, -48);// 3.552713678800501E-15;
        /// <summary>
        /// 2^-50
        /// </summary>
        public static double P2_50 = Math.Pow(2, -50);// 8.881784197001252E-16;
        /// <summary>
        /// 2^-55
        /// </summary>
        public static double P2_55 = Math.Pow(2, -55);//2.775557561562891E-17;
        /// <summary>
        /// 2^-59
        /// </summary>
        public static double P2_59 = Math.Pow(2, -59);
        /// <summary>
        /// 2^-66
        /// </summary>
        public static double P2_66 = Math.Pow(2, -66);
    }
}