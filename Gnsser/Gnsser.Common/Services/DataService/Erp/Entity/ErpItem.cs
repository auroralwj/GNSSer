//2015.04.12, cy, added
//2018.05.02, czs, edit in hmx, 增加注释，增加zero

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Gnsser.Service;
using Geo.Coordinates;
using Gnsser.Data.Rinex;
using Geo.Algorithm;
using Geo.Utils;
using Gnsser;
using Gnsser.Times;

namespace Gnsser.Data
{
    //http://gnsser.com/Information/ViewDetails/358 , ERP, Earth rotation parameter files

        //      field   contents/HEADER   comment
        //========================================================================

        // 1      MJD               modified Julian secondOfWeek, with 0.01-secondOfWeek precision
        // 2      Xpole             10**-6 arcsec, 0.000001-arcsec precision
        // 3      Ypole             10**-6 arcsec, 0.000001-arcsec precision
        // 4      UT1-UTC, UT1R-UTC
        //        UT1-TAI, UT1R-TAI 10**-7 s, 0.0000001-s precision (.1 us)
        // 5      LOD, LODR         10**-7 s/secondOfWeek  0.0001-ms/secondOfWeek precision (.1 us/secondOfWeek)
        // 6      Xsig              10**-6 arcsec, 0.000001-arcsec precision
        // 7      Ysig              10**-6 arcsec, 0.000001-arcsec precision
        // 8      UTsig             10**-7 s, 0.0000001-sec precision (.1 us)
        // 9      LODsig            10**-7 s/secondOfWeek, 0.0001-ms/secondOfWeek    "     (.1 us/secondOfWeek)
        //10      Nr                number of receivers in the solution (integer)
        //11      Nf                number of receivers with "fixed" coordinates
        //12      Nt                number of satellites (transmitters) in the solution
        //                          (integer)
        //optional (field 11- , only some may be coded, the order is also optional):
        //13      Xrt               10**-6 arcsec/secondOfWeek 0.001-mas/secondOfWeek precision
        //14      Yrt               10**-6 arcsec/secondOfWeek 0.001-mas/secondOfWeek precision
        //15      Xrtsig            10**-6 arcsec/secondOfWeek 0.001-mas/secondOfWeek    "
        //16      Yrtsig            10**-6 arcsec/secondOfWeek 0.001-mas/secondOfWeek    "
        //17      XYCorr            X-Y   Correlation 0.001 precision
        //18      XUTCor            X-UT1 Correlation 0.01    "
        //19      YUTCor            Y-UT1 Correlation 0.01    "
    /// <summary>
    /// ERP类，记录单天的ERP参数信息。
    ///单位在读取时已经被转换为正常的数值。
    /// </summary>
    public class ErpItem
    {
        /// <summary>
        /// 默认构造函数
        /// </summary>
         public ErpItem() { }
        /// <summary>
        /// 日期 modified Julian secondOfWeek, with 0.01-secondOfWeek precision
        /// </summary>
        public double Mjd { get; set; }


        /// <summary>
        ///  Xpole             10**-6 arcsec, 0.000001-arcsec precision
        /// </summary>
        public double Xpole { get; set; }
        /// <summary>
        ///  Ypole             10**-6 arcsec, 0.000001-arcsec precision
        /// </summary>
        public double Ypole { get; set; }
        /// <summary>
        ///  10**-7 s, 0.0000001-s precision (.1 us)
        /// </summary>
        public double Ut12Utc { get; set; }
        /// <summary>
        /// LOD, LODR         10**-7 s/secondOfWeek  0.0001-ms/secondOfWeek precision (.1 us/secondOfWeek)
        /// </summary>
        public double Lod { get; set; }
        /// <summary>
        ///  Xsig              10**-6 arcsec, 0.000001-arcsec precision
        /// </summary>
        public double Xsig { get; set; }
        /// <summary>
        ///   Ysig              10**-6 arcsec, 0.000001-arcsec precision
        /// </summary>
        public double Ysig { get; set; }
        /// <summary>
        ///  UTsig             10**-7 s, 0.0000001-sec precision (.1 us)
        /// </summary>
        public double UTsig { get; set; }
        /// <summary>
        ///  LODsig            10**-7 s/day, 0.0001-ms/day    "     (.1 us/day)
        /// </summary>
        public double LODsig { get; set; }
        /// <summary>
        ///  number of receivers in the solution (integer)
        /// </summary>
        public double Nr { get; set; }
        /// <summary>
        ///  Nf                number of receivers with "fixed" coordinates
        /// </summary>
        public double Nf { get; set; }
        /// <summary>
        ///  Nt                number of satellites (transmitters) in the solution     (integer)
        /// </summary>
        public double Nt { get; set; }

        /// <summary>
        ///  10**-6 arcsec/secondOfWeek 0.001-mas/secondOfWeek precision
        /// </summary>
        public double Xrt { get; set; } // 
        /// <summary>
        /// 10**-6 arcsec/secondOfWeek 0.001-mas/secondOfWeek precision
        /// </summary>
        public double Yrt { get; set; }//  
        /// <summary>
        /// 10**-6 arcsec/secondOfWeek 0.001-mas/secondOfWeek    "
        /// </summary>
        public double Xrtsig { get; set; }// 
        /// <summary>
        ///   10**-6 arcsec/secondOfWeek 0.001-mas/secondOfWeek    "
        /// </summary>
        public double Yrtsig { get; set; }//
        /// <summary>
        ///  X-Y   Correlation 0.001 precision
        /// </summary>
        public double XYCorr { get; set; }// 
        /// <summary>
        ///   X-UT1 Correlation 0.01    "
        /// </summary>
        public double XUTCor { get; set; }//  
        /// <summary>
        /// Y-UT1 Correlation 0.01    "
        /// </summary>
        public double YUTCor { get; set; }//  
        /// <summary>
        /// 字符串显示
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Mjd + ", Xpole:" + Xpole + ", Ypole:" + Ypole;
        }

        static ErpItem zero = new ErpItem();
        /// <summary>
        /// 0
        /// </summary>
        public static ErpItem Zero { get => zero; }
        /// <summary>
        /// 是否是0
        /// </summary>
        public bool IsZero
        {
            get => this.Xpole == 0 && Ypole == 0;
        }
        /// <summary>
        /// 克隆
        /// </summary>
        /// <returns></returns>
        public ErpItem Clone()
        {
            return new ErpItem()
            {
                Lod = this.Lod,
                LODsig = this.LODsig,
                Mjd = Mjd,
                Nf = Nf,
                Nr = Nr,
                Nt = Nt,
                UTsig = UTsig,
                Ut12Utc = Ut12Utc,
                Xpole = Xpole,
                Xrt = Xrt,
                Xrtsig = Xrtsig,
                Xsig = Xsig,
                XUTCor = XUTCor,
                XYCorr = XYCorr,
                Ypole = Ypole,
                Yrt = Yrt,
                Yrtsig = Yrtsig,
                Ysig = Ysig,
                YUTCor = YUTCor
            };
        }
    }
}