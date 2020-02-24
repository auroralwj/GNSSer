//2016.08.20, czs, create in 福建永安, 宽项计算器，多站观测数据遍历器
//2016.08.29, czs, edit in 西安洪庆, 重构多站观测数据遍历器
//2016.11.19，czs, refact in hongqing, 提取更通用的观测文件数据流
//2018.07.29, czs, edit in HMX, 修改通用GNSS数据流执行器, 提取 近似坐标设置器 


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
    /// 近似坐标设置器。
    /// </summary>
    public class ApproxCoordSetter<T>
    {
        Log log = new Log(typeof(ApproxCoordSetter<T>));
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="BufferedStream"></param>
        /// <param name="Option"></param>
        /// <param name="Context"></param>
        public ApproxCoordSetter(BufferedStreamService<T> BufferedStream, GnssProcessOption Option, DataSourceContext Context)
        {
            this.BufferedStream = BufferedStream;
            this.Option = Option;
            this.Context = Context;
        }

        /// <summary>
        /// 数据上下文
        /// </summary>
        public DataSourceContext Context { get; set; }

        /// <summary>
        /// 数据流
        /// </summary>
        public BufferedStreamService<T> BufferedStream { get; set; }
        /// <summary>
        /// 设置
        /// </summary>
        public GnssProcessOption Option { get; set; }

        #region 设置测站信息

        /// <summary>
        /// 检查和设置测站信息
        /// </summary>
        public void CheckOrUpdateStationInfo()
        {
            if (this.Option.IsUpdateStationInfo && this.Context.StaionInfoService != null)
            {
                if (this.BufferedStream.DataSource is RinexFileObsDataSource)
                {
                    var source = this.BufferedStream.DataSource as RinexFileObsDataSource;
                    TryUpdateStationInfo(source);
                }
                if (this.BufferedStream.DataSource is MultiSiteObsStream)
                {
                    var sources = this.BufferedStream.DataSource as MultiSiteObsStream;
                    foreach (var source in sources.DataSources)
                    {
                        TryUpdateStationInfo(source);
                    }
                }
            }
        }

        /// <summary>
        /// 通过服务设置坐标
        /// </summary>
        /// <param name="source"></param>
        private void TryUpdateStationInfo(ISingleSiteObsStream source)
        {
            if (Context.StaionInfoService == null) { return; }
            var info = Context.StaionInfoService.Get(source.Name.Substring(0, 4), source.ObsInfo.StartTime);
            if (info != null)
            {
                log.Info(Context.StaionInfoService.Name + " 获取到" + source.Name.Substring(0, 4) + "信息" + info);
                source.SiteInfo.Hen = info.AntHEN;
                source.SiteInfo.AntennaType = info.AntennaType;
                source.SiteInfo.AntennaNumber = info.AntennaNumber;
                source.SiteInfo.ReceiverType = info.ReceiverType;
                source.SiteInfo.ReceiverNumber = info.ReceiverNumber;
            }
            else
            {
                log.Warn(Context.StaionInfoService.Name + " 获取 " + source.Name + " 信息失败，将采用RINEX文件默认测站信息");
            }
        }

        #endregion

        /// <summary>
        /// 检查并设置近似坐标
        /// </summary>
        public void CheckOrSetApproxXyz()
        {
            //坐标赋值
            if (!this.Option.IsIndicatingApproxXyz && this.Option.IsApproxXyzRequired && this.Option.IsSetApproxXyzWithCoordService)
            {
                if (this.BufferedStream.DataSource is RinexFileObsDataSource)
                {
                    var source = this.BufferedStream.DataSource as RinexFileObsDataSource;
                    TrySetApproxXyzValue(source);
                }
                if (this.BufferedStream.DataSource is MultiSiteObsStream)
                {
                    var sources = this.BufferedStream.DataSource as MultiSiteObsStream;
                    foreach (var source in sources.DataSources)
                    {
                        TrySetApproxXyzValue(source);
                    }
                }
            }
        }
        /// <summary>
        /// 通过服务设置坐标
        /// </summary>
        /// <param name="source"></param>
        public void TrySetApproxXyzValue(ISingleSiteObsStream source)
        {
            if (Context.SiteCoordService == null || ! Option.IsSiteCoordServiceRequired) { return; }
            var xyz = Context.SiteCoordService.Get(source.SiteInfo.MarkerNumber, source.ObsInfo.StartTime);
            if (xyz != null)
            {
                log.Info(Context.SiteCoordService.Name + " 获取到" + source.Name + "坐标" + xyz);
                source.SiteInfo.SetApproxXyz(xyz.Value);
                source.SiteInfo.EstimatedXyz = (xyz.Value);
                source.SiteInfo.EstimatedXyzRms = xyz.Rms;
            }
        }
        /// <summary>
        /// 检查数据源初始坐标是否合法，如果否，则采用伪距定位设置之。
        /// </summary>
        public void CheckOrSetDatasourceApproxXyz()
        {
            //检查数据源是否需要初始坐标
            if (Option.IsApproxXyzRequired)
            {
                if (this.BufferedStream.DataSource is RinexFileObsDataSource || (this.BufferedStream.DataSource is MemoRinexFileObsDataSource))
                {
                    var source = this.BufferedStream.DataSource as ISingleSiteObsStream;
                    if (this.Option.IsIndicatingApproxXyz)
                    {
                        source.SiteInfo.SetApproxXyz(this.Option.InitApproxXyz);
                    }
                    else
                    {
                        CheckAndSetApproxXyz(source);
                    }
                }
                else if (this.BufferedStream.DataSource is MultiSiteObsStream)
                {
                    var sources = this.BufferedStream.DataSource as MultiSiteObsStream;
                    foreach (var source in sources.DataSources)
                    {
                        CheckAndSetApproxXyz(source);
                    }
                }
            }
        }
        /// <summary>
        /// 检查数据源初始坐标是否合法，如果否，则采用伪距定位设置之。
        /// </summary>
        /// <param name="source"></param>
        private void CheckAndSetApproxXyz(ISingleSiteObsStream source)
        {
            //若为0，或地心长度小于 MinAllowedApproxXyzLen，重新计算。
            if (XYZ.IsZeroOrEmpty(source.SiteInfo.ApproxXyz) || source.SiteInfo.ApproxXyz.Length < this.Option.MinAllowedApproxXyzLen)
            {
                source.SiteInfo.SetApproxXyz(new XYZ());

                log.Error(source.Name + " 没有发现有效的初始坐标，先置 0， 将用伪距定位赋予初值。");
                var result = SimpleRangePositioner.GetApproxPosition(Context);
                if (result == null) { log.Error(source.Name + " 初始定位失败！"); return; }
                var xyz = result.EstimatedXyz;
                source.SiteInfo.SetApproxXyz(xyz);
                log.Error(source.Name + " 初始坐标设置为 " + xyz);
            }
        }


    }
     

}