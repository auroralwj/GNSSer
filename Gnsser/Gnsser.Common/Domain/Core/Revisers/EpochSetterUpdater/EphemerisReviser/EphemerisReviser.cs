//2014.09.15, czs, create, 设置卫星星历。
//2014.10.12, czs, edit in hailutu, 对星历赋值进行了重新设计，分解为几个不同的子算法
//2015.02.08, 崔阳, 卫星钟差和精密星历若同时存在，则不可分割
//2017.08.06, czs, edit in hongqing, IEphemerisProcessor 命名为 IEphemerisReviser

using System;
using System.IO;
using System.Collections.Generic;
using Gnsser.Domain;
using System.Text;
using Geo.Coordinates;
using Gnsser.Data.Rinex;
using Geo.Algorithm;
using Geo.Algorithm.Adjust;
using Gnsser.Service;
using Geo.Utils;
using Gnsser.Checkers;
using Geo.Common;
using Gnsser.Times;
using Gnsser.Data;
using Geo.Times;
using Gnsser.Correction;

namespace Gnsser
{ 

   
    /// <summary>
    /// 星历矫正器。
    /// </summary>
    public abstract class EphemerisReviser : Geo.Named, IEphemerisReviser
    {
        /// <summary>
        /// 执行信息
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// 缓存
        /// </summary>
        public Geo.IWindowData<IEphemeris> Buffers { get; set; }

        public abstract bool Revise(ref IEphemeris eph);
        /// <summary>
        /// 初始化
        /// </summary>
        public virtual void Init()
        {

        }
        /// <summary>
        /// 完成
        /// </summary>
        public virtual void Complete()
        {

        }
    } 

}
