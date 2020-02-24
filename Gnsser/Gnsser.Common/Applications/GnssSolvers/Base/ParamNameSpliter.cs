//2018.11.08, czs, create in hmx, 参数名称分解器

using System;
using System.Collections.Generic;
using System.Text;
using Gnsser.Domain;
using Gnsser.Data.Sinex;
using Gnsser.Data.Rinex;
using Gnsser.Times;
using Geo.Algorithm;
using Geo.Coordinates;
using  Geo.Algorithm.Adjust;
using Geo;
using Geo.Times;

namespace Gnsser
{
    /// <summary>
    /// 参数类型
    /// </summary>
    public enum ParamType
    {
        Unknow,
        Coord,
        Trop,
        Iono,
        Ambiguity,
        Clock,
    }

    /// <summary>
    /// 参数差分
    /// </summary>
    public enum ParamDifferType
    {
        UnDifference,
        Single,
        Double,
        Triple
    }

    /// <summary>
    /// 参数名称分解器
    /// </summary> 
    public class ParamNameSpliter
    {
        /// <summary>
        /// 参数名称分解器
        /// </summary>
        /// <param name="nameStr"></param>
        public ParamNameSpliter(string nameStr)
        {
            this.ParamType = ParseParamType(nameStr);
            this.ParamDifferType = ParseParamDifferType(nameStr);
            if(ParamDifferType == ParamDifferType.Double)
            {
                var paramGroups = nameStr.Split(new string[] { ParamNames.Divider }, StringSplitOptions.RemoveEmptyEntries);
                //如果是双差，一般有以下几种情况，包含测站名称或不包含
                if(paramGroups.Length == 3)
                {
                    int i = 0;
                    foreach (var item in paramGroups)
                    {
                        var strs = item.Split(new string[] { ParamNames.Pointer }, StringSplitOptions.RemoveEmptyEntries);
                        if(i == 0)
                        {
                            SiteName = strs[0];
                            RefSiteName = strs[1];
                        }
                        if(i == 1 && strs[0].Length == 3)
                        {
                            Prn = SatelliteNumber.Parse(strs[0]);
                            RefPrn = SatelliteNumber.Parse(strs[1]);
                        }

                        i++;
                    }
                }
            }
        }
        /// <summary>
        /// 差分类型
        /// </summary>
        public ParamDifferType ParamDifferType { get; set; }
        /// <summary>
        /// 参数类型
        /// </summary>
        public ParamType ParamType { get; set; }
        /// <summary>
        /// 基准测站名称
        /// </summary>
        public string RefSiteName { get; set; }
        /// <summary>
        /// 流动测站名称
        /// </summary>
        public string SiteName { get; set; }
        /// <summary>
        /// 参考卫星
        /// </summary>
        public SatelliteNumber RefPrn {get;set;}
        /// <summary>
        /// 流动卫星
        /// </summary>
        public SatelliteNumber Prn { get; set; }

        #region  静态方法
        /// <summary>
        /// 参数差分类型解析
        /// </summary>
        /// <param name="nameStr"></param>
        /// <returns></returns>
        public static ParamDifferType ParseParamDifferType(string nameStr)
        {
            if (nameStr.Contains(ParamNames.DoubleDiffer))
            {
                return ParamDifferType.Double;
            }
            else if (nameStr.Contains(ParamNames.SingleDiffer))
            {
                return ParamDifferType.Single;
            }
            else if (nameStr.Contains(ParamNames.TripleDiffer))
            {
                return ParamDifferType.Triple;
            }
            return ParamDifferType.UnDifference;
        }

        /// <summary>
        /// 解析类型，最后一个字符对应
        /// </summary>
        /// <param name="nameStr"></param>
        /// <returns></returns>
        public static ParamType ParseParamType(string nameStr)
        {
            var strs = nameStr.Split(new char[] { '_' }, StringSplitOptions.RemoveEmptyEntries);
            var typeName = strs[strs.Length - 1];
            if (typeName.Contains(ParamNames.Dx)
                || typeName.Contains(ParamNames.Dy)
                || typeName.Contains(ParamNames.Dz)
                )
            {
                return ParamType.Coord;
            }
            if (typeName.Contains(ParamNames.ClkErrDistance)
                )
            {
                return ParamType.Clock;
            }
            if (typeName.Contains(ParamNames.AmbiguityLen)
                )
            {
                return ParamType.Ambiguity;
            }
            if (typeName.Contains(ParamNames.WetTropZpd)
                )
            {
                return ParamType.Trop;
            }
            if (typeName.Contains(ParamNames.Iono)
                )
            {
                return ParamType.Iono;
            }

            return ParamType.Unknow;
        }
        #endregion
    }
}