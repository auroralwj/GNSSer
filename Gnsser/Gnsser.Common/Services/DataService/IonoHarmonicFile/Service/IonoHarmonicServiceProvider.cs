//2018.05.26, czs, create in HMX, CODE电离层球谐函数

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
    public class IonoHarmonicFileProvider :  AbstractIgsProductSourceProvider<IonoHarmonicFile, IonoHarmonicFileService>
    {
        /// <summary>
        /// 电离层服务提供者
        /// </summary>
        public IonoHarmonicFileProvider(IgsProductSourceOption opt)
            : base(opt, IgsProductType.ION)
        {
        }
         
        /// <summary>
        /// 返回服务
        /// </summary>
        /// <returns></returns>
        public override IonoHarmonicFileService GetDataSourceService()
        {
            var data = BuildMultiSysServices();
            var first = data.FirstOrDefault();
            if (first.Value == null) { return null; }

            var list = first.Value;
            var file = list.FirstOrDefault();
            if (file.Value == null || file.Value.Count == 0) { return null; }


            var service = new IonoHarmonicFileService(file.Value[0]);

            return service;
        }
        /// <summary>
        /// 加载文件
        /// </summary>
        /// <param name="localPath"></param>
        /// <returns></returns>
        protected override IonoHarmonicFile LoadFile(string localPath)
        { 
            return new IonoHarmonicReader(localPath).ReadAll();
        }
    }
     
}
