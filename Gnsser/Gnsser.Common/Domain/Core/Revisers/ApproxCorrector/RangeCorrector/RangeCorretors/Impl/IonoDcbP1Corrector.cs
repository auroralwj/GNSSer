//2018.05.16, czs,  create in HMX, 电离层 DCB P1 距离硬件延迟改正
//2018.08.13, czs, eidt in HMX, 重构，重新整理,统一命名

using System;
using System.Text;
using Geo.Coordinates;
using Gnsser.Data.Rinex;
using Geo.Algorithm;
using Geo.Algorithm.Adjust;
using Gnsser.Service;
using Geo.Utils;
using Gnsser.Data;
using Gnsser.Domain;

namespace Gnsser.Correction
{ 

    /// <summary>
    /// 电离层 DCB P1 距离改正, 也可以直接改正IGS钟差产品
    /// </summary>
    public class IonoDcbP1Corrector : AbstractSelfRangeCorrector
    {
        /// <summary>
        /// 构造函数
        /// </summary> 
        public IonoDcbP1Corrector(IGridIonoFileService DcbDataService)
        {
            this.Name = "电离层 DCB P1 硬件延迟距离改正";
            this.CorrectionType = Gnsser.Correction.CorrectionType.Dcb;
            this.DcbDataService = DcbDataService;
        }

        IGridIonoFileService DcbDataService { get; set; }

        /// <summary>
        /// 改正
        /// </summary>
        /// <param name="sat"></param>
        public override void Correct(EpochSatellite sat)
        {
            if (this.DcbDataService == null)
            {
                log.Error(sat.Name + ", " + sat.ReceiverTime + ", 服务数据源 为空 ");
                return;
            }

            SatelliteNumber prn = sat.Prn;
            if (sat.Prn.SatelliteType == SatelliteType.G)
            { 
                var dcbOfP1P2 = DcbDataService.GetDcb(sat.ReceiverTime, prn);

                //此处与CODE的DCB采用相同的名称，避免重复改正
                double valForP1 = dcbOfP1P2.Value * GnssConst.DcbMultiplierOfGPSL1 * GnssConst.MeterPerNano;
                sat.FrequenceA.PseudoRange.SetCorrection(CorrectionNames.DcbOfP1ToLc, valForP1);
                if (sat.Contains(FrequenceType.B))
                {
                   double valForP2 = dcbOfP1P2.Value * GnssConst.DcbMultiplierOfGPSL2 * GnssConst.MeterPerNano;
                    sat.FrequenceB.PseudoRange.SetCorrection(CorrectionNames.DcbOfP2ToLc, valForP2);
                }
            }

        }

    }
}
