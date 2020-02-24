//2017.08.17， czs, create,格网电离层模型改正。
//2018.05.17, czs, edit in hmx, 修改为直接添加到改正数
//2018.05.27, czs, edit in hmx, 支持球谐函数电离层服务
//2018.06.09, czs, edit in hmx, 修改为支持所有频率，名称用 ParamNames.Iono

using System;
using System.Text;
using Geo.Coordinates;
using Gnsser.Data.Rinex;
using Geo.Algorithm;
using Geo.Algorithm.Adjust;
using Gnsser.Service;
using Geo.Utils;
using Gnsser.Domain;
using Gnsser.Data;

namespace Gnsser.Correction
{    
    /// <summary>
    /// IGS 电离层模型改正,单频,直接改正到站星对象
    /// </summary>
    public class IonoGridModelCorrector : AbstractRangeCorrector
    {
        /// <summary>
        /// 构造函数 格网电离层模型改正
        /// </summary>
        public IonoGridModelCorrector(IIonoService IonoService, bool isPhase = false)
        {
            this.Name = "格网或球谐函数电离层模型距离改正";
            this.IsCorrectionOnPhase = isPhase;
            this.CorrectionType = CorrectionType.IonoGridModel;
            this.IonoService = IonoService;
            if (IonoService == null) { log.Error(this.Name + "服务源为空"); }
        } 
        /// <summary>
        /// 是否改正到相位上。
        /// </summary>
        public bool IsCorrectionOnPhase { get; set; }

        /// <summary>
        /// 对流层文件服务
        /// </summary>
        IIonoService IonoService { get; set; }

     
        public override void Correct(EpochSatellite sat)
        {
            if (IonoService == null) { return; }

            foreach (var kv in sat.Data)
            {
                double slopeDelayFreq = GetGridModelCorrection(sat, kv.Key, IonoService);
                kv.Value.AddRangeCorrection(ParamNames.Iono, slopeDelayFreq);
                kv.Value.AddPhaseCorrection(ParamNames.Iono, -1.0 * slopeDelayFreq);//码延迟，波超前
            }

            //double slopeDelayFreqA = GetGridModelCorrection(sat, FrequenceType.A, IonoService);
            //sat.FrequenceA.AddRangeCorrection( ParamNames.Iono, slopeDelayFreqA);
            //sat.FrequenceA.AddPhaseCorrection(ParamNames.Iono, -1.0 * slopeDelayFreqA);//码延迟，波超前
 

            //if (sat.Contains(FrequenceType.B))
            //{
            //    double slopeDelayFreqB = GetGridModelCorrection(sat, FrequenceType.B, IonoService);
            //    //1.0 * GetIonoDelayRange(tec.Value, sat.FrequenceB.Frequence.Value);//斜方向延迟
            //    sat.FrequenceB.AddRangeCorrection(ParamNames.Iono, slopeDelayFreqB);
            //    sat.FrequenceB.AddPhaseCorrection(ParamNames.Iono, -1.0 * slopeDelayFreqB);//码延迟，波超前
            //} 

            this.Correction = 0;  
        }

        /// <summary>
        /// 获取电离层格网模型延迟
        /// </summary>
        /// <param name="sat"></param>
        /// <param name="frequenceType"></param>
        /// <param name="IonoService"></param>
        /// <returns></returns>
        public static double GetGridModelCorrection(EpochSatellite sat, FrequenceType frequenceType, IIonoService IonoService)
        { 
            //计算穿刺点获取电离层浓度 
            var punctPoint = sat.GetIntersectionXyz(IonoService.HeightOfModel);
            var geocentricLonLat = Geo.Coordinates.CoordTransformer.XyzToSphere(punctPoint);  
            var tec = IonoService.GetSlope(sat.ReceiverTime, geocentricLonLat, sat.SphereElevation);            
            if (tec == null || tec.Value == 0) { return 0; }
            Frequence freq = sat[frequenceType].Frequence;
            double slopeDelayFreqA = 1.0 * GetIonoDelayRange(tec.Value, freq.Value);//斜方向延迟  
            return slopeDelayFreqA; 
        }

        /// <summary>
        /// 电离层对于伪距的延迟距离，若是载波在换符号。
        /// </summary>
        /// <param name="tec">电子数量，单位 1e16.</param>
        /// <param name="freq">频率，单位 10^6</param>
        /// <returns></returns>
        public static double GetIonoDelayRange(double tec, double freq)
        { 
            return  tec * 40.28 / (freq * freq) * 1e4;//斜方向延迟             
        }    
    }
}
