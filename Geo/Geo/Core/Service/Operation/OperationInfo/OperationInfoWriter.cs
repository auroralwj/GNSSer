//2015.10.20, czs, create in xi'an hongqing, 操作信息数据文件的写入

using System;
using System.IO;
using Geo.IO;
using Geo.Common;
using Geo.Coordinates;
using System.Collections;
using System.Collections.Generic;

namespace Geo
{
    /// <summary>
    ///操作信息数据文件的写入
    /// </summary>
    public class OperationInfoWriter :   LineFileWriter<OperationInfo>, IBaseDirecory
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="gofFilePath"></param>
        /// <param name="metaFilePath"></param>
        public OperationInfoWriter(string gofFilePath, string metaFilePath = null) : base(gofFilePath, metaFilePath)
        {
            //默认路径是 ProjectDirectory/Script/GofFileName.gof
            this.BaseDirectory = (Path.GetDirectoryName(gofFilePath));
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="gofFilePath"></param>
        /// <param name="Gmetadata"></param>
        public OperationInfoWriter(string gofFilePath, Gmetadata Gmetadata)
            : base(gofFilePath, Gmetadata)
        {
            //默认路径是 ProjectDirectory/Script/GofFileName.gof
            this.BaseDirectory = (Path.GetDirectoryName(gofFilePath));
        }
        /// <summary>
        /// 构造函数，以数据流初始化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="Gmetadata"></param>
        public OperationInfoWriter(Stream stream, Gmetadata Gmetadata)
            : base(stream, Gmetadata)
        { 
        }

        /// <summary>
        /// 写入
        /// </summary>
        /// <param name="obj"></param>
        public override void Write(OperationInfo obj)
        {
            //保存为相对路径
            obj.ParamFilePath = Geo.Utils.PathUtil.GetRelativePath(obj.ParamFilePath, this.BaseDirectory);

            base.Write(obj);
        }
    }
}
