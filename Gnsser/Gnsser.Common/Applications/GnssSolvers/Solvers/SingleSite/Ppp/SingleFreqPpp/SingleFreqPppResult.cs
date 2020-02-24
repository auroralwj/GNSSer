//2017.05.26, czs, create in hongqing, 建立单频PPP框架
//2017.05.26, cuiyang, edit in chongqing, 修改自非差非组合PPP，实现单频PPP


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
    public class SingleFreqPppResult :SingleSiteGnssResult
    {
        /// <summary>
        /// 精密单点定位结果构造函数。
        /// </summary>
        /// <param name="receiverInfo">接收信息</param>
        /// <param name="Adjustment">平差</param>
        /// <param name="paramNameBuilder">参数名称</param>
        public SingleFreqPppResult(
            EpochInformation receiverInfo,
            AdjustResultMatrix Adjustment, GnssParamNameBuilder paramNameBuilder
            )
            : base(receiverInfo, Adjustment, paramNameBuilder)
        {
            //处理模糊度 
            this.AmbiguityDic = new Dictionary<SatelliteNumber, double>();           

            Vector vector = Adjustment.Corrected.CorrectedValue;
            foreach (var name in this.ParamNames)
            {
                if (name.Contains(Gnsser.ParamNames.AmbiguityLen))
                {
                    var prn = SatelliteNumber.Parse(name);
                    var val = vector[name];
                    AmbiguityDic.Add(prn, val);

                }
            }  
        }
         


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
