//2017.2.5 kyc 尝试用载波相位平滑伪距思想递归计算电离层延迟
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Geo;
using Geo.IO;

namespace Gnsser
{
    public class IonosphereWindow : WindowData<IonosphereItem>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="aboutSize"></param>
        public IonosphereWindow(int size)
            : base(size)
        {

        }

        /// <summary>
        /// 增加一个
        /// </summary>
        /// <param name="Iono"></param>
        /// <param name="deltaIono"></param>

        public void Add(double rangeIono, double deltaIono)
        {
            this.Add(new IonosphereItem { CurrentIono = rangeIono, DeltaPhaseIono = deltaIono });
        }

        public double CaculateIonoValue()
        {
            double dest = 0;
            int windowSize = 0;
            foreach (var item in Data)
            {
                windowSize++;
                dest = item.CaculateIonoValue(dest, windowSize);
            }
            return dest;
        }
    }

    public class IonosphereItem
    {
        /// <summary>
        /// 当前电离层延迟,由伪距计算
        /// </summary>
        public double CurrentIono;
        /// <summary>
        /// 当前相邻两历元电离层延迟变化量,由载波相位计算
        /// </summary>
        public double DeltaPhaseIono;
        public double CaculateIonoValue(double prevSmootedIono, int windowSize)
        {
            var smoothedIono = 0.0;
            if (windowSize <= 1) { smoothedIono = CurrentIono; }
            else
            {
                smoothedIono = CurrentIono / windowSize + (1 - 1.0 / windowSize) * (prevSmootedIono + DeltaPhaseIono);
            }
            return smoothedIono;
        }
    }

}

