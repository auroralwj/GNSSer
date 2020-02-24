//2018.03.16, czs, create in hmx, 根据输入设置，提供连续的多系统的星历服务
//2018.03.17, czs, edit in hmx, 提取为接口同时为钟差和星历服务

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
    public class IgsGridIonoSourceProvider : AbstractIgsProductSourceProvider<IonoFile, IGridIonoFileService>
    {
        /// <summary>
        /// 日志记录。错误信息记录在日志里面。
        /// </summary> 
       new  protected Log log = new Log(typeof(IgsEphemerisSourceProvider));

        /// <summary>
        /// 多系统数据源服务
        /// </summary>
       public IgsGridIonoSourceProvider(IgsProductSourceOption opt, IgsProductType IgsProductSourceType, int TimeIntervalSeconds = 86400)
            :base(opt,   IgsProductSourceType,   TimeIntervalSeconds)
       {   
       } 

        #region abstract methods 
        /// <summary>
        /// 返回服务
        /// </summary>
        /// <returns></returns>
        public override IGridIonoFileService GetDataSourceService()
        { 
            var data = BuildMultiSysServices();
            var first = data.FirstOrDefault();
            if (first.Value == null) { return null; }

            var list = first.Value;
            var file = list.FirstOrDefault();
            if (file.Value == null || file.Value.Count == 0) { return null; }
            //可以把两个都读入服务中

            var service = new GridIonoFileService(file.Value[0]);

            return service;
        } 
        /// <summary>
        /// 读取文件
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        protected override IonoFile LoadFile(string filePath)
        {
            return new IonoReader(filePath).ReadAll(this.Option.IsSkipIonoContent);
        }

        /// <summary>
        /// 构建文件缓存关键字
        /// </summary>
        /// <param name="filePath"></param>
        protected override string BuildFileBufferKey(string filePath)
        { 
            return Path.GetFileName(filePath).ToLower() +　"_" + this.Option.IsSkipIonoContent;
        }
        #endregion
    }
}