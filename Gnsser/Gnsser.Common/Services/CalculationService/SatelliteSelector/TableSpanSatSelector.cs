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

    /// <summary>
    /// 时段内最大卫星时段的选星方法。需要卫星高度文件表的支持。
    /// </summary>
    public class TableSpanSatSelector : BaseTableSatelliteSelector
    {
        /// <summary>
        /// 构造函数TYU
        /// </summary>
        /// <param name="satEleTable"></param>
        /// <param name="TimePeriod"></param> 
        public TableSpanSatSelector(ObjectTableStorage satEleTable, TimePeriod TimePeriod, double CutOffAngle)
            : base(satEleTable, TimePeriod, CutOffAngle)
        {
        }


        /// <summary>
        /// 执行选星
        /// </summary>
        /// <returns></returns>
        public override SatelliteNumber Select()
        {
            var start = TimePeriod.Start;
            var end = TimePeriod.End;
            var subTable = SatEleTable.GetSub(start, end);
            var dic = subTable.GetValidDataCount(m => m >= CutOffAngle);

            var max = dic.Where(m => m.Value == dic.Max(n => n.Value)).ToList();
          
            var maxItem = max.First();
            double maxValue =double.MinValue;
            foreach (var item in max)
            {
                var thisVal = subTable.GetMaxValue(item.Key, start, end);
                if (maxValue < thisVal)
                {
                    maxValue = thisVal;
                    maxItem = item;
                }
            }

            return SatelliteNumber.Parse(maxItem.Key);
        }
    }

}
