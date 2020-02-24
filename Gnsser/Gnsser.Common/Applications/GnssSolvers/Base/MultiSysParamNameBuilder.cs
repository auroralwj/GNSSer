//2017.09.05.23, czs, create in hongqing, 多系统参数命名器

using System;
using System.Collections.Generic;
using System.Text;
using Gnsser.Domain;
using Gnsser.Data.Sinex;
using Gnsser.Data.Rinex;
using Gnsser.Times;
using Geo.Algorithm;
using Geo.Coordinates;
using  Geo.Algorithm.Adjust;
using Geo;
using Geo.Times;

namespace Gnsser.Service
{
    /// <summary>
    /// 多系统参数命名器
    /// </summary>
    public abstract class MultiSysParamNameBuilder : GnssParamNameBuilder
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="option"></param>
        public MultiSysParamNameBuilder(GnssProcessOption option):base(option)
        {
        
        } 
    }   
}