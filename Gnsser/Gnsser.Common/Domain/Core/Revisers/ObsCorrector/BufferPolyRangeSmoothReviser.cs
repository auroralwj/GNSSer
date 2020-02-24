//2018.05.20, czs,  create in HMX, 缓存多项式伪距拟合平滑

using System;
using System.Collections.Generic;
using System.Text;
using Geo.Coordinates;
using Gnsser.Data.Rinex;
using Geo.Algorithm;
using Geo.Algorithm.Adjust;
using Gnsser.Service;
using Geo.Utils;
using Gnsser.Data;
using Gnsser.Domain;
using Geo;
using Gnsser;

namespace Gnsser.Correction
{
    /// <summary>
    /// 缓存多项式伪距拟合平滑
    /// </summary>
    public class BufferPolyRangeSmoothReviser : EpochInfoReviser
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="processOption"></param>
        public BufferPolyRangeSmoothReviser(GnssProcessOption processOption)
        {
            this.ProcessOption = processOption;
            WindowSize = ProcessOption.WindowSizeOfPhaseSmoothRange;
            this.Name = "缓存多项式伪距拟合平滑"; 
            TimedSmoothValueBuilderManager = new TimedSmoothValueBuilderManager(10);
            //窗口大小为原始缓存大小，再加上一定的过去数据
            LastWindowDataManager = new TimeNumeralWindowDataManager<string>(WindowSize / 2, 300);
            log.Info("启用：" + this.Name + " 对观测值进行改正！");
        }
        /// <summary>
        ///窗口
        /// </summary>
        public int WindowSize { get; set; }
        /// <summary>
        /// 一半窗口大小
        /// </summary>
        public int HalfWindowSize { get { return this.WindowSize / 2; } }
        GnssProcessOption ProcessOption { get; set; }
        TimedSmoothValueBuilderManager TimedSmoothValueBuilderManager { get; set; }
         /// <summary>
         /// 过去的数据
         /// </summary>
        TimeNumeralWindowDataManager<string> LastWindowDataManager { get; set; }

        /// <summary>
        /// 矫正
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Revise(ref EpochInformation obj)
        { 
            var buffer = this.Buffers;
            //添加到往期数据
            foreach (var sat in obj)
            {
                LastWindowDataManager
                     .GetOrCreate(BuildKey(sat.Prn, FrequenceType.A))
                     .Add(obj.ReceiverTime, sat[FrequenceType.A].PseudoRange.Value);

                if (sat.Contains(FrequenceType.B))
                {
                    LastWindowDataManager
                         .GetOrCreate(BuildKey(sat.Prn, FrequenceType.B))
                         .Add(obj.ReceiverTime, sat[FrequenceType.B].PseudoRange.Value);
                }
            }
            //逐个拟合
            //添加到往期数据
            foreach (var sat in obj)
            {
                var prn = sat.Prn;
                SetPolyFitCorrection(sat, FrequenceType.A);
                if (sat.Contains(FrequenceType.B))
                {
                    SetPolyFitCorrection(sat, FrequenceType.B);
                }
            }
            return true;
        }

        /// <summary>
        /// 设置改正的误差
        /// </summary>
        /// <param name="sat"></param>
        /// <param name="frequenceType"></param>
        private void SetPolyFitCorrection(EpochSatellite sat , FrequenceType frequenceType)
        {
            var prn = sat.Prn;
            var sp = GetBufferValues(prn, frequenceType).GetPolyFitValue(sat.ReceiverTime, 2);
            if (sp.Rms > 10)
            {
                log.Warn("多项式平滑伪距效果差！ RMS: " + sp.Rms);
                return ;
            }
            var correctionOfP = sp.Value - sat[frequenceType].PseudoRange.Value;
            sat[frequenceType].PseudoRange.Corrections.Add("PolyFit", correctionOfP);
        }

        /// <summary>
        /// 获取窗口
        /// </summary>
        /// <param name="prn"></param>
        /// <param name="frequenceType"></param>
        /// <returns></returns>
        public TimeNumeralWindowData GetBufferValues(SatelliteNumber prn, FrequenceType frequenceType)
        {
            TimeNumeralWindowData window = new TimeNumeralWindowData(WindowSize);
            var buffer = this.Buffers;
            //首先从过去的
            var lastWindowOfP1 = LastWindowDataManager.GetOrCreate(BuildKey(prn, frequenceType));
            window.Add(lastWindowOfP1);

            //然后从bufffer里面提取另一半
            var bufferOfEpoches = buffer.GetSubList(0, HalfWindowSize);

            foreach (var epoch in bufferOfEpoches)
            {
                if (epoch.Contains(prn))
                {
                    var sat = epoch[prn];
                    var time = epoch.ReceiverTime;
                    var p = sat[frequenceType].PseudoRange.Value;//原始数据

                    window.Add(time, p);
                }
            }
            return window;
        }

        string BuildKey(SatelliteNumber prn, FrequenceType frequenceType)
        {
            return prn + "_" + frequenceType;
        }





    }
}
