//2014.08.29, czs, edit, 行了继承设计

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
    public class UncombinedPppResult :SingleSiteGnssResult
    {
        /// <summary>
        /// 精密单点定位结果构造函数。
        /// </summary>
        /// <param name="receiverInfo">接收信息</param>
        /// <param name="Adjustment">平差</param>
        /// <param name="paramNames">参数名称</param>
        public UncombinedPppResult(
            EpochInformation receiverInfo,
            AdjustResultMatrix Adjustment, GnssParamNameBuilder positioner
            )
            : base(receiverInfo, Adjustment, positioner)
        {
            this.WetTropoFactor = this.ResultMatrix.Corrected.CorrectedValue[Adjustment.GetIndexOf(Gnsser.ParamNames.WetTropZpd)];
            //处理模糊度 
        }
        

        /// <summary>
        /// 天顶距延迟（zenith path delay，zpd)
        /// </summary>
        public double WetTropoFactor { get; set; }

       

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
