//2015.01.09, czs, edit in namu, 各种参数名称直接修改为字符串类型
//2015.05.03, lly, find in zz, 发现dz ，dx 反了，但是改正后，精度变差了，待解决??,2015.05.06, 测试，结果好像没有影响。
//2017.05.10, lly, edit in zz, GPS和Galileo,BD之间系统时间偏差的距离偏移量
//2017.08.15, czs, edit in hongqing, 采用枚举表示底层变量名称，避免重复
//2017.09.05, czs, edit, in hongqing, 整理，取消枚举，尽量采用常量命名，简化命名

using System;
using System.Collections.Generic;
using System.Text;
using Geo.Coordinates; 
using Geo.Algorithm; 
using  Geo.Algorithm.Adjust;
using Geo;

namespace Gnsser
{ 
    /// <summary>
    ///  平差计算或数据交换过程中的各个变量的名称，原则：简化，短小，易理解。
    ///  注意：重要变量尽量采用常量命名；变量名应易读，字符串应短小易理解；提供注释；
    ///  不同类型变量采用下划线分隔；避免采用逗号、乘号(*)等符号;
    ///  拼接参数必须在原参数后定义。
    ///  Δ 容易被解析成 位 ，此处暂时保留。
    /// </summary>
    public class ParamNames : GeoParamNames
    {
        /// <summary>
        /// GnssSolverType
        /// </summary>
        public  const  string GnssSolverType = "GnssSolverType";
        #region 原始参数名称

        #region 组合参数名称,调用者必须放在被调用者后面，否则字符串为空
        /// <summary>
        /// GPS和Galileo之间系统时间偏差的距离偏移量
        /// </summary>
        public static string GESystTimeDistance = SysTimeDistDifferOf + "GE";
        /// <summary>
        /// GPS和BD之间系统时间偏差的距离偏移量
        /// </summary>
        public static string GBSystTimeDistance = SysTimeDistDifferOf + "GB";
        /// <summary>
        ///_λ1N 相位1,波长乘以数量
        /// </summary>
        public static string PhaseALengthSuffix = Divider + "Nλ1";

        /// <summary>
        /// _λ2N 相位2，波长乘以数量
        /// </summary>
        public static string PhaseBLengthSuffix = Divider + "Nλ2";
        /// <summary>
        /// _λ2N 相位2，波长乘以数量
        /// </summary>
        public static string PhaseCLengthSuffix = Divider + "Nλ3";

        /// <summary>
        /// _λN 相位
        /// </summary>
        public static string WaveLengthSuffix = Divider +  "Nλ";
        /// <summary>
        /// 后缀分离符号加上双差模糊度后缀，"_▽ΔNλ”。
        /// </summary>
        public static string DoubleDifferAmbiguitySuffix = Divider + DoubleDifferAmbiguity;

        /// <summary>
        /// 后缀分离符号加上双差模糊度后缀，"_▽ΔNλ1”。
        /// </summary>
        public static string DoubleDifferL1AmbiguitySuffix = Divider + DoubleDifferL1Ambiguity;
        /// <summary>
        /// 后缀分离符号加上双差模糊度后缀，"_▽ΔNλ2”。
        /// </summary>
        public static string DoubleDifferL2AmbiguitySuffix = Divider + DoubleDifferL2Ambiguity;

        /// <summary>
        /// 后缀分离符号加上单差模糊度后缀，"_ΔNλ”。
        /// </summary>
        public static string DifferAmbiguitySuffix = Divider + DifferAmbiguity;
        /// <summary>
        /// 后缀分离符号加上模糊度后缀，"_Nλ”
        /// </summary>
        public static string AmbiguitySuffix = Divider + AmbiguityLen;

        /// <summary>
        /// 接收机钟差差分（单次）的距离偏移量，"_▽ΔNλ”。
        /// </summary>
        public static string DifferRcvClkErrDistanceSuffix = Divider + DifferRcvClkErrDistance;

        #endregion
        #endregion

        #region 便捷组合
        /// <summary>
        /// XYZ坐标偏差和接收机钟差偏差，适用于伪距单点定位。
        /// </summary>
        public static List<String> DxyzClk = new List<string> { ParamNames.Dx, ParamNames.Dy, ParamNames.Dz, ParamNames.RcvClkErrDistance };
        /// <summary>
        /// XYZ坐标名称
        /// </summary>
        public static List<String> Dxyz = new List<string> { ParamNames.Dx, ParamNames.Dy, ParamNames.Dz };
        /// <summary>
        /// XYZ获取，具有前后缀
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="postfix"></param>
        /// <param name="spliter"></param>
        /// <returns></returns>
        public static List<string> GetDxyz(string prefix, string postfix = "", string  spliter = "_")
        {
            List<string> result = new List<string>();
            foreach (var item in Dxyz)
            {
                var pre = "";
                if (!String.IsNullOrWhiteSpace(prefix))
                {
                    pre = prefix;
                    if (!prefix.EndsWith(spliter))
                    {
                        pre += spliter;
                    }                    
                }

                var post = "";
                if (!String.IsNullOrWhiteSpace(post))
                {
                    if (postfix.StartsWith(spliter))
                    {
                        post = spliter + postfix;
                    }
                    else
                    {
                        post = spliter + postfix;
                    }
                }

                result.Add(pre + item + post);
            }
            return result;
        }
        /// <summary>
        /// 坐标差时间差和速度差名称
        /// </summary>
        public static List<string> DxyzClkAndV = new List<string> { ParamNames.Dx, ParamNames.Dy, ParamNames.Dz, ParamNames.RcvClkErrDistance, ParamNames.Dvx, ParamNames.Dvy, ParamNames.Dvz, ParamNames.RcvClkDriftDistance };
        /// <summary>
        /// 闭合差名称
        /// </summary>
        public static string ClousureError = "ClousureError";

        /// <summary>
        /// 基线指示字符  "->" 
        /// </summary>
        public const string BaseLinePointer = "-";
        /// <summary>
        /// 单位权方差因子，标准差
        /// </summary>
        public const string StdDev = "StdDev";
        /// <summary>
        /// 窄巷，周
        /// </summary>
        public const string NarrowLaneBsdCycle = "NLBsd";
        /// <summary>
        /// 长度
        /// </summary>
        public const string Length = "Length";
        /// <summary>
        /// Index
        /// </summary>
        public const string Index = "Index";
        /// <summary>
        /// TimePeriod
        /// </summary>
        public const string TimePeriod = "TimePeriod";

        #endregion

        /// <summary>
        /// 是否包含Dx,Dy,Dz
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool IsDxyz(string key)
        {
            return Dxyz.Contains(key);
        }
    }
}