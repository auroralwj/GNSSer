//2015.10.24, czs, create in 彭州, 单点定位

using System;
using System.IO;
using Geo.Common;
using Geo.Coordinates;
using System.Collections;
using System.Collections.Generic;
using Geo.IO;

namespace Gnsser.Api
{
    /// <summary>
    ///解压的参数文件写入
    /// </summary>
    public class PointPositionParamWriter : LineFileWriter<PointPositionParam>
    {       /// <summary>
        /// 默认构造函数
        /// </summary>
        public PointPositionParamWriter() { }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="metaFilePath"></param>
        public PointPositionParamWriter(string filePath, string metaFilePath = null)
            : base(filePath, metaFilePath)
        {
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="Gmetadata"></param>
        public PointPositionParamWriter(string filePath, Gmetadata Gmetadata)
            : base(filePath, Gmetadata)
        {
        }

        ///// <summary>
        ///// 默认的元数据
        ///// </summary>
        ///// <returns></returns>
        //public override Gmetadata GetDefaultMetadata()
        //{
        //    Gmetadata satData = Gmetadata.NewInstance;
        //    satData.PropertyNames = new string[] { 
        //        Geo.Utils.ObjectUtil.GetPropertyName<PointPositionParam>(m=>m.InputPath),
        //        Geo.Utils.ObjectUtil.GetPropertyName<PointPositionParam>(m=>m.OutputPath),
        //        Geo.Utils.ObjectUtil.GetPropertyName<PointPositionParam>(m=>m.IsOverwrite),
        //        Geo.Utils.ObjectUtil.GetPropertyName<PointPositionParam>(m=>m.IsParallel),
        //        Geo.Utils.ObjectUtil.GetPropertyName<PointPositionParam>(m=>m.ParallelProcessCount),
        //        Geo.Utils.ObjectUtil.GetPropertyName<PointPositionParam>(m=>m.EphemerisPath),
        //        Geo.Utils.ObjectUtil.GetPropertyName<PointPositionParam>(m=>m.ClockPath),
        //    };
        //    return satData;
        //}
         
    }
}
