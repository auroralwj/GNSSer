//2018.06.07, czs,  create in HMX, 单频电离层自我改正

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
using Gnsser;
using Geo.Correction;

namespace Gnsser.Correction
{

    /// <summary>
    /// 单频电离层自我改正
    /// </summary>
    public class SingleFreqSelfIonoRangeReviser : EpochInfoReviser
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="processOption"></param>
        public SingleFreqSelfIonoRangeReviser(GnssProcessOption processOption)
            : this(processOption.IsUseGNSSerSmoothRangeMethod,
                  processOption.ObsDataType,
                  processOption.WindowSizeOfPhaseSmoothRange,
                  processOption.IsWeightedPhaseSmoothRange,
                  SatObsDataTypeHelper.GetFrequenceTypeFromObsDataType(processOption.ObsDataType), 
                  processOption.IonoDifferCorrectionType,
                   processOption.OrderOfDeltaIonoPolyFit,processOption.BufferSize, processOption.IonoFitEpochCount,processOption.SmoothRangeSuperPosType,
                   processOption.IonoDeltaFilePath
                  )
        {
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="IsUseGNSSerSmoothRangeMethod"></param>
        /// <param name="ObsDataType"></param>
        /// <param name="WindowSize"></param>
        /// <param name="isWeighted"></param>
        /// <param name="FrequenceType"></param>
        /// <param name="isDeltaIonoFit"></param>
        /// <param name="OrderOfDeltaIonoPolyFit"></param>
        /// <param name="BufferSize"></param>
        /// <param name="ionoFitEpochCount"></param>
        /// <param name="SmoothRangeType"></param>
        /// <param name="IndicatedIonoDeltaFilePath"></param>
        public SingleFreqSelfIonoRangeReviser(bool  IsUseGNSSerSmoothRangeMethod,
            SatObsDataType ObsDataType,
            int WindowSize,
            bool isWeighted,
            FrequenceType FrequenceType,
            IonoDifferCorrectionType isDeltaIonoFit,
            int OrderOfDeltaIonoPolyFit, int BufferSize, int ionoFitEpochCount, SmoothRangeSuperpositionType SmoothRangeType, string IndicatedIonoDeltaFilePath)
            : this(IsUseGNSSerSmoothRangeMethod, 
                  ObsDataType, 
                  WindowSize, isWeighted,
                  new List<FrequenceType>() { FrequenceType },
                  isDeltaIonoFit,
                  OrderOfDeltaIonoPolyFit, BufferSize, ionoFitEpochCount, SmoothRangeType, IndicatedIonoDeltaFilePath)
        {

        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="IsUseGNSSerSmoothRangeMethod"></param>
        /// <param name="ObsDataType"></param>
        /// <param name="WindowSize"></param>
        /// <param name="isWeighted"></param>
        /// <param name="FrequenceTypes"></param>
        /// <param name="isDeltaIonoFit"></param>
        /// <param name="OrderOfDeltaIonoPolyFit"></param>
        /// <param name="BufferSize"></param>
        /// <param name="ionoFitEpochCount"></param>
        /// <param name="SmoothRangeType"></param>
        /// <param name="IndicatedIonoDeltaFilePath"></param>
        public SingleFreqSelfIonoRangeReviser(
            bool  IsUseGNSSerSmoothRangeMethod,
            SatObsDataType ObsDataType, 
            int WindowSize,
            bool isWeighted,
            List<FrequenceType> FrequenceTypes,
            IonoDifferCorrectionType isDeltaIonoFit,
            int OrderOfDeltaIonoPolyFit,int BufferSize, int ionoFitEpochCount, SmoothRangeSuperpositionType SmoothRangeType, string IndicatedIonoDeltaFilePath)
        {
            this.Name = "单频电离层自我改正";
            this.ObsDataType = ObsDataType;
            this.FrequenceTypes = FrequenceTypes;
            this.PhaseSmoothedRangeBuilderManager = new CarrierSmoothedRangeBuilderManager(
                IsUseGNSSerSmoothRangeMethod, WindowSize, isWeighted, isDeltaIonoFit, OrderOfDeltaIonoPolyFit, BufferSize, ionoFitEpochCount, SmoothRangeType)
            {
                IndicatedIonoDeltaFilePath = IndicatedIonoDeltaFilePath
            };

            this.IsDualIonoFree = this.ObsDataType == SatObsDataType.IonoFreePhaseRange
                || this.ObsDataType == SatObsDataType.IonoFreeRange;
            this.Data = new BaseDictionary<SatelliteNumber, TimeNumeralWindowData>("数据",
                new Func<SatelliteNumber, TimeNumeralWindowData>(prn=> new TimeNumeralWindowData(WindowSize)                 
                ));

            TableObjectStorages = new ObjectTableManager();
            TableObjectStorage = TableObjectStorages.AddTable("模糊度概略距离值");

            log.Info("启用：" + this.Name + " 对观测值进行改正！是否双频：" + IsDualIonoFree);
        } 
        /// <summary>
        /// 观测值类型
        /// </summary>
        public SatObsDataType ObsDataType { get; set; }
        /// <summary>
        /// 是否双频
        /// </summary>
        public bool IsDualIonoFree { get; set; }
        /// <summary>
        /// 单频改正的类型
        /// </summary>
        public List<FrequenceType> FrequenceTypes { get; set; }
        /// <summary>
        /// 平滑器
        /// </summary>
        public CarrierSmoothedRangeBuilderManager PhaseSmoothedRangeBuilderManager { get; set; }

        public BaseDictionary<SatelliteNumber, TimeNumeralWindowData> Data { get; set; }
        ObjectTableManager TableObjectStorages { get; set; }
        ObjectTableStorage TableObjectStorage { get; set; }
        /// <summary>
        /// 上一个材料
        /// </summary>
        public EpochInformation PrevMaterial { get; set; }

        /// <summary>
        /// 矫正
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Revise(ref EpochInformation obj)
        {
            var buffer = this.Buffers;


            //根据接收机坐标和已计算的参数，估计模糊度值。


            TableObjectStorage.NewRow();
            TableObjectStorage.AddItem("Epoch", obj.ReceiverTime);

            //添加到往期数据
            foreach (var sat in obj)
            {
                var prn = sat.Prn;
                var windowData = this.Data.GetOrCreate(prn);


                foreach (var frequencyType in FrequenceTypes)
                {  
                    var freqValue = sat.Get(frequencyType);
                    double rawPseudoRange = freqValue.PseudoRange.CorrectedValue;//？？是否改用平滑后的伪距？？应该包含在内了。
                    double rawPhaseRange = freqValue.PhaseRange.CorrectedValue;
                    double approxRange = sat.GetApproxPseudoRange(frequencyType).CorrectedValue;

                    double timeCorrectionDistance = sat.Time.Correction * GnssConst.LIGHT_SPEED; //接收机钟差改正
                    //如果接收机钟差为0，则表示无法计算，则模糊度采用上一历元结果，或不改正电离层

                    if (timeCorrectionDistance != 0)
                    {
                        double ionoAlready = freqValue.GetCommonRangeCorrection().GetCorrection(ParamNames.Iono);
                        double approxOkRange = approxRange - timeCorrectionDistance - ionoAlready; //公式推导为加钟差，这里为改正数，需要取相反数
                                                                                                   //这样是没有电离层影响。，
                        double ambiguityRange = 2 * approxOkRange - (rawPseudoRange + rawPhaseRange);

                        freqValue.TempAmbiguity = ambiguityRange;

                        windowData.Add(sat.ReceiverTime, freqValue.TempAmbiguity);

                    }
                    else   if (PrevMaterial != null  )// 采用上一期的计算结果，如果有的话。且模糊度的变化不应很大（除了周跳），因此这里应该加权平均！！。
                    {

                       // double ambiguityRange = 2 * approxOkRange - (rawPseudoRange + rawPhaseRange);
                        var prevSat =  PrevMaterial.Get(sat.Prn);
                        if(prevSat == null) { continue; }

                        var freqVal = prevSat.Get(frequencyType);
                        freqValue.TempAmbiguity = freqVal.TempAmbiguity; 
                    } 

                    if (freqValue.TempAmbiguityAndIonoLength != 0)
                    {
                        double slopeDelayFreq = freqValue.TempIonoLength;
                        int ii = 0; 
                        freqValue.RangeOnlyCorrection.SetCorrection(ParamNames.Iono, slopeDelayFreq);
                        freqValue.PhaseOnlyCorrection.SetCorrection(ParamNames.Iono, -1.0 * slopeDelayFreq);//码延迟，波超前 
                    }


                    TableObjectStorage.AddItem(prn + "_" + frequencyType + "RawAmbiLen", freqValue.TempAmbiguity);
                    //TableObjectStorage.AddItem(prn + "_" + "AveAmbiLen", windowData.Average.Value);
                    //TableObjectStorage.AddItem(prn + "_" + "SmAmbiLen", windowData.GetLsPolyFitValue(sat.ReceiverTime, 1).Value);

                }


            }

            if (PrevMaterial == null || PrevMaterial.ReceiverTime != obj.ReceiverTime)
            {
                PrevMaterial = obj;
            }
            return true;
        }

        public override void Complete()
        {
            TableObjectStorages.WriteAllToFileAndClearBuffer();
            base.Complete();
        }

    }
}
