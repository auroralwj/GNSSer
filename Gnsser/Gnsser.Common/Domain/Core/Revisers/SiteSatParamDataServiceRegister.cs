//2018.12.28, czs, create in ryd,  注册时段参数服务

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

namespace Gnsser
{
    /// <summary>
    /// 测站卫星注册时段服务
    /// </summary>
    public class SiteSatParamDataServiceRegister : EpochInfoReviser
    {
        /// <summary>
        /// 构造函数
        /// </summary> 
        public SiteSatParamDataServiceRegister(SiteSatPeriodDataService SiteSatAppearenceService, double BreakCount, bool IsOutputPeriodData, string OutputDirectory)
        {
            this.Name = "注册时段参数服务";
            log.Info("启用 " + this.Name);
            this.SiteSatAppearenceService = SiteSatAppearenceService;
            this.IsOutputPeriodData = IsOutputPeriodData;
            this.BreakCount = BreakCount;
            this.OutputDirectory = OutputDirectory;
        }
        SiteSatPeriodDataService SiteSatAppearenceService { get; set; }
        double BreakCount { get; set; }
        /// <summary>
        /// 输出目录
        /// </summary>
        public string OutputDirectory { get; set; }
        /// <summary>
        /// 是否输出周期数据
        /// </summary>
        public bool IsOutputPeriodData { get; set; }
        /// <summary>
        /// 修正
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Revise(ref EpochInformation obj)
        {
           var service =  SiteSatAppearenceService.GetOrCreate(obj.SiteName);
            service.MaxGapSecond = obj.ObsInfo.Interval * BreakCount;
            service.Regist(obj);

            var buffer = this.Buffers;

            return true;
        }
        public override void Complete()
        {
            base.Complete();
            if (IsOutputPeriodData)
            {
               SiteSatAppearenceService.BuildTableObjectManager(OutputDirectory).WriteAllToFileAndClearBuffer();
            }

        }
    }
}
