//2018.12.27, czs, create in ryd, 断续性时段服务

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Geo;
using Geo.Times;

namespace Geo.Times
{

    /// <summary>
    /// 断续性时段服务
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    public class SuccessiveTimePeriodService<TKey> : BaseDictionary<TKey, SuccessiveTimePeriod>
    {
        /// <summary>
        /// 默认构造函数
        /// </summary>
        /// <param name="MaxGapSecond"></param>
        public SuccessiveTimePeriodService(double MaxGapSecond)
        {
            LastAppeared = new Dictionary<TKey, Time>();
            this.MaxGapSecond = MaxGapSecond;
        }
        /// <summary>
        /// 上一次出现
        /// </summary>
        public Dictionary<TKey, Time> LastAppeared { get; set; }
        /// <summary>
        /// 允许的最大断裂跨度
        /// </summary>
        public double MaxGapSecond { get; set; }
        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public override SuccessiveTimePeriod Create(TKey key)
        {
            return new SuccessiveTimePeriod();
        }
        /// <summary>
        /// 获取时段
        /// </summary>
        /// <param name="key"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        public TimePeriod GetTimePeriod(TKey key, Time time)
        {
            var ps = this.Get(key);
            return ps.Get(time);
        }

        /// <summary>
        /// 获取最后的时段分区
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public TimePeriod GetLastTimePeriod(TKey key)
        {
            var ps = this.Get(key);
            if(ps == null) { return null; }
            return ps.Last;
        }

        /// <summary>
        /// 注册，并返回所在时段，注册必须早于获取
        /// </summary>
        /// <param name="key"></param>
        /// <param name="time"></param>
        /// <param name="isBreakTime">是否手动新建</param>
        public TimePeriod Regist(TKey key, Time time, bool isBreakTime =false)
        {
            var periods = this.GetOrCreate(key);
            TimePeriod curentPeriod;
            Time last = time;
            if (LastAppeared.ContainsKey(key)) { last = LastAppeared[key]; } //支持逆序

            bool isBreaked = Math.Abs(last - time) > MaxGapSecond;

            if (isBreaked || isBreakTime)//断裂则新建
            {
                curentPeriod = periods.GetOrCreate(time);
            }
            else//连续则扩展
            {
                curentPeriod = periods.GetOrCreate(last);
                curentPeriod.ExppandSelf(time);//扩展时段 
            }

            //存储更新
            LastAppeared[key] = time;

            return curentPeriod;
        }

    }

}