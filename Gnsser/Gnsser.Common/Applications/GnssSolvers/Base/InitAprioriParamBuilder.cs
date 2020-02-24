//2017.09.10, czs, create in hongqing, 基于参数名称的初始先验信息构造器

using System;
using Gnsser.Domain;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Geo.Coordinates;
using Gnsser.Data.Rinex;
using Geo.Algorithm;
using Geo.Algorithm.Adjust;
using Gnsser.Service;
using Geo.Utils; 
using Geo;
using Gnsser;

namespace Gnsser.Service
{



    /// <summary>
    /// 基于参数名称的初始先验信息构造器
    /// </summary>
    public class InitAprioriParamBuilder : AbstractBuilder<WeightedVector>
    {
        /// <summary>
        /// 基于参数名称的初始先验信息构造器
        /// </summary>
        /// <param name="ParamNames"></param>
        /// <param name="option"></param>
        public InitAprioriParamBuilder(List<string> ParamNames, GnssProcessOption option)
        {
            this.paramNames = ParamNames;
            this.Option = option;
        }

        /// <summary>
        /// 参数名称
        /// </summary>
        public List<string> paramNames { get; set; }
        /// <summary>
        /// 选项
        /// </summary>
        public GnssProcessOption Option { get; set; }
        /// <summary>
        /// 初始值
        /// </summary>
        public Dictionary<String, RmsedNumeral> InitData { get; set; }

        /// <summary>
        /// 构造
        /// </summary>
        /// <returns></returns>
        public override WeightedVector Build()
        {
            Vector vector = new Vector(this.paramNames.Count) { ParamNames = this.paramNames };
            Matrix cova = new Matrix(vector.Count) { ColNames = this.paramNames, RowNames = this.paramNames };
            int i = 0;
            foreach (var item in this.paramNames)
            {
                var rmsedVal = Create(item);
                vector[i] = rmsedVal.Value;
                cova[i, i] = rmsedVal.Variance;

                i++;
            }

            WeightedVector wv = new WeightedVector(vector, cova) { ParamNames = paramNames };
            return wv;
        }


        /// <summary>
        /// 默认的状态转移模型生成
        /// </summary>
        /// <param name="key">参数名称</param>
        /// <returns></returns>
        public  RmsedNumeral Create(string key)
        {
            if (InitData != null && InitData.ContainsKey(key))
            {
                return InitData[key];
            }

            if (key.Contains(ParamNames.Dx)
                || key.Contains(ParamNames.Dy)
                || key.Contains(ParamNames.Dz)
                )
            {
                if (Option.PositionType == PositionType.静态定位)
                {
                    return new RmsedNumeral(0, 100);//10 m
                }
                else
                {
                    return new RmsedNumeral(1, 10000);
                }
            }
            else if (key.Contains(ParamNames.Dvx)
               || key.Contains(ParamNames.Dvy)
               || key.Contains(ParamNames.Dvz)
               )
            {
                if (Option.PositionType == PositionType.静态定位)
                {
                    return new RmsedNumeral(0, 100);
                }
                else
                {
                    return new RmsedNumeral(1, 10000);
                }
            }
            else if (key.Contains(ParamNames.RcvClkDriftDistance)) { return new RmsedNumeral(0, 1e5); }
            else if (key.Contains(ParamNames.RcvClkErrDistance)) { return new RmsedNumeral(0, 1e5); }
            else if (key.Contains(ParamNames.WetTropZpd) || key.Contains(ParamNames.Trop)) { return new RmsedNumeral(0, 0.5); }
            else if (key.Contains(ParamNames.Iono)) { return new RmsedNumeral(0, 1e2); }
            else if (key.Contains(ParamNames.Dcb)) { return new RmsedNumeral(0, 1e4); }
            else if (key.Contains(ParamNames.SysTimeDistDifferOf)) { return new RmsedNumeral(0, 1e5); }
            else if (key.Contains(ParamNames.PhaseLengthSuffix)
                || key.Contains(ParamNames.PhaseALengthSuffix)
                || key.Contains(ParamNames.PhaseBLengthSuffix)
                || key.Contains(ParamNames.PhaseBLengthSuffix)
                || key.Contains(ParamNames.DifferAmbiguity)
                || key.Contains(ParamNames.DoubleDifferAmbiguity)
                )
            { return new RmsedNumeral(0, 1e5); }

            return new RmsedNumeral(0, 1e5);
        }
    }


}