//2015.10.23, czs, create in  西安五路口 凉皮店, 观测文件的预处理
//2015.10.26, czs, edit in 洪庆, 重命名为定位预处理：PositionPreprocess

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
    public class PositionPreprocess : AbstractIoOperation<PointPositionParam>, IWithGnsserConfig
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public PositionPreprocess()
        {
            startTime = System.DateTime.MinValue;
            ProcessCount = 4;
        }

        #region 参数，属性
        /// <summary>
        /// 处理器数量
        /// </summary>
        public int ProcessCount { get; set; }
        /// <summary>
        /// 是否并行
        /// </summary>
        public bool IsParallel { get; set; }
        ///// <summary>
        ///// 定位器,此处暂时取消，以应对并行计算2015.10.26 czs k918  西安南公交
        ///// </summary>
        //IPointPositioner PointPositioner { get; set; }
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
        /// 工程输出目录
        /// </summary>
        public string OupputDirecory { get; set; }
        /// <summary>
        /// 当前参数
        /// </summary>
        PointPositionParam CurrentParam { get; set; }
        #region 内部参数
        //计时器
        DateTime startTime;
        #endregion
        #endregion

        public void Init(GnsserConfig GnsserConfig, IEphemerisService ephemerisDataSource, Data.ISimpleClockService clockFile)
        {
            this.GnsserConfig = GnsserConfig;
            this.EphemerisDataSource = ephemerisDataSource;
            this.ClockFile = clockFile;
        }

        /// <summary>
        /// 执行
        /// </summary>
        /// <returns></returns>
        public override bool Do()
        {
            if (startTime == System.DateTime.MinValue)
            {
                startTime = System.DateTime.Now;
            }

            var reader = new PointPositionParamReader(this.OperationInfo.ParamFilePath);
            foreach (var item in reader)
            {
                CurrentParam = item;
                this.ProcessCount = item.ParallelProcessCount;

                var inPath = item.InputPath;
                var outPath = item.OutputPath;

                if (OupputDirecory == null)
                {
                    OupputDirecory = Path.GetDirectoryName(outPath);
                }

                if (CurrentParam.IsParallel)
                {
                    ParallelCompute(item, inPath, outPath);
                }
                else { SerialProcess(item, inPath, outPath); }


            }
            return true;
        }
        /// <summary>
        /// 并行计算
        /// </summary>
        /// <param name="key"></param>
        /// <param name="inPath"></param>
        /// <param name="outPath"></param>
        public void ParallelCompute(PointPositionParam item, string inPath, string outPath)
        {
            var files = Geo.Utils.FileUtil.GetFiles(inPath, InputFileExtension);

            Parallel.ForEach(files, this.ParallelOptions, (file, state) =>
          {
              var outFile = BuildOutputFilePath(outPath, file);
              CheckOrExecute(file, outFile, item.IsOverwrite);

              //是否终止计算//|| (PointPositioner !=null &&  PointPositioner.IsCancel)
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
        /// <summary>
        /// 串行计算
        /// </summary>
        /// <param name="key"></param>
        /// <param name="inPath"></param>
        /// <param name="outPath"></param>
        private void SerialProcess(PointPositionParam item, string inPath, string outPath)
        {
            var files = Geo.Utils.FileUtil.GetFiles(inPath, InputFileExtension);
            foreach (var file in files)
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
        protected override string BuildOutputFilePath(string outPath, string file)
        {
            //   var outFile = Path.Combine(outPath, "Sites", Path.GetFileNameWithoutExtension(file) + ".PositionResult.xls");
            var outFile = Geo.Utils.FileUtil.GetOutputFilePath(outPath, file);// +".PositionResult.xsl";
            return outFile;
        }

        /// <summary>
        /// 具体的执行
        /// </summary>
        /// <param name="inPath"></param>
        /// <param name="outPath"></param>
        protected override void Execute(string inPath, string outPath)
        {
            this.EphemerisDataSource = EphemerisDataSourceFactory.Create(CurrentParam.EphemerisPath);
            if (File.Exists(CurrentParam.ClockPath))
            {
                this.ClockFile = new Data.SimpleClockService(CurrentParam.ClockPath); 
            }

            RinexFileObsDataSource obsDataSource = new RinexFileObsDataSource(inPath, true);
            GnssProcessOption option = GnssProcessOption.GetDefault(GnsserConfig, obsDataSource.ObsInfo);

            DataSourceContext DataSourceContext = DataSourceContext.LoadDefault(option, obsDataSource, this.EphemerisDataSource, ClockFile);
            EpochInfoReviseManager reviser = new EpochInfoReviseManager(DataSourceContext, option);

            //写入到流
            Gnsser.Data.Rinex.RinexObsFileWriter writer = new Data.Rinex.RinexObsFileWriter(outPath, CurrentParam.OutputVersion);
            EpochInfoToRinex EpochInfoToRinex = new Domain.EpochInfoToRinex(this.CurrentParam.OutputVersion, true);

            //直接写入数据流，并不存储，以节约空间。
            Gnsser.Data.Rinex.RinexObsFileHeader newHeader = null;
            int maxBufferEpoch = 200;
            int i = 0;
            foreach (var item in obsDataSource)
            {
                //预处理在此进行！！！
                var processed = item;
                 reviser.Revise( ref processed);

                if (processed != null)
                {
                    var epochObs = EpochInfoToRinex.Build(processed);
                    if (newHeader == null)
                    {
                        newHeader = epochObs.Header;
                        writer.WriteHeader(newHeader);
                    }

                    writer.WriteEpochObservation(epochObs);

                    if (i > maxBufferEpoch)
                    {
                        writer.Writer.Flush();
                    }
                }
                i++;
            }
            writer.Writer.Close();

            TimeSpan span = DateTime.Now - startTime;
            StringBuilder sb = new StringBuilder();
            sb.Append("耗时：" + DateTimeUtil.GetFloatString(span));
            sb.AppendLine(",输出到 " + outPath);

            //信息汇总
            lock (locker)
            {
                var path = Path.Combine(Path.GetDirectoryName(outPath), "PositionPreprocess.summery");

                Geo.Utils.FileUtil.CheckOrCreateDirectory(Path.GetDirectoryName(path));
                File.AppendAllText(path, sb.ToString());
            }
        }

        static object locker = new object();


    }


}
