// 2014.09.10, czs, create, 历元周跳处理器。

using System;
using System.Collections.Generic;
using System.Text;
using Gnsser.Domain;
using Gnsser.Service;
using Gnsser.Checkers;
using Gnsser.Data.Rinex;
using Geo.Utils;
using Geo.Algorithm.Adjust;
using Geo.Coordinates;
using Geo.Common;
using Geo.Algorithm;

namespace Gnsser.Domain
{
    /// <summary>
    /// 无电离层载波相位组合对齐器。
    /// </summary>
    public class AliningIonoFreePhaseProcessor : EpochInfoReviser
    {
        /// <summary>
        /// 无电离层载波相位组合对齐器。构造函数
        /// </summary> 
        public AliningIonoFreePhaseProcessor()
        {
            this.Name = "无电离层相位对齐处理器";
            this.SatIonoFreeAmbiguityManager = new SatAmbiguityManager();
        }

        /// <summary>
        /// 无电离层的对齐模糊度，伪模糊度
        /// </summary>
        SatAmbiguityManager SatIonoFreeAmbiguityManager { get; set; }

        public override bool Revise(ref EpochInformation epochInfo)
        {
            foreach (var sat in epochInfo)
            {
                //设置无电离层载波对齐
                SetAmbiguityOfIonoFreePhase(sat);
            }
            return true;
        }

        private void SetAmbiguityOfIonoFreePhase(EpochSatellite sat)
        {   
            long cycle = 0;
            if (sat.IsUnstable ) //|| csflag
            {
                sat.IsUnstable = true;

                //不用采用改正数。两者相减去可抵消。 
                double diffDistance = sat.Combinations.IonoFreeRange.CorrectedValue
                                    - sat.Combinations.IonoFreePhaseRange.CorrectedValue;

                cycle = sat.Combinations.IonoFreePhaseRange.Frequence.GetCycle(diffDistance);

                SatIonoFreeAmbiguityManager.SetInitCyle(sat, cycle);
            }
            else//直接赋值
            {
                cycle = SatIonoFreeAmbiguityManager.GetCycle(sat);
            }
            sat.AmbiguityOfIonoFreePhase = cycle;
             
        }
    }
}