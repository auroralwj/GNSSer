//2017.02.09, czs, create in hongqing, FCB 宽巷计算器
//2017.03.08, czs, edit in hongqing, MwTableBuilder单独提出，便于后续组建产品
//2018.08.30, czs, edit in hmx,去除卫星高度角文件，以星历服务代替，DCB改正适应所有日期
//2018.09.02, czs, create in hmx, 全球测站MW快速提取。
//2018.09.09, czs, create in hmx,  单站星时段MW数值快速提取。

using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections.Generic;
using Geo;
using Geo.Algorithm;
using Geo.Coordinates;
using Geo.Algorithm.Adjust;
using Gnsser.Times;
using Gnsser.Data;
using Gnsser.Data.Rinex;
using Gnsser.Domain;
using Gnsser.Service;
using Gnsser.Correction;
using Geo.Times;
using Geo.IO;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace Gnsser.Winform
{      
    /// <summary>
    /// 单站星时段MW数值快速提取。
    /// 提取一个测站所有时段卫星的MW平均值，作为具有卫星和接收机硬件延迟的宽巷模糊度。
    /// 此处假定时段内卫星和接收机硬件延迟变化可以忽略不计。
    /// </summary>
    public class SingleSiteMwValueBuilder  : EpochInfoProcessStreamer, IBuilder<MultiSatPeriodRmsNumeralStorage>
    {
        ILog log = new Log(typeof(MwTableBuilder));

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="path"></param>
        /// <param name="AngleCut"></param> 
        public SingleSiteMwValueBuilder(
            string path,
            double AngleCut = 15,
            List<SatelliteType> satelliteTypes = null):base(path,satelliteTypes)
        {
            this.AngleCut = AngleCut;

            DcbRangeCorrector = new DcbRangeCorrector(GlobalDataSourceService.Instance.DcbDataService, false);
            EphemerisService = GlobalNavEphemerisService.Instance;

            MinEpochCount = 40;
            MaxAllowedRmsOfAveMw = 1;
            IsSmoothRange = false;

        }


        #region 属性
        /// <summary>
        /// 平滑MW最大允许的中误差，不应超过1周
        /// </summary>
        public double MaxAllowedRmsOfAveMw { get; set; }
        public DcbRangeCorrector DcbRangeCorrector { get; set; }
        /// <summary>
        /// 卫星高度角
        /// </summary>
        public double AngleCut { get; set; }
        /// <summary>
        /// 最小的历元数量，小于此则不考虑
        /// </summary>
        public int MinEpochCount { get; set; }
        /// <summary>
        /// 星历服务
        /// </summary>
        public IEphemerisService EphemerisService { get; set; }
        /// <summary>
        /// 是否平滑伪距
        /// </summary>
        public bool IsSmoothRange { get; set; }
        #endregion

        #region 过程和输出产品 
        /// <summary>
        /// 计算主力
        /// </summary>
        WindowDataManager<SatelliteNumber, Time, WeightedNumeral> WindowDataManager { get; set; }
        public MultiSatPeriodRmsNumeralStorage Result { get; set; }
        #endregion

        public override void Init()
        {
            this.DataSource = new RinexFileObsDataSource(FilePath);
            var interval = DataSource.ObsInfo.Interval == 0? 30: DataSource.ObsInfo.Interval;    

            this.WindowDataManager = new WindowDataManager<SatelliteNumber, Time, WeightedNumeral>(int.MaxValue / 2, interval * 3); //过程计算工具
            this.BuffferStream = new BufferedStreamService<EpochInformation>(DataSource);


            //如果平滑伪距，则需要周跳探测
            Revisers = new EpochInfoReviseManager();
            if (IsSmoothRange)
            {
                int smoothEpochCount = (int)(600 / interval);//10 min windowData.MaxKeyGap = source.ObsInfo.Interval * 5;//已经包含着周跳探测中了
                var CycleSlipDetector = CycleSlipDetectReviser.DefaultDoubleFrequencyDetector();
                var rangeReviser = new PhaseSmoothRangeReviser(true, false, smoothEpochCount, true, IonoDifferCorrectionType.DualFreqCarrier);

                Revisers.AddProcessor(CycleSlipDetector);
                Revisers.AddProcessor(rangeReviser);
            }
            this.Result = new MultiSatPeriodRmsNumeralStorage( Path.GetFileName(FilePath) + "_MW最后结果"); //最后结果
            this.Result.Name = DataSource.SiteInfo.SiteName;//
            if (String.IsNullOrWhiteSpace(Result.Name))
            {
                Result.Name = Path.GetFileNameWithoutExtension(FilePath);
            }
        }

        #region 提取MW值

        /// <summary>
        /// 提取一个测站所有时段卫星的MW平均值，作为具有卫星和接收机硬件延迟的宽巷模糊度。
        /// 此处假定时段内卫星和接收机硬件延迟变化可以忽略不计。
        /// </summary>
        /// <returns></returns>
        public MultiSatPeriodRmsNumeralStorage Build()
        {
            Init();

            foreach (var epoch in BuffferStream)
            {
                //简单的质量控制
                if (epoch == null) { continue; }

                Run(epoch);

            }//end of stream

            //最后，将最后的窗口数据进行检查输出
            foreach (var item in WindowDataManager.Data)
            {
                var prn = item.Key;
                var windowData = WindowDataManager.GetOrCreate(prn);

                CheckBuildPeriodResultAndClear(windowData, prn);
            }

            this.OnCompleted();

            return Result;
        }


        public override void Run(EpochInformation epoch)
        {
            PreProcess(epoch);
            
            //平滑伪距  //周跳探测
            if (IsSmoothRange)
            {
                var ep = epoch;
                Revisers.Buffers = BuffferStream.MaterialBuffers;
                Revisers.Revise(ref ep);
            }

            Process(epoch);
        }

        /// <summary>
        /// 预处理
        /// </summary>
        /// <param name="epoch"></param>
        public override void PreProcess(EpochInformation epoch)
        {
            epoch.RemoveIonoFreeUnavailable();

            //预处理
            var tobeDeletes = new List<SatelliteNumber>();
            foreach (var sat in epoch)
            {
                if (!IsValid(sat) || !SatelliteTypes.Contains(sat.Prn.SatelliteType)) { tobeDeletes.Add(sat.Prn); }
            }
            epoch.Remove(tobeDeletes);
            tobeDeletes.Clear();


            foreach (var sat in epoch)
            {
                if (!IsValid(sat) || !SatelliteTypes.Contains(sat.Prn.SatelliteType)) { continue; }
                var prn = sat.Prn;

                var eph = EphemerisService.Get(sat.Prn, sat.ReceiverTime);
                if (eph == null)
                {
                    tobeDeletes.Add(sat.Prn);
                    continue;
                }
                sat.Ephemeris = eph;
                if (sat.GeoElevation < AngleCut)
                {
                    tobeDeletes.Add(sat.Prn);
                    continue;
                }

                //C1 改为 P1
                DcbRangeCorrector.Correct(sat);
                //周跳探测
                //sat.IsUnstable = Detector.Detect(sat);
            }

            epoch.Remove(tobeDeletes);
            tobeDeletes.Clear();
        }

        /// <summary>
        /// 处理一个
        /// </summary>
        /// <param name="epoch"></param>
        public void Process(EpochInformation epoch)
        {
            Time time = epoch.ReceiverTime;

            //计算
            foreach (var sat in epoch)
            {
                if (!IsValid(sat) || !SatelliteTypes.Contains(sat.Prn.SatelliteType) || sat.Ephemeris == null) { continue; }
                var prn = sat.Prn;

                double mwValue = sat.Combinations.MwPhaseCombinationValue;

                var weight = 1.0;
                if (sat.Polar.Elevation < 30)
                {
                    weight = 2 * Math.Sin(sat.GeoElevation * Geo.Coordinates.AngularConvert.DegToRadMultiplier);
                }
                
                var windowData = WindowDataManager.GetOrCreate(prn);

                //弧段断裂，求值
                if (sat.IsUnstable || windowData.IsKeyBreaked(time))
                {
                    CheckBuildPeriodResultAndClear(windowData, prn);
                }

                windowData.Add(time, new WeightedNumeral(mwValue, weight));
            }


            //每一历元结束，都做一次判断，确保数据完全输出
            foreach (var item in WindowDataManager.Data)
            {
                var prn = item.Key;
                var windowData = WindowDataManager.GetOrCreate(prn);

                //弧段断裂，求值
                if (windowData.IsKeyBreaked(time))
                {
                    CheckBuildPeriodResultAndClear(windowData, prn);
                }
            }
        }

        /// <summary>
        /// 检查并计算平均，最后一定要清空此时段。
        /// </summary>
        /// <param name="windowData"></param>
        /// <param name="prn"></param>
        private void CheckBuildPeriodResultAndClear(WindowData<Time, WeightedNumeral> windowData, SatelliteNumber prn)
        {
            if (windowData.Count >= MinEpochCount)//太少，不要//输出时段产品
            {
                var ave3 = Geo.Utils.DoubleUtil.WeightedAverageWithRms(windowData.Values);
                if (ave3.Rms <= MaxAllowedRmsOfAveMw) // 质量控制
                {
                    TimePeriod tp = new TimePeriod(windowData.FirstKey, windowData.LastKey);
                    Result.GetOrCreate(prn).Add(tp, ave3);
                }
            }

            //清空
            windowData.Clear();
        }


        /// <summary>
        /// 数据是否有效
        /// </summary>
        /// <param name="sat"></param>
        /// <returns></returns>
        public bool IsValid(EpochSatellite sat)
        {
            if (!sat.Contains(FrequenceType.B)) { return false; }

            var isOk = sat.FrequenceA.PhaseRange.RawPhaseValue != 0
                && sat.FrequenceA.PseudoRange.Value != 0
                && sat.FrequenceB.PhaseRange.RawPhaseValue != 0
                && sat.FrequenceB.PseudoRange.Value != 0;

            return isOk;
        }

        public override string ToString()
        {
            return FilePath;
        }

        #endregion
    }
    
}