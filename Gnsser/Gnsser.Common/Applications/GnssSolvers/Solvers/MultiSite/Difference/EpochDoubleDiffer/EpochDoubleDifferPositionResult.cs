//2018.07.26, czs, create in HMX, 简易近距离单历元载波相位双差
//2018.11.28, czs, edit in hmx, 增加 GetTabValues
//2019.01.02, czs, edit in hmx, 去掉简易（Simple）更名为 单历元双差


using System;
using System.Collections.Generic;
using Gnsser.Domain;
using System.Text;
using Geo.Coordinates;
using Gnsser.Data.Rinex;
using Geo.Algorithm;
using Geo.Algorithm.Adjust;
using Gnsser.Service;
using Geo.Utils;
using Gnsser.Filter;
using Gnsser.Checkers;
using Geo.Common; 


namespace Gnsser.Service
{
    /// <summary>
    /// 简易近距离单历元载波相位双差
    /// </summary>
    public class EpochDoubleDifferPositionResult : DualSiteEpochDoubleDifferResult, IFixableParamResult
    {

        /// <summary>
        /// 简易近距离单历元载波相位双差
        /// </summary>
        /// <param name="material"></param>
        /// <param name="Adjustment"></param>
        /// <param name="positioner"></param>
        /// <param name="baseSatPrn"></param>
        public EpochDoubleDifferPositionResult(
            MultiSiteEpochInfo material,
            AdjustResultMatrix Adjustment,
             GnssParamNameBuilder positioner,
            SatelliteNumber baseSatPrn
            )
            : base(material, Adjustment, positioner)
        {

            this.BasePrn = baseSatPrn;
            this.Option = positioner.Option;
        }
        /// <summary>
        /// 定位选项
        /// </summary>
        public GnssProcessOption Option { get; set; }

        /// <summary>
        /// 获取模糊度参数估计结果。单位为周，权逆阵单位依然为米。
        /// </summary>
        /// <returns></returns>
        public WeightedVector GetFixableVectorInUnit()
        {
       
            this.ResultMatrix.Estimated.ParamNames = this.ResultMatrix.ParamNames;
            //Lamdba方法计算模糊度
            var ambiParamNames = new List<string>();
            foreach (var name in ResultMatrix.ParamNames)
            {
                if (HasUnstablePrn(name)) { continue; }

                if (name.Contains(Gnsser.ParamNames.DoubleDifferAmbiguity))
                { ambiParamNames.Add(name); }
            }
            var estVector = this.ResultMatrix.Estimated.GetWeightedVector(ambiParamNames);
            if (Option.IsPhaseInMetterOrCycle)
            {
                //更新模糊度。
                Frequence Frequence = Gnsser.Frequence.GetFrequence(this.BasePrn, Option.ObsDataType, this.Material.ReceiverTime);
                var vectorInCycle = estVector * (1.0 / Frequence.WaveLength);
                return vectorInCycle;
            }
            return estVector;
        }

        /// <summary>
        /// 固定后的模糊度,单位周，值应该为整数。
        /// </summary>
        public WeightedVector FixedParams { get; set; }

    }
}
