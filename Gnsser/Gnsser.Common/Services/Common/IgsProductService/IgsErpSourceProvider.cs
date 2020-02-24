//2018.05.02, czs, create in hmx, 根据设置和需求，自动获取ERP服务

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
    ///根据设置和需求，自动获取ERP服务
    /// </summary> 
    public class IgsErpSourceProvider : AbstractIgsProductSourceProvider<ErpFile, FileErpService>
    {
        /// <summary>
        /// 日志记录。错误信息记录在日志里面。
        /// </summary> 
       new  protected Log log = new Log(typeof(IgsErpSourceProvider));

        /// <summary>
        /// 多系统数据源服务
        /// </summary>
       public IgsErpSourceProvider(IgsProductSourceOption opt, IgsProductType IgsProductSourceType, int TimeIntervalSeconds = 604800)
            :base(opt,   IgsProductSourceType,   TimeIntervalSeconds)
       {
            opt.SatelliteTypes = new List<SatelliteType> { SatelliteType.U };
       } 

        #region abstract methods 
        /// <summary>
        /// 返回服务
        /// </summary>
        /// <returns></returns>
        public override FileErpService GetDataSourceService()
        {
            var data = BuildMultiSysServices();
            if(data.Count == 0) { return null; }

            var first = data.FirstOrDefault();
            if(first.Value == null || first.Value.Count == 0) { return null; }
           

            var list = first.Value;
             var file = list.FirstOrDefault();
            if(file.Value == null || file.Value.Count == 0) { return null; }


            var service = new FileErpService(file.Value[0]) ;

            return service;
        } 
        /// <summary>
        /// 读取文件
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        protected override ErpFile LoadFile(string filePath)
        {
            return new ErpFileReader(filePath).Read();
        }
        #endregion
    }
}