//2015.10.24, czs, create in 彭州, 单点定位

using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using Geo.Common;
using Geo.Coordinates;
using Geo.IO;
using Geo;
using System.ComponentModel.DataAnnotations;

namespace Gnsser.Api
{
    /// <summary>
    /// 通用表格文件
    /// </summary>
    public class PointPositionParam : VersionedIoParam,  IParallelableParam
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public PointPositionParam()
        {
            this.ParallelProcessCount = 4;
            this.IsParallel = true;
            this.SatTypeString = "G";


            this.OrderedProperties = new System.Collections.Generic.List<string>
            {
                Geo.Utils.ObjectUtil.GetPropertyName<PointPositionParam>(m=>m.InputPath),
                Geo.Utils.ObjectUtil.GetPropertyName<PointPositionParam>(m=>m.OutputPath),
                Geo.Utils.ObjectUtil.GetPropertyName<PointPositionParam>(m=>m.SatTypeString),
                Geo.Utils.ObjectUtil.GetPropertyName<PointPositionParam>(m=>m.IsOverwrite),
                Geo.Utils.ObjectUtil.GetPropertyName<PointPositionParam>(m=>m.OutputVersion),
                Geo.Utils.ObjectUtil.GetPropertyName<PointPositionParam>(m=>m.IsParallel),
                Geo.Utils.ObjectUtil.GetPropertyName<PointPositionParam>(m=>m.ParallelProcessCount),
                Geo.Utils.ObjectUtil.GetPropertyName<PointPositionParam>(m=>m.EphemerisPath),
                Geo.Utils.ObjectUtil.GetPropertyName<PointPositionParam>(m=>m.ClockPath),
            };
        }
        /// <summary>
        /// 输入路径
        /// </summary> 
        [Display(Name = "GNSS系统")]
        public String SatTypeString { get; set; }

        public List<SatelliteType> SatelliteTypes
        {
            get
            {
                var types = SatTypeString.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                List<SatelliteType> typ = new List<SatelliteType>();
                foreach (var item in types)
                {
                    SatelliteType e = (SatelliteType)Enum.Parse(typeof(SatelliteType), item);
                    typ.Add(e);
                }
                return typ;
            }
        }

        /// <summary>
        /// 输入路径
        /// </summary> 
        [Display(Name = "星历地址")]
        public string EphemerisPath { get; set; }
        /// <summary>
        /// 输入路径
        /// </summary> 
        [Display(Name = "钟差地址")]
        public string ClockPath { get; set; }
        /// <summary>
        /// 是否并行计算
        /// </summary> 
        [Display(Name = "是否并行计算")]
        public Boolean IsParallel { get; set; }
        /// <summary>
        /// 并行线程数量
        /// </summary>       
        [Display(Name = "并行处理器数量")] 
        public int ParallelProcessCount { get; set; }
 }
}
