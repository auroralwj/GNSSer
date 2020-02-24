//2016.08.20, czs, create in 福建永安, 宽项计算器，多站观测数据遍历器
//2016.08.29, czs, edit in 西安洪庆, 重构多站观测数据遍历器
//2018.08.15, czs, edit in hmx, 更名为SingleSiteObsAdjustStreamer，因为其具有SimpleGnssResult

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
    public abstract class SingleSiteObsAdjustStreamer : ObsFileAdjustStreamer<EpochInformation, SimpleGnssResult>, IObsFileProcessStreamer<EpochInformation>
    {
        protected ILog log = new Log(typeof(SingleSiteObsAdjustStreamer));
        /// <summary>
        /// 构造函数
        /// </summary> 
        /// <param name="outputDirectory"></param>
        public SingleSiteObsAdjustStreamer(string outputDirectory = null)
        {
            if(String.IsNullOrWhiteSpace(  outputDirectory )) { outputDirectory = Setting.TempDirectory; }
            this.OutputDirectory = outputDirectory;
        }

        #region 属性
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
        #endregion

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="path"></param>
        public void Init(string path) { 
            this.Path = path;
            this.Name = System.IO.Path.GetFileName(Path);
            Init();

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
        /// 原料第一次进入
        /// </summary>
        /// <param name="material"></param>
        protected override void OnAfterMaterialCheckPassed(EpochInformation material)
        {
            //数据第一次进入，需要进行统计，并且做初步的星历赋值检核、周跳探测等。
            SatTimeInfoManager.Record(material); 
            base.OnAfterMaterialCheckPassed(material);
        }
         
         /// <summary>
         /// 初始矫正器
         /// </summary>
         /// <returns></returns>
        protected override IReviser<EpochInformation> BuildRawReviser()
        {
            return EpochInfoReviseManager.GetFirstStepEpochInfoReviser(Context, Option);
        }
        /// <summary>
        /// 检核器
        /// </summary>
        /// <returns></returns>
        protected override IChecker<EpochInformation> BuildChecker()
        {
            return EpochCheckingManager.GetDefaultCheckers(Context, Option);         
        }

        /// <summary>
        /// 矫正器，此处只需进行模型改正了。
        /// </summary>
        /// <returns></returns>
        protected override IReviser<EpochInformation> BuildProducingReviser()
        {
            SatTimeInfoManager = new SatTimeInfoManager(DataSource.ObsInfo.Interval); 

            return EpochInfoReviseManager.GetProducingReviser(Context, Option,SatTimeInfoManager);
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
            var o = obj as SingleSiteObsAdjustStreamer; 
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