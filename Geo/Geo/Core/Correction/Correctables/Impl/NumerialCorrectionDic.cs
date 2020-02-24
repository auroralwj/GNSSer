//2014.10.26，czs, create in numu, 具体的改正数表

using System;
using System.Text;
using System.Collections.Generic;
using Geo.Utils;

namespace Geo.Correction
{    
    /// <summary>
    /// 改正数为双精度数字的改正数字典。
    /// </summary>
    public  class NumerialCorrectionDic : AbstractCorrectionDic<Double>
    {
        /// <summary>
        /// 默认构造函数。改正数为双精度数字的改正数字典。
        /// </summary>
        public NumerialCorrectionDic():base()
        {
            
        }

        /// <summary>
        /// 总的改正数。
        /// </summary>
        public override double TotalCorrection
        {
            get { double total = 0;
            foreach (var item in this.Corrections)
            {
                total+= item.Value;
            }
            return total;
            }
        }

    }
}
