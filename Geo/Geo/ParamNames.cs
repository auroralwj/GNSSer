//2019.01.16, czs, create in hmx, 从GNSSer提取

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geo
{
    /// <summary>
    /// 名称
    /// </summary>
    public class GeoParamNames
    {

        #region 常量
        /// <summary>
        /// Name
        /// </summary>
        public const string Name = "Name";
        /// <summary>
        /// Epoch
        /// </summary>
        public const string Epoch = "Epoch";
        /// <summary>
        /// RmsX
        /// </summary>
        public const string RmsX = "RmsX";
        /// <summary>
        /// RmsY
        /// </summary>
        public const string RmsY = "RmsY";
        /// <summary>
        /// RmsZ
        /// </summary>
        public const string RmsZ = "RmsZ";
        /// <summary>
        /// RefX
        /// </summary>
        public const string RefX = "RefX";
        /// <summary>
        /// RefY
        /// </summary>
        public const string RefY = "RefY";
        /// <summary>
        /// RefZ
        /// </summary>
        public const string RefZ = "RefZ";
        /// <summary>
        /// RovX
        /// </summary>
        public const string RovX = "RovX";
        /// <summary>
        /// RovY
        /// </summary>
        public const string RovY = "RovY";
        /// <summary>
        /// RovZ
        /// </summary>
        public const string RovZ = "RovZ";

        /// <summary>
        /// X坐标的方差或协方差。
        /// </summary>
        public const string Qx = "Qx";
        /// <summary>
        ///y方差或协方差。
        /// </summary>
        public const string Qy = "Qy";
        /// <summary>
        /// z方差或协方差
        /// </summary>
        public const string Qz = "Qz";
        /// <summary>
        /// xy协方差
        /// </summary>
        public const string Qxy = "Qxy";
        /// <summary>
        /// xz协方差
        /// </summary>
        public const string Qxz = "Qxz";
        /// <summary>
        /// yz协方差
        /// </summary>
        public const string Qyz = "Qyz";
        /// <summary>
        /// Distance
        /// </summary>
        public const string Distance = "Distance";
        /// <summary>
        /// GeodeticAzimuth
        /// </summary>
        public const string GeodeticAzimuth = "GeodeticAzimuth";
        /// <summary>
        /// GeodeticLength
        /// </summary>
        public const string GeodeticLength = "GeodeticLength";
        /// <summary>
        /// ResultType
        /// </summary>
        public const string ResultType = "ResultType";
        /// <summary>
        /// L
        /// </summary>
        public const string PhaseL = "L";
        /// <summary>
        /// L1
        /// </summary>
        public const string L1 = "L1";
        /// <summary>
        /// L2
        /// </summary>
        public const string L2 = "L2";
        /// <summary>
        /// P
        /// </summary>
        public const string RangeP = "P";

        /// <summary>
        /// Δ单差符号
        /// </summary>
        public const string SingleDiffer = "Δ";
        /// <summary>
        /// ▽Δ 双差符号
        /// </summary>
        public const string DoubleDiffer = "▽Δ";
        /// <summary>
        /// Δ▽Δ 三差符号
        /// </summary>
        public const string TripleDiffer = "Δ▽Δ";
        /// <summary>
        /// MW周
        /// </summary>
        public const string MwCycle = "MwCycle";
        /// <summary>
        /// 波长符号，拉门达 λ lambda
        /// </summary>
        public const string Lambda = "λ";
        /// <summary>
        /// 指向符号，减号，指针，"-”。常用于差分。
        /// </summary>
        public const string Pointer = "-";
        /// <summary>
        /// 名称分离符号，"_”。
        /// </summary>
        public const string Divider = "_";
        /// <summary>
        /// 模糊度
        /// </summary>
        public const string Ambiguity = "N";
        /// <summary>
        /// 模糊度长度
        /// </summary>
        public const string AmbiguityLen = "Nλ";
        /// <summary>
        /// 参考站标识
        /// </summary>
        public const string Ref = "Ref";
        /// <summary>
        /// 流动站标识
        /// </summary>
        public const string Rov = "Rov";
        /// <summary>
        /// X坐标的量
        /// </summary>
        public const string X = "X";
        /// <summary>
        /// Y坐标的量
        /// </summary>
        public const string Y = "Y";
        /// <summary>
        /// Z坐标的量
        /// </summary>
        public const string Z = "Z";
        /// <summary>
        /// Lon坐标的量
        /// </summary>
        public const string Lon = "Lon";
        /// <summary>
        /// Lat坐标的量
        /// </summary>
        public const string Lat = "Lat";
        /// <summary>
        /// Height坐标的量
        /// </summary>
        public const string Height = "Height";
        /// <summary>
        /// X坐标的偏移量 dX DX
        /// </summary>
        public const string Dx = "Dx";
        /// <summary>
        /// Y坐标的偏移量 dy
        /// </summary>
        public const string Dy = "Dy";
        /// <summary>
        /// Z坐标的偏移量
        /// </summary>
        public const string Dz = "Dz";
        /// <summary>
        /// Ds
        /// </summary>
        public const string Ds = "Ds";
        /// <summary>
        /// X坐标的偏移量 dX DX
        /// </summary>
        public const string Dvx = "Dvx";
        /// <summary>
        /// Y坐标的偏移量
        /// </summary>
        public const string Dvy = "Dvy";
        /// <summary>
        /// Z坐标的偏移量
        /// </summary>
        public const string Dvz = "Dvz";
        /// <summary>
        /// N方向的偏移量
        /// </summary>
        public const string Dn = "Dn";
        /// <summary>
        /// E方向的偏移量
        /// </summary>
        public const string De = "De";
        /// <summary>
        /// U方向的偏移量
        /// </summary>
        public const string Du = "Du";
        /// <summary>
        /// cdt 通用时钟等价的距离偏移量
        /// </summary>
        public const string cDt = "cDt";
        /// <summary>
        /// 钟漂等效距离 cDvt
        /// </summary>
        public const string cDvt = "cDvt";
        /// <summary>
        /// P1 码
        /// </summary>
        public const string P1Code = "P1";
        /// <summary>
        /// L1 码
        /// </summary>
        public const string L1Code = "L1";
        /// <summary>
        /// P2 码
        /// </summary>
        public const string P2Code = "P2";
        /// <summary>
        /// L2 码
        /// </summary>
        public const string L2Code = "L2";
        /// <summary>
        /// P 码
        /// </summary>
        public const string PCode = "P";
        /// <summary>
        /// L 码
        /// </summary>
        public const string LCode = "L";
        /// <summary>
        /// 钟漂等效距离 cDvt
        /// </summary>
        public const string ClkDriftDistance = cDvt;
        /// <summary>
        /// cDt_r 接收机钟差等价的距离偏移量
        /// </summary>
        public const string RcvClkErrDistance = cDt + "_r";
        /// <summary>
        /// cDt 机钟差等价的距离偏移量
        /// </summary>
        public const string ClkErrDistance = cDt;
        /// <summary>
        /// cDvt_r 接收机钟漂等效距离
        /// </summary>
        public const string RcvClkDriftDistance = cDvt + "_r";
        /// <summary>
        /// cDt_s 卫星钟差等价的距离偏移量
        /// </summary>
        public const string SatClkErrDistance = cDt + "_s";
        /// <summary>
        /// cDt_ 不同系统时间偏差的距离偏移量
        /// </summary>
        public const string SysTimeDistDifferOf = "cDt_";
        /// <summary>
        /// 接收机钟差差分（单次）的距离偏移量
        /// </summary>
        public const string DifferRcvClkErrDistance = ("ΔcDt_r");

        /// <summary>
        /// 单差模糊度后缀，"ΔNλ”。
        /// </summary>
        public const string DifferAmbiguity = "ΔNλ";
        /// <summary>
        /// 双差模糊度后缀，"▽ΔNλ”。
        /// </summary>
        public const string DoubleDifferAmbiguity = "▽ΔNλ";

        /// <summary>
        /// 双差模糊度后缀，"▽ΔNλ2”。
        /// </summary>
        public const string DoubleDifferL2Ambiguity = "▽ΔNλ2";

        /// <summary>
        /// 双差模糊度后缀，"▽ΔNλ1”。
        /// </summary>
        public const string DoubleDifferL1Ambiguity = "▽ΔNλ1";
        /// <summary>
        /// 对流层
        /// </summary>
        public const string Trop = "Trop";

        /// <summary>
        /// 对流层湿延迟天顶距
        /// </summary>
        public const string WetTropZpd = "WetTropZpd";
        /// <summary>
        /// DCB
        /// </summary>
        public const string Dcb = "Dcb";

        /// <summary>
        /// 基准站对流层
        /// </summary>
        public const string RefWetTrop = ("RefWetTrop");

        /// <summary>
        /// 电离层距离改正
        /// </summary>
        public const string Iono = "Iono";
        /// <summary>
        /// 电离层距离变化
        /// </summary>
        public const string DifferIono = "ΔIono";

        /// <summary>
        /// _λN 载波相位长度符号，波长乘以数量后缀
        /// </summary>
        public const string PhaseLengthSuffix = "_" + AmbiguityLen;

        #endregion


    }
}
