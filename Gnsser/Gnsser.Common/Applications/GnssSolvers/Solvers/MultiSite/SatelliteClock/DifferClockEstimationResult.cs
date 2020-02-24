//2016.10.06 double creates on the train of quxian-xi'an 


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
using Gnsser.Service;
using Geo.IO;

namespace Gnsser
{
    /// <summary>
    /// 通用的钟差估计结果
    /// </summary>
    public class DifferClockEstimationResult : MultiSitePeriodGnssResult
    {
        ILog log = new Log(typeof(DifferClockEstimationResult));
        /// <summary>
        /// 钟差估计构造函数
        /// </summary>
        /// <param name="epochInfo">历元信息</param>
        /// <param name="Adjustment">平差信息</param>
        /// <param name="ClockEstimationer">钟差估计器</param>
        /// <param name="previousResult">上一历元结果</param>
        public DifferClockEstimationResult(
            MultiSitePeriodInfo epochInfo,
            AdjustResultMatrix Adjustment,
            GnssParamNameBuilder DifferenceClockEstimationer,
            DifferClockEstimationResult previousResult = null)
            : base(epochInfo, Adjustment, DifferenceClockEstimationer)
        { 
        }
        #region 显示
        /// <summary>
        /// 参数包括（测站钟差、对流程湿延迟、卫星钟差）
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
