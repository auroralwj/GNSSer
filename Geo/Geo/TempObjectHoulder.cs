
//2018.04.27, czs, create in hmx, 增加失败网址存储器，在指定时间内不再重复下载失败的链接，避免反复下载浪费时间
 
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Geo
{
    /// <summary>
    /// 用于短暂（指定的时间内）管理对象周期
    /// </summary>
    public class TempObjectHoulder<TKey> where TKey : IComparable<TKey>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="timeSpan"></param>
        public TempObjectHoulder(TimeSpan timeSpan)
        {
            this.TimeSpan = timeSpan;
            data = new BaseDictionary<TKey, DateTime>();
        }

        /// <summary>
        /// 保留时段
        /// </summary>
        public TimeSpan TimeSpan { get; set; }

        BaseDictionary<TKey, DateTime> data;

        /// <summary>
        /// 是否超出了指定的限制时间。
        /// 如果超出了，则自动解禁。必须访问本方法进行判断和解禁。
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool IsAvailable(TKey obj)
        {
            if (data.Contains(obj))
            { 
                if (GetRemainTime(obj) > TimeSpan.Zero)
                {
                    return false;
                }

                data.Remove(obj);
            }
            return true;
        }

        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="obj"></param>
        public void Regist(TKey obj)
        {
            data[obj] = DateTime.Now; 
        }

        /// <summary>
        /// 获取剩余解禁时间
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public TimeSpan GetRemainTime(TKey obj)
        {
            var passedTime = DateTime.Now - data[obj];
            return TimeSpan - passedTime;
        }
    }
}