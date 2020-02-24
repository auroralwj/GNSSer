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
    public class DoubleDifferParamWriter : LineFileWriter<DoubleDifferParam>
    {       /// <summary>
        /// 默认构造函数
        /// </summary>
        public DoubleDifferParamWriter() { }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="metaFilePath"></param>
        public DoubleDifferParamWriter(string filePath, string metaFilePath = null)
            : base(filePath, metaFilePath)
        {
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="Gmetadata"></param>
        public DoubleDifferParamWriter(string filePath, Gmetadata Gmetadata)
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
        //        Geo.Utils.ObjectUtil.GetPropertyName<DoubleDifferParam>(m=>m.InputPath),
        //        Geo.Utils.ObjectUtil.GetPropertyName<DoubleDifferParam>(m=>m.OutputPath),
        //        Geo.Utils.ObjectUtil.GetPropertyName<DoubleDifferParam>(m=>m.IsOverwrite),
        //        Geo.Utils.ObjectUtil.GetPropertyName<DoubleDifferParam>(m=>m.IsParallel),
        //        Geo.Utils.ObjectUtil.GetPropertyName<DoubleDifferParam>(m=>m.ParallelProcessCount),
        //        Geo.Utils.ObjectUtil.GetPropertyName<DoubleDifferParam>(m=>m.EphemerisPath),
        //        Geo.Utils.ObjectUtil.GetPropertyName<DoubleDifferParam>(m=>m.ClockPath),
        //    };
        //    return satData;
        //}

        public override void Write(DoubleDifferParam row)
        {
            row.InputPath = Geo.Utils.PathUtil.GetRelativePath(row.InputPath, this.BaseDirectory);
            row.OutputPath = Geo.Utils.PathUtil.GetRelativePath(row.OutputPath, this.BaseDirectory);
            row.EphemerisPath = Geo.Utils.PathUtil.GetRelativePath(row.EphemerisPath, this.BaseDirectory);
            row.ClockPath = Geo.Utils.PathUtil.GetRelativePath(row.ClockPath, this.BaseDirectory);
            row.SiteInfoPath = Geo.Utils.PathUtil.GetRelativePath(row.SiteInfoPath, this.BaseDirectory);
            row.BaselinePath = Geo.Utils.PathUtil.GetRelativePath(row.BaselinePath, this.BaseDirectory);

            base.Write(row); 
        }
         
    }
}
