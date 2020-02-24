//2015.10.24, czs, create in 彭州, 双差定位
//2016.11.27, czs, edit in hongqing, 基线解算，双差

using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Geo.IO;
using Geo;
using Gnsser.Domain; 
using System.ComponentModel;
using System.Data;  
using Gnsser;
using Gnsser.Data;
using Gnsser.Data.Rinex; 
using Gnsser.Service;
using Geo.Coordinates;
using Geo.Referencing;
using Geo.Algorithm.Adjust;
using Geo.Utils;
using Geo.Times;
using System.Threading.Tasks;

namespace Gnsser.Api
{ 
    /// <summary>
    /// 复制文件
    /// </summary>
    public class DoubleDiffer : ParamBasedOperation<DoubleDifferParam>, IWithGnsserConfig
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public DoubleDiffer()
        {
            ProcessCount = 4;
            this.InputFileExtension = "*.??O;*.??d.Z";
            FileExtension = Setting.RinexOFileFilter;
            InputFileManager = new InputFileManager();
            BaseLineSelector = new BaseLineSelector( BaseLineSelectionType.中心站法, "", "");
        } 
        #region 参数，属性
        /// <summary>
        /// 基线选择器，如果没有提供基线文件，则自动选择基线
        /// </summary>
        BaseLineSelector BaseLineSelector { get; set; }
        /// <summary>
        /// 输入文件后缀名
        /// </summary>
        public string InputFileExtension { get; set; }
        /// <summary>
        /// 计算文件后缀名
        /// </summary>
        public string FileExtension { get; set; }
        /// <summary>
        /// 处理器数量
        /// </summary>
        public int ProcessCount { get; set; }
        /// <summary>
        /// 是否并行
        /// </summary>
        public bool IsParallel { get; set; } 
        /// <summary>
        /// 手动指定的星历数据源
        /// </summary>
        public IEphemerisService EphemerisDataSource { get; set; }
        /// <summary>
        /// 外部指定的钟差文件，非系统配置
        /// </summary>
        public Data.ISimpleClockService ClockFile { get; set; }
        /// <summary>
        /// 定位全局设置
        /// </summary>
        public GnsserConfig GnsserConfig { get; set; }
        /// <summary>
        /// 工程输出目录
        /// </summary>
        public string OupputDirecory { get; set; } 
        InputFileManager InputFileManager { get; set; } 

        #endregion
          

        /// <summary>
        /// 执行
        /// </summary>
        /// <returns></returns>
        public override bool Do()
        {
            var reader = new DoubleDifferParamReader(this.OperationInfo.ParamFilePath);
            foreach (var item in reader)
            {
                CurrentParam = item;
                this.ProcessCount = item.ParallelProcessCount;

                var inPath = item.InputPath;
                var outPath = item.OutputPath;

                if (OupputDirecory == null)
                {
                    if (Geo.Utils.FileUtil.IsDirectory(outPath))
                    {
                        this.OupputDirecory = outPath;
                    }
                    else
                    { 
                        OupputDirecory = Path.GetDirectoryName(outPath);
                    }
                }

                //读取预知信息
                 this.ObsFiles = InputFileManager.GetLocalFilePathes(inPath, FileExtension);
                 InputObsFiles = new Dictionary<string, string>();
                 foreach (var file in ObsFiles)
                 {
                     var key = Path.GetFileName(file);
                     InputObsFiles.Add(key, file);
                 }
                 if (File.Exists(item.BaselinePath))
                 { 
                     this.Baselines = new BaselineReader(item.BaselinePath).ReadAll();
                 }
                 else
                 {
                     this.Baselines = BaseLineSelector.GetFileBaselines(ObsFiles.ToArray());
                 }

                if (File.Exists(item.SiteInfoPath))
                {
                    SiteInfoDics = new Dictionary<string, SiteInfo>();
                    List<SiteInfo> sites = new SiteInfoReader(item.SiteInfoPath).ReadAll();
                    foreach (var site in sites)
                    {
                        SiteInfoDics.Add(site.SiteName.ToUpper(), site);
                    }
                }

                //计算
                if (CurrentParam.IsParallel)
                {
                    ParallelCompute(item, inPath, outPath);
                }
                else { SerialProcess(item, inPath, outPath); }


            }
            return true;
        }

        /// <summary>
        /// 输入路径
        /// </summary>
        Dictionary<string, string> InputObsFiles = new Dictionary<string, string>();
        /// <summary>
        /// 观测文件
        /// </summary>
        List<string> ObsFiles { get; set; }
        List<Baseline> Baselines { get; set; }
        Dictionary<string, SiteInfo> SiteInfoDics { get; set; }
        public void ParallelCompute(DoubleDifferParam item, string inPath, string outPath)
        { 
            Parallel.ForEach(Baselines, this.ParallelOptions, (baseline, state) =>
          {
              var outFile = BuildOutputFilePath(outPath, baseline);
              CheckOrExecute(baseline, outFile, item.IsOverwrite);

              //是否终止计算//|| (DoubleDifferer !=null &&  DoubleDifferer.IsCancel)
              if (IsCancel) state.Break();
          });
        }
        /// <summary>
        ///  并行配置
        /// </summary>
        public ParallelOptions ParallelOptions
        {
            get
            {
                ParallelOptions option = new ParallelOptions();
                option.MaxDegreeOfParallelism = ProcessCount;
                return option;
            }
        }

        private void SerialProcess(DoubleDifferParam item, string inPath, string outPath)
        { 
            foreach (var file in Baselines)
            {
                var outFile = BuildOutputFilePath(outPath, file);
                CheckOrExecute(file, outFile, item.IsOverwrite);
            }
            var Message = "已完成 " + item.InputPath + " 到 " + item.OutputPath;
            this.OnStatedMessageProduced(StatedMessage.GetProcessed(Message));
        }


        /// <summary>
        /// 建立输出文件路径
        /// </summary>
        /// <param name="outPath"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        protected string BuildOutputFilePath(string outPath, Baseline file)
        {
            var outFile = Path.Combine(outPath, "Sites", file.StartName + "-" + file.EndName + ".BaselineResult.xls");
            //    var outFile = Geo.Utils.FileUtil.GetOutputFilePath(outPath, file) + ".PositionResult.xsl";
            return outFile;
        }
        /// <summary>
        /// 转换
        /// </summary>
        /// <param name="fileInPath">输入文件路径</param>
        /// <param name="fileOutPath">输出文件路径</param>
        /// <param name="isOverwrite"></param>
        protected  void CheckOrExecute(Baseline fileInPath, string fileOutPath, bool isOverwrite)
        {
            if (File.Exists(fileOutPath))
            {
                if (isOverwrite)
                {
                    var Message = "文件" + fileOutPath + "已存在，即将覆盖 ";
                    this.OnStatedMessageProduced(StatedMessage.GetProcessing(Message));
                    Execute(fileInPath, fileOutPath);
                }
                else
                {
                    var Message = "文件" + fileOutPath + "已存在，操作取消 ";
                    this.OnStatedMessageProduced(StatedMessage.GetProcessing(Message));
                }
            }
            else
            {
                var Message = "正在处理 " + fileInPath + " 到 " + fileOutPath;
                this.OnStatedMessageProduced(StatedMessage.GetProcessing(Message));

                Execute(fileInPath, fileOutPath);
            }
        }
        /// <summary>
        /// 具体的执行
        /// </summary>
        /// <param name="baseline"></param>
        /// <param name="outPath"></param>
        protected  void Execute(Baseline baseline, string outPath)
        { 
            if (PathUtil.IsValidPath(CurrentParam.EphemerisPath))
            {
                if (this.EphemerisDataSource == null || this.EphemerisDataSource.Name != Path.GetFileName(CurrentParam.EphemerisPath))
                {
                    var sp3 = InputFileManager.GetLocalFilePath(CurrentParam.EphemerisPath, "*.sp3");
                    if (PathUtil.IsValidPath(sp3))
                    {
                        this.EphemerisDataSource = EphemerisDataSourceFactory.Create(sp3); 
                    }
                }
            }
            if (PathUtil.IsValidPath(CurrentParam.ClockPath))
            {
                if (this.ClockFile == null || this.ClockFile.Name != Path.GetFileName(CurrentParam.ClockPath))
                {
                    var clk = InputFileManager.GetLocalFilePath(CurrentParam.ClockPath, "*.clk" );
                    if (PathUtil.IsValidPath(clk))
                    {
                        this.ClockFile = new Data.SimpleClockService(clk); 
                    }
                }
            }  

            var refPath = ObsFiles.Find(m => m.ToLower().Contains(baseline.StartName.ToLower()));
            var rovPath = ObsFiles.Find(m => m.ToLower().Contains(baseline.EndName.ToLower()));
            if (refPath == null || rovPath == null)
            {
                throw new ArgumentException("没有找到基线对应的文件！" + baseline.ToString());
                return;
            }
            var inputPathes = new string[] { refPath, rovPath };

            RinexFileObsDataSource refObsDataSource = new RinexFileObsDataSource(refPath, true);
            RinexFileObsDataSource rovObsDataSource = new RinexFileObsDataSource(rovPath, true);
           
            if(SiteInfoDics != null){
                var siteName = baseline.StartName.ToUpper();
                if (SiteInfoDics.ContainsKey(siteName))
                {
                    refObsDataSource.SiteInfo.SetApproxXyz(SiteInfoDics[siteName].EstimatedXyz);
                } 
                siteName = baseline.EndName.ToUpper();
                if (SiteInfoDics.ContainsKey(siteName))
                {
                    rovObsDataSource.SiteInfo.SetApproxXyz(SiteInfoDics[siteName].EstimatedXyz);
                }
            }

            GnssProcessOption option = GnssProcessOption.GetDefault(GnsserConfig, refObsDataSource.ObsInfo);
            option.GnssSolverType = GnssSolverType.无电离层双差;

            var source = new MultiSiteObsStream(inputPathes, BaseSiteSelectType.GeoCenter, true, "");
            DataSourceContext context = DataSourceContext.LoadDefault(option, source, EphemerisDataSource, ClockFile); 
            
            IntegralGnssFileSolver Solver = new IntegralGnssFileSolver();  
            Solver.Completed += Solver_Completed; 
             Solver.Option = option;
             Solver.IsCancel = false;
             Solver.Solver = GnssSolverFactory.Create(context, option); ;

            Solver.Init(inputPathes); 

            Solver.Run();
            var last = Solver.CurrentGnssResult;
             
            lock (locker)
            {
                Geo.Utils.FileUtil.CheckOrCreateDirectory(Path.GetDirectoryName(outPath)); 
                SaveLastResult(last);
            }
        } 

        void Solver_Completed(object sender, EventArgs e)
        { 

        }

        static object locker = new object();
        SiteInfoWriter SiteInfoWriter;
        /// <summary>
        /// 打印出执行结果
        /// </summary>
        /// <param name="last"></param>
        private void SaveLastResult(SimpleGnssResult last)
        { 
           // if(BaseGnssResult )
            var projPath = Path.Combine(this.OupputDirecory, "Baseline.Summery.baseline");
            Geo.Utils.FileUtil.CheckOrCreateDirectory(this.OupputDirecory);

            //if (SiteInfoWriter == null)
            //{
            //    SiteInfoWriter = new SiteInfoWriter(projPath);
            //}
            ////SiteInfoWriter.Write(last.SiteInfo);
            //SiteInfoWriter.StreamWriter.Flush();

            var ddResult = last as IonFreeDoubleDifferPositionResult;// DoubleDifferPositionResult;
            string msg = last.Name + "\t"
                  + Geo.Utils.DateTimeUtil.GetFormatedDateTimeNow() + "\t"
                  + ddResult.ReceiverTime + "\t"
                  + ddResult.GetEstimatedBaseline().EstimatedVector.GetTabValues() + "\t"
            ;

            if (!File.Exists(projPath))
            {
                var titles = new StringBuilder();
                titles.Append("Name");
                titles.Append("\t");
                titles.Append("CaculateTime");
                titles.Append("\t");
                titles.Append("ReceiverTime");
                titles.Append("\t");
                titles.Append("X");
                titles.Append("\t");
                titles.Append("Y");
                titles.Append("\t");
                titles.Append("Z");
                titles.AppendLine();
                File.AppendAllText(projPath, titles.ToString());
            }

            File.AppendAllText(projPath, msg + "\r\n");
        }
    }
}
