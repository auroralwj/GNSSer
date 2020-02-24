//2017.07.21, czs, create in hongiqng, 地理格网遍历器
//2017.10.09, czs, edit in hongqing, 以纬度为第一遍历参数

using System;
using System.Collections.Generic;
using System.Text; 
using Geo.Coordinates; 
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using Geo.Algorithm.Adjust;
using Geo;
using Geo.IO;
using Geo.Algorithm;

using Geo.Data;
using Geo.Times;
using Geo.Utils;

namespace Geo
{ 
    /// <summary>
    /// 地理格网遍历器。二维便利器。
    /// 此处经纬度只为方便，可以用XY替代。
    /// </summary>
    public class GeoGridLooper : AbstractProcess, ICancelAbale
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="LonSpan"></param>
        /// <param name="LonStep">秒</param>
        /// <param name="LatSpan"></param>
        /// <param name="LatStep">秒</param>
        /// <param name="IsLonDesc"></param>
        /// <param name="IsLatDesc"></param>
        /// <param name="isLatFirst"></param>
        public GeoGridLooper(NumerialSegment LonSpan, double LonStep, NumerialSegment LatSpan, double LatStep, bool IsLonDesc = false, bool IsLatDesc =true, bool isLatFirst =true)
        {
            this.LatSpan = LatSpan;
            this.LatStep = LatStep;
            this.LonSpan = LonSpan;
            this.LonStep = LonStep;
            this.IsLatFirst = isLatFirst;
            this.IsLatDesc = IsLatDesc;
            this.IsLonDesc = IsLonDesc; 
        }
        
        #region 属性，事件
        /// <summary>
        /// 并行计算配置，可选。
        /// </summary>
        public IParallelConfig ParallelableParam { get; set; }

        /// <summary>
        /// 事件
        /// </summary>
        public event Action<LonLat> Looping; 
        /// <summary>
        /// 数据存储
        /// </summary>
        public Object Tag { get; set; }
        /// <summary>
        /// 是否纬度优先
        /// </summary>
        public bool IsLatFirst { get; set; }
        /// <summary>
        /// 经度范围
        /// </summary>
        public NumerialSegment LonSpan { get; set; }
        /// <summary>
        /// 经度步长
        /// </summary>
        public double LonStep { get; set; }
        /// <summary>
        /// 纬度范围
        /// </summary>
        public NumerialSegment LatSpan { get; set; }
        /// <summary>
        /// 纬度步长
        /// </summary>
        public double LatStep { get; set; }
        /// <summary>
        /// 纬度是否逆序
        /// </summary>
        public bool IsLatDesc { get; set; }
        /// <summary>
        /// 经度是否逆序
        /// </summary>
        public bool IsLonDesc { get; set; }
        /// <summary>
        /// 是否取消
        /// </summary>
        public bool IsCancel { get; set; } 
        /// <summary>
        /// 总的次数，计算得出，与实际可能有偏差。
        /// </summary>
        public int TotalCount { get; set; }
        /// <summary>
        /// 纬度数量
        /// </summary>
        public int LatCount { get; set; }
        /// <summary>
        /// 经度数量
        /// </summary>
        public int LonCount { get; set; }
        /// <summary>
        /// 当前正在执行的数量
        /// </summary>
        public int CurrentCount { get; set; }



        Looper Outer = null;
        Looper Inner = null;

        /// <summary>
        /// 坐标
        /// </summary>
        public List<LonLat> TotalCoords { get; set; }
        #endregion
        /// <summary>
        /// 初始化一些参数
        /// </summary>
        public override void Init()
        {
            base.Init();

            LatCount = (int)(LatSpan.Span / LatStep);
            LonCount = (int)(LonSpan.Span / LonStep);
            TotalCount = LonCount * LatCount;
            CurrentCount = 0; 

            Looper lonLooper = GetLonLooper();
            Looper latLooper = GetLatLooper();

            if (IsLatFirst)
            {
                Outer = latLooper;
                Inner = lonLooper;
                InitProcess(LatCount);
            }
            else
            {
                Outer = lonLooper;
                Inner = latLooper;
                InitProcess(LonCount);
            }

            Outer.Looping += Outer_Looping;
            Inner.Looping += Inner_Looping;
            this.TotalCoords = new List<LonLat>();
            Outer.Run(); 
        }
        void Outer_Looping(double obj)
        {
            Inner.Run();
        }

        void Inner_Looping(double obj)
        {
            LonLat coord = null;
            if (IsLatFirst)
            {
                coord = new LonLat(obj, Outer.Current); 
            }
            else
            {
                coord = new LonLat(Outer.Current, obj); 
            }
             
            TotalCoords.Add(coord);         
        }


        /// <summary>
        /// 并行计算
        /// </summary>
        public override void Run()
        {
            if (ParallelableParam != null)
            {
                ParallelLoopResult result = Parallel.ForEach<LonLat>(TotalCoords,
                      ParallelableParam.ParallelOptions,
                      (lonlat, ParalleState)=>{
                          if (IsCancel) { ParalleState.Break(); }
                          OnLooping(lonlat);
                      });               
            }
            else//串行
            {
                foreach (var coord in TotalCoords)
                {
                    if (IsCancel) { break; }
                    OnLooping(coord);
                }
            }

            OnCompleted();
            FullProcess();
        }

        private Looper GetLatLooper()
        {
            Looper latLooper = null;
            if (IsLatDesc)  {  latLooper = new Looper(this.LatSpan.End, this.LatSpan.Start, -LatStep);  }
            else   {  latLooper = new Looper(this.LatSpan.Start, this.LatSpan.End, LatStep);   }
            return latLooper;
        }

        private Looper GetLonLooper()
        {
            Looper lonLooper = null;
            if (IsLonDesc)  {  lonLooper = new Looper(this.LonSpan.End, this.LonSpan.Start, -LonStep);  }
            else   {  lonLooper = new Looper(this.LonSpan.Start, this.LonSpan.End, LonStep);   }
            return lonLooper;
        }

        #region 老方法
        /// <summary>
        /// 接口
        /// </summary>
        public  void RunOld()
        {
            if (IsLatFirst)
            {
                InitProcess((int)Math.Round(LatSpan.Span / LatStep)); 
                
                if (IsLatDesc)
                {
                    if (IsLonDesc)
                    {
                        LatDescLonDescLoop();
                    }
                    else
                    {
                        LatDescLonAscLoop();
                    }
                }
                else
                {
                    if (IsLonDesc)
                    {
                        LatAscLonDescLoop();
                    }
                    else
                    {
                        LatAscLonAscLoop();
                    }
                } 
            }
            else
            {
                 InitProcess((int)Math.Round(LonSpan.Span / LonStep)); 
                
                if (IsLatDesc)
                {
                    if (IsLonDesc)
                    {
                        LonDescLatDescLoop();
                    }
                    else
                    {
                        LonAscLatDescLoop();
                    }
                }
                else
                {
                    if (IsLonDesc)
                    {
                        LonDescLatAscLoop();
                    }
                    else
                    {
                        LonAscLatAscLoop();
                    }
                } 
            }
            OnCompleted();// if (Completed != null) { Completed(); }
            FullProcess(); 
        }

        #region 实现细节
        #region  计划提取
        /// <summary>
        /// 经度顺序循环
        /// </summary>
        /// <param name="func">调用函数</param>
        private void LonAscLoop(Action<double> func)
        {
            for (double lon = LonSpan.Start; lon >= LonSpan.End; lon = lon + LonStep)
            {
                if (IsCancel) { break; }
                func(lon);
            }
        }
        /// <summary>
        /// 经度逆序循环
        /// </summary>
        /// <param name="func">调用函数</param>
        private void LonDescLoop(Action<double> func)
        {
            for (double lon = LonSpan.End; lon >= LonSpan.Start; lon = lon - LonStep)
            {
                if (IsCancel) { break; }
                func(lon);
            }
        }

        /// <summary>
        /// 纬度顺序循环
        /// </summary>
        /// <param name="func">调用函数</param>
        private void LatAscLoop(Action<double> func)
        {
            for (double lat = LatSpan.Start; lat <= LatSpan.End; lat = lat + LatStep)
            {
                if (IsCancel) { break; }
                func(lat);
            }
        }
        /// <summary>
        /// 纬度逆序循环
        /// </summary>
        /// <param name="func">调用函数</param>
        private void LatDescLoop(Action<double> func)
        {
            for (double lat = LatSpan.End; lat >= LatSpan.Start; lat = lat - LatStep)
            {
                if (IsCancel) { break; }
                func(lat);
            }
        }

        #endregion
         
        /// <summary>
        /// 经度先循环，经度顺序，纬度逆序
        /// </summary>
        private void LonAscLatDescLoop()
        {
            for (double lon = LonSpan.Start; lon <= LonSpan.End; lon = lon + LonStep)
            {
                if (IsCancel) { break; }
                InnerLatDescLoop(lon);
            }
        }
        /// <summary>
        /// 经度先循环，经度逆序，纬度逆序
        /// </summary>
        private void LonDescLatDescLoop()
        {
            for (double lon = LonSpan.End; lon >= LonSpan.Start; lon = lon - LonStep)
            {
                if (IsCancel) { break; }
                InnerLatDescLoop(lon);
            }
        }
        /// <summary>
        /// 经度先循环，经度顺序，纬度顺序
        /// </summary>
        private void LonAscLatAscLoop()
        {
            for (double lon = LonSpan.Start; lon <= LonSpan.End; lon = lon + LonStep)
            {
                if (IsCancel) { break; }
                InnerLatAscLoop(lon);
            }
        }
        /// <summary>
        /// 经度先循环，经度逆序，纬度顺序
        /// </summary>
        private void LonDescLatAscLoop()
        {
            for (double lon = LonSpan.Start; lon <= LonSpan.End; lon = lon + LonStep)
            {
                if (IsCancel) { break; }
                InnerLatDescLoop(lon);
            }
        }
        /// <summary>
        /// 内部计算纬度逆序
        /// </summary>
        /// <param name="lon"></param>
        private void InnerLatDescLoop(double lon)
        {
            for (double lat = LatSpan.End; lat >= LatSpan.Start; lat = lat - LatStep)
            {
                if (IsCancel) { break; }
                //高程是否需要地形数据？？？？？
                var geoCoord = new LonLat(lon, lat);
                OnLooping(geoCoord);
            }
        }
        /// <summary>
        /// 内部计算纬度顺序
        /// </summary>
        /// <param name="lon"></param>
        private void InnerLatAscLoop(double lon)
        {
            for (double lat = LatSpan.Start; lat <= LatSpan.End; lat = lat + LatStep)
            {
                if (IsCancel) { break; }
                //高程是否需要地形数据？？？？？
                var geoCoord = new LonLat(lon, lat);
                OnLooping(geoCoord);
            }
        }
        /// <summary>
        /// 纬度先先循环，纬度顺序，经度顺序
        /// </summary>
        private void LatAscLonAscLoop()
        {
            for (double lat = LatSpan.Start; lat <= LatSpan.End; lat = lat + LatStep)
            {
                if (IsCancel) { break; }
                InnerLonAscLoop(lat);
            }
        }
        /// <summary>
        /// 纬度先循环，纬度顺序，经度逆序
        /// </summary>
        private void LatAscLonDescLoop()
        {
            for (double lat = LatSpan.Start; lat <= LatSpan.End; lat = lat + LatStep)
            {
                if (IsCancel) { break; }
                InnerLonDescLoop(lat);
            }
        }
        /// <summary>
        /// 纬度先循环，纬度逆序经度顺序
        /// </summary>
        private void LatDescLonAscLoop()
        {
            for (double lat = LatSpan.End; lat >= LatSpan.Start; lat = lat - LatStep)
            {
                if (IsCancel) { break; }
                InnerLonAscLoop(lat);
            }
        }
        /// <summary>
        /// 内部经度顺序循环
        /// </summary>
        /// <param name="lat"></param>
        private void InnerLonAscLoop(double lat)
        {
            for (double lon = LonSpan.Start; lon <= LonSpan.End; lon = lon + LonStep)
            {
                if (IsCancel) { break; }
                //高程是否需要地形数据？？？？？
                var geoCoord = new LonLat(lon, lat);
                OnLooping(geoCoord);
            }
        }
        /// <summary>
        /// 纬度先循环，纬度逆序，经度逆序
        /// </summary>
        private void LatDescLonDescLoop()
        {
            for (double lat = LatSpan.End; lat >= LatSpan.Start; lat = lat - LatStep)
            {
                if (IsCancel) { break; }
                InnerLonDescLoop(lat);
            }
        }
        /// <summary>
        /// 内部经度逆序循环
        /// </summary>
        /// <param name="lat"></param>
        private void InnerLonDescLoop(double lat)
        {
            for (double lon = LonSpan.End; lon >= LonSpan.Start; lon = lon - LonStep)
            {
                if (IsCancel) { break; }
                //高程是否需要地形数据？？？？？
                var geoCoord = new LonLat(lon, lat);
                OnLooping(geoCoord);
            }
        }
        #endregion
        #endregion

        /// <summary>
        /// 循环
        /// </summary>
        /// <param name="geoCoord"></param>
        protected void OnLooping(Geo.Coordinates.LonLat geoCoord)
        {
            if (IsCancel) { return; }

            Looping?.Invoke(geoCoord); 
            
            //进度条控制
            CurrentCount++;
             
            if (IsLatFirst && CurrentCount %  LatCount ==0)
            {
                PerformProcessStep();
            }
            else if (CurrentCount % LonCount == 0)
            {
                PerformProcessStep();
            } 
        } 

        /// <summary>
        /// 复制一个
        /// </summary>
        /// <returns></returns>
        public GeoGridLooper Clone()
        {
            return new GeoGridLooper(LonSpan, LonStep, LatSpan, LatStep, IsLonDesc, IsLatDesc, IsLatFirst);
        }
    }     
}
