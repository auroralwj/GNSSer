//2016.10.17, czs, create in hongqing,  周跳性数据滤波器

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Geo.Times;
using Geo.IO;
using Geo;
using Geo.Algorithm;
using Geo.Algorithm.Adjust;
using Geo.Common;

namespace Geo
{ 
    /// <summary>
    /// 平差滤波器
    /// </summary>
    public class AdjustFilter
    {
        /// <summary>
        /// 上一个历元结果。
        /// </summary>
        AdjustResultMatrix PrevAdjustment { get; set; }

        /// <summary>
        /// 滤波
        /// </summary>
        /// <param name="newVal"></param> 
        /// <returns></returns>
        public RmsedNumeral Filter(RmsedNumeral newVal)
        {
            var builder = new OneDimAdjustMatrixBuilder(newVal, PrevAdjustment);
            var sp = new SimpleKalmanFilter();
            var a = sp.Run(builder);
            var est = a .Estimated;
            this.PrevAdjustment = a;//update
            return new RmsedNumeral(est[0], est.GetRmsVector()[0]);
        }

    }
}