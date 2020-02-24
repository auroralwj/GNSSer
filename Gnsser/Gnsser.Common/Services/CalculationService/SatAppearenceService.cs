//2018.12.27, czs, create in ryd, 断续性时段服务

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Geo;
using Geo.Times;  
using System.Linq; 
using System.Threading.Tasks;
using Gnsser.Domain;

namespace Gnsser.Service
{
    /// <summary>
    /// 测站卫星时段服务
    /// </summary>
    public class SiteSatAppearenceService : BaseDictionary<string, SatAppearenceService>
    {
        /// <summary>
        /// 测站卫星时段服务
        /// </summary>
        /// <param name="MaxGapSecond"></param>
        public SiteSatAppearenceService(double MaxGapSecond)
        {

            this.MaxGapSecond = MaxGapSecond;
        }
        /// <summary>
        /// 允许的最大断裂跨度
        /// </summary>
        public double MaxGapSecond { get; set; } 

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public override SatAppearenceService Create(string key)
        {
            return new SatAppearenceService(MaxGapSecond);
        }
        /// <summary>
        /// 设置断裂时间单位秒
        /// </summary>
        /// <param name="MaxGapSecond"></param>
        public void SetMaxGapSecond(double MaxGapSecond)
        {
            this.MaxGapSecond = MaxGapSecond;
            foreach (var item in this)
            {
                item.MaxGapSecond = MaxGapSecond;
            }
        }
    }


    /// <summary>
    /// 卫星断续性时段服务
    /// </summary>
    public class SatAppearenceService : SuccessiveTimePeriodService<SatelliteNumber>
    {
        public SatAppearenceService(double MaxGapSecond) : base(MaxGapSecond)
        {
        }
        /// <summary>
        /// 最后注册时间
        /// </summary>
        public Time LastRegistTime { get; set; }
        /// <summary>
        /// 注册时段
        /// </summary>
        /// <param name="obj"></param>
        public void Regist(EpochInformation obj)
        {
            foreach (var item in obj)
            {
                this.Regist(item.Prn, obj.ReceiverTime);
            }
            LastRegistTime = obj.ReceiverTime;
        }
    }
}
