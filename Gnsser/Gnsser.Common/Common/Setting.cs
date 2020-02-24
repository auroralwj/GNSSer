using System;
using Geo;
using Geo.Times;

namespace Gnsser
{
    /// <summary>
    /// 项目设置。
    /// </summary>
    public class Setting : Geo.Setting
    {
        /// <summary>
        /// RINEX 观测文件过滤
        /// </summary>
        public static string RinexOFileFilter = "RINEX 观测文件(*.??O;*.rnx;*.obs)|*.??O;*.rnx;*.obs|RINEX 压缩文件(*.??O.Z;*.??D.Z;*.crx.gz;*.cnx)|*.??O.Z;*.??D.Z;*.crx.gz;*.cnx|所有文件(*.*)|*.*";
        /// <summary>
        /// RINEX 钟差文件过滤
        /// </summary>
        public static string RinexClkFileFilter = "Clock卫星钟差文件|*.clk;*.clk_30s;*.clk_05s|所有文件|*.*";
        /// <summary>
        /// RINEX 星历文件过滤
        /// </summary>
        public static string RinexEphFileFilter = "星历文件|*.eph;*.sp3;*.??n;*.??p;*.??C|所有文件|*.*";
        /// <summary>
        /// RINEX 星历文件过滤
        /// </summary>
        public static string RinexNFileFilter = "RINEX星历文件|*.??N;*.??P;*.??R;*.??C|所有文件(*.*)|*.*";
        public static string SiteInfoFilter = "测站信息文件|*.stainfo|所有文件(*.*)|*.*";
        public static string SinexFilter = "SINEX文件|*.snx|所有文件(*.*)|*.*";

        public static string SatEleFileFilter = "卫星高度角文件|*SatEle.txt.xls|文本文件|*.txt|所有文件|*.*";
        public static string FcbFileFilter = "FCB 文件|*.fcb.txt.xls|" + TextTableFileFilter;

        public static string FcbExtension = ".fcb.txt.xls";
        /// <summary>
        /// 通用坐标文件
        /// </summary>
        public static string CoordFileFilter = "坐标文件|*.snx;*.txt.xls;*.txt;*.org;*.rep;*.pos|所有文件(*.*)|*.*";
        /// <summary>
        /// 坐标文件和基线文件
        /// </summary>
        public static string CoordAndBaseLineFileFilter = "坐标文件|*.snx;*.txt.xls;*.txt;*.org;*.rep;*.pos|基线文件|*.BaseLine.txt.xls;*.asc;*.txt.xls;|所有文件(*.*)|*.*";
        ///地球同步卫星高度约为 36000 km
        /// <summary>
        /// 卫星到接收机的有效距离范围。1000米到 100,000,000
        /// </summary>
        public static NumerialSegment ValidSatToReceiverDistance = new NumerialSegment(1000, 100000000);
        /// <summary>
        /// 接收机有效高度（距离地心距离）。地表下200,000米到地表上1,000,000米。
        /// </summary>
        public static NumerialSegment ValidReceverGeocentricAltitude = new NumerialSegment(GnssConst.EARTH_RADIUS_APPROX - 200000, GnssConst.EARTH_RADIUS_APPROX + 1000000);
        
        /// <summary>
        /// 是否测试计算信号发射时间。
        /// </summary>
        public static bool IsUpdateEpochInfo = true;
        /// <summary>
        /// 起始时间
        /// </summary>
        public static Time SampleStartTime = new Time(2013, 1, 1);

        /// <summary>
        /// 示例的会话时段
        /// </summary>
        public static TimePeriod SampleSession = new TimePeriod(SampleStartTime, SampleStartTime + TimeSpan.FromDays(1));
        /// <summary>
        /// 配置文件
        /// </summary>
        public static GnsserConfig GnsserConfig { get; set; }

    }

}
