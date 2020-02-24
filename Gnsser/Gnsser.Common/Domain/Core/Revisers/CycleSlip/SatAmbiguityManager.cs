// 2014.09.10, czs, create,  包含周跳记录和周跳跳变标记
//2015.01.07, czs, edit in namu, 名称从 CycleClipManager 修改为 SatAmbiguityManager，分离出标记周跳模块，只负责维护对齐模糊度
//2016.05.02, czs, edit in hongqing, 继承自  BaseDictionary

using System;
using System.Collections.Generic;
using System.Text;
using Geo.Utils;
using Geo.Common;
using Geo.Coordinates;
using Geo.Algorithm.Adjust;
using Gnsser.Domain;
using Gnsser.Data.Rinex;
using Gnsser.Times;
using Gnsser.Service;
using Gnsser.Checkers;
using Geo.Algorithm;
using Geo;

namespace Gnsser
{
    /// <summary>
    /// 负责责维护对齐模糊度
    /// </summary>
    public class SatAmbiguityManager : BaseDictionary<string, long>
    {
        /// <summary>
        /// 构造函数
        /// </summary> 
        public SatAmbiguityManager()
        {
             
        } 

        #region 设置初始模糊度
        /// <summary>
        /// 设置初始模糊度
        /// </summary>
        /// <param name="satelliteType"></param>
        /// <param name="gpsTime"></param>
        /// <param name="cycle"></param>
        public void SetInitCyle(string siteName, SatelliteNumber prn, long cycle) {
            var key = GetKey(siteName, prn);
            this[key] = cycle;
        } 
        /// <summary>
        /// 设置初始模糊度
        /// </summary>
        /// <param name="sat"></param>
        /// <param name="cycle"></param>
        public void SetInitCyle(EpochSatellite sat, long cycle)
        { 
            SetInitCyle(sat.EpochInfo.SiteName, sat.Prn, cycle);
        }
        #endregion
        
        /// <summary>
        /// 直接返回，不做检查。
        /// </summary>
        /// <param name="satelliteType"></param>
        /// <returns></returns>
        public long GetCycle(EpochSatellite sat)
        {
            SatelliteNumber prn = sat.Prn;
            var key = GetKey(sat.EpochInfo.SiteName, sat.Prn);
            return this[key]; 
        }


        public string GetKey(string siteName, SatelliteNumber prn)
        {
            return siteName + "-" + prn;
        }
    }
  
}
