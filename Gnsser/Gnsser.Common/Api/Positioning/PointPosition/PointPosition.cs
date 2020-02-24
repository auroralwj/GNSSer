//2015.10.24, czs, create in 彭州, 精密单点定位

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
    /// 单点定位方式
    /// </summary>
    public enum PositionType
    { 
        /// <summary>
        /// 简单伪距定位
        /// </summary>
        SimpleRangePositioner,
        /// <summary>
        /// 精密单点定位
        /// </summary>
        IonoFreePpp,
        /// <summary>
        /// 模糊度固定
        /// </summary>
        PppPartAmbiResolution
    }


    /// <summary>
    /// 定位选项
    /// </summary>
    public class PointPosition : AbstractParallelableIoOperation<PointPositionParam>, IWithGnsserConfig
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public PointPosition()
        {
            this.InputFileExtension = "*.??O;*.??d.Z";
            this.WorkFileExtension = Setting.RinexOFileFilter;
        }

        #region 参数，属性  
        /// <summary>
        /// 定位类型
        /// </summary>
        PositionType PositionType = PositionType.IonoFreePpp;
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
        /// 计算选项
        /// </summary>
        public GnssProcessOption Option { get; set; }
 
        #endregion

        /// <summary>
        /// 获取参数文件读取器
        /// </summary>
        /// <returns></returns> 
        protected override LineFileReader<PointPositionParam> GetParamFileReader()
        {
            return new PointPositionParamReader(this.OperationInfo.ParamFilePath);
        }

        /// <summary>
        /// 建立输出文件路径
        /// </summary>
        /// <param name="outPath"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        protected override string BuildOutputFilePath(string outPath, string file)
        {
            var outFile = Path.Combine(outPath, "Sites", Path.GetFileNameWithoutExtension(file) + ".PositionResult.xls");
            //    var outFile = Geo.Utils.FileUtil.GetOutputFilePath(outPath, file) + ".PositionResult.xsl";
            return outFile;
        }

        /// <summary>
        /// 具体的执行
        /// </summary>
        /// <param name="inPath"></param>
        /// <param name="outPath"></param>
        protected override void Execute(string inPath, string outPath)
        {
            if (this.IsCancel) { return; }
            var Solver = new SingleSiteGnssSolveStreamer();
            if (PathUtil.IsValidPath(CurrentParam.EphemerisPath))
            {
                if (this.EphemerisDataSource == null || this.EphemerisDataSource.Name != Path.GetFileName(CurrentParam.EphemerisPath))
                {
                    var sp3 = InputFileManager.GetLocalFilePath(CurrentParam.EphemerisPath, "*.sp3");
                    if (PathUtil.IsValidPath(sp3))
                    {
                        this.EphemerisDataSource = EphemerisDataSourceFactory.Create(sp3);
                        Solver.EphemerisDataSource = this.EphemerisDataSource;
                    }
                }
            }
            if (PathUtil.IsValidPath(CurrentParam.ClockPath))
            {
                if (this.ClockFile == null || this.ClockFile.Name != Path.GetFileName(CurrentParam.ClockPath))
                {
                    var clk = InputFileManager.GetLocalFilePath(CurrentParam.ClockPath, "*.clk");
                    if (PathUtil.IsValidPath(clk))
                    {
                        this.ClockFile = new Data.SimpleClockService(clk);
                        Solver.ClockFile = this.ClockFile;
                    }
                }
            }
            if (Option == null)
            {
                Option = GnssProcessOptionManager.Instance[GnssSolverType.无电离层组合PPP];
            }
            //Solver.InfoProduced += Solver_InfoProduced;
            //Solver.ResultProduced += Solver_ResultProduced;
            //Solver.EpochEntityProduced += Solver_EpochEntityProduced;
            Solver.Completed += Solver_Completed;
            Solver.Option = Option;
            Solver.Option.IsOutputEpochResult = false;
            Solver.Init(inPath);

            var count = Solver.DataSource.ObsInfo.Count;
            //串行进度条初始化,阶段计算的第一个
            //if (!ParallelConfig.EnableParallel) { this.ProgressBar.InitFirstProcessCount(count); }

            log.Info(Solver.Name + " 初始化完毕，开始计算。");

            Solver.Run();
            log.Info(Solver.Name + " 计算完成。"); 
        }

        void Solver_Completed(object sender, EventArgs e)
        { 
            var Solver = sender as SingleSiteGnssSolveStreamer;
            //显示和输出详细结果
            var result = Solver.CurrentGnssResult;
            if(result is BaseGnssResult)
            {
                log.Info("即将输出结果文件...");
                var writer = new GnssResultWriter(Solver.Option);
                 writer.WriteFinal((BaseGnssResult)result);
            }

            //保留一个供图形输出等。
            //if (this.Solver.Name != Solver.Name)
            {
                //RunningSolvers.Remove(Solver.Path);
                Solver.Dispose();
                Solver.Context = null;
                Solver.DataSource = null;
                Solver = null;
                //GC.Collect();//强制内存回收
            }
        } 
    }
}
