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
    public class IgsEphemerisSourceProvider  : AbstractIgsProductSourceProvider<Sp3File, CombinedSp3FileEphService>
    {
        /// <summary>
        /// 日志记录。错误信息记录在日志里面。
        /// </summary> 
       new  protected Log log = new Log(typeof(IgsEphemerisSourceProvider));

        /// <summary>
        /// 多系统数据源服务
        /// </summary>
       public IgsEphemerisSourceProvider(IgsProductSourceOption opt,
           IgsProductType IgsProductSourceType, int TimeIntervalSeconds = 86400)
            :base(opt,   IgsProductSourceType,   TimeIntervalSeconds)
       { 
       } 

        #region abstract methods 
        /// <summary>
        /// 返回服务
        /// </summary>
        /// <returns></returns>
        public override CombinedSp3FileEphService GetDataSourceService()
        {
            var data = BuildMultiSysServices();
            if(data == null || data.Count == 0 )
            {
                return null;
            }
            SatEphemerisCollection source = new SatEphemerisCollection(data, Option.IsUniqueSource, Option.IndicatedSourceCode);
            CombinedSp3FileEphService service = new CombinedSp3FileEphService(source, Option.MinSequentialSatCount, Option.Sp3EphMaxBreakingCount, Option.InterpolateOrder);
           
            return service;
        } 
        /// <summary>
        /// 读取文件
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        protected override Sp3File LoadFile(string filePath)
        {
            return new Data.Rinex.Sp3Reader(filePath).ReadAll();
        }
        #endregion
    }
}