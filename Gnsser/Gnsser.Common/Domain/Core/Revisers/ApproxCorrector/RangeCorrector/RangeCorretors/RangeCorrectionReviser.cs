//2014.12.01, czs, edit in jinxinliaomao shuangliao，实现了 IEpochInfoProcessor接口
//2017.09.05, czs, edit in hongqing, 改正器类型

using System;
using System.Text;
using Geo.Coordinates;
using Gnsser.Data.Rinex;
using Geo.Algorithm;
using Geo.Algorithm.Adjust;
using Gnsser.Service;
using Gnsser.Domain;
using Geo.Utils;
using System.Collections.Generic;
using System.Collections;
using Geo.Correction;

namespace Gnsser.Correction
{
   

    /// <summary>
    /// 通用伪距组合，对所有的伪距都起作用。
    /// 卫星改正的责任链,是一组改正对象的组合。一般采用此类将各种改正进行组合。
    /// </summary>
    public class RangeCorrectionReviser : GnssCorrectorChain<double, EpochSatellite>, IRangeCorrector<EpochSatellite>, IRangeCorrector, IEpochInfoReviser
    {
       
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="CorrectorType">改正器类型</param>
        public RangeCorrectionReviser(CorrectChianType CorrectorType = CorrectChianType.Common)
        {
            this.CorrectChianType = CorrectorType; 
            this.CorrectionType = Gnsser.Correction.CorrectionType.RangeChain;
            this.Name = "距离改正集合"; 
        }

        /// <summary>
        /// 执行信息
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// 改正类型
        /// </summary>
        public CorrectionType CorrectionType { get; protected set; }  

        /// <summary>
        /// 改正器类型
        /// </summary>
        public CorrectChianType CorrectChianType { get; set; }
        /// <summary>
        /// 缓存
        /// </summary>
        public Geo.IWindowData<EpochInformation> Buffers { get; set; }

        /// <summary>
        /// 历元信息矫正
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public bool Revise(ref EpochInformation info)
        {
            foreach (var sat in info.EnabledSats)//分别对指定卫星进行改正
            {  
                this.Correct(sat);

                switch (this.CorrectChianType)
                {
                    case CorrectChianType.Common:
                        sat.CommonCorrection.SetCorrection(this.Corrections);
                        break;
                    case CorrectChianType.PhaseRangeOnly:
                        sat.PhaseOnlyCorrection.SetCorrection(this.Corrections);
                        break;
                    case CorrectChianType.RangeOnly:
                        sat.RangeOnlyCorrection.SetCorrection(this.Corrections);
                        break;
                    case CorrectChianType.Self://不需要任何处理
                        break;
                    default:
                        break;
                }                  
            }
            return true;
        }

        /// <summary>
        /// 遍历计算改正数
        /// </summary>
        /// <param name="sat"></param>
        public override void Correct(EpochSatellite sat)
        {
            this.Corrections = new Dictionary<string, double>();
            double val = 0;
            foreach (var item in Correctors)
            {
                item.Correct(sat);
                val += item.Correction;

                this.Corrections.Add(item.Name, item.Correction);
            }
            //可在此设断点查看各个改正情况。
           this.Correction = (val);
        }

    } 
}
