// 2014.09.19, czs, create, in hailutu, 数据源统一设计

using System;
using System.Collections.Generic;
using System.Text;
using Geo.Algorithm;  
using Geo.Common;
using Geo.Coordinates;
using Geo.Algorithm.Adjust; 
using Gnsser.Data.Rinex;
using Gnsser.Service;
using Geo.Utils; 
using Gnsser.Data;
using Geo;

namespace Gnsser.Data
{  
    /// <summary>
    /// 卫星状态数据源。
    /// </summary>
    public class SatInfoService : GnssFileService<String>
    {
        /// <summary>
        /// 卫星状态数据源
        /// </summary>
        /// <param name="filePath">文件路径</param>
        public SatInfoService(FileOption filePath) :
            base(filePath)
        {
            if (filePath != null)
                this.SatInfoFile = new SatInfoReader(filePath.FilePath).Read();
        }
        /// <summary>
        /// 卫星状态数据源
        /// </summary>
        /// <param name="filePath">文件路径</param>
        public SatInfoService(string filePath) :
            base(filePath)
        {
            if (filePath != null)
                this.SatInfoFile = new SatInfoReader(filePath).Read();
        }
        /// <summary>
        /// 卫星状态信息。
        /// </summary>
        public SatInfoFile SatInfoFile { get; protected set; }  
    }
}
