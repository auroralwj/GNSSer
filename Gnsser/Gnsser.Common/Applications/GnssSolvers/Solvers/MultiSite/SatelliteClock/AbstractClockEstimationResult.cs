//2016.04.06 double create in Zhengzhou 参考AbstractPositionResult

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gnsser.Domain;
using Geo.Coordinates;
using Gnsser.Data.Rinex;
using Geo.Algorithm;
using Gnsser.Data.Sinex;
using Geo.Algorithm.Adjust;
using Geo;
using Geo.Algorithm;
using Geo.Times;
using Geo.IO;
using Gnsser.Service;

namespace Gnsser
{
    /// <summary>
    /// 通用的钟差估计结果
    /// </summary>
    public abstract class AbstractClockEstimationResult : MultiSiteGnssResult// BaseGnssResult, IDisposable, IToTabRow//Geo.Common.Named,
    {
        ILog log = new Log(typeof(AbstractClockEstimationResult));
        /// <summary>
        /// 
        /// </summary>
        /// <param name="epochInfos"></param>
        /// <param name="Adjustment"></param>
        /// <param name="nameBuilder"></param> 
        public AbstractClockEstimationResult(
           MultiSiteEpochInfo epochInfos,
           AdjustResultMatrix Adjustment,
           GnssParamNameBuilder nameBuilder) : base(epochInfos, Adjustment, nameBuilder)
        {    
            Vector corrected = Adjustment.Corrected.CorrectedValue;

             
            //update 

            this.DeltaTimeDistances = new Dictionary<string, double>(); 
            foreach (var epoch in epochInfos)
            {
                var key = NameBuilder.GetReceiverClockParamName(epoch);
                var val = corrected[Adjustment.GetIndexOf(key)];
                epoch.NumeralCorrections[Gnsser.ParamNames.cDt] = val;
                this.DeltaTimeDistances.Add(epoch.Name, val);
                epoch.Time.Correction = val / GnssConst.LIGHT_SPEED;
            }

        }
        /// <summary>
        /// 
        /// </summary>
        public int ParamCount
        {
            get
            {
                int ParamCount = 0;
                ParamCount = ObsCount + 2 * MaterialObj.Count + EnabledPrns.Count;
                return ParamCount;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public int ObsCount
        {
            get
            {
                int ObsCount = 0;
                foreach (var EpochInfo in MaterialObj)
                    ObsCount += EpochInfo.EnabledSatCount;
                return ObsCount;
            }
        } 
        #region 属性
        /// <summary>
        /// 钟差估计结果的元数据信息，用于分析结果
        /// </summary>
        public Dictionary<string, string> MetaInfo { get; set; }
        #region 基本属性 
        #endregion

        #region 需计算或提取转化的属性
        
        /// <summary>
        /// 接收机钟差等效距离偏差.
        /// </summary>
        public Dictionary<string, double> DeltaTimeDistances { get; set; }
        /// <summary>
        /// 卫星钟差等效距离偏差.
        /// </summary>
        public List<double> SatDeltaTimeDistance { get; set; } 
        #endregion

        #endregion 
        #region 显示
        /// <summary>
        /// 参数包括（测站钟差、对流程湿延迟、卫星钟差、N个模糊度）
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(this.Material.ReceiverTime.ToString() + ":");
            Vector vector = ResultMatrix.Corrected.CorrectedValue;
            foreach (var val in vector)
            {
                sb.Append(" " + Geo.Utils.StringUtil.FillSpaceLeft(val.ToString("0.0000"), 8) + ",");
            }
            return sb.ToString();
        }
         
        #endregion
    }
}