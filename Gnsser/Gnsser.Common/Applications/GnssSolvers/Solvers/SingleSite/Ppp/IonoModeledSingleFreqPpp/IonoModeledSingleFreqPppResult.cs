//2017.10.12, czs, create in hongqing, 电离层模型改正单频PPP


using System;
using System.Collections.Generic;
using System.Text;
using Geo.Coordinates;
using Gnsser.Data.Rinex;
using Geo.Algorithm;
using Gnsser.Data.Sinex;
using Gnsser.Domain;
using Geo.Algorithm;
using Geo.Algorithm.Adjust;
using Geo;


namespace Gnsser.Service
{
    /// <summary>
    /// 精密单点定位结果。与普通单点定位区别在于精密单点定位结果增加了模糊度参数。
    /// </summary>
    public class IonoModeledSingleFreqPppResult :SingleSiteGnssResult
    {
        /// <summary>
        /// 精密单点定位结果构造函数。
        /// </summary>
        /// <param name="receiverInfo">接收信息</param>
        /// <param name="Adjustment">平差</param>
        /// <param name="nameBuilder">参数名称生成器</param>
        public IonoModeledSingleFreqPppResult(
            EpochInformation receiverInfo,
            AdjustResultMatrix Adjustment, GnssParamNameBuilder nameBuilder
            )
            : base(receiverInfo, Adjustment, nameBuilder)
        {
            this.WetTropoFactor = this.ResultMatrix.Corrected.CorrectedValue[Adjustment.GetIndexOf(Gnsser.ParamNames.WetTropZpd)];
            //处理模糊度 
            this.AmbiguityDic = new Dictionary<SatelliteNumber, double>();

            int length = receiverInfo.EnabledSatCount * 2 + 5;

            Vector vector = Adjustment.Corrected.CorrectedValue;
            for (int i = 5 + receiverInfo.EnabledSatCount; i < 5 + receiverInfo.EnabledSatCount; i++)
            {
                SatelliteNumber prn = receiverInfo[i - 5 - receiverInfo.EnabledSatCount].Prn;
                //double val = vector[i];
                //double[] AmbiguityItem = new double[1];

                //AmbiguityItem[0] = val;
                ////AmbiguityItem[1] = vector[i + receiverInfo.EnabledSatCount];

                //AmbiguityDic.Add(prn, val);

            }
        }


        /// <summary>
        /// 基准卫星。
        /// </summary>
        public SatelliteNumber BasePrn { get; set; }


        /// <summary>
        /// 天顶距延迟（zenith path delay，zpd)
        /// </summary>
        public double WetTropoFactor { get; set; }

        /// <summary>
        /// 计算的等效模糊度距离。
        /// 如果有则返回，若无返回0。
        /// </summary>
        /// <param name="satelliteType"></param>
        /// <returns></returns>
        public double GetAmbiguityDistace(SatelliteNumber prn)
        {
            if (AmbiguityDic.ContainsKey(prn))
                return AmbiguityDic[prn];
            else return 0;
        }

        /// <summary>
        /// 模糊度字典.L1，L2组合整周模糊度等效的距离长度。每颗卫星按频率排列的模糊度，如L1、L2的模糊度
        /// </summary>
        private Dictionary<SatelliteNumber, double> AmbiguityDic { get; set; }


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
    }
}
