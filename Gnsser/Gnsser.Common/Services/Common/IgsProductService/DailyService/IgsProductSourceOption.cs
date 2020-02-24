//2017.08.18, czs, create in hongqing, IGS产品名称生成器

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

using Gnsser.Data;
using Geo.Times;

namespace Gnsser.Data
{
    /// <summary>
    /// IGS  产品选项。
    /// </summary> 
    public class IgsProductSourceOption : IOption
    {
        public IgsProductSourceOption()
        {
        }
        /// <summary>
        /// 默认构造函数
        /// </summary>
        /// <param name="SatelliteType">系统类型</param>
        /// <param name="timePeriod">时段信息</param>
        public IgsProductSourceOption(BufferedTimePeriod timePeriod, List<SatelliteType> SatelliteType)
        { 
            this.TimePeriod = timePeriod;
            this.SatelliteTypes = SatelliteType;
            this.IsDownloadingSurplurseIgsProducts = false;
            this.Sp3EphMaxBreakingCount = 5;
            this.MinSequentialSatCount = 11;
            this.MaxIgsProductCacheCount = 32;
            IsSkipIonoContent = false;
            InterpolateOrder = 10;
            IsConnectIgsProduct = true;
        }

        #region 属性
        /// <summary>
        /// 是否连接IGS产品
        /// </summary>
        public bool IsConnectIgsProduct { get; set; }
        /// <summary>
        /// 星历插值阶次
        /// </summary>
        public int InterpolateOrder { get; set; }
        /// <summary>
        /// 有时候只需要头文件即可，如Iono
        /// </summary>
        public bool IsSkipIonoContent { get; set; }
        /// <summary>
        /// 最小连续的卫星数量，用于星历拟合
        /// </summary>
        public int MinSequentialSatCount { get; set; }
        /// <summary>
        /// 精密星历允许的断裂次数
        /// </summary>
        public int Sp3EphMaxBreakingCount { get; set; } 
        /// <summary>
        /// 是否下载多余的IGS产品，否则只下载一个。
        /// </summary>
        public bool IsDownloadingSurplurseIgsProducts { get; set; }
        /// <summary>
        /// 数据源时段信息
        /// </summary>
        public BufferedTimePeriod TimePeriod { get; set; }
        /// <summary>
        /// 系统类型
        /// </summary>
        public List<SatelliteType> SatelliteTypes { get; set; }
        /// <summary>
        /// IGS数据源关键字,按照不同系统指定。
        /// </summary>
        public Dictionary<SatelliteType, List<string>> IgsProductSourceDic { get; set; }
        /// <summary>
        /// URL 目录
        /// </summary>
        public string [] IgsProductUrlDirectories { get; set; }
        /// <summary>
        /// URL 模板
        /// </summary>
        public string[] IgsProductUrlModels { get; set; }
        /// <summary>
        /// 通用 IGS 数据源关键字。
        /// </summary>
        public List<string> IgsProductSources { get; set; }
        /// <summary>
        /// IGS 产品本地目录。
        /// </summary>
        public string IgsProductLocalDirectory { get; set; }
        /// <summary>
        /// IGS 产品本地目录。
        /// </summary>
        public List<string> IgsProductLocalDirectories { get; set; }
        /// <summary>
        /// 是否采用唯一数据源,即IndicatedSourceCode的内容，高精度常用
        /// </summary>
        public bool IsUniqueSource { get;  set; }
        /// <summary>
        /// 指定的数据源代码如，ig,wh,co
        /// </summary>
        public string IndicatedSourceCode { get;  set; }
        /// <summary>
        /// IGS产品最大缓存数量,含SP3、Clk等总共的数量
        /// </summary>
        public int MaxIgsProductCacheCount { get;  set; }
        #endregion
    } 

    
}