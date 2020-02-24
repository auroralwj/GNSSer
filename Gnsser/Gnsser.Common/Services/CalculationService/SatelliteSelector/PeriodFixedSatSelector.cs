//2017.03.09, czs, create in hongqing, 分时段卫星编号选择器
//2017.05.13, czs, eidt in hongqing, 提取公共接口


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
    /// <summary>
    /// 基于时段内的分时段卫星编号选择器
    /// </summary>
    public class PeriodFixedSatSelector : AbstractPeriodBaseSatSelector
    {
        Log log = new Log(typeof(PeriodFixedSatSelector));

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="satEleTable"></param>
        /// <param name="periodCount"></param>
        /// <param name="IsExpandPeriodWhenSamePrn"></param>
        /// <param name="CutOffAngle"></param>
        public PeriodFixedSatSelector(ObjectTableStorage satEleTable, double CutOffAngle, int periodCount = 8, bool IsExpandPeriodWhenSamePrn = true, BaseSatSelectionType TableSatSelectorType = BaseSatSelectionType.MaxCenterElevation, double TotalPeriodSpan = 24 * 3600)
            : base(satEleTable, CutOffAngle, IsExpandPeriodWhenSamePrn)
        {
            this.TotalPeriodSpan = TotalPeriodSpan; 
            this.PeriodCount = periodCount; 
            this.TableSatSelectorType = TableSatSelectorType; 
        }
         
        #region  属性
     
        /// <summary>
        /// 时段
        /// </summary>
        int PeriodCount { get; set; } 
        /// <summary>
        /// 总时段全长，秒
        /// </summary>
        public double TotalPeriodSpan { get; set; }  
        /// <summary>
        /// 选择类型。
        /// </summary>
        public BaseSatSelectionType TableSatSelectorType { get; set; }
         

        #endregion

        /// <summary>
        /// 选择
        /// </summary>
        /// <returns></returns>
        public override PeriodPrnManager Select()
        {
            this.DetailResultTable = new ObjectTableStorage("DetailResultTableOf_" + TableSatSelectorType + "_" + PeriodCount + "Count");

            var PeriodPrnManager = new PeriodPrnManager();

            var startTime = (Time)SatElevationTable.GetIndexValue(0);
            var stepPeriod = TotalPeriodSpan / PeriodCount; 

            for (int i = 0; i < PeriodCount; i++)
            {
                var start = startTime + TimeSpan.FromSeconds(i * stepPeriod);
                var end = start + TimeSpan.FromSeconds(stepPeriod);
                TimePeriod TimePeriod = new TimePeriod(start, end);

                BaseTableSatelliteSelector selector = null;
                switch (TableSatSelectorType)
                {
                    case Gnsser.BaseSatSelectionType.MaxElevation:
                        selector = new TableElevationSatSelector(SatElevationTable, TimePeriod,CutOffAngle, false);
                        break;
                    case Gnsser.BaseSatSelectionType.MaxCenterElevation:
                        selector = new TableElevationSatSelector(SatElevationTable, TimePeriod,CutOffAngle, true, 1);
                        break;
                    case Gnsser.BaseSatSelectionType.MaxTimeSpan:
                        selector = new TableSpanSatSelector(SatElevationTable, TimePeriod, CutOffAngle);
                        break; 
                }              

                SatelliteNumber prn = selector.Select();
                var PeriodPrn = new PeriodPrn(TimePeriod, prn);

                this.AddPeriodPrnToManager(PeriodPrnManager, PeriodPrn); 
            }
            return PeriodPrnManager;
        }
    }

}