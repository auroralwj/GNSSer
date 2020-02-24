//2017.10.19， czs, create,历元电离层参数改正。

using System;
using System.Text;
using Geo.Coordinates;
using Gnsser.Data.Rinex;
using Geo.Algorithm;
using Geo.Algorithm.Adjust;
using Gnsser.Service;
using Geo.Utils;
using Gnsser.Domain;
using Gnsser.Data;

namespace Gnsser.Correction
{    
    /// <summary>
    /// 历元电离层参数改正,单频
    /// </summary>
    public class EpochParamIonoCorrector : AbstractRangeCorrector
    {
        /// <summary>
        /// 构造函数 历元电离层参数改正
        /// </summary>
        public EpochParamIonoCorrector(IonoEpochParamService IonoService, bool isPhase = false)
        {
            this.Name = "历元电离层参数改正";
            this.IsCorrectionOnPhase = isPhase;
            this.CorrectionType = CorrectionType.IonoGridModel;
            this.IonoService = IonoService;
            if (IonoService == null) { log.Error(this.Name + "服务源为空"); }
        } 
        /// <summary>
        /// 是否改正到相位上。
        /// </summary>
        public bool IsCorrectionOnPhase { get; set; } 

        /// <summary>
        /// 对流层文件服务
        /// </summary>
        IonoEpochParamService IonoService { get; set; }

          

     
        public override void Correct(EpochSatellite epochSatellite)
        {
            if (IonoService == null) { return; }
            double slopeDelay = IonoService.Get(epochSatellite.Prn, epochSatellite.ReceiverTime);

            this.Correction =1.0 * slopeDelay;

            if (IsCorrectionOnPhase)
            {
                this.Correction = -this.Correction;
            }
            //双频检核
            if (epochSatellite.FrequencyCount > 1)
            {
                var rangeA = epochSatellite.FrequenceA.PseudoRange.Value;
                var ionoFreeRange = epochSatellite.Combinations.IonoFreeRange.Value;
                var ionoDelay = ionoFreeRange - rangeA;
                int i = 0;
            }
         }  
    }
}
