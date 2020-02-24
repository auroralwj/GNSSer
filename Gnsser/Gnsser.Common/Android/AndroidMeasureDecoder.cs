// 2017/8/1,czs, create in hongqing, new class.
//2017.08.02, czs, create in hongqing,  Android 测量数据解码器
//2017.08.10, czs, edit in hongqing, 伪距基本调通

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Geo;
using Geo.Times;
using Gnsser.Times;
using Geo.IO;
using System.IO;
using Gnsser.Data.Rinex;
using Gnsser.Data;

namespace Gnsser
{
    /// <summary>
    /// Android 测量数据解码器。参数选项
    /// </summary>
    public class AndroidMeasureDecoderOption
    {
        /// <summary>
        /// 选项
        /// </summary>
        public AndroidMeasureDecoderOption()
        {
            IsFromFirstEpoch = false;
            IsToCylePhase = true;
            IsSkipZeroPseudorange = true;
            IsSkipZeroPhase = true;
            IsAligningPhase = true;
        }
        #region  参数选项
        /// <summary>
        /// 是否从从第一历元开始计算,与常规不同，伪距将逐历元增加。否则为逐历元计算，后者载波可能无法拼接成功！！
        /// </summary>
        public bool IsFromFirstEpoch { get; set; }

        /// <summary>
        /// 是否转换相位值为周
        /// </summary>
        public bool IsToCylePhase { get; set; }

        /// <summary>
        /// 是否移除 0 值伪距。
        /// </summary>
        public bool IsSkipZeroPseudorange { get; set; }
        /// <summary>
        /// 是否移除 0 值载波。
        /// </summary>
        public bool IsSkipZeroPhase { get; set; }
        /// <summary>
        /// 是否对齐初始相位
        /// </summary>
        public bool IsAligningPhase { get; set; }

        #endregion
    }

     /// <summary>
     /// Android 测量数据解码器。
     /// 注意：此处解码后的RINEX文件与常规文件不同，伪距范围增量，载波单位为米。
     /// </summary>
    public class AndroidMeasureDecoder :AbstractProcess<string> , ICancelAbale
    {
        Log log = new Log(typeof(AndroidMeasureDecoder));

        /// <summary>
        /// Android 测量数据解码器
        /// </summary>
        /// <param name="gnssLogerRawTable"></param>
        /// <param name="Option">参数选项</param> 
        public AndroidMeasureDecoder(ObjectTableStorage gnssLogerRawTable, AndroidMeasureDecoderOption  Option = null)
        {
            this.RawTable = gnssLogerRawTable;
            if (Option == null) { Option = new AndroidMeasureDecoderOption(); }
            this.Option = Option; 

            NumericalAlignerManager = new NumericalAlignerManager<SatelliteNumber, Time>(3, m => m.SecondsOfWeek);
            this.IsCancel = false; 
        }

        #region  属性 
        /// <summary>
        /// 记录处理了一个。
        /// </summary>
        public event Action<int> RecordProcessed;
        /// <summary>
        /// 是否取消
        /// </summary>
        public bool IsCancel { get; set; }
        #region  参数选项

        AndroidMeasureDecoderOption Option { get; set; }
        #endregion
        /// <summary>
        /// 输入数据表
        /// </summary>
        public ObjectTableStorage RawTable { get; set; }
        /// <summary>
        /// 载波对齐器
        /// </summary>
        NumericalAlignerManager<SatelliteNumber, Time> NumericalAlignerManager { get; set; }

        #region 表列常量
        const String TimeNanos = "TimeNanos";
        const String FullBiasNanos = "FullBiasNanos";
        const String BiasNanos = "BiasNanos";
        const String Svid = "Svid";
        const String TimeOffsetNanos = "TimeOffsetNanos";
        const String ReceivedSvTimeNanos = "ReceivedSvTimeNanos";
        const String ReceivedSvTimeUncertaintyNanos = "ReceivedSvTimeUncertaintyNanos";
        const String PseudorangeRateMetersPerSecond = "PseudorangeRateMetersPerSecond";
        const String PseudorangeRateUncertaintyMetersPerSecond = "PseudorangeRateUncertaintyMetersPerSecond";

        const String Cn0DbHz = "Cn0DbHz";
        const String ConstellationType = "ConstellationType";
        const String AccumulatedDeltaRangeMeters = "AccumulatedDeltaRangeMeters";
        const String HardwareClockDiscontinuityCount = "HardwareClockDiscontinuityCount";
        const String AccumulatedDeltaRangeState = "AccumulatedDeltaRangeState";
        #endregion

        #region 转换内部属性
        /// <summary>
        /// 当前历元观测信息
        /// </summary>
        RinexEpochObservation CurrentEpochInfo { get; set; }
        /// <summary>
        /// 当前文件
        /// </summary>
        RinexObsFile RinexOFile { get; set; }
        /// <summary>
        /// 时间解算器
        /// </summary>
        AndroidGnssTimeConverter TimeConverter { get; set; }
        /// <summary>
        /// 当前时间标识。
        /// </summary>
        long _currentTimeNanos { get; set; }
        /// <summary>
        /// 上一个硬件钟。
        /// </summary>
        int _prevHardwareClockDisCount { get; set; }
        #endregion
        #endregion

        /// <summary>
        /// 转换输出为RINEX文件。
        /// </summary>
        /// <param name="outOPath"></param>
        public override void Run(string outOPath)
        {
            RinexObsFile oFile = Convert();
            OutputRinexFile(outOPath, oFile);
        }

        /// <summary>
        /// 转换
        /// </summary>
        /// <returns></returns>
        private RinexObsFile Convert()
        {
            InitProcess(RawTable.RowCount); 

            TimeConverter = null;
            CurrentEpochInfo = null;
            RinexOFile = CreateRinexObsFile(); 
            int index = 0;//只有有效的观测历元才开始计数
             foreach (var item in RawTable.BufferedValues)
            {
                if (IsCancel) { break; }

                if (RecordProcessed != null) { RecordProcessed(index); }

                RinexSatObsData satData = ParseOneRecord(item);
                if (satData != null) {
                    CurrentEpochInfo.Add(satData);
                }              
                index++;
                PerformProcessStep();
            }
             FullProcess(); 

            return RinexOFile;
        }

        /// <summary>
        /// 解析一行数据。
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private RinexSatObsData ParseOneRecord(Dictionary<string, object> item)
        {
            RinexSatObsData satData = null;
            #region 赋值
            long timeNanos = ParseLong(item, TimeNanos);
            //当前时间到GPS起始时间的纳秒数，为负数。
            long fullBiasNanos = ParseLong(item, FullBiasNanos);
            double biasNanos = ParseDouble(item, BiasNanos);
            double timeOffsetNanos = ParseDouble(item, TimeOffsetNanos);
            long receivedSvTimeNanos = ParseLong(item, ReceivedSvTimeNanos);
            long receivedSvTimeUncertaintyNanos = ParseLong(item, ReceivedSvTimeUncertaintyNanos);
            int hardwareClockDiscontinuityCount = ParseInt(item, HardwareClockDiscontinuityCount);
            int accumulatedDeltaRangeState = ParseInt(item, AccumulatedDeltaRangeState);
            double cn0DbHz = ParseDouble(item, Cn0DbHz);
            double accumulatedDeltaRangeMeters = ParseDouble(item, AccumulatedDeltaRangeMeters);
            #endregion

            SatelliteNumber prn = ParsePrn(item);

            //伪距不确定度太大，本数据作废
            if (receivedSvTimeUncertaintyNanos > 1e8)
            {
                log.Debug(prn + " 卫星时间不确定度太大，忽略本数据 " + receivedSvTimeUncertaintyNanos);
                return satData;
            }//简单判断伪距
            //如果钟跳发生了，或指定的被减伪距值无效，则重置被减伪距
            bool isClockDisCountChanged = (_prevHardwareClockDisCount != hardwareClockDiscontinuityCount);
            _prevHardwareClockDisCount = hardwareClockDiscontinuityCount;

            //决定是否新建时间计算器。
            if (TimeConverter == null || !TimeConverter.IsEpochValid)
            {
                TimeConverter = new AndroidGnssTimeConverter(fullBiasNanos, Option.IsFromFirstEpoch);//fullBiasNanos值并不准确，含有偏差，不能用于计算所有伪距。 
                if (!TimeConverter.IsEpochValid)
                {
                    log.Debug("当前历元时间不合法，" + TimeConverter.FirstEpochOfUtc + ",将重试。");
                    return satData;
                }
            }
            TimeConverter.SetFullBias(fullBiasNanos)
                .SetTimeNanos(timeNanos)
                .SeBiasNanos(biasNanos)
                .SetTimeOffsetNanos(timeOffsetNanos);
            Time currentUtcTime = TimeConverter.GetReceiverUtcTime();

            TimeConverter.SetSatelliteType(prn.SatelliteType);

            //若是新历元
            if (IsNewEpoch(CurrentEpochInfo, _currentTimeNanos, timeNanos)) //新历元
            {
                _currentTimeNanos = timeNanos;//记录当接收机时间，用于判断是否是同一个历元

                CurrentEpochInfo = new RinexEpochObservation(currentUtcTime);
                RinexOFile.Add(CurrentEpochInfo);
            }
            //计算伪距
            double transTime = TimeConverter.GetTransmissionTime(receivedSvTimeNanos);
            double C1 = transTime * GnssConst.LIGHT_SPEED;

            bool isPassed = CheckPsuedorange(ref prn, C1);
            if (!isPassed) { C1 = 0; }

            if (C1 == 0 && Option.IsSkipZeroPseudorange)
            {
                log.Debug(currentUtcTime + ", " + prn + "伪距为0，忽略本数据 ");
                return satData;
            }
            if (accumulatedDeltaRangeMeters == 0 && Option.IsSkipZeroPhase)
            {
                log.Debug(currentUtcTime + ", " + prn + "载波为 0，忽略本数据 ");
                return satData;
            }

            //时间积累差，带来的近距离变化差
            double timeErrorDistance = TimeConverter.GetDifferDistance();
            var phaseRange = accumulatedDeltaRangeMeters - timeErrorDistance;//相位距离

            var aligner = NumericalAlignerManager.GetOrCreate(prn);

            if (isClockDisCountChanged) //如果钟跳发生，则将第一个差分伪距设置为当前伪距值。
            {
                aligner.IsReset = true;
            }

            phaseRange = aligner.GetAlignedValue(currentUtcTime, phaseRange, C1);
            double L1 = GetL1(phaseRange, prn, Option.IsToCylePhase);
            double S1 = cn0DbHz;
            double D1 = phaseRange; //test for range

            satData = BuildRinexSatObsData(prn, C1, L1, S1, D1);

            return satData;
        }

        #region 解析数字
        /// <summary>
        /// 解析 double
        /// </summary>
        /// <param name="dic">字典，行</param>
        /// <param name="key">关键字</param>
        /// <param name="defaultVal">默认值</param>
        /// <returns></returns>
        private static double ParseDouble(Dictionary<string, object> dic, string key, double defaultVal = 0)
        {
            double biasNanos = defaultVal;
            if (dic.ContainsKey(key))
            {
                biasNanos = double.Parse(dic[key].ToString());
            }
            return biasNanos;
        }
        /// <summary>
        /// 解析 int
        /// </summary>
        /// <param name="dic">字典，行</param>
        /// <param name="key">关键字</param>
        /// <param name="defaultVal">默认值</param>
        /// <returns></returns>
        private static int ParseInt(Dictionary<string, object> dic, string key, int defaultVal = 0)
        {
            int biasNanos = defaultVal;
            if (dic.ContainsKey(key))
            {
                biasNanos = int.Parse(dic[key].ToString());
            }
            return biasNanos;
        }
        /// <summary>
        /// 解析 long
        /// </summary>
        /// <param name="dic">字典，行</param>
        /// <param name="key">关键字</param>
        /// <param name="defaultVal">默认值</param>
        /// <returns></returns>
        private static long ParseLong(Dictionary<string, object> dic, string key, long defaultVal = 0L)
        {
            long biasNanos = defaultVal;
            if (dic.ContainsKey(key))
            {
                var val = dic[key].ToString();
                if(val.Contains("E"))
                {
                    biasNanos = (long)Double.Parse(val);
                }
                else
                {
                    biasNanos = long.Parse(val); 
                }
            }
            return biasNanos;
        }
        #endregion

        /// <summary>
        /// 解析PRN
        /// </summary>
        /// <param name="dic"></param> 
        /// <returns></returns>
        private static SatelliteNumber ParsePrn(Dictionary<string, object> dic)
        {
            int svid = ParseInt(dic, Svid);
            int constellationType = ParseInt(dic, ConstellationType,1);  
            SatelliteNumber prn = AnGnssUtil.getSatelliteNumbere(constellationType, svid);
            return prn;
        }

        /// <summary>
        /// 创建RINEX观测数据
        /// </summary>
        /// <param name="prn"></param>
        /// <param name="C1"></param>
        /// <param name="D1"></param>
        /// <param name="L1"></param>
        /// <param name="S1"></param>
        /// <returns></returns>
        private static RinexSatObsData BuildRinexSatObsData(SatelliteNumber prn, double C1, double L1, double S1,double D1)
        {
            RinexSatObsData satData = new RinexSatObsData(prn);
            satData.Add("C1", C1);
            satData.Add("L1", L1);
            satData.Add("S1", S1);
            satData.Add("D1", D1);//存储为距离递增量，方便调试
            return satData;
        }

        /// <summary>
        /// 创建RINEX观测文件
        /// </summary>
        /// <returns></returns>
        private static RinexObsFile CreateRinexObsFile()
        {
            RinexObsFile oFile = new RinexObsFile();
            oFile.Header = new RinexObsFileHeader() { Version = 2.11 };
            oFile.Header.ObsCodes = new Dictionary<SatelliteType, List<string>>();
            var codes = new List<String>() { "C1", "L1", "S1", "D1" };
            oFile.Header.ObsCodes[SatelliteType.M] = codes;
            oFile.Header.ObsCodes[SatelliteType.G] = codes;
            oFile.Header.ObsCodes[SatelliteType.R] = codes;
            oFile.Header.ObsCodes[SatelliteType.C] = codes;
            oFile.Header.MarkerName = "AndroidPhone";
            return oFile;
        }

        /// <summary>
        /// 判断是否是新历元
        /// </summary>
        /// <param name="currentEpochInfo"></param>
        /// <param name="currentTimeNanos"></param>
        /// <param name="timeNanos"></param>
        /// <returns></returns>
        private  bool IsNewEpoch(RinexEpochObservation currentEpochInfo, long currentTimeNanos, long timeNanos)
        {
            return currentTimeNanos != timeNanos || currentEpochInfo == null;
        } 

        /// <summary>
        /// 载波计算
        /// </summary>
        /// <param name="accumulatedDeltaRangeMeters"></param>
        /// <param name="prn"></param>
        /// <param name="IsToCylePhase"></param>
        /// <returns></returns>
        private static double GetL1(double accumulatedDeltaRangeMeters, SatelliteNumber prn, bool IsToCylePhase)
        {
            //计算历元伪距增量 
            double k = 1;// -1.0;//  k  ADR
            double L1 = k * accumulatedDeltaRangeMeters; //??????此处应该计算为周？？？？？？
             
            if (IsToCylePhase)
            {
                switch (prn.SatelliteType)
                {
                    case SatelliteType.G:
                        L1 /= Frequence.GpsL1.WaveLength;
                        break;
                    case SatelliteType.C:
                        L1 /= Frequence.CompassB1.WaveLength;
                        break;
                    default:
                        L1 /= Frequence.GpsL1.WaveLength;
                        break;
                }
            }
            return L1;
        }

        /// <summary>
        /// 检查伪距范围
        /// </summary>
        /// <param name="prn"></param>
        /// <param name="pseudorange"></param>
        /// <returns></returns>
        private bool CheckPsuedorange(ref SatelliteNumber prn, double pseudorange)
        {
            bool isPassed = true;
            if (pseudorange > 9999999999 || pseudorange < -999999999)//RINEX 最大范围
            {
                isPassed = false;
                log.Warn("out of range: " + pseudorange + " m");
            }//若为递增，则不判断
            else if (Option. IsFromFirstEpoch && prn.SatelliteType != SatelliteType.C && (pseudorange > 2.6e7 || pseudorange < 1.8e7))
            {
                isPassed = false;
                log.Warn("out of range: " + pseudorange + " m");
            }
            return isPassed;
        }

        /// <summary>
        /// 输出
        /// </summary>
        /// <param name="outOPath"></param>
        /// <param name="oFile"></param>
        private void OutputRinexFile(string outOPath, RinexObsFile oFile)
        {
            if (!outOPath.Last().ToString().Equals("o", StringComparison.CurrentCultureIgnoreCase))
            {
                outOPath += "." + oFile.StartTime.SubYear + "o";
            }

            RinexObsFileWriter writer = new RinexObsFileWriter(outOPath);
            writer.Write(oFile);
            writer.Flush();
            writer.Dispose(); 
        }

         

   //private double Calculatepesudorange(GnssMeasurement mGnssMeasurement, GnssClock mGnssClock){
   //     double Pesudorange;
   //     int GPSWeek;
   //     GPSWeek=(int)Math.Floor(-mGnssClock.getFullBiasNanos()/1000000000/ Consts.SecondsOfWeek);
   //     long GPSWeekNanos = (long)GPSWeek*GnssConsts.SecondsOfWeek*1000000000;
   //     double tRxNanos=mGnssClock.getTimeNanos()-mGnssClock.getFullBiasNanos();
   //     if(tRxNanos<0)System.out.println("tRxNanos should be >=0");
   //     double tRxSec=(tRxNanos-mGnssMeasurement.getTimeOffsetNanos()-mGnssClock.getBiasNanos())/1000000000;
   //     double tTxSec=(double) mGnssMeasurement.getReceivedSvTimeNanos()/1000000000;
   //     //Log.e(TAG,""+tTxSec);
   //     double prSec=CheckGpsWeekRollover(tRxSec,tTxSec);
   //     Pesudorange=prSec*Consts.LIGHT_SPEED;
   //     return Pesudorange;
   // }

   //     private double CheckGpsWeekRollover(double tRxSec, double tTxSec)
   //     {
   //         double prSec = tRxSec - tTxSec;
   //         boolean isRollover;
   //         isRollover = prSec > Consts.SecondsOfWeek / 2;
   //         if (isRollover)
   //         {
   //             double prS = prSec;
   //             double delS = Math.round(prS / Consts.SecondsOfWeek) * Consts.SecondsOfWeek;
   //             prSec = prS - delS;
   //         }
   //         return prSec;
   //     }


    }
}