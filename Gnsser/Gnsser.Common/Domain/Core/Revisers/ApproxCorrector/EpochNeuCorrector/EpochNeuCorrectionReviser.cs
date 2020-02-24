// 2014.12.01, czs, edit in jinxinliaomao shuangliao, 命名为 EpochNeuCorrectionProcessor ，实现了 IEpochInfoProcessor接口

using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using Geo.Algorithm;
using Gnsser.Domain;
using Gnsser.Service;
using Gnsser.Data.Rinex;
using Geo.Utils;
using Geo.Correction;
using Geo.Coordinates;
using Geo.Algorithm.Adjust;
namespace Gnsser.Correction
{

    /// <summary>
    /// 通用测站 NEU 改正，对所有的距离观测值都起作用。
    /// 卫星改正的责任链,是一组改正对象的组合。一般采用此类将各种改正进行组合。
    /// </summary>
    public class EpochNeuCorrectionReviser : GnssCorrectorChain<NEU, EpochInformation>, IEpochInfoReviser
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public EpochNeuCorrectionReviser()
        {
            this.CorrectionType = Gnsser.Correction.CorrectionType.NeuChain;
            this.Name = "接收机NEU改正集合"; 
        }
        /// <summary>
        /// 执行信息
        /// </summary>
        public string Message { get; set; }
        public CorrectionType CorrectionType { get; protected set; }
        
        public override void Correct(EpochInformation epochInfo)
        {
            this.Corrections = new Dictionary<string, NEU>();

            NEU val = new NEU();
            foreach (var item in Correctors)
            {
                item.Correct(epochInfo);

                if (item.Correction == null) continue;

                val += item.Correction;

                Corrections.Add(item.Name, item.Correction);
            }
            //可在此设断点查看各个改正情况。
           this.Correction = (val);
        }

        public bool Revise(ref EpochInformation info)
        { 
            //调用测站改正函数
            this.Correct(info);
            //赋值改正数到测站。 
            info.CorrectableNeu.SetCorrection(this.Corrections);

            foreach (var sat in info.EnabledSats)//对各个卫星分别计算
            {
                if (!sat.HasEphemeris) { continue; }

                foreach (var neuKv in this.Corrections)
                {
                    //这样考虑更加细致，容易查看改正结果
                    double rangeCorretion = CoordUtil.GetDirectionLength(neuKv.Value, info.SiteInfo.EstimatedXyz, sat.Ephemeris.XYZ);
                    //注意测站NEU坐标改正与伪距成发作用，为负号，如测站高到了卫星的高度，则伪距值则为 0 了。
                    sat.AddCommonCorrection(neuKv.Key, -1.0 * rangeCorretion);
                }
            }
            
            return true;
        }



        /// <summary>
        /// 缓存
        /// </summary>
        public Geo.IWindowData<EpochInformation> Buffers { get; set; }
    } 
}
