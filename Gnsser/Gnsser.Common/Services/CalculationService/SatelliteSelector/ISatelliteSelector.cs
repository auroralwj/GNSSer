//2017.03.09, czs, create in hongqing, 分时段卫星编号选择器



using System;
using System.Collections.Generic;
using System.Linq;
using Geo;
using Geo.IO;
using Geo.Common;
using Geo.Coordinates;

using Geo.Times;
using Gnsser.Core;
using Gnsser.Domain;
using System.Text;


namespace Gnsser
{  /// <summary>
    /// 分时段卫星编号选择器
    /// </summary>
    public abstract class BaseTableSatelliteSelector : BaseSatelliteSelector
    {
               /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="TimePeriod"></param>
        public BaseTableSatelliteSelector(ObjectTableStorage SatEleTable, TimePeriod TimePeriod,double CutOffAngle)
            : base(TimePeriod, CutOffAngle)
        {
            this.SatEleTable = SatEleTable;
        }
        /// <summary>
        /// 卫星高度文件表
        /// </summary>
        public ObjectTableStorage SatEleTable { get; set; }
    }
    /// <summary>
    /// 分时段卫星编号选择器
    /// </summary>
    public abstract class BaseSatelliteSelector : ISatelliteSelector
    {
        Log log = new Log(typeof(BaseSatelliteSelector));
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="TimePeriod"></param>
        public BaseSatelliteSelector(TimePeriod TimePeriod, double CutOffAngle)
        {
            this.TimePeriod = TimePeriod;
            this.CutOffAngle = CutOffAngle;
        }
        
        /// <summary>
        /// 时段
        /// </summary>
        public TimePeriod TimePeriod { get; set; }
        /// <summary>
        /// 卫星高度截止角
        /// </summary>
        public double CutOffAngle { get; set; }
        /// <summary>
        /// 选择基准卫星
        /// </summary>
        /// <returns></returns>
        public abstract SatelliteNumber Select();
    }

    /// <summary>
    /// 卫星编号选择器
    /// </summary>
    public interface ISatelliteSelector
    {
        /// <summary>
        /// 时段
        /// </summary>
        TimePeriod TimePeriod { get; set; }
        /// <summary>
        /// 选择基准卫星
        /// </summary>
        /// <returns></returns>
         SatelliteNumber Select();
    }
}
