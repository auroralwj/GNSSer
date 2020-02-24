//2016.05.02, czs, create in hongqing, 多站历元信息处理器

using System;
using System.Collections.Generic;
using System.Text;
using Geo.Algorithm;
using Geo;
using Geo.Utils;
using Geo.Common;
using Geo.Coordinates;
using Geo.Algorithm.Adjust;
using Gnsser.Domain;
using Gnsser.Service;
using Gnsser.Checkers;
using Gnsser.Data;
using Gnsser.Data.Rinex;
using Gnsser.Correction;
using Gnsser.Filter;
using Gnsser;
using System.IO;
using System.Threading.Tasks;

namespace Gnsser
{

    /// <summary>
    /// 历元信息处理器
    /// </summary>
    public class MultiSiteEpochInfoReviseManager : BaseDictionary<string, ReviserManager<EpochInformation>>, IReviser<MultiSiteEpochInfo>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="DataSourceContext"></param>
        /// <param name="Option"></param>
        public MultiSiteEpochInfoReviseManager(DataSourceContext DataSourceContext, GnssProcessOption Option)
        {
            this.DataSourceContext = DataSourceContext;
            this.Option = Option;
        }

        /// <summary>
        /// 数据上下文
        /// </summary>
        public DataSourceContext DataSourceContext { get; set; }
        /// <summary>
        /// 缓存
        /// </summary>
        public Geo.IWindowData<MultiSiteEpochInfo> Buffers { get; set; }
        /// <summary>
        /// 定位选项
        /// </summary>
        public GnssProcessOption Option { get; set; }
        /// <summary>
        /// 信息
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// 初始化
        /// </summary>
        public new virtual void Init(){
   
        }
        /// <summary>
        /// 完成
        /// </summary>
        public virtual void Complete()
        {
            foreach (var item in this)
            {
                item.Complete();
            }

        }
        /// <summary>
        /// 矫正
        /// </summary>
        /// <param name="multiSiteEpochInfo"></param>
        /// <returns></returns>
        public virtual bool Revise(ref MultiSiteEpochInfo multiSiteEpochInfo)
        {
            bool a = true;
            if (Option.GnssSolverType == Gnsser.GnssSolverType.钟差网解)//并行计算
            {
                //首先对各个单独的历元的并行矫正
                ParallelOptions option = new ParallelOptions();
                List<EpochInformation> list = new List<EpochInformation>();
                Parallel.ForEach(multiSiteEpochInfo, option, (EpochInfo, state) =>
                {
                    var val = EpochInfo;
                    var reviser = this[EpochInfo.Name];
                    if (this.Buffers != null) { reviser.Buffers = GetEpochBuffers(EpochInfo.Name); }
                    if (!reviser.Revise(ref val))
                    {
                        a = false;
                        //return false;
                    }
                    else
                        log.Debug(EpochInfo.Name+EpochInfo.ReceiverTime+"检核不通过");//list.Add(EpochInfo);
                });
            }
            else//串行计算
            {
                //首先对各个单独的历元的矫正
                foreach (var epochInfo in multiSiteEpochInfo)
                {
                    var val = epochInfo;
                    var reviser = this[epochInfo.Name];
                    //若缓存为null表示，为初始校验
                    if (this.Buffers != null ) {
                        reviser.Buffers = GetEpochBuffers(epochInfo.Name);
                    } 
                    if (!reviser.Revise(ref val))
                    {
                        return false;
                    }
                }
            }
            if (a == false)
                return false;

            //最后检查卫星是否相同
            if (Option.IsSameSatRequired)
            {
                multiSiteEpochInfo.DisableDifferSats();
            }

            return true;
        }


        /// <summary>
        /// 获取历元缓存
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public IWindowData<EpochInformation> GetEpochBuffers(string name)
        {
            IWindowData<EpochInformation> buffers = new WindowData<EpochInformation>(Buffers.Count);

            foreach (var item in Buffers)
            {
                var entity = item.Get(name);
                if (entity == null) { continue; }
                buffers.Add(entity);
            }
            return buffers;
        }

        /// <summary>
        /// 默认多文件矫正器
        /// </summary>
        /// <param name="DataSourceContext"></param>
        /// <param name="Option"></param>
        /// <param name="MultiSiteSatTimeInfoManager"></param>
        /// <returns></returns>
        public static MultiSiteEpochInfoReviseManager GetDefault(DataSourceContext DataSourceContext, GnssProcessOption Option, MultiSiteSatTimeInfoManager MultiSiteSatTimeInfoManager)
        {
            var reviser = new MultiSiteEpochInfoReviseManager(DataSourceContext, Option);
            foreach (var item in DataSourceContext.ObservationDataSources.DataSources)
            {
                var SatTimeInfoManager =  MultiSiteSatTimeInfoManager.GetOrCreate(item.Name);
                reviser[item.Name] = EpochInfoReviseManager.GetProducingReviser(DataSourceContext, Option, SatTimeInfoManager);
            }

            return reviser;
        }
        /// <summary>
        /// 默认多文件矫正器
        /// </summary>
        /// <param name="DataSourceContext"></param>
        /// <param name="Option"></param>
        /// <returns></returns>
        public static MultiSiteEpochInfoReviseManager GetDefaultRaw(DataSourceContext DataSourceContext, GnssProcessOption Option)
        {
            var reviser = new MultiSiteEpochInfoReviseManager(DataSourceContext, Option);
            foreach (var item in DataSourceContext.ObservationDataSources.DataSources)
            {
                reviser[item.Name] = EpochInfoReviseManager.GetFirstStepEpochInfoReviser(DataSourceContext, Option);
            }

            return reviser;
        }

        /// <summary>
        /// 写周跳探测结果到文件
        /// </summary>
        /// <param name="OutputDirectory"></param>
        public void WriteStorageToFile(string OutputDirectory)
        {
            foreach (var key in this.Keys)
            {
                var reviser = this[key].GetReviser<CycleSlipDetectReviser>();
                if (reviser == null) { log.Info("没有启用周跳探测功能，将不输出周跳探测文件。"); return; }
                
               reviser.WriteStorageToFile(OutputDirectory, key, Option.ObsDataType );
            }
        } 
    }
}
