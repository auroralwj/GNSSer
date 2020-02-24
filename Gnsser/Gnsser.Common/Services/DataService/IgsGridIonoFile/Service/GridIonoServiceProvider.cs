//2017.08.17, czs, create in hongqing, 电离层服务提供者 , IONO文件服务池
//2018.05.02, czs, create in hmx, 采用与钟差相同的处理策略，删除以往按日组织方式。

using System;
using Gnsser.Times;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gnsser.Data.Rinex;
using Gnsser;
using Geo.Coordinates;
using Geo.Referencing;
using Geo.Algorithm;
using Gnsser.Service;
using Geo.Times;
using Geo;
using Gnsser.Data;

namespace Gnsser.Service
{ 
    
    /// <summary>
    /// 电离层服务提供者
    /// </summary> 
    public class GridIonoServiceProvider :  AbstractIgsProductSourceProvider<IonoFile, GridIonoFileService>
    {
        /// <summary>
        /// 电离层服务提供者
        /// </summary>
        public GridIonoServiceProvider(IgsProductSourceOption opt)
            : base(opt, IgsProductType.I)
        {
        }
         
        /// <summary>
        /// 返回服务
        /// </summary>
        /// <returns></returns>
        public override GridIonoFileService GetDataSourceService()
        {
            var data = BuildMultiSysServices();
            var first = data.FirstOrDefault();
            if (first.Value == null) { return null; }

            var list = first.Value;
            var file = list.FirstOrDefault();
            if (file.Value == null || file.Value.Count == 0) { return null; }


            var service = new GridIonoFileService(file.Value[0]);

            return service;
        }
        /// <summary>
        /// 加载文件
        /// </summary>
        /// <param name="localPath"></param>
        /// <returns></returns>
        protected override IonoFile LoadFile(string localPath)
        { 
            return new IonoReader(localPath).ReadAll();
        }
    }
     
}
