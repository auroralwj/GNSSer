//2014.05.22, Cui Yang, created
//2014.07.04, Cui Yang, 增加多映射通用集合类，添加了MultiMap引用
//2014.07.05, czs, edit, 进行了代码重构
//2014.09.24, czs, edit, 重命名为 SateInfoFile
//2018.10.07, czs, edit in hmx, 性能优化

using System;
using System.Collections.Generic;
using System.Linq;
using Gnsser.Times;
using System.Text;
using System.IO;
using Gnsser.Service;
using Geo.Coordinates;
using Gnsser.Data.Rinex;
using Geo.Algorithm;
using Geo.Utils;
using Gnsser;
using Geo.Times;
using Geo;

namespace Gnsser.Data
{ 
    /// <summary>
    /// 时间段的卫星状态文件。
    /// </summary>
    public class SatInfoFile : PeriodValueStorage<SatelliteNumber, SatInfo>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="SatInfos">卫星状态列表</param>
        public SatInfoFile(List<SatInfo> SatInfos)
        {
            foreach (var item in SatInfos)
            {
                this.GetOrCreate(item.Prn)[item.TimePeriod] = item;
            }

        }
        /// <summary>
        /// 获取第一个满足条件的对象
        /// </summary>
        /// <param name="prn"></param>
        /// <param name="epoch"></param>
        /// <returns></returns>
        public string GetBlock(SatelliteNumber prn, Time epoch)
        {
            var dic = GetOrCreate(prn);
            foreach (var item in dic.KeyValues)
            {
                if (item.Key.Contains(epoch)) return item.Value.Block;
            } 
             
           return ""; 
        }          
    }  
}
