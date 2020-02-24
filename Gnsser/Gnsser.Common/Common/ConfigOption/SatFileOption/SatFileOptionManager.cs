//2015.05.11, czs, create in namu, 卫星系统相关文件管理器

using System;
using Geo; 
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Geo.Common;


namespace Gnsser
{
    /// <summary>
    /// 卫星系统相关文件管理器
    /// </summary>
    public class SatFileOptionManager : BaseDictionary<SatelliteType, SatFileOption>, IManager<SatFileOption>
    {
        /// <summary>
        ///构造函数
        /// </summary>
        public SatFileOptionManager(string Name ):base(Name)
        { 
        } 
    }
}
