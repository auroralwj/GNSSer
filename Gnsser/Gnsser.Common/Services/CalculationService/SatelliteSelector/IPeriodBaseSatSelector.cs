
//2017.05.13, czs, create in hongqing, 提取公共接口


using System;
using Geo;
using Geo.IO;

namespace Gnsser
{
    /// <summary>
    /// 卫星选择器接口。
    /// </summary>
    public interface IPeriodBaseSatSelector
    {
        /// <summary>
        /// 卫星高度表
        /// </summary>
        double CutOffAngle { get; set; }
        /// <summary>
        /// 执行选择。
        /// </summary>
        /// <returns></returns>
        global::Gnsser.PeriodPrnManager Select();
    }

    /// <summary>
    /// 抽象卫星选择器
    /// </summary>
    public abstract class AbstractPeriodBaseSatSelector : IPeriodBaseSatSelector
    {
        Log log = new Log(typeof(AbstractPeriodBaseSatSelector));

        /// <summary>
        /// 抽象卫星选择器， 构造函数。
        /// </summary>
        /// <param name="SatElevationTable"></param>
        /// <param name="IsExpandPeriodWhenSamePrn"></param>
        /// <param name="CutOffAngle"></param>
        public AbstractPeriodBaseSatSelector(ObjectTableStorage SatElevationTable, double CutOffAngle = 15, bool IsExpandPeriodWhenSamePrn = true)
        {
            this.SatElevationTable = SatElevationTable;
            this.CutOffAngle = CutOffAngle;
            this.IsExpandPeriodWhenSamePrn = IsExpandPeriodWhenSamePrn;
        }

        /// <summary>
        /// 如果与上一个卫星一致，是否扩展弧段.
        /// 是否拼接相近相同的卫星
        /// </summary>
        public bool IsExpandPeriodWhenSamePrn { get; set; }
        /// <summary>
        /// 卫星高度表
        /// </summary>
        protected ObjectTableStorage SatElevationTable { get; set; }

        /// <summary>
        /// 详细结果表
        /// </summary>
        public ObjectTableStorage DetailResultTable { get; set; }
        /// <summary>
        /// 指定的卫星高度截止角
        /// </summary>
        public double CutOffAngle { get; set; }
        /// <summary>
        /// 选择器。
        /// </summary>
        /// <returns></returns>
        public abstract PeriodPrnManager Select();

        /// <summary>
        /// 将选择结果添加到管理器
        /// </summary>
        /// <param name="PeriodPrnManager"></param>
        /// <param name="PeriodPrn"></param>
        protected void AddPeriodPrnToManager(Gnsser.PeriodPrnManager PeriodPrnManager, PeriodPrn PeriodPrn)
        {
            DetailResultTable.NewRow();
            DetailResultTable.AddItem("Period", PeriodPrn.TimePeriod.ToString());
            DetailResultTable.AddItem("Span", PeriodPrn.TimePeriod.TimeSpan.TotalHours.ToString("0.00"));
            DetailResultTable.AddItem("Prn", PeriodPrn.Value);

            var statAngle = SatElevationTable.GetValue<double>(PeriodPrn.TimePeriod.Start, PeriodPrn.Value.ToString());
            var endAngle = SatElevationTable.GetValue<double>(PeriodPrn.TimePeriod.End, PeriodPrn.Value.ToString());
            var maxAngle = SatElevationTable.GetMaxValue(PeriodPrn.Value.ToString(), PeriodPrn.TimePeriod.Start, PeriodPrn.TimePeriod.End);
            DetailResultTable.AddItem("StartAngle", statAngle);
            DetailResultTable.AddItem("MaxAngle", maxAngle);
            DetailResultTable.AddItem("EndAngle", endAngle);
            DetailResultTable.EndRow();


            //如果与上次基准星一致，则扩展弧段
            if (IsExpandPeriodWhenSamePrn && PeriodPrnManager.Count > 0 && PeriodPrnManager.Last.Value == PeriodPrn.Value)
            {
                log.Debug("基准星 " + PeriodPrn.Value + "相同，扩展时段 " + PeriodPrnManager.Last.TimePeriod + " 到 " + PeriodPrn.TimePeriod.End);
                PeriodPrnManager.Last.TimePeriod.End = PeriodPrn.TimePeriod.End;
            }
            else
            {
                if (PeriodPrn.Value.SatelliteType == SatelliteType.U)
                {
                    int a = 0;
                }
                if (PeriodPrn.Value != null)
                {
                    PeriodPrnManager.Add(PeriodPrn);
                }
            }
        }
    }


}
