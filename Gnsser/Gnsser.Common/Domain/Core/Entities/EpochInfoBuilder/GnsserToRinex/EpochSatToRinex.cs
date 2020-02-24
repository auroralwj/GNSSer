//2015.10.16, czs, create in D5181达州到成都东， Gnsser 站星对象转换为RINEX对象。
//2018.06.23, czs, edit in HMX, 获取RINEX类型代码修改，有待改进
//2018.07.18, czs, edit in HMX, 采用ObservationCode进行转换
//2018.09.06, czs, edit in HMX, 增加可选使用改正数，同时更新观测码

using System;
using System.Collections.Generic;
using System.Text;
using Geo.Algorithm;
using Geo;
using Geo.Correction;
using Geo.Coordinates;
using Geo.Algorithm.Adjust;
using Gnsser.Service;
using Gnsser.Data.Rinex;
using Gnsser.Correction;

namespace Gnsser.Domain
{

    /// <summary>
    /// Gnsser 站星对象转换为RINEX对象。
    /// </summary>
    public class EpochSatToRinex : AbstractBuilder<RinexSatObsData, EpochSatellite>
    {
        /// <summary>
        /// 构造函数。 Gnsser 频率标识码转换到RINEX观测标识。
        /// </summary>
        /// <param name="rinexVersion"></param>
        /// <param name="IsUseCorrectedOfRange">是否将伪距观测值改正加入，包括平滑伪距，DCB等，注意DCB改正后，应该修改对应的观测码，
        /// 如P1C1改正后应该将C1C修改为C1W，如果觉得麻烦，则不要勾选P1C1修改。</param> 
        public EpochSatToRinex(double rinexVersion = 3.02, bool IsUseCorrectedOfRange = false)
        {
            this.RinexVersion = rinexVersion;
            this.IsUseCorrectedOfRange = IsUseCorrectedOfRange;
        }
        /// <summary>
        /// 版本
        /// </summary>
        public double RinexVersion { get; set; }
        /// <summary>
        /// 是否采用伪距改正数，若有，需改正观测码
        /// </summary>
        public bool IsUseCorrectedOfRange { get; set; }
        /// <summary>
        /// 创建，转换为RINEX格式
        /// </summary>
        /// <param name="epochSat"></param> 
        /// <returns></returns>
        public override RinexSatObsData Build(EpochSatellite epochSat)
        {
            RinexSatObsData rinex = new RinexSatObsData();
            rinex.Prn = epochSat.Prn;
            
            foreach (var observtion in epochSat)
            {
                //C1 ，可能有多种伪距的情况
                var cc = observtion.GetPseudoRanges();
                foreach (var c in cc)
                {
                    RinexObsValue C = new RinexObsValue(c.Value, c.ObservationCode);
                    var obsCode = c.ObservationCode;
                    if (IsUseCorrectedOfRange) {//观测值更新
                        C.Value = c.CorrectedValue;
                        if(c.ContainsCorrection(CorrectionNames.DcbP1C1.ToString())){
                            ObservationCode.ChagngeCaToP( ref obsCode);
                        }
                    }
                    var rinexCode = c.ObservationCode.GetRinexCode(RinexVersion);
                    rinex.Add(rinexCode, C); 
                }


                //L1
                var l = observtion.PhaseRange;
                RinexObsValue L = new RinexObsValue(l.RawPhaseValue, l.ObservationCode); 
                L.LossLockIndicator = observtion.PhaseRange.LossLockIndicator;//还给你
                if (epochSat.IsUnstable) //如果探测出周跳，则重新标记为有周跳。
                {
                    L.LossLockIndicator = LossLockIndicator.CyclePossible1;
                }
                L.SignalStrength = observtion.PhaseRange.SignalStrength;
                var lcode = l.ObservationCode.GetRinexCode(RinexVersion);
                rinex.Add(lcode, L);
            }

            return rinex;
        }
    }
    



}