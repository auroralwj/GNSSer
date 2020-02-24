//2017.04.24, czs, create in hongqing, 实时GNSS定位器

using System.Collections.Generic;
using System;
using System.Drawing;
using System.Diagnostics;
using System.Data;
using System.Text;
using Microsoft.VisualBasic;
using System.Collections;
using System.Windows.Forms;
using System.IO;
using Microsoft.VisualBasic.CompilerServices;
using Gnsser.Ntrip.Rtcm;
using Geo;
using Geo.IO;
using Geo.Utils;
using Geo.Times;
using Gnsser.Data.Rinex; 
using Geo.Algorithm;
using Geo.Coordinates;
using Geo.Algorithm.Adjust;
using Geo.Algorithm;
using Gnsser.Times;
using Gnsser.Data; 
using Gnsser.Domain;
using Gnsser.Service;
using Gnsser.Correction; 
using Gnsser;
using Geo.Referencing; 
using Gnsser.Checkers;

namespace Gnsser.Ntrip
{
    /// <summary>
    /// 实时GNSS定位器， 对应一个测站。
    /// </summary>
    public class RealTimeGnssPositioner : RealTimeGnssDataUser
    {
        Log log = new Log(typeof(RealTimeGnssPositioner));
        static object locker = new object();
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="siteName"></param>
        /// <param name="OutpuDirectory"></param>
        public RealTimeGnssPositioner(GnssSolverType GnssSolverType, String OutpuDirectory, string siteName, Time startTime)
        {
            this.GnssSolverType = GnssSolverType;
            this.OutpuDirectory = OutpuDirectory;
            lock (locker)
            {
                if (NavFile == null)
                {
                    NavFile = new ParamNavFile();
                }
            }
            this.ObsFile = new RinexObsFile();
            SSRSp3Section = new InstantSp3Section();
            Sp3File = new Sp3File(); 
            this.ObsFile.Header = new Data.Rinex.RinexObsFileHeader()
            {
                MarkerName = siteName,
                StartTime = startTime,
                ObsCodes = new Dictionary<SatelliteType, List<string>>(),
            };
            this.ObsFile.Header.ObsCodes[SatelliteType.G] = new List<string>(){"C1X","L1X","C2X","L2X"};
        }
        GnssSolverType GnssSolverType { get; set; }
        /// <summary>
        /// 初始化计算器
        /// </summary>
        private void InitSolver()
        {
            this.Solver = new SingleSiteGnssSolveStreamer(OutpuDirectory);
            var Option =  GnssProcessOptionManager.Instance.Get(GnssSolverType.无电离层组合PPP);
            Option.OutputDirectory = OutpuDirectory;
           // Option.EnableAutoFindingFile = false;
            Setting.EnableNet = false;
            var epheService = EphemerisService;
            if (epheService == null)
            {
                epheService = new SingleParamNavFileEphService(NavFile);
            } 

            var obsData = new MemoRinexFileObsDataSource(ObsFile);
            this.Solver.Context = DataSourceContext.LoadDefault(Option, obsData, epheService, null);
            this.Solver.BufferedStream = BuildBufferedStream();
            this.Solver.Option = Option;
            this.Solver.DataSource = obsData;
            //Solver.InfoProduced += Solver_InfoProduced;
            //Solver.ResultProduced += Solver_ResultProduced;
            //Solver.EpochEntityProduced += Solver_EpochEntityProduced;
            //Solver.Completed += OneSolver_Completed;
            this.Solver.Init();
        }

        #region 属性
        /// <summary>
        /// 计算器
        /// </summary>
        public SingleSiteGnssSolveStreamer Solver { get; set; }
        public string OutpuDirectory { get; set; }
        /// <summary>
        /// 导航文件
        /// </summary>
        public static ParamNavFile NavFile { get; set; }
        /// <summary>
        /// 基于实时星历改正数的实时星历
        /// </summary>
        public Sp3File Sp3File { get; set; }

        /// <summary>
        /// 实时星历改正数
        /// </summary>
        public static InstantSp3Section SSRSp3Section { get; set; }


        /// <summary>
        /// 手动指定的星历服务
        /// </summary>
        public static IEphemerisService EphemerisService { get; set; }
        /// <summary>
        /// 观测文件
        /// </summary>
        RinexObsFile ObsFile { get; set; }
        public static Time currentTime { get; set; }
        #endregion

        #region  与数据提供器方法绑定

        protected override void dataProvider_ObsHeaderUpdated(RinexObsFileHeader obj)
        {
            this.ObsFile.Header = obj;
        }
        protected override void dataProvider_ObsHeaderCreated(RinexObsFileHeader obj)
        {
            this.ObsFile.Header = obj;
        }
        /// <summary>
        /// 单系统的观测数据
        /// </summary>
        /// <param name="obj"></param>
        protected override void dataProvider_EpochObservationReceived(RinexEpochObservation obj)
        {
            if (obj == null || obj.Count == 0 || obj.Prns[0].SatelliteType != SatelliteType.G) { return; }
            ObsFile.Add(obj);
            currentTime = obj.ReceiverTime;
            RTSp3InfoCorrected(obj);

            if (NavFile.Count!=0)
                CheckAndPosition(obj);
        }

        protected override void dataProvider_EphemerisInfoReceived(EphemerisParam obj)
        {
            if (obj == null ) { return; }
            NavFile.Add(obj);
        }
        protected override void dataProvider_SSRSp3RecordReceived(Sp3Section obj)
        {
            if (obj == null ) { return; }

            SSRSp3Section[obj.First.Prn.SatelliteType] = obj;            
        }

        /// <summary>
        /// 实时星历改正。
        /// </summary>
        /// <param name="obs"></param>
        /// <param name="maxCorrectionSecond">允许的最大改正秒数，是否该每颗卫星单独考虑</param>
        protected void RTSp3InfoCorrected(RinexEpochObservation obs, double maxCorrectionSecond = 60)//Time time, SatelliteType satType)
        {
            Time time = obs.ReceiverTime; 
            if (SSRSp3Section.Count == 0) { return; }

            if (time - SSRSp3Section.GetMaxTime() > maxCorrectionSecond) { 
                return; }

            Sp3Section Sp3Section = new Sp3Section();
            var keys = SSRSp3Section.Keys;
            foreach (var key in keys)
            {
                var obj = SSRSp3Section[key];
                foreach (var item in obj)
                {
                    if (!NavFile.Prns.Contains(item.Prn))
                        continue;
                    var ss = new SingleParamNavFileEphService(NavFile);
                    var unkown = NavFile.GetEphemerisParams(item.Prn).Find(b => Math.Abs(b.Time.TickTime.TotalSeconds - time.TickTime.TotalSeconds) < 3 * 3600);
                    if (unkown == null)
                        continue;
                    if (ss == null)
                        continue;
                    Ephemeris ss1 = ss.Get(item.Prn, time);
                    XYZ eA = ss1.XyzDot / ss1.XyzDot.Length;
                    XYZ eC = ss1.XYZ.Cross(ss1.XyzDot) / (ss1.XYZ.Cross(ss1.XyzDot)).Length;
                    XYZ eR = eA.Cross(eC) / (eA.Cross(eC)).Length;

                    XYZ deltaO = item.XYZ + item.XyzDot * (time.TickTime.TotalSeconds - item.Time.TickTime.TotalSeconds);
                    double x = eR.X * deltaO.X + eA.X * deltaO.Y + eC.X * deltaO.Z;
                    double y = eR.Y * deltaO.X + eA.Y * deltaO.Y + eC.Y * deltaO.Z;
                    double z = eR.Z * deltaO.X + eA.Z * deltaO.Y + eC.Z * deltaO.Z;
                    if (x * x + y * y + z * z > 100)
                    { }

                    Ephemeris Sp3Record = new Ephemeris();
                    Sp3Record.Prn = item.Prn;
                    Sp3Record.XYZ = ss1.XYZ - new XYZ(x, y, z);
                    Sp3Record.Time = time;

                    if (item.Prn.SatelliteType == SatelliteType.R)
                        Sp3Record.ClockBias = ss1.ClockBias + item.ClockBias;
                    else
                    {
                        double relativetime = 2 * ss1.XYZ.Dot(ss1.XyzDot) / (GnssConst.LIGHT_SPEED * GnssConst.LIGHT_SPEED);
                        Sp3Record.ClockBias = ss1.ClockBias - ss1.RelativeCorrection - item.ClockBias; //relativetime + key.ClockBias; 
                    }
                    Sp3Section.Add(Sp3Record.Prn, Sp3Record);
                }
            }

            if (Sp3Section.Count != 0)
            {
                Sp3Section.Time = time;
                Sp3File.Add(Sp3Section);
            }
            else
            {
                int a = 0;
            }

            if (Sp3File.Count > 11)
            {
                // Sp3File.Header = new Sp3Header();
                Sp3File.Header.EpochInterval = 1;
                //Sp3File.TimePeriod = new BufferedTimePeriod(Sp3File.First.Time, Sp3File.Last.Time, 60);
                if (EphemerisService == null)
                {
                    EphemerisService = new SingleSp3FileEphService(this.Sp3File.GetSatEphemerisCollection(), 10);
                }
                else
                {
                    ((SingleSp3FileEphService)EphemerisService).SetSp3File(Sp3File.GetSatEphemerisCollection());
                }
            }
            if (Sp3File.Count > 20)
            {
                Sp3File.RemoveFirst();
            }

        }
        #endregion

        /// <summary>
        /// 数据流
        /// </summary>
        /// <returns></returns>
        protected BufferedStreamService<EpochInformation> BuildBufferedStream()
        {
            var DataSource = new MemoRinexFileObsDataSource(ObsFile);
            var bufferStream = new BufferedStreamService<EpochInformation>(DataSource, 10);
            return bufferStream;
        }

        public void CheckAndPosition(RinexEpochObservation obj)
        {
            if (CheckIfCanPosition())
            {
                Positioning(obj);
            }
        }

        public void Positioning(RinexEpochObservation obj)
        {
         //   this.ObsFile.Header = obj.Header;
            if (obj == null || obj.Count == 0) { return; }

             var EpochInfoBuilder = new RinexEpochInfoBuilder(obj.Header.ObsInfo.SatelliteTypes);
            var epochInfo = EpochInfoBuilder.Build(obj);
            if (epochInfo.First.FrequencyCount < 2) 
            { return; }
            //RTSp3InfoCorrected(obj.Time);
            Solver.RawRevise(epochInfo);
            Solver.PreProcess(epochInfo);
            Solver.Run(epochInfo);

            if (Solver.CurrentGnssResult is BaseGnssResult)
            {
                var result = ((BaseGnssResult)Solver.CurrentGnssResult);

                if (obj.Header.ApproxXyz == null || obj.Header.ApproxXyz.IsZero)
                {

                    obj.Header.ApproxXyz = result.EstimatedXyz;

                    Solver.RawRevise(epochInfo);
                    Solver.PreProcess(epochInfo);
                    Solver.Run(epochInfo);
                }


                if (Solver.CurrentGnssResult != null) { log.Info(obj.Name + "\t" + obj.ReceiverTime + ":\t" + result.XyzCorrection + ":\t" + result.EstimatedXyzRms + ""); }
                else { log.Info(obj + "结果为空。"); }
            }
        }


        /// <summary>
        /// 检查是否可以定位了。
        /// </summary>
        /// <returns></returns>
        public bool CheckIfCanPosition()
        {
            if ((NavFile.Count >= 4 || ObsFile.Header.StartTime.DateTime < (DateTime.Now - TimeSpan.FromDays(7))) && EphemerisService != null && ObsFile.Count > 5)
            {
                if (Solver == null)
                {
                    log.Info("初始化定位器。");
                    InitSolver();
                }
                return true;
            }
            return false;
        }
    }
}