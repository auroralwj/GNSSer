//2016.08.20, czs, create in 福建永安, 宽项计算器，多站观测数据遍历器
//2016.08.29, czs, edit in 西安洪庆, 重构多站观测数据遍历器

using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using Geo;
using Geo.Algorithm;
using Geo.Coordinates;
using Geo.Algorithm.Adjust;
using Geo.Algorithm;
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
    /// 多站观测数据遍历器
    /// </summary>
    public abstract class MultiSiteObsStreamer : ObsFileAdjustStreamer<MultiSiteEpochInfo, SimpleGnssResult>, IObsFileProcessStreamer<MultiSiteEpochInfo>
    {
        protected  ILog log = new Log(typeof(MultiSiteObsStreamer));
        /// <summary>
        /// 构造函数
        /// </summary> 
        /// <param name="OutputDirectory"></param>
        public MultiSiteObsStreamer( string OutputDirectory)
        {
            this.OutputDirectory = OutputDirectory;
            InputFileManager = new Geo.IO.InputFileManager();
        } 
        #region 属性
        /// <summary>
        /// 路径
        /// </summary>
        public string[] Pathes { get; set; }
        /// <summary>
        /// 数据源
        /// </summary>
        public MultiSiteObsStream DataSource { get; set; }
        /// <summary>
        /// 多站卫星出现时间管理器
        /// </summary>
        public MultiSiteSatTimeInfoManager MultiSiteSatTimeInfoManager { get; set; }
        #endregion



        #region 方法

        #region 主运行方法       

        /// <summary>
        /// 运行前调用
        /// </summary>
        public virtual void Init(string[] pathes)
        { 
            this.Pathes = pathes;
            this.Name = "MultiSiteOf";
            int i = 0;
            foreach (var item in Pathes)
            {
                if (i > 3) { break; }
                this.Name += Path.GetFileName(item);
            }
            Init();
        }
        /// <summary>
        /// 初始化
        /// </summary>
        public override void Init()
        {
            BuildGnssOption();
            base.Init();
            IsOutputResult = this.Option.IsOutputResult;
        }
        /// <summary>
        /// 原料第一次进入
        /// </summary>
        /// <param name="material"></param>
        protected override void OnAfterMaterialCheckPassed(MultiSiteEpochInfo material)
        {
            //数据第一次进入，需要进行统计，并且做初步的星历赋值检核、周跳探测等。
            MultiSiteSatTimeInfoManager.Record(material);

            base.OnAfterMaterialCheckPassed(material);
        }
        #endregion 
         
        #region 各种生成
        /// <summary>
        /// 生成解算选项
        /// </summary>
        /// <returns></returns>
        protected override GnssProcessOption BuildGnssOption()
        {
            if (Option == null) { Option = new GnssProcessOption(); } 
            //Option.OutputDirectory = this.OutputDirectory; 
            return Option;
        }

        /// <summary>
        /// 构建检核器
        /// </summary>
        /// <returns></returns>
        protected override IChecker<MultiSiteEpochInfo> BuildChecker()
        {
            return MultiSiteEpochCheckingManager.GetDefault(Context, Option);
        }

        /// <summary>
        /// 初始校验器
        /// </summary>
        /// <returns></returns>
        protected override IReviser<MultiSiteEpochInfo> BuildRawReviser()
        {
            return MultiSiteEpochInfoReviseManager.GetDefaultRaw(Context, Option);
        }
         
        /// <summary>
        /// 构建矫正器
        /// </summary>
        /// <returns></returns>
        protected override IReviser<MultiSiteEpochInfo> BuildProducingReviser()
        {
            MultiSiteSatTimeInfoManager = new Gnsser.MultiSiteSatTimeInfoManager(DataSource.BaseDataSource.ObsInfo.Interval);

            return MultiSiteEpochInfoReviseManager.GetDefault(Context, Option, MultiSiteSatTimeInfoManager);
        } 
        /// <summary>
        /// 数据流
        /// </summary>
        /// <returns></returns>
        protected override BufferedStreamService<MultiSiteEpochInfo> BuildBufferedStream()
        {
            //if (this.DataSource == null)
            //{
                this.DataSource = BuildDataSource();
            //}
            //else {
            //    this.DataSource.Reset();
            //}

            if (this.IsReversedDataSource)
            {
                return new BufferedStreamService<MultiSiteEpochInfo>(new ReversedMultiSiteObsStream(DataSource), Option.BufferSize);
            }
            return new BufferedStreamService<MultiSiteEpochInfo>(DataSource, Option.BufferSize);
        }

        /// <summary>
        /// 数据源加载
        /// </summary> 
        /// <returns></returns>
        protected virtual MultiSiteObsStream BuildDataSource()
        {
            var oPathes = InputFileManager.GetLocalFilePathes(Pathes, RunnerFileExtension, "*.*");
            var DataSource = new MultiSiteObsStream(oPathes, Option.BaseSiteSelectType, Option.IsSameSatRequired, this.Option.IndicatedBaseSiteName);
            DataSource.IsAllowMissingEpochSite = Option.IsAllowMissingEpochSite; 


            return DataSource;
        }
        #endregion
        #endregion

        /// <summary>
        /// 区别是否相等
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            var o = obj as MultiSiteObsStreamer;
            return  Pathes.Equals(o.Pathes);
        }
        /// <summary>
        /// 区别是否相等
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return Pathes.GetHashCode();
        }

    }

}