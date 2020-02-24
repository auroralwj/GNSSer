//2014.10, cy,  create, DCB
//2014.10.27， czs, edit in namu, DcbRangeCorrector
//2015.01.16, lly, edit in zz, 增加 P1 改正
//2017.05.10, lly, edit in zz, 分别对伪距观测值改正改正
//2017.09.27, cy, edit in chongqing, 添加2-3频DCB改正
//2018.05.17, czs, edit in HMX, 增加P1P2开关，分开显示P1C1，P2C2等。
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
    /// DCB， 伪距改正数
    /// </summary>
    public class DcbRangeCorrector : AbstractSelfRangeCorrector
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="DcbDataService"></param>
        /// <param name="IsDcbOfP1P2Enabled">是否改正P1P2，单频适用，IonoFreePPP不适用</param>
        public DcbRangeCorrector(DcbDataService DcbDataService, bool IsDcbOfP1P2Enabled)
        {
            this.IsP1P2Enabled = IsDcbOfP1P2Enabled;
            this.Name = "伪距 DCB 距离改正";
            this.CorrectionType = Gnsser.Correction.CorrectionType.Dcb;
            this.DcbDataService = DcbDataService;
            log.Info(Name + "， 目前只会改正GPS系统的DCB！");
        }
        /// <summary>
        /// DCB 服务
        /// </summary>
        DcbDataService DcbDataService { get; set; }
        /// <summary>
        /// 是否启用P1P2改正,单频需要，若是无电离层组合，则不需要。
        /// </summary>
        public bool IsP1P2Enabled { get; set; }
        /// <summary>
        /// 改正
        /// </summary>
        /// <param name="sat"></param>
        public override void Correct(EpochSatellite sat)
        {
            if (this.DcbDataService == null)
            {
                log.Warn(sat.Name + ", " + sat.ReceiverTime + ", DcbDataService 为空 ");
                return;
            }

            SatelliteNumber prn = sat.Prn;
            //在单频中使用双频产品时必须改正硬件延迟
            if (sat.Prn.SatelliteType == SatelliteType.G)
            { 
                double DcbP1P2 = this.DcbDataService.GetP1P2(prn, sat.Time.Value).Value * GnssConst.MeterPerNano; 

                foreach (var kv in sat.Data)
                {
                    var freqType = kv.Key;
                    var freq = kv.Value;

                    //逐个频率考虑
                    if (freqType == FrequenceType.A) //对C1改正
                    {
                        //CA to P1 一定要改正

                        foreach (var sameObs in kv.Value)
                        {
                            if (sameObs.ObservationType == ObservationType.C)
                            {
                                foreach (var obs in sameObs)
                                {
                                    if (obs.GnssCodeType == GnssCodeType.CA)
                                    {
                                        double DcbP1C1 = this.DcbDataService.GetP1C1(prn, sat.Time.Value).Value * GnssConst.MeterPerNano;
                                        sat.DcbP1C1 = DcbP1C1;   //暂时保留，用于MW，2018.08.13， czs
                                        obs.SetCorrection(CorrectionNames.DcbP1C1, DcbP1C1);//直接改正，没有系数
                                    }


                                    //可选，单频才需要，否则还会影响精度
                                    if (IsP1P2Enabled)//是否启用P1P2改正,单频需要，若是无电离层组合，则不需要。
                                    {
                                        var corr = GnssConst.DcbMultiplierOfGPSL2 * DcbP1P2;
                                        obs.SetCorrection(CorrectionNames.DcbOfP1ToLc, corr);

                                        //载波具有单独的DCB改正，但是通常并不改正他们，因为消化在模糊度中了，2018.08.13, czs, Hmx, 
                                        // freq.PhaseRange.SetCorrection("DcbP1P2", b * DcbP1P2);
                                    }
                                }
                            }
                        }
                    }

                    if (freqType == FrequenceType.B) //对C1改正
                    {
                        ////CA to P2 一定要改正
                        //if (freq.PseudoRange.GnssCodeType == GnssCodeType.CA)
                        //{
                        //    double DcbP2C2 = this.DcbDataService.GetP2C2(prn, sat.Time.Value).Value * GnssConst.MeterPerNano;
                        //    freq.PseudoRange.SetCorrection(CorrectionNames.DcbP2C2, DcbP2C2);//直接改正，没有系数
                        //}
                        ////可选，单频才需要，否则还会影响精度
                        //if (IsP1P2Enabled) 
                        //{
                        //    var corr = GnssConst.DcbMultiplierOfGPSL2 * DcbP1P2;
                        //    freq.PseudoRange.SetCorrection(CorrectionNames.DcbOfP2ToLc, corr);
                        //}


                        foreach (var sameObs in kv.Value)
                        {
                            if (sameObs.ObservationType == ObservationType.C)
                            {
                                foreach (var obs in sameObs)
                                {
                                    if (obs.GnssCodeType == GnssCodeType.CA)
                                    {
                                        double DcbP2C2 = this.DcbDataService.GetP2C2(prn, sat.Time.Value).Value * GnssConst.MeterPerNano;
                                        obs.SetCorrection(CorrectionNames.DcbP2C2, DcbP2C2);//直接改正，没有系数
                                    }

                                    //可选，单频才需要，否则还会影响精度
                                    if (IsP1P2Enabled)
                                    {
                                        var corr = GnssConst.DcbMultiplierOfGPSL2 * DcbP1P2;
                                        obs.SetCorrection(CorrectionNames.DcbOfP2ToLc, corr);
                                    }
                                }
                            }
                        }


                    }


                    if (freqType == FrequenceType.C) //对P5改正
                    {
                        //to be continue!
                    }

                }
            }



        }

    }
}
