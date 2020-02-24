//2017.03.09, czs, create in hongqing, 分时段卫星编号选择器
//2017.03.20, czs, edit in hongqing, 单独类提出来

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
{
    //2018.08.04, czs, edit in hmx, 更名为 BaseSatSelectionType
    /// <summary>
    /// 选星类型
    /// </summary>
    public enum BaseSatSelectionType
    {
        /// <summary>
        /// 中间高度选星
        /// </summary>
        MaxCenterElevation,
        /// <summary>
        /// 最大高度角选星
        /// </summary>
        MaxElevation,
        /// <summary>
        /// 跨度最长选星
        /// </summary>
        MaxTimeSpan, 
    }


    //2018.08.04, czs, create in hmx, 选择基准星工厂 



    //2018.08.04, czs, create in hmx, BaseSatelliteSelector 工厂
    /// <summary>
    /// BaseSatelliteSelector 工厂
    /// </summary>
    public static class BaseSatelliteSelectorFactory
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="option"></param>
        /// <param name="oFilePath"></param>
        /// <returns></returns>
        public static FlexiblePeriodSatSelector GetPeriodPrnManager(GnssProcessOption option, string oFilePath)
        {
            var satEleTable = SatElevatoinTableBuilder.BuildTable(oFilePath, option.VertAngleCut);

            return new FlexiblePeriodSatSelector(satEleTable, option.VertAngleCut);
            switch (option.BaseSatSelectionType)
            {
                case BaseSatSelectionType.MaxCenterElevation: 
                    break;
                case BaseSatSelectionType.MaxElevation:
                    break;
                case BaseSatSelectionType.MaxTimeSpan: 
                    break;
                default:
                    break;
            }
        }

    }




    /// <summary>
    /// 时段内最大卫星高度角的选星方法。需要卫星高度文件表的支持。
    /// </summary>
    public class TableElevationSatSelector : BaseTableSatelliteSelector
    {
        /// <summary>
        /// 构造函数TYU
        /// </summary>
        /// <param name="satEleTable"></param>
        /// <param name="TimePeriod"></param>
        /// <param name="CutOffAngle"></param>
        /// <param name="EnableCenterMaxSelection">是否启用</param>
        /// <param name="centerCount">中间窗口大小，秒</param>
        public TableElevationSatSelector(ObjectTableStorage satEleTable, TimePeriod TimePeriod,double CutOffAngle, bool EnableCenterMaxSelection = true, int centerCount = 1)
            : base(satEleTable, TimePeriod, CutOffAngle)
        {
            this.CenterCount = centerCount;
            this.EnableCenterMaxSelection = EnableCenterMaxSelection;
            var first = (Time)SatEleTable.FirstIndex;
            var second = (Time)SatEleTable.SecondIndex;
            this.Interval = TimeSpan.FromSeconds(second - first); 
        }
        /// <summary>
        /// 间隔
        /// </summary>
        public TimeSpan Interval { get; set; }

        /// <summary>
        /// 中心选星法，中间窗口大小，在时段中间区域选星。单位：秒。
        /// </summary>
        public int CenterCount { get; set; }
        /// <summary>
        /// 是否启用,中心选星法
        /// </summary>
        public bool EnableCenterMaxSelection { get; set; }

        /// <summary>
        /// 执行选星
        /// </summary>
        /// <returns></returns>
        public override SatelliteNumber Select()
        {
            var start = TimePeriod.Start;
            var end = TimePeriod.End;
            TableCell maxCell = null;
            if (EnableCenterMaxSelection)
            {   
                var windowSpan = Math.Min(CenterCount, TimePeriod.Span);
                var middle = Time.Parse( start + TimeSpan.FromSeconds(TimePeriod.Span / 2.0));

                maxCell = SatEleTable.GetMax(middle, CenterCount);
                return SatelliteNumber.Parse(maxCell.ColName);

                //start = middle - TimeSpan.FromSeconds(windowSpan / 2.0);
                //end = middle + TimeSpan.FromSeconds(windowSpan / 2.0);
            }


            maxCell = SatEleTable.GetMax(start, end);
            return SatelliteNumber.Parse(maxCell.ColName);
        }
    }




}
