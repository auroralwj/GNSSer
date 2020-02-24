//2017.02.09, czs, create in hongqing, FCB 宽巷计算器
//2017.03.08, czs, edit in hongqing, MwTableBuilder单独提出，便于后续组建产品
//2018.08.30, czs, edit in hmx,去除卫星高度角文件，以星历服务代替，DCB改正适应所有日期
//2018.09.03, czs, edit in hmx, 引入平滑伪距
//2018.09.09, czs, eidt in hmx, 重构

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
    /// MW 单独提出，便于后续组建产品
    /// </summary>
    public class RawMwTableBuilder : EpochInfoProcessStreamer, IBuilder<ObjectTableStorage>
    {
        ILog log = new Log(typeof(MwTableBuilder));

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="path"></param>
        /// <param name="AngleCut"></param> 
        /// <param name="RowCountToBeEmpty">移除前面的数据行数量，避免数据偏差太大</param>
        /// <param name="OutputDirectory"></param>
        public RawMwTableBuilder(
            string path,
            double AngleCut = 30,
            int RowCountToBeEmpty = 40,
            List<SatelliteType> satelliteTypes = null) : base(path, satelliteTypes)
        {
            this.FilePath = path;
            this.AngleCut = AngleCut;
            this.RowCountToBeEmpty = RowCountToBeEmpty;

            DcbRangeCorrector = new DcbRangeCorrector(GlobalDataSourceService.Instance.DcbDataService, false);
            EphemerisService = GlobalNavEphemerisService.Instance;

        }


        #region 属性
        public event Action OneFileProcessed;
        /// <summary>
        /// DCB 改正
        /// </summary>
        DcbRangeCorrector DcbRangeCorrector { get; set; }

        /// <summary>
        /// 移除前面的数据行数量，避免数据偏差太大
        /// </summary>
        public int RowCountToBeEmpty { get; set; }

        /// <summary>
        /// 卫星高度角
        /// </summary>
        public double AngleCut { get; set; }
        string namePostfix = "_RawMW";
        #region 输出产品
        /// <summary>
        /// 星历，用于计算卫星高度
        /// </summary>
        IEphemerisService EphemerisService { get; set; }
        /// <summary>
        /// 是否平滑伪距
        /// </summary>
        public bool IsSmoothRange { get; set; }
        /// <summary>
        /// 输出产品
        /// </summary>
        public ObjectTableStorage Result { get; private set; }
        #endregion
        #endregion
        public override void Init()
        {
            this.DataSource = new RinexFileObsDataSource(FilePath);
            this.Result = new ObjectTableStorage(); //最后结果
            this.Result.Name = DataSource.SiteInfo.SiteName + namePostfix;//
            if (String.IsNullOrWhiteSpace(Result.Name))
            {
                Result.Name = Path.GetFileNameWithoutExtension(FilePath);
            }

            this.BuffferStream = new BufferedStreamService<EpochInformation>(DataSource);


            //如果平滑伪距，则需要周跳探测
            Revisers = new EpochInfoReviseManager();
            if (IsSmoothRange)
            {
                var interval = DataSource.ObsInfo.Interval;
                int smoothEpochCount = (int)(600 / interval);//10 min windowData.MaxKeyGap = source.ObsInfo.Interval * 5;//已经包含着周跳探测中了
                var CycleSlipDetector = CycleSlipDetectReviser.DefaultDoubleFrequencyDetector();
                var rangeReviser = new PhaseSmoothRangeReviser(true, false, smoothEpochCount, true, IonoDifferCorrectionType.DualFreqCarrier);

                Revisers.AddProcessor(CycleSlipDetector);
                Revisers.AddProcessor(rangeReviser);
            }
        }
        #region 提取MW值
        /// <summary>
        /// 提取一个MW原始值。作为具有卫星和接收机硬件延迟的宽巷模糊度。
        /// </summary>
        /// <param name="FilePath"></param>
        /// <param name="namePostfix"></param>
        /// <returns></returns>
        public ObjectTableStorage Build()
        {
            this.Init();

            foreach (var epoch in BuffferStream)
            {
                if (epoch == null ) { continue; }
                Run(epoch);
            }

            this.OnCompleted();

            return Result;
        }

        public override void Run(EpochInformation epoch)
        {
            PreProcess(epoch);

            Process(epoch);

            Result.EndRow();
        }

        public void Process(EpochInformation epoch)
        {
            Result.NewRow();
            Result.AddItem("Epoch", epoch.ReceiverTime);

            //计算
            foreach (var sat in epoch)
            {
                if (!IsValid(sat)) { continue; }
                //已验证以下二方法等价。
                double mwValue = 0;
                bool isOld = false;
                if (isOld)
                {
                    var f1 = sat.FrequenceA.Frequence.Value;
                    var f2 = sat.FrequenceB.Frequence.Value;

                    double L1 = sat.FrequenceA.PhaseRange.Value;
                    double L2 = sat.FrequenceB.PhaseRange.Value;
                    double P1Raw = sat.FrequenceA.PseudoRange.Value;//原始值
                    double P1 = sat.FrequenceA.PseudoRange.CorrectedValue;//改正到P1的值，加上了P1C1

                    double P2 = sat.FrequenceB.PseudoRange.CorrectedValue; //P2
                    mwValue = GetMwValue(f1, f2, L1, L2, P1, P2) / 0.86191840032200528; //注意以周为单位
                }
                else
                {
                    mwValue = sat.Combinations.MwPhaseCombinationValue;
                }

                Result.AddItem(sat.Prn, mwValue);
            }
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
        /// 款项载波减去窄巷伪距
        /// </summary>
        /// <param name="f1"></param>
        /// <param name="f2"></param>
        /// <param name="L1"></param>
        /// <param name="L2"></param>
        /// <param name="P1"></param>
        /// <param name="P2"></param>
        /// <returns></returns>
        public static double GetMwValue(double f1, double f2, double L1, double L2, double P1, double P2)
        {
            double e = f1 / (f1 - f2);
            double f = f2 / (f1 - f2);
            double c = f1 / (f1 + f2);
            double d = f2 / (f1 + f2);

            double value =
                e * L1
              - f * L2

              - c * P1
              - d * P2;
            return value;
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

        #endregion
    }
}