//2015.01.07, czs, edigt in namu, 周跳组合探测处理器，采用探测器标记是否具有周跳。
//2016.01.28, czs, edit in hongqing, 周跳探测增加时间判断器，避免重复探测周跳
//2016.11.11, double, edit in zhengzhou, 修改了单频周跳探测器
//2017.08.13, czs, edit in hongiqng, 面向对象重构，参数可配置处理
//2017.09.03, czs, edit in hongqing, 重新设计周跳输出函数，逆序周跳探测取消已有标记的影响

using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using Gnsser.Domain;
using Geo.Algorithm.Adjust;
using Geo.Times;
using Geo;
using Geo.Utils; 
using Geo.Common;

namespace Gnsser
{

    /// <summary>
    /// 周跳组合探测处理器，采用探测器标记是否具有周跳。
    /// 通常探测器只能探测一个测站的周跳，如果有多个探测器，请使用多个对象。
    /// </summary>
    public class CycleSlipDetectReviser : EpochInfoReviser, ICycleSlipDetector
    {
        /// <summary>
        /// 默认构造函数。
        /// </summary>
        public CycleSlipDetectReviser()
        {
            this.Name = "周跳组合探测处理器";
            CycleSlipDetectors = new List<ICycleSlipDetector>();
            this.IsMarkingCycleSlipe = true;
            this.IsRemoveCycleSlipeMarker = false;
            CycleSlipStorage = new InstantValueStorage();
        }
        public bool CurrentResult { get; set; }
        /// <summary>
        /// 周跳探测类型。
        /// </summary>
        public CycleSlipDetectorType DetectorType { get { return CycleSlipDetectorType.数值平均法; } }
        /// <summary>
        /// 是否标记周跳已经发生
        /// </summary>
        public bool IsMarkingCycleSlipe { get; set; }
        /// <summary>
        /// 是否标记周跳没有发生
        /// </summary>
        public bool IsRemoveCycleSlipeMarker { get; set; }


        /// <summary>
        /// 周跳探测结果存储器
        /// </summary>
        public InstantValueStorage CycleSlipStorage { get; set; }
        /// <summary>
        /// 探测器
        /// </summary>
        public List<ICycleSlipDetector> CycleSlipDetectors { get; set; }
        /// <summary>
        /// 最后一次探测时间
        /// </summary>
        public Time LastDetectingTime { get; set; }

        /// <summary>
        /// 添加一个检核器。
        /// </summary>
        /// <param name="checker"></param>
        public void Add(ICycleSlipDetector checker)
        {
            if (!CycleSlipDetectors.Contains(checker))
            { 
                CycleSlipDetectors.Add(checker);
            }
        }
        /// <summary>
        /// 添加检核器。
        /// </summary>
        /// <param name="checker"></param>
        public void Add(ICollection<ICycleSlipDetector> checkers)
        {
            foreach (var checker in checkers)
            {               
               CycleSlipDetectors.Add(checker);
            }
        }
        

        /// <summary>
        /// 是否具有周跳。
        /// </summary>
        /// <param name="epochSat">被检核者</param>
        public bool Detect(EpochSatellite epochSat)
        {
            bool result = false;
            foreach (var item in CycleSlipDetectors)
            { 
                if(item.Detect(epochSat))//即使已经得出结果了，也要让所有的周跳探测器都走一遍，才可以起到探测的作用。
                {
                    result = true;
                }
                else
                {
                    int a = 0;
                }             
            }
            if ( result)
            {
                CycleSlipStorage.Regist(epochSat.Prn.ToString(), epochSat.Time.Value);
            }
            this.CurrentResult = result;
            return result;
        }

        /// <summary>
        /// 处理
        /// </summary>
        /// <param name="epochInfo"></param>
        /// <returns></returns>
        public override bool Revise(ref EpochInformation epochInfo)
        {
            //判断历元，避免重复探测而引起探测失败
            if (epochInfo.ReceiverTime == LastDetectingTime)
            {
                log.Debug("此历元周跳已经探测过了" + LastDetectingTime);
                return true;
            }
             
            LastDetectingTime = epochInfo.ReceiverTime;  
            foreach (var sat in epochInfo)
            {
                var result = Detect(sat);


                if(result && IsMarkingCycleSlipe){
                    MarkCycleSlip(sat, result);
                }

                if (!result && IsRemoveCycleSlipeMarker)
                {
                    MarkCycleSlip(sat, result);
                } 
            } 

            return true;
        }

        /// <summary>
        /// 写周跳到文件
        /// </summary>
        /// <param name="outputDirectory"></param>
        /// <param name="soureName"></param>
        /// <param name="ObsPhaseDataType"></param>
        public void WriteStorageToFile(string outputDirectory, string soureName, SatObsDataType ObsPhaseDataType)
        {
            var OutputDirectory = Path.Combine(outputDirectory, "CycleSlipOf_" + soureName);
            //先写汇总
            Geo.Utils.FileUtil.CheckOrCreateDirectory(OutputDirectory); 
            var path = Path.Combine(OutputDirectory, BuildFileName(soureName, this.DetectorType, ObsPhaseDataType));
            this.CycleSlipStorage.WriteToFile(path);

            //再写各个分结果
            foreach (var itemReviser in this.CycleSlipDetectors)
            {
                //生成子目录
                var directory = Path.Combine(OutputDirectory, itemReviser.DetectorType.ToString());
                Geo.Utils.FileUtil.CheckOrCreateDirectory(directory); 

                var subName = BuildFileName(soureName, itemReviser.DetectorType, ObsPhaseDataType);
                var subPath = Path.Combine(directory, subName);
                itemReviser.CycleSlipStorage.WriteToFile(subPath);

                itemReviser.TableObjectManager.OutputDirectory = directory;
                itemReviser.TableObjectManager.WriteAllToFileAndCloseStream(); 
            }
        }

        /// <summary>
        /// 构造文件名称
        /// </summary>
        /// <returns></returns>
        public string BuildFileName(string ObsFileName, CycleSlipDetectorType DetectType, SatObsDataType SatObsDataType)
        {
            var timeMark = DateTime.Now.Ticks.ToString();
            StringBuilder sb = new StringBuilder();
            sb.Append(ObsFileName);
            //sb.Append("_");
            //sb.Append(timeMark.Substring(timeMark.Length / 2));//避免相同文件文件的时间冲突。
            sb.Append("_");
            sb.Append(DetectType.ToString());
            sb.Append("_");
            sb.Append(SatObsDataType.ToString());
            sb.Append(".CycleSlip");
            return sb.ToString();
        }

        /// <summary>
        /// 标记周跳发生或者没有发生
        /// </summary>
        /// <param name="sat"></param>
        /// <param name="trueOrFalse"></param>
        private static void MarkCycleSlip(EpochSatellite sat, bool trueOrFalse)
        {
            sat.IsUnstable = trueOrFalse;
            sat.SetCycleSlip(sat.IsUnstable);
        }

        /// <summary>
        /// GNSSer 默认周跳探测器。
        /// </summary>
        /// <param name="Option">GNSS数据处理选项</param>
        /// <param name="isRevered">是否逆序探测，如果是，探测时将不会使用已有周跳信息</param>
        /// <returns></returns>
        public static CycleSlipDetectReviser DefaultDoubleFrequencyDetector(GnssProcessOption Option, bool isRevered = false)
        {
            var isUseRecoredCsInfo = !isRevered && Option.IsUsingRecordedCycleSlipInfo;
            //if (Option == null) { Option = new GnssProcessOption(); }
            CycleSlipDetectReviser reviser = new CycleSlipDetectReviser();

            //reviser.Add(new HigherDifferCycleSlipRemoveor(source, PositionOption.SatelliteTypes));
            reviser.Add(new LiCycleSlipDetector(Option.MaxBreakingEpochCount, isUseRecoredCsInfo));
            reviser.Add(new MwCycleSlipDetector(Option.MaxBreakingEpochCount, Option.MaxDifferValueOfMwCs, isUseRecoredCsInfo));
            return reviser;
        }
        public static CycleSlipDetectReviser DefaultDoubleFrequencyDetector()
        { 
            CycleSlipDetectReviser reviser = new CycleSlipDetectReviser();

            //reviser.Add(new HigherDifferCycleSlipRemoveor(source, PositionOption.SatelliteTypes));
            reviser.Add(new LiCycleSlipDetector(5, false));
            reviser.Add(new MwCycleSlipDetector(5, 8.6, false));
            return reviser;
        }

        /// <summary>
        /// GNSSer 默认周跳探测器。
        /// </summary>
        /// <param name="isRevered">是否逆序探测，如果是，探测时将不会使用已有周跳信息</param>
        /// <returns></returns>
        public static CycleSlipDetectReviser DefaultTripleFrequencyDetector(bool isRevered = false)
        {
           // var isUseRecoredCsInfo = !isRevered && Option.IsUsingRecordedCycleSlipInfo;
            CycleSlipDetectReviser reviser = new CycleSlipDetectReviser();

            //reviser.Add(new TriFreqMwCycleSlipDetector());
            reviser.Add(new TriFreqGF1BaseOnPhaseCycleSlipDetector());
            reviser.Add(new TriFreqGF2BaseOnPhaseCycleSlipDetector());
            return reviser;
        }

        /// <summary>
        /// 单频 默认周跳探测器
        /// </summary>
        /// <param name="Option">GNSS数据处理选项</param>
        /// <param name="isRevered">是否逆序探测，如果是，探测时将不会使用已有周跳信息</param>
        /// <returns></returns>
        public static CycleSlipDetectReviser DefaultSingeFrequencyDetector(GnssProcessOption Option, bool isRevered = false)
        {
            var isUseRecoredCsInfo = !isRevered && Option.IsUsingRecordedCycleSlipInfo;
            CycleSlipDetectReviser reviser = new CycleSlipDetectReviser();
            if (Option.BufferSize < 200)
            {
                reviser.Add(new HighDifferSlipDetector(Option) { IsUsingRecordedCsInfo = isUseRecoredCsInfo });
                reviser.Add(new AverageDetector(Option) { IsUsingRecordedCsInfo = isUseRecoredCsInfo });

               // reviser.Add(new LsPolyFitDetector(Option) { IsUsingRecordedCsInfo = isUseRecoredCsInfo });

                reviser.Add(new GreyPolyFitDetector(Option) { IsUsingRecordedCsInfo = isUseRecoredCsInfo });
            }
            return reviser;
        }

       


        /// <summary>
        /// 创建周跳探测的矫正器
        /// </summary>
        /// <param name="obsType"></param>
        /// <param name="types"></param>
        /// <param name="Option">GNSS数据处理选项</param>
        /// <param name="isRevered">是否逆序探测，如果是，探测时将不会使用已有周跳信息</param>
        /// <returns></returns>
        public static CycleSlipDetectReviser CreateCycleSlipReviser(SatObsDataType obsType, List<CycleSlipDetectorType> types, GnssProcessOption Option, bool isRevered = false)
        {
            bool isUseRecorded = !isRevered && Option.IsUsingRecordedCycleSlipInfo;
            CycleSlipDetectReviser reviser = new CycleSlipDetectReviser();
            foreach (var type in types)
            {
                reviser.Add(Create(obsType, type, Option, isRevered));
            }
            return reviser;
        }
        /// <summary>
        /// 周跳探测器建立
        /// </summary>
        /// <param name="type"></param>
        /// <param name="Option"></param>
        /// <param name="isRevered"></param>
        /// <returns></returns>
        public static ICycleSlipDetector Create(CycleSlipDetectorType type, GnssProcessOption Option, bool isRevered = false)
        {
            return Create(Option.ObsDataType, type, Option, isRevered);
        }
        /// <summary>
        /// 周跳探测器建立
        /// </summary>
        /// <param name="obsType"></param>
        /// <param name="type"></param>
        /// <param name="Option"></param>
        /// <param name="isRevered"></param>
        /// <returns></returns>
        public static ICycleSlipDetector Create(SatObsDataType obsType, CycleSlipDetectorType type, GnssProcessOption Option, bool isRevered = false)
        {
            bool isUseRecorded = !isRevered && Option.IsUsingRecordedCycleSlipInfo;
            switch (type)
            {
                case CycleSlipDetectorType.高次差法:
                    return (new HighDifferSlipDetector(Option) { IsUsingRecordedCsInfo = isUseRecorded });
                    break;
                case CycleSlipDetectorType.LI组合:
                    return (new LiCycleSlipDetector(Option) { IsUsingRecordedCsInfo = isUseRecorded });
                    break;
                case CycleSlipDetectorType.多项式拟合法:
                    return (new LsPolyFitDetector(Option) { IsUsingRecordedCsInfo = isUseRecorded });
                    break;
                case CycleSlipDetectorType.数值平均法:
                    return (new AverageDetector(Option) { IsUsingRecordedCsInfo = isUseRecorded });
                    break;
                case CycleSlipDetectorType.灰色模型法:
                    return (new GreyPolyFitDetector(Option) { IsUsingRecordedCsInfo = isUseRecorded });
                    break;
                case CycleSlipDetectorType.MW组合:
                    return (new MwCycleSlipDetector(Option) { IsUsingRecordedCsInfo = isUseRecorded });
                    break;
                case CycleSlipDetectorType.首次出现标记法:
                    return (new TimeValueCycleSlipDetector(Option) { IsUsingRecordedCsInfo = isUseRecorded });
                    break;
                default:
                    throw new NotSupportedException("尚未实现！" + type);
            }
        }

        #region override interface
        /// <summary>
        /// 当前观测的卫星信息
        /// </summary>
        public EpochSatellite EpochSat { get; set; }
        /// <summary>
        /// 是否保存到表
        /// </summary>
        public bool IsSaveResultToTable { get; set; }
        /// <summary>
        /// 是否使用已有周跳信息
        /// </summary>
        public bool IsUsingRecordedCsInfo { get; set; }
        /// <summary>
        /// 表存储器
        /// </summary>
        public ObjectTableManager TableObjectManager { get; set; }
        #endregion
    }
}
