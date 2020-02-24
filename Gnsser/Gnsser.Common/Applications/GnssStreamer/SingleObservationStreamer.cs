//2018.08.15, czs, create in hmx, 单站数据流处理器，用于预处理

using System;
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
using Gnsser; 
using Geo.Referencing; 
using Geo.Utils; 
using Gnsser.Checkers;

namespace Gnsser
{ 

    /// <summary>
    /// 单站数据流处理器
    /// </summary>
    public abstract class SingleObservationStreamer : ObsFileEpochRunner<EpochInformation>, IObsFileProcessStreamer<EpochInformation>
    {
        new protected ILog log = new Log(typeof(SingleObservationStreamer));
        /// <summary>
        /// 构造函数
        /// </summary> 
        /// <param name="Option"></param>
        public SingleObservationStreamer(GnssProcessOption Option)
        {
            string outputDirectory = Option.OutputDirectory;
            if (String.IsNullOrWhiteSpace(  outputDirectory )) { outputDirectory = Setting.TempDirectory; }
            this.OutputDirectory = outputDirectory;

            this.ExtraStreamLoopCount = Option.ExtraStreamLoopCount;
        }

        #region 属性
        /// <summary>
        /// 选项
        /// </summary>
        public GnssProcessOption Option { get; set; }
        /// <summary>
        /// 文件路径
        /// </summary>
        public string Path { get; set; }
        /// <summary>
        /// 数据源
        /// </summary>
        public ISingleSiteObsStream DataSource { get; set; }

        /// <summary>
        /// 卫星时段统计器
        /// </summary>
        public SatTimeInfoManager SatTimeInfoManager { get; set; }
        /// <summary>
        ///是否反向计算
        /// </summary>
        public bool IsReversedDataSource { get; private set; }
        #endregion

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="path"></param>
        public void Init(string path) { 
            this.Path = path;
            this.Name = System.IO.Path.GetFileName(Path);
            Init();
            this.IsReversedDataSource = this.Option.IsReversedDataSource;
        }

        /// <summary>
        /// 初始检核
        /// </summary>
        /// <returns></returns>
        public override bool InitCheck()
        {
            foreach (var sattype in this.Option.SatelliteTypes)
            {
                var freqCount = this.DataSource.ObsInfo.GetFrequenceCount(sattype);
                if (freqCount < Option.MinFrequenceCount)
                {
                    log.Error( "验证失败： " + this.DataSource.Name + ", 频率数量为 " + freqCount + ", 计算所需要频率数量为 " + Option.MinFrequenceCount );
                    return false;
                }
            }
            
            return base.InitCheck();
        }
        
        /// <summary>
        /// 数据流
        /// </summary>
        /// <returns></returns>
        protected override BufferedStreamService<EpochInformation> BuildBufferedStream()
        {
            if (this.DataSource == null)
            {
                this.DataSource = new RinexFileObsDataSource(Path);
            }
            else { this.DataSource.Reset(); }
           
            if (this.IsReversedDataSource)
            {
                return new BufferedStreamService<EpochInformation>(new ReversedSingleSiteObsStream(DataSource), Option.BufferSize);   
            }
            return new BufferedStreamService<EpochInformation>(DataSource, Option.BufferSize);  
        } 

        /// <summary>
        /// 缓存满了
        /// </summary>
        /// <param name="obj"></param>
        protected override void OnMaterialBuffersFullOrEnd(IWindowData<EpochInformation> obj)
        { 
        } 
          
        /// <summary>
        /// 处理
        /// </summary>
        /// <param name="mEpochInfo"></param>
        public override void Process(EpochInformation mEpochInfo)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 预处理
        /// </summary>
        /// <param name="mEpochInfo"></param>
        /// <returns></returns>

        public LoopControlType PreProcess(EpochInformation mEpochInfo)
        {
            return LoopControlType.GoOn;
        }
        /// <summary>
        /// 执行前触发
        /// </summary>
        /// <param name="mEpochInfo"></param>
        /// <returns></returns>
        public LoopControlType ProducingRevise(EpochInformation mEpochInfo)
        {
            return LoopControlType.GoOn;
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        public override void Dispose()
        {
            base.Dispose(); 
        }
        /// <summary>
        /// 区别是否相等
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            var o = obj as SingleObservationStreamer; 
            return Path.Equals(o.Path);
        }
        /// <summary>
        /// 区别是否相等
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return Path.GetHashCode();
        }
    }     
}