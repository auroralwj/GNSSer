//2014.05.22, Cui Yang, created
//2014.07.04, Cui Yang, 增加多映射通用集合类，添加了MultiMap引用,注：已经取消。
//2014.07.05, czs, edit, 进行了代码重构

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Gnsser.Service;
using Geo.Coordinates;
using Gnsser.Data.Rinex;
using Geo.Algorithm;
using Geo.Utils;
using Gnsser;
using Gnsser.Times;
using Geo;
using Geo.Common;

namespace Gnsser.Data
{

    /// CODE每月发布DCB文件
    /// <summary>
    /// DCB基础信息。
    /// 说明：CODE发布了 "P1P2YYMM.DCB P1C1YYMM.DCB"文件，用于记录卫星信息，包括PRN，VALUE，RMS等。
    /// 该文件对于精密定位具有重要的作用。
    /// </summary>
    public class DcbValue : RmsedNumeral//, Geo.Common.Namable
    {
        /// <summary>
        /// 初始化一个卫星信息
        /// </summary>
        public DcbValue(SatelliteNumber prn, double val, double rms)
            : base(val, rms)
        {
            this.Prn = prn;
        } 

        /// <summary>
        /// 名称或者卫星编号。
        /// </summary>
        //public string Name { get; set; }
        /// <summary>
        /// 卫星编号
        /// </summary>
        public SatelliteNumber Prn { get; set; } 

        public override string ToString()
        {
            return Prn + " " + Value + " " + Rms;
        }
    }
}