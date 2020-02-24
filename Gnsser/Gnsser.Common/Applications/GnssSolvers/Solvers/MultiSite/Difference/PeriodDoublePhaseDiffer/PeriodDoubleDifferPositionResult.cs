//2015.01.06, czs,  create in namu, 双差差分定位结果。具有模糊度。
//2018.07.31, czs, edit in hmx, 名称前冠名Period

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
using Geo.Algorithm;

namespace Gnsser.Service
{
    /// <summary>
    /// 双差差分定位结果。具有模糊度。。
    /// </summary>
    public class PeriodDoubleDifferPositionResult : TwoSitePeriodDifferPositionResult, IFixableParamResult
    {
        /// <summary>
        /// 双差差分定位结果。具有模糊度。
        /// </summary>
        /// <param name="Adjustment"></param>
        /// <param name="BasePrn"></param>
        /// <param name="DifferPositionOption"></param>
        /// <param name="PeriodDifferInfo"></param>
        /// <param name="nameBuilder"></param>
        public PeriodDoubleDifferPositionResult(
            MultiSitePeriodInfo PeriodDifferInfo,
            AdjustResultMatrix Adjustment,
            SatelliteNumber BasePrn,
            GnssProcessOption DifferPositionOption, GnssParamNameBuilder nameBuilder)
            : base(PeriodDifferInfo, Adjustment, BasePrn, nameBuilder)
        {
            this.Option = DifferPositionOption;
        }
        GnssProcessOption Option { get; set; }

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
        /// 获取模糊度参数估计结果。单位为周，权逆阵单位依然为米。
        /// </summary>
        /// <returns></returns>
        public WeightedVector GetFixableVectorInUnit00()
        {
            this.ResultMatrix.Estimated.ParamNames = this.ResultMatrix.ParamNames;
            //更新模糊度。
            Frequence Frequence = Frequence.GetFrequence(this.BasePrn, Option.ObsDataType,this.Material.ReceiverTime);
            //Lamdba方法计算模糊度
            var ambiParamNames = new List<string>();
            foreach (var name in ResultMatrix.ParamNames)
            {
                if (HasUnstablePrn(name)) { continue; }

                if (name.Contains(Gnsser.ParamNames.DoubleDifferAmbiguity))
                { ambiParamNames.Add(name); }
            }
            var estVector = this.ResultMatrix.Estimated.GetWeightedVector(ambiParamNames);
            var vectorInCycle = estVector * (1.0 / Frequence.WaveLength);
            return vectorInCycle;
        }

        /// <summary>
        /// 获取模糊度参数估计结果。单位为周，权逆阵单位依然为米。
        /// </summary>
        /// <returns></returns>
        public WeightedVector GetFixableVectorInUnit22()
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
                //更新模糊度单位，前一半为L1，后一半为L2
                Frequence Frequence1 = Gnsser.Frequence.GetFrequence(this.BasePrn, FrequenceType.A, this.Material.ReceiverTime);
                Frequence Frequence2 = Gnsser.Frequence.GetFrequence(this.BasePrn, FrequenceType.B, this.Material.ReceiverTime);
                int i = 0;
                int half = estVector.Count / 2;
                foreach (var item in estVector)
                {
                    double len = Frequence1.WaveLength;
                    if (i >= half)
                    {
                        len = Frequence2.WaveLength;
                    }
                    estVector[i] = estVector[i] * (1.0 / len);
                    i++;
                }
                //log.Warn("此处前一半和后一半频率应该分开计算。"); 

                return estVector;
            }
            return estVector;
        }
        /// <summary>
        /// 固定后的模糊度,单位周，值应该为整数。
        /// </summary>
        public WeightedVector FixedParams { get; set; }

    }
}