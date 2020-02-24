//2015.05.11, czs, create in namu, 卫星系统相关的文件选项

using System;
using Geo; 
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Geo.Common;


namespace Gnsser 
{
    /// <summary>
    /// 卫星系统相关的文件选项
    /// </summary>
    public class SatFileOption : FileOption
    {
        #region 构造函数
        /// <summary>
        /// 构造函数。输入名称。
        /// </summary>
        /// <param name="FilePath">单文件</param>
        /// <param name="name">名称</param>
        /// <param name="SatelliteType">系统名称</param>
        public SatFileOption(SatelliteType SatelliteType, string FilePath, string name = "单文件配置")
            : base(FilePath, name) { this.SatelliteType = SatelliteType; }

        /// <summary>
        /// 构造函数。输入名称。顺序与输出一致。
        /// </summary>
        /// <param name="FilePathes">集合文件</param>
        /// <param name="name">名称</param>
        /// <param name="SatelliteType">系统名称</param>
        public SatFileOption(SatelliteType SatelliteType, string[] FilePathes, string name = "集合文件配置")
            : base(FilePathes, name) { this.SatelliteType = SatelliteType; }

        /// <summary>
        /// 构造函数。输入名称。顺序与输出一致。
        /// </summary>
        /// <param name="SatelliteType">类型</param>
        /// <param name="FilePathes">集合文件</param>
        /// <param name="name">系统名称</param>
        public SatFileOption(SatelliteType SatelliteType, List<string> FilePathes, string name = "集合文件配置")
            : base(FilePathes, name) { this.SatelliteType = SatelliteType; }
        #endregion

        /// <summary>
        /// 卫星类型
        /// </summary>
        public SatelliteType SatelliteType { get; set; }

        
    }
}
