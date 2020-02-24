//2015.10.15, czs, create in 西安五路口袁记肉夹馍店, 卫星分析器


using System;
using System.Text;
using System.IO;
using System.Collections.Generic;
using Geo;
using Geo.Algorithm;
using Geo.Coordinates;
using Geo.Algorithm.Adjust;
using Geo.Algorithm;
using Gnsser.Times;
using Gnsser.Data;
using Gnsser.Data.Rinex;
using Gnsser.Domain;
using Gnsser.Service;
using Gnsser.Correction;
using Geo.Utils;
using Geo.Common;
using Gnsser.Checkers;
using Gnsser.Filter;
using Geo.Times;

namespace Gnsser
{
    

    /// <summary>
    /// 卫星分析器顶层类。
    /// </summary>
    public abstract  class SatAnalyst : Reviser<EpochInformation>
    {
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="interval">采样间隔</param>
        public SatAnalyst( double interval = 30.0)
        {
            this.SatSequentialPeriod = new SatPeriodInfoManager(interval);
        }
        /// <summary>
        /// 卫星时段信息标记。
        /// </summary>
       public  SatPeriodInfoManager SatSequentialPeriod { get; set; }

     
    } 
    
}
