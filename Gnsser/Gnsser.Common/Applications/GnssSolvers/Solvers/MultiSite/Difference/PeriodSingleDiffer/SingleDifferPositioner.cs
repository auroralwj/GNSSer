//2012.05.29, czs, creare, 单差,载波差分定位。许其凤,164-167 
//2014.12.10, czs, edit in jinxinliaomao shaungliao, 短基线载波相位差分，单差

using System;
using System.Collections.Generic;
using Gnsser.Domain;
using System.Text;
using Geo.Coordinates;
using Gnsser.Data.Rinex;
using Geo.Algorithm;
using Geo.Algorithm.Adjust;
using Gnsser.Service;
using Geo.Utils;
using Gnsser.Filter;
using Gnsser.Checkers;
using Geo.Common;

namespace Gnsser.Service
{
    /// <summary>
    ///  短基线载波相位差分，单差
    ///  方法：以一个站作为参考站，另一个作为流动站（待算站)
    /// </summary>
    public class SingleDifferPositioner : AbstracMultiSitePeriodPositioner
    { 
        /// <summary>
        /// 短基线载波相位差分。
        /// </summary> 
        /// <param name="DataSourceContext">数据源</param>
        /// <param name="Option">解算模型，数据输入模型</param> 

        public SingleDifferPositioner(DataSourceContext DataSourceContext, GnssProcessOption Option)
            : base(DataSourceContext, Option)
        { 
            this.Name = "载波相位单差";
            this.BaseParamCount = 5; 
            this.MatrixBuilder = new SingleDifferMatrixBuilder(Option);
        } 




    }
}
