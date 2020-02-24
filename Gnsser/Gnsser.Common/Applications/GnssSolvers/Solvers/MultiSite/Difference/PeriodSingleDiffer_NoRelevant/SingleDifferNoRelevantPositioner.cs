//2012.05.29, czs, creare, 单差,载波差分定位。许其凤,164-167 
//2014.12.10, czs, edit in jinxinliaomao shaungliao, 短基线载波相位差分，单差
//2015.01.06, czs, edit in namu, 消除模糊度互差与钟差互差的相关性。

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
    public class SingleDifferNoRelevantPositioner : AbstracMultiSitePeriodPositioner
    { 
         /// <summary>
        /// 最简化的构造函数，可以多个定位器同时使用的参数，而不必多次读取
        /// </summary>
        /// <param name="DataSourceContext"></param>
        /// <param name="PositionOption"></param>
        public SingleDifferNoRelevantPositioner(DataSourceContext DataSourceContext, GnssProcessOption PositionOption)
            : base(DataSourceContext, PositionOption)
        {
            this.Name = "载波相位无相关单差";
            this.MatrixBuilder = new SingleDifferNoRelevantMatrixBuilder(this.Option); 
        } 
          
   
    }
}
