//2017.8.1, cy, create in chongqing, 双频相位载波双差定位结果

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
using Geo.Algorithm;


namespace Gnsser.Service
{
    /// <summary>
    /// 单历元载波相位双差差分定位结果。具有模糊度。。
    /// </summary>
    public class EpochDouFreDoubleDifferPositionResult : DualSiteEpochDoubleDifferResult
    {
        /// <summary>
        /// 双差差分定位结果。具有模糊度。
        /// </summary>
        /// <param name="epochInfo">历元观测信息</param>
        /// <param name="PointPositionType">单点定位类型</param>
        public EpochDouFreDoubleDifferPositionResult(
            MultiSiteEpochInfo receiverInfo,
            AdjustResultMatrix Adjustment,
             GnssParamNameBuilder positioner,
            SatelliteNumber baseSatPrn,
            int baseParamCount = 5
            )
            : base(receiverInfo, Adjustment, positioner)
        {
            this.BasePrn = baseSatPrn;
            int index = Adjustment.GetIndexOf(Gnsser.ParamNames.WetTropZpd);
            if (index != -1)
            {
                this.WetTropoFactor = this.ResultMatrix.Corrected.CorrectedValue[Adjustment.GetIndexOf(Gnsser.ParamNames.WetTropZpd)];
            }
            else
                this.WetTropoFactor = 0.0;
            //处理模糊度 
            this.AmbiguityDic = new Dictionary<SatNumberAndFrequence, double>();
             
            Vector vector = Adjustment.Corrected.CorrectedValue;
             
            int satIndex = 0;
            foreach(var item in receiverInfo.EnabledPrns)
            {
                if(item!=BasePrn)
                {
                    Frequence FrequenceA = Frequence.GetFrequence(this.BasePrn, SatObsDataType.PhaseRangeA, receiverInfo.ReceiverTime);
                    Frequence FrequenceB = Frequence.GetFrequence(this.BasePrn, SatObsDataType.PhaseRangeB, receiverInfo.ReceiverTime);
                    SatNumberAndFrequence satA = new SatNumberAndFrequence();
                    satA.SatNumber = item; satA.Frequence = FrequenceA;
                    SatNumberAndFrequence satB = new SatNumberAndFrequence();
                    satB.SatNumber = item; satB.Frequence = FrequenceB;
                    double val = vector[satIndex];
                    AmbiguityDic.Add(satA, val);
                    AmbiguityDic.Add(satB, val);
                    satIndex++;
                }
            }
             
        }
       public  GnssProcessOption DifferPositionOption { get; set; }

       public struct SatNumberAndFrequence
       {
           /// <summary>
           /// 卫星编号
           /// </summary>
           public SatelliteNumber SatNumber { get; set; }
           /// <summary>
           /// 频率
           /// </summary>
           public Frequence Frequence { get; set; }
       }
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
        public double GetAmbiguityDistace(SatNumberAndFrequence PrnFre)
        {
            if (AmbiguityDic.ContainsKey(PrnFre))
                return AmbiguityDic[PrnFre];
            else return 0;
        }

        /// <summary>
        /// 模糊度字典.L1，L2组合整周模糊度等效的距离长度。
        /// </summary>
        private Dictionary<SatNumberAndFrequence, double> AmbiguityDic { get; set; }


        public override string GetTabTitles()
        {
            return base.GetTabTitles() + "\t" + "CycleClip";
        }

        public override string GetTabValues()
        {
            StringBuilder sb = new StringBuilder();
            if (this.Material.UnstablePrns.Count > 0)
            {
               // sb.Append("CS:");
                foreach (var item in this.Material.UnstablePrns)
                {
                    sb.Append(item.ToString());
                    sb.Append(" ");
                }
            }
            return base.GetTabValues() + "\t" + sb.ToString();
        }



        /// <summary>
        /// 获取模糊度参数估计结果。单位为周，权逆阵单位依然为米。权逆阵单位改为周，edited by CuiYang.
        /// 注意：这里是针对双频的L1和L2。
        /// </summary>
        /// <returns></returns>
        public WeightedVector GetWeightedAmbiguityVectorInCycle()
        {
            this.ResultMatrix.Estimated.ParamNames = this.ResultMatrix.ParamNames;
            
            //Lamdba方法计算模糊度
            List<string> ambiParamNames = new List<string>();
            foreach (var name in ResultMatrix.ParamNames)
            {

                if (name.Contains(Gnsser.ParamNames.DoubleDifferL1Ambiguity)||name.Contains(Gnsser.ParamNames.DoubleDifferL2Ambiguity))
                    ambiParamNames.Add(name);
            }
            WeightedVector estVector = this.ResultMatrix.Estimated.GetWeightedVector(ambiParamNames);

            IVector vector = estVector;//系数矩阵是波长，所以，已经是周为单位了。
            IMatrix matrix = estVector.InverseWeight;
            var vec = new WeightedVector(vector, matrix);
            vec.ParamNames = new List<string>(estVector.ParamNames);
            return vec;

        }
        /// <summary>
        /// 固定后的模糊度,单位周，值应该为整数。
        /// </summary>
        public WeightedVector FixedIntAmbiguities { get; set; }
    }
}
