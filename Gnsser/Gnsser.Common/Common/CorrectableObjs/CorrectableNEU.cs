//2014.10.25, czs, create in namu shuangliao, 具有改正数的坐标

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Geo.Times;
using Geo.Correction;
using Gnsser.Times;
using Geo.Coordinates;

namespace Gnsser.Correction
{
    /// <summary>
    /// 具有改正数的坐标
    /// </summary> 
    public class CorrectableNEU : AbstractDetailedCorrectable<NEU, NEU>, IDetailedCorrectable<NEU, NEU>
    {
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public CorrectableNEU()
            : base(NEU.Zero)
        {
        }
        /// <summary>
        /// 观测量构造函数。
        /// </summary>
        /// <param name="val">改正值</param> 
        public CorrectableNEU(NEU val)
            : base(val)
        {
        }

        /// <summary>
        /// 所有改正的集合
        /// </summary>
        public override NEU TotalCorrection
        {
            get
            {
                NEU neu = NEU.Zero;
                foreach (var item in this.Corrections)
                {
                    neu += item.Value;
                }
                return neu;
            }
        }

        /// <summary>
        /// 改正数
        /// </summary>
        public override NEU Correction { get { return TotalCorrection; } set { throw new NotSupportedException("不必要设置！请使用 AddCorrection 方法"); } }

        /// <summary>
        /// 改正后的值。
        /// </summary>
        public override NEU CorrectedValue { get { return Value + Correction; } } 
    }
}