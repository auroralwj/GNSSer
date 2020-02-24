//2018.03.16, czs, create in hmx, 根据输入设置，提供连续的多系统的星历服务
//2018.03.17, czs, edit in hongqing, 提取为接口同时为钟差和星历服务

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Geo;
using Geo.Common;
using Gnsser.Service;
using Gnsser.Times;
using Geo.IO;
using System.IO;
using Gnsser.Data.Rinex;
using Gnsser.Data;
using Geo.Times;

namespace Gnsser.Data
{

    /// <summary>
    /// 根据输入设置，提供连续的多系统的星历服务
    /// </summary> 
    public class IgsSatSimpleClockSourceProvider : AbstractIgsProductSourceProvider<SimpleClockFile, SimpelCombinedSatClockService>
    {
        /// <summary>
        /// 日志记录。错误信息记录在日志里面。
        /// </summary> 
       new  protected Log log = new Log(typeof(IgsSatSimpleClockSourceProvider));

        /// <summary>
        /// 多系统数据源服务
        /// </summary>
       public IgsSatSimpleClockSourceProvider(IgsProductSourceOption opt, IgsProductType IgsProductSourceType, int TimeIntervalSeconds = 86400)
            :base(opt,   IgsProductSourceType,   TimeIntervalSeconds)
       {
            IsSkipSite = true;
       }

        /// <summary>
        /// 是否略过测站钟差，这样可以节约内存
        /// </summary>
        public bool IsSkipSite { get; set; }
        #region abstract methods 
        /// <summary>
        /// 返回服务
        /// </summary>
        /// <returns></returns>
        public override SimpelCombinedSatClockService GetDataSourceService()
        {
            var data = BuildMultiSysServices();
            SatSimpleClockCollection source = new SatSimpleClockCollection(data, Option.IsUniqueSource, Option.IndicatedSourceCode);
            SimpelCombinedSatClockService service = new SimpelCombinedSatClockService(source);
            return service;
        } 
        /// <summary>
        /// 读取文件
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        protected override SimpleClockFile LoadFile(string filePath)
        {
            return new Data.Rinex.SimpleClockFileReader(filePath, IsSkipSite).ReadAll();
        }
        #endregion
    }





    /// <summary>
    /// 根据输入设置，提供连续的多系统的星历服务
    /// </summary> 
    public class IgsSatClockSourceProvider : AbstractIgsProductSourceProvider<ClockFile, CombinedSatClockService>
    {
        /// <summary>
        /// 日志记录。错误信息记录在日志里面。
        /// </summary> 
       new  protected Log log = new Log(typeof(IgsEphemerisSourceProvider));

        /// <summary>
        /// 多系统数据源服务
        /// </summary>
       public IgsSatClockSourceProvider(IgsProductSourceOption opt, IgsProductType IgsProductSourceType, int TimeIntervalSeconds = 86400)
            :base(opt,   IgsProductSourceType,   TimeIntervalSeconds)
       {
            IsSkipSite = true;
       }

        /// <summary>
        /// 是否略过测站钟差，这样可以节约内存
        /// </summary>
        public bool IsSkipSite { get; set; }
        #region abstract methods 
        /// <summary>
        /// 返回服务
        /// </summary>
        /// <returns></returns>
        public override CombinedSatClockService GetDataSourceService()
        {
            var data = BuildMultiSysServices();
            SatClockCollection source = new SatClockCollection(data, Option.IsUniqueSource, Option.IndicatedSourceCode);
            CombinedSatClockService service = new CombinedSatClockService(source);
            return service;
        } 
        /// <summary>
        /// 读取文件
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        protected override ClockFile LoadFile(string filePath)
        {
            return new Data.Rinex.ClockFileReader(filePath, IsSkipSite).ReadAll();
        }
        #endregion
    }


}