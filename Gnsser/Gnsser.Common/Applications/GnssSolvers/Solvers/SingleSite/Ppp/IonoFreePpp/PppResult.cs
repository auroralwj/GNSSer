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
    public class PppResult : SingleSiteGnssResult, IFixableParamResult// PhasePositionResult
    {
        /// <summary>
        /// 精密单点定位结果构造函数。
        /// </summary>
        /// <param name="receiverInfo">接收信息</param>
        /// <param name="Adjustment">平差</param>
        /// <param name="positioner">定位器</param>
        /// <param name="baseParamCount">基础参数数量</param>
        public PppResult(
            EpochInformation receiverInfo,
            AdjustResultMatrix Adjustment,
            GnssParamNameBuilder positioner,
            int baseParamCount = 5, bool isTopSpeed = false
            )
            : base(receiverInfo, Adjustment, positioner, isTopSpeed)
        {
            if (!isTopSpeed)
            {
                if (this.ResultMatrix.ParamNames.Contains(Gnsser.ParamNames.WetTropZpd) || this.ResultMatrix.ParamNames.Contains(Gnsser.ParamNames.Trop))
                {
                    this.WetTropoFactor = this.ResultMatrix.Corrected.CorrectedValue[Adjustment.GetIndexOf(Gnsser.ParamNames.WetTropZpd)];
                }

                //处理模糊度 
                this.AmbiguityDic = new Dictionary<string, double>();
                int length = receiverInfo.EnabledSatCount + baseParamCount;
                //this.BasePrn = positioner.CurrentBasePrn;

                Vector vector = Adjustment.Corrected.CorrectedValue;
                var enabledPrns = receiverInfo.EnabledPrns;
                int i = baseParamCount;
                foreach (var prn in enabledPrns)
                {
                    var name = NameBuilder.GetParamName(prn);
                    var index = Adjustment.GetIndexOf(name);
                    if (index == -1) { continue; }

                    double val = vector[index];
                    var key = GetSiteSatMaker(SiteInfo.SiteName, prn);
                    AmbiguityDic.Add(key, val);
                    i++;
                }
            }
        }
        /// <summary>
        /// 模糊度字典.L1，L2组合整周模糊度等效的距离长度。
        /// </summary>
        public Dictionary<string, double> AmbiguityDic { get; set; }

        /// <summary>
        /// 天顶距延迟（zenith path delay，zpd)
        /// </summary>
        public double WetTropoFactor { get; set; }


        public string GetSiteSatMaker(string siteName, SatelliteNumber prn)
        {
            return siteName + "-" + prn.ToString();
        }
        /// <summary>
        /// 是否具有指定卫星的模糊度浮点解
        /// </summary>
        /// <param name="prn"></param>
        /// <returns></returns>
        public bool HasAmbituityValue(SatelliteNumber prn)
        {
            var key = GetSiteSatMaker(SiteInfo.SiteName, prn);
            return this.AmbiguityDic.ContainsKey(key);
        }
        /// <summary>
        /// 计算的等效模糊度距离。
        /// 如果有则返回，若无返回0。
        /// </summary>
        /// <param name="prn"></param>
        /// <returns></returns>
        public double GetAmbiguityDistance(SatelliteNumber prn)
        {
            var key = GetSiteSatMaker(SiteInfo.SiteName, prn);
            if (AmbiguityDic.ContainsKey(key))
                return AmbiguityDic[key];
            else return 0;
        }

        /// <summary>
        /// 计算的等效模糊度距离。
        /// 如果有则返回，若无返回0。
        /// </summary>
        /// <param name="prn"></param>
        /// <returns></returns>
        public double GetFloatAmbiguityCycle(SatelliteNumber prn)
        {
            var key = GetSiteSatMaker(SiteInfo.SiteName, prn);
            var val = 0.0;
            if (AmbiguityDic.ContainsKey(key))
                val = AmbiguityDic[key];
            var len = 1.0;
            if (this.Material.EnabledPrns.Contains(prn))
            {
                var waveLength = Frequence.GpsL1.WaveLength;//???
                                                            //    len = Frequence.GetIonoFreeFrequence(prn.SatelliteType).WaveLength;// this.MaterialObj[prn].Combinations.IonoFreePhaseRange.Frequence.WaveLength;
                int i = 0;
            }

            return val / len;
        }
        /// <summary>
        /// 获取指定测站和卫星的模糊度距离。
        /// </summary>
        /// <param name="siteName"></param>
        /// <param name="prn"></param>
        /// <returns></returns>
        public double GetAmbiguityDistance(string siteName, SatelliteNumber prn)
        {
            var key = GetSiteSatMaker(siteName, prn);
            if (AmbiguityDic.ContainsKey(key))
                return AmbiguityDic[key];
            else return 0;
        }

        /// <summary>
        /// 标题
        /// </summary>
        /// <returns></returns>
        public override string GetTabTitles()
        {
            return base.GetTabTitles() + "\t" + "CycleClip";
        }

        /// <summary>
        /// 行模式数值
        /// </summary>
        /// <returns></returns>
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

        #region IFixableParamResult

        /// <summary>
        /// 固定后的模糊度,单位周，值应该为整数。
        /// </summary>
        public WeightedVector FixedParams { get; set; }

        /// <summary>
        /// 获取模糊度参数估计结果。单位依然为米。
        /// </summary>
        /// <returns></returns>
        public WeightedVector GetFixableVectorInUnit()
        {
            this.ResultMatrix.Estimated.ParamNames = this.ResultMatrix.ParamNames;

            var ambiParamNames = new List<string>();
            foreach (var name in ResultMatrix.ParamNames)
            {
                //if (HasUnstablePrn(name)) { continue; }

                if (name.Contains(Gnsser.ParamNames.AmbiguityLen))
                { ambiParamNames.Add(name); }
            }
            var estVector = this.ResultMatrix.Estimated.GetWeightedVector(ambiParamNames);
            //if (Option.IsPhaseInMetterOrCycle)
            //{
            //    //更新模糊度。
            //    Frequence Frequence = Gnsser.Frequence.GetFrequence(this.BasePrn, Option.ObsDataType, this.Material.ReceiverTime);
            //    var vectorInCycle = estVector * (1.0 / Frequence.WaveLength);
            //    return vectorInCycle;
            //}
            return estVector;
        }
        #endregion
    }
}