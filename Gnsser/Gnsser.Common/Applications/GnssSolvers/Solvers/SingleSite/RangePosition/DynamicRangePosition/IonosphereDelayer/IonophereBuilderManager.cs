//2017.2.5 kyc ：载波相位平滑伪距思想求电离层延迟
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Geo;
using Geo.IO;

namespace Gnsser
{
    /// <summary>
    /// 管理器
    /// </summary>
    public class IonophereBuilderManager : BaseDictionary<SatelliteNumber, IonophereBuilder>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public IonophereBuilderManager()
        {

        }
        public override IonophereBuilder Create(SatelliteNumber key)
        {
            return new IonophereBuilder(key + "");
        }
    }
    public class IonophereBuilder : AbstractBuilder<double>, Geo.Namable
    {
        Log log = new Log(typeof(IonophereBuilder));
        public IonophereBuilder(string name = "")
        {
            this.Name = name;
            IonosphereWindow = new IonosphereWindow(75);
        }

        public IonosphereWindow IonosphereWindow { get; set; }
        
        /// <summary>
        /// 当前电离层延迟,由伪距计算
        /// </summary>
        public double CurrentIono { get; set; }

        public string Name { get; set; }

        public double LastPhaseIono { get; set; }

        /// <summary>
        /// 当前相邻两历元电离层延迟变化量,由载波相位计算
        /// </summary>

        public double DeltaPhaseIono { get; set; }

        public double SmoothedIono { get; set; }
        /// <summary>
        /// 设置当前电离层延迟,利用伪距差计算
        /// </summary>
        /// <param name="DeltaRange">P2-P1</param>
        /// <returns></returns>
        public IonophereBuilder SetCurrentIono(double DeltaRange) { this.CurrentIono = DeltaRange/(1575.42*1575.42/(1227.6*1227.6)-1); return this; }
        /// <summary>
        /// 设置相邻两历元电离层延迟变化量,利用载波相位差计算
        /// </summary>
        /// <param name="CurrentDeltaPhase">phi1-phi2</param>
        /// <returns></returns>
        public IonophereBuilder SetCurrentPhaseIono(double CurrentDeltaPhase)
        {
            this.DeltaPhaseIono = CurrentDeltaPhase / (1575.42 * 1575.42 / (1227.6 * 1227.6) - 1) - LastPhaseIono;
            this.LastPhaseIono = CurrentDeltaPhase / (1575.42 * 1575.42 / (1227.6 * 1227.6) - 1);//update
            return this;
        }

         public override double Build()
        {

            if (IonosphereWindow.Count > 1)
            {
                var differ = Math.Abs(CurrentIono - SmoothedIono );
                if (differ > 10)
                {
                    log.Error("电离层延迟量有误,可能发生了周跳，直接采用伪距值计算,电离层平滑计算值清零！重新来过。CurrentIono : " + CurrentIono + ", SmoothedIono :" + SmoothedIono);
                    IonosphereWindow.Clear();
                    return CurrentIono;
                }
            }
            IonosphereWindow.Add(CurrentIono, DeltaPhaseIono);
            SmoothedIono = IonosphereWindow.CaculateIonoValue();

            return SmoothedIono;
        }
    }
}
