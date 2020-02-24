//2017.05.13, czs, create in hongqing, 不固定历元长度的基准卫星选择器


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
    /// 不固定历元长度的基准卫星选择器，基于卫星分时段卫星编号选择器
    /// </summary>
    public class FlexiblePeriodSatSelector : AbstractPeriodBaseSatSelector
    {
        Log log = new Log(typeof(FlexiblePeriodSatSelector));

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="satEleTable"></param>
        /// <param name="CutOffAngle"></param>
        public FlexiblePeriodSatSelector(ObjectTableStorage satEleTable, double CutOffAngle = 30, double timeSpanHour = 6)
            : base(satEleTable, CutOffAngle)
        { 
            this.SatElevationTable = satEleTable; 

            this.CutOffAngle = CutOffAngle;
            this.SearchTimeSpan = TimeSpan.FromHours(timeSpanHour);
        }

        #region  属性

        /// <summary>
        /// 搜索的时间范围
        /// </summary>
        public TimeSpan SearchTimeSpan { get; set; }

        #endregion

        /// <summary>
        /// 选择
        /// </summary>
        /// <returns></returns>
        public override PeriodPrnManager Select()
        {
            this.DetailResultTable = new ObjectTableStorage("DetailResultTableOfFlexible_" + CutOffAngle + "deg");

            var PeriodPrnManager = new PeriodPrnManager();

            var totalEndTime = SatElevationTable.GetLastIndexValue<Time>();
            var firstStartTime = SatElevationTable.GetFirstIndexValue<Time>();
             
            var prevEndPeriodTime = firstStartTime;
            while (prevEndPeriodTime < totalEndTime)
            {
               // PeriodPrn PeriodPrn = BuildOnePeriodPrnByCenterTop2(prevEndPeriodTime);
                var PeriodPrn = BuildOnePeriodPrnByCenterTop(prevEndPeriodTime);

                if ( PeriodPrn == null || PeriodPrn.TimePeriod.Span == 0) {
                    log.Error("无满足条件的时段。" + PeriodPrn);
                    break; 
                }

                AddPeriodPrnToManager(PeriodPrnManager, PeriodPrn);

                prevEndPeriodTime = PeriodPrn.TimePeriod.End; 
            }

            return PeriodPrnManager;
        }

        //首先，选择在起始时刻所有满足条件的卫星，
        //查找这些卫星的最大高度角的位置(附近6小时内)，选择距离远的，
        //查找其结束点高度角时刻，
        //继续遍历
        public PeriodPrn BuildOnePeriodPrnByCenterTop(Time startTime)
        {
            var endTime = startTime + SearchTimeSpan;

            List<String> prnsString = GetStartPrns(startTime);
            List<TableCell> maxCells = new List<TableCell>();
            foreach (var colName in prnsString)
            {
                if (SatElevationTable.MinusNext(colName, startTime) < 0) //方向必须朝上
                {
                   var max =  SatElevationTable.GetFirstSlopeApproxTo(colName, CutOffAngle, startTime, false);

                 //   var max = SatElevationTable.GetMaxCell(colName, startTime, endTime);
                    maxCells.Add(max);
                }
            }
            if (maxCells.Count == 0) { return null; }

            var longest = maxCells.OrderByDescending(m => m.RowNumber).First();

            SatelliteNumber selectedPrn = SatelliteNumber.Parse(longest.ColName); 

            var endTimePeriod = (Time)SatElevationTable.GetIndexValue(longest.RowNumber);

            TimePeriod TimePeriod = new Geo.Times.TimePeriod(startTime, endTimePeriod);
            var PeriodPrn = new PeriodPrn(TimePeriod, selectedPrn);

            return PeriodPrn;
        }


        private PeriodPrn BuildOnePeriodPrnByCenterTop2(Time startTime)
        {
            var endTime = startTime + SearchTimeSpan;

            List<String> prnsString = GetStartPrns(startTime);
            List<TableCell> maxCells = new List<TableCell>();
            foreach (var item in prnsString)
            {
                if (SatElevationTable.MinusNext(item, startTime) < 0) //方向必须朝上
                {
                    var max = SatElevationTable.GetMaxCell(item, startTime, endTime);
                    maxCells.Add(max);
                }
            }
            var longest = maxCells.FindAll(m => (double)m.Value > CutOffAngle).OrderByDescending(m => m.RowNumber).First();

            SatelliteNumber selectedPrn = SatelliteNumber.Parse(longest.ColName);
            //查找其结尾处，满足条件的时刻或历元
            var startTime2 = (Time)SatElevationTable.GetIndexValue(longest.RowNumber);
            var endTime2 = startTime2 + SearchTimeSpan;
            TableCell endCell = SatElevationTable.GetCellApproxTo(longest.ColName, CutOffAngle, startTime2, endTime2);
            var endTimePeriod = (Time)SatElevationTable.GetIndexValue(endCell.RowNumber);

            TimePeriod TimePeriod = new Geo.Times.TimePeriod(startTime, endTimePeriod);
            var PeriodPrn = new PeriodPrn(TimePeriod, selectedPrn);

            return PeriodPrn;
        }
        /// <summary>
        /// 获取起始PRN
        /// </summary>
        /// <param name="startTime"></param>
        /// <returns></returns>
        private List<string> GetStartPrns(Time startTime)
        {
            List<String> prnsString = new List<String>();
            var startRow = this.SatElevationTable.GetRow(startTime);
            foreach (var item in startRow)
            {
                var prnStr = item.Key;
                var prn = SatelliteNumber.TryParse(prnStr);
                if (prn.PRN != 00 && Geo.Utils.ObjectUtil.IsNumerial(startRow[prnStr]) && Geo.Utils.ObjectUtil.GetNumeral(startRow[prnStr]) >= CutOffAngle)
                {
                    prnsString.Add(item.Key);
                }
            }
            return prnsString;
        }
    }
}