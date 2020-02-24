//2014.12.07, czs , create in jinxingliaomao shuangliao, 伪距差分改正

using System;
using System.Text;
using Gnsser.Domain;
using Gnsser.Service;
using Gnsser.Data.Rinex;
using Geo.Algorithm.Adjust;
using Geo.Utils;
using Geo.Coordinates;
using Geo.Algorithm;

namespace Gnsser.Correction
{
    /// <summary>
    /// 伪距差分改正 EΔ = R - pho + dtc
    /// </summary>
    public class RangeDifferCorrector :  AbstractRangeCorrector
    {
        /// <summary>
        /// 构造函数
        /// </summary> 
        public RangeDifferCorrector( EpochInformation refEpochInfo, double refClkError, SatObsDataType dataType = SatObsDataType.PseudoRangeA)
        {
            this.Name = "伪距差分距离改正";
            this.CorrectionType = CorrectionType.RangeDifferCorrector;
            this.RefEpochInfo = refEpochInfo;
            this.RefClkError = refClkError;
            this.EpochSatDataType = dataType;
        }

        SatObsDataType EpochSatDataType { get; set; }
        /// <summary>
        /// 参考历元信息
        /// </summary>
        EpochInformation RefEpochInfo { get; set; }
        /// <summary>
        /// 参考接收机的钟差
        /// </summary>
        double RefClkError { get; set; }

        public override void Correct(EpochSatellite epochSatellite)
        {
            if (RefEpochInfo.Contains(epochSatellite.Prn))
            {
                //计算公共模型改正数
                var sat = RefEpochInfo[epochSatellite.Prn];
                var obs = sat[EpochSatDataType];
                //这个改正数，直接添加到流动站的站星距离上，就不用再添加其它任何改正了。
                this.Correction = obs.CorrectedValue //参考站伪距观测值
                    - sat.ApproxVector.Norm//参考站站星距离真值
                    + RefClkError * GnssConst.LIGHT_SPEED; //参考站接收机距离误差
            }
        } 
    }
}
