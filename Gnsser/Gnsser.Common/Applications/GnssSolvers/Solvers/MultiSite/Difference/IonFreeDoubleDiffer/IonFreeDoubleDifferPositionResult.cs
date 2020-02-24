//2015.05.23,cy, 复杂的东东总是搞的很复杂，应该简单化，重新定义


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
    /// 双差差分定位结果。具有模糊度。。
    /// </summary>
    public class IonFreeDoubleDifferPositionResult : DualSiteEpochDoubleDifferResult, IFixableParamResult
    {

        /// <summary>
        /// 双差差分定位结果。具有模糊度。
        /// </summary>
        /// <param name="mInfo">历元观测信息</param>
        /// <param name="Adjustment"></param>
        /// <param name="positioner"></param>
        /// <param name="baseSatPrn"></param>
        /// <param name="baseParamCount"></param>
        public IonFreeDoubleDifferPositionResult(
            MultiSiteEpochInfo mInfo,
            AdjustResultMatrix Adjustment,
             GnssParamNameBuilder positioner,
            SatelliteNumber baseSatPrn,
            int baseParamCount = 5
            )
            : base(mInfo, Adjustment, positioner)
        {
            this.Name = mInfo.BaseEpochInfo.Name + Gnsser.ParamNames.BaseLinePointer+ mInfo.OtherEpochInfo.Name;
            this.BasePrn = baseSatPrn;            

            int index = Adjustment.GetIndexOf(Gnsser.ParamNames.WetTropZpd);
            if (index != -1)
            {
                this.WetTropoFactor = this.ResultMatrix.Corrected.CorrectedValue[Adjustment.GetIndexOf(Gnsser.ParamNames.WetTropZpd)];
            }
            else
            {
                this.WetTropoFactor = 0.0;
            }
            //处理模糊度 
            this.AmbiguityDic = new Dictionary<SatelliteNumber, double>();             
            Vector vector = Adjustment.Corrected.CorrectedValue; 

            int satIndex = 0;
            foreach(var item in mInfo.EnabledPrns)
            {
                if(item!=BasePrn)
                {
                    double val = vector[satIndex];
                    AmbiguityDic.Add(item, val);
                    satIndex++;
                }
            }
        }
        /// <summary>
        /// 定位选项
        /// </summary>
        public GnssProcessOption Option { get; set; }

        /// <summary>
        /// 天顶距延迟（zenith path delay，zpd)
        /// </summary>
        public double WetTropoFactor { get; set; }
         
        /// <summary>
        /// 计算的等效模糊度距离。
        /// 如果有则返回，若无返回0。
        /// </summary>
        /// <param name="prn"></param>
        /// <returns></returns>
        public double GetAmbiguityDistace(SatelliteNumber prn)
        {
            if (AmbiguityDic.ContainsKey(prn))
                return AmbiguityDic[prn];
            else return 0;
        }

        /// <summary>
        /// 模糊度字典.L1，L2组合整周模糊度等效的距离长度。
        /// </summary>
        private Dictionary<SatelliteNumber, double> AmbiguityDic { get; set; }


        /// <summary>
        /// 固定后的模糊度,单位周，值应该为整数。
        /// </summary>
        public WeightedVector FixedParams { get; set; }

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

    }
}
