//2018.05.29, czs,  create in HMX, 载波相位平滑伪距
//2018.09.06, czs, edit in hmx, 单频逐个频率改正

using System;
using System.Collections.Generic;
using System.Text;
using Geo.Coordinates;
using Gnsser.Data.Rinex;
using Geo.Algorithm;
using Geo.Algorithm.Adjust;
using Gnsser.Service;
using Geo.Utils;
using Gnsser.Data;
using Gnsser.Domain;
using Geo;
using Geo.Times;
using Gnsser;
using Geo.Correction;

namespace Gnsser.Correction
{

    /// <summary>
    /// 缓存多项式伪距拟合平滑。
    /// 根据观测类型进行改正。支持双频。
    /// 需要在周跳探测后使用。
    /// </summary>
    public class PhaseSmoothRangeReviser : EpochInfoReviser
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="processOption"></param>
        public PhaseSmoothRangeReviser(GnssProcessOption processOption)
            : this(processOption.IsUseGNSSerSmoothRangeMethod,
                 processOption.ObsDataType == SatObsDataType.IonoFreePhaseRange || processOption.ObsDataType == SatObsDataType.IonoFreeRange,
                 processOption.WindowSizeOfPhaseSmoothRange,
                 processOption.IsWeightedPhaseSmoothRange,
                 processOption.IonoDifferCorrectionType,
                  processOption.OrderOfDeltaIonoPolyFit,
                  processOption.BufferSize,
                  processOption.IonoFitEpochCount,
                  processOption.SmoothRangeSuperPosType,
                  processOption.IonoDeltaFilePath
                 )
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="isUseingApprovedMethod"></param>
        /// <param name="isIonoFreeDualFreq"></param>
        /// <param name="WindowSize"></param>
        /// <param name="isWeighted"></param>
        /// <param name="ionoDifferCorrectionType"></param>
        /// <param name="OrderOfDeltaIonoPolyFit"></param>
        /// <param name="BufferSize"></param>
        /// <param name="ionoFitEpochCount"></param>
        /// <param name="SmoothRangeType"></param>
        /// <param name="ionoDeltaFilePath"></param>
        public PhaseSmoothRangeReviser(
            bool isUseingApprovedMethod,
            bool isIonoFreeDualFreq,
            int WindowSize,
            bool isWeighted,
            IonoDifferCorrectionType ionoDifferCorrectionType,
            int OrderOfDeltaIonoPolyFit = 1,
             int BufferSize = 60,
             int ionoFitEpochCount = 30,
            SmoothRangeSuperpositionType SmoothRangeType = SmoothRangeSuperpositionType.窗口AK迭代,
            string ionoDeltaFilePath = null
            )
        {
            this.Name = "载波相位平滑伪距改正观测值";
            this.IsDualIonoFree = isIonoFreeDualFreq;
            this.IsNeedBuffer = IonoDifferCorrectionType.WindowPolyfit == ionoDifferCorrectionType || ionoDifferCorrectionType == IonoDifferCorrectionType.WindowWeightedAverage;
           
            this.PhaseSmoothedRangeBuilderManager = new CarrierSmoothedRangeBuilderManager(
                isUseingApprovedMethod,
                WindowSize, isWeighted, ionoDifferCorrectionType, OrderOfDeltaIonoPolyFit, BufferSize, ionoFitEpochCount, SmoothRangeType)
            {
                IndicatedIonoDeltaFilePath = ionoDeltaFilePath
            };


            if (ionoDifferCorrectionType == IonoDifferCorrectionType.IndicatedFile && System.IO.File.Exists(ionoDeltaFilePath))
            {
                ObjectTableReader reader = new ObjectTableReader(ionoDeltaFilePath);
                this.IonoDeltaTable = reader.Read();
                var interval = IonoDeltaTable.GetInterval<Time>();
                InputedIonoDelta = IonoDeltaTable.GetInteruptableData(new Func<Time, double>(key => key.SecondsOfWeek), interval * 5);
            }

            log.Info("初始化" + this.Name + " 对观测值进行改正！是否双频：" + IsDualIonoFree );
        }

        #region 属性
        /// <summary>
        /// 是否需要缓存，用于拟合电离层变化
        /// </summary>
        public bool IsNeedBuffer { get; set; }
        /// <summary>
        /// 电离层延迟表格。
        /// </summary>
        public ObjectTableStorage IonoDeltaTable { get; set; }
        /// <summary>
        /// 外部输入电离层变化数据，用以改正电离层。
        /// </summary>
        public BaseDictionary<string, InteruptableData<Time>> InputedIonoDelta { get; set; }    
        /// <summary>
        /// 是否双频
        /// </summary>
        public bool IsDualIonoFree { get; set; }
        /// <summary>
        /// 平滑器
        /// </summary>
        public CarrierSmoothedRangeBuilderManager PhaseSmoothedRangeBuilderManager { get; set; }
        #endregion

        /// <summary>
        /// 矫正
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Revise(ref EpochInformation obj)
        {
            if (IsDualIonoFree)
            {
                ReviseDualIonoFree(obj);
                return true;
            }

            //以下是单频
            RevisedEachFrequence(obj);

            return true;
        }

        /// <summary>
        /// 单频逐个改正
        /// </summary>
        /// <param name="obj"></param>
        private void RevisedEachFrequence(EpochInformation obj)
        {
            var buffer = this.Buffers;
            //缓存增加，注意：看起来费时间，如果不需要缓存，则不用增加了。
            if (IsNeedBuffer)
            {
                foreach (var item in buffer)
                {
                    foreach (var sat in item)
                    {
                        foreach (var kv in sat.Data)
                        {
                            var freq = kv.Key;
                            var obsFreq = kv.Value;
                            if (obsFreq.Count < 2) { continue; }//如果没有载波，那还改正个球啊！

                            var PS = PhaseSmoothedRangeBuilderManager.GetOrCreate(BuildSatFreqName(sat, freq));
                            var rawRange = obsFreq.PseudoRange.Value;
                            var rawPhase = obsFreq.PhaseRange.Value;
                            PS.SetBufferValue(item.ReceiverTime, rawRange, rawPhase);
                        }
                    }
                }
            }

            //执行校正
            foreach (var sat in obj)
            {
                foreach (var kv in sat.Data)
                {
                    var freq = kv.Key;
                    var obsFreq = kv.Value;
                    if (obsFreq.Count < 2) { continue; }//如果没有载波，那还改正个球啊！

                    ReviseOneFreq(sat, freq, obsFreq);
                }
            }
        }
        /// <summary>
        /// 校正其中一个。
        /// </summary>
        /// <param name="sat"></param>
        /// <param name="freq"></param>
        /// <param name="obsFreq"></param>
        private void ReviseOneFreq(EpochSatellite sat, FrequenceType freq, FreqenceObservation obsFreq)
        {
            double correction = 0; 
            RmsedNumeral corredRange = RmsedNumeral.Zero;
            var rangeObservation = obsFreq.PseudoRange;
            double rawRange = rangeObservation.Value;
            double rawPhase = obsFreq.PhaseRange.Value;

            var PS = PhaseSmoothedRangeBuilderManager.GetOrCreate(BuildSatFreqName(sat, freq));

            double ionoDiffer = 0;
            switch (this.PhaseSmoothedRangeBuilderManager.IonoDifferCorrectionType)
            {
                case IonoDifferCorrectionType.No:
                    break;
                case IonoDifferCorrectionType.DualFreqCarrier:
                    ionoDiffer = sat.GetIonoLenByDifferPhase(freq);
                    break;
                case IonoDifferCorrectionType.WindowPolyfit:
                    break;
                case IonoDifferCorrectionType.WindowWeightedAverage:
                    break;
                case IonoDifferCorrectionType.IndicatedFile:
                    if (IonoDeltaTable == null)
                    {
                        log.Warn("电离层延迟变化表格为NULL！无法改正");
                    }
                    else
                    {
                        var dat = InputedIonoDelta.Get(sat.Prn.ToString());
                        if (dat != null)
                        {
                            var win = dat.GetNumeralWindowData(sat.ReceiverTime);

                            if (win != null)
                            {
                                ionoDiffer = win.GetPolyFitValue(sat.ReceiverTime, 1, 2).Value;
                            }
                        }
                    }
                    break;
                default:
                    break;
            }

            corredRange = PS.SetReset(sat.IsUnstable)
                                       .SetRawValue(sat.ReceiverTime, rawRange, rawPhase, ionoDiffer)
                                       .Build();
            correction = corredRange.Value - rawRange;

            //规定：原生观测量改正到观测值上【在观测方程的左边，因此符号不变】。 //2018.05.29, czs, int hmx
            CorrectionNames name = CorrectionNames.PhaseSmoothRange;
            switch (freq)
            { 
                case FrequenceType.A:
                    name = CorrectionNames.PhaseSmoothRangeA;
                    break;
                case FrequenceType.B:
                    name = CorrectionNames.PhaseSmoothRangeB;
                    break;
                case FrequenceType.C:
                    name = CorrectionNames.PhaseSmoothRangeC;
                    break;
                default:
                    name = CorrectionNames.PhaseSmoothRange;
                    break;
            }

            rangeObservation.SetCorrection(name, correction);

            sat.StdDevOfRange = corredRange.StdDev;

            //检查更新电离层改正
            if (PS is GnsserWindowedPhaseSmoothedRangeBuilder)
            {

                obsFreq.TempAmbiguityAndIonoLength = ((GnsserWindowedPhaseSmoothedRangeBuilder)PS).SmoothRangeWindow.CurrentIonoAndHalfLambdaLen;
            }
        }

  
        private static string BuildSatFreqName(EpochSatellite sat, FrequenceType freq)
        {
            return sat.Prn + "_" + freq;
        }

        /// <summary>
        /// 双频无电离层组合校正
        /// </summary>
        /// <param name="obj"></param>
        private void ReviseDualIonoFree(EpochInformation obj)
        {
            foreach (var sat in obj)
            {
                double correction = 0;
                double rawRange = 0;
                double rawPhase = 0;
                RmsedNumeral corredRange = RmsedNumeral.Zero;


                var PS = PhaseSmoothedRangeBuilderManager.GetOrCreate(sat.Prn);

                rawRange = sat.Combinations.IonoFreeRange.Value;
                rawPhase = sat.Combinations.IonoFreePhaseRange.Value;
                corredRange = PS.SetReset(sat.IsUnstable)
                                             .SetRawValue(obj.ReceiverTime, rawRange, rawPhase, 0)
                                             .Build();
                correction = corredRange.Value - rawRange;

                //规定：非原生观测量改正到近似值上【在观测方程的右边，因此符号相反】。 //2018.05.29, czs, int hmx
                sat.RangeOnlyCorrection.AddCorrection("IonFreePhaseSmoothRangeOfAB", -1.0 * correction);
                sat.StdDevOfRange = corredRange.StdDev;
            }
        }


    }
}
