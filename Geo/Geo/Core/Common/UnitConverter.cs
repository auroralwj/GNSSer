//2016.02.10 czs, create in xi'an hongqing, 不同换算单位的转换

using System;
using System.IO;
using Geo.Common;
using Geo.Coordinates;
using System.Collections;
using System.Collections.Generic;
using Geo.IO;
using Geo;

namespace Geo
{
    /// <summary>
    /// 单位转换器
    /// </summary>
    public class UnitConverter
    {
        public UnitConverter()
        {

        }

        /// <summary>
        /// 转换
        /// </summary>
        /// <param name="val"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public static Double Convert(double val, Unit from, Unit to)
        {
            //if (from.UnitType != to.UnitType)
            //{
            //    throw new NotSupportedException("不支持不同类型的单位转换：" + from.UnitType + "->" + to.UnitType);
            //}
            if (from.Equals(to))
            {
                return val;
            }

            //首选转换为米
            if (from.Gauge != 0 && to.Gauge != 0)
            {
                double meter = val * from.Gauge;
                double result = meter / to.Gauge;
                return result;
            }

            if (from.UnitType == UnitType.Angle)
            {
                if (from.Equals(Unit.HMS_S))
                {
                    //转换为度，再转换
                    DMS dms = new DMS(val, AngleUnit.HMS_S);

                    return Convert(dms.Degrees, Unit.Degree, to);
                }
                if (from.Equals(Unit.DMS_S))
                {
                    //转换为度，再转换
                    DMS dms = new DMS(val, AngleUnit.DMS_S);
                    return Convert(dms.Degrees, Unit.Degree, to);
                }

                if (to.Equals(Unit.DMS_S))
                {
                    //转换为度，再转换
                    var degree = Convert(val, from, Unit.Degree);

                    DMS dms = new DMS(degree, AngleUnit.Degree);
                    return dms.Dms_s;
                }
                if (to.Equals(Unit.D_MS))
                {
                    //转换为度，再转换
                    var degree = Convert(val, from, Unit.Degree);

                    DMS dms = new DMS(degree, AngleUnit.Degree);
                    return dms.D_ms;
                }
                if (to.Equals(Unit.HMS_S))
                {
                    //转换为度，再转换
                    var degree = Convert(val, from, Unit.Degree);

                    DMS dms = new DMS(degree, AngleUnit.HMS_S);
                    return dms.Hms_s;
                }
            }
            
           throw new NotSupportedException("不支持的单位转换：" + from + "->" + to);
        }
    }
}