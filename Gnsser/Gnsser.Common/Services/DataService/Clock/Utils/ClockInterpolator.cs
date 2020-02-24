//2015.04.16, czs, edit in namu, 移除未使用方法

using System;
using Gnsser.Times;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gnsser.Data.Rinex;
using Gnsser;
using Geo.Coordinates;
using Geo.Referencing;
using Geo.Algorithm;
using Geo.Times; 
namespace Gnsser
{



    /// <summary>
    /// 钟差插值器。
    /// 每次只能处理一个卫星，即PRN，否则容易引起混乱。
    /// </summary>
    public class SimpleClockInterpolator
    {
        #region 构造函数与初始化
        /// <summary>
        /// 以排好序的钟差列表初始化
        /// </summary>
        /// <param name="sortedRecords">排好序的钟差列表</param>
        /// <param name="order">拟合阶数</param>
        /// <param name="expand">推估时段（秒）</param>
        public SimpleClockInterpolator(List<SimpleClockBias> sortedRecords, int order = 2, double expand = 300)
        {
            this.sortedRecords = sortedRecords;
            this.Order = order;
            this.Expand = expand;

            this.minDataTime = sortedRecords[0].Time;
            this.maxDataTime = sortedRecords[sortedRecords.Count - 1].Time;

            this.MinAvailableTime = minDataTime - Expand;
            this.MaxAvailableTime = maxDataTime + Expand;

            Init();
        }
        private void Init()
        {
            int count = sortedRecords.Count;
            double[] x = new double[count];
            double[] yOffset = new double[count];
            double[] yDrift = new double[count];

            for (int i = 0; i < count; i++)
            {
                SimpleClockBias clk = sortedRecords[i];
            //    x[i] = (double)(clk.Time.DateTime - minDataTime.DateTime).TotalSeconds;//Y为秒。
                x[i] = (clk.Time - minDataTime);//Y为秒。
                yOffset[i] = clk.ClockBias; 
            }

            Order = Math.Min(Order, count);
            interpError = new LagrangeInterplation(x, yOffset, Order);
            interpDrift = new LagrangeInterplation(x, yDrift, Order);
        }

        #endregion

        #region 属性
        /// <summary>
        /// 最大输入数据时间。
        /// </summary>
        Time maxDataTime;
        /// <summary>
        /// 最小的输入数据时间。
        /// </summary>
        Time minDataTime;

        /// <summary>
        /// 支持的最大时间
        /// </summary>
        public Time MaxAvailableTime { get; private set; }
        /// <summary>
        /// 支持的最小时间
        /// </summary>
        public Time MinAvailableTime { get; private set; }
        /// <summary>
        /// 推估的秒
        /// </summary>
        public double Expand { get; private  set; }

        public int Order { get; private set; }

        public List<SimpleClockBias> sortedRecords;
        IGetY interpError, interpDrift;

        #endregion
        
        #region 方法
        /// <summary>
        /// 是否可以进行插值。
        /// </summary> 
        /// <param name="time"></param>
        /// <returns></returns>
        public bool IsAvailable(Time time)
        {
            if (sortedRecords == null) return false;
            if (sortedRecords.Count == 0) return false;
            if (time > this.MaxAvailableTime || time < this.MinAvailableTime)
                return false;
            return true;
        }

        /// <summary>
        /// 获取插值后的 AtomicClock。
        /// </summary>
        /// <param name="PRN"></param>
        /// <param name="gpsTime"></param>
        /// <returns></returns>
        public SimpleClockBias GetAtomicClock(Time gpsTime)
        { 
            if (gpsTime < MinAvailableTime || gpsTime > MaxAvailableTime)
            {
                throw new ArgumentException("历元在给定的时间段外，不可进行插值。");
            }
            double deltaX = (double)(gpsTime - minDataTime); 
            return new SimpleClockBias()
            { 
                ClockBias = interpError.GetY(deltaX),
                Time = gpsTime 
            };
        }


        /// <summary>
        /// 获取插值后的 AtomicClock。
        /// </summary>
        /// <param name="from">起始时间</param>
        /// <param name="to">终止时间</param>
        /// <param name="interval">间隔（秒）</param>
        /// <returns></returns>
        public List<SimpleClockBias> GetAtomicClocks(Time from, Time to, double interval)
        {
            List<SimpleClockBias> list = new List<SimpleClockBias>();
            for (Time i = from; i <= to; i = i + interval)
            {
                list.Add(GetAtomicClock(i));
            }
            return list;
        }

#endregion
    }








    /// <summary>
    /// 钟差插值器。
    /// 每次只能处理一个卫星，即PRN，否则容易引起混乱。
    /// </summary>
    public class ClockInterpolator
    {
        #region 构造函数与初始化
        /// <summary>
        /// 以排好序的钟差列表初始化
        /// </summary>
        /// <param name="sortedRecords">排好序的钟差列表</param>
        /// <param name="order">拟合阶数</param>
        /// <param name="expand">推估时段（秒）</param>
        public ClockInterpolator(List<AtomicClock> sortedRecords, int order = 2, double expand = 300)
        {
            this.sortedRecords = sortedRecords;
            this.Order = order;
            this.Expand = expand;

            this.minDataTime = sortedRecords[0].Time;
            this.maxDataTime = sortedRecords[sortedRecords.Count - 1].Time;

            this.MinAvailableTime = minDataTime - Expand;
            this.MaxAvailableTime = maxDataTime + Expand;

            Init();
        }
        private void Init()
        {
            int count = sortedRecords.Count;
            double[] x = new double[count];
            double[] yOffset = new double[count];
            double[] yDrift = new double[count];

            for (int i = 0; i < count; i++)
            {
                AtomicClock clk = sortedRecords[i];
            //    x[i] = (double)(clk.Time.DateTime - minDataTime.DateTime).TotalSeconds;//Y为秒。
                x[i] = (clk.Time - minDataTime);//Y为秒。
                yOffset[i] = clk.ClockBias;
                yDrift[i] = clk.ClockDrift;
            }

            Order = Math.Min(Order, count);
            interpError = new LagrangeInterplation(x, yOffset, Order);
            interpDrift = new LagrangeInterplation(x, yDrift, Order);
        }

        #endregion

        #region 属性
        /// <summary>
        /// 最大输入数据时间。
        /// </summary>
        Time maxDataTime;
        /// <summary>
        /// 最小的输入数据时间。
        /// </summary>
        Time minDataTime;

        /// <summary>
        /// 支持的最大时间
        /// </summary>
        public Time MaxAvailableTime { get; private set; }
        /// <summary>
        /// 支持的最小时间
        /// </summary>
        public Time MinAvailableTime { get; private set; }
        /// <summary>
        /// 推估的秒
        /// </summary>
        public double Expand { get; private  set; }

        public int Order { get; private set; }

        public List<AtomicClock> sortedRecords;
        IGetY interpError, interpDrift;

        #endregion
        
        #region 方法
        /// <summary>
        /// 是否可以进行插值。
        /// </summary> 
        /// <param name="time"></param>
        /// <returns></returns>
        public bool IsAvailable(Time time)
        {
            if (sortedRecords == null) return false;
            if (sortedRecords.Count == 0) return false;
            if (time > this.MaxAvailableTime || time < this.MinAvailableTime)
                return false;
            return true;
        }

        /// <summary>
        /// 获取插值后的 AtomicClock。
        /// </summary>
        /// <param name="PRN"></param>
        /// <param name="gpsTime"></param>
        /// <returns></returns>
        public AtomicClock GetAtomicClock(Time gpsTime)
        {
            String name = sortedRecords[0].Name;
            Data.Rinex.ClockType clkType = sortedRecords[0].ClockType;

            if (gpsTime < MinAvailableTime || gpsTime > MaxAvailableTime)
            {
                throw new ArgumentException("历元在给定的时间段外，不可进行插值。");
            }
            double deltaX = (double)(gpsTime - minDataTime);
            if (clkType == Data.Rinex.ClockType.Satellite)
            {
                return new AtomicClock()
                {
                    Name = name,
                    ClockType = clkType,
                    ClockDrift = interpDrift.GetY(deltaX),
                    ClockBias = interpError.GetY(deltaX),
                    Time = gpsTime,
                    Prn = sortedRecords[0].Prn
                };
            }
            return new AtomicClock()
            {
                Name = name,
                ClockType = clkType,
                ClockDrift = interpDrift.GetY(deltaX),
                ClockBias = interpError.GetY(deltaX),
                Time = gpsTime

            };
        }


        /// <summary>
        /// 获取插值后的 AtomicClock。
        /// </summary>
        /// <param name="from">起始时间</param>
        /// <param name="to">终止时间</param>
        /// <param name="interval">间隔（秒）</param>
        /// <returns></returns>
        public List<AtomicClock> GetAtomicClocks(Time from, Time to, double interval)
        {
            List<AtomicClock> list = new List<AtomicClock>();
            for (Time i = from; i <= to; i = i + interval)
            {
                list.Add(GetAtomicClock(i));
            }
            return list;
        }

#endregion
    }

}
