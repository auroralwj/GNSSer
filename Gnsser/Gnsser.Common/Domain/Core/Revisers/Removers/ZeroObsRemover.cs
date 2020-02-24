//2018.05.05, czs, create in HMX,  移除观测值为0的卫星

using System;
using System.Collections.Generic;
using System.Text;
using Geo.Algorithm;
using Geo;
using Geo.Utils;
using Geo.Common;
using Geo.Coordinates;
using Geo.Algorithm.Adjust;
using Gnsser.Domain;
using Gnsser.Service;
using Gnsser.Checkers;
using Gnsser.Data;
using Gnsser.Data.Rinex;
using Gnsser.Correction;
using Gnsser.Filter;

namespace Gnsser
{
    /// <summary>
    /// 移除观测值为0的卫星
    /// </summary>
    public class ZeroObsRemover : EpochInfoReviser
    {
        /// <summary>
        /// 构造函数
        /// </summary> 
        public ZeroObsRemover()
        {
            this.Name = "移除观测值为0的卫星";
            log.Info("将移除观测值为 0 的卫星");
        } 
        /// <summary>
        /// 修正
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Revise(ref EpochInformation obj)
        {
            obj.RemoveZeroObsSat();

            return true;
        }
    }
}
