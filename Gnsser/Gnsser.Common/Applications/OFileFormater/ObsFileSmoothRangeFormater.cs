//2018.07.17, czs, create in HMX, 伪距平滑转换

using System;
using System.IO;
using System.Linq;
using System.Linq.Dynamic;
using System.Linq.Expressions;
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
using Gnsser; 
using Geo.Referencing; 
using Geo.Utils; 
using Gnsser.Checkers;

namespace Gnsser
{


    /// <summary>
    /// 伪距平滑转换
    /// </summary>
    public class ObsFileSmoothRangeFormater : SingleSiteObsAdjustStreamer
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="OutputDirectory"></param>
        public ObsFileSmoothRangeFormater(string OutputDirectory=null) : base(OutputDirectory)
        {
        }


        /// <summary>
        /// 历元写
        /// </summary>
        public EpochRinexObsFileWriter EpochRinexObsFileWriter { get; set; }


        /// <summary>
        /// 初始化
        /// </summary>
        public override void Init()
        { 
            base.Init();
            var destPath = System.IO.Path.Combine(this.Option.OutputDirectory, System.IO.Path.GetFileName(Path));
            EpochRinexObsFileWriter = new Data.Rinex.EpochRinexObsFileWriter(destPath, this.Option.OutputRinexVersion); 
        }

        /// <summary>
        /// 处理一个历元
        /// </summary>
        /// <param name="epoch"></param>
        public override void Process(EpochInformation epoch)
        {

            foreach (var sat in epoch)
            {
                foreach (var freqObs in sat)
                {
                   var pseudoRanges = freqObs.GetPseudoRanges();

                    foreach (var pseudoRange in pseudoRanges)
                    {

                        pseudoRange.Value = pseudoRange.CorrectedValue;
                    }
                }
            }

            EpochRinexObsFileWriter.Write(epoch);
        }


        /// <summary>
        /// 释放资源
        /// </summary>
        public override void Dispose()
        {
            EpochRinexObsFileWriter.Dispose();
            base.Dispose();
        }

        public override SimpleGnssResult Produce(EpochInformation material)
        {
            throw new NotImplementedException();
        }
    }



    /// <summary>
    /// 伪距平滑转换。
    /// </summary>
    public class ObsFileSmoothRangeFormater1 : ObsFileEpochRunner<EpochInformation>
    {
        Log log = new Log(typeof(ObsFileSmoothRangeFormater));

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="smoothWindow">平滑窗口</param>
        /// <param name="inPath"></param>
        public ObsFileSmoothRangeFormater1(string inPath, int smoothWindow)
        {
            this.FilePath = inPath;
            this.SmoothWindow = smoothWindow;
            this.BufferSize = smoothWindow;
            this.FileName = Path.GetFileName(FilePath);
            this.IsHeaderWrited = false;
            this.Inverval = Double.MaxValue;
        }
        #region 属性
        /// <summary>
        /// 平滑窗口
        /// </summary>
        public int SmoothWindow { get; set; }
        /// <summary>
        /// 缓存大小
        /// </summary>
        public int BufferSize { get; set; }
        /// <summary>
        /// 只是名称
        /// </summary>
        public string FileName { get; set; } 

        /// <summary>
        /// 文件名称
        /// </summary>
        public string FilePath { get; set; }
        /// <summary>
        /// 写入器
        /// </summary>
        RinexObsFileWriter Writer { get; set; } 
        /// <summary>
        /// 是否写了头部
        /// </summary>
        bool IsHeaderWrited { get; set; }
        /// <summary>
        /// 原始头部文件。
        /// </summary>
        public RinexObsFileHeader OldHeader { get; set; }
        /// <summary>
        /// 原始头部文件。
        /// </summary>
        public RinexObsFileHeader CurrentHeader { get; set; }
        /// <summary>
        /// 当前时段起始时间。
        /// </summary>
        public Time CurrentStartTime { get; set; }
        /// <summary>
        /// 当前历元观测数据,在写入前赋值。
        /// </summary>
        public EpochInformation CurrentEpochObs { get; set; } 
        /// <summary>
        /// 文件的初始时长
        /// </summary>
        public double InputTimeInMinutes { get; set; }  
        /// <summary>
        /// 采样间隔
        /// </summary>
        public double Inverval { get; set; }
        /// <summary>
        /// 输出的子目录
        /// </summary>
        public string SubDirectory { get; set; }
        #endregion
        /// <summary>
        /// 初始化
        /// </summary>
        public override void Init()
        {
            base.Init();
            InitWriter(FileName);


            this.BufferedStream.MaterialEnded += BufferedStream_MaterialEnded;
            var OldHeader = ReadOriginalHeader(FilePath);
            this.InputTimeInMinutes = (OldHeader.EndTime - OldHeader.StartTime) / 60.0;
            this.CurrentStartTime = OldHeader.StartTime;
            this.OldHeader = OldHeader;
            this.TableTextManager = new ObjectTableManager(10000, OutputDirectory);
            this.TableTextManager.Clear();
            EpochInfoToRinex = new EpochInfoToRinex(OldHeader.Version, true);
            LastEpoch = this.OldHeader.EndTime;
            //医学院， 75-112,33, 900-9500，学区房，五证全
        }
        Time LastEpoch { get; set; }
        void BufferedStream_MaterialEnded()
        {
    //        LastEpoch = BufferedStream.Last().Time;//czs, 2018.06.23, 这个不可用，否则末尾数据将离奇消失！！！
        }


        /// <summary>
        /// 初探
        /// </summary>
        /// <param name="info"></param>
        public override void RawRevise(EpochInformation info)
        {
        
            base.RawRevise(info);
        }

        public Time PrevOkEpoch { get; set; }
        public EpochInfoToRinex EpochInfoToRinex { get; private set; }

        /// <summary>
        /// 处理过程
        /// </summary>
        /// <param name="current"></param>
        public override void Process(EpochInformation current)
        {
            if (this.CurrentIndex == 0)
            {      
                //首次建立头文件，需要缓存支持
                this.CurrentHeader = (OldHeader);
            }

            //判断并写入文件
            WriteToFile(current);
        }

        /// <summary>
        /// 写到文件.判断是否分段写入
        /// </summary>
        /// <param name="current"></param>
        private void WriteToFile(EpochInformation current)
        {
            this.CurrentEpochObs = current;
            var epochObs = this.EpochInfoToRinex.Build(current);

            //最后，满足条件，写入文件
            Writer.WriteEpochObservation(epochObs);
        }

        public override void PostRun() { 
            if(CurrentEpochObs!=null){
                this.Writer.Flush();
            }
        }

        /// <summary>
        /// 构建子时段名称，精确到分钟，以示区别。
        /// </summary>
        /// <param name="current"></param>
        /// <returns></returns>
        private string BuildSectionFileName(EpochInformation current, RinexObsFileHeader header)
        {
            RinexFileNameBuilder nameBuilder = new RinexFileNameBuilder();
            return nameBuilder.Build(header); 
        }
         

        private RinexObsFileHeader ReadOriginalHeader(string inFilePath)
        {
            var reader = new RinexObsFileReader(inFilePath);
            return  reader.GetHeader();//以原头文件为蓝本
        }
        /// <summary>
        /// 初始化读取器
        /// </summary>
        /// <param name="outFilePath"></param>
        private void InitWriter(string outFilePath)
        {
            if (Writer != null) { Writer.Dispose(); }

            var toPath = "";
            if (!String.IsNullOrWhiteSpace(SubDirectory))
            {
                toPath = Path.Combine(OutputDirectory, SubDirectory, Path.GetFileName(outFilePath));
            }
            else
            {
                toPath = Path.Combine(OutputDirectory, Path.GetFileName(outFilePath));
            }

            Writer = new RinexObsFileWriter(toPath, CurrentHeader.Version);
        }
        /// <summary>
        /// 写头部
        /// </summary>
        /// <param name="header"></param>
        private void WriteHeader(RinexObsFileHeader header)
        {
            Writer.WriteHeader(header);
        }

        #region 统计观测类型 
         
        #endregion
        /// <summary>
        /// 数据流。
        /// </summary>
        /// <returns></returns>
        protected override BufferedStreamService<EpochInformation> BuildBufferedStream()
        {
            return new BufferedStreamService<EpochInformation>(new RinexFileObsDataSource(FilePath), BufferSize);
        } 
        /// <summary>
        /// 完成
        /// </summary>
        protected override void OnCompleted()
        {
            if (Writer != null)
            {
                Writer.Flush();
                Writer.Dispose();
            }
            base.OnCompleted();
        }
    }

}